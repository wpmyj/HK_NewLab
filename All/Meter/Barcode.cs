using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace All.Meter
{
    public class BarCode : Meter
    {
        Dictionary<string, string> initParm = new Dictionary<string, string>();

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            this.initParm = initParm;
            if (InitParm.ContainsKey("Text"))
            {
                this.Text = InitParm["Text"];
            }
            if (initParm.ContainsKey("TimeOut"))
            {
                this.TimeOut = All.Class.Num.ToInt(initParm["TimeOut"]);
            }
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            bool result = true;
            value = new List<T>();
            switch (Class.TypeUse.GetType<T>())
            {
                case Class.TypeUse.TypeList.String:
                case Class.TypeUse.TypeList.Byte:
                    break;
                default:
                    All.Class.Error.Add(string.Format("{0}:读取数据类型不正确", this.Text), Environment.StackTrace);
                    return false;
            }
            byte[] readBuff;
            if (Read<byte[]>(0, out readBuff))
            {
                if (readBuff != null)
                {
                    switch (Class.TypeUse.GetType<T>())
                    {
                        case Class.TypeUse.TypeList.String:
                            value.Add((T)(object)All.Class.Num.GetVisableStr(Encoding.ASCII.GetString(readBuff)));
                            break;
                        case Class.TypeUse.TypeList.Byte:
                            for (int i = 0; i < readBuff.Length; i++)
                            {
                                value.Add((T)(object)readBuff[i]);
                            }
                            break;
                    }
                }
                else
                {
                    switch (Class.TypeUse.GetType<T>())
                    {
                        case Class.TypeUse.TypeList.String:
                            value.Add((T)(object)"");
                            break;
                        case Class.TypeUse.TypeList.Byte:
                            break;
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        public override bool Test()
        {
            return this.Parent != null && this.Parent.IsOpen;//条码没有办法测试,所以直接返回
        }
        public override bool Read<T>(out List<T> value, int start, int end)
        {
            return Read<T>(out value, new Dictionary<string, string>());
        }
        public override bool Read<T>(out T value, int start)
        {
            List<T> tmpValue = new List<T>();
            value = default(T);
            bool result = Read<T>(out tmpValue, start, start);
            if (tmpValue.Count > 0)
            {
                value = tmpValue[0];
            }
            return result;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
        public override bool WriteInternal<T>(List<T> value, int start, int end)
        {
            throw new NotImplementedException();
        }
        public override bool WriteInternal<T>(T value, int start)
        {
            throw new NotImplementedException();
        }
    }
}
