using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace All.Control.Mine
{
    public partial class Num : System.Windows.Forms.Control
    {
        float value = 8888.88f;
        /// <summary>
        /// 显示值
        /// </summary>
        [Description("显示值")]
        [Category("Shuai")]
        public float Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        bool point = true;
        /// <summary>
        /// 是否显示小数点
        /// </summary>
        [Description("是否显示小数点")]
        [Category("Shuai")]
        public bool Point
        {
            get { return point; }
            set { point = value; }
        }
        bool shadow = true;
        /// <summary>
        /// 是否有影子
        /// </summary>
        [Description("是否有影子")]
        [Category("Shuai")]
        public bool Shadow
        {
            get { return shadow; }
            set { shadow = value; }
        }
        int thickness = 4;
        /// <summary>
        /// 边框粗细
        /// </summary>
        [Description("边框粗细")]
        [Category("Shuai")]
        public int Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }
        int maxLen = 6;
        /// <summary>
        /// 最长有效数字
        /// </summary>
        [Description("最长有效数字")]
        [Category("Shuai")]
        public int MaxLen
        {
            get { return maxLen; }
            set { maxLen = value; }
        }
        bool symbol = false;
        /// <summary>
        /// 是否有符号
        /// </summary>
        [Description("是否有符号")]
        [Category("Shuai")]
        public bool Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        [Description("字体颜色")]
        [Category("Shuai")]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }
        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色")]
        [Category("Shuai")]
        public override System.Drawing.Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        Bitmap backImage;
        public Num()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            Init();
            base.OnSizeChanged(e);
        }
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (backImage == null)
            {
                Init();
            }
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(this.BackColor);

            }
            e.Graphics.DrawImageUnscaledAndClipped(backImage, this.DisplayRectangle);
        }
        private void Init()
        {
            //if (seven != null)
            //{
            //    for (int i = 0; i < seven.Length; i++)
            //    {
            //        seven[i].Dispose();
            //    }
            //}
            //seven = new Seven[formatValue.Length];
            //for (int i = 0; i < seven.Length; i++)
            //{
            //    seven[i] = new Seven();
            //    seven[i].Width = this.Width / seven.Length - 5;
            //    seven[i].Height = this.Height;
            //    seven[i].Left = this.Width / seven.Length * i;
            //    seven[i].FontSize = this.Height / 10;
            //    seven[i].FontSpace = this.Height / 30;
            //    seven[i].Top = 0;
            //    seven[i].Shardow = false;
            //    switch (formatValue.Substring(i, 1))
            //    {
            //        case "y":
            //        case "M":
            //        case "d":
            //        case "H":
            //        case "m":
            //        case "s":
            //            seven[i].Simplor = Seven.simplorList.Value;
            //            break;
            //        case "-":
            //            seven[i].Simplor = Seven.simplorList.Del;
            //            break;
            //        case ":":
            //            seven[i].Simplor = Seven.simplorList.DoublePoint;
            //            break;
            //        default:
            //            seven[i].Simplor = Seven.simplorList.Null;
            //            break;
            //    }
            //    this.Controls.Add(seven[i]);
            //}
        }

    }
}
