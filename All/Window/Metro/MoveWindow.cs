using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Window
{
    /// <summary>
    /// 鼠标显示处窗体
    /// </summary>
    public partial class MoveWindow : BaseWindow
    {
        /// <summary>
        /// 播放屏幕
        /// </summary>
        public Screen PlayScreen
        { get; set; }
        public MoveWindow()
        {
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Normal;
            InitializeComponent();
        }
        
        private void MoveWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
