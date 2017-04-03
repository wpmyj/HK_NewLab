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
    public partial class LoginWindow :BestWindow
    {
        List<string> name;
        List<string> password;
        List<string> level;
        string currentLevel = "";
        All.Class.cUser user;
        public string CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }
        string userName = "";
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public LoginWindow(All.Class.cUser user)
        {
            this.name = user.UserName;
            this.password = user.Password;
            this.level = user.Level;
            this.user = user;
            InitializeComponent();
        }

        private void LoginWindow_Load(object sender, EventArgs e)
        {
            Init();
        }
        private void Init()
        {
            cbbName.Items.Clear();
            for (int i = 0; i < name.Count && i < password.Count && i < level.Count; i++)
            {
                cbbName.Items.Add(name[i]);
            }
            if (cbbName.Items.Count > 0)
            {
                cbbName.SelectedIndex = cbbName.Items.Count - 1;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (cbbName.SelectedIndex < 0)
            {
                cbbName.Focus();
                MessageWindow mw = new MessageWindow("请选择要登陆的用户", "错误", MessageWindow.EButton.OK, MessageWindow.EIcon.Error);
                mw.ShowDialog();
                return;
            }
            if (txtPassword.Text.ToUpper() != password[cbbName.SelectedIndex].ToUpper())
            {
                txtPassword.Focus();
                MessageWindow mw = new MessageWindow("对不起,输入的用户名和密码不匹配", "错误", MessageWindow.EButton.OK, MessageWindow.EIcon.Error);
                mw.ShowDialog();
                return;
            }
            userName = name[cbbName.SelectedIndex];
            currentLevel = level[cbbName.SelectedIndex];
            user.CurrentUser = userName;
            user.CurrentLevel = currentLevel;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
