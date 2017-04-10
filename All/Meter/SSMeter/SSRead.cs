using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class SSRead:Meter
    {
        ushort[] ushortValue = new ushort[0];
        int[] intValue = new int[0];
        string[] stringValue = new string[0];
        float[] floatValue = new float[0];
        double[] doubleValue = new double[0];
        long[] longValue = new long[0];
        byte[] byteValue = new byte[0];
        bool[] boolValue = new bool[0];
        DateTime[] dateTimeValue = new DateTime[0];
        List<bool> tmpBool;
        List<byte> tmpByte;
        List<ushort> tmpUshort;
        List<int> tmpInt;
        List<long> tmpLong;
        List<double> tmpDouble;
        List<float> tmpFloat;
        List<string> tmpString;
        List<DateTime> tmpDateTime;
        Dictionary<string, string> initParm = new Dictionary<string, string>();

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            InitParm = initParm;
            if (this.Parent == null)
            {
                All.Class.Error.Add("SSMeter初始化前,必须先初始化设置Parent通讯类");
                return;
            }
            if (this.Parent.Meters.Count > 0)
            {
                All.Class.Error.Add("SSMeter必须独占一个通讯类,如果有多组通讯类,请分开处理");
                return;
            }
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
            #region//缓存区初始化
            if (initParm.ContainsKey("Byte"))
            {
                byteValue = new byte[initParm["Byte"].ToInt()];
                byteValue.Initialize();
            }
            if (initParm.ContainsKey("Bool"))
            {
                boolValue = new bool[initParm["Bool"].ToInt()];
                boolValue.Initialize();
            }
            if (initParm.ContainsKey("Long"))
            {
                longValue = new long[initParm["Long"].ToInt()];
                longValue.Initialize();
            }
            if (initParm.ContainsKey("Ushort"))
            {
                ushortValue = new ushort[initParm["Ushort"].ToInt()];
                ushortValue.Initialize();
            }
            if (initParm.ContainsKey("Int"))
            {
                intValue = new int[initParm["Int"].ToInt()];
                intValue.Initialize();
            }
            if (initParm.ContainsKey("Float"))
            {
                floatValue = new float[initParm["Float"].ToInt()];
                floatValue.Initialize();
            }
            if (initParm.ContainsKey("Double"))
            {
                doubleValue = new double[initParm["Double"].ToInt()];
                doubleValue.Initialize();
            }
            if (initParm.ContainsKey("String"))
            {
                stringValue = new string[initParm["String"].ToInt()];
                for (int i = 0; i < stringValue.Length; i++)
                {
                    stringValue[i] = "";
                }
            }
            if (initParm.ContainsKey("DateTime"))
            {
                dateTimeValue = new DateTime[initParm["DateTime"].ToInt()];
                for (int i = 0; i < dateTimeValue.Length; i++)
                {
                    dateTimeValue[i] = DateTime.Now;
                }
            }
            if (initParm.ContainsKey("byte"))
            {
                byteValue = new byte[initParm["byte"].ToInt()];
                byteValue.Initialize();
            }
            if (initParm.ContainsKey("bool"))
            {
                boolValue = new bool[initParm["bool"].ToInt()];
                boolValue.Initialize();
            }
            if (initParm.ContainsKey("long"))
            {
                longValue = new long[initParm["long"].ToInt()];
                longValue.Initialize();
            }
            if (initParm.ContainsKey("ushort"))
            {
                ushortValue = new ushort[initParm["ushort"].ToInt()];
                ushortValue.Initialize();
            }
            if (initParm.ContainsKey("int"))
            {
                intValue = new int[initParm["int"].ToInt()];
                intValue.Initialize();
            }
            if (initParm.ContainsKey("float"))
            {
                floatValue = new float[initParm["float"].ToInt()];
                floatValue.Initialize();
            }
            if (initParm.ContainsKey("double"))
            {
                doubleValue = new double[initParm["double"].ToInt()];
                doubleValue.Initialize();
            }
            if (initParm.ContainsKey("string"))
            {
                stringValue = new string[initParm["string"].ToInt()];
                for (int i = 0; i < stringValue.Length; i++)
                {
                    stringValue[i] = "";
                }
            }
            if (initParm.ContainsKey("dateTime"))
            {
                dateTimeValue = new DateTime[initParm["dateTime"].ToInt()];
                for (int i = 0; i < dateTimeValue.Length; i++)
                {
                    dateTimeValue[i] = DateTime.Now;
                }
            }
            #endregion
            this.Parent.GetArgs += Parent_GetArgs;
        }
        private void Parent_GetArgs(object sender, Communicate.Base.Base.ReciveArgs reciveArgs)
        {
            lock (lockObject)
            {
                Communicate.Communicate parent = (Communicate.Communicate)sender;
                int len = parent.DataRecive;
                byte[] buff;
                if (len > 0 && this.Read<byte[]>(len, out buff))
                {
                    SSMeter.SSMeter.DataStyle ds = SSMeter.SSMeter.DataStyle.Parse(buff);
                    if (ds == null)
                    {
                        return;
                    }
                    SSMeter.SSMeter.ResultStyle rs = new SSMeter.SSMeter.ResultStyle(ds.Random, false);
                    if (ds.Value != null)
                    {
                        rs.Result = true;
                        switch (ds.Type)
                        {
                            case Class.TypeUse.TypeList.Boolean:
                                tmpBool = (List<bool>)ds.Value;
                                for (int i = ds.Start; i < boolValue.Length && i < ds.Start + tmpBool.Count; i++)
                                {
                                    boolValue[i] = tmpBool[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.Byte:
                                tmpByte = (List<byte>)ds.Value;
                                for (int i = ds.Start; i < byteValue.Length && i < ds.Start + tmpByte.Count; i++)
                                {
                                    byteValue[i] = tmpByte[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.DateTime:
                                tmpDateTime = (List<DateTime>)ds.Value;
                                for (int i = ds.Start; i < dateTimeValue.Length && i < ds.Start + tmpDateTime.Count; i++)
                                {
                                    dateTimeValue[i] = tmpDateTime[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.Double:
                                tmpDouble = (List<double>)ds.Value;
                                for (int i = ds.Start; i < doubleValue.Length && i < ds.Start + tmpDouble.Count; i++)
                                {
                                    doubleValue[i] = tmpDouble[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.Float:
                                tmpFloat = (List<float>)ds.Value;
                                for (int i = ds.Start; i < floatValue.Length && i < ds.Start + tmpFloat.Count; i++)
                                {
                                    floatValue[i] = tmpFloat[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.Int:
                                tmpInt = (List<int>)ds.Value;
                                for (int i = ds.Start; i < intValue.Length && i < ds.Start + tmpInt.Count; i++)
                                {
                                    intValue[i] = tmpInt[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.Long:
                                tmpLong = (List<long>)ds.Value;
                                for (int i = ds.Start; i < longValue.Length && i < ds.Start + tmpLong.Count; i++)
                                {
                                    longValue[i] = tmpLong[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.String:
                                tmpString = (List<string>)ds.Value;
                                for (int i = ds.Start; i < stringValue.Length && i < ds.Start + tmpString.Count; i++)
                                {
                                    stringValue[i] = tmpString[i - ds.Start];
                                }
                                break;
                            case Class.TypeUse.TypeList.UShort:
                                tmpUshort = (List<ushort>)ds.Value;
                                for (int i = ds.Start; i < ushortValue.Length && i < ds.Start + tmpUshort.Count; i++)
                                {
                                    ushortValue[i] = tmpUshort[i - ds.Start];
                                }
                                break;
                        }
                    }
                    if (parent is Communicate.Udp)
                    {
                        Dictionary<string, string> parm = new Dictionary<string, string>();
                        parm.Add("RemotHost", reciveArgs.RemotIP);
                        parm.Add("RemotPort", reciveArgs.RemotPort.ToString());
                        parent.Send<byte[]>(rs.GetBytes(), parm);
                    }
                    else
                    {
                        parent.Send<byte[]>(rs.GetBytes());
                    }
                }
            }
        }
        public override void Close()
        {
            this.Parent.GetArgs -= Parent_GetArgs;
            base.Close();
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            int start = -1;
            int end = -1;
            value = new List<T>();
            if (!parm.ContainsKey("Start"))
            {
                All.Class.Error.Add(string.Format("{0}:读取数据不包含起始点", this.Text), Environment.StackTrace);
                return false;
            }
            start = All.Class.Num.ToInt(parm["Start"]);
            if (start < 0)
            {
                All.Class.Error.Add(string.Format("{0}:读取数据点不能从小于0的点开始", this.Text), Environment.StackTrace);
                return false;
            }
            if (!parm.ContainsKey("End"))
            {
                All.Class.Error.Add(string.Format("{0}:读取数据不包含结束点", this.Text), Environment.StackTrace);
                return false;
            }
            end = All.Class.Num.ToInt(parm["End"]);
            switch (All.Class.TypeUse.GetType<T>())
            {
                case Class.TypeUse.TypeList.Boolean:
                    for (int i = start; i <= end && i < boolValue.Length; i++)
                    {
                        value.Add((T)(object)boolValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Byte:
                    for (int i = start; i <= end && i < byteValue.Length; i++)
                    {
                        value.Add((T)(object)byteValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.DateTime:
                    for (int i = start; i <= end && i < dateTimeValue.Length; i++)
                    {
                        value.Add((T)(object)dateTimeValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Double:
                    for (int i = start; i <= end && i < doubleValue.Length; i++)
                    {
                        value.Add((T)(object)doubleValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Float:
                    for (int i = start; i <= end && i < floatValue.Length; i++)
                    {
                        value.Add((T)(object)floatValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Int:
                    for (int i = start; i <= end && i < intValue.Length; i++)
                    {
                        value.Add((T)(object)intValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.Long:
                    for (int i = start; i <= end && i < longValue.Length; i++)
                    {
                        value.Add((T)(object)longValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.String:
                    for (int i = start; i <= end && i < stringValue.Length; i++)
                    {
                        value.Add((T)(object)stringValue[i]);
                    }
                    break;
                case Class.TypeUse.TypeList.UShort:
                    for (int i = start; i <= end && i < ushortValue.Length; i++)
                    {
                        value.Add((T)(object)ushortValue[i]);
                    }
                    break;
            }
            return true;
        }
        public override bool Test()
        {
            return true;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            throw new Exception("只读数据类,不能进行数据写入操作,写入类请使用SSWrite");
        }
    }
}
