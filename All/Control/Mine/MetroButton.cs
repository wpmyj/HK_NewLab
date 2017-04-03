using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
namespace All.Control.Mine
{
    [DefaultEvent("Click")]
    public partial class MetroButton : System.Windows.Forms.Control,All.Class.Style.ChangeTheme
    {
        /// <summary>
        /// 自动刷新委托
        /// </summary>
        internal delegate void FlushHandle();
        
        Content title = new Content("标题文字",new Font("宋体", 36, FontStyle.Bold));
        /// <summary>
        /// 主显示内容
        /// </summary>
        [Description("主显示内容")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Content Title
        {
            get { return title; }
            set { title = value; lock (lockObject) { InitGrpahic(); } }
        }
        Content value = new Content("文本说明字符",new Font("宋体", 15, FontStyle.Bold));
        /// <summary>
        /// 副显示内容
        /// </summary>
        [Description("副显示内容")]
        [Category("Shuai")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Content Value
        {
            get { return value; }
            set { this.value = value; lock (lockObject) { InitGrpahic(); } }
        }
        Sence first = new Sence();
        /// <summary>
        /// 第一场景
        /// </summary>
        [Category("Shuai")]
        [Description("第一场景")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Sence First
        {
            get { return first; }
            set { first = value; lock (lockObject) { InitGrpahic(); } }
        }
        Sence second = new Sence();
        /// <summary>
        /// 第二场景
        /// </summary>
        [Category("Shuai")]
        [Description("第二场景")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Sence Second
        {
            get { return second; }
            set { second = value; lock (lockObject) { InitGrpahic(); } }
        }
        /// <summary>
        /// 当前显示场景
        /// </summary>
        public enum ShowList : int
        {
            First = 0,
            Second
        }
        ShowList senceNow = ShowList.First;
        /// <summary>
        /// 首先显示场景
        /// </summary>
        [Category("Shuai")]
        [Description("首先显示场景")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ShowList SenceNow
        {
            get { return senceNow; }
            set { senceNow = value; this.Invalidate(); }
        }
        protected override void OnEnabledChanged(EventArgs e)
        {
            InitGrpahic();
            timFlush.Enabled = this.Enabled;
            base.OnEnabledChanged(e);
        }
        //图形切换变量
        Bitmap imgOne, imgTwo, img;
        Graphics grapOne, grapTwo, grap;
        IntPtr hdcOne, hwndOne, oldHdcOne, hdc;
        IntPtr hdcTwo, hwndTwo, oldHdcTwo;
        object lockObject = new object();
        System.Windows.Forms.Timer timFlush;
        const int ChangeTime = 1000;//变换时间
        const int FlushTime = 20;//刷新时间
        int changeIndex = 0;//变换数字量
        float changeEveryTime = 0;
        int delayStart = 0;//当前是否正在等待中
        //

        StringFormat sf = new StringFormat();
        bool isMouseDown = false;
        Point oldPoint = Point.Empty;
        public MetroButton()
        {
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint, true);
            this.MinimumSize = new Size(10, 10);
            if (timFlush == null && !All.Class.Environment.IsDesignMode)
            {
                delayStart = Environment.TickCount;
                timFlush = new Timer();
                timFlush.Interval = FlushTime;
                timFlush.Tick += timFlush_Tick;
                timFlush.Enabled = true;
            }
        }

        private void timFlush_Tick(object sender, EventArgs e)
        {
            lock (lockObject)
            {
                if (imgOne == null || imgTwo == null || img == null)
                {
                    InitGrpahic();
                }
                if (delayStart > 0)
                {
                    if ((senceNow == ShowList.First && ((Environment.TickCount - delayStart) >= first.DelayTime))
                        || (senceNow == ShowList.Second && ((Environment.TickCount - delayStart) >= second.DelayTime)))
                    {
                        delayStart = -1;
                    }
                }
                else
                {
                    if ((changeIndex * changeEveryTime) >= this.Height)
                    {
                        senceNow = (ShowList)((1 + (int)senceNow) % 2);
                        changeIndex = 0;
                        delayStart = Environment.TickCount;
                        All.Class.Api.BitBlt(hdc, 0, 0, this.Width, this.Height, senceNow == ShowList.First ? hdcOne : hdcTwo, 0, 0, All.Class.Api.ROP_SrcCopy);
                    }
                    else
                    {
                        switch (senceNow)
                        {
                            case ShowList.First:
                                All.Class.Api.BitBlt(hdc, 0, 0, this.Width, imgOne.Height - (int)(changeIndex * changeEveryTime), hdcOne, 0, (int)(changeIndex * changeEveryTime), All.Class.Api.ROP_SrcCopy);
                                All.Class.Api.BitBlt(hdc, 0, imgOne.Height - (int)(changeIndex * changeEveryTime), this.Width, (int)(changeIndex * changeEveryTime), hdcTwo, 0, 0, All.Class.Api.ROP_SrcCopy);
                                break;
                            case ShowList.Second:
                                All.Class.Api.BitBlt(hdc, 0, 0, this.Width, imgOne.Height - (int)(changeIndex * changeEveryTime), hdcTwo, 0, (int)(changeIndex * changeEveryTime), All.Class.Api.ROP_SrcCopy);
                                All.Class.Api.BitBlt(hdc, 0, imgOne.Height - (int)(changeIndex * changeEveryTime), this.Width, (int)(changeIndex * changeEveryTime), hdcOne, 0, 0, All.Class.Api.ROP_SrcCopy);
                                break;
                        }
                        changeIndex++;
                    }
                }
            }
        }
        private void InitGrpahic()
        {
            img = new Bitmap(Width, Height);
            imgOne = new Bitmap(Width, Height);
            imgTwo = new Bitmap(Width, Height);

            if (!All.Class.Environment.IsDesignMode)
            {
                grap = Graphics.FromHwnd(this.Handle);
                grapOne = Graphics.FromImage(imgOne);
                grapTwo = Graphics.FromImage(imgTwo);

                grapOne.Clear(this.Enabled ? first.BackColor : Color.Gray);
                grapTwo.Clear(this.Enabled ? second.BackColor : Color.LightGray);
                //if (this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition))
                //{
                //    grapOne.DrawRectangle(new Pen(All.Class.Style.BoardColor, 3), 1, 1, Width - 3, Height - 3);
                //    grapTwo.DrawRectangle(new Pen(All.Class.Style.BoardColor, 3), 1, 1, Width - 3, Height - 3);
                //}
                sf.LineAlignment = StringAlignment.Far;
                grapOne.DrawString(title.Text, title.Font, new SolidBrush(title.ForeColor), new Rectangle(0, 0, Width, Height * 2 / 3), sf);
                grapTwo.DrawString(title.Text, title.Font, new SolidBrush(title.ForeColor), new Rectangle(0, 0, Width, Height * 2 / 3), sf);
                sf.LineAlignment = StringAlignment.Near; ;
                grapOne.DrawString(value.Text, value.Font, new SolidBrush(value.ForeColor), new Rectangle(0, Height * 2 / 3, Width, Height / 3), sf);
                grapTwo.DrawString(value.Text, value.Font, new SolidBrush(value.ForeColor), new Rectangle(0, Height * 2 / 3, Width, Height / 3), sf);

                hdc = grap.GetHdc();
                hdcOne = grapOne.GetHdc();
                hdcTwo = grapTwo.GetHdc();
                hwndOne = imgOne.GetHbitmap();
                hwndTwo = imgTwo.GetHbitmap();
                oldHdcOne = All.Class.Api.SelectObject(hdcOne, hwndOne);
                oldHdcTwo = All.Class.Api.SelectObject(hdcTwo, hwndTwo);

                changeEveryTime = (float)this.Height * FlushTime / ChangeTime;
            }
        }
        public void ChangeBack(All.Class.Style.BackColors backColor)
        {
            lock (lockObject)
            {
                InitGrpahic();
            }
        }
        public void ChangeFront(All.Class.Style.FrontColors frontColor)
        {
            lock (lockObject)
            {
                InitGrpahic();
            }
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
        protected override void OnSizeChanged(EventArgs e)
        {
            lock (lockObject)
            {
                InitGrpahic();
            }
            base.OnSizeChanged(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.ClientRectangle.Contains(e.Location))
            {
                isMouseDown = true;
                if (oldPoint == Point.Empty)
                {
                    oldPoint = this.Location;
                }
                this.Location = new Point(oldPoint.X + 2, oldPoint.Y + 2);
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = oldPoint;
                oldPoint = Point.Empty;
                isMouseDown = false;
            }
            base.OnMouseUp(e);
        }
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = oldPoint;
                oldPoint = Point.Empty;
                isMouseDown = false;
            }
            base.OnMouseLeave(e);
        }
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (img != null)
            {
                if (first.Invalidate == null)
                {
                    first.Invalidate = new FlushHandle(this.Invalidate);
                }
                if (second.Invalidate == null)
                {
                    second.Invalidate = new FlushHandle(this.Invalidate);
                }
                if (title.Invalidate == null)
                {
                    title.Invalidate = new FlushHandle(this.Invalidate);
                }
                if (value.Invalidate == null)
                {
                    value.Invalidate = new FlushHandle(this.Invalidate);
                }
                using (Graphics g = Graphics.FromImage(img))
                {
                    switch (senceNow)
                    {
                        case ShowList.First:
                            g.Clear(this.Enabled ? first.BackColor : Color.Gray);
                            break;
                        case ShowList.Second:
                            g.Clear(this.Enabled ? second.BackColor : Color.LightGray);
                            break;
                    }
                    sf.LineAlignment = StringAlignment.Far;
                    g.DrawString(title.Text, title.Font, new SolidBrush(title.ForeColor), new Rectangle(0, 0, Width, Height * 2 / 3), sf);
                    sf.LineAlignment = StringAlignment.Near; ;
                    g.DrawString(value.Text, value.Font, new SolidBrush(value.ForeColor), new Rectangle(0, Height * 2 / 3, Width, Height / 3), sf);
                }
                e.Graphics.DrawImageUnscaled(img, 0, 0);
            }
            base.OnPaint(e);
        }
        /// <summary>
        /// 场景
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Description("场景")]
        public class Sence
        {
            Color backColor = All.Class.Style.BoardColor;
            /// <summary>
            /// 背景颜色
            /// </summary>
            [Description("背景颜色")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public Color BackColor
            {
                get { return backColor; }
                set { backColor = value; if (Invalidate != null)Invalidate(); }
            }
            int delayTime = 3000;
            /// <summary>
            /// 场景停留时间
            /// </summary>
            [Description("场景停留时间")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public int DelayTime
            {
                get { return delayTime; }
                set { delayTime = value; if (Invalidate != null)Invalidate(); }
            }
            FlushHandle invalidate = null;
            /// <summary>
            /// 刷新
            /// </summary>
            [Description("刷新")]
            internal FlushHandle Invalidate
            {
                get { return invalidate; }
                set { invalidate = value;}
            }
            public Sence()
            {
            }
        }
        /// <summary>
        /// 显示内容
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Description("显示内容")]
        public class Content
        {
            string text = "";
            /// <summary>
            /// 显示文本
            /// </summary>
            [Description("显示文本")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public string Text
            {
                get { return text; }
                set { text = value; if (Invalidate != null)Invalidate(); }
            }
            Font font = new Font("宋体", 12);
            /// <summary>
            /// 文本字体 
            /// </summary>
            [Description("文本字体")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public Font Font
            {
                get { return font; }
                set { font = value; if (Invalidate != null)Invalidate(); }
            }
            Color foreColor = All.Class.Style.FontColor;
            /// <summary>
            /// 文本颜色
            /// </summary>
            [Description("文本颜色")]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
            public Color ForeColor
            {
                get { return foreColor; }
                set { foreColor = value; if (Invalidate != null)Invalidate(); }
            }
            FlushHandle invalidate = null;
            /// <summary>
            /// 刷新
            /// </summary>
            [Description("刷新")]
            internal FlushHandle Invalidate
            {
                get { return invalidate; }
                set { invalidate = value; }
            }
            /// <summary>
            /// 显示内容
            /// </summary>
            /// <param name="text">显示文本</param>
            public Content(string text,Font font)
            {
                this.Text = text;
                this.Font = font;
            }
            //这里一定不能省,无参数的构造函数省略的话,会造成界面不能自动生成本地代码.会不能保存界面属性
            public Content()
            {
            }
        }
    }

}
