using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate
{
    public class Udp:Communicate
    {
        Base.Udp udpClient;
        /// <summary>
        /// UDP端
        /// </summary>
        public Base.Udp UdpClient
        {
            get { return udpClient; }
            set { udpClient = value; }
        }
        public override bool IsOpen
        {
            get
            {
                if (udpClient == null)
                {
                    return false;
                }
                return udpClient.IsOpen;
            }
        }
        public override int DataRecive
        {
            get
            {
                if (udpClient == null)
                {
                    return 0;
                }
                return udpClient.DataRecive;
            }
        }
        public override void Init(Dictionary<string, string> buff)
        {
            if (udpClient == null)
            {
                udpClient = new Base.Udp(3000);
            }
            if (udpClient.IsOpen)
            {
                udpClient.Close();
            }
            if (buff.ContainsKey("Text"))
            {
                this.Text = buff["Text"];
            }
            if (buff.ContainsKey("FlushTick"))
            {
                this.FlushTick = buff["FlushTick"].ToInt();
            }
            if (!buff.ContainsKey("LocalPort"))
            {
                AddError(new Exception(string.Format("{0}:Udp.Init Error,parm<buff> need LocalPort values", this.Text)));
                return;
            }
            udpClient.LocalPort = buff["LocalPort"].ToInt();
            InitCommunite(buff);
        }
        public override void Open()
        {
            try
            {
                if (!udpClient.IsOpen)
                {
                    udpClient.Open();
                    udpClient.GetArgs += OnGetArgs;
                }
            }
            catch (Exception e)
            {
                AddError(e);
            }
        }
        public override void Close()
        {
            try
            {
                if (udpClient.IsOpen)
                {
                    udpClient.GetArgs -= OnGetArgs;
                    udpClient.Close();
                }
            }
            catch (Exception e)
            {
                AddError(e);
            }
        }
        protected override void OnGetArgs(object sender, Base.Base.ReciveArgs reciveArgs)
        {
            base.OnGetArgs(sender, reciveArgs);
        }
        public override void Read<T>(out T value)
        {
            value = default(T);
            if (udpClient == null || !udpClient.IsOpen)
            {
                AddError(new Exception(string.Format("{0}:Udp.Read Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            int readLen = DataRecive;
            byte[] buff = new byte[readLen];
            udpClient.Read(buff, 0, readLen);

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
                AddError(new Exception(string.Format("{0}:Udp.Read Error,Typeof(T) must be string or byte[]", this.Text)));
            }
        }
        public override void Send<T>(T value)
        {
            if (udpClient == null || !udpClient.IsOpen)
            {
                AddError(new Exception(string.Format("{0}:Udp.Send Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            udpClient.DiscardBuffer();
            if (typeof(T) == typeof(byte[]))
            {
                byte[] buff = (byte[])(object)value;
                udpClient.Write(buff);
            }
            else if (typeof(T) == typeof(string))
            {
                string buff = (string)(object)value;
                udpClient.Write(buff);
            }
            else
            {
                AddError(new Exception(string.Format("{0}:Udp.Send Error,Typeof(T) must be string or byte[]", this.Text)));
            }
        }
        public override void Send<T>(T value, Dictionary<string, string> buff)
        {
            InitCommunite(buff);
            Send<T>(value);
        }
        public override void InitCommunite(Dictionary<string, string> buff)
        {
            if (buff.ContainsKey("RemotHost"))
            {
                udpClient.RemotHost = buff["RemotHost"];
            }
            if (buff.ContainsKey("RemotIP"))
            {
                udpClient.RemotHost = buff["RemotIP"];
            }
            if (buff.ContainsKey("RemotPort"))
            {
                udpClient.RemotPort = buff["RemotPort"].ToInt();
            }
        }
    }
}
