using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace All.Class
{
    /// <summary>
    /// 读写注册表
    /// </summary>
    public class Region
    {
        const string ApplicationName = "SSClass";
        /// <summary>
        /// 写入注册表
        /// </summary>
        /// <param name="KeyName">键名称</param>
        /// <param name="value">键值</param>
        public static void WriteValue(string KeyName, string value)
        {
            Microsoft.Win32.Registry.SetValue(string.Format("HKEY_CURRENT_USER\\Software\\{0}", ApplicationName), KeyName, value);
        }
        /// <summary>
        /// 读取注册表
        /// </summary>
        /// <param name="KeyName">键名称</param>
        /// <returns>键值</returns>
        public static string ReadValue(string KeyName)
        {
            try
            {
                return Microsoft.Win32.Registry.GetValue(string.Format("HKEY_CURRENT_USER\\Software\\{0}", ApplicationName), KeyName, "").ToString();
            }
            catch (NullReferenceException)
            {
                WriteValue(KeyName, "");
            }
            return "";
        }
    }
}
