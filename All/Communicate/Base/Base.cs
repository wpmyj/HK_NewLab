using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate.Base
{
    public  class Base
    {
        /// <summary>
        /// 数据接收委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reciveArgs"></param>
        public delegate void GetArgsHandle(object sender, Base.ReciveArgs reciveArgs);
        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event GetArgsHandle GetArgs;
        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="reciveArgs"></param>
        /// <returns>数据是否已处理完毕</returns>
        protected virtual void OnGetArgs(object sender, Base.ReciveArgs reciveArgs)
        {
            if (GetArgs != null)
            {
                GetArgs(sender, reciveArgs);
            }
        }
        /// <summary>
        /// 网络接收数据事件
        /// </summary>
        public class ReciveArgs
        {
            /// <summary>
            /// 远程地址
            /// </summary>
            public string RemotIP
            { get; set; }
            /// <summary>
            /// 远程端口
            /// </summary>
            public int RemotPort
            { get; set; }
            /// <summary>
            /// 网络接收数据事件
            /// </summary>
            /// <param name="remotIP"></param>
            /// <param name="remotPort"></param>
            /// <param name="value"></param>
            public ReciveArgs(string remotIP, int remotPort)
            {
                this.RemotIP = remotIP;
                this.RemotPort = remotPort;
            }
        }
    }
}
