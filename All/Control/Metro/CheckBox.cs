 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace All.Control.Metro
{
    public partial class CheckBox : System.Windows.Forms.CheckBox,All.Class.Style.ChangeTheme
    {
        const int boxSize = 12;
        public CheckBox()
        {
            InitializeComponent();
        }
        public void ChangeFront(All.Class.Style.FrontColors color)
        {
            this.ForeColor = All.Class.Style.FontColor;
            this.Invalidate();
        }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.ForeColor = All.Class.Style.FontColor;
            this.BackColor = All.Class.Style.BackColor;
            this.Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor = Cursors.Hand;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Default;
            base.OnMouseLeave(e);
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
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            All.Class.GDIHelp.Init(e.Graphics);
            e.Graphics.Clear(this.BackColor);
            e.Graphics.DrawString(this.Text.Replace("&", ""), this.Font, All.Class.Style.FontBrush,
                new Rectangle(4 + boxSize, 1, this.Width - 4, this.Height), All.Class.GDIHelp.StringFormat(this.TextAlign));
            e.Graphics.DrawRectangle(All.Class.Style.BoardPen,
                 2, this.Height / 2 - boxSize / 2, boxSize, boxSize);
            switch (this.CheckState)
            {
                case System.Windows.Forms.CheckState.Checked:
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    e.Graphics.DrawLine(new Pen(All.Class.Style.BoardColor, 1.6f), 3, this.Height / 2, 1 + boxSize / 2, this.Height / 2 + boxSize / 2 - 2);
                    e.Graphics.DrawLine(new Pen(All.Class.Style.BoardColor, 1.6f), 1 + boxSize / 2, this.Height / 2 + boxSize / 2 - 2, 1 + boxSize, this.Height / 2 - boxSize / 2 + 2);
                    break;
                case System.Windows.Forms.CheckState.Indeterminate:
                    //e.Graphics.FillRectangle(new SolidBrush(All.Class.Style.BoardColor),
                    //    new RectangleF(this.Height / 3f, this.Height / 3f, this.Height / 3f, this.Height / 3f));
                    e.Graphics.FillRectangle(new SolidBrush(All.Class.Style.BoardColor),
                        new RectangleF(2 + 2.5f, this.Height / 2 - boxSize / 2 + 2.5f, boxSize - 5, boxSize - 5));
                    break;
            }
        }
    }
}
