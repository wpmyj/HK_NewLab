using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace All.Data
{
    public class SqlServer:DataReadAndWrite
    {
        SqlConnection conn;
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
                    using (SqlCommand cmd = new SqlCommand(string.Format("select * from {0}", dt.TableName), conn))
                    {
                        using (SqlDataAdapter ada = new SqlDataAdapter(cmd))
                        {
                            using (SqlCommandBuilder scb = new SqlCommandBuilder(ada))
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
        public override bool Login(Dictionary<string,string> buff)// string Address, string Data, string UserName, string Password)
        {
            string Address= buff["Address"];
            string Data = buff["DataBase"];
            string UserName = buff["UserName"];
            string Password = buff["Password"];
            bool result = false;
            try
            {
                conn = new SqlConnection(string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Pooling=true;Max Pool Size=10000;Min Pool Size=0",
                        Address, Data, UserName, Password));
                if (Address == "127.0.0.1")//如果是本机数据库,则登陆用户使用电脑本地账户验证
                {
                    conn = new SqlConnection(string.Format("Data Source=127.0.0.1;Initial Catalog={0};Integrated Security=True;Pooling=true;Max Pool Size=1000;Min Pool Size=0", Data));
                }
                conn.Open();
                result = (conn.State == ConnectionState.Open);
            }
            catch (Exception e)
            {
                All.Class.Error.Add("连接字符", string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Pooling=true;Max Pool Size=10000;Min Pool Size=0",
                        Address, Data, UserName, Password));
                All.Class.Error.Add(e);
            }
            return result;
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
                    using (SqlDataAdapter oda = new SqlDataAdapter(sql, conn))
                    {
                        oda.Fill(result);
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add("故障语句", sql);
                    All.Class.Error.Add(e);
                }
                return result.Copy();
            }
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
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
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
                        All.Class.Error.Add("SQL插入空数据");
                        continue;
                    }
                    switch (System.Type.GetTypeCode(value[i].GetType()))
                    {
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
                            allDatas = string.Format("{0},'{1:yyyy-MM-dd HH:mm:ss}'", allDatas, value[i]);
                            break;
                        case TypeCode.Boolean:
                            allDatas = string.Format("{0},{1}", allDatas, value[i].ToString().ToBool() ? 1 : 0);
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
            string sql = string.Format("insert into [{0}] {1} values {2}", tableName, allColumns, allDatas);
            lock (lockObject)
            {
                int result = 0;
                try
                {
                    if (!CheckConn())
                    {
                        return 0;
                    }
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
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
        }
        public override int Update(string tableName, string[] columns, System.Collections.ArrayList value, string[] conditions)
        {
            throw new NotImplementedException();
        }
        
    }
}
