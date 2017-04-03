using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections;
namespace All.Data
{
    /// <summary>
    /// 数据库读写
    /// </summary>
    [Serializable]
    public abstract class DataReadAndWrite:IDisposable
    {
        /// <summary>
        /// 连接文本示例
        /// </summary>
        public const string ExampleXmlSet=
"<?xml version=\"1.0\" encoding=\"utf-8\" ?>"+
"<Connection>"+
"	<Conn1 Class=\"All.Data.SqlServer\" Name=\"AllData\" Text=\"所有数据\">"+
"		<Address>127.0.0.1</Address>"+
"		<DataBase>FillData</DataBase>"+
"		<UserName>admin</UserName>"+
"		<Password></Password>"+
"	</Conn1>"+
"	<Conn2 Class=\"All.Data.Sqlite\" Name=\"LocalSet\" Text=\"本地设置数据\">"+
"		<Address>.\\Data\\</Address>"+
"		<DataBase>value.db</DataBase>"+
"		<UserName></UserName>"+
"		<Password></Password>"+
"	</Conn2>"+
"</Connection>";
        public string Text
        { get; set; }
        /// <summary>
        /// 锁
        /// </summary>
        internal object lockObject = new object();
        /// <summary>
        /// 数据库连接
        /// </summary>
        public abstract System.Data.Common.DbConnection Conn
        { get; }
        /// <summary>
        /// 将表批量更新到数据库,一定要有主键
        /// </summary>
        /// <param name="dt">要更新的表,dt.TableName不能为空</param>
        /// <returns>更新的行数</returns>
        public abstract int BlockCommand(DataTable dt);

        
        /// <summary>
        /// 登陆到指定数据库
        /// </summary>
        /// <param name="address">网络数据库填写IP地址,文件数据库填写路径</param>
        /// <param name="dataBase">网络数据库填写数据库名称,文件数据库填写文件名</param>
        /// <param name="userName">登陆用户名</param>
        /// <param name="password">登陆密码</param>
        /// <returns>返回是否登陆成功</returns>
        public bool Login(string address, string dataBase, string userName, string password)
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("Address", address);
            buff.Add("DataBase", dataBase);
            buff.Add("UserName", userName);
            buff.Add("Password", password);
            return Login(buff);
        }
        /// <summary>
        /// 登陆到指定数据库
        /// </summary>
        /// <param name="buff">必须包含Address,DataBase,UserName,Password四项</param>
        /// <returns></returns>
        public abstract bool Login(Dictionary<string,string> buff);
        /// <summary>
        /// 将数据写入到数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract int Write(string sql);
        /// <summary>
        /// 将符合条件的指定数据更新到指定列
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="value"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public abstract int Update(string tableName, string[] columns, ArrayList value, string[] conditions);
        /// <summary>
        /// 将指定数据更新到指定列
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Update(string tableName, string[] columns, ArrayList value)
        {
            return Update(tableName, columns, value, null);
        }
        /// <summary>
        /// 指定列插入指定数据
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract int Insert(string tableName, string[] columns, ArrayList value);
        /// <summary>
        /// 所有列插入指定数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Insert(string tableName, ArrayList value)
        {
            return Insert(tableName, null, value);
        }
        /// <summary>
        /// 从数据库读取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract DataTable Read(string sql);

        public void Dispose()
        {
            this.Close();
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            lock (lockObject)
            {
                try
                {
                    if (Conn != null)
                    {
                        if (Conn.State != ConnectionState.Closed)
                        {
                            Conn.Close();
                        }
                        Conn.Dispose();
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                }
            }
        }
        /// <summary>
        /// 登陆 
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            if (Conn == null)
            {
                return false;
            }
            bool result = false;
            try
            {
                Conn.Open();
                result = (Conn.State == ConnectionState.Open);
            }
            catch (Exception e)
            {
                if (Conn != null)
                {
                    Class.Error.Add("连接字符", Conn.ConnectionString);
                }
                Class.Error.Add(e);
            }
            return result;
        }
        /// <summary>
        /// 检查连接状态
        /// </summary>
        protected bool CheckConn()
        {
            return (Conn != null && Conn.State == ConnectionState.Open) || Login();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="columns">要读取的列名</param>
        /// <param name="tableName">表名</param>
        /// <param name="conditions">条件</param>
        /// <param name="orders">排序</param>
        /// <param name="Desc">是否须要逆排序</param>
        /// <returns></returns>
        public abstract DataTable Read(string tableName, string[] columns, string[] conditions, string[] orders, bool Desc);
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="columns">要读取的列名</param>
        /// <param name="tableName">表名</param>
        /// <param name="conditions">条件</param>
        /// <param name="orders">排序</param>
        /// <param name="Desc">是否须要逆排序</param>
        /// <returns></returns>
        public DataTable Read(string tableName, string[] columns, string[] conditions, string[] orders)
        {
            return Read(tableName, columns, conditions, orders, false);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="columns">要读取的列名</param>
        /// <param name="tableName">表名</param>
        /// <param name="conditions">条件</param>
        public DataTable Read(string tableName, string[] columns, string[] conditions)
        {
            return Read(tableName, columns, conditions, null);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="columns">要读取的列名</param>
        /// <param name="tableName">表名</param>
        public DataTable Read(string tableName, string[] columns)
        {
            return Read(tableName, columns, null);
        }
        /// <summary>
        /// 从文本文件中反射数据库连接 
        /// </summary>
        /// <param name="fileName">文本文件位置</param>
        /// <param name="text">数据描述文本</param>
        /// <returns></returns>
        public static DataReadAndWrite GetData(string fileName, string text)
        {
            DataReadAndWrite result = null;
            XmlNode tmpNode = All.Class.XmlHelp.GetXmlNode(fileName);
            if (tmpNode == null)
            {
                return result;
            }
            foreach (XmlNode tmpConn in tmpNode.ChildNodes)
            {
                if (tmpConn.NodeType != XmlNodeType.Element)
                {
                    continue;//注释等其他东西忽略
                }
                Dictionary<string, string> connAttribute = All.Class.XmlHelp.GetAttribute(tmpConn);
                if (connAttribute.ContainsKey("Name") && connAttribute["Name"].ToUpper() == text.ToUpper())
                {
                    try
                    {
                        Dictionary<string, string> connStr = All.Class.XmlHelp.GetInner(tmpConn);

                        All.Class.Reflex<DataReadAndWrite> r = new All.Class.Reflex<DataReadAndWrite>("All", connAttribute["Class"]);
                        result = (DataReadAndWrite)r.Get();
                        if (result != null && connAttribute.ContainsKey("Text"))
                        {
                            result.Text = connAttribute["Text"];

                            if (!result.Login(connStr))
                            {
                                All.Class.Error.Add(string.Format("{0}:数据库登陆失败,请检查数据库连接", result.Text), Environment.StackTrace);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        All.Class.Error.Add(e);
                    }
                    break;
                }
            }
            return result;
        }
        
    }
}
