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
    public partial class RadioButton : System.Windows.Forms.RadioButton,All.Class.Style.ChangeTheme
    {
        public RadioButton()
        {
            InitializeComponent();
        }

        public void ChangeFront(All.Class.Style.FrontColors color)
        {
            this.ForeColor = All.Class.Style.FontColor;
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
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.ForeColor = All.Class.Style.FontColor;
            this.BackColor = All.Class.Style.BackColor;
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
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //base.OnPaint(e);
            All.Class.GDIHelp.Init(e.Graphics);
            e.Graphics.Clear(this.BackColor);
            e.Graphics.DrawString(this.Text.Replace("&", ""), this.Font, All.Class.Style.FontBrush,
                new RectangleF(this.Height * 3 / 4f, 1, this.Width - this.Height * 3f / 4f, this.Height-1), All.Class.GDIHelp.StringFormat(this.TextAlign));
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.DrawEllipse(new Pen(All.Class.Style.BoardColor, 1.6f),
                0, this.Height / 6f, this.Height * 2f / 3, this.Height * 2f / 3);
            if (this.Checked)
            {
                e.Graphics.FillEllipse(new SolidBrush(All.Class.Style.BoardColor),
                    this.Height / 6f, this.Height /3f, this.Height / 3f, this.Height / 3f);
            }
        }
    }
}
