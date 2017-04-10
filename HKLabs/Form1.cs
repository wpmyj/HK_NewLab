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
        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Visible = false;
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
            All.Class.Style.ChangeBack((All.Class.Style.BackColors)Enum.Parse(typeof(All.Class.Style.BackColors), listBox1.Text));
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            //All.Class.Reflex<All.Data.DataReadAndWrite> sql = new All.Class.Reflex<All.Data.DataReadAndWrite>("All.Data.SqlCe", "All.Data.SqlCe");
            //All.Data.DataReadAndWrite sql2 = sql.Get();
            //sql2.Login("C:\\", "1.sdf", "", "");
            //DataTable dt = sql2.Read("select * from tableA");
        }
    }
}
