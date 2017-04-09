using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate
{
    public class TcpClient:Communicate
    {

        Base.TcpClient tcpClient;
        /// <summary>
        /// UDP端
        /// </summary>
        public Base.TcpClient Client
        {
            get { return tcpClient; }
            set { tcpClient = value; }
        }
        public override bool IsOpen
        {
            get
            {
                if (tcpClient == null)
                {
                    return false;
                }
                return tcpClient.IsOpen;
            }
        }
        public override int DataRecive
        {
            get
            {
                if (tcpClient == null)
                {
                    return 0;
                }
                return tcpClient.DataRecive;
            }
        }
        public override void Init(Dictionary<string, string> buff)
        {
            if (tcpClient == null)
            {
                tcpClient = new Base.TcpClient("127.0.0.1", 3000);
            }
            if (tcpClient.IsOpen)
            {
                tcpClient.Close();
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
        public override void Open()
        {
            try
            {
                if (!tcpClient.IsOpen)
                {
                    tcpClient.Open();
                    tcpClient.GetArgs += OnGetArgs;
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
                if (tcpClient.IsOpen)
                {
                    tcpClient.GetArgs -= OnGetArgs;
                    tcpClient.Close();
                }
            }
            catch (Exception e)
            {
                AddError(e);
            }
        }
        protected override void OnGetArgs(object sender, Base.Base.ReciveArgs reciveArgs)
        {
            base.OnGetArgs(this, reciveArgs);
        }
        public override void Read<T>(out T value)
        {
            value = default(T);
            if (tcpClient == null || !tcpClient.IsOpen)
            {
                AddError(new Exception(string.Format("{0}:TcpClient.Read Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            int readLen = DataRecive;
            byte[] buff = new byte[readLen];
            tcpClient.Read(buff, 0, readLen);

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
                AddError(new Exception(string.Format("{0}:TcpClient.Read Error,Typeof(T) must be string or byte[]", this.Text)));
            }
        }
        public override void Send<T>(T value)
        {
            if (tcpClient == null || !tcpClient.IsOpen)
            {
                AddError(new Exception(string.Format("{0}:TcpClient.Send Error,serialPort is null or it's not open", this.Text)));
                return;
            }
            tcpClient.DiscardBuffer();
            if (typeof(T) == typeof(byte[]))
            {
                byte[] buff = (byte[])(object)value;
                tcpClient.Write(buff);
            }
            else if (typeof(T) == typeof(string))
            {
                string buff = (string)(object)value;
                tcpClient.Write(buff);
            }
            else
            {
                AddError(new Exception(string.Format("{0}:TcpClient.Send Error,Typeof(T) must be string or byte[]", this.Text)));
            }
        }
        public override void Send<T>(T value, Dictionary<string, string> buff)
        {
            InitCommunite(buff);
            Send<T>(value);
        }
        public override void InitCommunite(Dictionary<string, string> buff)
        {
            this.Close();
            if (!buff.ContainsKey("RemotHost") && !buff.ContainsKey("RemotIP"))
            {
                AddError(new Exception(string.Format("{0}:TcpClient.InitCommunite Error,parm<buff> need RemotHost values", this.Text)));
                return;
            }
            if (!buff.ContainsKey("RemotPort"))
            {
                AddError(new Exception(string.Format("{0}:TcpClient.InitCommunite Error,parm<buff> need RemotPort values", this.Text)));
                return;
            }
            if (buff.ContainsKey("RemotIP"))
            {
                tcpClient.RemotHost = buff["RemotIP"];
            }
            if (buff.ContainsKey("RemotHost"))
            {
                tcpClient.RemotHost = buff["RemotHost"];
            }
            tcpClient.RemotPort = buff["RemotPort"].ToInt();
            this.Open();
        }
    }
}
