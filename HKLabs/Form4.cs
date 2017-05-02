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
    public partial class Form4 : form3
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Reflection.PropertyInfo pi = typeof(bbbbb).GetProperties()[0];
            object[] obj = pi.GetCustomAttributes(typeof(All.Attribute.SaveAttribute), false);
            if (obj != null && obj.Length > 0)
            {
                for (int i = 0; i < obj.Length; i++)
                {
                    All.Attribute.SaveAttribute tmp = (All.Attribute.SaveAttribute)obj[i];
                    switch (tmp.Statue)
                    {
                        case All.Attribute.Statue.Key:
                            break;
                        case All.Attribute.Statue.None:
                            break;
                        case All.Attribute.Statue.AutoSave:
                            break;
                    }
                }
            }
        }
    }
    [All.Attribute.Save(All.Attribute.Statue.AutoSave)]
    public class bbbbb
    {
        [All.Attribute.Save(All.Attribute.Statue.Key)]
        public int C
        { get; set; }
    }
}
