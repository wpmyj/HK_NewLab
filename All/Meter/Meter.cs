using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Meter
{
    /// <summary>
    /// 所有仪表的基础类
    /// </summary>
    public abstract class Meter:IDisposable
    {
        /// <summary>
        /// 故障事件
        /// </summary>
        public event Action<string> ErrorRaise;
        /// <summary>
        /// 仪表连接状态改变
        /// </summary>
        public event Action<bool> ConnChange;
        /// <summary>
        /// 仪表初始化设置
        /// </summary>
        public abstract Dictionary<string, string> InitParm
        { get; set; }
        /// <summary>
        /// 仪表名称
        /// </summary>
        public string Text
        { get; set; }
        /// <summary>
        /// 当前设备有通讯类
        /// </summary>
        public Communicate.Communicate Parent
        { get; set; }

        bool conn = true;
        int tmpErrorCount = 0;
        /// <summary>
        /// 仪表连接状态
        /// </summary>
        public bool Conn
        {
            get { return conn; }
            set
            {
                if (value)
                {
                    tmpErrorCount = 0;
                    if (ConnChange != null && conn != value)
                    {
                        ConnChange(value);
                    }
                    conn = true;
                }
                else
                {
                    tmpErrorCount++;
                    if (tmpErrorCount > errorCount)
                    {
                        conn = false;
                        tmpErrorCount = errorCount;
                        if (ConnChange != null && conn != value)
                        {
                            ConnChange(value);
                        }
                    }
                }
            }
        }
        int timeOut = 1000;
        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }
        int errorCount = 3;
        /// <summary>
        /// 故障最大次数
        /// </summary>
        public int ErrorCount
        {
            get { return errorCount; }
            set { errorCount = value; }
        }
        string error = "";
        /// <summary>
        /// 故障信息
        /// </summary>
        public string Error
        {
            get { return error; }
            set {
                if (error != value && value != "")
                {
                    if (ErrorRaise != null)
                    {
                        ErrorRaise(value);
                    }
                    All.Class.Error.Add(value);
                }
                error = value; }
        }
        internal object lockObject = new object();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="InitBuff"></param>
        public abstract void Init(Dictionary<string, string> initParm);

        public void Dispose()
        { 
        }
        /// <summary>
        /// 关闭当前设备
        /// </summary>
        public virtual void Close()
        { 

        }
        ~Meter()
        {
            this.Close();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="value">读取返回值</param>
        /// <param name="start">起始点</param>
        /// <param name="end">结束点</param>
        /// <returns></returns>
        public virtual bool Read<T>(out List<T> value, int start, int end)
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("Start", start.ToString());
            buff.Add("End", end.ToString());
            return Read<T>(out value, buff);
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T">返回数据类型</typeparam>
        /// <param name="value">读取返回值</param>
        /// <param name="start">起始点</param>
        /// <returns>返回结果值</returns>
        public virtual bool Read<T>(out T value, int start)
        {
            List<T> buff = new List<T>();
            bool result = Read<T>(out buff, start, start);
            if (buff == null || buff.Count <= 0)
            {
                value = default(T);
            }
            else
            {
                value = buff[0];
            }
            return result;
        }
        /// <summary>
        /// 读取非兼容格式数据
        /// </summary>
        /// <param name="init"></param>
        /// <returns></returns>
        public abstract bool Read<T>(out List<T> value, Dictionary<string, string> parm);
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public virtual bool WriteInternal<T>(T value, int start)
        {
            List<T> buff = new List<T>();
            buff.Add(value);
            return WriteInternal<T>(buff, start, start);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public virtual bool WriteInternal<T>(List<T> value, int start, int end)
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("Start", start.ToString());
            buff.Add("End", end.ToString());
            return WriteInternal<T>(value, buff);
        }
        /// <summary>
        /// 写入非兼容格式数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool WriteInternal<T>(List<T> value, Dictionary<string, string> parm);
        /// <summary>
        /// 测试一次连接
        /// </summary>
        /// <returns></returns>
        public abstract bool Test();
        /// <summary>
        /// 将数据直接写入通讯
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sendBuff"></param>
        /// <returns></returns>
        public bool Write<T>(T sendBuff)
        {
            bool result = true;
            try
            {
                if (this.Parent == null)
                {
                    All.Class.Error.Add("当前设备没有指定的通讯类", Environment.StackTrace);
                    return false;
                }
                if ((this.Parent is Communicate.Udp || this.Parent is Communicate.TcpClient)
                    && InitParm.ContainsKey("RemotHost") && InitParm.ContainsKey("RemotPort"))
                {
                    this.Parent.Send<T>(sendBuff, InitParm);
                }
                else
                {
                    this.Parent.Send<T>(sendBuff);
                }
            }
            catch (Exception e)
            {
                result = false;
                All.Class.Error.Add(e);
            }
            return result;
        }
        public bool Read<T>(int len, out T readBuff)
        {
            int readLen = 0;
            bool result = true;
            readBuff = default(T);
            try
            {
                if (this.Parent == null)
                {
                    All.Class.Error.Add("当前设备没有指定的通讯类", Environment.StackTrace);
                    result = false;
                    return result;
                }
                if (!this.Parent.IsOpen)
                {
                    this.Parent.Open();
                }
                int startTime = Environment.TickCount;
                bool timeOut = false;
                bool getData = false;
                if (len > 0)
                {
                    do
                    {
                        readLen = this.Parent.DataRecive;
                        System.Threading.Thread.Sleep(50);
                        if ((Environment.TickCount - startTime) > TimeOut)
                        {
                            timeOut = true;
                        }
                        if (this.Parent.DataRecive >= len && this.Parent.DataRecive == readLen)
                        {
                            getData = true;
                        }
                    } while (!timeOut && !getData);
                }
                else
                {
                    if (this.Parent.DataRecive > 0)
                    {
                        do
                        {
                            readLen = this.Parent.DataRecive;
                            System.Threading.Thread.Sleep(50);
                            if ((Environment.TickCount - startTime) > TimeOut)
                            {
                                timeOut = true;
                            }
                            if (this.Parent.DataRecive == readLen)
                            {
                                getData = true;
                            }
                        }
                        while (!timeOut && !getData);
                    }
                    else
                    {
                        getData = true;
                    }
                }
                if (timeOut && !getData)//超时
                {
                    result = false;
                    //if (mostLog)
                    //{
                    //    All.Class.Log.Add(string.Format("{0}读取数据超时,要求长度:{1},实际长度:{2}", Text, len, this.Parent.DataRecive));
                    //}
                }
                else//读取数据OK
                {
                    if (len > 0)//实际长度不为0，主要判断 读取条码枪时，长度为0，通讯也正常的情况
                    {
                        byte[] readTmpBuff;
                        this.Parent.Read<byte[]>(out readTmpBuff);
                        if (readTmpBuff.Length < len)
                        {
                            //if (mostLog)
                            //{
                            //    All.Class.Log.Add(string.Format("{0}实际读取的参数和要求参数长度不一致,要求长度:{1},实际长度:{2}", Text, len, readTmpBuff.Length));
                            //}
                            result = false;
                        }
                        else
                        {
                            switch (Class.TypeUse.GetType<T>())
                            {
                                case All.Class.TypeUse.TypeList.String:
                                    readBuff = (T)(object)Encoding.UTF8.GetString(readTmpBuff);
                                    break;
                                case All.Class.TypeUse.TypeList.Bytes:
                                    readBuff = (T)(object)readTmpBuff;
                                    break;
                                default:
                                    All.Class.Error.Add(string.Format("读取的参数类型不正确,只能读取Byte[]或者String,当前读取类型为:{0}", typeof(T).ToString()));
                                    result = false;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //条码等，没有数据时，直接返回，不进行连接的判断
                        switch (All.Class.TypeUse.GetType<T>())
                        {
                            case Class.TypeUse.TypeList.Bytes:
                                readBuff = default(T);
                                break;
                            case Class.TypeUse.TypeList.String:
                                readBuff = (T)(object)"";
                                break;
                            case Class.TypeUse.TypeList.Boolean:
                                readBuff = (T)(object)false;
                                break;
                            case Class.TypeUse.TypeList.Byte:
                            case Class.TypeUse.TypeList.Double:
                            case Class.TypeUse.TypeList.Float:
                            case Class.TypeUse.TypeList.Int:
                            case Class.TypeUse.TypeList.UShort:
                            case Class.TypeUse.TypeList.Long:
                                readBuff = (T)(object)0;
                                break;
                            case Class.TypeUse.TypeList.DateTime:
                                readBuff = (T)(object)DateTime.Now;
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                All.Class.Error.Add(e);
            }
            Conn = result;
            return result;
        }
        /// <summary>
        /// 将数据写入通讯并延时读取返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="sendBuff"></param>
        /// <param name="len"></param>
        /// <param name="readBuff"></param>
        /// <returns></returns>
        public bool WriteAndRead<T, U>(T sendBuff, int len, out U readBuff)
        {
            bool result = Write<T>(sendBuff);
            readBuff = default(U);
            result = result && Read<U>(len, out readBuff);
            return result;
        }
        /// <summary>
        /// 从xml中解析出设备
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static Meter Parse(Dictionary<string, string> buff)
        {
            return Parse(buff, null);
        }
        /// <summary>
        /// 从xml中解析出设备,并初始化设备
        /// </summary>
        /// <param name="buff">xml字符</param>
        /// <param name="comm">通讯类</param>
        /// <returns></returns>
        public static Meter Parse(Dictionary<string, string> buff,Communicate.Communicate comm)
        {
            if (!buff.ContainsKey("Class"))
            {
                All.Class.Error.Add("初始化通讯错误,初始化字符串不包含反射类", Environment.StackTrace);
                return null;
            }
            All.Class.Reflex<Meter> tmpReflex = new Class.Reflex<Meter>("All", buff["Class"]);
            Meter result = (Meter)tmpReflex.Get();
            if (result == null)
            {
                All.Class.Error.Add(string.Format("从命令空间反射类失败，请检查反射名称：{0}是否正确", buff["Class"]));
                return null;
            }
            result.Parent = comm;
            result.InitParm = buff;
            if (comm != null && (!buff.ContainsKey("Run") || buff["Run"].ToBool()))
            {
                result.Init(buff);
            }
            return result;
        }
    }
}
