using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
namespace All.Data
{
    public class Excel:DataReadAndWrite
    {
        OleDbConnection conn;
        public override System.Data.Common.DbConnection Conn
        {
            get { return conn; }
            set
            {
                if (conn is OleDbConnection)
                {
                    this.conn = conn as OleDbConnection;
                }
            }
        }
        public override int BlockCommand(DataTable dt)
        {
            throw new NotImplementedException();
        }
        public override bool Login(string FileName, string UserName, string Password, string Version)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    //连接字符串
                    //Provider:程序版本,Microsoft.ACE.OLEDB.12.0代表office2007版本,Microsoft.Jet.OLEDB.4.0代表office2003版本,貌似2007是兼容2003的

                    //Data Source:数据源,指定Excel文件路径.
                    //HDR:第一行是否有数据,为YES时,表示第一行为表头,没有数据.为NO时,表示第一行有数据.系统默认值为Yes
                    //Imex:操作模式,为0时代表导出模式,为1时代表导入模式,为2时代表完全模式,
                    //简单点,为0时,驱动根据数据表前8行来推算数据类型,
                    //       为1时,所有单元格都按文本数据类型来计算,其实也不是...Bug么?
                    //       为2时,完全写入模式
                    //Extended Properties:驱动版本Excel 8.0代表office2003驱动,Excel 12.0代表office2007驱动

                    //报ISAM错误,解决办法,用引号将Extended Properties的内容引用起来
                    //唯一的解决办法,将所有列设为文本数据类型,方法一,将注册表的前8行改为所有行,方法二,第一行设为标题文本行.HDR=No来判断
                    string connStr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;IMEX=2;HDR=YES\"",
                        FileName);
                    if (Version.IndexOf("97") >= 0 || Version.IndexOf("03") >= 0 || Version.ToUpper().IndexOf("LOW") >= 0)//是否使用97版EXCEL
                    {
                        connStr = string.Format("Provider=MicrosoftJet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=2;HDR=YES\"",
                        FileName);
                    }
                    conn = new OleDbConnection(connStr);
                    conn.Open();
                    result = (conn.State == ConnectionState.Open);
                }
                catch (Exception e)
                {
                    result = false;
                    if (conn != null)
                    {
                        All.Class.Error.Add("连接字符", conn.ConnectionString);
                    }
                    All.Class.Error.Add(e);
                }
                return result;
            }
        }
        public bool Login(string FileName,string UserName,string Password)
        {
            return Login(FileName, UserName, Password, "");
        }
        public override bool Login(Dictionary<string,string> buff)// string Address, string Data, string UserName, string Password)
        {
            if (buff.ContainsKey("Version"))
            {
                return Login(string.Format("{0}\\{1}", buff["Address"], buff["DataBase"]), buff["UserName"], buff["Password"], buff["Version"]);
            }
            return Login(string.Format("{0}\\{1}", buff["Address"], buff["DataBase"]), buff["UserName"], buff["Password"]);
        }
        private new bool CheckConn()
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            return Login();
        }
        public override DataTable Read(string sql)
        {
            lock (lockObject)
            {
                DataTable result = new DataTable();
                try
                {
                    if (!CheckConn())
                    {
                        return result;
                    }
                    using (OleDbDataAdapter oda = new OleDbDataAdapter(sql, conn))
                    {
                        oda.Fill(result);
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    All.Class.Error.Add("故障语句", sql);
                    All.Class.Error.Add(e);
                }
                return result.Copy();
            }
        }
        public override int Insert(string tableName, string[] columns, System.Collections.ArrayList value)
        {
            //产生数据的时间基本可以忽略
            string allColumns = "";
            string allDatas = "";
            if (columns != null && columns.Length > 0)
            {
                columns.ToList().ForEach(col => allColumns = string.Format("{0}{1},", allColumns, col));
                allColumns = allColumns.Substring(0, allColumns.Length - 1);
                allColumns = string.Format(" ({0}) ", allColumns);
            }
            if (value != null && value.Count > 0)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i] == null)
                    {
                        All.Class.Error.Add("Excel插入空数据");
                        continue;
                    }
                    switch (System.Type.GetTypeCode(value[i].GetType()))
                    {
                        case TypeCode.Boolean:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                        case TypeCode.Byte:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                        case TypeCode.Single:
                        case TypeCode.SByte:
                            allDatas = string.Format("{0},{1}", allDatas, value[i]);
                            break;
                        case TypeCode.DateTime:
                            allDatas = string.Format("{0},#{1:yyyy-MM-dd HH:mm:ss}#", allDatas, value[i]);
                            break;
                        case TypeCode.String:
                            allDatas = string.Format("{0},'{1}'", allDatas, value[i]);
                            break;
                    }
                }
                if (allDatas.StartsWith(","))
                {
                    allDatas = allDatas.Substring(1);
                }
                allDatas = string.Format(" ({0}) ", allDatas);
            }
            else
            {
                return 0;
            }
            string sql = string.Format("insert into {0} {1} values {2}", tableName, allColumns, allDatas);
            lock (lockObject)
            {
                int result = 0;
                try
                {
                    if (!CheckConn())
                    {
                        return 0;
                    }
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    All.Class.Error.Add("故障语句", sql);
                    All.Class.Error.Add(e);
                }
                return result;
            }
        }
        public override DataTable Read(string tableName, string[] columns, string[] conditions, string[] orders, bool Desc)
        {
            throw new NotImplementedException();
            //条件部分暂时没法解决,合一起不通用,分开写太SB
            //string allColumns = "*";
            //string allConditions = "1=1";
            //string allOrders = "";
            //string sql = "";
            //if (columns != null && columns.Length > 0)
            //{
            //    columns.ToList().ForEach(col => allColumns = string.Format("{0}{1},", allColumns, col));
            //    allColumns = allColumns.Substring(0, allColumns.Length - 1);
            //}

            //lock (lockObject)
            //{
            //    DataTable result = new DataTable();
            //    try
            //    {
            //        CheckConn();
            //        using (OleDbDataAdapter oda = new OleDbDataAdapter(sql, conn))
            //        {
            //            oda.Fill(result);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        All.Class.Error.Add("故障语句", sql);
            //        All.Class.Error.Add(e);
            //    }
            //    return result.Copy();
            //}
        }
        public override int Update(string tableName, string[] columns, System.Collections.ArrayList value, string[] conditions)
        {
            throw new NotImplementedException();
        }
        public override int Write(string sql)
        {
            lock (lockObject)
            {
                int result = 0;
                try
                {
                    if (!CheckConn())
                    {
                        return 0;
                    }
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                catch (Exception e)
                {
                    All.Class.Error.Add("故障语句", sql);
                    All.Class.Error.Add(e);
                    return -1;
                }
                return result;
            }
        }
        /// <summary>
        /// 获取所有数据表名称
        /// </summary>
        /// <returns></returns>
        public string[] GetAllSheets()
        {
            if (!CheckConn())
            {
                return null;
            }
            string[] result = null;
            try
            {
                using (DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }))
                {
                    //包含excel中表名的字符串数组
                    result = new string[dtSheetName.Rows.Count];
                    for (int k = 0; k < dtSheetName.Rows.Count; k++)
                    {
                        result[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
                return null;
            }
            return result;
        }
    }
}
