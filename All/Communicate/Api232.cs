using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace All.Communicate
{
    public class Api232 : Communicate
    {
        #region//API232
        [DllImport("Api232.dll")]
        public static extern int sio_ioctl(int port, int baud, int mode);
        [DllImport("Api232.dll")]
        public static extern int sio_getch(int port);
        [DllImport("Api232.dll")]
        public static extern int sio_read(int port, string buf, int length);
        [DllImport("Api232.dll")]
        public static extern int sio_putch(int port, int term);
        [DllImport("Api232.dll")]
        public static extern int sio_write(int port, string buf, int length);
        [DllImport("Api232.dll")]
        public static extern int sio_flush(int port, int func);
        [DllImport("Api232.dll")]
        public static extern int sio_iqueue(int port);
        [DllImport("Api232.dll")]
        public static extern int sio_oqueue(int port);
        [DllImport("Api232.dll")]
        public static extern int sio_close(int port);
        [DllImport("Api232.dll")]
        public static extern int sio_open(int port);
        [DllImport("Api232.dll")]
        public static extern int sio_DTR(int port, int mode);
        [DllImport("Api232.dll")]
        public static extern int sio_RTS(int port, int mode);
        [DllImport("Api232.dll")]
        public static extern int sio_baud(int port, int speed);
        [DllImport("Api232.dll")]
        public static extern int sio_getbaud(int port);
        [DllImport("Api232.dll")]
        public static extern int sio_getmode(int port);
        /*
        //波特率设置
        static int B50 = 0x0;
        static int B75 = 0x1;
        static int B110 = 0x2;
        static int B134 = 0x3;
        static int B150 = 0x4;
        static int B300 = 0x5;
        static int B600 = 0x6;
        static int B1200 = 0x7;
        static int B1800 = 0x8;
        static int B2400 = 0x9;
        static int B4800 = 0xA;
        static int B7200 = 0xB;
        static int B9600 = 0xC;
        static int B19200 = 0xD;
        static int B38400 = 0xE;
        static int B57600 = 0xF;
        static int B115200 = 0x10;
        static int B230400 = 0x11;
        static int B460800 = 0x12;
        static int B921600 = 0x13;
        //数据位
        static int BIT_5 = 0x0;
        static int BIT_6 = 0x1;
        static int BIT_7 = 0x2;
        static int BIT_8 = 0x3;

        //Stop bits define (停止位定义)
        static int STOP_1 = 0x0;
        static int STOP_2 = 0x4;

        //Parity define (奇偶效验位定义)
        static int P_EVEN = 0x18;
        static int P_ODD = 0x8;
        static int P_SPC = 0x38;
        static int P_MRK = 0x28;
        static int P_NONE = 0x0;

        //*********************** Modem Control Setting ****************************************
        static int C_DTR = 0x1;
        static int C_RTS = 0x2;

        //*********************** Modem Line Status *********************************************
        static int S_CTS = 0x1;
        static int S_DSR = 0x2;
        static int S_RI = 0x4;
        static int S_CD = 0x8;

        //*********************** Error Code *****************************************************
        static int SIO_OK = 0;
        static int SIO_BADPORT = -1;                              // no such port or port not opened
        static int SIO_OUTCONTROL = -2;                           // can't control AdvanTech board
        static int SIO_NODATA = -4;                               // no data to read or no buffer to write
        static int SIO_BADPARM = -7;                              // bad parameter
        static int SIO_WIN32FAIL = -8;                            // win32 function call fails, please call
        */
        #endregion
        int port = 1;
        int baud = 0x0C;
        int mode = 0x03;
        bool isOpen = false;
        public override bool IsOpen
        {
            get { return isOpen; }
        }
        public override int DataRecive
        {
            get {
                if (isOpen)
                {
                    return sio_iqueue(port);
                }
                return 0;
            }
        }
        public override void Init(Dictionary<string, string> buff)
        {
            if (buff.ContainsKey("PortName") && buff["PortName"] != null)
            {
                port = buff["PortName"].ToUpper().Replace("COM", "").Replace(":", "").ToInt();
            }
            if (buff.ContainsKey("Text"))
            {
                this.Text = buff["Text"];
            }
            if (buff.ContainsKey("FlushTick"))
            {
                this.FlushTick = buff["FlushTick"].ToInt();
            }
            InitCommunite(buff);
        }
        public override void InitCommunite(Dictionary<string, string> buff)
        {
            if (buff.ContainsKey("Setting"))
            {
                string[] tmp = buff["Setting"].Split(',');
                switch (tmp.Length)
                {
                    case 1:
                        if (!buff.ContainsKey("BaudRate"))
                            buff.Add("BaudRate", tmp[0]);
                        break;
                    case 2:
                        if (!buff.ContainsKey("BaudRate"))
                            buff.Add("BaudRate", tmp[0]);
                        if (!buff.ContainsKey("Parity"))
                            buff.Add("Parity", tmp[1]);
                        break;
                    case 3:
                        if (!buff.ContainsKey("BaudRate"))
                            buff.Add("BaudRate", tmp[0]);
                        if (!buff.ContainsKey("Parity"))
                            buff.Add("Parity", tmp[1]);
                        if (!buff.ContainsKey("DataBits"))
                            buff.Add("DataBits", tmp[2]);
                        break;
                    case 4:
                        if (!buff.ContainsKey("BaudRate"))
                            buff.Add("BaudRate", tmp[0]);
                        if (!buff.ContainsKey("Parity"))
                            buff.Add("Parity", tmp[1]);
                        if (!buff.ContainsKey("DataBits"))
                            buff.Add("DataBits", tmp[2]);
                        if (!buff.ContainsKey("StopBits"))
                            buff.Add("StopBits", tmp[3]);
                        break;
                }
            }
            if (buff.ContainsKey("BaudRate"))
            {
                switch (buff["BaudRate"].ToInt())
                {
                    case 50: baud = 0x0; break;
                    case 75: baud = 0x1; break;
                    case 110: baud = 0x2; break;
                    case 134: baud = 0x3; break;
                    case 150: baud = 0x4; break;
                    case 300: baud = 0x5; break;
                    case 600: baud = 0x6; break;
                    case 1200: baud = 0x7; break;
                    case 1800: baud = 0x8; break;
                    case 2400: baud = 0x9; break;
                    case 4800: baud = 0xA; break;
                    case 7200: baud = 0xB; break;
                    case 9600: baud = 0xC; break;
                    case 19200: baud = 0xD; break;
                    case 38400: baud = 0xE; break;
                    case 57600: baud = 0xF; break;
                    case 115200: baud = 0x10; break;
                    case 230400: baud = 0x11; break;
                    case 460800: baud = 0x12; break;
                    case 921600: baud = 0x13; break;
                    default:
                        All.Class.Error.Add(string.Format("当前使用的波特率不支持:{0}", buff["BaudRate"]), Environment.StackTrace);
                        return;
                }
            }
            else
            {
                if (isOpen)
                {
                    baud = sio_getbaud(port);
                }
            }
            if (isOpen)
            {
                mode = sio_getmode(port);
            }
            if (buff.ContainsKey("DataBits"))
            {
                mode = mode & 0xFC;
                switch (buff["DataBits"].ToInt())
                {
                    case 5:
                        mode += 0x00;
                        break;
                    case 6:
                        mode += 0x01;
                        break;
                    case 7:
                        mode += 0x02;
                        break;
                    case 8:
                    default:
                        mode += 0x03;
                        break;
                }
            }
            if (buff.ContainsKey("StopBits"))
            {
                mode = mode & 0xFB;
                switch (buff["StopBits"].ToUpper())
                {
                    case "1":
                    case "ONE":
                        mode += 0x00;
                        break;
                    case "2":
                    case "TWO":
                        mode += 0x04;
                        break;
                }
            }
            if (buff.ContainsKey("Parity"))
            {
                mode = mode & 0x07;
                switch (buff["Parity"].ToUpper())
                {
                    case "N":
                    case "NONE":
                    case "0":
                        mode += 0x00;
                        break;
                    case "EVEN":
                    case "2":
                    case "E":
                        mode += 0x18;
                        break;
                    case "ODD":
                    case "1":
                    case "O":
                        mode += 0x08;
                        break;
                }
            }
            if (buff.ContainsKey("RtsEnable"))
            {
                switch (buff["RtsEnable"].ToUpper())
                {
                    case "0":
                    case "FALSE":
                        //sio_DTR(port,
                        break;
                    case "1":
                    case "TRUE":
                        //serialPort.RtsEnable = true;
                        break;
                }
            }
            if (buff.ContainsKey("DtrEnable"))
            {
                switch (buff["DtrEnable"].ToUpper())
                {
                    case "0":
                    case "FALSE":
                        //serialPort.DtrEnable = false;
                        break;
                    case "1":
                    case "TRUE":
                        //serialPort.DtrEnable = true;
                        break;
                }
            }
        }
        private void Set()
        {
            switch (sio_ioctl(port, baud, mode))
            {
                case 0:
                    break;
                case -1:
                    All.Class.Error.Add("设置串口错误,no such port or port not opened");
                    break;
                case -2:
                    All.Class.Error.Add("设置串口错误,can't control AdvanTech board");
                    break;
                case -4:
                    All.Class.Error.Add("设置串口错误,no data to read or no buffer to write");
                    break;
                case -7:
                    All.Class.Error.Add("设置串口错误,bad parameter");
                    break;
                case -8:
                    All.Class.Error.Add("设置串口错误,win32 function call fails, please call");
                    break;
            }
        }
        public override void Open()
        {
            if (!isOpen && sio_open(port) == 0)
            {
                isOpen = true;
                Set();
            }
            else
            {
                All.Class.Error.Add(string.Format("打开串口:{0} 失败", port), Environment.StackTrace);
                return;
            }
        }
        public override void Close()
        {
            if (isOpen)
            {
                if (sio_close(port) == 0)
                {
                    isOpen = false;
                }
            }
        }
        public override void Read<T>(out T value)
        {
            value = default(T);
            if (!isOpen)
            {
                AddError(new Exception(string.Format("{0}:Com.Read Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            int readLen = DataRecive;
            byte[] buff = new byte[readLen];
            for (int i = 0; i < readLen; i++)
            {
                buff[i] = (byte)(0xFF & sio_getch(port));
            }

            if (typeof(T) == typeof(byte[]))
            {
                value = (T)(object)buff;
            }
            else if (typeof(T) == typeof(string))
            {
                value = (T)(object)Encoding.ASCII.GetString(buff, 0, readLen);
            }
            else
            {
                AddError(new Exception(string.Format("{0}:Com.Read Error,Typeof(T) must be string or byte[]", this.Text)));
            }
        }
        public override void Send<T>(T value)
        {
            if (!isOpen)
            {
                AddError(new Exception(string.Format("{0}:Com.Send Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            sio_flush(port, 2);
            if (typeof(T) == typeof(byte[]))
            {
                byte[] buff = (byte[])(object)value;
                for (int i = 0; i < buff.Length; i++)
                {
                    sio_putch(port, buff[i]);
                }
            }
            else if (typeof(T) == typeof(string))
            {
                string buff = (string)(object)value;
                sio_write(port, buff, buff.Length);
            }
            else
            {
                AddError(new Exception(string.Format("{0}:Com.Send Error,Typeof(T) must be string or byte[]", this.Text)));
            }
        }
        public override void Send<T>(T value, Dictionary<string, string> buff)
        {
            InitCommunite(buff);
            Send<T>(value);
        }
    }
}