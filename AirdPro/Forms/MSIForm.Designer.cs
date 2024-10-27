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
            this.BtnShowImage = new System.Windows.Forms.Button();
            this.TbAirdFile = new System.Windows.Forms.TextBox();
            this.BtnAirdImport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TbMz = new System.Windows.Forms.TextBox();
            this.webViewMSI = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.LbImageParams = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TbScanNumber = new System.Windows.Forms.TextBox();
            this.BtnShowMS = new System.Windows.Forms.Button();
            this.LbRawData = new System.Windows.Forms.ListBox();
            this.webViewMS = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webViewMSI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.webViewMS)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
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
            this.panel2.Size = new System.Drawing.Size(1928, 66);
            this.panel2.TabIndex = 4;
            // 
            // BtnShowImage
            // 
            this.BtnShowImage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnShowImage.Location = new System.Drawing.Point(1651, 13);
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
            this.label1.Location = new System.Drawing.Point(1471, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "m/z";
            // 
            // TbMz
            // 
            this.TbMz.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbMz.Location = new System.Drawing.Point(1521, 20);
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
            this.webViewMSI.Size = new System.Drawing.Size(856, 916);
            this.webViewMSI.TabIndex = 0;
            this.webViewMSI.ZoomFactor = 1D;
            // 
            // LbImageParams
            // 
            this.LbImageParams.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LbImageParams.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbImageParams.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LbImageParams.FormattingEnabled = true;
            this.LbImageParams.ItemHeight = 24;
            this.LbImageParams.Location = new System.Drawing.Point(0, 0);
            this.LbImageParams.Name = "LbImageParams";
            this.LbImageParams.Size = new System.Drawing.Size(1072, 240);
            this.LbImageParams.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(1076, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Scan #";
            // 
            // TbScanNumber
            // 
            this.TbScanNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbScanNumber.Location = new System.Drawing.Point(1144, 21);
            this.TbScanNumber.Name = "TbScanNumber";
            this.TbScanNumber.Size = new System.Drawing.Size(124, 30);
            this.TbScanNumber.TabIndex = 8;
            // 
            // BtnShowMS
            // 
            this.BtnShowMS.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnShowMS.Location = new System.Drawing.Point(1274, 13);
            this.BtnShowMS.Name = "BtnShowMS";
            this.BtnShowMS.Size = new System.Drawing.Size(176, 39);
            this.BtnShowMS.TabIndex = 9;
            this.BtnShowMS.Text = "Show Spectrum";
            this.BtnShowMS.UseVisualStyleBackColor = true;
            this.BtnShowMS.Click += new System.EventHandler(this.BtnShowMS_Click);
            // 
            // LbRawData
            // 
            this.LbRawData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LbRawData.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbRawData.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LbRawData.FormattingEnabled = true;
            this.LbRawData.ItemHeight = 24;
            this.LbRawData.Location = new System.Drawing.Point(0, 0);
            this.LbRawData.Name = "LbRawData";
            this.LbRawData.Size = new System.Drawing.Size(1072, 144);
            this.LbRawData.TabIndex = 1;
            // 
            // webViewMS
            // 
            this.webViewMS.AllowExternalDrop = true;
            this.webViewMS.CreationProperties = null;
            this.webViewMS.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewMS.Location = new System.Drawing.Point(0, 0);
            this.webViewMS.Name = "webViewMS";
            this.webViewMS.Size = new System.Drawing.Size(1072, 532);
            this.webViewMS.TabIndex = 0;
            this.webViewMS.ZoomFactor = 1D;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.LbRawData);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1072, 916);
            this.panel1.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.LbImageParams);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 144);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1072, 772);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.webViewMS);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 240);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1072, 532);
            this.panel4.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.webViewMSI);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(1072, 66);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(856, 916);
            this.panel5.TabIndex = 6;
            // 
            // MSIImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1928, 982);
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
            this.panel4.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox LbImageParams;
        public Microsoft.Web.WebView2.WinForms.WebView2 webViewMSI;
        private System.Windows.Forms.Button BtnShowMS;
        private System.Windows.Forms.TextBox TbScanNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox LbRawData;
        private Microsoft.Web.WebView2.WinForms.WebView2 webViewMS;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}