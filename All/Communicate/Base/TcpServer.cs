using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace All.Communicate.Base
{
    public class TcpServer : Base
    {
        /// <summary>
        /// 本地监听端口
        /// </summary>
        public int LocalPort
        { get; set; }
        bool isOpen = false;
        /// <summary>
        /// 当前监听是否打开
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
        }
        Thread thListen;
        System.Net.Sockets.TcpListener tcp;
        /// <summary>
        /// 所有连接客户端
        /// </summary>
        List<System.Net.Sockets.TcpClient> clients = new List<System.Net.Sockets.TcpClient>();
        protected override void OnGetArgs(object sender, ReciveArgs reciveArgs)
        {
            base.OnGetArgs(sender, reciveArgs);
        }
        public TcpServer(int localPort)
        {
            this.LocalPort = localPort;
        }
        /// <summary>
        /// 打开监听
        /// </summary>
        public bool Open()
        {
            bool result = true;
            try
            {
                tcp = new TcpListener(IPAddress.Any, this.LocalPort);
                tcp.Start();
                thListen = new Thread(() => Listen());
                thListen.IsBackground = true;
                thListen.Start();
                isOpen = true;
            }
            catch (Exception e)
            {
                All.Class.Error.Add("本地端口", this.LocalPort.ToString());
                All.Class.Error.Add(e);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 关闭端口连接
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
                tcp.Server.Close();
                tcp.Stop();
            }
            tcp = null;
            isOpen = false;
        }
        ~TcpServer()
        {
            Close();
        }
        private void Listen()
        {
            while (true)
            {
                System.Net.Sockets.TcpClient tmp = tcp.AcceptTcpClient();
                clients.Add(tmp);
            }
        }
        
    }
}
