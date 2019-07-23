namespace RadarScreen
{
    partial class 雷达幕
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(雷达幕));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.雷达幕picBox = new System.Windows.Forms.PictureBox();
            this.panel_dot = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.雷达幕picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.panel_dot);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(63, 67);
            this.panel1.TabIndex = 1;
            this.panel1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::RadarScreen.Properties.Resources.天线图标;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(24, 18);
            this.panel2.TabIndex = 0;
            // 
            // 雷达幕picBox
            // 
            this.雷达幕picBox.Location = new System.Drawing.Point(3, 3);
            this.雷达幕picBox.Name = "雷达幕picBox";
            this.雷达幕picBox.Size = new System.Drawing.Size(224, 212);
            this.雷达幕picBox.TabIndex = 0;
            this.雷达幕picBox.TabStop = false;
            // 
            // panel_dot
            // 
            this.panel_dot.BackgroundImage = global::RadarScreen.Properties.Resources.dot;
            this.panel_dot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_dot.Location = new System.Drawing.Point(3, 27);
            this.panel_dot.Name = "panel_dot";
            this.panel_dot.Size = new System.Drawing.Size(24, 24);
            this.panel_dot.TabIndex = 1;
            // 
            // 雷达幕
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.雷达幕picBox);
            this.Name = "雷达幕";
            this.Size = new System.Drawing.Size(230, 215);
            this.Load += new System.EventHandler(this.雷达幕_Load);
            this.SizeChanged += new System.EventHandler(this.雷达幕_SizeChanged);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.雷达幕picBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox 雷达幕picBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel_dot;
    }
}
