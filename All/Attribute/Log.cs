
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Contexts;
namespace All.Attribute
{
    /// <summary>
    /// 类特性,用于记录日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class LogClassAttribute : ContextAttribute, IContributeObjectSink
    {
        public LogClassAttribute()
            : base("Log")
        { }
        //实现IContributeObjectSink接口当中的消息接收器接口
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink next)
        {
            return new AopLogHandle(next);
        }
    }
    /// <summary>
    /// 方法特性,用于记录日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class LogMethodAttribute : System.Attribute { }

    public sealed class AopLogHandle :System.Runtime.Remoting.Messaging. IMessageSink
    {
        //下一个接收器
        private IMessageSink nextSink;
        public IMessageSink NextSink
        {
            get { return nextSink; }
        }
        public AopLogHandle(IMessageSink nextSink)
        {
            this.nextSink = nextSink;
        }
        //同步处理方法
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage retMsg = null;
            //方法调用消息接口
            IMethodCallMessage call = msg as IMethodCallMessage;

            //如果被调用的方法没打MyInterceptorMethodAttribute标签
            if (call == null || (System.Attribute.GetCustomAttribute(call.MethodBase, typeof(LogMethodAttribute))) == null)
            {
                retMsg = nextSink.SyncProcessMessage(msg);
            }
            //如果打了MyInterceptorMethodAttribute标签
            else
            {
                //System.Runtime.Remoting.Messaging
                string className = "", methodName = "";
                if (msg.Properties.Contains("__MethodName"))
                {
                    methodName = (string)msg.Properties["__MethodName"];
                }
                if (msg.Properties.Contains("__TypeName"))
                {
                    className = msg.Properties["__TypeName"].ToString().Split(',')[0];
                }
                All.Class.Log.Add(string.Format("Method=In,Class={0},Method={1}", className, methodName));
                int timeStart = Environment.TickCount;
                retMsg = nextSink.SyncProcessMessage(msg);
                All.Class.Log.Add(string.Format("Method=Out,Class={0},Method={1},Delay={2}", className, methodName, Environment.TickCount - timeStart));
            }

            return retMsg;
        }

        //异步处理方法（不需要）
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return null;
        }
    }

}
