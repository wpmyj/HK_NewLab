using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace All.Communicate
{
    public abstract class Communicate:Base.Base,IDisposable
    {
        /// <summary>
        /// 刷新时间
        /// </summary>
        public int FlushTick
        { get; set; }
        /// <summary>
        /// 通讯名称
        /// </summary>
        public string Text
        { get; set; }
        /// <summary>
        /// 所有子设备
        /// </summary>
        public List<Meter.Meter> Meters
        { get; set; }
        /// <summary>
        /// 通讯是否已正常打开
        /// </summary>
        public abstract bool IsOpen
        { get; }
        /// <summary>
        /// 当前数据接收量
        /// </summary>
        public abstract int DataRecive
        { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="buff"></param>
        public abstract void Init(Dictionary<string, string> buff);
        /// <summary>
        /// 打开通讯
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// 关闭通讯
        /// </summary>
        public abstract void Close();
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public abstract void Read<T>(out T value);
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public abstract void Send<T>(T value);
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="buff"></param>
        public abstract void Send<T>(T value, Dictionary<string, string> buff);
        /// <summary>
        /// 初始化端口
        /// </summary>
        /// <param name="buff"></param>
        public abstract void InitCommunite(Dictionary<string, string> buff);
        /// <summary>
        /// 故障
        /// </summary>
        /// <param name="e"></param>
        public delegate void CommunicateErrorHandle(Exception e);
        /// <summary>
        /// 通讯故障
        /// </summary>
        public event CommunicateErrorHandle CommunicateErrorRaise;
        /// <summary>
        /// 添加通讯故障
        /// </summary>
        /// <param name="e"></param>
        internal void AddError(Exception e)
        {
            All.Class.Error.Add(e);
            if (CommunicateErrorRaise != null)
            {
                CommunicateErrorRaise(e);
            }
        }
        public Communicate()
        {
            this.Meters = new List<Meter.Meter>();
        }
        public void Dispose()
        {
            Close();
        }
        ~Communicate()
        {
            Close();
        }
        /// <summary>
        /// 从xml中解析出通讯类
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static Communicate Parse(Dictionary<string, string> buff)
        {
            if (!buff.ContainsKey("Class"))
            {
                All.Class.Error.Add("初始化通讯错误,初始化字符串不包含反射类", Environment.StackTrace);
                return null;
            }
            All.Class.Reflex<Communicate> tmpReflex = new Class.Reflex<Communicate>("All", buff["Class"]);
            Communicate result = (Communicate)tmpReflex.Get();
            if (result == null)
            {
                All.Class.Error.Add(string.Format("从命令空间反射类失败，请检查反射名称：{0}是否正确", buff["Class"]));
                return null;
            }
            result.Init(buff);
            if (!buff.ContainsKey("Run") || buff["Run"].ToBool())
            {
                result.Open();
            }
            return result;
        }
    }
}
