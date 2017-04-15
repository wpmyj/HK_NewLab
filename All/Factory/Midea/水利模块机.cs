using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
namespace All.Factory.Midea
{
    public class 水利模块机:Factory
    {
        /// <summary>
        /// 设置当前机器
        /// </summary>
        public enum SetMachineList
        {
            普通模块机,
            水利模块机
        }
        /// <summary>
        /// 当前机型运行模式
        /// </summary>
        public enum ModeList : ushort
        {
            待机 = 0,
            制冷 = 2,
            制热 = 3,
            强制制冷 = 4,
            制热水 = 5,
            未知 = 65535
        }
        /// <summary>
        /// 当前机器类型
        /// </summary>
        public enum MachineList : ushort
        {
            bdr = 1,
            M_thermal,
            Monobloc,
            未知 = 65535
        }
        #region//属性
        public override string Info
        {
            get { return "水利模块线控器通讯协议,水机Modbus通讯协议,版本V1.1.1,时间2016-01-05"; }
        }
        Communicate.Communicate com;
        /// <summary>
        /// 通讯类
        /// </summary>
        public override Communicate.Communicate Com
        {
            get { return com; }
            set { com = value; }
        }
        /// <summary>
        /// 频率
        /// </summary>
        public ushort Hz
        { get { return buff[0]; } }
        /// <summary>
        /// 当前模式
        /// </summary>
        public ModeList Mode
        {
            get
            {
                if (buff[1] == 0x00 || buff[1] == 0x02 || buff[1] == 0x03 || buff[1] == 0x04 || buff[1] == 0x05)
                {
                    return (ModeList)buff[1];
                }
                return ModeList.未知;
            }
        }
        /// <summary>
        /// 风档,0-15代表0到15档风
        /// </summary>
        public ushort Speed
        { get { return buff[2]; } }
        /// <summary>
        /// 冷凝温度
        /// </summary>
        public ushort T3
        { get { return buff[6]; } }
        /// <summary>
        /// 环境温度
        /// </summary>
        public ushort T4
        { get { return buff[7]; } }
        /// <summary>
        /// 排气温度
        /// </summary>
        public ushort TExhaust
        { get { return buff[8]; } }
        /// <summary>
        /// 进水温度
        /// </summary>
        public ushort Tin
        { get { return buff[9]; } }
        /// <summary>
        /// 出水温度
        /// </summary>
        public ushort Tout
        { get { return buff[10]; } }
        public ushort Tb1
        { get { return buff[11]; } }
        public ushort Tb2
        { get { return buff[12]; } }
        /// <summary>
        /// 备用散热片温度
        /// </summary>
        public ushort TBack
        { get { return buff[13]; } }
        /// <summary>
        /// 故障代码
        /// </summary>
        public ushort ErrorCode
        { get { return buff[14]; } }
        /// <summary>
        /// 电流
        /// </summary>
        public ushort Cur
        { get { return buff[15]; } }
        /// <summary>
        /// 电压 
        /// </summary>
        public ushort Vol
        { get { return buff[16]; } }
        /// <summary>
        /// 机型能力值
        /// </summary>
        public ushort Power
        { get { return buff[18]; } }
        /// <summary>
        /// 版本号
        /// </summary>
        public ushort Code
        { get { return buff[19]; } }
        /// <summary>
        /// 当前故障
        /// </summary>
        public ushort CurrentError
        { get { return buff[20]; } }
        /// <summary>
        /// 故障代码1
        /// </summary>
        public ushort Error1
        { get { return buff[21]; } }
        /// <summary>
        /// 故障代码2
        /// </summary>
        public ushort Error2
        { get { return buff[22]; } }
        /// <summary>
        /// 故障代码3
        /// </summary>
        public ushort Error3
        { get { return buff[23]; } }
        /// <summary>
        /// 状态位1
        /// </summary>
        public ushort Statue1
        { get { return buff[24]; } }
        /// <summary>
        /// 状态2
        /// </summary>
        public ushort Statue2
        { get { return buff[25]; } }
        /// <summary>
        /// 总出水温度
        /// </summary>
        public ushort T1
        { get { return buff[27]; } }
        /// <summary>
        /// 系统总出水温度
        /// </summary>
        public ushort T1b
        { get { return buff[28]; } }
        /// <summary>
        /// 冷媒液侧温度
        /// </summary>
        public ushort T2
        { get { return buff[29]; } }
        /// <summary>
        /// 冷媒气侧温度
        /// </summary>
        public ushort T2b
        { get { return buff[30]; } }
        /// <summary>
        /// 水箱温度
        /// </summary>
        public ushort T5
        { get { return buff[31]; } }
        /// <summary>
        /// 孩童温度
        /// </summary>
        public ushort Ta
        { get { return buff[32]; } }
        /// <summary>
        /// 实际电流1
        /// </summary>
        public float Cur1
        { get { return buff[33] / 10f; } }
        /// <summary>
        /// 实际电流2
        /// </summary>
        public float Cur2
        { get { return buff[34] / 10f; } }
        /// <summary>
        /// 匹数
        /// </summary>
        public float HorsePower
        { get { return buff[35] / 10f; } }
        /// <summary>
        /// 压力1
        /// </summary>
        public float Press1
        { get { return buff[36] / 100f; } }
        /// <summary>
        /// 压力2
        /// </summary>
        public float Press2
        { get { return buff[37] / 100f; } }
        /// <summary>
        /// 回气温度
        /// </summary>
        public ushort Trecover
        { get { return buff[38]; } }
        /// <summary>
        /// 机型
        /// </summary>
        public MachineList Machine
        {
            get
            {
                if (buff[39] == 1 || buff[39] == 2 || buff[39] == 3)
                {
                    return (MachineList)buff[39];
                }
                return MachineList.未知;
            }
        }
        public bool SIBH1
        { get { return ((buff[26] >> 0) & 1) == 1; } }
        public bool SIBH2
        { get { return ((buff[26] >> 1) & 1) == 1; } }
        public bool STBH
        { get { return ((buff[26] >> 2) & 1) == 1; } }
        /// <summary>
        /// 水泵
        /// </summary>
        public bool SPUMP
        { get { return ((buff[26] >> 3) & 1) == 1; } }
        public bool SSV1
        { get { return ((buff[26] >> 4) & 1) == 1; } }
        public bool SSV2
        { get { return ((buff[26] >> 5) & 1) == 1; } }
        public bool SPO
        { get { return ((buff[26] >> 6) & 1) == 1; } }
        public bool SPP
        { get { return ((buff[26] >> 7) & 1) == 1; } }
        public bool SPM
        { get { return ((buff[26] >> 8) & 1) == 1; } }
        public bool SSV3
        { get { return ((buff[26] >> 9) & 1) == 1; } }
        /// <summary>
        /// 电加热
        /// </summary>
        public bool SHeat4
        { get { return ((buff[26] >> 10) & 1) == 1; } }
        /// <summary>
        /// 
        /// </summary>
        public bool SPUMP2
        { get { return ((buff[26] >> 11) & 1) == 1; } }
        public bool SAlarm
        { get { return ((buff[26] >> 12) & 1) == 1; } }
        public bool SRun
        { get { return ((buff[26] >> 13) & 1) == 1; } }
        public bool SSTOVE
        { get { return ((buff[26] >> 14) & 1) == 1; } }
        public bool SDEFROST
        { get { return ((buff[26] >> 15) & 1) == 1; } }
        /// <summary>
        /// 所有通讯字节
        /// </summary>
        public ushort[] Buff
        { get { return buff; } }
        /// <summary>
        /// 是否进入在线测试模式
        /// </summary>
        public bool OnLineTest
        { get; set; }
        /// <summary>
        /// 当前机型
        /// </summary>
        public SetMachineList SetMachine
        { get; set; }
        #endregion
        #region//字段
        bool goalTest = false;//快检目标值
        bool exit = false;
        ushort[] buff = new ushort[40];
        string portName = "COM1";
        #endregion
        public 水利模块机(string portName)
        {
            this.portName = portName;
        }
        /// <summary>
        /// 打开通讯端口
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        public override bool Open()
        {
            this.buff.Initialize();
            this.OnLineTest = false;
            this.goalTest = true;
            this.exit = false;

            this.com = new All.Communicate.Com();
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("PortName", this.portName);
            buff.Add("BaudRate", "9600");
            buff.Add("Parity", "None");
            buff.Add("DataBits", "8");
            buff.Add("StopBits", "1");
            buff.Add("Text", "水利模块使用串口");
            com.Init(buff);
            this.com.Open();

            this.Meter = new All.Meter.ModbusRtu();
            this.Meter.Parent = this.com;
            buff.Clear();
            buff.Add("Address", "16");
            this.Meter.Init(buff);

            thRead = new Thread(() => Flush());
            thRead.IsBackground = true;
            thRead.Start();

            return this.Com.IsOpen;
        }
        public override void Close()
        {
            goalTest = false;
            SetTestStatue();
            exit = true;
            base.Close();
        }
        private void Flush()
        {
            List<ushort> tmp;
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("Start", "200");
            parm.Add("End", "239");
            while (!exit)
            {
                lock (lockObject)
                {
                    if (this.Meter != null)
                    {
                        SetTestStatue();
                        if (this.Meter.Read<ushort>(out tmp, parm))
                        {
                            Array.Copy(tmp.ToArray(), buff, 40);
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }
        /// <summary>
        /// 设置工厂模式
        /// </summary>
        private void SetTestStatue()
        {
            lock (lockObject)
            {
                if (this.Meter != null)
                {
                    List<ushort> tmp;
                    Dictionary<string, string> parm = new Dictionary<string, string>();
                    parm.Add("Start", "23");
                    parm.Add("End", "23");
                    switch (SetMachine)
                    {
                        case SetMachineList.普通模块机:
                            parm["Start"] = "5";
                            parm["End"] = "5";
                            break;
                        case SetMachineList.水利模块机:
                            parm["Start"] = "23";
                            parm["End"] = "23";
                            break;
                    }
                    if (this.Meter.Read<ushort>(out tmp, parm))
                    {
                        OnLineTest = (tmp[0] & 1) == 1;
                        if (this.OnLineTest != goalTest)
                        {
                            this.Meter.WriteInternal<ushort>((ushort)(goalTest ? 1 : 0), parm["Start"].ToInt());
                        }
                    }
                }
            }
        }
    }
}
