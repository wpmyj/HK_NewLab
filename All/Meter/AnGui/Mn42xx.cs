using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class Mn42xx : AnGui.AnGui
    {
        Dictionary<string, string> initParm;
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            this.InitParm = initParm;
            if (initParm.ContainsKey("Text"))
            {
                this.Text = initParm["Text"];
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
            value = new List<T>();
            return true;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            bool result = true;
            return result;
        }
    }
}
