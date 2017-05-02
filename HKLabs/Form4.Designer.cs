namespace HKLabs
{
    partial class Form4
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.combobox1 = new HKLabs.combobox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.button1 = new All.Control.Metro.Button();
            this.SuspendLayout();
            // 
            // combobox1
            // 
            this.combobox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.combobox1.BBB = null;
            this.combobox1.FormattingEnabled = true;
            this.combobox1.Location = new System.Drawing.Point(579, 43);
            this.combobox1.Name = "combobox1";
            this.combobox1.Size = new System.Drawing.Size(157, 20);
            this.combobox1.TabIndex = 3;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(579, 81);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.SelectedObject = this.combobox1;
            this.propertyGrid1.Size = new System.Drawing.Size(243, 505);
            this.propertyGrid1.TabIndex = 4;
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(63, 98);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.SelectedObject = this;
            this.propertyGrid2.Size = new System.Drawing.Size(315, 500);
            this.propertyGrid2.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button1.Boarder = true;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button1.Location = new System.Drawing.Point(412, 42);
            this.button1.MinimumSize = new System.Drawing.Size(10, 10);
            this.button1.Name = "button1";
            this.button1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.button1.Size = new System.Drawing.Size(153, 56);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form4
            // 
            this.AAA = typeof(HKLabs.Tabel);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 624);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.combobox1);
            this.Name = "Form4";
            this.Text = "Form4";
            this.Controls.SetChildIndex(this.combobox1, 0);
            this.Controls.SetChildIndex(this.propertyGrid1, 0);
            this.Controls.SetChildIndex(this.propertyGrid2, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private combobox combobox1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private All.Control.Metro.Button button1;
    }
}