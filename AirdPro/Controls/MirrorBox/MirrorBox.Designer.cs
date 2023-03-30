namespace AirdPro.Controls.MirrorBox
{
    partial class MirrorBox
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MirrorBox));
            this.btnVendorFolder = new System.Windows.Forms.Button();
            this.btnAirdFolder = new System.Windows.Forms.Button();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.tbVendor = new System.Windows.Forms.TextBox();
            this.tbAird = new System.Windows.Forms.TextBox();
            this.tipVendor = new System.Windows.Forms.ToolTip(this.components);
            this.tipAird = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnVendorFolder
            // 
            this.btnVendorFolder.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnVendorFolder.Location = new System.Drawing.Point(299, 3);
            this.btnVendorFolder.Name = "btnVendorFolder";
            this.btnVendorFolder.Size = new System.Drawing.Size(36, 23);
            this.btnVendorFolder.TabIndex = 0;
            this.btnVendorFolder.Text = "...";
            this.btnVendorFolder.UseVisualStyleBackColor = true;
            this.btnVendorFolder.Click += new System.EventHandler(this.btnVendorFolder_Click);
            // 
            // btnAirdFolder
            // 
            this.btnAirdFolder.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAirdFolder.Location = new System.Drawing.Point(661, 3);
            this.btnAirdFolder.Name = "btnAirdFolder";
            this.btnAirdFolder.Size = new System.Drawing.Size(36, 23);
            this.btnAirdFolder.TabIndex = 1;
            this.btnAirdFolder.Text = "...";
            this.btnAirdFolder.UseVisualStyleBackColor = true;
            this.btnAirdFolder.Click += new System.EventHandler(this.btnAirdFolder_Click);
            // 
            // picBox
            // 
            this.picBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picBox.BackgroundImage")));
            this.picBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picBox.InitialImage = null;
            this.picBox.Location = new System.Drawing.Point(343, 2);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(25, 23);
            this.picBox.TabIndex = 2;
            this.picBox.TabStop = false;
            // 
            // tbVendor
            // 
            this.tbVendor.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.tbVendor.Location = new System.Drawing.Point(10, 5);
            this.tbVendor.Name = "tbVendor";
            this.tbVendor.Size = new System.Drawing.Size(280, 20);
            this.tbVendor.TabIndex = 3;
            // 
            // tbAird
            // 
            this.tbAird.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.tbAird.Location = new System.Drawing.Point(376, 5);
            this.tbAird.Name = "tbAird";
            this.tbAird.Size = new System.Drawing.Size(280, 20);
            this.tbAird.TabIndex = 4;
            // 
            // tipVendor
            // 
            this.tipVendor.ToolTipTitle = "Select Vendor File Repository";
            // 
            // tipAird
            // 
            this.tipAird.ToolTipTitle = "Select Aird Repository";
            // 
            // MirrorBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbAird);
            this.Controls.Add(this.tbVendor);
            this.Controls.Add(this.picBox);
            this.Controls.Add(this.btnAirdFolder);
            this.Controls.Add(this.btnVendorFolder);
            this.Name = "MirrorBox";
            this.Size = new System.Drawing.Size(700, 30);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVendorFolder;
        private System.Windows.Forms.Button btnAirdFolder;
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.TextBox tbVendor;
        private System.Windows.Forms.TextBox tbAird;
        private System.Windows.Forms.ToolTip tipVendor;
        private System.Windows.Forms.ToolTip tipAird;
    }
}
