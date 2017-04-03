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
    public partial class AboutWindow : BestWindow
    {
		bool isNeedRegion = true;
        delegate void GetLocalCodeHandle();
        public AboutWindow(string SystemName,bool IsNeedRegion)
        {
            InitializeComponent();
            lblName.Text = SystemName;
			isNeedRegion = IsNeedRegion;
        }
        private void GetLocalCode()
        {
            lblID.Text = All.Class.Code.Region.Encryption();
            if (isNeedRegion)
            {
                lblCode.Text = All.Class.Region.ReadValue(All.Class.Code.Region.RegionCode);
            }
            else
            {
                lblCode.Text = "无须单独授权";
            }
            txtCode.Text = lblCode.Text;
        }

        private void AboutWindow_Load(object sender, EventArgs e)
        {
            this.BeginInvoke(new GetLocalCodeHandle(GetLocalCode));

        }

        private void lblCode_Click(object sender, EventArgs e)
        {
            txtCode.Left = lblCode.Location.X;
            txtCode.Top = lblCode.Location.Y - 4;
            txtCode.Size = lblCode.Size;
            txtCode.Text = lblCode.Text;
            txtCode.Visible = true;
            lblCode.Visible = false;
        }
        private void txtCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    lblCode.Visible = true;
                    txtCode.Visible = false;
                    break;
                case Keys.Enter:
                    lblCode.Visible = true;
                    txtCode.Visible = false;
                    lblCode.Text = txtCode.Text;
                    string error = "";
                    if (Class.Code.Region.isRegion(lblID.Text, lblCode.Text,out error)==All.Class.Code.Region.ErrorList.注册码合格)
                    {
                        Class.Code.Region.WriteInfoToReion(lblID.Text, lblCode.Text);
                        MessageBox.Show("程序已成功注册,十分感谢您的使用", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("对不起,您输入的注册不正确,请输入正确的注册码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }

        }
    }
}
