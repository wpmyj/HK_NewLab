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
    public partial class MetroControl : UserControl,All.Class.Style.ChangeTheme
    {
        public MetroControl()
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
            //this.Invalidate();
        }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.BackColor = All.Class.Style.BackColor;
        }

    }
}
