using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Data
{
    public class PostGreSQL:All.Data.DataReadAndWrite
    {
        public override System.Data.Common.DbConnection Conn
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        public override int BlockCommand(System.Data.DataTable dt)
        {
            throw new NotImplementedException();
        }
        public override int Insert(string tableName, string[] columns, System.Collections.ArrayList value)
        {
            throw new NotImplementedException();
        }
        public override bool Login(Dictionary<string, string> buff)
        {
            return true;
        }
        public override bool Login(string address, string dataBase, string userName, string password)
        {
            return base.Login(address, dataBase, userName, password);
        }
        public override System.Data.DataTable Read(string sql)
        {
            throw new NotImplementedException();
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
            return 10;
        }
    }
}
