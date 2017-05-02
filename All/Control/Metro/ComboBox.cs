using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace All.Control.Metro
{
    public partial class ComboBox : System.Windows.Forms.ComboBox,All.Class.Style.ChangeTheme
    {
        public ComboBox()
        {
            InitializeComponent();
            this.BackColor = All.Class.Style.BackColor;
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
        bool drawing = false;
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case All.Class.Api.WM_PAINT:
                case All.Class.Api.WM_CTLCOLOREDIT:
                    if (!drawing)
                    {
                        drawing = true;
                        All.Class.Api.PAINTSTRUCT ps = new All.Class.Api.PAINTSTRUCT();
                        All.Class.Api.BeginPaint(m.HWnd, ref ps);
                        this.WmPaint(ref m);
                        All.Class.Api.EndPaint(m.HWnd, ref ps);
                        m.Result = All.Class.Api.True;
                        drawing = false;
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
        private void WmPaint(ref Message m)
        {
            Graphics g = Graphics.FromHwnd(m.HWnd);
            All.Class.GDIHelp.Init(g);
            g.Clear(this.BackColor);
            switch (this.DropDownStyle)
            {
                case ComboBoxStyle.DropDown:
                    All.Class.GDIHelp.InitAntiAlias(g);
                    g.FillRectangle(new SolidBrush(System.Windows.Forms.ControlPaint.LightLight(All.Class.Style.TitleColor)), this.Width - this.Height, 0, this.Height, this.Height);
                    g.DrawLines(new Pen(All.Class.Style.BoardColor, 2), new Point[] { new Point(this.Width - this.Height * 3 / 4, this.Height * 3 / 8), new Point(this.Width - this.Height / 2, this.Height * 5 / 8), new Point(this.Width - this.Height * 1 / 4, this.Height * 3 / 8) });
                    break;
                case ComboBoxStyle.DropDownList:
                    All.Class.GDIHelp.InitAntiAlias(g);
                    g.DrawLines(new Pen(All.Class.Style.BoardColor, 2), new Point[] { new Point(this.Width - this.Height * 3 / 4, this.Height * 3 / 8), new Point(this.Width - this.Height / 2, this.Height * 5 / 8), new Point(this.Width - this.Height * 1 / 4, this.Height * 3 / 8) });
                    if (this.Text != null)
                    {
                        g.DrawString(this.Text, this.Font, All.Class.Style.FontBrush, this.ClientRectangle, All.Class.GDIHelp.StringFormat(ContentAlignment.MiddleLeft));
                    }
                    break;
                case ComboBoxStyle.Simple:
                    break;
            }
            g.DrawRectangle(All.Class.Style.BoardPen, this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
        }
    }
}
