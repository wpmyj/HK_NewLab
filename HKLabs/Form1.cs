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
            label1.Text = string.Format("{0}", this.Size);
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

    }
}
