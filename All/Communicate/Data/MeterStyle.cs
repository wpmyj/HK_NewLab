using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate.Data
{
    /// <summary>
    /// 设备读取信息
    /// </summary>
    public class MeterStyle
    {
        /// <summary>
        /// 读取类
        /// </summary>
        public class ReadStyle
        {
            /// <summary>
            /// 读取内容
            /// </summary>
            public Dictionary<string, string> Values
            { get; set; }
            /// <summary>
            /// 当前读取类型
            /// </summary>
            public All.Class.TypeUse.TypeList ReadType
            { get; set; }
            public ReadStyle()
                : this(new Dictionary<string, string>())
            {
            }
            public ReadStyle(Dictionary<string, string> value)
            {
                this.Values = value;
                this.ReadType = All.Class.TypeUse.TypeList.UnKnow;
            }
        }
        /// <summary>
        /// 所有读取信息
        /// </summary>
        public List<ReadStyle> Reads
        { get; set; }
        /// <summary>
        /// 设备值
        /// </summary>
        public Meter.Meter Value
        { get; set; }

        List<byte> tmpByte;
        List<bool> tmpBool;
        List<DateTime> tmpDateTime;
        List<int> tmpInt;
        List<long> tmpLong;
        List<float> tmpFloat;
        List<double> tmpDouble;
        List<ushort> tmpUshort;
        List<string> tmpString;
        Class.TypeUse.TypeList tmpType;
        List<int> tmpIndex;

        public MeterStyle()
        {
            Reads = new List<ReadStyle>();
            Value = null;
            tmpByte = new List<byte>();
            tmpBool = new List<bool>();
        }
    }
}
