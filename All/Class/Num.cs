using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace All.Class
{
    public static class Num
    {
        /// <summary>
        /// 分隔字符
        /// </summary>
        public const char SplitRow = (char)0x1F;
        /// <summary>
        /// 分段字符
        /// </summary>
        public const char SplitColumn = (char)0x1C;
        /// <summary>
        /// 分割符
        /// </summary>
        public static char[] Split = new char[] { SplitRow, SplitColumn };
        /// <summary>
        /// 浮点数据与字节互转规则
        /// </summary>
        public enum QueueList
        {
            一二三四,
            二一四三,
            三四一二,
            四三二一,
        }
        /// <summary>
        /// 字节顺序重组
        /// </summary>
        public enum ByteQueueList
        {
            一二三四五六七八,
            二一四三六五八七,
            三四一二七八五六,
            四三二一八七六五,
            五六七八一二三四,
            六五八七二一四三,
            七八五六三四一二,
            八七六五四三二一
        }
        #region//数据类型转换
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value)
        {
            DateTime tmp = DateTime.Now;
            if (value == null)
            {
                return tmp;
            }
            DateTime.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Single ToSingle(object value)
        {
            return ToFloat(value);
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(object value)
        {
            short tmp = 0;
            if (value == null)
            {
                return tmp;
            }
            short.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(object value)
        {
            float tmp = 0;
            if (value == null)
            {
                return tmp;
            }
            float.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(object value)
        {
            double tmp = 0;
            if (value == null)
            {
                return tmp;
            }
            string tmpvalue = value.ToString(); 
            double.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            long tmp = 0;
            if (value == null)
            {
                return tmp;
            }
            long.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(object value)
        {
            byte tmp = 0;
            if (value == null)
            {
                return tmp;
            }
            byte.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(object value)
        {
            int tmp = 0;
            if (value == null)
            {
                return tmp;
            } 
            int.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort ToUshort(object value)
        {
            ushort tmp = 0;
            if (value == null)
            {
                return tmp;
            }
            ushort.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(object value, int NullValue)
        {
            if (value == null)
            {
                return NullValue;
            }
            return ToInt(value);
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value)
        {
            if (value == null)
            {
                return "";
            }
            return value.ToString();
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(object value, string NullValue)
        {
            if (value == null)
            {
                return NullValue;
            }
            return ToString(value);
        }
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(object value)
        {
            bool tmp = false;
            if (value == null)
            {
                return false;
            }
            bool.TryParse(value.ToString(), out tmp);
            return tmp;
        }
        #endregion
        #region//取随机数
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns></returns>
        public static double GetRandom()
        {
            return GetRandom(1)[0];
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static double[] GetRandom(int len)
        {
            return GetRandom(len, 0, 1);
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double GetRandom(double min, double max)
        {
            return GetRandom(1, min, max)[0];
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="len"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double[] GetRandom(int len, double min, double max)
        {
            Guid g; Random r;
            double[] tmpRandom = new double[len];
            for (int i = 0; i < len; i++)
            {
                g = Guid.NewGuid();
                byte[] tmpB = g.ToByteArray();
                int tmpI = tmpB[0] * 1 + tmpB[2] * 2 + tmpB[4] * 4 + tmpB[6] * 8 + tmpB[8] * 16 + tmpB[10] * 32 + tmpB[12] * 64 + tmpB[14] * 128;
                r = new Random(tmpI);
                tmpRandom[i] = r.NextDouble() * (max - min) + min;
            }
            return tmpRandom;
        }
        #endregion
        #region//文本绘画高度与宽度
        /// <summary>
        /// 根据字体大小来获取文字高度
        /// </summary>
        /// <param name="Font"></param>
        /// <returns></returns>
        public static int GetFontHeight(System.Drawing.Font Font)
        {
            return System.Windows.Forms.TextRenderer.MeasureText("Shuai", Font).Height;
        }
        /// <summary>
        /// 根据字体大小来获取文字长度
        /// </summary>
        /// <param name="Font"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetFontWidth(System.Drawing.Font Font, string value)
        {
            using (System.Drawing.Bitmap b = new System.Drawing.Bitmap(1, 1))
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(b))
                {
                    return (int)g.MeasureString(value, Font).Width;
                }
            }
        }
        #endregion
        #region//浮点数与字节转换
        /// <summary>
        /// 将数组转化为浮点数
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static float[] BytesToFloat(byte[] buff, QueueList queue)
        {
            if ((buff.Length % 4) != 0)
            {
                return null;
            }
            float[] result = new float[buff.Length / 4];
            byte[] tmpBuff = new byte[4];
            for (int i = 0, j = 0; i < buff.Length && j < result.Length; i = i + 4, j++)
            {
                tmpBuff = new byte[4];
                Array.Copy(buff, i, tmpBuff, 0, 4);
                result[j] = ByteToFloat(tmpBuff, queue);
            }
            return result;
        }
        /// <summary>
        /// 数组转化为浮点数
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static float ByteToFloat(byte[] buff, int start, QueueList queue)
        {
            if ((start + 4) > buff.Length)
            {
                return 0;
            }
            byte[] tmpBuff = new byte[4];
            Array.Copy(buff, start, tmpBuff, 0, 4);
            return ByteToFloat(tmpBuff, queue);
        }
        /// <summary>
        /// 数组转化为浮点数
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static float ByteToFloat(byte[] buff, QueueList queue)
        {
            if (buff.Length != 4)
            {
                return 0;
            }
            byte[] tmpbuff = new byte[4];
            switch (queue)
            {
                case QueueList.二一四三:
                    tmpbuff[0] = buff[1];
                    tmpbuff[1] = buff[0];
                    tmpbuff[2] = buff[3];
                    tmpbuff[3] = buff[2];
                    break;
                case QueueList.三四一二:
                    tmpbuff[0] = buff[2];
                    tmpbuff[1] = buff[3];
                    tmpbuff[2] = buff[0];
                    tmpbuff[3] = buff[1];
                    break;
                case QueueList.四三二一:
                    tmpbuff[0] = buff[3];
                    tmpbuff[1] = buff[2];
                    tmpbuff[2] = buff[1];
                    tmpbuff[3] = buff[0];
                    break;
                default:
                    tmpbuff = buff;
                    break;
            }
            return BitConverter.ToSingle(tmpbuff, 0);
        }
        /// <summary>
        /// 将浮点转化为指定数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static byte[] FloatToByte(float value, QueueList queue)
        {
            byte[] buff = BitConverter.GetBytes(value);
            byte[] result = new byte[4];
            switch (queue)
            {
                case QueueList.二一四三:
                    result[0] = buff[1];
                    result[1] = buff[0];
                    result[2] = buff[3];
                    result[3] = buff[2];
                    break;
                case QueueList.三四一二:
                    result[0] = buff[2];
                    result[1] = buff[3];
                    result[2] = buff[0];
                    result[3] = buff[1];
                    break;
                case QueueList.四三二一:
                    result[0] = buff[3];
                    result[1] = buff[2];
                    result[2] = buff[1];
                    result[3] = buff[0];
                    break;
                default:
                    result = buff;
                    break;
            }
            return result;
        }
        #endregion
        #region//字节操作
        /// <summary>
        /// 获取当天是第几周
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime time)
        {
            CultureInfo ci = new CultureInfo("zh-CN");
            return ci.Calendar.GetWeekOfYear(time, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        }
        /// <summary>
        /// 截取字节
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] GetByte(byte[] buff, int start, int len)
        {
            if ((start + len) > buff.Length)
            {
                return null;
            }
            byte[] result = new byte[len];
            Array.Copy(buff, start, result, 0, len);
            return result;
        }
        /// <summary>
        /// 将字节内高低按位调反
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        [Obsolete("过时,请用SwitchByte")]
        public static byte[] SwitchBit(byte[] buff)
        {
            byte[] result = new byte[buff.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                result[i] = Bool2Byte(Byte2Bool(buff[i]).Reverse().ToArray())[0];
            }
            return result;
        }
        /// <summary>
        /// 按双字的方法来改变字节顺序
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static byte[] SwitchWord(byte[] buff, QueueList queue)
        {
            if ((buff.Length % 4) != 0)
            {
                return buff;
            }
            byte[] result = new byte[buff.Length];
            switch (queue)
            {
                case QueueList.一二三四:
                    return buff;
                case QueueList.二一四三:
                    for (int i = 0; i < buff.Length; i = i + 4)
                    {
                        result[i + 0] = buff[i + 1];
                        result[i + 1] = buff[i + 0];
                        result[i + 2] = buff[i + 3];
                        result[i + 3] = buff[i + 2];
                    }
                    break;
                case QueueList.三四一二:
                    for (int i = 0; i < buff.Length; i = i + 4)
                    {
                        result[i + 0] = buff[i + 2];
                        result[i + 1] = buff[i + 3];
                        result[i + 2] = buff[i + 0];
                        result[i + 3] = buff[i + 1];
                    }
                    break;
                case QueueList.四三二一:
                    for (int i = 0; i < buff.Length; i = i + 4)
                    {
                        result[i + 0] = buff[i + 3];
                        result[i + 1] = buff[i + 2];
                        result[i + 2] = buff[i + 1];
                        result[i + 3] = buff[i + 0];
                    }
                    break;
            }
            return result;
        }
        /// <summary>
        /// 改变字节内顺序
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static byte[] SwitchByte(byte[] buff, ByteQueueList queue)
        {
            List<byte> result = new List<byte>();
            switch (queue)
            {
                case ByteQueueList.一二三四五六七八:
                    buff.ToList().ForEach(value => result.Add(value));
                    break;
                case ByteQueueList.二一四三六五八七:
                    buff.ToList().ForEach(value => result.Add((byte)((((value & 0x55) << 1) | ((value & 0xAA) >> 1)))));
                    break;
                case ByteQueueList.三四一二七八五六:
                    buff.ToList().ForEach(value => result.Add((byte)((((value & 0x33) << 2) | ((value & 0xCC) >> 2)))));
                    break;
                case ByteQueueList.四三二一八七六五:
                    buff.ToList().ForEach(value => result.Add((byte)((((value & 0x11) << 3) | ((value & 0x22) << 1) | ((value & 0x44) >> 1) | ((value & 0x88) >> 3)))));
                    break;
                case ByteQueueList.五六七八一二三四:
                    buff.ToList().ForEach(value => result.Add((byte)((((value & 0x0F) << 4) | ((value & 0xF0) >> 4)))));
                    break;
                case ByteQueueList.六五八七二一四三:
                    buff.ToList().ForEach(value => result.Add((byte)((((value & 0xA0) >> 5) | ((value & 0x50) >> 3) | ((value & 0x0A) << 3) | ((value & 0x05) << 5)))));
                    break;
                case ByteQueueList.七八五六三四一二:
                    buff.ToList().ForEach(value => result.Add((byte)((((value & 0x03) << 6) | ((value & 0x0C) << 2) | ((value & 0xC0) >> 6) | ((value & 0x30) >> 2)))));
                    break;
                case ByteQueueList.八七六五四三二一:
                    SwitchByte(SwitchByte(buff, ByteQueueList.五六七八一二三四), ByteQueueList.四三二一八七六五).ToList().ForEach(value => result.Add(value));
                    break;
            }
            return result.ToArray();
        }
        /// <summary>
        /// 将高低字节调反
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static byte[] SwitchHighAndLow(byte[] buff)
        {
            byte tmp = 0;
            for (int i = 0; i < buff.Length; i = i + 2)
            {
                if ((i + 1) < buff.Length)
                {
                    tmp = buff[i];
                    buff[i] = buff[i + 1];
                    buff[i + 1] = tmp;
                }
            }
            return buff;
        }
        /// <summary>
        /// 高低字节调反
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort SwitchHighAndLow(ushort value)
        {
            return (ushort)(((value >> 8) & 0xFF) + ((value & 0xFF) << 8));
        }
        /// <summary>
        /// 将bool数据转为字节数组表示的0,1状态
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static byte[] Bool2Byte(bool[] buff)
        {
            return Bool2Byte(buff, 0, buff.Length);
        }
        /// <summary>
        /// 将bool数据转为字节数组表示的0,1状态
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] Bool2Byte(bool[] buff, int start, int len)
        {
            int curLen = (int)Math.Ceiling(len / 8.0f);
            byte[] result = new byte[curLen];
            for (int i = 0, k = 0; i < buff.Length && k < result.Length; i = i + 8, k++)
            {
                result[k] = 0;
                for (int j = 0; j < 8 && (i + j) < buff.Length; j++)
                {
                    if (buff[i + j])
                    {
                        result[k] += (byte)(1 << j);// Math.Pow(2, j);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 将UShort数组转化Bool数组
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static bool[] Ushort2Bool(ushort[] buff, int start, int len)
        {
            byte[] tmpBuff = new byte[len * 2];
            for (int i = 0; i < len; i++)
            {
                tmpBuff[i * 2] = (byte)(buff[start + i] & 0xFF);
                tmpBuff[i * 2 + 1] = (byte)((buff[start + i] >> 8) & 0xFF);
            }
            return Byte2Bool(tmpBuff, 0, tmpBuff.Length);
        }
        /// <summary>
        /// 将UShort数组转化Bool数组
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static bool[] Ushort2Bool(ushort[] buff)
        {
            return Ushort2Bool(buff, 0, buff.Length);
        }
        /// <summary>
        /// 将UShort转化为Bool数组
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static bool[] Ushort2Bool(ushort buff)
        {
            ushort[] tmpBuff = new ushort[1];
            tmpBuff[0] = buff;
            return Ushort2Bool(tmpBuff);
        }
        /// <summary>
        /// 将UInt32转化为Bool数组
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static bool[] UInt2Bool(UInt32 buff)
        {
            ushort[] tmpBuff = new ushort[2];
            tmpBuff[0] = (ushort)(buff & ushort.MaxValue);
            tmpBuff[1] = (ushort)((buff >> 16) & ushort.MaxValue);
            return Ushort2Bool(tmpBuff);
        }
        /// <summary>
        /// 将字节数组的0,1状态转换为bool数组
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static bool[] Byte2Bool(byte[] buff, int start, int len)
        {

            bool[] result = new bool[Math.Min(buff.Length, len) * 8];
            for (int i = 0; i < buff.Length && i < len; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //if ((buff[i + start] & (byte)Math.Pow(2, j)) == (byte)Math.Pow(2, j))
                    if (((buff[i + start] >> j) & 1) == 1)
                    {
                        result[i * 8 + j] = true;
                    }
                    else
                    {
                        result[i * 8 + j] = false;
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 将BYTE转化为BOOL
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static bool[] Byte2Bool(byte buff)
        {
            return Byte2Bool(new byte[] { buff });
        }
        /// <summary>
        /// 将字节数组的0,1状态转化bool数组
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static bool[] Byte2Bool(byte[] buff)
        {
            return Byte2Bool(buff, 0, buff.Length);
        }
        /// <summary>
        /// 将USHORT字节转化为数组
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] Ushort2Byte(ushort[] buff, int start, int len)
        {
            byte[] result = new byte[len * 2];
            for (int i = 0; i < len; i++)
            {
                result[i * 2] = (byte)((buff[start + i] >> 8) & 0xFF);
                result[i * 2 + 1] = (byte)((buff[start + i] >> 0) & 0xFF);
            }
            return result;
        }
        /// <summary>
        /// 将USHORT字节转化为数组
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static byte[] Ushort2Byte(ushort[] buff)
        {
            return Ushort2Byte(buff, 0, buff.Length);
        }
        #endregion
        /// <summary>
        /// 去除掉字符串中的非数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimOnlyNum(string value)
        {
            //string result = "";
            //for (int i = 0; i < value.Length; i++)
            //{
            //    if (Check.isFix(value.Substring(i, 1), Check.RegularList.非负整数))
            //    {
            //        result = string.Format("{0}{1}", result, value.Substring(i, 1));
            //    }
            //}
            if (value == null)
            {
                throw new ArgumentNullException();
            }
            return Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value).ToList().Where(
                tmp =>
                {
                    return tmp >= 0x30 && tmp <= 0x39;
                }).ToArray());
        }
        /// <summary>
        /// 筛选可见字符
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string GetVisableHex(byte[] buff)
        {
            return GetVisableHex(buff, null);
        }
        /// <summary>
        /// 筛选可见字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetVisableHex(string value)
        {
            return GetVisableHex(value, "");
        }
        /// <summary>
        /// 筛选可见字符
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string GetVisableStr(byte[] buff)
        {
            //List<byte> result = new List<byte>();
            //if (buff != null && buff.Length > 0)
            //{
            //    for (int i = 0; i < buff.Length; i++)
            //    {
            //        if (buff[i] >= 33 && buff[i] <= 126)
            //        {
            //            result.Add(buff[i]);
            //        }
            //    }
            //}
            //return Encoding.ASCII.GetString(result.ToArray());
            if (buff == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(buff.ToList().Where(
                tmp =>
                {
                    return tmp >= 0x21 && tmp <= 0x7E;
                }).ToArray());
        }
        /// <summary>
        /// 筛选可见字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetVisableStr(string value)
        {
            return GetVisableStr(Encoding.ASCII.GetBytes(value));
        }
        /// <summary>
        /// 筛选可见字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="except">例外字符</param>
        /// <returns></returns>
        public static string GetVisableHex(string value, string except)
        {
            return GetVisableHex(Encoding.ASCII.GetBytes(value), Encoding.ASCII.GetBytes(except));
        }
        /// <summary>
        /// 筛选可见字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="except">例外字符</param>
        /// <returns></returns>
        public static string GetVisableHex(byte[] buff, byte[] except)
        {
            if (buff == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(buff.ToList().Where(
                  tmp =>
                  {
                      return IsVisableHex(tmp) || (except != null && except.Contains(tmp));
                  }).ToArray());
        }
        /// <summary>
        /// 去掉指定字符以外的其他字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string TrimOnlyUse(string value, string[] buff)
        {
            string result = "";
            List<string> tmpBuff = buff.ToList();
            string str = "";
            for (int i = 0; i < value.Length; i++)
            {
                str = value.Substring(i, 1);
                if (tmpBuff.Contains(str))
                {
                    result = string.Format("{0}{1}", result, str);
                }
            }
            return result;
        }
        [Obsolete("方法过时，请用GetVisableHex")]
        /// <summary>
        /// 获取可见字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimOnlyVisible(string value)
        {
            List<byte> tmpBuff = new List<byte>();
            try
            {
                byte[] buff = Encoding.ASCII.GetBytes(value);
                for (int i = 0; i < buff.Length; i++)
                {
                    if (IsVisableHex(buff[i]))
                    {
                        tmpBuff.Add(buff[i]);
                    }
                }
            }
            catch
            {
            }
            if (tmpBuff.Count == 0)
            {
                return "";
            }
            return Encoding.ASCII.GetString(tmpBuff.ToArray(), 0, tmpBuff.Count);
        }
        /// <summary>
        /// 当前字符是否为可见条码字符
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static bool IsVisableHex(byte buff)
        {
            return (buff >= 0x30 && buff <= 0x39) ||
                    (buff >= 0x41 && buff <= 0x5A) ||
                    (buff >= 0x61 && buff <= 0x7A);
        }
        /// <summary>
        /// 将数组以字符串的形式输出
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static string Hex2Str(byte[] buff)
        {
            return Hex2Str(buff, 0, buff.Length);
        }
        /// <summary>
        /// 将数组以字符串的形式输出
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string Hex2Str(byte[] buff, int start, int len)
        {
            StringBuilder result = new StringBuilder();
            for (int i = start; i < buff.Length && i < start + len; i++)
            {
                result.Append(string.Format("{0:X2}", buff[i]));
            }
            return result.ToString();
        }
        /// <summary>
        /// 将16进制字符串转化对应的字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Str2Hex(string str)
        {
            return Str2Hex(str, 0, str.Length);
        }
        /// <summary>
        /// 将16进制字符串转化对应的字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] Str2Hex(string str, int start, int len)
        {
            List<byte> result = new List<byte>();
            if ((len & 1) == 1)
            {
                return result.ToArray();
            }
            try
            {
                for (int i = start; i < start + len && i < str.Length; i = i + 2)
                {
                    result.Add(Convert.ToByte(str.Substring(i, 2), 16));
                }
            }
            catch { }
            return result.ToArray();
        }
        /// <summary>
        /// 获取全局唯一标识符
        /// </summary>
        /// <returns></returns>
        public static string CreateGUID()
        {
            return System.Guid.NewGuid().ToString().ToUpper();
        }
    }
}
