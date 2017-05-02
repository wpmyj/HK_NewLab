using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class
{
    public static class Log
    {
        static string logPath = "";
        /// <summary>
        /// log路径
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == "")
                {
                    logPath = string.Format("{0}\\Log\\", FileIO.NowPath);
                    if (!System.IO.Directory.Exists(LogPath))
                    {
                        System.IO.Directory.CreateDirectory(logPath);
                    }
                }
                return logPath;
            }
        }
        static string logFile = "";
        /// <summary>
        /// log文件
        /// </summary>
        public static string LogFile
        {
            get
            {
                if (logFile == "")
                {
                    logFile = string.Format("{0}\\{1:yyyy-MM-dd}.Txt", LogPath, DateTime.Now);
                    if (!System.IO.File.Exists(logFile))
                    {
                        System.IO.StreamWriter sw = System.IO.File.CreateText(logFile);
                        sw.Close();
                        sw.Dispose();
                    }
                    DelMoreError(10);
                }
                return logFile;
            }
        }
        static List<string> buff = new List<string>();
        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="value"></param>
        public static void Add(string value)
        {
            lock (buff)
            {
                Thread.CreateOrOpen("AllLogThread", Flush, 1000);
                buff.Add(value);
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
                            tmp = string.Format("{0}{1:yyyy-MM-dd HH:mm:ss} ->{2}\r\n", tmp, DateTime.Now, str);
                        });
                    FileIO.Write(LogFile, tmp, System.IO.FileMode.Append);
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
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(LogPath);
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
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(LogPath);
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
