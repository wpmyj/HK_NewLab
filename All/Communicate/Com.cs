using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
namespace All.Communicate
{
    public class Com:Communicate
    {
        SerialPort serialPort;

        public SerialPort SerialPort
        {
            get { return serialPort; }
            set { serialPort = value; }
        }
        public override bool IsOpen
        {
            get
            {
                if (serialPort == null)
                {
                    return false;
                }
                return serialPort.IsOpen;
            }
        }
        public override int DataRecive
        {
            get
            {
                if (serialPort == null)
                {
                    return 0;
                }
                return serialPort.BytesToRead;
            }
        }
        public override void Init(Dictionary<string, string> buff)
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort();
            }
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            if (buff.ContainsKey("Text"))
            {
                this.Text = buff["Text"];
            }
            if (buff.ContainsKey("FlushTick"))
            {
                this.FlushTick = buff["FlushTick"].ToInt();
            }
            if (buff.ContainsKey("PortName") && buff["PortName"] != null)
            {
                serialPort.PortName = buff["PortName"];
            }
            InitCommunite(buff);
        }
        
        public override void Open()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    serialPort.DataReceived += serialPort_DataReceived;
                }
            }
            catch(Exception e)
            {
                All.Class.Error.Add("打开字符", string.Format("{0}:{1},{2},{3},{4}",
                    serialPort.PortName, serialPort.BaudRate, serialPort.Parity, serialPort.DataBits, serialPort.StopBits));
                AddError(e);
            }
        }
        public override void Close()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.DataReceived -= serialPort_DataReceived;
                    serialPort.Close();
                }
            }
            catch (Exception e)
            {
                AddError(e);
            }
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            OnGetArgs(this, new ReciveArgs(null, 0));
        }
        protected override void OnGetArgs(object sender, Base.Base.ReciveArgs reciveArgs)
        {
            base.OnGetArgs(sender, reciveArgs);
        }
        public override void Read<T>(out T value)
        {
            value = default(T);
            if (serialPort == null || !serialPort.IsOpen)
            {
                AddError(new Exception( string.Format("{0}:Com.Read Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            int readLen = DataRecive;
            byte[] buff = new byte[readLen];
            serialPort.Read(buff, 0, readLen);

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
                AddError( new Exception(string.Format("{0}:Com.Read Error,Typeof(T) must be string or byte[]", this.Text)));
            }
        }
        public override void Send<T>(T value)
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                AddError(new Exception(string.Format("{0}:Com.Send Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            serialPort.DiscardInBuffer();
            if (typeof(T) == typeof(byte[]))
            {
                byte[] buff = (byte[])(object)value;
                serialPort.Write(buff, 0, buff.Length);
            }
            else if (typeof(T) == typeof(string))
            {
                string buff = (string)(object)value;
                serialPort.Write(buff);
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
        /// <summary>
        /// 对串口进行各种参数设置
        /// </summary>
        /// <param name="buff"></param>
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
                serialPort.BaudRate = buff["BaudRate"].ToInt();
            }
            if (buff.ContainsKey("Parity"))
            {
                switch (buff["Parity"].ToUpper())
                {
                    case "N":
                    case "NONE":
                    case "0":
                        serialPort.Parity = Parity.None;
                        break;
                    case "EVEN":
                    case "2":
                    case "E":
                        serialPort.Parity = Parity.Even;
                        break;
                    case "ODD":
                    case "1":
                    case "O":
                        serialPort.Parity = Parity.Odd;
                        break;
                }
            }
            if (buff.ContainsKey("DataBits"))
            {
                serialPort.DataBits = buff["DataBits"].ToInt();
            }
            if (buff.ContainsKey("StopBits"))
            {
                switch (buff["StopBits"].ToUpper())
                {
                    case "1":
                    case "ONE":
                        serialPort.StopBits = StopBits.One;
                        break;
                    case "2":
                    case "TWO":
                        serialPort.StopBits = StopBits.Two;
                        break;
                }
            }
            if (buff.ContainsKey("RtsEnable"))
            {
                switch (buff["RtsEnable"].ToUpper())
                {
                    case "0":
                    case "FALSE":
                        serialPort.RtsEnable = false;
                        break;
                    case "1":
                    case "TRUE":
                        serialPort.RtsEnable = true;
                        break;
                }
            }
            if (buff.ContainsKey("DtrEnable"))
            {
                switch (buff["DtrEnable"].ToUpper())
                {
                    case "0":
                    case "FALSE":
                        serialPort.DtrEnable = false;
                        break;
                    case "1":
                    case "TRUE":
                        serialPort.DtrEnable = true;
                        break;
                }
            }
        }
    }
}
