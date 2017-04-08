using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace All.Meter
{
    public class SSMeter:Meter
    {
        List<ushort> ushortVaulue = new List<ushort>();
        List<int> intValue = new List<int>();
        List<string> stringValue = new List<string>();
        List<float> floatValue = new List<float>();
        List<double> doubleValue = new List<double>();
        List<long> longValue = new List<long>();
        List<byte> byteValue = new List<byte>();
        List<bool> boolValue = new List<bool>();

        Dictionary<string, string> initParm = new Dictionary<string, string>();

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            InitParm = initParm;
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
        
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            throw new NotImplementedException();
        }
        public override bool Test()
        {
            throw new NotImplementedException();
        }
    }
}
