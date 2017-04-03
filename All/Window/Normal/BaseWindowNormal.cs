using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Window
{
    public partial class BaseWindowNormal :Form
    {
        private bool btnHelp;
        /// <summary>
        /// 显示help按钮
        /// </summary>
        [Description("显示help按钮")]
        [Category("Shuai")]
        public bool BtnHelp
        {
            get { return btnHelp; }
            set { btnHelp = value; Init(); }
        }
        private bool btnMax = true;
        /// <summary>
        /// 显示最大化按钮
        /// </summary>
        [Description("显示最大化按钮")]
        [Category("Shuai")]
        public bool BtnMax
        {
            get { return btnMax; }
            set { btnMax = value; Init(); }
        }
        private bool btnSetting;
        /// <summary>
        /// 显示设置按钮
        /// </summary>
        [Description("显示设置按钮")]
        [Category("Shuai")]
        public bool BtnSetting
        {
            get { return btnSetting; }
            set { btnSetting = value; Init(); }
        }
        private bool btnMin = true;
        /// <summary>
        /// 显示最小化按钮
        /// </summary>
        [Description("显示最小化按钮")]
        [Category("Shuai")]
        public bool BtnMin
        {
            get { return btnMin; }
            set { btnMin = value; Init(); }
        }
        private bool btnClose = true;
        /// <summary>
        /// 显示关闭按钮
        /// </summary>
        [Description("显示关闭按钮")]
        [Category("Shuai")]
        public bool BtnClose
        {
            get { return btnClose; }
            set { btnClose = value; Init(); }
        }
        private bool showTitle = true;
        /// <summary>
        /// 显示标题栏
        /// </summary>
        [Description("显示标题栏")]
        [Category("Shuai")]
        public bool ShowTitle
        {
            get { return showTitle; }
            set { showTitle = value; Init(); }
        }
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        [Category("Shuai")]
        public new string Text
        {
            get { return base.Text; }
            set { base.Text = value; lblTitle.Text = value; }
        }
        [Description("缩放模式")]
        [Category("Shuai")]
        public new bool ShowIcon
        {
            get { return base.ShowIcon; }
            set { base.ShowIcon = value; picIcon.Visible = value; }
        }
        [Description("缩放模式")]
        [Category("Shuai")]
        public new Icon Icon
        {
            get { return base.Icon; }
            set { base.Icon = value; picIcon.Image = value.ToBitmap(); }
        }
        public BaseWindowNormal()
        {
            InitializeComponent();
            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
        }
        private void Init()
        {
            int space = 5;
            int left = this.Width - space;
            lblTitle.Visible = showTitle;
            lblClose.Visible = showTitle & btnClose;
            lblMax.Visible = showTitle & btnMax;
            lblMin.Visible = showTitle & btnMin;
            lblSetting.Visible = showTitle & btnSetting;
            lblHelp.Visible = showTitle & btnHelp;

            if (showTitle & btnClose)
            {
                lblClose.Left = left - lblClose.Width;
                left = lblClose.Left - space;
            }
            if (showTitle & btnMax)
            {
                lblMax.Left = left - lblMax.Width;
                left = lblMax.Left - space;
            }
            if (showTitle & btnMin)
            {
                lblMin.Left = left - lblMin.Width;
                left = lblMin.Left - space;
            }
            if (showTitle & btnSetting)
            {
                lblSetting.Left = left - lblSetting.Width;
                left = lblSetting.Left - space;
            }
            if (showTitle & btnHelp)
            {
                lblHelp.Left = left - lblHelp.Width;
                left = lblHelp.Left - space;
            }
            if (WindowState == FormWindowState.Maximized)
            {
                lblMax.Text = "2";
            }
            else
            {
                lblMax.Text = "1";
            }
        }
        protected override void OnResize(EventArgs e)
        {
            Init();
            base.OnResize(e);
        }

        private void lblTitle_DoubleClick(object sender, EventArgs e)
        {
            if (lblMax.Visible)
            {
                lblMax_Click(lblMax, new EventArgs());
            }
        }
        bool isMouseDown = false;
        Point oldMousePoint = Point.Empty;
        Point oldWindowPoint = Point.Empty;
        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            oldMousePoint = this.PointToScreen(e.Location);
            oldWindowPoint = this.Location;
            this.Cursor = Cursors.SizeAll;
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            oldMousePoint = Point.Empty;
            oldWindowPoint = Point.Empty;
            this.Cursor = Cursors.Default;
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point nowMousePoint = this.PointToScreen(e.Location);
                int x = oldWindowPoint.X + nowMousePoint.X - oldMousePoint.X;
                int y = oldWindowPoint.Y + nowMousePoint.Y - oldMousePoint.Y;
                this.Location = new Point(x, y);
            }
        }

        private void lblMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void lblMax_Click(object sender, EventArgs e)
        {
            if (lblMax.Text == "1")
            {
                lblMax.Text = "2";
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                lblMax.Text = "1";
                this.WindowState = FormWindowState.Normal;
            }

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BaseWindow_Load(object sender, EventArgs e)
        {

        }

        private void lblButton_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            switch (lbl.Name)
            {
                case "lblClose":
                    lbl.BackColor = Color.White;
                    lbl.ForeColor = Color.Red;
                    break;
                case "lblMax":
                case "lblMin":
                    lbl.BackColor = Color.White;
                    lbl.ForeColor = Color.Blue;
                    break;
                case "lblSetting":
                case "lblHelp":
                    lbl.BackColor = Color.White;
                    lbl.ForeColor = Color.Cyan;
                    break;
            }
        }

        private void lblButton_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            switch (lbl.Name)
            {
                case "lblClose":
                    lbl.BackColor = lblTitle.BackColor;
                    lbl.ForeColor = Color.White;
                    break;
                case "lblMax":
                case "lblMin":
                    lbl.BackColor = lblTitle.BackColor;
                    lbl.ForeColor = Color.White;
                    break;
                case "lblSetting":
                case "lblHelp":
                    lbl.BackColor = lblTitle.BackColor;
                    lbl.ForeColor = Color.White;
                    break;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark, 1), new Point(0, 0), new Point(Width - 2, 0));
            e.Graphics.DrawLine(new Pen(SystemColors.Control, 1), new Point(1, 1), new Point(Width, 1));

            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark, 1), new Point(0, 0), new Point(0, Height));
            e.Graphics.DrawLine(new Pen(SystemColors.Control, 1), new Point(1, 1), new Point(1, Height));

            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark, 1), new Point(1, Height - 2), new Point(Width - 2, Height - 2));
            e.Graphics.DrawLine(new Pen(SystemColors.Control, 1), new Point(0, Height - 1), new Point(Width - 1, Height - 1));

            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark, 1), new Point(Width - 2, 1), new Point(Width - 2, Height - 2));
            e.Graphics.DrawLine(new Pen(SystemColors.Control, 1), new Point(Width - 1, 0), new Point(Width - 1, Height - 1));

            base.OnPaint(e);
        }
    }
}
