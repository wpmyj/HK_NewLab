using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Contexts;

namespace HKLabs
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        int i = 0;
        object lockObject = new object();
        ccccc c = new ccccc();
        All.Meter.SSRead ss = new All.Meter.SSRead();
        private void Form5_Load(object sender, EventArgs e)
        {
            All.Class.Thread.CreateOrOpen("Test", Flush, 10);
            All.Class.Error.SingleError = false;
        }
        private void Flush()
        {
            lock (lockObject)
            {
                ss.Init(new Dictionary<string, string>());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
    [All.Attribute.LogClass]
    public class BusinessHandler : ContextBoundObject
    {
        [MyInterceptorMethod]
        public void DoSomething()
        {
            MessageBox.Show("执行了方法本身！");
        }
    }
    [All.Attribute.LogClass]
    public class ccccc : ContextBoundObject
    {
        [All.Attribute.LogMethod]
        public void AAAA()
        {
            All.Class.Log.Add("12312");
        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class MyInterceptorMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MyInterceptorAttribute : ContextAttribute, IContributeObjectSink
    {
        public MyInterceptorAttribute()
            : base("MyInterceptor")
        { }

        //实现IContributeObjectSink接口当中的消息接收器接口
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink next)
        {
            return new MyAopHandler(next);
        }
    }
    public sealed class MyAopHandler : IMessageSink
    {
        //下一个接收器
        private IMessageSink nextSink;
        public IMessageSink NextSink
        {
            get { return nextSink; }
        }
        public MyAopHandler(IMessageSink nextSink)
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
            if (call == null || (Attribute.GetCustomAttribute(call.MethodBase, typeof(MyInterceptorMethodAttribute))) == null)
            {
                retMsg = nextSink.SyncProcessMessage(msg);
            }
            //如果打了MyInterceptorMethodAttribute标签
            else
            {
                MessageBox.Show("执行之前");
                retMsg = nextSink.SyncProcessMessage(msg);
                MessageBox.Show("执行之后");
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
