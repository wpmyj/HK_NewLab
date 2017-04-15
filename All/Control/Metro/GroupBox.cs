using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Control.Metro
{
    public partial class GroupBox : System.Windows.Forms.GroupBox,All.Class.Style.ChangeTheme
    {
        float fontHeight = 10;
        /// <summary>
        /// 获取或设置控件显示的文字的字体
        /// </summary>
        [Description("获取或设置控件显示的文字的字体")]
        [Category("外观")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                fontHeight = All.Class.Num.GetFontHeight(this.Font);
            }
        }
        Bitmap backImage = null;
        public GroupBox()
        {
            InitializeComponent();
            this.MinimumSize = new Size(10, 10);
        }

        public GroupBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            this.MinimumSize = new Size(10, 10);
        }
        public void ChangeFront(All.Class.Style.FrontColors color)
        {
            this.Invalidate();
        }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.BackColor = All.Class.Style.BackColor;
            this.ForeColor = All.Class.Style.FontColor;
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            Init();
            base.OnSizeChanged(e);
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            ChangeBack(All.Class.Style.Back);
            All.Class.Style.AllStyle.Add(this);
            base.OnHandleCreated(e);
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnHandleDestroyed(e);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (backImage == null)
            {
                Init();
            }
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.Clear(this.BackColor);
                g.DrawRectangle(All.Class.Style.BoardPen, 0, fontHeight / 2, this.Width - 1, this.Height - fontHeight / 2 - 1);
                g.FillRectangle(All.Class.Style.BackBrush, 8, 0, All.Class.Num.GetFontWidth(this.Font, this.Text), fontHeight);
                g.DrawString(this.Text, this.Font, All.Class.Style.FontBrush, 8, 0);
            }
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
        }
        private void Init()
        {
            fontHeight = All.Class.Num.GetFontHeight(this.Font);
            backImage = new Bitmap(this.Width, this.Height);
        }
    }
}
