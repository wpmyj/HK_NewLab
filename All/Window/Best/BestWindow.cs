using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace All.Window
{
    public partial class BestWindow : Form, All.Class.Style.ChangeTheme
    {
        bool closeBox = true;
        /// <summary>
        /// 显示关闭按钮
        /// </summary>
        [Description("显示关闭按钮")]
        [Category("Shuai")]
        public bool CloseBox
        {
            get { return closeBox; }
            set { closeBox = value; 
                this.Invalidate();
            }
        }

        bool theme = false;
        /// <summary>
        /// 显示主题按钮
        /// </summary>
        [Description("显示主题按钮")]
        [Category("Shuai")]
        public bool Theme
        {
            get { return theme; }
            set
            {
                theme = value; this.Invalidate();
            }
        }
        bool themeBack = true;
        /// <summary>
        /// 显示主题背景色切换按钮
        /// </summary>
        [Description("显示主题背景色切换按钮")]
        [Category("Shuai")]
        public bool ThemeBack
        {
            get { return themeBack; }
            set
            {
                if (themeBack != value)
                {
                    if (!All.Class.Environment.IsDesignMode && menuTheme != null)
                    {
                        if (menuTheme.Items.ContainsKey("separator"))
                        {
                            menuTheme.Items["separatro"].Visible = value;
                        }
                        Enum.GetNames(typeof(All.Class.Style.BackColors)).ToList().ForEach(
                            b =>
                            {
                                if (menuTheme.Items.ContainsKey(b))
                                {
                                    menuTheme.Items[b].Visible = value;
                                }
                            });
                    }
                }
                themeBack = value;
            }
        }
        const int iconHeight = 21;
        const int TitleHeight = 28;
        protected override void OnTextChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnTextChanged(e);
        }
        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return All.Class.Style.BackColor;
            }
        }
        Rectangle rIcon = Rectangle.Empty;
        Rectangle rTheme = Rectangle.Empty;
        Rectangle rClose = Rectangle.Empty;
        Rectangle rMax = Rectangle.Empty;
        Rectangle rMin = Rectangle.Empty;
        Rectangle rLine = Rectangle.Empty;
        Rectangle rText = Rectangle.Empty;
        Rectangle rTitle = Rectangle.Empty;
        StringFormat sf;
        StringFormat sf2;
        Bitmap backImage = null;
        public void ChangeBack(All.Class.Style.BackColors backColor)
        {
            this.Invalidate();
        }
        public void ChangeFront(All.Class.Style.FrontColors frontColor)
        {
            this.Invalidate();
        }
        public BestWindow()
        {
            InitializeComponent();
            this.Padding = new Padding(2, TitleHeight + 1, 2, 2);
            backImage = new Bitmap(this.Width, Height);
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint, true);
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }
        private void NormalWindow_Load(object sender, EventArgs e)
        {
            ChangeBack(All.Class.Style.Back);
            menuTheme.Items.Clear();
            ToolStripMenuItem tsi;
            Bitmap img;
            Color tmp;
            Enum.GetNames(typeof(All.Class.Style.FrontColors)).ToList().ForEach(
                c =>
                {
                    tmp = Color.FromArgb((int)(uint)(All.Class.Style.FrontColors)Enum.Parse(typeof(All.Class.Style.FrontColors), c));
                    img = new Bitmap(40, 10);
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        g.Clear(tmp);
                        g.DrawRectangle(new Pen(ControlPaint.Dark(tmp)), 0, 0, img.Width - 1, img.Height - 1);
                    }
                    tsi = new ToolStripMenuItem(c, img);
                    tsi.ImageScaling = ToolStripItemImageScaling.None;
                    tsi.ForeColor = tmp;
                    tsi.Click += tsi_Click;
                    menuTheme.Items.Add(tsi);
                });
            ToolStripSeparator tss = new ToolStripSeparator();
            tss.Name = "separator";
            tss.Visible = themeBack;
            menuTheme.Items.Add(tss);

            Enum.GetNames(typeof(All.Class.Style.BackColors)).ToList().ForEach(
                b =>
                {
                    tmp = Color.FromArgb((int)(((uint)(All.Class.Style.BackColors)Enum.Parse(typeof(All.Class.Style.BackColors), b)) & 0xFFFFFFFF));
                    img = new Bitmap(40, 10);
                    using (Graphics g = Graphics.FromImage(img))
                    {
                        g.Clear(tmp);
                        g.DrawRectangle(new Pen(ControlPaint.Dark(tmp)), 0, 0, img.Width - 1, img.Height - 1);
                    }
                    tsi = new ToolStripMenuItem(b, img);
                    tsi.ImageScaling = ToolStripItemImageScaling.None;
                    tsi.Click += tsiBack_Click;
                    tsi.Visible = themeBack;
                    menuTheme.Items.Add(tsi);
                });
        }
        private void ChangeSpace()
        {
            #region//各绘画矩形位置
            rTitle = new Rectangle(1, 1, this.Width - 2, TitleHeight);
            //右上角
            int tmpRight = Width - 5;
            int top = (TitleHeight - iconHeight) / 2 + 1;
            if (closeBox)
            {
                rClose = new Rectangle(tmpRight - iconHeight, top, iconHeight, iconHeight);
                tmpRight = tmpRight - iconHeight - 5;
            }
            else
            {
                rClose = Rectangle.Empty;
            }
            if (MaximizeBox)
            {
                rMax = new Rectangle(tmpRight - iconHeight, top, iconHeight, iconHeight);
                tmpRight = tmpRight - iconHeight - 5;
            }
            else
            {
                rMax = Rectangle.Empty;
            }
            if (MinimizeBox)
            {
                rMin = new Rectangle(tmpRight - iconHeight, top, iconHeight, iconHeight);
                tmpRight = tmpRight - iconHeight - 5;
            }
            else
            {
                rMin = Rectangle.Empty;
            }
            //左上角
            int tmpleft = 8;
            if (ShowIcon)
            {
                rIcon = new Rectangle(tmpleft, top, iconHeight, iconHeight);
                tmpleft = tmpleft + iconHeight + 5;
            }
            else
            {
                rIcon = Rectangle.Empty;
            }
            if (ShowIcon && theme)
            {
                rLine = new Rectangle(tmpleft, top + 2, tmpleft, iconHeight + 2);
                tmpleft = tmpleft + 1 + 7;
            }
            else
            {
                rLine = Rectangle.Empty;
            }
            if (theme)
            {
                rTheme = new Rectangle(tmpleft, top, iconHeight, iconHeight);
                tmpleft = tmpleft + iconHeight + 7;
            }
            else
            {
                rTheme = Rectangle.Empty;
            }
            rText = new Rectangle(tmpleft, 4, tmpRight - tmpleft, TitleHeight);
            #endregion
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            backImage = new Bitmap(Width, Height);
            this.Invalidate();
            base.OnSizeChanged(e);
        }
        bool isMouseDown = false;
        bool isMouseInBtn = false;
        Point oldMousePoint = Point.Empty;
        Point oldWindowPoint = Point.Empty;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            MouseDown(this,e);
            base.OnMouseDown(e);
        }
        protected override void OnDoubleClick(EventArgs e)
        {
            if (this.MaximizeBox && this.RectangleToScreen(rTitle).Contains(MousePosition))
            {
                switch (this.WindowState)
                {
                    case FormWindowState.Maximized:
                        this.WindowState = FormWindowState.Normal;
                        break;
                    case FormWindowState.Normal:
                        this.WindowState = FormWindowState.Maximized;
                        break;
                }
            }
            base.OnDoubleClick(e);
        }
        private new void  MouseDown(object sender,MouseEventArgs e)
        {
            if (rIcon.Contains(e.Location))
            {
                this.ContextMenuStrip = menuClose;
                this.menuClose.Show(this.PointToScreen(e.Location));
                return;
            }
            if (rTheme.Contains(e.Location) && theme)
            {
                this.ContextMenuStrip = menuTheme;
                this.menuTheme.Show(this.PointToScreen(e.Location));
                return;
            }
            if (rClose.Contains(e.Location))
            {
                this.Close();
                return;
            }
            if (rMax.Contains(e.Location))
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                }
                return;
            }
            if (rMin.Contains(e.Location))
            {
                this.WindowState = FormWindowState.Minimized;
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                oldMousePoint = this.PointToScreen(e.Location);
                oldWindowPoint = this.Location;
                this.Cursor = Cursors.SizeAll;
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            MouseUp(this, e);
            base.OnMouseUp(e);
        }
        private new void MouseUp(object sender,MouseEventArgs e)
        {
            isMouseDown = false;
            oldMousePoint = Point.Empty;
            oldWindowPoint = Point.Empty;
            this.Cursor = Cursors.Default;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            MouseMove(this, e);
            base.OnMouseMove(e);
        }
       private new void MouseMove(object sender,MouseEventArgs e)
       {
            Point nowMousePoint = this.PointToScreen(e.Location);
            if (rClose.Contains(e.Location) || rMax.Contains(e.Location) || rMin.Contains(e.Location) || rTheme.Contains(e.Location))
            {
                if (!isMouseInBtn)
                {
                    this.Cursor = Cursors.Hand;
                    this.Invalidate();
                }
                isMouseInBtn = true;
            }
            else
            {
                if (isMouseInBtn)
                {
                    this.Cursor = Cursors.Default;
                    this.Invalidate();
                }
                isMouseInBtn = false;
            }
            if (isMouseDown)
            {
                int x = oldWindowPoint.X + nowMousePoint.X - oldMousePoint.X;
                int y = oldWindowPoint.Y + nowMousePoint.Y - oldMousePoint.Y;
                this.Location = new Point(x, y);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            All.Class.Style.AllStyle.Add(this);
            base.OnLoad(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnClosed(e);
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                this.Close();
            }
            base.OnPreviewKeyDown(e);
        }
        private void PaintBack()
        {
            if (backImage == null)
            {
                backImage = new Bitmap(Width, Height);
            }
            ChangeSpace();
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(All.Class.Style.BackColor);
                if (sf == null)
                {
                    sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    sf2 = new StringFormat();
                    sf2.Alignment = StringAlignment.Near;
                    sf2.LineAlignment = StringAlignment.Center;
                }
                #region//3D框
                #endregion
                //标题栏
                g.FillRectangle(new SolidBrush(All.Class.Style.TitleColor),rTitle);
                //图标
                if (rIcon != Rectangle.Empty)
                {
                    g.DrawImage(this.Icon.ToBitmap(), rIcon);
                }
                if (rLine != Rectangle.Empty)
                {
                    g.DrawLine(new Pen(Color.Silver), rLine.X, rLine.Y, rLine.Width, rLine.Height);
                }
                //主题
                if (rTheme != Rectangle.Empty)
                {
                    g.DrawImage(All.Properties.Resources.Colors.ToBitmap(), rTheme);
                }
                //关闭按钮
                if (rClose != Rectangle.Empty)
                {
                    if (this.RectangleToScreen(rClose).Contains(MousePosition))
                    {
                        g.FillRectangle(new SolidBrush(All.Class.Style.BoardColor), rClose);
                        g.DrawString("r", new Font("Webdings", 10, FontStyle.Regular), new SolidBrush(All.Class.Style.BackColor), rClose, sf);
                    }
                    else
                    {
                        g.DrawString("r", new Font("Webdings", 10, FontStyle.Regular), new SolidBrush(All.Class.Style.FontColor), rClose, sf);
                    }
                }
                //最大化
                if (rMax != Rectangle.Empty)
                {
                    if (this.RectangleToScreen(rMax).Contains(MousePosition))
                    {
                        g.FillRectangle(new SolidBrush(All.Class.Style.BoardColor), rMax);
                        g.DrawString(WindowState == FormWindowState.Normal ? "1" : "2", new Font("Webdings", 10, FontStyle.Regular), new SolidBrush(All.Class.Style.BackColor), rMax, sf);
                    }
                    else
                    {
                        g.DrawString(WindowState == FormWindowState.Normal ? "1" : "2", new Font("Webdings", 10, FontStyle.Regular), new SolidBrush(All.Class.Style.FontColor), rMax, sf);
                    }
                }
                //最小化
                if (rMin != Rectangle.Empty)
                {
                    if (this.RectangleToScreen(rMin).Contains(MousePosition))
                    {
                        g.FillRectangle(new SolidBrush(All.Class.Style.BoardColor), rMin);
                        g.DrawString("0", new Font("Webdings", 10, FontStyle.Regular), new SolidBrush(All.Class.Style.BackColor), rMin, sf);
                    }
                    else
                    {
                        g.DrawString("0", new Font("Webdings", 10, FontStyle.Regular), new SolidBrush(All.Class.Style.FontColor), rMin, sf);
                    }
                }
                //标题
                if (rText != Rectangle.Empty)
                {
                    g.DrawString(this.Text, new Font("新宋体", 10, FontStyle.Bold), new SolidBrush(All.Class.Style.FontColor), rText, sf2);
                }
                g.DrawRectangle(new Pen(new SolidBrush(SystemColors.ControlDark)), 0, 0, this.Width - 1, this.Height - 1);
                g.DrawRectangle(new Pen(new SolidBrush(SystemColors.Control)), 1, 1, this.Width - 3, this.Height - 3);
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        private void menuClose_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.ContextMenuStrip = null;
        }
        private void 退出CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void 最小化MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void 最大化XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void 还原RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }
        private void menuClose_Opening(object sender, CancelEventArgs e)
        {
            最小化MToolStripMenuItem.Enabled = this.MinimizeBox;
            最大化XToolStripMenuItem.Enabled = this.MaximizeBox && this.WindowState != FormWindowState.Maximized;
            还原RToolStripMenuItem.Enabled = this.MaximizeBox && this.WindowState != FormWindowState.Normal;
        }
        private void tsi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsi=sender  as ToolStripMenuItem;
            if (tsi != null)
            {
                All.Class.Style.ChangeFront((All.Class.Style.FrontColors)Enum.Parse(typeof(All.Class.Style.FrontColors), tsi.Text));
            }
        }
        private void tsiBack_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsi = sender as ToolStripMenuItem;
            if (tsi != null)
            {
                All.Class.Style.ChangeBack((All.Class.Style.BackColors)Enum.Parse(typeof(All.Class.Style.BackColors), tsi.Text));
            }
        }
        bool drawing = false;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case All.Class.Api.WM_PAINT:
                    if (!drawing)
                    {
                        All.Class.Api.PAINTSTRUCT ps = new All.Class.Api.PAINTSTRUCT();

                        drawing = true;
                        All.Class.Api.BeginPaint(m.HWnd, ref ps);
                        this.PaintBack();
                        using (Graphics g = Graphics.FromHwnd(m.HWnd))
                        {
                            g.DrawImageUnscaled(backImage, 0, 0);
                        }
                        All.Class.Api.EndPaint(m.HWnd, ref ps);
                        drawing = false;
                        m.Result = All.Class.Api.True;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        private void BestWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
