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
    public partial class RegionCreate : BaseWindowNormal
    {
        public RegionCreate()
        {
            InitializeComponent();
        }

        private void RegionCreate_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            txtPassword.Text = Class.Code.Region.Decryption(txtLocal.Text,dateTimePicker1.Value);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
