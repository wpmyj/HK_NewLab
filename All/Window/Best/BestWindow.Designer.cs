namespace All.Window
{
    partial class BestWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BestWindow));
            this.menuClose = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.还原RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.大小ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.最小化MToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.最大化XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTheme = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panTitleForMdi = new All.Control.Metro.Panel(this.components);
            this.menuClose.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuClose
            // 
            this.menuClose.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.还原RToolStripMenuItem,
            this.移动MToolStripMenuItem,
            this.大小ToolStripMenuItem,
            this.最小化MToolStripMenuItem,
            this.最大化XToolStripMenuItem,
            this.toolStripSeparator1,
            this.退出CToolStripMenuItem});
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(168, 142);
            this.menuClose.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.menuClose_Closed);
            this.menuClose.Opening += new System.ComponentModel.CancelEventHandler(this.menuClose_Opening);
            // 
            // 还原RToolStripMenuItem
            // 
            this.还原RToolStripMenuItem.Enabled = false;
            this.还原RToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("还原RToolStripMenuItem.Image")));
            this.还原RToolStripMenuItem.Name = "还原RToolStripMenuItem";
            this.还原RToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.还原RToolStripMenuItem.Text = "还原(&R)";
            this.还原RToolStripMenuItem.Click += new System.EventHandler(this.还原RToolStripMenuItem_Click);
            // 
            // 移动MToolStripMenuItem
            // 
            this.移动MToolStripMenuItem.Enabled = false;
            this.移动MToolStripMenuItem.Name = "移动MToolStripMenuItem";
            this.移动MToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.移动MToolStripMenuItem.Text = "移动(&M)";
            // 
            // 大小ToolStripMenuItem
            // 
            this.大小ToolStripMenuItem.Enabled = false;
            this.大小ToolStripMenuItem.Name = "大小ToolStripMenuItem";
            this.大小ToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.大小ToolStripMenuItem.Text = "大小(&S)";
            // 
            // 最小化MToolStripMenuItem
            // 
            this.最小化MToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("最小化MToolStripMenuItem.Image")));
            this.最小化MToolStripMenuItem.Name = "最小化MToolStripMenuItem";
            this.最小化MToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.最小化MToolStripMenuItem.Text = "最小化(&N)";
            this.最小化MToolStripMenuItem.Click += new System.EventHandler(this.最小化MToolStripMenuItem_Click);
            // 
            // 最大化XToolStripMenuItem
            // 
            this.最大化XToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("最大化XToolStripMenuItem.Image")));
            this.最大化XToolStripMenuItem.Name = "最大化XToolStripMenuItem";
            this.最大化XToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.最大化XToolStripMenuItem.Text = "最大化(&X)";
            this.最大化XToolStripMenuItem.Click += new System.EventHandler(this.最大化XToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
            // 
            // 退出CToolStripMenuItem
            // 
            this.退出CToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.退出CToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("退出CToolStripMenuItem.Image")));
            this.退出CToolStripMenuItem.Name = "退出CToolStripMenuItem";
            this.退出CToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.退出CToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.退出CToolStripMenuItem.Text = "关闭(&C)";
            this.退出CToolStripMenuItem.Click += new System.EventHandler(this.退出CToolStripMenuItem_Click);
            // 
            // menuTheme
            // 
            this.menuTheme.Name = "menuTheme";
            this.menuTheme.Size = new System.Drawing.Size(61, 4);
            this.menuTheme.Opening += new System.ComponentModel.CancelEventHandler(this.menuTheme_Opening);
            // 
            // panTitleForMdi
            // 
            this.panTitleForMdi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panTitleForMdi.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.panTitleForMdi.Location = new System.Drawing.Point(0, 0);
            this.panTitleForMdi.Name = "panTitleForMdi";
            this.panTitleForMdi.Size = new System.Drawing.Size(800, 26);
            this.panTitleForMdi.TabIndex = 2;
            this.panTitleForMdi.Paint += new System.Windows.Forms.PaintEventHandler(this.panTitleForMdi_Paint);
            this.panTitleForMdi.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDown);
            this.panTitleForMdi.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);
            this.panTitleForMdi.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUp);
            // 
            // BestWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.panTitleForMdi);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "BestWindow";
            this.Text = "NormalWindow";
            this.Load += new System.EventHandler(this.NormalWindow_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BestWindow_KeyPress);
            this.menuClose.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuClose;
        private System.Windows.Forms.ToolStripMenuItem 还原RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 大小ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最小化MToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最大化XToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出CToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip menuTheme;
        private Control.Metro.Panel panTitleForMdi;
    }
}