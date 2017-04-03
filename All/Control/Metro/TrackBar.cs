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
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("LargeChange")]
    public partial class TrackBar : System.Windows.Forms.Control,All.Class.Style.ChangeTheme
    {
        int maximum = 10;
        /// <summary>
        /// 获取或设置此 TrackBar 使用的范围的上限。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 使用的范围的上限。")]
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
        ///     获取或设置此 TrackBar 使用的范围的下限。
        /// </summary>   
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 使用的范围的下限。")]
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
        int largeChange = 1;
        /// <summary>
        /// 获取或设置此 TrackBar 点击时改变值。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 点击时改变值。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int LargeChange
        {
            get { return largeChange; }
            set { largeChange = value; this.Invalidate(); }
        }
        int smallChange = 1;
        /// <summary>
        /// 获取或设置此 TrackBar 按键时改变值。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 按键时改变值。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int SmallChange
        {
            get { return smallChange; }
            set 
            {
                if (value > maximum)
                {
                    throw new ArgumentException(string.Format("Value\r\n\t必须大于 0"));
                }
                smallChange = value;
                
                this.Invalidate(); }
        }
        int value = 4;
        /// <summary>
        /// 获取或设置此 TrackBar 当前实际值。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 当前实际值。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Value
        {
            get { return value; }
            set {
                if (value > maximum || value < minimum)
                {
                    throw new ArgumentException(string.Format("Value\r\n\t必须在 {0} 与 {1} 之间", minimum, maximum));
                }
                this.value = value;
                if (ValueChanged != null)
                {
                    ValueChanged(this, new EventArgs());
                }
                this.Invalidate(); }
        }
        System.Windows.Forms.Orientation orientation = System.Windows.Forms.Orientation.Horizontal;
        /// <summary>
        /// 获取或设置此 TrackBar 方向。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 方向。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public System.Windows.Forms.Orientation Orientation
        {
            get { return orientation; }
            set
            {
                if (orientation != value)
                {
                    orientation = value;
                    this.Size = new Size(this.Height, this.Width);
                }
            }
        }
        bool ownerDraw = false;
        /// <summary>
        /// 获取或设置此 TrackBar 是否使用用户颜色。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 是否使用用户颜色。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool OwnerDraw
        {
            get { return ownerDraw; }
            set { ownerDraw = value; this.Invalidate(); }
        }
        Color miniColor = System.Windows.Forms.ControlPaint.LightLight(All.Class.Style.BoardColor);
        /// <summary>
        /// 获取或设置此 TrackBar 小值区域颜色。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 小值区域颜色。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color MiniColor
        {
            get { return miniColor; }
            set { miniColor = value; this.Invalidate(); }
        }
        Color maxiColor = System.Windows.Forms.ControlPaint.Light(ControlPaint.Dark(All.Class.Style.BoardColor));
        /// <summary>
        /// 获取或设置此 TrackBar 大值区域颜色。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 大值区域颜色。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color MaxiColor
        {
            get { return maxiColor; }
            set { maxiColor = value; this.Invalidate(); }
        }
        Color scrollColor = All.Class.Style.FontColor;
        /// <summary>
        /// 获取或设置此 TrackBar 滑块颜色。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 滑块颜色。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ScrollColor
        {
            get { return scrollColor; }
            set { scrollColor = value; this.Invalidate(); }
        }
        bool scrollValue = true;
        /// <summary>
        /// 获取或设置此 TrackBar 滑块中间是否显示数值。
        /// </summary>
        [Category("Shuai")]
        [Description("获取或设置此 TrackBar 滑块中间是否显示数值。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool ScrollValue
        {
            get { return scrollValue; }
            set { scrollValue = value; this.Invalidate(); }
        }

        /// <summary>
        /// 此 TrackBar 数值改变时发生。
        /// </summary>
        [Category("Shuai")]
        [Description("此 TrackBar 数值改变时发生。")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event EventHandler ValueChanged;
        Bitmap backImage;
        Rectangle scroll = Rectangle.Empty;
        Rectangle min = Rectangle.Empty;
        Rectangle max = Rectangle.Empty;
        bool mouseInScroll = false;
        bool mouseDownScroll = false;
        Point mouseDownPoint = Point.Empty;
        public TrackBar()
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
            switch (orientation)
            {
                case System.Windows.Forms.Orientation.Horizontal:
                    if (this.Width < this.Height * 2)
                    {
                        return;
                    }
                    break;
                case System.Windows.Forms.Orientation.Vertical:
                    if (this.Height < this.Width * 2)
                    {
                        return;
                    }
                    break;
            }
            Init();
            base.OnSizeChanged(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseDownPoint = Point.Empty;
            mouseDownScroll = false;
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!scroll.Contains(e.Location))
            {
                switch (orientation)
                {
                    case System.Windows.Forms.Orientation.Horizontal:
                        if (e.X > scroll.Left + scroll.Width)
                        {
                            this.Value = Math.Min(this.maximum, this.value + this.largeChange);
                        }
                        if (e.X < scroll.Left)
                        {
                            this.Value = Math.Max(this.minimum, this.value - this.largeChange);
                        }
                        break;
                    case System.Windows.Forms.Orientation.Vertical:
                        if (e.Y > scroll.Top + scroll.Height)
                        {
                            this.Value = Math.Min(this.maximum, this.value + this.largeChange);
                        }
                        if (e.Y < scroll.Top)
                        {
                            this.Value = Math.Max(this.minimum, this.value - this.largeChange);
                        }
                        break;
                }
            }
            else
            {
                if (!mouseDownScroll)
                {
                    mouseDownPoint = new Point(scroll.X + scroll.Width / 2, scroll.Y + scroll.Height / 2);
                }
                mouseDownScroll = true;
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            mouseDownScroll = false;
            mouseInScroll = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDownScroll)
            {
                if (this.Width != this.Height && this.minimum != this.maximum)
                {
                    int tmp = 0;
                    switch (orientation)
                    {
                        case System.Windows.Forms.Orientation.Horizontal:
                            tmp = Convert.ToInt32((float)(e.X - mouseDownPoint.X) * (this.maximum - this.minimum) / (this.Width - this.Height));
                            if (tmp >= this.smallChange || tmp <= -this.smallChange)
                            {
                                this.Value = Math.Min(Math.Max(this.minimum, this.value + tmp), this.maximum);
                                mouseDownPoint.X = mouseDownPoint.X + tmp * (this.Width - this.Height) / (this.maximum - this.minimum);
                            }
                            break;
                        case System.Windows.Forms.Orientation.Vertical:
                            tmp = Convert.ToInt32((float)(e.Y - mouseDownPoint.Y) * (this.maximum - this.minimum) / (this.Height - this.Width));
                            if (tmp >= this.smallChange || tmp <= -this.smallChange)
                            {
                                this.Value = Math.Min(Math.Max(this.minimum, this.value + tmp), this.maximum);
                                mouseDownPoint.Y = mouseDownPoint.Y + tmp * (this.Height - this.Width) / (this.maximum - this.minimum);
                            }
                            break;
                    }
                }
            }
            else
            {
                if (scroll.Contains(e.Location))
                {
                    if (!mouseInScroll)
                    {
                        mouseInScroll = true;
                        this.Invalidate();
                    }
                }
                else
                {
                    if (mouseInScroll)
                    {
                        mouseInScroll = false;
                        this.Invalidate();
                    }
                }
            }
            base.OnMouseMove(e);
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
                    g.Clear(scrollColor);
                }
                else 
                {
                    Color tmpMiniColor = miniColor;
                    Color tmpMaxiColor = maxiColor;
                    Color tmpScrollColor = scrollColor;
                    int fCur = 0;
                    switch (orientation)
                    {
                        case System.Windows.Forms.Orientation.Horizontal:
                            fCur = (this.Width - this.Height) * value / (this.maximum - this.minimum);
                            scroll = new Rectangle(fCur, 0, this.Height, this.Height);
                            min = new Rectangle(0, 0, fCur, this.Height);
                            max = new Rectangle(fCur, 0, this.Width - fCur, this.Height);
                            break;
                        case System.Windows.Forms.Orientation.Vertical:
                            fCur = (this.Height - this.Width) * value / (this.maximum - this.minimum);
                            min = new Rectangle(0, 0, this.Width, fCur);
                            max = new Rectangle(0, fCur, this.Width, this.Height - fCur);
                            scroll = new Rectangle(0, fCur, this.Width, this.Width);
                            break;
                    }
                    //画最大,最小值 
                    if (!ownerDraw)
                    {
                        tmpMiniColor = All.Class.Style.BoardColor;
                        tmpMaxiColor = Color.Silver;//All.Class.Style.BoardColor;
                        switch (All.Class.Style.Back)
                        {
                            case Class.Style.BackColors.White:
                                tmpScrollColor = ControlPaint.Light(All.Class.Style.FontColor);
                                break;
                            case Class.Style.BackColors.Black:
                                tmpScrollColor = ControlPaint.Dark(All.Class.Style.FontColor);
                                break;
                        }
                    }
                    if (mouseInScroll)
                    {
                        switch (All.Class.Style.Back)
                        {
                            case Class.Style.BackColors.White:
                                tmpScrollColor = ControlPaint.Dark(tmpScrollColor);
                                break;
                            case Class.Style.BackColors.Black:
                                tmpScrollColor = ControlPaint.Light(tmpScrollColor);
                                break;
                        }
                        tmpMiniColor = ControlPaint.LightLight(tmpMiniColor);
                        tmpMaxiColor = ControlPaint.LightLight(tmpMaxiColor);
                    }
                    g.FillRectangle(new SolidBrush(tmpMaxiColor), max);
                    g.FillRectangle(new SolidBrush(tmpMiniColor), min);
                    //画滑动块
                    g.FillRectangle(new SolidBrush(tmpScrollColor), scroll);

                }
            }
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            base.OnPaint(e);
        }
    }
}
