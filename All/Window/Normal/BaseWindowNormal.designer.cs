namespace All.Window
{
    partial class BaseWindowNormal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseWindowNormal));
            this.lblTitle = new System.Windows.Forms.Label();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.lblMin = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblClose = new System.Windows.Forms.Label();
            this.lblHelp = new System.Windows.Forms.Label();
            this.lblSetting = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.Location = new System.Drawing.Point(2, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(534, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.DoubleClick += new System.EventHandler(this.lblTitle_DoubleClick);
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseMove);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseUp);
            // 
            // picIcon
            // 
            this.picIcon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.picIcon.Image = global::All.Properties.Resources.information;
            this.picIcon.Location = new System.Drawing.Point(11, 7);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(20, 20);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picIcon.TabIndex = 1;
            this.picIcon.TabStop = false;
            // 
            // lblMin
            // 
            this.lblMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblMin.Font = new System.Drawing.Font("Webdings", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lblMin.ForeColor = System.Drawing.Color.White;
            this.lblMin.Location = new System.Drawing.Point(460, 7);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(20, 20);
            this.lblMin.TabIndex = 2;
            this.lblMin.Tag = "";
            this.lblMin.Text = "0";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMin.Click += new System.EventHandler(this.lblMin_Click);
            this.lblMin.MouseEnter += new System.EventHandler(this.lblButton_MouseEnter);
            this.lblMin.MouseLeave += new System.EventHandler(this.lblButton_MouseLeave);
            // 
            // lblMax
            // 
            this.lblMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblMax.Font = new System.Drawing.Font("Webdings", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lblMax.ForeColor = System.Drawing.Color.White;
            this.lblMax.Location = new System.Drawing.Point(486, 7);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(20, 20);
            this.lblMax.TabIndex = 3;
            this.lblMax.Tag = "";
            this.lblMax.Text = "1";
            this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMax.Click += new System.EventHandler(this.lblMax_Click);
            this.lblMax.MouseEnter += new System.EventHandler(this.lblButton_MouseEnter);
            this.lblMax.MouseLeave += new System.EventHandler(this.lblButton_MouseLeave);
            // 
            // lblClose
            // 
            this.lblClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblClose.Font = new System.Drawing.Font("Webdings", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lblClose.ForeColor = System.Drawing.Color.White;
            this.lblClose.Location = new System.Drawing.Point(512, 7);
            this.lblClose.Name = "lblClose";
            this.lblClose.Size = new System.Drawing.Size(20, 20);
            this.lblClose.TabIndex = 4;
            this.lblClose.Tag = "";
            this.lblClose.Text = "r";
            this.lblClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
            this.lblClose.MouseEnter += new System.EventHandler(this.lblButton_MouseEnter);
            this.lblClose.MouseLeave += new System.EventHandler(this.lblButton_MouseLeave);
            // 
            // lblHelp
            // 
            this.lblHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblHelp.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblHelp.ForeColor = System.Drawing.Color.White;
            this.lblHelp.Location = new System.Drawing.Point(425, 7);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(33, 20);
            this.lblHelp.TabIndex = 5;
            this.lblHelp.Tag = "";
            this.lblHelp.Text = "Help";
            this.lblHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHelp.MouseEnter += new System.EventHandler(this.lblButton_MouseEnter);
            this.lblHelp.MouseLeave += new System.EventHandler(this.lblButton_MouseLeave);
            // 
            // lblSetting
            // 
            this.lblSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblSetting.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSetting.ForeColor = System.Drawing.Color.White;
            this.lblSetting.Location = new System.Drawing.Point(355, 7);
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(54, 20);
            this.lblSetting.TabIndex = 6;
            this.lblSetting.Tag = "";
            this.lblSetting.Text = "Setting";
            this.lblSetting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSetting.MouseEnter += new System.EventHandler(this.lblButton_MouseEnter);
            this.lblSetting.MouseLeave += new System.EventHandler(this.lblButton_MouseLeave);
            // 
            // BaseWindowNormal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 292);
            this.Controls.Add(this.picIcon);
            this.Controls.Add(this.lblSetting);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.lblClose);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BaseWindowNormal";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Base";
            this.Load += new System.EventHandler(this.BaseWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picIcon;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblClose;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Label lblSetting;
        private System.Windows.Forms.Label lblTitle;
    }
}