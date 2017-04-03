using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace All.Control.Mine
{
    public partial class Shape : UserControl,All.Class.Style.ChangeTheme
    {
        /// <summary>
        /// 图形形状
        /// </summary>
        [Description("图形形状")]
        public enum Shapes
        {
            /// <summary>
            /// 直线
            /// </summary>
            [Description("直线")]
            Line,
            /// <summary>
            /// 方形
            /// </summary>
            [Description("方形")]
            Square,
            /// <summary>
            /// 圆形
            /// </summary>
            [Description("圆形")]
            Circle,
            /// <summary>
            /// 椭圆
            /// </summary>
            [Description("椭圆")]
            Oval
        }
        Shapes value = Shapes.Line;
        /// <summary>
        /// 图形形状
        /// </summary>
        [Description("图形形状")]
        [Category("Shuai")]
        public Shapes Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        int lineWidth = 5;
        /// <summary>
        /// 线条宽度
        /// </summary>
        [Description("线条宽度")]
        [Category("Shuai")]
        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }
        bool transparent = false;
        /// <summary>
        /// 是否中空
        /// </summary>
        [Description("是否中空")]
        [Category("Shuai")]
        public bool Transparent
        {
            get { return transparent; }
            set { transparent = value; }
        }

        bool autoColor = true;
        /// <summary>
        /// 自动根据主题颜色改变
        /// </summary>
        [Description("自动根据主题颜色改变")]
        [Category("Shuai")]
        public bool AutoColor
        {
            get { return autoColor; }
            set { autoColor = value; }
        }
        public Shape()
        {
            InitializeComponent();
        }
        public void ChangeBack(All.Class.Style.BackColors color)
        {
        }
        public void ChangeFront(All.Class.Style.FrontColors color)
        {
            if (autoColor)
            {
                this.BackColor = All.Class.Style.BoardColor;
            }
            this.Invalidate();
        }
        
        protected override void OnHandleCreated(EventArgs e)
        {
            All.Class.Style.AllStyle.Add(this);
            ChangeFront(All.Class.Style.Board);
            base.OnHandleCreated(e);
        }
        protected override void OnHandleDestroyed(EventArgs e)
        {
            All.Class.Style.AllStyle.Remove(this);
            base.OnHandleDestroyed(e);
        }

        private void Shape_Load(object sender, EventArgs e)
        {
            
        }
    }
}
