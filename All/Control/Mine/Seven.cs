using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace All.Control.Mine
{
    internal sealed partial class Seven : System.Windows.Forms.Control
    {
        /// <summary>
        /// 显示符号
        /// </summary>
        public enum simplorList
        {
            /// <summary>
            /// "+"号
            /// </summary>
            Add,
            /// <summary>
            /// "-"号
            /// </summary>
            Del,
            /// <summary>
            /// "."号
            /// </summary>
            Point,
            /// <summary>
            /// 数据
            /// </summary>
            Value,
            /// <summary>
            /// 空白
            /// </summary>
            Null,
            /// <summary>
            /// ":"号
            /// </summary>
            DoublePoint
        }
        bool shardow = true;

        public bool Shardow
        {
            get { return shardow; }
            set { shardow = value; this.Invalidate(); }
        }

        simplorList simplor = simplorList.Value;

        [Category("Shuai")]
        [Description("数码字体显示符号")]
        public simplorList Simplor
        {
            get { return simplor; }
            set { simplor = value; this.Invalidate(); }
        }

        [Category("Shuai")]
        [Description("数码字体的颜色")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; this.Invalidate(); }
        }
        [Category("Shuai")]
        [Description("数码字体背景颜色")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; this.Invalidate(); }
        }
        int fontSize = 20;

        [Category("Shuai")]
        [Description("数码字体的粗细")]
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; this.Invalidate(); }
        }
        int fontSpace = 5;

        [Category("Shuai")]
        [Description("数码字体的液晶段间隙")]
        public int FontSpace
        {
            get { return fontSpace; }
            set { fontSpace = value; this.Invalidate(); }
        }
        Padding margin = new Padding(0);

        [Category("Shuai")]
        [Description("控件内容到边框距离")]
        public new Padding Margin
        {
            get { return margin; }
            set { margin = value; this.Invalidate(); }
        }

        byte value = 0;
        [Category("Shuai")]
        [Description("显示数据值")]
        public byte Value
        {
            get { return this.value; }
            set
            {
                if (value < 0)
                {
                    this.value = 0;
                }
                else if (value > 9)
                {
                    this.value = 9;
                }
                else
                {
                    this.value = value;
                }
                this.Invalidate();
            }
        }

        public Seven()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.Clear(this.BackColor);
            switch (simplor)
            {
                case simplorList.Value:
                    Point[] One = new Point[4];
                    One[0] = new Point(margin.Left, margin.Left);
                    One[1] = new Point(margin.Left + fontSize, margin.Left + fontSize);
                    One[2] = new Point(margin.Left + fontSize, Height / 2 - fontSize / 2 - fontSpace);
                    One[3] = new Point(margin.Left, Height / 2 - fontSpace);
                    if (value == 4 || value == 5 || value == 6 || value == 7 || value == 8 || value == 9 || value == 0)
                    {
                        e.Graphics.FillPolygon(new SolidBrush(ForeColor), One, FillMode.Winding);
                    }
                    else
                    {
                        if (shardow)
                        {
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, ForeColor.R, ForeColor.G, ForeColor.B)), One, FillMode.Winding);
                        }
                    }
                    Point[] Two = new Point[4];
                    Two[0] = new Point(margin.Left + fontSpace, margin.Top);
                    Two[1] = new Point(Width - margin.Right - fontSpace, margin.Top);
                    Two[2] = new Point(Width - margin.Right - fontSize - fontSpace, margin.Top + fontSize);
                    Two[3] = new Point(margin.Left + fontSize + fontSpace, margin.Top + fontSize);
                    if (value == 2 || value == 3 || value == 5 || value == 6 || value == 8 || value == 7 || value == 9 || value == 0)
                    {
                        e.Graphics.FillPolygon(new SolidBrush(ForeColor), Two, FillMode.Winding);
                    }
                    else
                    {
                        if (shardow)
                        {
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, ForeColor.R, ForeColor.G, ForeColor.B)), Two, FillMode.Winding);
                        }
                    }

                    Point[] Three = new Point[4];
                    Three[0] = new Point(Width - margin.Right - fontSize, margin.Top + fontSize);
                    Three[1] = new Point(Width - margin.Right, margin.Top);
                    Three[2] = new Point(Width - margin.Right, Height / 2 - fontSpace);
                    Three[3] = new Point(Width - margin.Right - fontSize, Height / 2 - fontSize / 2 - fontSpace);
                    if (value == 1 || value == 2 || value == 3 || value == 4 || value == 7 || value == 8 || value == 9 || value == 0)
                    {
                        e.Graphics.FillPolygon(new SolidBrush(ForeColor), Three, FillMode.Winding);

                    }
                    else
                    {
                        if (shardow)
                        {
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, ForeColor.R, ForeColor.G, ForeColor.B)), Three, FillMode.Winding);
                        }
                    }

                    Point[] Four = new Point[4];
                    Four[0] = new Point(Width - margin.Right - fontSize, Height - margin.Bottom - fontSize);
                    Four[1] = new Point(Width - margin.Right, Height - margin.Bottom);
                    Four[2] = new Point(Width - margin.Right, Height / 2 + fontSpace);
                    Four[3] = new Point(Width - margin.Right - fontSize, Height / 2 + fontSize / 2 + fontSpace);
                    if (value == 1 || value == 3 || value == 4 || value == 5 || value == 6 || value == 7 || value == 8 || value == 9 || value == 0)
                    {
                        e.Graphics.FillPolygon(new SolidBrush(ForeColor), Four, FillMode.Winding);

                    }
                    else
                    {
                        if (shardow)
                        {
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, ForeColor.R, ForeColor.G, ForeColor.B)), Four, FillMode.Winding);
                        }
                    }

                    Point[] Five = new Point[4];
                    Five[0] = new Point(Width - margin.Right - fontSize - fontSpace, Height - margin.Bottom - fontSize);
                    Five[1] = new Point(Width - margin.Right - fontSpace, Height - margin.Bottom);
                    Five[2] = new Point(margin.Left + fontSpace, Height - margin.Bottom);
                    Five[3] = new Point(margin.Left + fontSize + fontSpace, Height - margin.Bottom - fontSize);
                    if (value == 2 || value == 3 || value == 5 || value == 6 || value == 8 || value == 9 || value == 0)
                    {
                        e.Graphics.FillPolygon(new SolidBrush(ForeColor), Five, FillMode.Winding);
                    }
                    else
                    {
                        if (shardow)
                        {
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, ForeColor.R, ForeColor.G, ForeColor.B)), Five, FillMode.Winding);
                        }
                    }

                    Point[] Six = new Point[4];
                    Six[0] = new Point(margin.Left + fontSize, Height / 2 + fontSize / 2 + fontSpace);
                    Six[1] = new Point(margin.Left, Height / 2 + fontSpace);
                    Six[2] = new Point(margin.Left, Height - margin.Bottom);
                    Six[3] = new Point(margin.Left + fontSize, Height - margin.Bottom - fontSize);
                    if (value == 2 || value == 6 || value == 8 || value == 0)
                    {

                        e.Graphics.FillPolygon(new SolidBrush(ForeColor), Six, FillMode.Winding);
                    }
                    else
                    {
                        if (shardow)
                        {
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, ForeColor.R, ForeColor.G, ForeColor.B)), Six, FillMode.Winding);
                        }
                    }

                    Point[] Seven = new Point[6];
                    Seven[0] = new Point(margin.Left, Height / 2);
                    Seven[1] = new Point(margin.Left + fontSize, Height / 2 - fontSize / 2);
                    Seven[2] = new Point(Width - margin.Right - fontSize, Height / 2 - fontSize / 2);
                    Seven[3] = new Point(Width - margin.Right, Height / 2);
                    Seven[4] = new Point(Width - margin.Right - fontSize, Height / 2 + fontSize / 2);
                    Seven[5] = new Point(margin.Left + fontSize, Height / 2 + fontSize / 2);
                    if (value == 2 || value == 3 || value == 5 || value == 6 || value == 8 || value == 4 || value == 9)
                    {
                        e.Graphics.FillPolygon(new SolidBrush(ForeColor), Seven, FillMode.Winding);
                    }
                    else
                    {
                        if (shardow)
                        {
                            e.Graphics.FillPolygon(new SolidBrush(Color.FromArgb(60, ForeColor.R, ForeColor.G, ForeColor.B)), Seven, FillMode.Winding);
                        }
                    }
                    break;
                case simplorList.Del:
                    Point[] simpolDel = new Point[6];
                    simpolDel[0] = new Point(margin.Left + fontSize, Height / 2);
                    simpolDel[1] = new Point(margin.Left + fontSize, Height / 2 - fontSize / 2);
                    simpolDel[2] = new Point(Width - margin.Right - fontSize, Height / 2 - fontSize / 2);
                    simpolDel[3] = new Point(Width - margin.Right - fontSize, Height / 2);
                    simpolDel[4] = new Point(Width - margin.Right - fontSize, Height / 2 + fontSize / 2);
                    simpolDel[5] = new Point(margin.Left + fontSize, Height / 2 + fontSize / 2);
                    e.Graphics.FillPolygon(new SolidBrush(ForeColor), simpolDel, FillMode.Winding);
                    break;
                case simplorList.Add:
                    Point[] simpolAdd = new Point[6];
                    simpolAdd[0] = new Point(margin.Left + fontSize, Height / 2);
                    simpolAdd[1] = new Point(margin.Left + fontSize, Height / 2 - fontSize / 2);
                    simpolAdd[2] = new Point(Width - margin.Right - fontSize, Height / 2 - fontSize / 2);
                    simpolAdd[3] = new Point(Width - margin.Right - fontSize, Height / 2);
                    simpolAdd[4] = new Point(Width - margin.Right - fontSize, Height / 2 + fontSize / 2);
                    simpolAdd[5] = new Point(margin.Left + fontSize, Height / 2 + fontSize / 2);
                    e.Graphics.FillPolygon(new SolidBrush(ForeColor), simpolAdd, FillMode.Winding);

                    simpolAdd[0] = new Point(Width / 2 - fontSize / 2, Height / 2 - Width / 2 + fontSize + margin.Left);
                    simpolAdd[1] = new Point(Width / 2, Height / 2 - Width / 2 + fontSize + margin.Left);
                    simpolAdd[2] = new Point(Width / 2 + fontSize / 2, Height / 2 - Width / 2 + fontSize + margin.Left);
                    simpolAdd[3] = new Point(Width / 2 + fontSize / 2, Height / 2 + Width / 2 - fontSize - margin.Left);
                    simpolAdd[4] = new Point(Width / 2, Height / 2 + Width / 2 - fontSize - margin.Left);
                    simpolAdd[5] = new Point(Width / 2 - fontSize / 2, Height / 2 + Width / 2 - fontSize - margin.Left);
                    e.Graphics.FillPolygon(new SolidBrush(ForeColor), simpolAdd, FillMode.Winding);
                    break;
                case simplorList.Point:
                    e.Graphics.FillEllipse(new SolidBrush(ForeColor), new Rectangle(Width / 2 - fontSize / 2, Height - margin.Bottom - fontSize, fontSize, fontSize));
                    break;
                case simplorList.DoublePoint:
                    e.Graphics.FillEllipse(new SolidBrush(ForeColor), new Rectangle(Width / 2 - fontSize / 2, Height * 1 / 4 + fontSize / 2, fontSize, fontSize));
                    e.Graphics.FillEllipse(new SolidBrush(ForeColor), new Rectangle(Width / 2 - fontSize / 2, Height * 3 / 4 - fontSize, fontSize, fontSize));
                    break;
            }
            //base.OnPaint(e);
        }

    }
}
