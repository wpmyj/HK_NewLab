using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
namespace All.Control
{
    public partial class Light : System.Windows.Forms.Control
    {
        Color ledColor = Color.Red;
        /// <summary>
        /// 图形颜色
        /// </summary>
        [Description("图形颜色")]
        [Category("Shuai")]
        public Color LedColor
        {
            get { return ledColor; }
            set { ledColor = value; this.Invalidate(); }
        }
        public Light()
        {
            this.BackColor = SystemColors.Control;
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
        /// <summary>
        /// 设置图形颜色 ,跨线程方法
        /// </summary>
        /// <param name="color"></param>
        public void SetLedColor(Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Color>(SetLedColor), color);
            }
            else
            {
                this.LedColor = color;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Width < this.Height)
            {
                this.Height = this.Width;
            }
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(Width / 2 - Height / 2, 0, Height, Height);
            this.Region = new Region(gp);

            base.OnSizeChanged(e);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            int left = Width / 2 - Height / 2;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            GraphicsPath gp = new GraphicsPath();
            PathGradientBrush pgb;

            gp.AddEllipse(left, 0, Height, Height);
            pgb = new PathGradientBrush(gp);
            pgb.CenterColor = Color.White;// System.Windows.Forms.ControlPaint.LightLight(BackColor);
            pgb.SurroundColors = new Color[] { ledColor };
            pgb.CenterPoint = new PointF(left + Height * 1 / 3.0f, Height * 1 / 3.0f);
            e.Graphics.FillEllipse(pgb, left + 2, 2, Height - 4, Height - 4);

            gp.Dispose();
            pgb.Dispose();

            base.OnPaint(e);
        }
    }
}
