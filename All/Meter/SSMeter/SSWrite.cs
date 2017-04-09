using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class SSWrite:Meter
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
            if (initParm.ContainsKey("ErrorCount"))
            {
                this.ErrorCount = All.Class.Num.ToInt(initParm["ErrorCount"]);
            }
        }
        public override bool Test()
        {
            return WriteInternal<string>(null, 0, 0);
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            throw new Exception("只写数据类,不能进行读取接收,接收类请使用SSRead");
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                try
                {
                    int address = -1;
                    if (parm.ContainsKey("Start"))
                    {
                        address = All.Class.Num.ToUshort(parm["Start"]);
                    }
                    if (parm.ContainsKey("Address"))
                    {
                        address = All.Class.Num.ToUshort(parm["Address"]);
                    }
                    if (address < 0)
                    {
                        All.Class.Error.Add("当前写入数据,起始地址不存在,无法写入");
                        return false;
                    }
                    SSMeter.SSMeter.DataStyle ds = new SSMeter.SSMeter.DataStyle(
                        All.Class.TypeUse.GetType<T>(), address, value);
                    byte[] readBuff;
                    if (WriteAndRead<byte[], byte[]>(ds.GetBytes(), 6, out readBuff))
                    {
                        SSMeter.SSMeter.ResultStyle rs = SSMeter.SSMeter.ResultStyle.Parse(readBuff);
                        if (rs == null || rs.Random != ds.Random || !rs.Result)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                    return false;
                }
                return true;
            }
        }
    }
}
