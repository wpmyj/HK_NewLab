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
    public partial class MessageBox : System.Windows.Forms.Form
    {
        Color Default = Color.FromArgb(57, 179, 215);
        Color Error = Color.FromArgb(210, 50, 45);
        Color Warnning = Color.FromArgb(237, 156, 40);
        Color Success = Color.FromArgb(0, 170, 173);
        Color Question = Color.FromArgb(71, 164, 71);
        public MessageBox()
        {
            InitializeComponent();
        }
        public MessageBox(string text)
            : this(text, "")
        { }
        public MessageBox(string text, string caption)
            : this(text, caption, MessageBoxButtons.OK)
        { }
        public MessageBox(string text, string caption, MessageBoxButtons buttons)
            : this(text, caption, buttons, MessageBoxIcon.None)
        { }
        public MessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
            : this(text, caption, buttons, icon, MessageBoxDefaultButton.Button1)
        { }
        public MessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
            : this(text, caption, buttons, icon, defaultButton, false)
        { }
        public MessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, bool fullWindows)
            : this()
        {
            lblValue.Text = text;
            lblTitle.Text = caption;
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    button1.Text = "确定";
                    button1.Tag = DialogResult.OK;
                    button1.Location = button3.Location;
                    button2.Visible = false;
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.OKCancel:
                    button1.Text = "确定";
                    button2.Text = "取消";
                    button1.Tag = DialogResult.OK;
                    button2.Tag = DialogResult.Cancel;
                    button1.Location = button2.Location;
                    button2.Location = button3.Location;
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.RetryCancel:
                    button1.Text = "重试";
                    button2.Text = "取消";
                    button1.Tag = DialogResult.Retry;
                    button2.Tag = DialogResult.Cancel;
                    button1.Location = button2.Location;
                    button2.Location = button3.Location;
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.YesNo:
                    button1.Text = "是";
                    button2.Text = "否";
                    button1.Tag = DialogResult.Yes;
                    button2.Tag = DialogResult.No;
                    button1.Location = button2.Location;
                    button2.Location = button3.Location;
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    button1.Text = "是";
                    button2.Text = "否";
                    button3.Text = "取消";
                    button1.Tag = DialogResult.Yes;
                    button2.Tag = DialogResult.No;
                    button3.Tag = DialogResult.Cancel;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    button1.Text = "取消";
                    button2.Text = "重试";
                    button3.Text = "忽略";
                    button1.Tag = DialogResult.Abort;
                    button2.Tag = DialogResult.Retry;
                    button3.Tag = DialogResult.Ignore;
                    break;
            }
            switch (icon)
            {
                case MessageBoxIcon.Information:
                    this.BackColor = Success;
                    break;
                case MessageBoxIcon.Error:
                    this.BackColor = Error;
                    break;
                case MessageBoxIcon.Warning:
                    this.BackColor = Warnning;
                    break;
                case MessageBoxIcon.Question:
                    this.BackColor = Question;
                    break;
                case MessageBoxIcon.None:
                    this.BackColor = Default;
                    break;
            }
            switch (defaultButton)
            {
                case MessageBoxDefaultButton.Button1:
                    button1.Focus();
                    break;
                case MessageBoxDefaultButton.Button2:
                    button2.Focus();
                    break;
                case MessageBoxDefaultButton.Button3:
                    button3.Focus();
                    break;
            }
            button1.Click += button_Click;
            button2.Click += button_Click;
            button3.Click += button_Click;

            if (fullWindows)
            {
                this.Width = Screen.GetWorkingArea(this).Width;
            }
            this.Left = Screen.GetWorkingArea(this).Width / 2 - this.Width / 2;
            this.Top = Screen.GetWorkingArea(this).Height / 2 - this.Height / 2;
        }
        void button_Click(object sender, EventArgs e)
        {
            All.Control.Metro.Button btn = (All.Control.Metro.Button)sender;
            this.DialogResult = (DialogResult)btn.Tag;
            this.Close();
        }
        /// <summary>
        /// 显示包含文本,按钮和图标的消息框
        /// </summary>
        /// <param name="text">消息中要显示的内容</param>
        /// <returns>返回用户处理消息结果</returns>
        public static DialogResult Show(string text)
        {
            return Show(text, "");
        }
        /// <summary>
        /// 显示包含文本,按钮和图标的消息框
        /// </summary>
        /// <param name="text">消息中要显示的内容</param>
        /// <param name="caption">消息中要显示的标题</param>
        /// <returns>返回用户处理消息结果</returns>
        public static DialogResult Show(string text, string caption)
        {
            return Show(text, caption, MessageBoxButtons.OK);
        }
        /// <summary>
        /// 显示包含文本,按钮和图标的消息框
        /// </summary>
        /// <param name="text">消息中要显示的内容</param>
        /// <param name="caption">消息中要显示的标题</param>
        /// <param name="buttons">消息中要显示的按钮</param>
        /// <returns>返回用户处理消息结果</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Show(text, caption, buttons, MessageBoxIcon.None);
        }
        /// <summary>
        /// 显示包含文本,按钮和图标的消息框
        /// </summary>
        /// <param name="text">消息中要显示的内容</param>
        /// <param name="caption">消息中要显示的标题</param>
        /// <param name="buttons">消息中要显示的按钮</param>
        /// <param name="icon">消息中要显示的图标</param>
        /// <returns>返回用户处理消息结果</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }
        /// <summary>
        /// 显示包含文本,按钮和图标的消息框
        /// </summary>
        /// <param name="text">消息中要显示的内容</param>
        /// <param name="caption">消息中要显示的标题</param>
        /// <param name="buttons">消息中要显示的按钮</param>
        /// <param name="icon">消息中要显示的图标</param>
        /// <param name="defaultButton">消息中的默认按钮</param>
        /// <returns>返回用户处理消息结果</returns>
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(text, caption, buttons, icon, defaultButton, false);
        }
        public static DialogResult Show(string text,string caption,MessageBoxButtons buttons,MessageBoxIcon icon,MessageBoxDefaultButton defaultButton,bool fullWindows)
        {
            DialogResult result = DialogResult.OK;
            using (MessageBox mb = new MessageBox(text, caption, buttons, icon, defaultButton,fullWindows))
            {
                result = mb.ShowDialog();
            }
            return result;
        }
    }
}
