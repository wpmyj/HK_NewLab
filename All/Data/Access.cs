using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
namespace All.Data
{
    public class Access:DataReadAndWrite
    {
        System.Data.OleDb.OleDbConnection conn;
        public override System.Data.Common.DbConnection Conn
        {
            get { return conn; }
        }
        public override int BlockCommand(DataTable dt)
        {
            lock (lockObject)
            {
                int result = 0;

                if (!CheckConn())
                {
                    return 0;
                }
                if (dt.TableName == "")
                {
                    All.Class.Error.Add("无法完成批量数据更新,数据表名不能为空");
                    All.Class.Error.Add(Environment.StackTrace);
                    return 0;
                }
                try
                {
                    using (OleDbCommand cmd = new OleDbCommand(string.Format("select * from {0}", dt.TableName), conn))
                    {
                        using (OleDbDataAdapter ada = new OleDbDataAdapter(cmd))
                        {
                            using (OleDbCommandBuilder scb = new OleDbCommandBuilder(ada))
                            {
                                ada.InsertCommand = scb.GetInsertCommand();
                                ada.DeleteCommand = scb.GetDeleteCommand();
                                ada.UpdateCommand = scb.GetUpdateCommand();
                                result = ada.Update(dt);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(string.Format("出错Table为：{0}", dt.TableName));
                    All.Class.Error.Add(e);//数据库中一定要有主键，不然当前方法会出错。即没有办法生成删除命令
                }
                return result;
            }
        }
        public bool Login(string FileName, string UserName, string Password)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    conn = new OleDbConnection(string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Database Password={1}",
                            FileName, Password));
                    conn.Open();
                    result = (conn.State == ConnectionState.Open);
                }
                catch (Exception e)
                {
                    if (conn != null)
                    {
                        All.Class.Error.Add("连接字符", conn.ConnectionString);
                    }
                    All.Class.Error.Add(e);
                }
                return result;
            }
        }
        public override bool Login(Dictionary<string,string> buff)//string Address, string Data, string UserName, string Password)
        {
            return Login(string.Format("{0}\\{1}", buff["Address"], buff["DataBase"]), buff["UserName"], buff["Password"]);
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
                }
                catch (Exception e)
                {
                    All.Class.Error.Add("故障语句", sql);
                    All.Class.Error.Add(e);
                }
                return result.Copy() ;
            }
        }
        public override int Insert(string tableName, string[] columns, System.Collections.ArrayList value)
        {
            //产生数据的时间基本可以忽略
            string allColumns = "";
            string allDatas = "";
            if (columns != null && columns.Length > 0)
            {
                columns.ToList().ForEach(col => allColumns = string.Format("{0}[{1}],", allColumns, col));
                allColumns = allColumns.Substring(0, allColumns.Length - 1);
                allColumns = string.Format(" ({0}) ", allColumns);
            }
            if (value != null && value.Count > 0)
            {
                for (int i = 0; i < value.Count; i++)
                {
                    if (value[i] == null)
                    {
                        All.Class.Error.Add("Access插入空数据");
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
    }
}
