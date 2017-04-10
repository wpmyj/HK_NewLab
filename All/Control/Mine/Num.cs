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
        Seven[] seven;
        float value = 8888.88f;
        /// <summary>
        /// 显示值
        /// </summary>
        [Description("显示值")]
        [Category("Shuai")]
        public float Value
        {
            get { return this.value; }
            set { this.value = value; flush(); }
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
            set { point = value; Init(); }
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
            set
            {
                shadow = value;
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].Shardow = value;
                }
            }
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
            set { maxLen = value; Init(); }
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
            set { symbol = value; flush(); }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        [Description("字体颜色")]
        [Category("Shuai")]
        public override System.Drawing.Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].ForeColor = value;
                }
            }
        }
        /// <summary>
        /// 背景色
        /// </summary>
        [Description("背景色")]
        [Category("Shuai")]
        public override System.Drawing.Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].BackColor = value;
                }
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
        private void flush()
        { 
        }
        private void Init()
        {
            if (seven != null)
            {
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].Dispose();
                }
            }
            seven = new Seven[maxLen + 1 + (point ? 1 : 0)];
            for (int i = 0; i < seven.Length; i++)
            {
                seven[i] = new Seven();
                seven[i].Width = this.Width / seven.Length - 5;
                seven[i].Height = this.Height;
                seven[i].Left = this.Width / seven.Length * i;
                seven[i].FontSize = this.Height / 10;
                seven[i].FontSpace = this.Height / 30;
                seven[i].Top = 0;
                seven[i].Shardow = this.shadow;
                seven[i].BackColor = this.BackColor;
                seven[i].ForeColor = this.ForeColor;
                this.Controls.Add(seven[i]);
            }
            flush();
        }

    }
}
