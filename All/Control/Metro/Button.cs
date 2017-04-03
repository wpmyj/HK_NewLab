using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace All.Control.Metro
{
    public partial class Button : System.Windows.Forms.Control, All.Class.Style.ChangeTheme,System.Windows.Forms.IButtonControl
    {
        bool boarder = true;
        /// <summary>
        /// 是否有边框
        /// </summary>
        [Category("Shuai")]
        [Description("是否有边框")]
        public bool Boarder
        {
            get { return boarder; }
            set
            {
                boarder = value;
                this.Invalidate();
            }
        }

        Orientation orientation = Orientation.Vertical;
        /// <summary>
        /// 水平或者竖起放置图像
        /// </summary>
        [Category("Shuai")]
        [Description("水平或者竖起放置图像")]
        public Orientation Orientation
        {
            get { return orientation; }
            set
            {
                orientation = value;
                this.Invalidate();
            }
        }
        Bitmap backImage;
        bool isMouseDown = false;
        Point oldPoint = Point.Empty;
        StringFormat sf = new StringFormat();

        DialogResult dialogResult = DialogResult.OK;

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }
        // 摘要: 
        //     通知某个控件是默认按钮，以便相应调整其外观和行为。
        //
        // 参数: 
        //   value:
        //     如果控件要用作默认按钮，值为 true；反之，值为 false。
        public void NotifyDefault(bool value)
        { }
        //
        // 摘要: 
        //     为该控件生成 System.Windows.Forms.Control.Click 事件。
        public void PerformClick()
        {
            OnClick(new EventArgs());
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnTextChanged(e);
        }
        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    this.Invalidate();
        //    base.OnEnabledChanged(e);
        //}
        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnBackgroundImageChanged(e);
        }
        public Button()
        {
            InitializeComponent();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            this.MinimumSize = new Size(10, 10);
        }
        public void ChangeFront(All.Class.Style.FrontColors color)
        {
            this.Invalidate();
        }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
            this.BackColor = All.Class.Style.BackColor;
            this.ForeColor = All.Class.Style.FontColor;
            this.Invalidate();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            Init();
            base.OnSizeChanged(e);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor = Cursors.Hand;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Default;
            base.OnMouseLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            if (this.ClientRectangle.Contains(e.Location))
            {
                isMouseDown = true;
                if (oldPoint == Point.Empty)
                {
                    oldPoint = this.Location;
                }
                this.Location = new Point(oldPoint.X + 1, oldPoint.Y + 1);
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
        protected override void OnHandleCreated(EventArgs e)
        {
            All.Class.Style.AllStyle.Add(this);
            ChangeBack(All.Class.Style.Back);
            base.OnHandleCreated(e);
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnHandleDestroyed(e);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
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
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;
                if (this.Enabled)
                {
                    g.Clear(this.BackColor);
                }
                else
                {
                    g.Clear(All.Class.Style.UEnableColor);
                }
                if (boarder)
                {
                    g.DrawRectangle(new Pen(All.Class.Style.BoardColor, 1), 1, 1, Width - 3, Height - 3);
                    //g.DrawRectangle(new Pen(ControlPaint.Light(ControlPaint.Dark(All.Class.Style.BoardColor)), 1), 0, 0, Width - 1, Height - 1);
                }
                if (this.BackgroundImage == null)
                {
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);
                }
                else
                {
                    float len = 0;
                    int start = 0;
                    switch (orientation)
                    {
                        case System.Windows.Forms.Orientation.Vertical:
                            len = Math.Min(this.Width * 2f / 3, this.Height / 3f + All.Class.Num.GetFontHeight(this.Font)) - 2;
                            if (len > 0)
                            {
                                g.DrawImage(this.BackgroundImage, new Rectangle((int)((this.Width - len) / 2), (int)((this.Height * 2 / 3 - len) * 2f / 3), (int)len, (int)len), new Rectangle(new Point(0, 0), this.BackgroundImage.Size), GraphicsUnit.Pixel);
                            }
                            sf.LineAlignment = StringAlignment.Near;
                            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), new Rectangle(0, Height * 2 / 3, Width, Height / 3), sf);
                            break;
                        case System.Windows.Forms.Orientation.Horizontal:
                            len = Math.Min(this.Height * 2 / 3, this.Width * 3 / 10);
                            start = (int)(len * (this.Text.Length == 0 ? 1.1 : 1.3f)) + (int)(All.Class.Num.GetFontWidth(this.Font, this.Text));
                            g.DrawImage(this.BackgroundImage, new Rectangle(this.Width / 2 - start / 2, this.Height / 2 - (int)(len) / 2, (int)len, (int)len), new Rectangle(new Point(0, 0), this.BackgroundImage.Size), GraphicsUnit.Pixel);
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Near;
                            g.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor),
                                new Point(this.Width / 2 - start / 2 + (int)(len * 1.3f),
                                    this.Height / 2 - All.Class.Num.GetFontHeight(this.Font) / 2), sf);
                            break;
                    }
                }
            }
            e.Graphics.DrawImageUnscaled(backImage, 0, 0);
            base.OnPaint(e);
        }
    }
}
