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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TbDAL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TbPPM = new System.Windows.Forms.TextBox();
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
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.TbDAL);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.TbPPM);
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
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1530, 44);
            this.panel2.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(1251, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "or";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(1359, 16);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Dal";
            // 
            // TbDAL
            // 
            this.TbDAL.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbDAL.Location = new System.Drawing.Point(1276, 13);
            this.TbDAL.Margin = new System.Windows.Forms.Padding(2);
            this.TbDAL.Name = "TbDAL";
            this.TbDAL.Size = new System.Drawing.Size(79, 23);
            this.TbDAL.TabIndex = 13;
            this.TbDAL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TbDAL.Leave += new System.EventHandler(this.TbDAL_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(1213, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "ppm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(1106, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "tolerance";
            // 
            // TbPPM
            // 
            this.TbPPM.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbPPM.Location = new System.Drawing.Point(1172, 13);
            this.TbPPM.Margin = new System.Windows.Forms.Padding(2);
            this.TbPPM.Name = "TbPPM";
            this.TbPPM.Size = new System.Drawing.Size(37, 23);
            this.TbPPM.TabIndex = 10;
            this.TbPPM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TbPPM.Leave += new System.EventHandler(this.TbPPM_Leave);
            // 
            // BtnShowMS
            // 
            this.BtnShowMS.Enabled = false;
            this.BtnShowMS.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnShowMS.Location = new System.Drawing.Point(851, 9);
            this.BtnShowMS.Margin = new System.Windows.Forms.Padding(2);
            this.BtnShowMS.Name = "BtnShowMS";
            this.BtnShowMS.Size = new System.Drawing.Size(117, 26);
            this.BtnShowMS.TabIndex = 9;
            this.BtnShowMS.Text = "Show Spectrum";
            this.BtnShowMS.UseVisualStyleBackColor = true;
            this.BtnShowMS.Click += new System.EventHandler(this.BtnShowMS_Click);
            // 
            // TbScanNumber
            // 
            this.TbScanNumber.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbScanNumber.Location = new System.Drawing.Point(765, 11);
            this.TbScanNumber.Margin = new System.Windows.Forms.Padding(2);
            this.TbScanNumber.Name = "TbScanNumber";
            this.TbScanNumber.Size = new System.Drawing.Size(84, 23);
            this.TbScanNumber.TabIndex = 8;
            this.TbScanNumber.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(719, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Scan #";
            // 
            // BtnShowImage
            // 
            this.BtnShowImage.Enabled = false;
            this.BtnShowImage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnShowImage.Location = new System.Drawing.Point(1390, 12);
            this.BtnShowImage.Margin = new System.Windows.Forms.Padding(2);
            this.BtnShowImage.Name = "BtnShowImage";
            this.BtnShowImage.Size = new System.Drawing.Size(118, 26);
            this.BtnShowImage.TabIndex = 6;
            this.BtnShowImage.Text = "Show MSI Image";
            this.BtnShowImage.UseVisualStyleBackColor = true;
            this.BtnShowImage.Click += new System.EventHandler(this.BtnShowImage_Click);
            // 
            // TbAirdFile
            // 
            this.TbAirdFile.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbAirdFile.Location = new System.Drawing.Point(8, 12);
            this.TbAirdFile.Margin = new System.Windows.Forms.Padding(2);
            this.TbAirdFile.Name = "TbAirdFile";
            this.TbAirdFile.Size = new System.Drawing.Size(629, 23);
            this.TbAirdFile.TabIndex = 4;
            // 
            // BtnAirdImport
            // 
            this.BtnAirdImport.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnAirdImport.Location = new System.Drawing.Point(640, 9);
            this.BtnAirdImport.Margin = new System.Windows.Forms.Padding(2);
            this.BtnAirdImport.Name = "BtnAirdImport";
            this.BtnAirdImport.Size = new System.Drawing.Size(67, 26);
            this.BtnAirdImport.TabIndex = 3;
            this.BtnAirdImport.Text = "*.aird";
            this.BtnAirdImport.UseVisualStyleBackColor = true;
            this.BtnAirdImport.Click += new System.EventHandler(this.BtnAirdImport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(984, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "m/z";
            // 
            // TbMz
            // 
            this.TbMz.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TbMz.Location = new System.Drawing.Point(1015, 12);
            this.TbMz.Margin = new System.Windows.Forms.Padding(2);
            this.TbMz.Name = "TbMz";
            this.TbMz.Size = new System.Drawing.Size(84, 23);
            this.TbMz.TabIndex = 1;
            // 
            // webViewMSI
            // 
            this.webViewMSI.AllowExternalDrop = true;
            this.webViewMSI.CreationProperties = null;
            this.webViewMSI.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewMSI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewMSI.Location = new System.Drawing.Point(0, 0);
            this.webViewMSI.Margin = new System.Windows.Forms.Padding(2);
            this.webViewMSI.Name = "webViewMSI";
            this.webViewMSI.Size = new System.Drawing.Size(815, 667);
            this.webViewMSI.TabIndex = 0;
            this.webViewMSI.ZoomFactor = 1D;
            // 
            // LbAirdInfo
            // 
            this.LbAirdInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LbAirdInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbAirdInfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LbAirdInfo.FormattingEnabled = true;
            this.LbAirdInfo.ItemHeight = 17;
            this.LbAirdInfo.Location = new System.Drawing.Point(0, 0);
            this.LbAirdInfo.Margin = new System.Windows.Forms.Padding(2);
            this.LbAirdInfo.Name = "LbAirdInfo";
            this.LbAirdInfo.Size = new System.Drawing.Size(715, 459);
            this.LbAirdInfo.TabIndex = 1;
            // 
            // webViewMS
            // 
            this.webViewMS.AllowExternalDrop = true;
            this.webViewMS.CreationProperties = null;
            this.webViewMS.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webViewMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewMS.Location = new System.Drawing.Point(0, 0);
            this.webViewMS.Margin = new System.Windows.Forms.Padding(2);
            this.webViewMS.Name = "webViewMS";
            this.webViewMS.Size = new System.Drawing.Size(715, 208);
            this.webViewMS.TabIndex = 0;
            this.webViewMS.ZoomFactor = 1D;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.LbAirdInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 44);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 667);
            this.panel1.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.webViewMS);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 459);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(715, 208);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.webViewMSI);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(715, 44);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(815, 667);
            this.panel5.TabIndex = 6;
            // 
            // MSIImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1530, 711);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TbPPM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TbDAL;
    }
}