using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
namespace All.Control.Metro
{
    public partial class ProgressBar : System.Windows.Forms.Control, All.Class.Style.ChangeTheme
    {
        int maximum = 10;
        /// <summary>
        /// 获取或设置此 ProgressBar 使用的范围的上限。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 ProgressBar 使用的范围的上限。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Maximum
        {
            get { return maximum; }
            set
            {
                if (value < minimum)
                {
                    throw new ArgumentException(string.Format("Value\r\n\t必须大于 {0}", minimum));
                }
                maximum = value;
                if (Value > value)
                {
                    Value = value;
                }
                this.Invalidate();
            }
        }
        int minimum = 0;
        /// <summary>
        ///     获取或设置此 ProgressBar 使用的范围的下限。
        /// </summary>   
        [Category("Shuai")]
        [Description("获取或设置此 ProgressBar 使用的范围的下限。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Minimum
        {
            get { return minimum; }
            set
            {
                if (value > maximum)
                {
                    throw new ArgumentException(string.Format("Value\r\n\t必须小于 {0}", maximum));
                }
                minimum = value;
                if (Value < value)
                {
                    Value = value;
                }
                this.Invalidate();
            }
        }
        int value = 4;
        /// <summary>
        /// 获取或设置此 ProgressBar 当前实际值。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 ProgressBar 当前实际值。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Value
        {
            get { return value; }
            set
            {
                if (value > maximum || value < minimum)
                {
                    return;
                    //throw new ArgumentException(string.Format("Value\r\n\t必须在 {0} 与 {1} 之间", minimum, maximum));
                }
                this.value = value;
                this.Invalidate();
            }
        }
        Color miniColor = System.Windows.Forms.ControlPaint.LightLight(All.Class.Style.BoardColor);
        /// <summary>
        /// 获取或设置此 ProgressBar 小值区域颜色。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 ProgressBar 小值区域颜色。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color MiniColor
        {
            get { return miniColor; }
            set { miniColor = value; this.Invalidate(); }
        }
        Color maxiColor = System.Windows.Forms.ControlPaint.Light(ControlPaint.Dark(All.Class.Style.BoardColor));
        /// <summary>
        /// 获取或设置此 ProgressBar 大值区域颜色。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 ProgressBar 大值区域颜色。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color MaxiColor
        {
            get { return maxiColor; }
            set { maxiColor = value; this.Invalidate(); }
        }
        bool ownerDraw = false;
        /// <summary>
        /// 获取或设置此 ProgressBar 是否使用用户颜色。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 ProgressBar 是否使用用户颜色。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OwnerDraw
        {
            get { return ownerDraw; }
            set { ownerDraw = value; this.Invalidate(); }
        }
        Bitmap backImage;
        Rectangle scroll = Rectangle.Empty;
        Rectangle min = Rectangle.Empty;
        Rectangle max = Rectangle.Empty;
        public ProgressBar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint, true);
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
        public void ChangeBack(All.Class.Style.BackColors backColor)
        {
            this.Invalidate();
        }
        public void ChangeFront(All.Class.Style.FrontColors frontColor)
        {
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.Width < this.Height * 2)
            {
                return;
            }
            Init();
            base.OnSizeChanged(e);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }
        private void Init()
        {
            backImage = new Bitmap(Width, Height);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (backImage == null)
            {
                Init();
            }
            using (Graphics g = Graphics.FromImage(backImage))
            {
                if (minimum >= maximum)
                {
                    g.Clear(maxiColor);
                }
                else
                {
                    Color tmpMiniColor = miniColor;
                    Color tmpMaxiColor = maxiColor;
                    int fCur  = this.Width * value / (this.maximum - this.minimum);
                    min = new Rectangle(0, 0, fCur, this.Height);
                    max = new Rectangle(fCur, 0, this.Width - fCur, this.Height);
                    //画最大,最小值 
                    if (!ownerDraw)
                    {
                        tmpMiniColor = All.Class.Style.BoardColor;
                        tmpMaxiColor = Color.Silver;//All.Class.Style.BoardColor;
                    }
                    g.FillRectangle(new SolidBrush(tmpMaxiColor), max);
                    g.FillRectangle(new SolidBrush(tmpMiniColor), min);
                }
            }
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            base.OnPaint(e);
        }
    }
}
