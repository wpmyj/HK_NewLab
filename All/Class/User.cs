using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace All.Class
{
    public class cUser
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public string CurrentUser
        { get; set; }
        /// <summary>
        /// 当前权限
        /// </summary>
        public string CurrentLevel
        { get; set; }
        public enum LevelList : int
        {
            管理员,
            操作员,
            其他
        }
        static string UserFilePath = Path.Combine(FileIO.NowPath, "User");
        /// <summary>
        /// 所有用记
        /// </summary>
        public List<UserSet> AllUser
        { get; set; }
        public List<string> UserName
        {
            get
            {
                List<string> result = new List<string>();
                if (AllUser != null)
                {
                    AllUser.ForEach(tmp => result.Add(tmp.UserName));
                }
                return result;
            }
        }
        public List<string> Password
        {
            get
            {
                List<string> result = new List<string>();
                if (AllUser != null)
                {
                    AllUser.ForEach(tmp => result.Add(tmp.PassWord));
                }
                return result;
            }
        }
        public List<string> Level
        {
            get
            {
                List<string> result = new List<string>();
                if (AllUser != null)
                {
                    AllUser.ForEach(tmp => result.Add(tmp.Level));
                }
                return result;
            }
        }
        public cUser()
        {
            AllUser = new List<UserSet>();
            if (!Directory.Exists(UserFilePath))
            {
                Directory.CreateDirectory(UserFilePath);
            }
        }
        public void Load()
        {
            DirectoryInfo di = new DirectoryInfo(UserFilePath);
            UserSet tmp;
            foreach (FileInfo fi in di.GetFiles())
            {
                tmp = UserSet.Read(fi.Name);
                if (AllUser.FindIndex(
                    user => user.UserName == tmp.UserName) < 0)
                {
                    AllUser.Add(tmp);
                }
            }
            if (AllUser.Count <= 0)
            {
                tmp = new UserSet();
                tmp.Save();
                AllUser.Add(tmp);
            }
        }
        public static void Delete(string username)
        {
            if (File.Exists(Path.Combine(UserFilePath, username)))
            {
                File.Delete(Path.Combine(UserFilePath, username));
            }
        }
        public void Save(string username, string password, string level)
        {
            Delete(username);
            UserSet tmp = new UserSet();
            tmp.UserName = username;
            tmp.PassWord = password;
            tmp.Level = level;
            tmp.Save();
        }
        public class UserSet
        {
            public string UserName
            { get; set; }
            public string PassWord
            { get; set; }
            public string Level
            { get; set; }
            public UserSet()
            {
                PassWord = "";
                UserName = "Administrator";
                Level = string.Format("{0}", (LevelList)0);
            }
            public static UserSet Read(string fileName)
            {
                UserSet result = new UserSet();
                UserSet tmp = (UserSet)All.Class.XmlHelp.ReadXml(System.IO.Path.Combine(UserFilePath, fileName), typeof(UserSet), new UserSet());
                result.UserName = Encoding.UTF8.GetString(All.Class.Num.Str2Hex(tmp.UserName));
                result.PassWord = Encoding.UTF8.GetString(All.Class.Num.Str2Hex(tmp.PassWord));
                result.Level = Encoding.UTF8.GetString(All.Class.Num.Str2Hex(tmp.Level));
                return result;
            }
            public void Save()
            {
                UserSet tmp = new UserSet();
                tmp.UserName = All.Class.Num.Hex2Str(Encoding.UTF8.GetBytes(UserName));
                tmp.PassWord =All.Class.Num.Hex2Str( Encoding.UTF8.GetBytes(PassWord));
                tmp.Level = All.Class.Num.Hex2Str(Encoding.UTF8.GetBytes(Level));
                All.Class.XmlHelp.SaveXml(System.IO.Path.Combine(UserFilePath, UserName), typeof(UserSet), tmp);
            }
        }
    }
}
