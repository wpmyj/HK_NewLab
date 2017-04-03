using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class McgsModbusRtu : Meter
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
        byte address = 0;
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
            if (!InitParm.ContainsKey("Address"))
            {
                All.Class.Error.Add("标准Modbus参数中没有地址", Environment.StackTrace);
            }
            else
            {
                address = All.Class.Num.ToByte(InitParm["Address"]);
            }
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            bool result = true;
            value = new List<T>();
            int start = 0;
            int end = 0;
            byte[] sendBuff = new byte[12];
            int readLen = 0;
            if (!parm.ContainsKey("Start"))
            {
                All.Class.Error.Add(string.Format("{0}:读取数据不包含起始点", this.Text), Environment.StackTrace);
                return false;
            }
            start = All.Class.Num.ToInt(parm["Start"]);
            if (start < 1)
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
            lock (lockObject)
            {
                switch (All.Class.TypeUse.GetType<T>())
                {
                    case Class.TypeUse.TypeList.String:
                        if (end > start)
                        {
                            All.Class.Error.Add(string.Format("{0}:MCGS一次只能读取一个字符串,开始地址,{1},结束地址,{2}", this.Text, start, end));
                            return false;
                        }
                        break;
                    case Class.TypeUse.TypeList.UShort:
                    case Class.TypeUse.TypeList.Float:
                        break;
                    case Class.TypeUse.TypeList.Boolean:
                        break;
                    default:
                        All.Class.Error.Add("McgsModbusRtu不支持当前的数据类型读取");
                        return false;
                }
                sendBuff[0] = 0x00;
                sendBuff[1] = 0x00;
                sendBuff[2] = 0x00;
                sendBuff[3] = 0x00;
                sendBuff[4] = 0x00;
                sendBuff[5] = 0x06;
                sendBuff[6] = address;
                sendBuff[8] = (byte)(((start - 1) >> 8) & 0xFF);
                sendBuff[9] = (byte)(((start - 1) >> 0) & 0xFF);
                switch (All.Class.TypeUse.GetType<T>())
                {
                    case Class.TypeUse.TypeList.UShort:
                        sendBuff[7] = 0x03;
                        sendBuff[10] = (byte)(((end - start + 1) >> 8) & 0xFF);
                        sendBuff[11] = (byte)(((end - start + 1) >> 0) & 0xFF);
                        readLen = 9 + (end - start + 1) * 2;
                        break;
                    case Class.TypeUse.TypeList.Float:
                        sendBuff[7] = 0x03;
                        sendBuff[10] = (byte)(((end - start + 1) >> 8) & 0xFF);
                        sendBuff[11] = (byte)(((end - start + 1) >> 0) & 0xFF);
                        readLen = 9 + (end - start + 1) * 2;
                        break;
                    case Class.TypeUse.TypeList.Boolean:
                        sendBuff[7] = 0x01;
                        sendBuff[10] = (byte)((((int)(Math.Ceiling((end - start + 1) / 16.0f) * 16.0f)) >> 8) & 0xFF);
                        sendBuff[11] = (byte)((((int)(Math.Ceiling((end - start + 1) / 16.0f) * 16.0f)) >> 0) & 0xFF);
                        readLen = 9 + (int)(Math.Ceiling((end - start + 1) / 16.0f)) * 2;
                        break;
                    case Class.TypeUse.TypeList.String://字符串一次只能读一个
                        sendBuff[7] = 0x03;
                        sendBuff[10] = 0x00;
                        sendBuff[11] = 0x01;
                        readLen = 17;
                        break;
                }
                byte[] readBuff;
                if (WriteAndRead<byte[], byte[]>(sendBuff, readLen, out readBuff))
                {
                    if (sendBuff[0] == readBuff[0] && sendBuff[1] == readBuff[1] && sendBuff[2] == readBuff[2] && sendBuff[3] == readBuff[3] &&
                        sendBuff[4] == readBuff[4] && (readBuff[5] + 6) == readBuff.Length && readBuff[6] == sendBuff[6] && readBuff[7] == sendBuff[7])
                    {
                        switch (Class.TypeUse.GetType<T>())
                        {
                            case Class.TypeUse.TypeList.UShort:
                                for (int i = 0, j = 9; i < (end - start + 1) && j < readBuff.Length; i++, j = j + 2)
                                {
                                    value.Add((T)(object)(ushort)(readBuff[j] * 0x100 + readBuff[j + 1]));
                                }
                                break;
                            case Class.TypeUse.TypeList.Float:
                                for (int i = 0, j = 9; i < (end - start + 1) && j < readBuff.Length; i++, j = j + 4)
                                {
                                    value.Add((T)(object)All.Class.Num.ByteToFloat(readBuff, j, All.Class.Num.QueueList.四三二一));
                                }
                                break;
                            case Class.TypeUse.TypeList.String:
                                string tmpStr = Encoding.GetEncoding("GB2312").GetString(readBuff, 9, readBuff.Length - 9);
                                if (tmpStr.IndexOf("MCGSSTR:") == 0)
                                {
                                    tmpStr = tmpStr.Substring(8);
                                }
                                value.Add((T)(object)tmpStr);
                                break;
                            case Class.TypeUse.TypeList.Boolean:
                                bool[] tmpBool = All.Class.Num.Byte2Bool(All.Class.Num.GetByte(readBuff, 9, readLen - 9));
                                for (int i = start; i <= end; i++)
                                {
                                    value.Add((T)(object)tmpBool[i - start]);
                                }
                                break;
                            default:
                                All.Class.Error.Add("McgsModbusRtu不支持当前的数据类型读取");
                                return false;
                        }
                    }
                    else
                    {
                        All.Class.Error.Add(string.Format("{0}:读取数据校验错误", this.Text), Environment.StackTrace);
                        return false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        public override bool Test()
        {
            ushort value = 0;
            return Read<ushort>(out value, 0);
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            bool result = true;
            if (value == null || value.Count <= 0)
            {
                return false;
            }
            int start = 0;
            //int end = 0;
            if (!parm.ContainsKey("Start"))
            {
                All.Class.Error.Add(string.Format("{0}:读取数据不包含起始点", this.Text), Environment.StackTrace);
                return false;
            }
            start = All.Class.Num.ToInt(parm["Start"]);
            //if (!parm.ContainsKey("End"))
            //{
            //    All.Class.Error.Add(string.Format("{0}:读取数据不包含结束点", this.Text), Environment.StackTrace);
            //    return false;
            //}
            //end = All.Class.Num.ToInt(parm["End"]);
            lock (lockObject)
            {
                byte[] sendBuff = new byte[0];
                byte[] strBuff = new byte[0];
                int readLen = 0;
                switch (Class.TypeUse.GetType<T>())
                {
                    case Class.TypeUse.TypeList.UShort:
                        sendBuff = new byte[13 + value.Count * 2];
                        sendBuff[7] = 0x10;
                        sendBuff[11] = (byte)(((sendBuff.Length - 13) / 2) & 0xFF);
                        sendBuff[12] = (byte)(((sendBuff.Length - 13) / 1) & 0xFF);
                        break;
                    case Class.TypeUse.TypeList.Float:
                        sendBuff = new byte[13 + value.Count * 4];
                        sendBuff[7] = 0x10;
                        sendBuff[11] = (byte)(((sendBuff.Length - 13) / 2) & 0xFF);
                        sendBuff[12] = (byte)(((sendBuff.Length - 13) / 1) & 0xFF);
                        break;
                    case Class.TypeUse.TypeList.String:
                        strBuff = Encoding.GetEncoding("GB2312").GetBytes((string)(object)value[0]);
                        if ((strBuff.Length % 2) == 1)
                        {
                            List<byte> tmpList = strBuff.ToList();
                            tmpList.Add(0);
                            strBuff = tmpList.ToArray();
                        }
                        sendBuff = new byte[13 + strBuff.Length];
                        sendBuff[7] = 0x10;
                        sendBuff[11] = (byte)(((sendBuff.Length - 13) / 2) & 0xFF);
                        sendBuff[12] = (byte)(((sendBuff.Length - 13) / 1) & 0xFF);
                        break;
                    case Class.TypeUse.TypeList.Boolean:
                        sendBuff = new byte[13 + (int)(Math.Ceiling(value.Count / 8.0f))];
                        sendBuff[7] = 0x0F;
                        sendBuff[11] = (byte)(value.Count);
                        sendBuff[12] = (byte)(sendBuff.Length - 13);
                        break;
                    default:
                        All.Class.Error.Add("McgsModbusRtu不支持当前的数据类型写入");
                        return false;
                }
                sendBuff[0] = 0x00;
                sendBuff[1] = 0x00;
                sendBuff[2] = 0x00;
                sendBuff[3] = 0x00;
                sendBuff[4] = 0x00;
                sendBuff[5] = (byte)(sendBuff.Length - 6);
                sendBuff[6] = address;
                sendBuff[8] = (byte)(((start - 1) >> 8) & 0xFF);
                sendBuff[9] = (byte)(((start - 1) >> 0) & 0xFF);
                sendBuff[10] = 0x00;
                switch (Class.TypeUse.GetType<T>())
                {
                    case Class.TypeUse.TypeList.UShort:
                        for (int i = 0, j = 13; i < value.Count && j < sendBuff.Length; i++, j = j + 2)
                        {
                            sendBuff[j] = (byte)(((ushort)(object)value[i] >> 8) & 0xFF);
                            sendBuff[j + 1] = (byte)(((ushort)(object)value[i] >> 0) & 0xFF);
                        }
                        readLen = 12;
                        break;
                    case Class.TypeUse.TypeList.String:
                        Array.Copy(strBuff, 0, sendBuff, 13, strBuff.Length);
                        readLen = 12;
                        break;
                    case Class.TypeUse.TypeList.Float:
                        for (int i = 0, j = 13; i < value.Count && j < sendBuff.Length; i++, j = j + 4)
                        {
                            Array.Copy(All.Class.Num.FloatToByte((float)(object)value[i], Class.Num.QueueList.四三二一), 0, sendBuff, j, 4);
                        }
                        readLen = 12;
                        break;
                    case Class.TypeUse.TypeList.Boolean:
                        int tmpLen = (int)(Math.Ceiling(value.Count / 8.0f));
                        List<bool> tmpBool = new List<bool>();
                        for (int i = 0; i < value.Count; i++)
                        {
                            tmpBool.Add((bool)(object)value[i]);
                        }
                        for (int i = value.Count; i < tmpLen * 8; i++)
                        {
                            tmpBool.Add(false);
                        }//将数据补齐为8的整数个，方便转为字节
                        Array.Copy(All.Class.Num.Bool2Byte(tmpBool.ToArray()), 0, sendBuff, 13, sendBuff.Length - 13);
                        readLen = 12;
                        break;
                }
                byte[] readBuff;
                if (WriteAndRead<byte[], byte[]>(sendBuff, readLen, out readBuff))
                {
                    if (sendBuff[0] == readBuff[0] && sendBuff[1] == readBuff[1] && sendBuff[2] == readBuff[2] && sendBuff[3] == readBuff[3] &&
                        sendBuff[4] == readBuff[4] && (readBuff[5] + 6) == readBuff.Length && readBuff[6] == sendBuff[6] && readBuff[7] == sendBuff[7])
                    {
                        //写入OK
                    }
                    else
                    {
                        All.Class.Error.Add(string.Format("{0}:读取数据校验错误", this.Text), Environment.StackTrace);
                        return false;
                    }
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
