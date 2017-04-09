using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Threading;
namespace All.Control.Mine
{
    public partial class DateTime : System.Windows.Forms.Control,All.Class.Style.ChangeTheme
    {
        Seven[] seven;

        string formatValue = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 显示数据内容
        /// </summary>
        [Description("显示数据内容")]
        public enum FormatList
        {
            日期,
            时间,
            日期和时间
        }
        FormatList format = FormatList.日期和时间;
        /// <summary>
        /// 显示数据内容
        /// </summary>
        [Category("Shuai")]
        [Description("显示数据内容")]
        public FormatList Format
        {
            get { return format; }
            set
            {
                format = value;
                switch (format)
                {
                    case FormatList.日期和时间:
                        formatValue = "yyyy-MM-dd HH:mm:ss";
                        break;
                    case FormatList.日期:
                        formatValue = "yyyy-MM-dd";
                        break;
                    case FormatList.时间:
                        formatValue = "HH:mm:ss";
                        break;
                }
                init();
            }
        }

        bool shardow = false;
        /// <summary>
        /// 是否显示阴影
        /// </summary>
        [Category("Shuai")]
        [Description("是否显示阴影")]
        public bool Shardow
        {
            get { return shardow; }
            set
            {
                shardow = value;
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].Shardow = value;
                }
            }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        [Category("Shuai")]
        [Description("字体颜色")]
        public override System.Drawing.Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
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
        /// 背景颜色
        /// </summary>
        [Category("Shuai")]
        [Description("背景颜色")]
        public override System.Drawing.Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].BackColor = value;
                }
            }
        }
        private System.DateTime now = System.DateTime.Now;
        /// <summary>
        /// 显示数据
        /// </summary>
        [Category("Shuai")]
        [Description("显示数据")]
        public System.DateTime Value
        {
            get { return now; }
            set
            {
                now = value;
                SetValue(now);
            }
        }
        void All.Class.Style.ChangeTheme.ChangeBack(All.Class.Style.BackColors backColor)
        {
            this.BackColor = All.Class.Style.BackColor;
        }
        void All.Class.Style.ChangeTheme.ChangeFront(All.Class.Style.FrontColors fontColor)
        { 
        }
        private void SetValue(System.DateTime value)
        {
            string tmpTime = value.ToString(formatValue);
            for (int i = 0; i < tmpTime.Length; i++)
            {
                switch (tmpTime.Substring(i, 1))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        //seven[i].Simplor = Seven.simplorList.Value;
                        seven[i].Value = Class.Num.ToByte(tmpTime.Substring(i, 1));
                        break;
                    case "-":
                        //seven[i].Simplor = Seven.simplorList.Del;
                        break;
                    case ":":
                        //seven[i].Simplor = Seven.simplorList.DoublePoint;
                        break;
                    default:
                        //seven[i].Simplor = Seven.simplorList.Null;
                        break;
                }
            }
        }
        private void init()
        {
            if (seven != null)
            {
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].Dispose();
                }
            }
            seven = new Seven[formatValue.Length];
            for (int i = 0; i < seven.Length; i++)
            {
                seven[i] = new Seven();
                seven[i].Width = this.Width / seven.Length - 5;
                seven[i].Height = this.Height;
                seven[i].Left = this.Width / seven.Length * i;
                seven[i].FontSize = this.Height / 10;
                seven[i].FontSpace = this.Height / 30;
                seven[i].Top = 0;
                seven[i].Shardow = false;
                switch (formatValue.Substring(i, 1))
                {
                    case "y":
                    case "M":
                    case "d":
                    case "H":
                    case "m":
                    case "s":
                        seven[i].Simplor = Seven.simplorList.Value;
                        break;
                    case "-":
                        seven[i].Simplor = Seven.simplorList.Del;
                        break;
                    case ":":
                        seven[i].Simplor = Seven.simplorList.DoublePoint;
                        break;
                    default:
                        seven[i].Simplor = Seven.simplorList.Null;
                        break;
                }
                this.Controls.Add(seven[i]);
            }
            SetValue(now);
        }
        public DateTime()
        {
            init();
            InitializeComponent();
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
            if (seven == null)
            {
                init();
            }
            else
            {
                for (int i = 0; i < seven.Length; i++)
                {
                    seven[i].Width = this.Width / seven.Length - 5;
                    seven[i].Height = this.Height;
                    seven[i].Left = this.Width / seven.Length * i;
                    seven[i].FontSize = this.Height / 10;
                    seven[i].FontSpace = this.Height / 30;
                    seven[i].Top = 0;
                }
            }
            base.OnSizeChanged(e);
        }
    }
}
