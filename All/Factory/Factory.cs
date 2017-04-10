using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Factory
{
    public abstract class Factory
    {
        /// <summary>
        /// 当前机型的说明
        /// </summary>
        public abstract string Info
        { get;  }
        /// <summary>
        /// 机器通讯所使用的通讯类
        /// </summary>
        public abstract All.Communicate.Communicate Com
        { get; set; }
        /// <summary>
        /// 使用的底层设备,如标准Modbus等
        /// </summary>
        public All.Meter.Meter Meter
        { get; set; }
        int timeOut = 1000;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        /// <summary>
        /// 连接状态
        /// </summary>
        public virtual bool Conn
        {
            get
            {
                if (this.Com != null && this.Meter != null)
                {
                    return this.Meter.Conn;
                }
                return false;
            }
        }
        string alarm = "";
        /// <summary>
        /// 报警
        /// </summary>
        public string Alarm
        {
            get { return alarm; }
            set { alarm = value; }
        }
        protected System.Threading.Thread thRead;
        /// <summary>
        /// 多线程锁
        /// </summary>
        protected object lockObject = new object();
        /// <summary>
        /// 初始化端口
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public abstract bool Open();
        /// <summary>
        /// 关闭端口
        /// </summary>
        public virtual void Close()
        {
            if (thRead != null)
            {
                thRead.Join(300);
                thRead.Abort();
                thRead = null;
            }
            if (Meter != null)
            {
                this.Meter.Close();
                this.Meter = null;
            }
            if (Com != null)
            {
                this.Com.Close();
                this.Com = null;
            }
        }
    }
}
