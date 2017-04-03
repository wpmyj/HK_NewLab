using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace All.Communicate.Base
{
    public class TcpClient:Base
    {
        System.Net.Sockets.TcpClient tcp;
        /// <summary>
        /// 缓冲区数据
        /// </summary>
        public int DataRecive
        {
            get 
            {
                return readBuff.Count;
            }
        }

        bool isOpen = false;
        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }
        /// <summary>
        /// 远程地址
        /// </summary>
        public string RemotHost
        { get; set; }
        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotPort
        { get; set; }

        Thread thListen;//监听线程
        List<byte> readBuff = new List<byte>();//缓冲区
        /// <summary>
        /// 实例化一个TCP
        /// </summary>
        public TcpClient(string remotIP, int remotPort)
        {
            this.RemotHost = remotIP;
            this.RemotPort = remotPort;
        }
        /// <summary>
        /// 打开UDP端口监听
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            bool result = true;
            try
            {
                tcp = new System.Net.Sockets.TcpClient();
                tcp.Connect(this.RemotHost, this.RemotPort);
                if (tcp.Connected)
                {
                    thListen = new Thread(() => Listen());
                    thListen.IsBackground = true;
                    thListen.Start();
                    isOpen = true;
                }
            }
            catch (Exception e)
            {
                All.Class.Error.Add("远程地址", this.RemotHost);
                All.Class.Error.Add("远程端口", this.RemotPort.ToString());
                All.Class.Error.Add(e);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 关闭监听端口
        /// </summary>
        public void Close()
        {
            if (thListen != null)
            {
                thListen.Abort();
            }
            thListen = null;
            if (tcp != null)
            {
                tcp.Close();
            }
            tcp = null;
            isOpen = false;
        }
        ~TcpClient()
        {
            Close();
        }
        public void Listen()
        {
            byte[] buff = new byte[65536];
            byte[] tmp;
            IPEndPoint ie = new IPEndPoint(IPAddress.Any, 0);
            EndPoint send = (EndPoint)ie;
            int len = 0;
            try
            {
                while (true)
                {

                    len = tcp.Client.ReceiveFrom(buff, ref send);

                    OnGetArgs(this, new Base.ReciveArgs(this.RemotHost, this.RemotPort));

                    if (readBuff.Count > 65535)
                    {
                        All.Class.Error.Add("TCP缓冲区字节数组过长", Environment.StackTrace);
                        DiscardBuffer();
                    }
                    tmp = new byte[len];
                    Array.Copy(buff, 0, tmp, 0, len);
                    readBuff.AddRange(tmp.ToList());
                }
            }
            catch (Exception e)
            {
                if (tcp != null && tcp.Connected)
                {
                    tcp.Close();
                }
                isOpen = false;
                tcp = null;
                All.Class.Error.Add(e);
            }
        }
        protected override void OnGetArgs(object sender, Base.ReciveArgs reciveArgs)
        {
            base.OnGetArgs(sender, reciveArgs);
        }
        /// <summary>
        /// 清除缓冲区
        /// </summary>
        public void DiscardBuffer()
        {
            readBuff.Clear();
        }
        /// <summary>
        /// 读取缓冲区所有数据
        /// </summary>
        /// <param name="buff"></param>
        public void Read(byte[] buff)
        {
            Read(buff, 0, readBuff.Count);
        }
        /// <summary>
        /// 读取指定偏移缓冲区数据
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public void Read(byte[] buff, int offset, int count)
        {
            if ((offset + count) > readBuff.Count)
            {
                All.Class.Error.Add("UDP读取数据长度不正确", Environment.StackTrace);
                return;
            }
            Array.Copy(readBuff.ToArray(), offset, buff, 0, count);
            readBuff.RemoveRange(offset, count);
        }
        /// <summary>
        /// 发送数据到远程
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public bool Write(byte[] buff)
        {
            if (tcp == null)
            {
                All.Class.Error.Add("TCP为Null,不能发送数据", Environment.StackTrace);
                return false;
            }
            if (!isOpen)
            {
                All.Class.Error.Add("TCP端口没有打开,不能发送数据", Environment.StackTrace);
                return false;
            }
            try
            {
                tcp.Client.Send(buff);
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
            }
            return true;
        }
        /// <summary>
        /// 发送数据到远程,只能发送ASCII,其他编码,请手动编码后再发送
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Write(string value)
        {
            return Write(Encoding.ASCII.GetBytes(value));
        }
    }
}
