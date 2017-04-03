using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Window
{
    public partial class BaseWindow : Form, All.Class.Style.ChangeTheme
    {
        int boardWidth = 0;
        [Browsable(false)]
        public int BoardWidth
        {
            get {
                if (boardWidth == 0)
                {
                    boardWidth = Math.Max(4, this.Width / 200);
                }
                return boardWidth;
            }
        }
        public BaseWindow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint, true);
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return All.Class.Style.BackColor;
            }
            set
            {
                //base.BackColor = value;
            }
        }
        public void ChangeBack(All.Class.Style.BackColors backColor)
        {
            this.Invalidate();
        }
        public void ChangeFront(All.Class.Style.FrontColors frontColor)
        {
            this.Invalidate();
        }
       
        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.Padding = new System.Windows.Forms.Padding(BoardWidth);
            ChangeBack(All.Class.Style.Back);
        }
        bool isMouseDown = false;
        Point oldMousePoint = Point.Empty;
        Point oldWindowPoint = Point.Empty;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            isMouseDown = true;
            oldMousePoint = this.PointToScreen(e.Location);
            oldWindowPoint = this.Location;
            this.Cursor = Cursors.SizeAll;
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = false;
            oldMousePoint = Point.Empty;
            oldWindowPoint = Point.Empty;
            this.Cursor = Cursors.Default;
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isMouseDown)
            {
                Point nowMousePoint = this.PointToScreen(e.Location);
                int x = oldWindowPoint.X + nowMousePoint.X - oldMousePoint.X;
                int y = oldWindowPoint.Y + nowMousePoint.Y - oldMousePoint.Y;
                this.Location = new Point(x, y);
            }
            base.OnMouseMove(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            All.Class.Style.AllStyle.Add(this);
            base.OnLoad(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnClosed(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(new Pen(All.Class.Style.BoardColor, BoardWidth), BoardWidth / 2, BoardWidth / 2, Width - BoardWidth, Height - BoardWidth);
            base.OnPaint(e);
        }
    }
}
