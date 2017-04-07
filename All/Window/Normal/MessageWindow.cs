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
    public partial class MessageWindow : BestWindow
    {
        public enum EIcon
        {
            Alert,
            Information,
            Error,
            Question
        }
        public enum EButton
        {
            OK,
            YesNo,
            OKCancel
        }
        [Obsolete("过期")]
        public static DialogResult Show(string text, string title, EButton button, EIcon icon)
        {
            using (MessageWindow mw = new MessageWindow(text, title, button, icon))
            {
                return mw.ShowDialog();
            }
        }
        [Obsolete("过期")]
        public static DialogResult Show(string text, string title, EButton button)
        {
            using (MessageWindow mw = new MessageWindow(text, title, button))
            {
                return mw.ShowDialog();
            }
        }
        [Obsolete("过期")]
        public MessageWindow(string text, string title, EButton button)
            : this(text, title,button,EIcon.Information)
        {
        }
        [Obsolete("过期")]
        public MessageWindow(string text, string title, EButton button, EIcon icon)
        {
            InitializeComponent();
            lblText.Text = text;
            this.Text = title;
            this.Width = lblText.Left + 10 + (int)lblText.CreateGraphics().MeasureString(lblText.Text, lblText.Font).Width;
            this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
            this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2; switch (icon)
            {
                case EIcon.Information:
                case EIcon.Alert:
                    pictureBox1.Image = All.Properties.Resources.information;
                    break;
                case EIcon.Error:
                    pictureBox1.Image = All.Properties.Resources.error1;
                    break;
                case EIcon.Question:
                    pictureBox1.Image = All.Properties.Resources.question2;
                    break;
            }
            switch (button)
            {
                case EButton.OK:
                    btnCancel.Visible = false;
                    btnOk.Visible = true;
                    btnOk.Left = this.Width / 2 - btnOk.Width / 2;
                    btnOk.Text = "确定";
                    break;
                case EButton.OKCancel:
                    btnCancel.Visible = true;
                    btnOk.Visible = true;
                    btnOk.Left = this.Width / 2 - btnOk.Width - 10;
                    btnCancel.Left = this.Width / 2 + 10;
                    btnOk.Text = "确定";
                    btnCancel.Text = "取消";
                    break;
                case EButton.YesNo:
                    btnCancel.Visible = true;
                    btnOk.Visible = true;
                    btnOk.Left = this.Width / 2 - btnOk.Width - 10;
                    btnCancel.Left = this.Width / 2 + 10;
                    btnOk.Text = "是";
                    btnCancel.Text = "否";
                    break;
            }
            lblText.Text = text;
            this.Text = title;
        }
        public static DialogResult Show(string text, string title, MessageBoxButtons button, MessageBoxIcon icon)
        {
            using (MessageWindow mw = new MessageWindow(text, title, button, icon))
            {
                return mw.ShowDialog();
            }
        }
        public MessageWindow(string text, string title, MessageBoxButtons button, MessageBoxIcon icon)
        {
            InitializeComponent();
            lblText.Text = text;
            this.Text = title;
            this.Width = lblText.Left + 10 + (int)lblText.CreateGraphics().MeasureString(lblText.Text, lblText.Font).Width;
            this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
            this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                case MessageBoxIcon.Exclamation:
                case MessageBoxIcon.None:
                    pictureBox1.Image = All.Properties.Resources.information;
                    break;
                case MessageBoxIcon.Question:
                    pictureBox1.Image = All.Properties.Resources.question2;
                    break;
                case MessageBoxIcon.Error:
                    pictureBox1.Image = All.Properties.Resources.error1;
                    break;
            }
            switch (button)
            {
                case MessageBoxButtons.OK:
                    btnCancel.Visible = false;
                    btnOk.Visible = true;
                    btnOk.Left = this.Width / 2 - btnOk.Width / 2;
                    btnOk.Text = "确定";
                    break;
                case MessageBoxButtons.OKCancel:
                    btnCancel.Visible = true;
                    btnOk.Visible = true;
                    btnOk.Left = this.Width / 2 - btnOk.Width - 10;
                    btnCancel.Left = this.Width / 2 + 10;
                    btnOk.Text = "确定";
                    btnCancel.Text = "取消";
                    break;
                case MessageBoxButtons.RetryCancel:
                case MessageBoxButtons.AbortRetryIgnore:
                    btnCancel.Visible = true;
                    btnOk.Visible = true;
                    btnOk.Left = this.Width / 2 - btnOk.Width - 10;
                    btnCancel.Left = this.Width / 2 + 10;
                    btnOk.Text = "重试";
                    btnCancel.Text = "取消";
                    break;
                case MessageBoxButtons.YesNo:
                case MessageBoxButtons.YesNoCancel:
                    btnCancel.Visible = true;
                    btnOk.Visible = true;
                    btnOk.Left = this.Width / 2 - btnOk.Width - 10;
                    btnCancel.Left = this.Width / 2 + 10;
                    btnOk.Text = "是";
                    btnCancel.Text = "否";
                    break;
            }
        }
        public static DialogResult Show(string text, string title, MessageBoxButtons button)
        {
            using (MessageWindow mw = new MessageWindow(text, title, button))
            {
                return mw.ShowDialog();
            }
        }
        public MessageWindow(string text, string title, MessageBoxButtons button)
            : this(text, title,button,MessageBoxIcon.Information)
        {
        }
        public static DialogResult Show(string Text, string Title)
        {
            using (MessageWindow mw = new MessageWindow(Text, Title))
            {
                return mw.ShowDialog();
            }
        }
        public MessageWindow(string text, string title)
            :this(text,title,MessageBoxButtons.OK)
        {
        }
        public static DialogResult Show(string text)
        {
            using (MessageWindow mw = new MessageWindow(text))
            {
                return mw.ShowDialog();
            }
        }
        public MessageWindow(string text)
            : this(text, "")
        {
        }
        private void MessageWindow_Load(object sender, EventArgs e)
        {
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (btnOk.Text.IndexOf("确定") >= 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            if (btnOk.Text.IndexOf("是") >= 0)
            {
                this.DialogResult = DialogResult.Yes;
            }
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text.IndexOf("取消") >= 0)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            if (btnCancel.Text.IndexOf("否") >= 0)
            {
                this.DialogResult = DialogResult.No;
            }
            this.Close();
        }
    }
}
