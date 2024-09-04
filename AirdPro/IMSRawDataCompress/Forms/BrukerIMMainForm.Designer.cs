namespace AirdPro.Forms
{
    partial class BrukerIMMainForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnTDFImport = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnSelectIMS = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.BtnConvertToAird = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.LbFileNames = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.LbTdfImport = new System.Windows.Forms.ListBox();
            this.LbPwizImport = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnTDFImport);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.BtnSelectIMS);
            this.panel1.Controls.Add(this.BtnClear);
            this.panel1.Controls.Add(this.BtnConvertToAird);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1816, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 1177);
            this.panel1.TabIndex = 12;
            // 
            // BtnTDFImport
            // 
            this.BtnTDFImport.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTDFImport.Location = new System.Drawing.Point(18, 129);
            this.BtnTDFImport.Name = "BtnTDFImport";
            this.BtnTDFImport.Size = new System.Drawing.Size(256, 40);
            this.BtnTDFImport.TabIndex = 20;
            this.BtnTDFImport.Text = "TDF Import";
            this.BtnTDFImport.UseVisualStyleBackColor = true;
            this.BtnTDFImport.Click += new System.EventHandler(this.BtnTDFImport_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "Select From Folder";
            // 
            // BtnSelectIMS
            // 
            this.BtnSelectIMS.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSelectIMS.Location = new System.Drawing.Point(18, 37);
            this.BtnSelectIMS.Name = "BtnSelectIMS";
            this.BtnSelectIMS.Size = new System.Drawing.Size(256, 40);
            this.BtnSelectIMS.TabIndex = 18;
            this.BtnSelectIMS.Text = "All *.d";
            this.BtnSelectIMS.UseVisualStyleBackColor = true;
            this.BtnSelectIMS.Click += new System.EventHandler(this.BtnSelectIMS_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear.Location = new System.Drawing.Point(18, 83);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(256, 40);
            this.BtnClear.TabIndex = 14;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnConvertToAird
            // 
            this.BtnConvertToAird.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConvertToAird.Location = new System.Drawing.Point(18, 175);
            this.BtnConvertToAird.Name = "BtnConvertToAird";
            this.BtnConvertToAird.Size = new System.Drawing.Size(256, 40);
            this.BtnConvertToAird.TabIndex = 13;
            this.BtnConvertToAird.Text = "Convert to Aird";
            this.BtnConvertToAird.UseVisualStyleBackColor = true;
            this.BtnConvertToAird.Click += new System.EventHandler(this.BtnConvertToAird_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1816, 37);
            this.panel2.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Bruker Ion Mobility MS Files ";
            // 
            // LbFileNames
            // 
            this.LbFileNames.Dock = System.Windows.Forms.DockStyle.Top;
            this.LbFileNames.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbFileNames.FormattingEnabled = true;
            this.LbFileNames.ItemHeight = 20;
            this.LbFileNames.Location = new System.Drawing.Point(0, 0);
            this.LbFileNames.Name = "LbFileNames";
            this.LbFileNames.Size = new System.Drawing.Size(1816, 264);
            this.LbFileNames.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Controls.Add(this.LbFileNames);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1816, 1140);
            this.panel3.TabIndex = 14;
            // 
            // LbTdfImport
            // 
            this.LbTdfImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbTdfImport.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbTdfImport.FormattingEnabled = true;
            this.LbTdfImport.HorizontalScrollbar = true;
            this.LbTdfImport.ItemHeight = 20;
            this.LbTdfImport.Location = new System.Drawing.Point(0, 0);
            this.LbTdfImport.Name = "LbTdfImport";
            this.LbTdfImport.Size = new System.Drawing.Size(1207, 876);
            this.LbTdfImport.TabIndex = 8;
            // 
            // LbPwizImport
            // 
            this.LbPwizImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbPwizImport.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbPwizImport.FormattingEnabled = true;
            this.LbPwizImport.HorizontalScrollbar = true;
            this.LbPwizImport.ItemHeight = 20;
            this.LbPwizImport.Location = new System.Drawing.Point(0, 0);
            this.LbPwizImport.Name = "LbPwizImport";
            this.LbPwizImport.Size = new System.Drawing.Size(605, 876);
            this.LbPwizImport.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 264);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LbPwizImport);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LbTdfImport);
            this.splitContainer1.Size = new System.Drawing.Size(1816, 876);
            this.splitContainer1.SplitterDistance = 605;
            this.splitContainer1.TabIndex = 9;
            // 
            // BrukerIMMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(2110, 1177);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "BrukerIMMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bruker Ion Mobility MS Research";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnSelectIMS;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnConvertToAird;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox LbFileNames;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnTDFImport;
        private System.Windows.Forms.ListBox LbTdfImport;
        public System.Windows.Forms.ListBox LbPwizImport;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}