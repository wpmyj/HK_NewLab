using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using All.Data;
namespace All.Class
{
    public static class FileIO
    {
        /// <summary>
        /// 获取当前程序工作目录
        /// </summary>
        public static string NowPath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath;
            }
        }
        static object lockObject = new object();
        /// <summary>
        /// 检测文件夹的路径是否存在，不存在则新建
        /// </summary>
        /// <param name="directory"></param>
        public static void CheckDirectory(string directory)
        {
            string[] dir = directory.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (dir.Length == 1)
            {
                return;
            }
            else
            {
                string tmpDir = "";
                for (int i = 0; i < dir.Length - 1; i++)
                {
                    tmpDir = string.Format("{0}{1}\\", tmpDir, dir[i]);
                }
                CheckDirectory(tmpDir);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }
        /// <summary>
        /// 检测文件的路径是否存在，不存在则新建
        /// </summary>
        /// <param name="file"></param>
        public static void CheckFileDirectory(string file)
        {
            CheckDirectory(file.Substring(0, file.LastIndexOf('\\')));
        }
        /// <summary>
        /// 读取文本字节
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] Read(string fileName)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate))
            {
                byte[] buff = new byte[fs.Length];
                fs.Read(buff, 0, buff.Length);
                fs.Flush();
                return buff;
            }
        }
        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFile(string fileName, Encoding encod)
        {
            byte[] buff = Read(fileName);
            if (buff != null && buff.Length > 3//这个是UTF8的头文件
                && buff[0] == 0xEF
                && buff[1] == 0xBB
                && buff[2] == 0xBF)
            {
                return Encoding.UTF8.GetString(buff, 3, buff.Length - 3);
            }
            return encod.GetString(buff);
        }
        /// <summary>
        /// 读取指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFile(string fileName)
        {
            return ReadFile(fileName, Encoding.UTF8);
        }
        /// <summary>
        /// 将数据写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buff"></param>
        /// <param name="fm"></param>
        public static void Write(string fileName, byte[] buff, FileMode fileMode)
        {
            lock (lockObject)
            {
                using (FileStream fs = new FileStream(fileName, fileMode))
                {
                    fs.Write(buff, 0, buff.Length);
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 将数据写入文件 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buff"></param>
        public static void Write(string fileName, byte[] buff)
        {
            Write(fileName, buff, FileMode.Create);
        }
        /// <summary>
        /// 将数据写入指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void Write(string fileName, string value, FileMode fm)
        {
            Write(fileName, Encoding.UTF8.GetBytes(value), fm);
        }
        /// <summary>
        /// 将数据写入指定文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void Write(string fileName, string value)
        {
            Write(fileName, value, FileMode.Create);
        }
        /// <summary>
        /// 将指定文字写入到文件,并换行
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void WriteLine(string fileName, string value)
        {
            Write(fileName, string.Format("{0}\r\n", value));
        }
        /// <summary>
        /// 将指定文字写入到文件,并换行,且添加日期
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        public static void WriteDate(string fileName, string value)
        {
            Write(fileName, string.Format("{0:yyyy-MM-dd HH:mm:ss}->{1}\r\n", DateTime.Now, value));
        }
        /// <summary>
        /// 获取指定文件的版本,Exe,Dll等执行文件取版本号,Txt,Mdb等数据文件取Hash值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileCode(string fileName, string NullValue)
        {
            string result = NullValue;
            if (System.IO.File.Exists(fileName))
            {
                lock (lockObject)
                {
                    System.Diagnostics.FileVersionInfo fi = System.Diagnostics.FileVersionInfo.GetVersionInfo(fileName);
                    result = fi.FileVersion;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取文件Hash值,如果文件正在使用中,则会报错
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="NullValue"></param>
        /// <returns></returns>
        public static string GetFileHash(string fileName, string NullValue)
        {
            string result = NullValue;
            if (System.IO.File.Exists(fileName))
            {
                lock (lockObject)
                {
                    try
                    {
                        using (System.Security.Cryptography.HashAlgorithm ah = System.Security.Cryptography.HashAlgorithm.Create())
                        {
                            using (FileStream fs = new FileStream(fileName, FileMode.Open))
                            {
                                result = All.Class.Num.Hex2Str(ah.ComputeHash(fs));
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        All.Class.Error.Add(e);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取文件的MD5值.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="NullValue"></param>
        /// <returns></returns>
        public static string GetFileMD5(string fileName, string NullValue)
        {
            string result = NullValue;
            if (System.IO.File.Exists(fileName))
            {
                lock (lockObject)
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Open))
                    {
                        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                        result = All.Class.Num.Hex2Str(md5.ComputeHash(fs));
                    }
                }
            }
            return result;
        }
    }
}
