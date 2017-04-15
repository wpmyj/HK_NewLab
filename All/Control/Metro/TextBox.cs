using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Control.Metro
{
    public partial class TextBox : System.Windows.Forms.TextBox
    {
        public TextBox()
        {
            InitializeComponent();
            this.BorderStyle = BorderStyle.None;
        }
        protected override void WndProc(ref Message m)
        {//TextBox是由系统进程绘制，重载OnPaint方法将不起作用

            base.WndProc(ref m);
            if (m.Msg == All.Class.Api.WM_PAINT || m.Msg == All.Class.Api.WM_CTLCOLOREDIT)
            {
                WmPaint(ref m);
            }
        }
        private void WmPaint(ref Message m)
        {
            Graphics g = Graphics.FromHwnd(base.Handle);
            g.DrawRectangle(All.Class.Style.BoardPen, this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
        }
    }
}
