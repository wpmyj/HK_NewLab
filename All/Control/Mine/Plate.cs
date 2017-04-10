using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
namespace All.Control.Mine
{
    public partial class Plate : System.Windows.Forms.Control
    {
        double value = 5;
        /// <summary>
        /// 当前指示值
        /// </summary>
        [Description("当前指示值")]
        [Category("Shuai")]
        public double Value
        {
            get { return this.value; }
            set
            {
                if (value < min)
                {
                    this.value = min;
                }
                else if (value > max)
                {
                    this.value = max;
                }
                else
                {
                    this.value = value;
                }

                this.Invalidate();
            }
        }
        int partValue1 = 3;
        /// <summary>
        /// 第一分段点,0到part之间的整数值
        /// </summary>
        [Description("第一分段点,0到part之间的整数值")]
        [Category("Shuai")]
        public int PartValue1
        {
            get { return partValue1; }
            set
            {
                if (value < 1)
                {
                    partValue1 = 1;
                }
                else if (value > partValue2)
                {
                    partValue1 = partValue2;
                }
                else
                {
                    partValue1 = value;
                }
                drawAll = true;
                this.Invalidate();
            }
        }
        int partValue2 = 6;
        /// <summary>
        /// 第二分段点,0到part之间的整数值
        /// </summary>
        [Description("第二分段点,0到part之间的整数值")]
        [Category("Shuai")]
        public int PartValue2
        {
            get { return partValue2; }
            set
            {
                if (value < partValue1)
                {
                    partValue2 = partValue1;
                }
                else if (value > part)
                {
                    partValue2 = part;
                }
                else
                {
                    partValue2 = value;
                }
                drawAll = true;
                this.Invalidate();
            }
        }
        Color colorPart1 = Color.Yellow;
        /// <summary>
        /// 第一分段色,0到第一分段点之间的颜色
        /// </summary>
        [Description("第一分段色,0到第一分段点之间的颜色")]
        [Category("Shuai")]
        public Color ColorPart1
        {
            get { return colorPart1; }
            set
            {
                colorPart1 = value;
                this.Invalidate();
            }
        }
        Color colorPart2 = Color.Green;
        /// <summary>
        /// 第二分段色,第一分段点到第二分段点之间的颜色
        /// </summary>
        [Description("第二分段色,第一分段点到第二分段点之间的颜色")]
        [Category("Shuai")]
        public Color ColorPart2
        {
            get { return colorPart2; }
            set
            {
                colorPart2 = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        Color colorPart3 = Color.Red;
        /// <summary>
        /// 第三分段色,第二分段点到终点之间的颜色
        /// </summary>
        [Description("第三分段色,第二分段点到终点之间的颜色")]
        [Category("Shuai")]
        public Color ColorPart3
        {
            get { return colorPart3; }
            set
            {
                colorPart3 = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        string title = "显示值";
        /// <summary>
        /// 显示标题
        /// </summary>
        [Description("显示标题")]
        [Category("Shuai")]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        int part = 10;
        /// <summary>
        /// 整体分段
        /// </summary>
        [Description("整体分段")]
        [Category("Shuai")]
        public int Part
        {
            get { return part; }
            set
            {
                part = Math.Max(1, value);
                drawAll = true;
                this.Invalidate();
            }
        }
        double max = 10;
        /// <summary>
        /// 显示最大值
        /// </summary>
        [Description("显示最大值")]
        [Category("Shuai")]
        public double Max
        {
            get { return max; }
            set
            {
                max = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        double min = 0;
        /// <summary>
        /// 显示最小值
        /// </summary>
        [Description("显示最小值")]
        [Category("Shuai")]
        public double Min
        {
            get { return min; }
            set
            {
                min = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        Color plateColor = Color.Blue;
        /// <summary>
        /// 表盘颜色
        /// </summary>
        [Description("表盘颜色")]
        [Category("Shuai")]
        public Color PlateColor
        {
            get { return plateColor; }
            set
            {
                plateColor = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        Color arrowColor = Color.Red;
        /// <summary>
        /// 指针颜色
        /// </summary>
        [Description("指针颜色")]
        [Category("Shuai")]
        public Color ArrowColor
        {
            get { return arrowColor; }
            set
            {
                arrowColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 字体
        /// </summary>
        [Description("字体")]
        [Category("Shuai")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        [Description("字体颜色")]
        [Category("Shuai")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                drawAll = true;
                this.Invalidate();
            }
        }
        string format = "F3";
        /// <summary>
        /// 数据格式化
        /// </summary>
        [Description("数据格式化")]
        [Category("Shuai")]
        public string Format
        {
            get { return format; }
            set { format = value; this.Invalidate(); }
        }
        bool drawAll = false;
        Bitmap backImage;
        Bitmap tmpBackImage;
        public Plate()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            this.Height = this.Width;
            backImage = new Bitmap(this.Width, this.Height);
            tmpBackImage = new Bitmap(this.Width, this.Height);
            drawAll = true;
            DrawBack();
            base.OnSizeChanged(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawBack();
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            base.OnPaint(e);
        }
        /// <summary>
        /// 画表盘,改变此函数,可实际多表盘的风格
        /// </summary>
        private void DrawPlate()
        {
            if (tmpBackImage == null)
            {
                tmpBackImage = new Bitmap(this.Width, this.Height);
            }
            using (Graphics g = Graphics.FromImage(tmpBackImage))
            {
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(BackColor);

                //外层彩圏
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, Width - 1, Height - 1);
                PathGradientBrush pgb = new PathGradientBrush(gp);
                pgb.CenterColor = Color.White;
                pgb.SurroundColors = new Color[] { plateColor };
                pgb.CenterPoint = new PointF(30, 30);
                g.FillEllipse(pgb, 0, 0, Width - 1, Height - 1);
                //白框
                gp.ClearMarkers();
                gp.AddEllipse(10, 10, Width - 1 - 20, Height - 1 - 20);
                pgb = new PathGradientBrush(gp);
                pgb.CenterColor = Color.Black;
                pgb.SurroundColors = new Color[] { Color.White };
                g.FillEllipse(pgb, 10, 10, Width - 1 - 20, Height - 1 - 20);
                //黑圈
                g.FillEllipse(new SolidBrush(Color.Black), 20, 20, Width - 1 - 40, Height - 1 - 40);
                //灰背景
                gp.ClearMarkers();
                gp.AddEllipse(10, 10, Width - 1 - 20, Height - 1 - 20);
                pgb = new PathGradientBrush(gp);
                pgb.CenterColor = System.Windows.Forms.ControlPaint.LightLight(plateColor);
                pgb.SurroundColors = new Color[] { Color.White };
                pgb.CenterPoint = new PointF(30, 30);
                g.FillEllipse(pgb, 22, 22, Width - 1 - 44, Height - 1 - 44);
                //刻度

                float tmpJiaoDu = 270.00f / part;
                float tmpR = Width / 2.00f - 30;
                for (int i = 0; i <= part; i++)
                {
                    g.DrawLine(new Pen(Color.Black, 1), new PointF(Width / 2.0f, Height / 2.0f),
                        new PointF((float)(Width / 2.0 - Math.Sin(Math.PI * (45 + i * tmpJiaoDu) / 180) * tmpR),
                            (float)(Height / 2.0 + Math.Cos(Math.PI * (45 + i * tmpJiaoDu) / 180) * tmpR)));
                }
                //刻度上的弧
                g.DrawArc(new Pen(Color.Black, 2), 37, 37, Width - 1 - 74, Height - 1 - 74, 135, 225);
                g.DrawArc(new Pen(Color.Black, 2), 37, 37, Width - 1 - 74, Height - 1 - 74, 0, 45);
                //报警颜色段
                g.FillPie(new SolidBrush(colorPart1), 38, 38, Width - 1 - 76, Height - 1 - 76, 135, tmpJiaoDu * partValue1);
                g.FillPie(new SolidBrush(colorPart2), 38, 38, Width - 1 - 76, Height - 1 - 76, (135 + (int)(tmpJiaoDu * partValue1)) % 360, tmpJiaoDu * (partValue2 - partValue1));
                g.FillPie(new SolidBrush(colorPart3), 38, 38, Width - 1 - 76, Height - 1 - 76, (135 + (int)(tmpJiaoDu * partValue2)) % 360, tmpJiaoDu * (part - partValue2));

                //灰背景,把多余的报警颜色段覆盖
                g.FillEllipse(pgb, 53, 53, Width - 1 - 106, Height - 1 - 106);

                //画刻度上的文字
                string tmpStr = "";
                Font tmpFont = new Font("黑体", 10, FontStyle.Bold);
                for (int i = 0; i <= part; i++)
                {
                    g.TranslateTransform(Width / 2.0f, Height / 2.0f);//把g的中心移动到图像中间
                    g.RotateTransform(i * tmpJiaoDu + 225);//把g旋转一定角度
                    tmpStr = string.Format("{0}", (max - min) * i / part + min);
                    g.DrawString(tmpStr, tmpFont, new SolidBrush(Color.Black), -tmpStr.Length * tmpFont.GetHeight() / 4.0f, 39 - Height / 2.0f);
                    g.RotateTransform(360 - i * tmpJiaoDu - 225);
                    g.TranslateTransform(-Width / 2.0f, -Height / 2.0f);
                }
                //画下方数据区
                gp = new GraphicsPath();
                tmpR = Width / 2.00f - 21;
                gp.AddArc(21, 21, Width - 1 - 42, Height - 1 - 42, 45, 90);
                gp.AddLine((float)(Width / 2.0f - Math.Sin(45 * Math.PI / 180) * tmpR), (float)(Height / 2.0f + Math.Cos(45 * Math.PI / 180) * tmpR),
                        (float)(Width / 2.0f + Math.Sin(45 * Math.PI / 180) * tmpR), (float)(Height / 2.0f + Math.Cos(45 * Math.PI / 180) * tmpR));
                gp.CloseFigure();
                pgb = new PathGradientBrush(gp);
                pgb.CenterColor = Color.White;
                pgb.SurroundColors = new Color[] { System.Windows.Forms.ControlPaint.Light(plateColor) };
                g.FillPath(pgb, gp);

                //画标题
                tmpStr = title;
                //tmpFont = new Font("宋体", 12, FontStyle.Bold);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                //sf.LineAlignment = StringAlignment.Center;
                g.DrawString(tmpStr, Font, new SolidBrush(ForeColor), new RectangleF(0, Height * 0.65f, Width, tmpFont.GetHeight()), sf);
                gp = new GraphicsPath();


                sf.Dispose();
                pgb.Dispose();
                gp.Dispose();
            }
        }
        /// <summary>
        /// 画指针
        /// </summary>
        private void DrawBack()
        {
            if (backImage == null)
            {
                backImage = new Bitmap(this.Width, this.Height);
            }
            using (Graphics g = Graphics.FromImage(backImage))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                if (drawAll)
                {
                    DrawPlate();
                    drawAll = false;
                }
                g.DrawImageUnscaled(tmpBackImage, 0, 0);
                ////指针
                GraphicsPath gp = new GraphicsPath();
                float tmpR = Width / 2.0f - 58;
                float tmpJiaoDu = (float)((value - min) * 270 / (max - min));

                gp.AddArc(58, 58, Width - 1 - 116, Height - 1 - 116, (int)(135 + tmpJiaoDu) % 360, 1);

                //gp.AddLine((float)(Width / 2.0f - Math.Sin(Math.PI * (46 + tmpJiaoDu) / 180) * tmpR), (float)(Height / 2.0f + Math.Cos(Math.PI * (46 + tmpJiaoDu) / 180) * tmpR),
                //    Width / 2.0f + (float)(8 * Math.Cos(Math.PI * (495 - tmpJiaoDu) / 180)), Height / 2.0f - (float)(8 * Math.Sin(Math.PI * (495 - tmpJiaoDu) / 180)));

                gp.AddArc(Width / 2 - 5, Height / 2 - 5, 10, 10, (int)(225 + tmpJiaoDu) % 360, 180);

                //gp.AddLine((float)(Width / 2.0f + Math.Sin(Math.PI * (44 + tmpJiaoDu) / 180) * tmpR), (float)(Height / 2.0f - Math.Cos(Math.PI * (44 + tmpJiaoDu) / 180) * tmpR),
                //    Width / 2.0f - (float)(8 * Math.Sin(Math.PI * (270 + tmpJiaoDu) / 180)), Height / 2.0f + (float)(8 * Math.Cos(Math.PI * (270 + tmpJiaoDu) / 180)));

                gp.CloseFigure();
                LinearGradientBrush lgb = new LinearGradientBrush(new Point(0, 0), new Point(8, 8), Color.Pink, Color.Red);
                g.FillPath(new SolidBrush(arrowColor), gp);

                //数字
                tmpR = Width / 2.00f - 21;
                RectangleF tmpRect = new RectangleF((float)(Width / 2.0f - Math.Sin(45 * Math.PI / 180) * tmpR), (float)(Height / 2.0f + Math.Cos(45 * Math.PI / 180) * tmpR),
                        tmpR * 1.414f, tmpR - tmpR * 1.414f / 2.0f);
                string tmpStr = value.ToString(format);
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString(tmpStr, Font, new SolidBrush(ForeColor), tmpRect, sf);


                //中心圆
                //g.DrawArc(new Pen(Color.Black, 4), Width / 2 - 13, Height / 2 - 13, 26, 26, 0, 360);

                //g.FillEllipse(new SolidBrush(Color.White), Width / 2 - 11, Height / 2 - 11, 22, 22);

                gp = new GraphicsPath();
                gp.AddEllipse(Width / 2 - 10, Height / 2 - 10, 20, 20);
                PathGradientBrush pgb = new PathGradientBrush(gp);
                pgb.CenterColor = Color.WhiteSmoke;
                pgb.SurroundColors = new Color[] { Color.Black };
                g.FillPath(pgb, gp);

                sf.Dispose();
                pgb.Dispose();
                gp.Dispose();
            }
        }
    }
}
