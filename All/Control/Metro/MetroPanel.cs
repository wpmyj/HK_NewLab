using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Control.Metro
{
    public partial class MetroPanel :System.Windows.Forms.UserControl, All.Class.Style.ChangeTheme
    {

        public MetroPanel()
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
            if (this.BorderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
            {
                e.Graphics.DrawRectangle(All.Class.Style.BoardPen, new System.Drawing.Rectangle(0, 0, Width - 3, Height - 3));
            }
            base.OnPaint(e);
        }
    }
}
