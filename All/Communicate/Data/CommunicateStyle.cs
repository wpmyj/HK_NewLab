using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate.Data
{
    /// <summary>
    /// 通讯节点
    /// </summary>
    public class CommunicateStyle
    {
        /// <summary>
        /// 通讯值
        /// </summary>
        public Communicate Value
        { get; set; }
        /// <summary>
        /// 通讯类下的有设备
        /// </summary>
        public List<MeterStyle> Meters
        { get; set; }
        /// <summary>
        /// 通讯节点,即一个通讯父类
        /// </summary>
        public CommunicateStyle()
        {
            this.Value = null;
            this.Meters = new List<MeterStyle>();
        }
    }
}
