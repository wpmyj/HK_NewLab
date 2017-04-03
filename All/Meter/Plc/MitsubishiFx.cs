using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class MitsubishiFx:Meter
    {
        Dictionary<string, string> initParm;
        public override Dictionary<string, string> InitParm
        {
            get
            {
                return initParm;
            }
            set
            {
                initParm = value;
            }
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
            lock (lockObject)
            {
                value = new List<T>();
                bool result = true;
                //读取开始点
                if (!parm.ContainsKey("Start") && !parm.ContainsKey("Address"))
                {
                    All.Class.Error.Add("数据读取参数中不包含寄存器地址", Environment.StackTrace);
                    return false;
                }
                ushort tmpAddress = 0;
                if (parm.ContainsKey("Start"))
                {
                    tmpAddress = parm["Start"].ToUshort();
                }
                if (parm.ContainsKey("Address"))
                {
                    tmpAddress = parm["Address"].ToUshort();
                }
                Class.TypeUse.TypeList t = All.Class.TypeUse.GetType<T>();
                //读取区域
                if (!parm.ContainsKey("Code"))
                {
                    switch (t)
                    {
                        case Class.TypeUse.TypeList.Boolean:
                            parm.Add("Code", "M");
                            break;
                        case Class.TypeUse.TypeList.String:
                        case Class.TypeUse.TypeList.UShort:
                        case Class.TypeUse.TypeList.Int:
                        case Class.TypeUse.TypeList.Long:
                        case Class.TypeUse.TypeList.Byte:
                            parm.Add("Code", "D");
                            break;
                    }
                }
                else
                {
                    switch (t)
                    {
                        case Class.TypeUse.TypeList.Boolean:
                            if (parm["Code"].ToUpper() != "X" && parm["Code"].ToUpper() != "M" && parm["Code"].ToUpper() != "Y")
                            {
                                All.Class.Error.Add("读取参数中区域类型和返回数据类型不一致", Environment.StackTrace);
                                return false;
                            }
                            break;
                        case Class.TypeUse.TypeList.String:
                        case Class.TypeUse.TypeList.UShort:
                        case Class.TypeUse.TypeList.Int:
                        case Class.TypeUse.TypeList.Long:
                        case Class.TypeUse.TypeList.Byte:
                            if (parm["Code"].ToUpper() != "D")
                            {
                                All.Class.Error.Add("读取参数中区域类型和返回数据类型不一致", Environment.StackTrace);
                            }
                            break;
                    }
                }
                //读取结束点
                ushort tmpLen = 1;
                int tmpBoolLen = 0;
                int tmpStart = 0;
                if (!parm.ContainsKey("End"))
                {
                    parm.Add("End", tmpAddress.ToString());
                }
                ushort end = parm["End"].ToUshort();
                if (end < tmpAddress)
                {
                    All.Class.Error.Add("读取参数中结束点大于起始点", Environment.StackTrace);
                    return false;
                }
                switch (parm["Code"].ToUpper())
                {
                    case "M":
                        tmpLen = (ushort)(((int)Math.Floor(end / 8f) - (int)Math.Floor(tmpAddress / 8f) + 1) & 0xFFFF);
                        tmpStart = tmpAddress % 8;
                        tmpBoolLen = (tmpLen - 1) * 8 - tmpStart + (end % 8) + 1;
                        tmpAddress = (ushort)((int)Math.Floor(tmpAddress / 8f) + 0x100);
                        break;
                    case "X":
                        tmpLen = (ushort)(((int)Math.Floor(end / 10f) - (int)Math.Floor(tmpAddress / 10f) + 1) & 0xFFFF);
                        tmpStart = tmpAddress % 10;
                        tmpBoolLen = (tmpLen - 1) * 8 - tmpStart + (end % 10) + 1;
                        tmpAddress = (ushort)((int)Math.Floor(tmpAddress / 10f) + 0x80);
                        break;
                    case "Y":
                        tmpLen = (ushort)(((int)Math.Floor(end / 10f) - (int)Math.Floor(tmpAddress / 10f) + 1) & 0xFFFF);
                        tmpStart = tmpAddress % 10;
                        tmpBoolLen = (tmpLen - 1) * 8 - tmpStart + (end % 10) + 1;
                        tmpAddress = (ushort)((int)Math.Floor(tmpAddress / 10f) + 0xA0);
                        break;
                    case "D":
                        tmpLen = (ushort)(2 * (end - tmpAddress + 1) & 0xFFFF);
                        tmpAddress = (ushort)((tmpAddress * 2 + 0x1000) & 0xFFFF);
                        break;
                    case "T":
                    case "C":
                    case "S":
                        All.Class.Error.Add("读取参数中的区域暂时没有驱动,须要添加", Environment.StackTrace);
                        break;
                    default:
                        All.Class.Error.Add("读取参数中的区域错误,PLC没有此区域", Environment.StackTrace);
                        break;
                }
                int len = 4 + 2 * tmpLen;
                byte[] sendBuff = new byte[11];
                byte[] readBuff;
                sendBuff[0] = 0x02;
                sendBuff[1] = 0x30;
                Array.Copy(Encoding.ASCII.GetBytes(string.Format("{0:X4}", tmpAddress)), 0, sendBuff, 2, 4);
                Array.Copy(Encoding.ASCII.GetBytes(string.Format("{0:X2}", tmpLen)), 0, sendBuff, 6, 2);
                sendBuff[8] = 0x03;
                byte sumLow, sumHigh;
                All.Class.Check.SumCheck(sendBuff.ToArray(), 1, sendBuff.Length - 3, out sumLow, out sumHigh);
                Array.Copy(Encoding.ASCII.GetBytes(string.Format("{0:X2}", sumLow)), 0, sendBuff, 9, 2);
                if (WriteAndRead<byte[], byte[]>(sendBuff, len, out readBuff))
                {
                    if (readBuff[0] != 0x02 && readBuff[readBuff.Length - 3] != 0x03)
                    {
                        All.Class.Error.Add("读取参数帧头或帧尾校验失败", Environment.StackTrace);
                        return false;
                    }
                    All.Class.Check.SumCheck(readBuff.ToArray(), 1, readBuff.Length - 3, out sumLow, out sumHigh);
                    if (Encoding.ASCII.GetString(readBuff, readBuff.Length - 2, 2) != string.Format("{0:X2}", sumLow))
                    {
                        All.Class.Error.Add("读取参数和校验失败", Environment.StackTrace);
                        return false;
                    }
                    byte[] tmp;
                    bool[] tmpBool;
                    switch (parm["Code"].ToUpper())
                    {
                        case "X":
                        case "Y":
                        case "M":
                            tmp = Encoding.ASCII.GetString(readBuff, 1, readBuff.Length - 4).ToHexBytes();
                            tmpBool = All.Class.Num.Byte2Bool(tmp);
                            for (int i = tmpStart, j = 0; i < tmpBool.Length && j < tmpBoolLen; i++, j++)
                            {
                                value.Add((T)(object)tmpBool[i]);
                            }
                            break;
                        case "D":
                            tmp = All.Class.Num.SwitchHighAndLow(Encoding.ASCII.GetString(readBuff, 1, readBuff.Length - 4).ToHexBytes());
                            switch (t)
                            {
                                case Class.TypeUse.TypeList.Byte:
                                    for (int i = 0; i < tmp.Length; i++)
                                    {
                                        value.Add((T)(object)tmp[i]);
                                    }
                                    break;
                                case Class.TypeUse.TypeList.UShort:
                                case Class.TypeUse.TypeList.Long:
                                    for (int i = 0; i < tmp.Length - 1; i = i + 2)
                                    {
                                        value.Add((T)(object)((tmp[i] << 8) + tmp[i + 1]));
                                    }
                                    break;
                                case Class.TypeUse.TypeList.String:
                                    value.Add((T)(object)Encoding.ASCII.GetString(tmp));
                                    break;
                                case Class.TypeUse.TypeList.Int:
                                    int tmpInt = 0;
                                    for (int i = 0; i < tmp.Length - 1; i = i + 2)
                                    {
                                        tmpInt = (tmp[i] << 8) + tmp[i + 1];
                                        if ((tmpInt & 0x8000) == 0x8000)
                                        {
                                            tmpInt = -(tmpInt ^ 0xFFFF) - 1;
                                        }
                                        value.Add((T)(object)tmpInt);
                                    }
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }
        public override bool Test()
        {
            ushort value = 0;
            return Read<ushort>(out value, 0);
        }
        /// <summary>
        /// 单个点强制写入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private bool Write(bool value,Dictionary<string,string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                //写入地址
                if (!parm.ContainsKey("Start") && !parm.ContainsKey("Address"))
                {
                    All.Class.Error.Add("数据写入参数中不包含寄存器地址", Environment.StackTrace);
                    return false;
                }
                ushort tmpAddress = 0;
                if (parm.ContainsKey("Start"))
                {
                    tmpAddress = All.Class.Num.ToUshort(parm["Start"]);
                }
                if (parm.ContainsKey("Address"))
                {
                    tmpAddress = All.Class.Num.ToUshort(parm["Address"]);
                }
                //写入区域
                if (!parm.ContainsKey("Code"))
                {
                    parm.Add("Code", "M");
                }
                List<byte> sendBuff = new List<byte>();
                byte[] readBuff;
                switch (parm["Code"])
                {
                    case "M":
                        tmpAddress = (ushort)(0x800 + tmpAddress);
                        break;
                    case "X":
                        tmpAddress = (ushort)(0x400 + Convert.ToUInt16(tmpAddress.ToString(), 8));
                        break;
                    case "Y":
                        tmpAddress = (ushort)(0x500 + Convert.ToUInt16(tmpAddress.ToString(), 8));
                        break;
                    case "S":
                    case "T":
                    case "C":
                        All.Class.Error.Add("数据写入指令正确,但当前驱动没有添加实现,请添加并测试", Environment.StackTrace);
                        break;
                    default:
                        All.Class.Error.Add("数据写入指令不正确,当前驱动没有指定的指令", Environment.StackTrace);
                        return false;
                }
                sendBuff.Add(0x02);
                sendBuff.Add((byte)(value ? 0x37 : 0x38));
                sendBuff.AddRange(All.Class.Num.SwitchWord(Encoding.ASCII.GetBytes(string.Format("{0:X4}", tmpAddress)), Class.Num.QueueList.三四一二));
                sendBuff.Add(0x03);
                byte sumLow,sumHigh;
                All.Class.Check.SumCheck(sendBuff.ToArray(), 1, sendBuff.Count - 1, out sumLow, out sumHigh);
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", sumLow)));
                if (WriteAndRead<byte[], byte[]>(sendBuff.ToArray(), 1, out readBuff))
                {
                    if (readBuff == null || readBuff.Length < 1 || readBuff[0] != 0x06)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }
        /// <summary>
        /// 批量写入的时候,
        /// Y点地址0 -> Y0~Y7 , 1 -> Y10~Y17 , 2->Y20~Y27
        /// M点地址0 -> M0~M7 , 1 -> M08~M15 , 2->M16~M23
        /// X点地址0 -> X0~X7 , 1 -> X10~X17 , 2->X20~X27
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="parm"></param>
        /// <returns></returns>
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                //写入数据
                if (value == null || value.Count <= 0)
                {
                    All.Class.Error.Add("写入数据为空,不能写入空数据", Environment.StackTrace);
                    return false;
                }
                //单个boolean值写入
                All.Class.TypeUse.TypeList t = All.Class.TypeUse.GetType<T>();
                if (t == All.Class.TypeUse.TypeList.Boolean && ((value.Count % 8) != 0))
                {
                    return Write((bool)(object)value[0], parm);
                }
                //写入地址
                if (!parm.ContainsKey("Start") && !parm.ContainsKey("Address"))
                {
                    All.Class.Error.Add("数据写入参数中不包含寄存器地址", Environment.StackTrace);
                    return false;
                }
                ushort tmpAddress = 0;
                if (parm.ContainsKey("Start"))
                {
                    tmpAddress = parm["Start"].ToUshort();
                }
                if (parm.ContainsKey("Address"))
                {
                    tmpAddress = parm["Address"].ToUshort();
                }
                //写入区域
                List<byte> sendBuff = new List<byte>();//发送的总字符
                List<byte> sendValue = new List<byte>();//发送的数据区域
                if (!parm.ContainsKey("Code"))
                {
                    switch (t)
                    {
                        case Class.TypeUse.TypeList.Boolean:
                            parm.Add("Code", "M");
                            break;
                        case Class.TypeUse.TypeList.UShort:
                        case Class.TypeUse.TypeList.Byte:
                        case Class.TypeUse.TypeList.Int:
                        case Class.TypeUse.TypeList.String:
                        case Class.TypeUse.TypeList.Long:
                            parm.Add("Code", "D");
                            break;
                        default:
                            All.Class.Error.Add("数据写入类型不正确,须要先添加并测试", Environment.StackTrace);
                            break;
                    }
                }
                sendBuff.Add(0x02);
                sendBuff.Add(0x31);
                string tmpCode = parm["Code"];
                switch (tmpCode)
                {
                    case "D":
                        tmpAddress = (ushort)(0x1000 + 2 * tmpAddress);
                        break;
                    case "M":
                        tmpAddress = (ushort)(0x0100 + tmpAddress);
                        break;
                    case "X":
                        tmpAddress = (ushort)(0x0080 + Convert.ToUInt16(tmpAddress.ToString(), 8));
                        break;
                    case "Y":
                        tmpAddress = (ushort)(0x00A0 + Convert.ToUInt16(tmpAddress.ToString(), 8));
                        break;
                    case "S":
                    case "T":
                    case "C":
                        All.Class.Error.Add("数据写入指令正确,但当前驱动没有添加实现此区域操作,请添加并测试", Environment.StackTrace);
                        break;
                    default:
                        All.Class.Error.Add("数据写入指令不正确,当前驱动没有指定的指令", Environment.StackTrace);
                        return false;
                }
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X4}", (ushort)(tmpAddress & 0xFFFF))));
                switch (tmpCode)
                {
                    case "D":
                        switch (t)
                        {
                            case Class.TypeUse.TypeList.String:
                                //下面的字符串字节与高低交换,如果不须要,则改
                                sendValue.AddRange(All.Class.Num.SwitchHighAndLow(Encoding.ASCII.GetBytes((string)(object)value[0])));
                                break;
                            case Class.TypeUse.TypeList.Byte:
                                for (int i = 0; i < value.Count; i++)
                                {
                                    sendValue.Add((byte)(object)value[i]);
                                }
                                break;
                            case Class.TypeUse.TypeList.Int:
                                ushort tmpInt = 0;
                                for (int i = 0; i < value.Count; i++)
                                {
                                    tmpInt = (ushort)(((int)(object)value[i]) & 0xFFFF);
                                    sendValue.AddRange(new byte[] { (byte)(tmpInt & 0xFF), (byte)(tmpInt >> 8) });
                                }
                                break;
                            case Class.TypeUse.TypeList.UShort:
                                ushort tmpUshort = 0;
                                for (int i = 0; i < value.Count; i++)
                                {
                                    tmpUshort = (ushort)(((ushort)(object)value[i]) & 0xFFFF);
                                    sendValue.AddRange(new byte[] { (byte)(tmpUshort & 0xFF), (byte)(tmpUshort >> 8) });
                                }
                                break;
                            case Class.TypeUse.TypeList.Long:
                                ushort tmpLong = 0;
                                for (int i = 0; i < value.Count; i++)
                                {
                                    tmpInt = (ushort)(((long)(object)value[i]) & 0xFFFF);
                                    sendValue.AddRange(new byte[] { (byte)(tmpLong & 0xFF), (byte)(tmpLong >> 8) });
                                }
                                break;
                        }
                        break;
                    case "M":
                    case "X":
                    case "Y":
                        bool[] tmpBool = new bool[8 * (int)(Math.Floor(value.Count / 8.0f))];
                        for (int i = 0; i < tmpBool.Length; i++)
                        {
                            tmpBool[i] = (bool)(object)value[i];
                        }
                        sendValue.AddRange(All.Class.Num.Bool2Byte(tmpBool));
                        break;
                }
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", sendValue.Count)));
                for (int i = 0; i < sendValue.Count; i++)
                {
                    sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", sendValue[i])));
                }
                sendBuff.Add(0x03);
                byte sumLow, sumHigh;
                byte[] readBuff;
                All.Class.Check.SumCheck(sendBuff.ToArray(), 1, sendBuff.Count - 1, out sumLow, out sumHigh);
                sendBuff.AddRange(Encoding.ASCII.GetBytes(string.Format("{0:X2}", sumLow)));
                if (WriteAndRead<byte[], byte[]>(sendBuff.ToArray(), 1, out readBuff))
                {
                    if (readBuff == null || readBuff.Length < 1 || readBuff[0] != 0x06)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
                return result;
            }
        }
    }
}
