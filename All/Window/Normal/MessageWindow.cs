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
        public static DialogResult Show(string Text, string Title, EButton button, EIcon icon)
        {
            using (MessageWindow MW = new MessageWindow(Text, Title, button, icon))
            {
                return MW.ShowDialog();
            }            
        }
        public MessageWindow(string Text, string Title, EButton button, EIcon icon)
            : this(Text, Title, button)
        {
            switch (icon)
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
        }
        public static DialogResult Show(string Text, string Title, EButton button)
        {
            using (MessageWindow MW = new MessageWindow(Text, Title, button))
            {
                return MW.ShowDialog();
            }
        }
        public MessageWindow(string Text, string Title, EButton button)
            : this(Text, Title)
        {
            int width = (int)lblText.CreateGraphics().MeasureString(lblText.Text, lblText.Font).Width;
            switch (button)
            {
                case EButton.OK:
                    btnCancel.Visible = false;
                    btnOk.Visible = true;
                    btnOk.Left = width/ 2 + lblText.Left - btnOk.Width / 2;
                    btnOk.Text = "确定[&O]";
                    break;
                case EButton.YesNo:
                    btnCancel.Visible = true;
                    btnOk.Visible = true;
                    btnOk.Left = (width - btnOk.Width * 2) / 3 + lblText.Left;
                    btnCancel.Left = (width - btnOk.Width * 2) / 3 + btnOk.Left+btnOk.Width;
                    btnOk.Text = "是[&Y]";
                    btnCancel.Text = "否[&N]";
                    break;
                case EButton.OKCancel:
                    btnCancel.Visible = true;
                    btnOk.Visible = true;
                    btnOk.Left = (width - btnOk.Width * 2) / 3 + lblText.Left;
                    btnCancel.Left = (width - btnOk.Width * 2) / 3 + btnOk.Left + btnOk.Width;
                    btnOk.Text = "确定[&O]";
                    btnCancel.Text = "取消[&C]";
                    break;
            }
        }
        public static DialogResult Show(string Text, string Title)
        {
            using (MessageWindow MW = new MessageWindow(Text, Title))
            {
                return MW.ShowDialog();
            }
        }
        public MessageWindow(string Text, string Title)
            :this(Text)
        {
            string tmp = "";
            for (int i = 0; i < Title.Length; i++)
            {
                tmp = string.Format("{0} {1}", tmp, Title.Substring(i, 1));
            }
            tmp = string.Format("{0} ", tmp);
            this.Text = tmp;
        }
        public static DialogResult Show(string Text)
        {
            using (MessageWindow MW = new MessageWindow(Text))
            {
                return MW.ShowDialog();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        public MessageWindow(string Text)
        {
            InitializeComponent();
            lblText.Text = Text;
        }

        private void MessageWindow_Load(object sender, EventArgs e)
        {
            this.Width = lblText.Left + 10 + (int)lblText.CreateGraphics().MeasureString(lblText.Text, lblText.Font).Width;
            this.Top = Screen.PrimaryScreen.Bounds.Height / 2 - this.Height / 2;
            this.Left = Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2;
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
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
