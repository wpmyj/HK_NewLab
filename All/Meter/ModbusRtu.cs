using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace All.Meter
{
    public class ModbusRtu : Meter
    {
        Dictionary<string, string> initParm;

        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        byte address = 0;
        /// <summary>
        /// 模块通讯地址
        /// </summary>
        public byte Address
        {
            get { return address; }
            set { address = value; }
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
            if (!InitParm.ContainsKey("Address"))
            {
                All.Class.Error.Add("标准Modbus参数中没有地址", Environment.StackTrace);
            }
            else
            {
                address = All.Class.Num.ToByte(InitParm["Address"]);
            }
        }
        /// <summary>
        /// 按指令读取指定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">读取返回数据</param>
        /// <param name="parm">读取参数,必须包含Code,Address,Len三个参数</param>
        /// <returns></returns>
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                value = new List<T>();
                try
                {
                    int returnCount = 0;
                    byte[] sendBuff = new byte[8];
                    byte crcLo = 0;
                    byte crcHi = 0;
                    sendBuff[0] = address;

                    if (!parm.ContainsKey("Code"))
                    {
                        sendBuff[1] = 0x03;
                    }
                    else
                    {
                        byte tmpCode = All.Class.Num.ToByte(parm["Code"]);
                        switch (tmpCode)
                        {
                            case 1:
                            case 3:
                                sendBuff[1] = tmpCode;
                                break;
                            default:
                                All.Class.Error.Add("数据读取指令不正确,须要先测试", Environment.StackTrace);
                                return false;
                        }
                    }
                    if (!parm.ContainsKey("Start") && !parm.ContainsKey("Address"))
                    {
                        All.Class.Error.Add("数据读取参数中不包含寄存器地址", Environment.StackTrace);
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
                    sendBuff[2] = (byte)(tmpAddress >> 8);
                    sendBuff[3] = (byte)(tmpAddress & 0xFF);


                    if (!parm.ContainsKey("End"))
                    {
                        All.Class.Error.Add("数据读取参数中不包含读取结束", Environment.StackTrace);
                        return false;
                    }
                    ushort tmpLen = (ushort)(All.Class.Num.ToUshort(parm["End"]) - tmpAddress + 1);
                    sendBuff[4] = (byte)(tmpLen >> 8);
                    sendBuff[5] = (byte)(tmpLen & 0xFF);
                    switch (sendBuff[1])
                    {
                        case 1:
                            returnCount = (ushort)Math.Ceiling(tmpLen / 8.0f) + 5;
                            break;
                        case 3:
                            returnCount = tmpLen * 2 + 5;
                            break;
                    }


                    All.Class.Check.Crc16(sendBuff, 6, out crcLo, out crcHi);
                    sendBuff[6] = crcLo;
                    sendBuff[7] = crcHi;
                    byte[] readBuff;
                    if (WriteAndRead<byte[], byte[]>(sendBuff, returnCount, out readBuff))
                    {
                        if (sendBuff[0] != readBuff[0])
                        {
                            All.Class.Error.Add("返回数据地址与读取地址不一致", Environment.StackTrace);
                            return false;
                        }
                        if (sendBuff[1] != readBuff[1])
                        {
                            All.Class.Error.Add("返回数据指令码与读取指令码不一致", Environment.StackTrace);
                            return false;
                        }
                        All.Class.Check.Crc16(readBuff, returnCount - 2, out crcLo, out crcHi);
                        if (readBuff[returnCount - 2] != crcLo || readBuff[returnCount - 1] != crcHi)
                        {
                            All.Class.Error.Add("返回数据CRC校验失败", Environment.StackTrace);
                            return false;
                        }
                        switch (Class.TypeUse.GetType<T>())
                        {
                            case All.Class.TypeUse.TypeList.Boolean:
                                switch (readBuff[1])
                                {
                                    case 1:
                                        bool[] tmpBuff = All.Class.Num.Byte2Bool(readBuff, 3, returnCount - 5);
                                        for (int i = 0; i < tmpLen && i < tmpBuff.Length; i++)
                                        {
                                            value.Add((T)(object)tmpBuff[i]);
                                        }
                                        break;
                                    default:
                                        All.Class.Error.Add("当前使用的读取指令不能将数据转化bool");
                                        result = false;
                                        break;
                                }
                                break;
                            case All.Class.TypeUse.TypeList.UShort:
                                switch (readBuff[1])
                                {
                                    case 3:
                                        for (int i = 3; i < returnCount - 2; i = i + 2)
                                        {
                                            value.Add((T)(object)(ushort)(readBuff[i] * 256 + readBuff[i + 1]));
                                        }
                                        break;
                                    default:
                                        All.Class.Error.Add("当前使用的读取指令不能将数据转化Ushort");
                                        result = false;
                                        break;
                                }
                                break;
                            case All.Class.TypeUse.TypeList.Byte:
                                for (int i = 3; i < returnCount - 2; i++)
                                {
                                    value.Add((T)(object)readBuff[i]);
                                }
                                break;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
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
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    byte[] sendBuff = new byte[1];
                    byte crcLo = 0;
                    byte crcHi = 0;
                    int returnCount = 8;
                    if (!parm.ContainsKey("Code"))
                    {
                        parm.Add("Code", "16");
                    }
                    else
                    {
                        byte tmpCode = All.Class.Num.ToByte(parm["Code"]);
                        switch (tmpCode)
                        {
                            case 5:
                            case 6:
                                sendBuff = new byte[8];
                                break;
                            case 16:
                                sendBuff = new byte[value.Count * 2 + 9];
                                break;
                            default:
                                All.Class.Error.Add("数据读取指令不正确,须要先测试", Environment.StackTrace);
                                return false;
                        }
                        sendBuff[0] = address;
                        sendBuff[1] = tmpCode;
                    }
                    if (!parm.ContainsKey("Start") && !parm.ContainsKey("Address"))
                    {
                        All.Class.Error.Add("数据读取参数中不包含寄存器地址", Environment.StackTrace);
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
                    sendBuff[2] = (byte)(tmpAddress >> 8);
                    sendBuff[3] = (byte)(tmpAddress & 0xFF);
                    switch (sendBuff[1])
                    {
                        case 5:
                            string tmpGetValue = value[0].ToString().ToUpper();
                            int tmpSendValue = -1;
                            if (tmpGetValue == "1" || tmpGetValue == "TRUE" || tmpGetValue == "是")
                            {
                                tmpSendValue = 1;
                            }
                            if (tmpGetValue == "0" || tmpGetValue == "FALSE" || tmpGetValue == "否")
                            {
                                tmpSendValue = 0;
                            }
                            switch (tmpSendValue)
                            {
                                case -1:
                                    All.Class.Error.Add("写入的数据在当前指令下为不可识别数据", Environment.StackTrace);
                                    return false;
                                case 0:
                                    sendBuff[4] = 0x00;
                                    sendBuff[5] = 0x00;
                                    break;
                                case 1:
                                    sendBuff[4] = 0xFF;
                                    sendBuff[5] = 0x00;
                                    break;
                            }
                            break;
                        case 6:
                            if (Class.TypeUse.GetType<T>() != All.Class.TypeUse.TypeList.UShort)
                            {
                                All.Class.Error.Add("ModBus写入的数据类型不正确,ModBus批量数据只能为Ushort类型", Environment.StackTrace);
                                return false;
                            }
                            sendBuff[4] = (byte)(All.Class.Num.ToUshort(value[0].ToString()) >> 8);
                            sendBuff[5] = (byte)(All.Class.Num.ToUshort(value[0].ToString()) & 0xFF);
                            break;
                        case 16:
                            if (Class.TypeUse.GetType<T>() != All.Class.TypeUse.TypeList.UShort)
                            {
                                All.Class.Error.Add("ModBus写入的数据类型不正确,ModBus批量数据只能为Ushort类型", Environment.StackTrace);
                                return false;
                            }
                            sendBuff[4] = (byte)(value.Count >> 8);
                            sendBuff[5] = (byte)(value.Count & 0xFF);
                            sendBuff[6] = (byte)(value.Count * 2);
                            for (int i = 0; i < value.Count; i++)
                            {
                                sendBuff[7 + i * 2] = (byte)(((ushort)(object)value[i]) >> 8);
                                sendBuff[8 + i * 2] = (byte)(((ushort)(object)value[i]) & 0xFF);
                            }
                            break;
                        default:
                            All.Class.Error.Add("数据读取指令不正确,须要先测试", Environment.StackTrace);
                            return false;
                    }
                    All.Class.Check.Crc16(sendBuff, sendBuff.Length - 2, out crcLo, out crcHi);
                    sendBuff[sendBuff.Length - 2] = crcLo;
                    sendBuff[sendBuff.Length - 1] = crcHi;
                    byte[] readBuff;
                    if (WriteAndRead<byte[], byte[]>(sendBuff, returnCount, out readBuff))
                    {
                        if (sendBuff[0] != readBuff[0])
                        {
                            All.Class.Error.Add("返回数据地址与读取地址不一致", Environment.StackTrace);
                            return false;
                        }
                        if (sendBuff[1] != readBuff[1])
                        {
                            All.Class.Error.Add("返回数据指令码与读取指令码不一致", Environment.StackTrace);
                            return false;
                        }
                        All.Class.Check.Crc16(readBuff, returnCount - 2, out crcLo, out crcHi);
                        if (readBuff[returnCount - 2] != crcLo || readBuff[returnCount - 1] != crcHi)
                        {
                            All.Class.Error.Add("返回数据CRC校验失败", Environment.StackTrace);
                            return false;
                        }
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                    result = false;
                }
                return result;
            }
        }

    }
}
