using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate.Data
{
    /// <summary>
    /// 所有读取数据
    /// </summary>
    public class AllData
    {
        /// <summary>
        /// 字节数据
        /// </summary>
        public ReadDataStyle<byte> ByteValue
        { get; set; }
        /// <summary>
        /// 整形数据
        /// </summary>
        public ReadDataStyle<int> IntValue
        { get; set; }
        /// <summary>
        /// 无符号整数
        /// </summary>
        public ReadDataStyle<ushort> UshortValue
        { get; set; }
        /// <summary>
        /// 单精度浮点数
        /// </summary>
        public ReadDataStyle<float> FloatValue
        { get; set; }
        /// <summary>
        /// 双精度浮点数
        /// </summary>
        public ReadDataStyle<double> DoubleValue
        { get; set; }
        /// <summary>
        /// 长整形数据
        /// </summary>
        public ReadDataStyle<long> LongValue
        { get; set; }
        /// <summary>
        /// 日期数据
        /// </summary>
        public ReadDataStyle<DateTime> DateTimeValue
        { get; set; }
        /// <summary>
        /// 布尔数据
        /// </summary>
        public ReadDataStyle<bool> BoolValue
        { get; set; }
        /// <summary>
        /// 字符串数据
        /// </summary>
        public ReadDataStyle<string> StringValue
        { get; set; }
        /// <summary>
        /// 字符数组数据
        /// </summary>
        public ReadDataStyle<byte[]> BytesValue
        { get; set; }
        public AllData()
        {
            this.BoolValue = new ReadDataStyle<bool>();
            this.BytesValue = new ReadDataStyle<byte[]>();
            this.ByteValue = new ReadDataStyle<byte>();
            this.DateTimeValue = new ReadDataStyle<DateTime>();
            this.DoubleValue = new ReadDataStyle<double>();
            this.FloatValue = new ReadDataStyle<float>();
            this.IntValue = new ReadDataStyle<int>();
            this.LongValue = new ReadDataStyle<long>();
            this.StringValue = new ReadDataStyle<string>();
            this.UshortValue = new ReadDataStyle<ushort>();
        }
    }
}
