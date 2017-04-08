using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
namespace All.Control.Mine
{
    public partial class PicturePlayer : System.Windows.Forms.Control
    {
        uint change = 1000;
        /// <summary>
        /// 图片切换时间
        /// </summary>
        public uint Change
        {
            get { return change; }
            set { change = value; }
        }
        uint delay = 3000;
        /// <summary>
        /// 画面停留时间
        /// </summary>
        public uint Delay
        {
            get { return delay; }
            set { delay = value; }
        }
        bool exit = false;
        Thread thFlush;
        public PicturePlayer()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
        private void Start()
        {
            if (!All.Class.Environment.IsDesignMode)
            {
                thFlush = new Thread(() => Flush())
                {
                    IsBackground = true
                };
                thFlush.Start();
            }
        }
        private void Stop()
        {
            if (thFlush != null)
            {
                thFlush.Join(50);
                thFlush.Abort();
                thFlush = null;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
        private void Flush()
        {
            while (!exit)
            {

                Thread.Sleep(10);
            }
        }
    }
}
