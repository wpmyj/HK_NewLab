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
    public partial class MainWindow : BaseWindow
    {
        int designWidth = 800;
        /// <summary>
        /// 设计大小,用于自动缩放时的原始宽度
        /// </summary>
        [Description("设计大小,用于自动缩放时的原始宽度")]
        [Category("Shuai")]
        public int DesignWidth
        {
            get { return designWidth; }
            set { designWidth = value; }
        }
        int designHeight = 600;
        /// <summary>
        /// 设计大小,用于自动缩放时的原始高度
        /// </summary>
        [Description("设计大小,用于自动缩放时的原始高度")]
        [Category("Shuai")]
        public int DesignHeight
        {
            get { return designHeight; }
            set { designHeight = value; }
        }
        /// <summary>
        /// 缩放模式
        /// </summary>
        [Description("缩放模式")]
        public enum ResizeModes
        {
            // 摘要: 
            //     控件沿窗体的矩形工作区顶部左对齐。
            None = 0,
            //
            // 摘要: 
            //     控件在窗体的矩形工作区中居中显示。
            Center = 2,
            ////
            //// 摘要: 
            ////     控件在窗体的矩形工作区拉伸。
            //Stretch = 3,
            //
            // 摘要: 
            //     控件在窗体的矩形工作区中放大。
            Zoom = 4
        }
        /// <summary>
        /// 缩放模式
        /// </summary>
        [Description("缩放模式")]
        [Category("Shuai")]
        public ResizeModes ResizeMode
        { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            ReSetLocation(this.Controls);
        }
        private void ReSetLocation(System.Windows.Forms.Control.ControlCollection controls)
        {
            switch (ResizeMode)
            {
                case ResizeModes.None:
                    break;
                case ResizeModes.Center://使控件整体居中
                    int x = 10000, y = 10000, width = 0, height = 0;
                    foreach (System.Windows.Forms.Control c in controls)
                    {
                        if (c.Left < x)
                            x = c.Left;
                        if (c.Top < y)
                            y = c.Top;
                        if ((c.Left + c.Width) > width)
                            width = c.Left + c.Width;
                        if ((c.Top + c.Height) > height)
                            height = c.Top + c.Height;
                    }
                    if (x > 0 && y > 0 && width > 0 && height > 0)
                    {
                        width = this.Width / 2 - width / 2 + x / 2;
                        height = this.Height / 2 - height / 2 + y / 2;
                    }
                    foreach (System.Windows.Forms.Control c in controls)
                    {
                        c.Left += width - x;
                        c.Top += height - y;
                    }
                    break;
                //case ResizeModes.Stretch:
                //    foreach (System.Windows.Forms.Control c in controls)
                //    {
                //        c.Left = (int)((float)c.Left * this.Width / designWidth);
                //        c.Top = (int)((float)c.Top * this.Height / designHeight);
                //        ReSetLocation(c.Controls);
                //    }
                //    break;
                case ResizeModes.Zoom://使所有控件缩放
                    foreach (System.Windows.Forms.Control c in controls)
                    {
                        c.Left = (int)((float)c.Left * this.Width / designWidth);
                        c.Top = (int)((float)c.Top * this.Height / designHeight);
                        c.Width = (int)((float)c.Width * this.Width / designWidth);
                        c.Height = (int)((float)c.Height * this.Height / designHeight);
                        c.Font = new System.Drawing.Font(c.Font.FontFamily, c.Font.Size * this.Width / designWidth);
                        ReSetLocation(c.Controls);
                    }
                    break;
            }
        }
    }
}
