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
    public partial class frmChangeUser : BestWindow
    {
        All.Class.cUser alluser;
        public frmChangeUser(All.Class.cUser user)
        {
            alluser = user;
            InitializeComponent();
        }

        private void frmChangeUser_Load(object sender, EventArgs e)
        {
            Init();
        }
        private void Init()
        {
            cbbName.Items.Clear();
            for (int i = 0; i < alluser.AllUser.Count; i++)
            {
                cbbName.Items.Add(alluser.AllUser[i].UserName);
            }
            if (cbbName.Items.Count > 0)
            {
                cbbName.SelectedIndex = cbbName.Items.Count - 1;
            }
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            All.Window.MessageWindow mw;
            if (cbbName.SelectedIndex < 0)
            {
                cbbName.Focus();
                mw = new MessageWindow("请选择要修改的用户", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mw.ShowDialog();
                return;
            }
            if (txtOldPassword.Text.ToUpper() != alluser.AllUser[cbbName.SelectedIndex].PassWord.ToUpper())
            {
                txtPassword.Focus();
                mw = new MessageWindow("对不起,输入的用户名和原始密码不匹配", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mw.ShowDialog();
                return;
            }
            if (txtPassword.Text != txtPasswordAgain.Text)
            {
                txtPassword.Focus();
                mw = new All.Window.MessageWindow("两次输入的密码不一致,请重新输入密码!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mw.ShowDialog();
                return;
            }
            alluser.AllUser[cbbName.SelectedIndex].PassWord = txtPassword.Text;
            alluser.AllUser[cbbName.SelectedIndex].Save();
            mw = new All.Window.MessageWindow(string.Format("[{0}] 用户已修改成功!", cbbName.Text.Trim()), "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mw.ShowDialog();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            MessageWindow mw;
            if (cbbName.Text == "Administrator")
            {
                mw = new MessageWindow("对不起,[Administratro]用户是默认管理员账户,不能删除", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cbbName.SelectedIndex < 0)
            {
                cbbName.Focus();
                mw = new MessageWindow("请选择要删除的用户", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mw.ShowDialog();
                return;
            }
            //if (txtOldPassword.Text.ToUpper() != password[cbbName.SelectedIndex].ToUpper())
            //{
            //    txtPassword.Focus();
            //    MessageWindow mw = new MessageWindow("对不起,输入的用户名和原始密码不匹配", "错误", MessageWindow.EButton.OK, MessageWindow.EIcon.Error);
            //    mw.ShowDialog();
            //    return;
            //}

            alluser.AllUser.RemoveAt(alluser.AllUser.FindIndex(tmp => tmp.UserName == cbbName.Text));
            All.Class.cUser.Delete(cbbName.Text);
            mw = new MessageWindow("当前选中用户已成功删除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            mw.ShowDialog();
            Init();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
