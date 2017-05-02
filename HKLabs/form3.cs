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
    public partial class form3 : All.Window.BestWindow
    {

        [TypeConverter(typeof(TypeConverterTable2String))]
        public Type AAA
        { get; set; }
        public form3()
        {
            InitializeComponent();
        }

        private void form3_Load(object sender, EventArgs e)
        {

        }
    }
}
