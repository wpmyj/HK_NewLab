using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management;

namespace All.Class
{
    public static class HardInfo
    {
        public enum HardList
        {
            CPU序列号,
            主板序列号,
            硬盘序列号,
            BIOS,
            内存,
            IP地址
        }
        /// <summary>
        /// 判断当前环境是否为自己的电脑
        /// </summary>
        /// <returns></returns>
        public static bool MyTestComputer()
        {
            return ("BSN12345678901234567" == GetHardInfo(HardList.主板序列号) || "C02614401KZGDQP17" == GetHardInfo(HardList.主板序列号)) &&
                ("BFEBFBFF00040651" == GetHardInfo(HardList.CPU序列号) || "BFEBFBFF00040661" == GetHardInfo(HardList.CPU序列号));
        }
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static System.Net.IPAddress[] GetIpAddress()
        {
            return System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
        }
        /// <summary>
        /// 指定IP地址前段的IP
        /// </summary>
        /// <param name="PrevIp"></param>
        /// <returns></returns>
        public static string GetIpAddress(string PrevIp)
        {
            string result = "";
            System.Net.IPAddress[] tmpIp = GetIpAddress();
            for (int i = 0; i < tmpIp.Length; i++)
            {
                result = tmpIp[i].ToString();
                if (result.Split('.').Length == 4 && result.IndexOf(PrevIp) == 0)
                {
                    return result;
                }
            }
            return "";
        }
        /// <summary>
        /// 获取指定硬件的ID
        /// </summary>
        /// <param name="hardList"></param>
        /// <returns></returns>
        public static string GetHardInfo(HardList hardList)
        {
            string Key = "Win32_Processor";//读取硬件名
            string Pro = "ProcessorId";//硬件ID属性
            string value = "";
            switch (hardList)
            {
                case HardList.CPU序列号:
                    Key = "Win32_Processor";
                    Pro = "ProcessorId";
                    break;
                case HardList.硬盘序列号:
                    Key = "Win32_PhysicalMedia";
                    Pro = "SerialNumber";
                    break;
                case HardList.主板序列号:
                    Key = "Win32_BaseBoard";
                    Pro = "SerialNumber";
                    break;
                case HardList.BIOS:
                    Key = "Win32_BIOS";
                    Pro = "SerialNumber";
                    break;
                case HardList.内存:
                    Key = "Win32_PhysicalMemory";
                    Pro = "PartNumber";
                    break;
                case HardList.IP地址:
                    System.Net.IPAddress[] tmpIp = GetIpAddress();
                    for (int i = 0; i < tmpIp.Length; i++)
                    {
                        if (tmpIp[i].ToString().Split('.').Length == 4)
                        {
                            return tmpIp[i].ToString();
                        }
                    }
                    return "";
                default:
                    break;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format("select {0} from {1}", Pro, Key));
            ManagementObjectCollection tmpMo = searcher.Get();
            foreach (ManagementObject mo in tmpMo)
            {
                foreach (PropertyData pd in mo.Properties)
                {
                    if (pd.Name == Pro)
                    {
                        if (pd.Value != null)
                        {
                            value = pd.Value.ToString();
                            break;
                        }
                    }
                }
                if (value != "")
                {
                    break;
                }
            }
            return value;
        }
    }
}
