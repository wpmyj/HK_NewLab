using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    public class An9651:AnGui.AnGui
    {
        public int Address
        { get; set; }
        Dictionary<string, string> initParm;
        public override Dictionary<string, string> InitParm
        {
            get { return initParm; }
            set { initParm = value; }
        }
        public override void Init(Dictionary<string, string> initParm)
        {
            this.Address = 15;
            this.InitParm = initParm;
            if (initParm.ContainsKey("Text"))
            {
                this.Text = initParm["Text"];
            }
            if (initParm.ContainsKey("TimeOut"))
            {
                this.TimeOut = initParm["TimeOut"].ToInt();
            }
            if (initParm.ContainsKey("ErrorCount"))
            {
                this.ErrorCount = initParm["ErrorCount"].ToInt();
            }
            if (initParm.ContainsKey("Address"))
            {
                this.Address = initParm["Address"].ToInt();
            }
        }
        public override bool Read<T>(out List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                value = new List<T>();
                try
                {
                    if (!parm.ContainsKey("Code"))
                    {
                        All.Class.Error.Add("数据读取参数中不包含命令", Environment.StackTrace);
                        return false;
                    }
                    string sendValue = string.Format("{0}{1:D3}000{2}", "{", Address, "}");
                    byte[] readBuff;
                    if (WriteAndRead<string, byte[]>(sendValue, 193, out readBuff))
                    {
                        List<byte> buff = readBuff.ToList();
                        for (int i = buff.Count - 1; i >= 0; i--)
                        {
                            if (buff[i] != 0x7D)
                            {
                                buff.RemoveAt(i);
                            }
                            else
                            {
                                break;
                            }
                        }
                        buff.Reverse();
                        for (int i = buff.Count - 1; i >= 0; i--)
                        {
                            if (buff[i] != 0x7B)
                            {
                                buff.RemoveAt(i);
                            }
                            else
                            {
                                break;
                            }
                        }
                        buff.Reverse();
                        readBuff = buff.ToArray();

                        if (sendValue.Substring(0, 4) != Encoding.ASCII.GetString(readBuff, 0, 4))
                        {
                            All.Class.Error.Add("返回数据校验失败", Environment.StackTrace);
                            All.Class.Error.Add("发送数据", sendValue);
                            All.Class.Error.Add("返回数据", Encoding.ASCII.GetString(readBuff));
                            return false;
                        }
                        switch (All.Class.TypeUse.GetType<T>())
                        {
                            case Class.TypeUse.TypeList.Float:
                            case Class.TypeUse.TypeList.Double:
                                for (int i = 5; i < readBuff.Length - 9; i = i + 23)
                                {
                                    switch (readBuff[i])//项目
                                    {
                                        case 0x31:
                                            value.Add((T)(object)(float)AnGui.AnGui.Projects.接地);
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 1, 4)) / 100f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 5, 4)) / 10f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 9, 4)) / 1f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 13, 4)) / 1f));
                                            break;
                                        case 0x32:
                                            value.Add((T)(object)(float)AnGui.AnGui.Projects.绝缘);
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 1, 4)) / 1f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 5, 4)) / 10f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 9, 4)) / 1f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 13, 4)) / 1f));
                                            break;
                                        case 0x33:
                                            value.Add((T)(object)(float)AnGui.AnGui.Projects.耐压);
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 1, 4)) / 1f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 5, 4)) / 100f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 9, 4)) / 1f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 13, 4)) / 1f));
                                            break;
                                        case 0x34:
                                            value.Add((T)(object)(float)AnGui.AnGui.Projects.泄漏);
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 1, 4)) / 10f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 5, 4)) / 1f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 9, 4)) / 1f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 13, 4)) / 1f));
                                            break;
                                        case 0x35:
                                            value.Add((T)(object)(float)AnGui.AnGui.Projects.功率);
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 1, 4)) / 10f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 5, 4)) / 100f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 9, 4)) / 10f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 13, 4)) / 1f));
                                            break;
                                        case 0x36:
                                            value.Add((T)(object)(float)AnGui.AnGui.Projects.启动);
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 1, 4)) / 10f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 5, 4)) / 100f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 9, 4)) / 100f));
                                            value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 13, 4)) / 1f));
                                            break;
                                        default://空白测试步骤,要补上
                                            value.Add((T)(object)(float)AnGui.AnGui.Projects.空白);
                                            value.Add((T)(object)0f);
                                            value.Add((T)(object)0f);
                                            value.Add((T)(object)0f);
                                            value.Add((T)(object)0f);
                                            break;
                                    }
                                    value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 17, 4)) / 10f));//时间
                                    value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 21, 1)) / 1f));//动静
                                    value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, i + 22, 1)) / 1f));//结果
                                }
                                //
                                value.Add((T)(object)(Format(Encoding.ASCII.GetString(readBuff, 189, 1)) == 2 ? 1f : 0f));//完成状态
                                bool testResult = true;
                                for (int i = 27; i < readBuff.Length - 9; i = i + 23)
                                {
                                    testResult = testResult && (readBuff[i] == 0x31 || readBuff[i] == 0x30);
                                }
                                value.Add((T)(object)(testResult ? 1f : 0f));//总结果
                                result = true;
                                break;
                            case Class.TypeUse.TypeList.Byte:
                                for (int i = 5; i < readBuff.Length - 9; i++)
                                {
                                    value.Add((T)(object)(readBuff[i]));
                                }
                                result = true;
                                break;
                            default:
                                All.Class.Error.Add("读取的数据类型不正确,不支持当前的数据类型", Environment.StackTrace);
                                return false;
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
                }
                return result;
            }
        }
        /// <summary>
        /// 格式化值,达到4位数时,值须要遵守艾诺的奇葩算法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Format(int value)
        {
            if (value <= 0)
            {
                return "0000";
            }
            if (value >= 10000)
            {
                return string.Format("{0}{1:D3}",
                    Encoding.ASCII.GetString(new byte[] { (byte)(0x30 + ((int)Math.Floor(value / 1000f) & 0x7F)) }),
                    value % 1000);
            }
            return string.Format("{0:D4}", value);
        }
        /// <summary>
        /// 格式化值,艾诺的奇葩算法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int Format(string value)
        {
            int result = 0;
            byte[] buff = Encoding.ASCII.GetBytes(value);
            for (int i = 0; i < buff.Length; i++)
            {
                result = result + (buff[i] - 0x30) * (int)Math.Pow(10, value.Length - i - 1);
            }
            return result;
        }
        public override bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm)
        {
            lock (lockObject)
            {
                bool result = true;
                try
                {
                    if (!parm.ContainsKey("Code"))
                    {
                        All.Class.Error.Add("数据写入参数中不包含命令", Environment.StackTrace);
                        return false;
                    }
                    bool goon = false;
                    if (parm.ContainsKey("Goon"))
                    {
                        goon = parm["Goon"].ToBool();
                    }
                    int group = 1;
                    if (parm.ContainsKey("Group"))
                    {
                        group = parm["Group"].ToInt();
                    }
                    string sendValue = "";
                    int len = 9;
                    switch (parm["Code"].ToUpper())
                    {
                        case "START":
                            sendValue = string.Format("{0}{1:D3}100{2}", "{", Address, "}");
                            break;
                        case "STOP":
                            sendValue = string.Format("{0}{1:D3}200{2}", "{", Address, "}");
                            break;
                        case "SETTING":
                            Type t = typeof(AnGui.AnGui.SendValue.StepValue);
                            int members = t.GetProperties().Count();
                            if (value == null || (value.Count % members) != 0)
                            {
                                All.Class.Error.Add("写入的数据量不正确,不能识别的数据", Environment.StackTrace);
                                return false;
                            }
                            sendValue = string.Format("{0}{1:D3}7", "{", Address);
                            int steps = 0;
                            for (int i = 0, j = 0; i < value.Count && j < 8; i = i + members, j++)
                            {
                                steps++;
                                switch ((AnGui.AnGui.Projects)(int)(object)value[i])//步骤
                                {
                                    case Projects.接地:
                                        sendValue += string.Format("1{0}{1}{2}{3}{4}",
                                            Format((int)(100 * (float)(object)value[i + 1])),//设置输出值
                                            Format((int)((float)(object)value[i + 2])),//最小值
                                            Format((int)((float)(object)value[i + 3])),//最大值
                                            Format((int)(10 * (float)(object)value[i + 4])),//测试时间
                                            (int)((float)(object)value[i + 5]));//静态或动态
                                        break;
                                    case Projects.绝缘:
                                        sendValue += string.Format("2{0}{1}{2}{3}{4}",
                                            Format((int)((float)(object)value[i + 1])),//设置输出值
                                            Format((int)(10 * (float)(object)value[i + 2])),//最小值
                                            Format((int)(10 * (float)(object)value[i + 3])),//最大值
                                            Format((int)(10 * (float)(object)value[i + 4])),//测试时间
                                            (int)((float)(object)value[i + 5]));//静态或动态
                                        break;
                                    case Projects.耐压:
                                        sendValue += string.Format("3{0}{1}{2}{3}{4}",
                                            Format((int)((float)(object)value[i + 1])),//设置输出值
                                            Format((int)(100 * (float)(object)value[i + 2])),//最小值
                                            Format((int)(100 * (float)(object)value[i + 3])),//最大值
                                            Format((int)(10 * (float)(object)value[i + 4])),//测试时间
                                            (int)((float)(object)value[i + 5]));//静态或动态
                                        break;
                                    case Projects.泄漏:
                                        sendValue += string.Format("4{0}{1}{2}{3}{4}",
                                            Format((int)(10 * (float)(object)value[i + 1])),//设置输出值
                                            Format((int)((float)(object)value[i + 2])),//最小值
                                            Format((int)((float)(object)value[i + 3])),//最大值
                                            Format((int)(10 * (float)(object)value[i + 4])),//测试时间
                                            (int)((float)(object)value[i + 5]));//静态或动态
                                        break;
                                    case Projects.功率:
                                        sendValue += string.Format("5{0}{1}{2}{3}{4}",
                                            Format((int)(10 * (float)(object)value[i + 1])),//设置输出值
                                            Format((int)(10 * (float)(object)value[i + 2])),//最小值
                                            Format((int)(10 * (float)(object)value[i + 3])),//最大值
                                            Format((int)(10 * (float)(object)value[i + 4])),//测试时间
                                            (int)((float)(object)value[i + 5]));//静态或动态
                                        break;
                                    case Projects.启动:
                                        sendValue += string.Format("6{0}{1}{2}{3}{4}",
                                            Format((int)(10 * (float)(object)value[i + 1])),//设置输出值
                                            Format((int)(100 * (float)(object)value[i + 2])),//最小值
                                            Format((int)(100 * (float)(object)value[i + 3])),//最大值
                                            Format((int)(10 * (float)(object)value[i + 4])),//测试时间
                                            (int)((float)(object)value[i + 5]));//静态或动态
                                        break;
                                    case Projects.直耐:
                                        sendValue += string.Format("7{0}{1}{2}{3}{4}",
                                            Format((int)((float)(object)value[i + 1])),//设置输出值
                                            Format((int)((float)(object)value[i + 2])),//最小值
                                            Format((int)((float)(object)value[i + 3])),//最大值
                                            Format((int)(10 * (float)(object)value[i + 4])),//测试时间
                                            (int)((float)(object)value[i + 5]));//静态或动态
                                        break;
                                    default:
                                        All.Class.Error.Add(string.Format("出现未知的测试序号,错误序号为{0}", (int)(object)value[i]), Environment.StackTrace);
                                        return false;
                                }
                            }
                            for (int i = steps; i < 8; i++)
                            {
                                sendValue += "000000000000000000";//填写空步骤
                            }
                            sendValue += string.Format("{0}0000000", goon ? 1 : 0);//是否继续
                            sendValue += "11111111";//是否补偿
                            sendValue += "0000000000000000000000000000000000000000";
                            sendValue += string.Format("{0}", group);
                            sendValue += "00}";
                            break;
                        default:
                            All.Class.Error.Add(string.Format("数据写入参数命令不正确,不能识别的指令,{0}", parm["Code"]), Environment.StackTrace);
                            break;
                    }
                    string readValue = "";
                    if (WriteAndRead<string, string>(sendValue, len, out readValue))
                    {
                        if (sendValue.Substring(0, 4) == readValue.Substring(0, 4))
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                }
                return result;
            }
        }
    }
}
