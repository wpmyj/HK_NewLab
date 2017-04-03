using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Control.Mine
{
    [DefaultEvent("CheckValueChange")]
    public partial class TabTitleButton : System.Windows.Forms.Control,All.Class.Style.ChangeTheme
    {
        /// <summary>
        /// 选中属性更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        public delegate void CheckValueChangeHandle(TabTitleButton sender, bool value);
        /// <summary>
        /// 选中属性更改事件
        /// </summary>
        [Category("Shuai")]
        [Description("选中属性更改事件")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event CheckValueChangeHandle CheckValueChange;

        bool check = false;
        /// <summary>
        /// 当前控件是否被选中
        /// </summary>
        [Category("Shuai")]
        [Description("当前控件是否被选中")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool Check
        {
            get { return check; }
            set {
                if (check != value)
                {
                    check = value;
                    this.Invalidate();
                    if (CheckValueChange != null)
                    {
                        CheckValueChange(this, value);
                    }
                }
            }
        }
        protected override void OnTextChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnTextChanged(e);
        }
        StringFormat sf;
        Bitmap backImage;
        bool mouseInButton = false;
        public TabTitleButton()
        {
            if (sf == null)
            {
                sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
            }
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint, true);
            this.MinimumSize = new Size(10, 10);
        }
        public void ChangeBack(All.Class.Style.BackColors backColor)
        {
            this.Invalidate();
        }
        public void ChangeFront(All.Class.Style.FrontColors frontColor)
        {
            this.Invalidate();
        }
        protected override void OnCreateControl()
        {
            All.Class.Style.AllStyle.Add(this);
            base.OnCreateControl();
        }
        protected override void DestroyHandle()
        {
            All.Class.Style.AllStyle.Remove(this);
            base.DestroyHandle();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            if (!mouseInButton)
            {
                mouseInButton = true;
                this.Invalidate();
            }
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (mouseInButton)
            {
                mouseInButton = false;
                this.Invalidate();
            }
            base.OnMouseLeave(e);
        }
        protected override void OnClick(EventArgs e)
        {
            this.Check = true;
            base.OnClick(e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            Init();
            base.OnSizeChanged(e);
        }
        private void Init()
        {
            backImage = new Bitmap(Width, Height);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (backImage == null)
            {
                Init();
            }
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(All.Class.Style.BackColor);
                if (!check)
                {
                    g.DrawString(this.Text,new Font(this.Font.FontFamily,this.Font.Size,FontStyle.Regular), new SolidBrush(All.Class.Style.FontColor), this.ClientRectangle, sf);
                }
                else
                {
                    g.DrawString(this.Text, new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold), new SolidBrush(All.Class.Style.BoardColor), this.ClientRectangle, sf);
                }
                if (mouseInButton)
                {
                    g.FillRectangle(new SolidBrush(All.Class.Style.BoardColor), new Rectangle(Width / 10, Height - 4, Width * 4 / 5, 4));
                }
            }
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            base.OnPaint(e);
        }
    }
}
