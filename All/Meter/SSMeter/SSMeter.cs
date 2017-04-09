using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter.SSMeter
{
    [Serializable]
    public class SSMeter
    {
        public class ResultStyle
        {
            /// <summary>
            /// 随机数据,做数据校验检测用
            /// </summary>
            public int Random
            { get; set; }
            /// <summary>
            /// 接收结果 
            /// </summary>
            public bool Result
            { get; set; }
            public ResultStyle()
                : this(0, false)
            { }
            public ResultStyle(int random, bool result)
            {
                this.Random = random;
                this.Result = result;
            }
            /// <summary>
            /// 将类转化为发关的数据
            /// </summary>
            /// <returns></returns>
            public byte[] GetBytes()
            {
                List<byte> buff = new List<byte>();
                buff.AddRange(BitConverter.GetBytes(this.Random));
                buff.AddRange(BitConverter.GetBytes(this.Result));
                buff.Add(All.Class.Check.XorCheck(buff.ToArray(), 0, buff.Count));
                return buff.ToArray();
            }
            /// <summary>
            /// 将字节转化为类
            /// </summary>
            /// <param name="buff"></param>
            /// <returns></returns>
            public static ResultStyle Parse(byte[] buff)
            {
                if (buff == null)
                {
                    return null;
                }
                if (buff.Length !=6)
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到数据不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                if (buff[buff.Length - 1] != All.Class.Check.XorCheck(buff, 0, buff.Length - 1))
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到的校验不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                ResultStyle result = new ResultStyle();
                result.Random = BitConverter.ToInt32(buff, 0);
                result.Result = BitConverter.ToBoolean(buff, 4);
                return result;
            }
        }
        public class DataStyle
        {
            /// <summary>
            /// 发送或接收的数据类型
            /// </summary>
            public All.Class.TypeUse.TypeList Type
            {get;set;}
            /// <summary>
            /// 发送或接收的起始值
            /// </summary>
            public int Start
            {get;set;}
            /// <summary>
            /// 发送或接收的数据
            /// </summary>
            public object Value
            {get;set;}
            /// <summary>
            /// 随机数
            /// </summary>
            public int Random
            { get; set; }
            public DataStyle()
                : this(All.Class.TypeUse.TypeList.UnKnow, 0, null)
            { }
            public DataStyle(All.Class.TypeUse.TypeList type,int start,object value)
            {
                this.Random = (int)All.Class.Num.GetRandom(0, 99999);
                this.Type = type;
                this.Start = start;
                this.Value = value;
            }
            /// <summary>
            /// 将当前类,转化为字节
            /// </summary>
            /// <returns></returns>
            public byte[] GetBytes()
            {
                //类型1,开始4,随机码4,数据n,校验1
                List<byte> buff = new List<byte>();
                buff.Add((byte)Type);
                buff.AddRange(BitConverter.GetBytes(this.Start));
                buff.AddRange(BitConverter.GetBytes(this.Random));
                buff.AddRange(All.Class.SingleFileSave.SSFile.Object2Byte(this.Value));
                buff.Add(All.Class.Check.XorCheck(buff.ToArray(), 0, buff.Count));
                return buff.ToArray();
            }
            /// <summary>
            /// 将字节转化为类
            /// </summary>
            /// <param name="buff"></param>
            /// <returns></returns>
            public static DataStyle Parse(byte[] buff)
            {
                if (buff == null)
                {
                    return null;
                }
                if (buff.Length < 10)
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到数据不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                if (buff[buff.Length - 1] != All.Class.Check.XorCheck(buff, 0, buff.Length - 1))
                {
                    All.Class.Error.Add("错误数据", string.Format("接收到的校验不正确:{0}", All.Class.Num.Hex2Str(buff)));
                    return null;
                }
                DataStyle result = new DataStyle();
                result.Type = (All.Class.TypeUse.TypeList)buff[0];
                result.Start = BitConverter.ToInt32(buff, 1);
                result.Random = BitConverter.ToInt32(buff, 5);
                result.Value = All.Class.SingleFileSave.SSFile.Byte2Object(buff, 9, buff.Length - 10);
                return result;
            }
        }
    }
}
