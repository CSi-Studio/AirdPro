namespace AirdPro.Forms
{
    partial class MSIImageForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnShowImage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TbAirdFile = new System.Windows.Forms.TextBox();
            this.BtnSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TbMZ = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.PnlImage = new System.Windows.Forms.Panel();
            this.PnlParams = new System.Windows.Forms.Panel();
            this.LbParams = new System.Windows.Forms.ListBox();
            this.PbMsiImage = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.PnlImage.SuspendLayout();
            this.PnlParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbMsiImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnShowImage);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.TbAirdFile);
            this.panel2.Controls.Add(this.BtnSelect);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.TbMZ);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1399, 62);
            this.panel2.TabIndex = 4;
            // 
            // BtnShowImage
            // 
            this.BtnShowImage.Location = new System.Drawing.Point(1210, 16);
            this.BtnShowImage.Name = "BtnShowImage";
            this.BtnShowImage.Size = new System.Drawing.Size(177, 39);
            this.BtnShowImage.TabIndex = 6;
            this.BtnShowImage.Text = "show image viewer";
            this.BtnShowImage.UseVisualStyleBackColor = true;
            this.BtnShowImage.Click += new System.EventHandler(this.BtnShowImage_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "*.Aird";
            // 
            // TbAirdFile
            // 
            this.TbAirdFile.Location = new System.Drawing.Point(80, 21);
            this.TbAirdFile.Name = "TbAirdFile";
            this.TbAirdFile.Size = new System.Drawing.Size(853, 28);
            this.TbAirdFile.TabIndex = 4;
            // 
            // BtnSelect
            // 
            this.BtnSelect.Location = new System.Drawing.Point(939, 16);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(75, 39);
            this.BtnSelect.TabIndex = 3;
            this.BtnSelect.Text = "Select";
            this.BtnSelect.UseVisualStyleBackColor = true;
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1033, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "m/z";
            // 
            // TbMZ
            // 
            this.TbMZ.Location = new System.Drawing.Point(1075, 23);
            this.TbMZ.Name = "TbMZ";
            this.TbMZ.Size = new System.Drawing.Size(124, 28);
            this.TbMZ.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.PnlImage);
            this.panel3.Controls.Add(this.PnlParams);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1399, 743);
            this.panel3.TabIndex = 5;
            // 
            // PnlImage
            // 
            this.PnlImage.Controls.Add(this.PbMsiImage);
            this.PnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlImage.Location = new System.Drawing.Point(331, 0);
            this.PnlImage.Name = "PnlImage";
            this.PnlImage.Size = new System.Drawing.Size(1068, 743);
            this.PnlImage.TabIndex = 1;
            // 
            // PnlParams
            // 
            this.PnlParams.Controls.Add(this.LbParams);
            this.PnlParams.Dock = System.Windows.Forms.DockStyle.Left;
            this.PnlParams.Location = new System.Drawing.Point(0, 0);
            this.PnlParams.Name = "PnlParams";
            this.PnlParams.Size = new System.Drawing.Size(331, 743);
            this.PnlParams.TabIndex = 0;
            // 
            // LbParams
            // 
            this.LbParams.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbParams.FormattingEnabled = true;
            this.LbParams.ItemHeight = 18;
            this.LbParams.Location = new System.Drawing.Point(0, 0);
            this.LbParams.Name = "LbParams";
            this.LbParams.Size = new System.Drawing.Size(331, 743);
            this.LbParams.TabIndex = 0;
            // 
            // PbMsiImage
            // 
            this.PbMsiImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PbMsiImage.Location = new System.Drawing.Point(0, 0);
            this.PbMsiImage.Name = "PbMsiImage";
            this.PbMsiImage.Size = new System.Drawing.Size(1068, 743);
            this.PbMsiImage.TabIndex = 0;
            this.PbMsiImage.TabStop = false;
            // 
            // MSIImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1399, 805);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "MSIImageForm";
            this.Text = "MSI Image Viewer";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.PnlImage.ResumeLayout(false);
            this.PnlParams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbMsiImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel PnlParams;
        private System.Windows.Forms.Panel PnlImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbMZ;
        private System.Windows.Forms.Button BtnShowImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TbAirdFile;
        private System.Windows.Forms.Button BtnSelect;
        private System.Windows.Forms.ListBox LbParams;
        private System.Windows.Forms.PictureBox PbMsiImage;
    }
}