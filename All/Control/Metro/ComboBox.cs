using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace All.Control.Metro
{
    public partial class ComboBox : System.Windows.Forms.ComboBox
    {
        public ComboBox()
        {
            InitializeComponent();
        }
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            //if (e.Index >= 0 && e.Index < this.Items.Count)
            //{
            //    e.Graphics.FillRectangle(new System.Drawing.SolidBrush(All.Class.Style.BackColor),e.Bounds);
            //    e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, new System.Drawing.SolidBrush(All.Class.Style.FontColor), e.Bounds);
            //}
            base.OnDrawItem(e);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(new System.Drawing.SolidBrush(All.Class.Style.BackColor), this.Bounds);
            //e.Graphics.DrawRectangle(new System.Drawing.Pen(All.Class.Style.BoardColor, 2), this.ClientRectangle);
            base.OnPaint(e);
        }
    }
}
