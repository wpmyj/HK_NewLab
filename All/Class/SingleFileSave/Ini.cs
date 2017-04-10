using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Class.SingleFileSave
{
    /// <summary>
    /// INI文件格式保存文本
    /// </summary>
    public static class Ini
    {
        /// <summary>
        /// 将指定数据写入指定ini文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="title"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Write(string fileName, string section, string key, string value)
        {
            All.Class.Api.WritePrivateProfileString(section, key, value, fileName);
        }
        /// <summary>
        /// 从文件读取指定数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Read(string fileName, string section, string key, string defaultValue)
        {
            StringBuilder result = new StringBuilder(255);
            try
            {
                All.Class.Api.GetPrivateProfileString(section, key, defaultValue, result, 255, fileName);
            }
            catch { }
            return result.ToString().Trim();
        }
        /// <summary>
        /// 将字典转化为标准字符串
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string Dictionary2Text(string section, Dictionary<string, string> buff)
        {
            StringBuilder result = new StringBuilder(500);
            result.Append(string.Format("[{0}]\r\n", section));
            buff.Keys.ToList().ForEach(key =>
            {
                result.Append(string.Format("{0}={1}\r\n", key, buff[key].Trim()));
            });
            return result.ToString().Trim();
        }
        public static Dictionary<string, string> Text2Dictionary(string section, string value)
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            string[] tmp = value.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (tmp != null && tmp.Length > 0)
            {
                for (int i = 0; i < tmp.Length - 1; i++)
                {
                    if (tmp[i].ToUpper() == section.ToUpper())
                    {
                        string[] tmpSection = tmp[i + 1].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmpSection != null && tmpSection.Length > 0)
                        {
                            string key, data;
                            for (int j = 0; j < tmp.Length; j++)
                            {
                                key = tmpSection[j].Substring(0, tmpSection[j].IndexOf('=')).Trim();
                                data = tmpSection[j].Substring(tmpSection[j].IndexOf('=') + 1).Trim();
                                if (!buff.ContainsKey(key))
                                {
                                    buff.Add(key, data);
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return buff;
        }
    }
}
