using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace All.Control.Metro
{
    public partial class ListBox : System.Windows.Forms.ListBox,All.Class.Style.ChangeTheme
    {
        public ListBox()
        {
            InitializeComponent();
        }
        public void ChangeFront(All.Class.Style.FrontColors color)
        { }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.BackColor = All.Class.Style.BackColor;
            this.ForeColor = All.Class.Style.FontColor;
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
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
            e.Graphics.DrawRectangle(All.Class.Style.BoardPen, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}
