using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
namespace All.Communicate.Base
{
    public class Udp:Base
    {
        UdpClient udp;
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
        /// 本地端口
        /// </summary>
        public int LocalPort
        { get; set; }
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
        /// 实例化一个UDP
        /// </summary>
        /// <param name="localPort">本地监听商品</param>
        public Udp(int localPort)
        {
            this.LocalPort = localPort;
            this.RemotHost = "127.0.0.1";
            this.RemotPort = 20000;
        }
        public Udp(int localPort, string remotIP, int remotPort)
            :this(localPort)
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
                udp = new UdpClient(LocalPort);
                udp.Client.IOControl((IOControlCode)0x9800000C, new byte[] { Convert.ToByte(false) }, new byte[4]);//去掉远程断开连接的故障
                thListen = new Thread(() => Listen());
                thListen.IsBackground = true;
                thListen.Start();
                isOpen = true;
            }
            catch (Exception e)
            {
                All.Class.Error.Add("本地端口", this.LocalPort.ToString());
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
            if (udp != null)
            {
                udp.Close();
            }
            udp = null;
            isOpen = false;
        }
        ~Udp()
        {
            Close();
        }
        /// <summary>
        /// 循环监听端口
        /// </summary>
        private void Listen()
        {
            while (true)
            {
                try
                {
                    IPEndPoint remot = new IPEndPoint(IPAddress.Parse(RemotHost), RemotPort);
                    while (true)
                    {
                        byte[] tmpBuff = udp.Receive(ref remot);
                        if (readBuff.Count > 65535)
                        {
                            DiscardBuffer();
                            All.Class.Error.Add("UDP接收数据过长,已自动清除", Environment.StackTrace);
                        }
                        readBuff.AddRange(tmpBuff.ToList());
                        OnGetArgs(this, new Base.ReciveArgs(remot.Address.ToString(), remot.Port));
                        Thread.Sleep(10);
                    }
                }
                catch (Exception e)
                {
                    All.Class.Error.Add(e);
                }
                Thread.Sleep(20);
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
            return Write(buff, this.RemotHost, this.RemotPort);
        }
        /// <summary>
        /// 发送数据到远程
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="remotIP"></param>
        /// <param name="remotPort"></param>
        /// <returns></returns>
        public bool Write(byte[] buff, string remotIP, int remotPort)
        {
            if (udp == null)
            {
                All.Class.Error.Add("本地端口", this.LocalPort.ToString());
                All.Class.Error.Add("UDP为Null,不能发送数据",Environment.StackTrace);
                return false;
            }
            if (!isOpen)
            {
                All.Class.Error.Add("UDP端口没有打开,不能发送数据", Environment.StackTrace);
                return false;
            }
            try
            {
                udp.Send(buff, buff.Length, remotIP, remotPort);
            }
            catch (Exception e)
            {
                All.Class.Error.Add(e);
            }
            return true;
        }
        /// <summary>
        /// 发送数据到远程
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Write(string value)
        {
            return Write(value, this.RemotHost, this.RemotPort);
        }
        /// <summary>
        /// 发送数据到远程,只能发送ASCII,其他编码,请手动编码后再发送
        /// </summary>
        /// <param name="value"></param>
        /// <param name="remotIP"></param>
        /// <param name="remotPort"></param>
        /// <returns></returns>
        public bool Write(string value, string remotIP, int remotPort)
        {
            return Write(Encoding.ASCII.GetBytes(value), remotIP, remotPort);
        }
    }

}
