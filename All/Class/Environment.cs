using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
namespace All.Class
{
    public static class Environment
    {
        /// <summary>
        ///  当前是否处于设计模式
        /// </summary>
        public static bool IsDesignMode
        {
            get
            {
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                {
                    return true;
                }
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// 系统是否是x86
        /// </summary>
        public static bool IsX86
        {
            get
            {
                ConnectionOptions oConn = new ConnectionOptions();
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\localhost", oConn);
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
                ManagementObjectCollection oReturnCollection = oSearcher.Get();
                string addressWidth = null;
                foreach (ManagementObject oReturn in oReturnCollection)
                {
                    addressWidth = oReturn["AddressWidth"].ToString();
                }
                return addressWidth != "64";
            }
        }
    }
}
