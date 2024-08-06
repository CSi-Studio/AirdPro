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
            this.btnImportData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAllBrukerFiles = new System.Windows.Forms.Button();
            this.btnIMDataView = new System.Windows.Forms.Button();
            this.btnDataView = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnConvertToAird = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbFileNames = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnImportData);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnAllBrukerFiles);
            this.panel1.Controls.Add(this.btnIMDataView);
            this.panel1.Controls.Add(this.btnDataView);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnConvertToAird);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1448, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 727);
            this.panel1.TabIndex = 12;
            // 
            // btnImportData
            // 
            this.btnImportData.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportData.Location = new System.Drawing.Point(18, 129);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(256, 40);
            this.btnImportData.TabIndex = 20;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
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
            // btnAllBrukerFiles
            // 
            this.btnAllBrukerFiles.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllBrukerFiles.Location = new System.Drawing.Point(18, 37);
            this.btnAllBrukerFiles.Name = "btnAllBrukerFiles";
            this.btnAllBrukerFiles.Size = new System.Drawing.Size(256, 40);
            this.btnAllBrukerFiles.TabIndex = 18;
            this.btnAllBrukerFiles.Text = "All *.d";
            this.btnAllBrukerFiles.UseVisualStyleBackColor = true;
            this.btnAllBrukerFiles.Click += new System.EventHandler(this.btnAllBrukerFiles_Click);
            // 
            // btnIMDataView
            // 
            this.btnIMDataView.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIMDataView.Location = new System.Drawing.Point(18, 221);
            this.btnIMDataView.Name = "btnIMDataView";
            this.btnIMDataView.Size = new System.Drawing.Size(256, 40);
            this.btnIMDataView.TabIndex = 16;
            this.btnIMDataView.Text = "Ion Mobility Data Overview";
            this.btnIMDataView.UseVisualStyleBackColor = true;
            this.btnIMDataView.Click += new System.EventHandler(this.btnIMDataView_Click);
            // 
            // btnDataView
            // 
            this.btnDataView.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDataView.Location = new System.Drawing.Point(18, 175);
            this.btnDataView.Name = "btnDataView";
            this.btnDataView.Size = new System.Drawing.Size(256, 40);
            this.btnDataView.TabIndex = 15;
            this.btnDataView.Text = "Data Overview";
            this.btnDataView.UseVisualStyleBackColor = true;
            this.btnDataView.Click += new System.EventHandler(this.btnDataView_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(18, 83);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(256, 40);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnConvertToAird
            // 
            this.btnConvertToAird.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvertToAird.Location = new System.Drawing.Point(18, 267);
            this.btnConvertToAird.Name = "btnConvertToAird";
            this.btnConvertToAird.Size = new System.Drawing.Size(256, 40);
            this.btnConvertToAird.TabIndex = 13;
            this.btnConvertToAird.Text = "Convert to Aird";
            this.btnConvertToAird.UseVisualStyleBackColor = true;
            this.btnConvertToAird.Click += new System.EventHandler(this.btnConvertToAird_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1448, 37);
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
            // lbFileNames
            // 
            this.lbFileNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFileNames.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFileNames.FormattingEnabled = true;
            this.lbFileNames.ItemHeight = 20;
            this.lbFileNames.Location = new System.Drawing.Point(0, 0);
            this.lbFileNames.Name = "lbFileNames";
            this.lbFileNames.Size = new System.Drawing.Size(1448, 690);
            this.lbFileNames.TabIndex = 6;
            this.lbFileNames.SelectedIndexChanged += new System.EventHandler(this.lbFileNames_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbFileNames);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1448, 690);
            this.panel3.TabIndex = 14;
            // 
            // ImportMSDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1742, 727);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ImportMSDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bruker Ion Mobility MS Research";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAllBrukerFiles;
        private System.Windows.Forms.Button btnIMDataView;
        private System.Windows.Forms.Button btnDataView;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnConvertToAird;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lbFileNames;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnImportData;
    }
}