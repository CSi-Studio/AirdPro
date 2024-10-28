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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MSIImageForm));
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnShowMS = new System.Windows.Forms.Button();
            this.TbScanNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnShowImage = new System.Windows.Forms.Button();
            this.TbAirdFile = new System.Windows.Forms.TextBox();
            this.BtnAirdImport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TbMz = new System.Windows.Forms.TextBox();
            this.webViewMSI = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.LbAirdInfo = new System.Windows.Forms.ListBox();
            this.webViewMS = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewMSI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webViewMS)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnShowMS);
            this.panel2.Controls.Add(this.TbScanNumber);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.BtnShowImage);
            this.panel2.Controls.Add(this.TbAirdFile);
            this.panel2.Controls.Add(this.BtnAirdImport);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.TbMz);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2295, 66);
            this.panel2.TabIndex = 4;
            // 
            // BtnShowMS
            // 
            this.BtnShowMS.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnShowMS.Location = new System.Drawing.Point(1721, 17);
            this.BtnShowMS.Name = "BtnShowMS";
            this.BtnShowMS.Size = new System.Drawing.Size(176, 39);
            this.BtnShowMS.TabIndex = 9;
            this.BtnShowMS.Text = "Show Spectrum";
            this.BtnShowMS.UseVisualStyleBackColor = true;
            this.BtnShowMS.Click += new System.EventHandler(this.BtnShowMS_Click);
            // 
            // TbScanNumber
            // 
            this.TbScanNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbScanNumber.Location = new System.Drawing.Point(1591, 20);
            this.TbScanNumber.Name = "TbScanNumber";
            this.TbScanNumber.Size = new System.Drawing.Size(124, 30);
            this.TbScanNumber.TabIndex = 8;
            this.TbScanNumber.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(1523, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Scan #";
            // 
            // BtnShowImage
            // 
            this.BtnShowImage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnShowImage.Location = new System.Drawing.Point(2098, 17);
            this.BtnShowImage.Name = "BtnShowImage";
            this.BtnShowImage.Size = new System.Drawing.Size(177, 39);
            this.BtnShowImage.TabIndex = 6;
            this.BtnShowImage.Text = "Show MSI Image";
            this.BtnShowImage.UseVisualStyleBackColor = true;
            this.BtnShowImage.Click += new System.EventHandler(this.BtnShowImage_Click);
            // 
            // TbAirdFile
            // 
            this.TbAirdFile.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbAirdFile.Location = new System.Drawing.Point(12, 21);
            this.TbAirdFile.Name = "TbAirdFile";
            this.TbAirdFile.Size = new System.Drawing.Size(942, 30);
            this.TbAirdFile.TabIndex = 4;
            // 
            // BtnAirdImport
            // 
            this.BtnAirdImport.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnAirdImport.Location = new System.Drawing.Point(960, 17);
            this.BtnAirdImport.Name = "BtnAirdImport";
            this.BtnAirdImport.Size = new System.Drawing.Size(100, 39);
            this.BtnAirdImport.TabIndex = 3;
            this.BtnAirdImport.Text = "*.aird";
            this.BtnAirdImport.UseVisualStyleBackColor = true;
            this.BtnAirdImport.Click += new System.EventHandler(this.BtnAirdImport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(1918, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "m/z";
            // 
            // TbMz
            // 
            this.TbMz.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbMz.Location = new System.Drawing.Point(1968, 19);
            this.TbMz.Name = "TbMz";
            this.TbMz.Size = new System.Drawing.Size(124, 30);
            this.TbMz.TabIndex = 1;
            // 
            // webViewMSI
            // 
            this.webViewMSI.AllowExternalDrop = true;
            this.webViewMSI.CreationProperties = null;
            this.webViewMSI.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewMSI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewMSI.Location = new System.Drawing.Point(0, 0);
            this.webViewMSI.Name = "webViewMSI";
            this.webViewMSI.Size = new System.Drawing.Size(1223, 1112);
            this.webViewMSI.TabIndex = 0;
            this.webViewMSI.ZoomFactor = 1D;
            // 
            // LbAirdInfo
            // 
            this.LbAirdInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LbAirdInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbAirdInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LbAirdInfo.FormattingEnabled = true;
            this.LbAirdInfo.ItemHeight = 24;
            this.LbAirdInfo.Location = new System.Drawing.Point(0, 0);
            this.LbAirdInfo.Name = "LbAirdInfo";
            this.LbAirdInfo.Size = new System.Drawing.Size(1072, 648);
            this.LbAirdInfo.TabIndex = 1;
            // 
            // webViewMS
            // 
            this.webViewMS.AllowExternalDrop = true;
            this.webViewMS.CreationProperties = null;
            this.webViewMS.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewMS.Location = new System.Drawing.Point(0, 0);
            this.webViewMS.Name = "webViewMS";
            this.webViewMS.Size = new System.Drawing.Size(1072, 464);
            this.webViewMS.TabIndex = 0;
            this.webViewMS.ZoomFactor = 1D;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.LbAirdInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1072, 1112);
            this.panel1.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.webViewMS);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 648);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1072, 464);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.webViewMSI);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(1072, 66);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1223, 1112);
            this.panel5.TabIndex = 6;
            // 
            // MSIImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2295, 1178);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MSIImageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MSI Image Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewMSI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.webViewMS)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbMz;
        private System.Windows.Forms.Button BtnShowImage;
        private System.Windows.Forms.TextBox TbAirdFile;
        private System.Windows.Forms.Button BtnAirdImport;
        public Microsoft.Web.WebView2.WinForms.WebView2 webViewMSI;
        private System.Windows.Forms.Button BtnShowMS;
        private System.Windows.Forms.TextBox TbScanNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox LbAirdInfo;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewMS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
    }
}