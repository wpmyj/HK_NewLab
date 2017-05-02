using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace All.Class
{
    public static class Error
    {
        /// <summary>
        /// 已存在故障值的MD5
        /// </summary>
        static List<string> exitsErrors = new List<string>();
        static List<string> buff = new List<string>();
        static bool singleError = true;
        /// <summary>
        /// 是否开启单故障模式,单故障模式下,相同故障只保存一次
        /// </summary>
        public static bool SingleError
        {
            get { return Error.singleError; }
            set { Error.singleError = value; }
        }
        static string errorPath = "";
        /// <summary>
        /// 故障路径
        /// </summary>
        public static string ErrorPath
        {
            get
            {
                if (errorPath == "")
                {
                    errorPath = string.Format("{0}\\Error\\", FileIO.NowPath);
                    if (!System.IO.Directory.Exists(ErrorPath))
                    {
                        System.IO.Directory.CreateDirectory(errorPath);
                    }
                }
                return errorPath;
            }
        }
        static string errorFile = "";
        /// <summary>
        /// 故障文件
        /// </summary>
        public static string ErrorFile
        {
            get
            {
                if (errorFile == "")
                {
                    lock (buff)
                    {
                        errorFile = string.Format("{0}\\{1:yyyy-MM-dd}.Txt", ErrorPath, DateTime.Now);
                        if (!System.IO.File.Exists(errorFile))
                        {
                            System.IO.StreamWriter sw = System.IO.File.CreateText(errorFile);
                            sw.Close();
                            sw.Dispose();
                        }
                        DelMoreError(10);
                    }
                }
                return errorFile;
            }
        }
        /// <summary>
        /// 将消息写入Log
        /// </summary>
        /// <param name="e"></param>
        public static void Add(Exception e)
        {
            lock (buff)
            {
                Thread.CreateOrOpen("AllErrorThread", Flush);
                string value = "";
                if (e.Source != null)
                {
                    if (singleError)
                    {
                        if (exitsErrors.FindIndex(
                        errors =>
                        { return errors == e.Message; }) >= 0)
                        {
                            return;
                        }
                        exitsErrors.Add(e.Message);
                    }
                    string[] tmpBuff = e.StackTrace.Split('\n');
                    if (tmpBuff.Length > 2 && tmpBuff[2].IndexOf("位置") >= 0)
                    {
                        tmpBuff = tmpBuff[2].Split(new string[] { "位置" }, System.StringSplitOptions.RemoveEmptyEntries);
                    }
                    if (tmpBuff.Length > 0 && tmpBuff[0].IndexOf("在") >= 0)
                    {
                        value = string.Format("{0}出错模块  ->  {1}\r\n", value, tmpBuff[0].Substring(tmpBuff[0].IndexOf("在")));
                    }
                    if (tmpBuff.Length > 1 && tmpBuff[1].IndexOf("行号") >= 0)
                    {
                        value = string.Format("{0}出错位置  ->  {1}\r\n", value, tmpBuff[1].Substring(tmpBuff[1].IndexOf("行号")));
                    }
                    value = string.Format("{0}出错原因  ->  {1}", value, e.Message);
                    buff.Add(string.Format("出错时间  ->  {0:yyyy-MM-dd HH:mm:ss}\r\n{1}\r\n", DateTime.Now, value));
                }
                else
                {
                    if (singleError)
                    {
                        if (exitsErrors.FindIndex(
                        errors =>
                        { return errors == e.ToString(); }) >= 0)
                        {
                            return;
                        }
                        exitsErrors.Add(e.ToString());
                    }
                    value = string.Format("{0}出错原因  ->  {1}", value, e.ToString());
                    buff.Add(string.Format("出错时间  ->  {0:yyyy-MM-dd HH:mm:ss}\r\n{1}\r\n", DateTime.Now, value));
                }
            }
        }
        /// <summary>
        /// 将消息写入Log
        /// </summary>
        /// <param name="title">附加标题</param>
        /// <param name="value">附加信息</param>
        public static void Add(string title, string value)
        {
            lock (buff)
            {
                Thread.CreateOrOpen("AllErrorThread", Flush);
                if (singleError)
                {
                    if (exitsErrors.FindIndex(
                    errors =>
                    { return errors == value; }) >= 0)
                    {
                        return;
                    }
                    exitsErrors.Add(value);
                }
                buff.Add( string.Format("{0}  ->  {1}\r\n", title, value));
            }
        }
        /// <summary>
        /// 将消息写入Log
        /// </summary>
        /// <param name="e"></param>
        public static void Add(string message)
        {
            lock (buff)
            {
                Thread.CreateOrOpen("AllErrorThread", Flush);
                if (singleError)
                {
                    if (exitsErrors.FindIndex(
                    errors =>
                    { return errors == message; }) >= 0)
                    {
                        return;
                    }
                    exitsErrors.Add(message);
                }
                buff.Add(string.Format("出错时间  ->  {0:yyyy-MM-dd HH:mm:ss}\r\n出错原因  ->  {1}\r\n", DateTime.Now, message));
            }
        }
        /// <summary>
        /// 多线程写入消息
        /// </summary>
        private static void Flush()
        {
            string tmp = "";
            lock (buff)
            {
                if (buff.Count > 0)
                {
                    tmp = "";
                    buff.ForEach(str =>
                    {
                        tmp = string.Format("{0}{1}\r\n", tmp, str);
                    });
                    FileIO.Write(ErrorFile, tmp, System.IO.FileMode.Append);
                    buff.Clear();
                }
            }
        }
        /// <summary>
        /// 删除指定日期之前的文档
        /// </summary>
        /// <param name="time">指定日期</param>
        public static void DelMoreError(DateTime time)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(ErrorPath);
            List<string> fileName = new List<string>();
            foreach (System.IO.FileInfo fi in di.GetFiles())
            {
                if (fi.LastAccessTime < time)
                {
                    fileName.Add(fi.FullName);
                }
            }
            fileName.ForEach(
                file =>
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch
                    { }
                });
        }
        /// <summary>
        /// 删除指定天数之前的文档
        /// </summary>
        /// <param name="days"></param>
        public static void DelMoreError(int days)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(ErrorPath);
            List<string> fileName = new List<string>();
            foreach (System.IO.FileInfo fi in di.GetFiles())
            {
                if (fi.LastAccessTime.AddDays(days) < DateTime.Now)
                {
                    fileName.Add(fi.FullName);
                }
            }
            fileName.ForEach(
                file =>
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch
                    { }
                });
        }
    }
}
