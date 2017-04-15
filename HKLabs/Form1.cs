using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HKLabs
{
    public partial class Form1 : All.Window.BaseWindow
    {
        public Form1()
        {
            InitializeComponent();
        }
        All.Communicate.Udp udp1 = new All.Communicate.Udp();
        All.Communicate.Udp udp2 = new All.Communicate.Udp();
        All.Communicate.Udp udp3 = new All.Communicate.Udp();
        All.Meter.SSRead read1 = new All.Meter.SSRead();
        All.Meter.SSRead read2 = new All.Meter.SSRead();
        All.Meter.SSWrite write1 = new All.Meter.SSWrite();
        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("LocalPort", "9999");
            buff.Add("RemotPort", "8888");
            buff.Add("RemotHost", "127.0.0.1");
            udp1.Init(buff);
            udp1.Open();
            buff = new Dictionary<string, string>();
            buff.Add("LocalPort", "8888");
            buff.Add("RemotPort", "9999");
            buff.Add("RemotHost", "127.0.0.1");
            udp2.Init(buff);
            udp2.Open();
            buff = new Dictionary<string, string>();
            buff.Add("LocalPort", "7777");
            buff.Add("RemotPort", "9999");
            buff.Add("RemotHost", "127.0.0.1");
            udp3.Init(buff);
            udp3.Open();
            buff = new Dictionary<string, string>();
            buff.Add("String", "10");
            read1.Parent = udp1;
            read1.Init(buff);
            write1.Parent = udp2;
            write1.Init(buff);
            read2.Parent = udp3;
            read2.Init(buff);
            new System.Threading.Thread(() => timer1_Tick(timer1, new EventArgs())) { IsBackground = true }.Start();

            foreach (string ss in Enum.GetNames(typeof(All.Class.Style.BackColors)))
            {
                listBox1.Items.Add(ss);
            }
            foreach (string ss in Enum.GetNames(typeof(All.Class.Style.FrontColors)))
            {
                listBox2.Items.Add(ss);
            }

        }
        protected override void OnShown(EventArgs e)
        {
            //this.Visible = false;
            base.OnShown(e);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            All.Class.Style.ChangeBack((All.Class.Style.BackColors)Enum.Parse(typeof(All.Class.Style.BackColors), listBox1.Text));
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0)
            {
                return;
            }
            All.Class.Style.ChangeFront((All.Class.Style.FrontColors)Enum.Parse(typeof(All.Class.Style.FrontColors), listBox2.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            All.Communicate.Base.TcpServer server = new All.Communicate.Base.TcpServer(9999);
            server.Open();


            All.Communicate.Base.TcpClient tcp = new All.Communicate.Base.TcpClient("127.0.0.1", 9999);
            tcp.Open();
            tcp.GetArgs += tcp_GetArgs;
        }

        void tcp_GetArgs(object sender, All.Communicate.Base.Base.ReciveArgs reciveArgs)
        {
            All.Communicate.Base.TcpClient tcp=(All.Communicate.Base.TcpClient)sender;
            byte[] buff = new byte[tcp.DataRecive];
            tcp.Read(buff);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            All.Window.RegionCreate rc = new All.Window.RegionCreate();
            rc.Show();
            All.Window.Region rr = new All.Window.Region();
            rr.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Show("天天向上,你好吗,内容长点是不是好看很多试试看?", "标题", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            this.Show("天天向上,你好吗,内容长点是不是好看很多试试看?", "标题", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
            this.Show("天天向上,你好吗,内容长点是不是好看很多试试看?", "标题", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Show("天天向上,你好吗,内容长点是不是好看很多试试看?", "标题", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Dictionary<string, string> parm = new Dictionary<string, string>();
            parm.Add("RemotPort", "7777");
            int i = 0;
            string s1 = "", s2 = "";
            while (true)
            {
                int start = Environment.TickCount;
                parm["RemotPort"] = "7777";
                write1.Parent.InitCommunite(parm);
                write1.WriteInternal<string>(string.Format("{0}", i++), 0);
                parm["RemotPort"] = "9999";
                write1.Parent.InitCommunite(parm);
                write1.WriteInternal<string>(string.Format("{0}", i), 0);

                read1.Read<string>(out s1, 0);
                read2.Read<string>(out s2, 0);


                label1.SetText(string.Format("{0}_{1}", s1, s2));
                this.CrossThreadDo(() =>
                {
                    textBox1.Text = string.Format("{0:F0}", Environment.TickCount - start );
                    dateTime1.Value = DateTime.Now;
                });
                System.Threading.Thread.Sleep(100);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }
    }
    public class AAA
    {
        public int CCC
        { get; set; }
        public int DDD
        { get; set; }
        public int BBB
        { get; set; }
        public int EEE
        { get; set; }
    }
}
