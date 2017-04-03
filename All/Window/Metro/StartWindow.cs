using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace All.Window
{
    [DefaultEvent("Load")]
    public partial class StartWindow : BaseWindow
    {
        const int space = 22;
        string title = "美联博空调外机信息化生产系统";
        /// <summary>
        /// 标题
        /// </summary>
        [Category("Shuai")]
        [Description("标题")]
        public string Title
        {
            get { return title; }
            set { title = value; this.Invalidate(); }
        }
        string company = "广州市弘科自动化工程有限公司";
        /// <summary>
        /// 公司名称
        /// </summary>
        [Category("Shuai")]
        [Description("公司名称")]
        public string Company
        {
            get { return company; }
            set { company = value; this.Invalidate(); }
        }
        string info = "正在等待与主机连接，请稍候。。。";
        /// <summary>
        /// 显示信息
        /// </summary>
        [Category("Shuai")]
        [Description("显示信息")]
        public string Info
        {
            get { return info; }
            set { info = value; this.Invalidate(rectInfo); }
        }
        Font f1 = new Font("Segoe UI", 9, FontStyle.Bold);
        Font f2 = new Font("Segoe UI", 16.25f, FontStyle.Bold);
        string code = "";
        string copyRight = string.Format("All CopyRight © 2015-{0:yyyy} By ShuaiShuai", DateTime.Now);
        System.Drawing.StringFormat sf = new StringFormat();
        Rectangle rectInfo;
        Thread t;
        /// <summary>
        /// 加载程序
        /// </summary>
        [Category("Shuai")]
        [Description("加载程序")]
        public new event EventHandler Load;
        /// <summary>
        /// 程序加载完毕
        /// </summary>
        [Category("Shuai")]
        [Description("程序加载完毕")]
        public event EventHandler LoadOver;
        System.Windows.Forms.Timer time = new System.Windows.Forms.Timer();
        public StartWindow()
        {
            sf.Alignment = System.Drawing.StringAlignment.Far;
            sf.LineAlignment = System.Drawing.StringAlignment.Center;
            string fileName = string.Format(".\\{0}", System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            if (fileName.EndsWith(".vshost"))
            {
                fileName = fileName.Replace(".vshost", "");
            }
            if (All.Class.Environment.IsDesignMode)
            { code = "V1.0.0.0"; }
            else
            {
                code = string.Format("V{0}", All.Class.FileIO.GetFileCode(string.Format("{0}.exe", fileName), "1.0.0.2"));
            } InitializeComponent();
            rectInfo = new Rectangle(new Point(pictureBox1.Left + pictureBox1.Width + space, 260 * this.Height / 321), new Size(this.Width - space - pictureBox1.Left - pictureBox1.Width - space, 20));
            this.StartPosition = FormStartPosition.CenterScreen;
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.DrawString(company, f1, new SolidBrush(All.Class.Style.FontColor), new Rectangle(pictureBox1.Width + pictureBox1.Left + space, 15* this.Height / 321 , this.Width - space - space - pictureBox1.Width - pictureBox1.Left, 20), sf);
            e.Graphics.DrawString(title, f2, new SolidBrush(All.Class.Style.FontColor), new Point(pictureBox1.Left + pictureBox1.Width + space, 135 * this.Height / 321));
            e.Graphics.DrawString(code, f1, new SolidBrush(All.Class.Style.FontColor), new Rectangle(pictureBox1.Width + pictureBox1.Left + space, 143 * this.Height / 321, this.Width - space - space - pictureBox1.Width - pictureBox1.Left, 20), sf);
            e.Graphics.DrawLine(new Pen(All.Class.Style.FontColor, 1), pictureBox1.Left + pictureBox1.Width + space, 166* this.Height / 321 , this.Width - space, 166* this.Height / 321 );
            e.Graphics.DrawString(copyRight, f1, new SolidBrush(All.Class.Style.FontColor), new Rectangle(pictureBox1.Width + pictureBox1.Left + space, 169 * this.Height / 321, this.Width - space - space - pictureBox1.Width - pictureBox1.Left, 20), sf);
            e.Graphics.DrawString(info, f1, new SolidBrush(All.Class.Style.FontColor), rectInfo);
            base.OnPaint(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        void time_Tick(object sender, EventArgs e)
        {
            if (t.ThreadState == ThreadState.Aborted || t.ThreadState == ThreadState.Stopped)
            {
                time.Tick -= time_Tick;
                time.Enabled = false;
                if (LoadOver != null)
                {
                    this.HideMe();
                    LoadOver(sender, e);
                    this.CloseMe();
                }
            }
        }
        private void LoadAll(object send,EventArgs e)
        {
            if (Load != null)
            {
                Load(send, e);
            }
        }
        void HideMe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(HideMe));
            }
            else
            {
                this.Hide();
            }
        }
        void CloseMe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(CloseMe));
            }
            else
            {
                this.Close();
            }
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (progressBar1 != null)
            {
                progressBar1.Left = pictureBox1.Left + pictureBox1.Width + space;
                progressBar1.Width = this.Width - space - progressBar1.Left;
                progressBar1.Top = 283 * this.Height / 321;
                rectInfo = new Rectangle(new Point(pictureBox1.Left + pictureBox1.Width + space, 260 * this.Height / 321), new Size(this.Width - space - pictureBox1.Left - pictureBox1.Width - space, 20));
            }
            base.OnSizeChanged(e);
        }
        private void StartWindow_Shown(object sender, EventArgs e)
        {
            if (Load != null)
            {
                time.Enabled = false;
                time.Tick += time_Tick;
                time.Interval = 500;

                t = new Thread(() => LoadAll(this, e));
                t.IsBackground = true;
                t.Start();
                time.Enabled = true;
            }
        }

        private void StartWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
