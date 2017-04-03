using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
namespace All.Data
{
    public class Sqlite:DataReadAndWrite
    {
        string[] allNeedFileName = new string[]{
            "System.Data.Sqlite.x86.dll","System.data.sqlite.x64.dll"};
        public Sqlite()
        {
            if (!CheckSystem)
            {
                CheckSystem = true;
                CheckDllNeedCopy();
            }
        }
        static bool CheckSystem = false;

        System.Data.SQLite.SQLiteConnection conn;
        public override System.Data.Common.DbConnection Conn
        {
            get { return conn; }
        }
        /// <summary>
        /// 因为Sqlite对不同的位数的操作系统使用不同的DLL,所以要判断是否使用不同的DLL
        /// </summary>
        private void CheckDllNeedCopy()
        {
            int i = All.Class.Environment.IsX86 ? 0 : 1;
            if (!System.IO.File.Exists(string.Format(".\\DllAndOcx\\Data\\Sqlite\\{0}", allNeedFileName[i])))
            {
                return;
            }
            if (System.IO.File.Exists(".\\System.Data.Sqlite.dll"))
            {
                if ((All.Class.FileIO.GetFileMD5(string.Format(".\\DllAndOcx\\Data\\Sqlite\\{0}", allNeedFileName[i]), "0") ==
                    All.Class.FileIO.GetFileMD5(".\\System.Data.Sqlite.dll", "1")))
                {
                    return;
                }
            }
            System.IO.File.Copy(string.Format(".\\DllAndOcx\\Data\\Sqlite\\{0}", allNeedFileName[i]),
                ".\\System.Data.Sqlite.dll", true);
        }
        public override bool Login(string Address, string Data, string UserName, string Password)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    conn = new SQLiteConnection(string.Format("Data Source={0}\\{1};Password={2}",
                            Address, Data, Password));
                    conn.Open();
                    result = (conn.State ==ConnectionState.Open);
                }
                catch (Exception e)
                {
                    if (conn != null)
                    {
                        All.Class.Error.Add("连接字符", string.Format("Data Source={0}\\{1};Password={2}",
                            Address, Data, Password));
                    }
                    All.Class.Error.Add(e);
                }
                return result;
            }
        }
        public override int BlockCommand(System.Data.DataTable dt)
        {
            lock (lockObject)
            {
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
                int result = 0;
                try
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(string.Format("select * from {0}", dt.TableName), conn))
                    {
                        using (SQLiteDataAdapter ada = new SQLiteDataAdapter(cmd))
                        {
                            using (SQLiteCommandBuilder scb = new SQLiteCommandBuilder(ada))
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
                        All.Class.Error.Add("Sqlite插入空数据");
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
                            allDatas = string.Format("{0},'{1:yyyy-MM-dd HH:mm:ss}'", allDatas, value[i]);
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
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
        public override System.Data.DataTable Read(string sql)
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
                    using (SQLiteDataAdapter oda = new SQLiteDataAdapter(sql, conn))
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
        public override System.Data.DataTable Read(string tableName, string[] columns, string[] conditions, string[] orders, bool Desc)
        {
            throw new NotImplementedException();
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
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
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
