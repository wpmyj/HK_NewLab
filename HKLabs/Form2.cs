using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HKLabs
{
    public partial class Form2 : All.Window.StartWindow
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            All.Data.SqlServer sq = new All.Data.SqlServer();
            string sql = "";
            Dictionary<string, string> buff = new Dictionary<string, string>();
            buff.Add("Address","
            if (sq.Login("10.46.1.42", "Main", "sa", "123456"))
            {
                using (DataTable dt = sq.Read("select * from tb_dp where LineName='G3'"))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sql = "'通用'";
                            for (int j = 1; j < 20; j++)
                            {
                                switch (j)
                                {
                                    case 3:
                                    case 6:
                                    case 7:
                                    case 11:
                                    case 12:
                                        sql = string.Format("{0},", sql, dt.Rows[i][j]);
                                        break;
                                    default:
                                        sql = string.Format("{0},'", sql);
                                        break;
                                }
                                sql = string.Format("{0}{1}", sql, dt.Rows[i][j]);

                                switch (j)
                                {
                                    case 3:
                                    case 6:
                                    case 7:
                                    case 11:
                                    case 12:
                                        break;
                                    default:
                                        sql = string.Format("{0}'", sql);
                                        break;
                                }
                            }
                            if (sq.Write(string.Format("insert into tb_dp values ({0})", sql)) <= 0)
                            {
                            }
                        }
                    }
                }
            }
        }

        void Form2_LoadOver(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.ShowDialog();
        }
        private void f(string ss, string s2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, string>(f), ss, s2);
            }
        }
    }
}
