using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Control.Metro
{
    public partial class Panel : System.Windows.Forms.Panel, All.Class.Style.ChangeTheme
    {
        public Panel(IContainer contain)
        {
            contain.Add(this);
            InitializeComponent();
        }
        Bitmap backImg;
        public Panel()
        {
            InitializeComponent();
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

        protected override void OnHandleCreated(EventArgs e)
        {
            All.Class.Style.AllStyle.Add(this);
            ChangeBack(All.Class.Style.Back);
            base.OnHandleCreated(e);
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnHandleDestroyed(e);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            Init();
            this.Invalidate();
            base.OnSizeChanged(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (backImg == null)
            {
                Init();
            }
            if (backImg == null)
            { return; }
            using (Graphics g = Graphics.FromImage(backImg))
            {
                g.Clear(this.BackColor);
                if (this.BorderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
                {
                    g.DrawRectangle(new System.Drawing.Pen(All.Class.Style.BoardColor, 2), new System.Drawing.Rectangle(0, 0, Width - 2, Height - 2));
                }
            }
            e.Graphics.DrawImageUnscaled(backImg, 0, 0);
            base.OnPaint(e);
        }
        private void Init()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                backImg = new Bitmap(this.Width, this.Height);
            }
        }
    }
}
