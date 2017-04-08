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
    public partial class Region : BestWindow
    {
        public Region()
        {
            InitializeComponent();
        }

        private void Region_Load(object sender, EventArgs e)
        {
            txtLocal.Text = Class.Code.Region.Encryption();

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            MessageWindow mw = new MessageWindow("输入的注册码正确,系统已成功注册", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            string str = "";
            Class.Code.Region.ErrorList result = Class.Code.Region.isRegion(txtLocal.Text, txtPassword.Text,out str);
            switch (result)
            {
                case All.Class.Code.Region.ErrorList.注册码合格:
                    Class.Code.Region.WriteInfoToReion(txtLocal.Text, txtPassword.Text);
                    break;
                default:
                    mw = new MessageWindow(str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            mw.ShowDialog();
            mw.Dispose();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
