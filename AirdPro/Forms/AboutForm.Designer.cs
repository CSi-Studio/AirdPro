namespace AirdPro.Forms
{
    partial class AboutForm
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
            this.lblSoftwareVersion = new System.Windows.Forms.Label();
            this.lblCPUInfo = new System.Windows.Forms.Label();
            this.lblPhysicMemory = new System.Windows.Forms.Label();
            this.lblOpVersion = new System.Windows.Forms.Label();
            this.llLicense = new System.Windows.Forms.LinkLabel();
            this.llAirdProGitee = new System.Windows.Forms.LinkLabel();
            this.llAirdProonGithub = new System.Windows.Forms.LinkLabel();
            this.llAirdProSdkGitee = new System.Windows.Forms.LinkLabel();
            this.llAirdProSDKGithub = new System.Windows.Forms.LinkLabel();
            this.llAirdProPaper = new System.Windows.Forms.LinkLabel();
            this.lblCompanyInfo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblOPName = new System.Windows.Forms.Label();
            this.lblCPUName = new System.Windows.Forms.Label();
            this.lblPhysicMemoryName = new System.Windows.Forms.Label();
            this.lblComupterInfoName = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSoftwareVersion
            // 
            this.lblSoftwareVersion.AutoSize = true;
            this.lblSoftwareVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoftwareVersion.ForeColor = System.Drawing.Color.DimGray;
            this.lblSoftwareVersion.Location = new System.Drawing.Point(160, 12);
            this.lblSoftwareVersion.Name = "lblSoftwareVersion";
            this.lblSoftwareVersion.Size = new System.Drawing.Size(114, 19);
            this.lblSoftwareVersion.TabIndex = 3;
            this.lblSoftwareVersion.Text = "SoftwareVersion";
            // 
            // lblCPUInfo
            // 
            this.lblCPUInfo.AutoSize = true;
            this.lblCPUInfo.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblCPUInfo.Location = new System.Drawing.Point(148, 277);
            this.lblCPUInfo.Name = "lblCPUInfo";
            this.lblCPUInfo.Size = new System.Drawing.Size(50, 17);
            this.lblCPUInfo.TabIndex = 4;
            this.lblCPUInfo.Text = "cpuInfo";
            // 
            // lblPhysicMemory
            // 
            this.lblPhysicMemory.AutoSize = true;
            this.lblPhysicMemory.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblPhysicMemory.Location = new System.Drawing.Point(148, 317);
            this.lblPhysicMemory.Name = "lblPhysicMemory";
            this.lblPhysicMemory.Size = new System.Drawing.Size(103, 17);
            this.lblPhysicMemory.TabIndex = 5;
            this.lblPhysicMemory.Text = "Physical Memory";
            // 
            // lblOpVersion
            // 
            this.lblOpVersion.AutoSize = true;
            this.lblOpVersion.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblOpVersion.Location = new System.Drawing.Point(148, 237);
            this.lblOpVersion.Name = "lblOpVersion";
            this.lblOpVersion.Size = new System.Drawing.Size(65, 17);
            this.lblOpVersion.TabIndex = 6;
            this.lblOpVersion.Text = "OPVersion";
            // 
            // llLicense
            // 
            this.llLicense.AutoSize = true;
            this.llLicense.Font = new System.Drawing.Font("Calibri", 10F);
            this.llLicense.Location = new System.Drawing.Point(403, 12);
            this.llLicense.Name = "llLicense";
            this.llLicense.Size = new System.Drawing.Size(87, 17);
            this.llLicense.TabIndex = 7;
            this.llLicense.TabStop = true;
            this.llLicense.Text = "License Status";
            this.llLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseCheck_Click);
            // 
            // llAirdProGitee
            // 
            this.llAirdProGitee.AutoSize = true;
            this.llAirdProGitee.Font = new System.Drawing.Font("Calibri", 10F);
            this.llAirdProGitee.Location = new System.Drawing.Point(403, 67);
            this.llAirdProGitee.Name = "llAirdProGitee";
            this.llAirdProGitee.Size = new System.Drawing.Size(56, 17);
            this.llAirdProGitee.TabIndex = 8;
            this.llAirdProGitee.TabStop = true;
            this.llAirdProGitee.Text = "on Gitee";
            this.llAirdProGitee.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProonGitee_Click);
            // 
            // llAirdProonGithub
            // 
            this.llAirdProonGithub.AutoSize = true;
            this.llAirdProonGithub.Font = new System.Drawing.Font("Calibri", 10F);
            this.llAirdProonGithub.Location = new System.Drawing.Point(479, 67);
            this.llAirdProonGithub.Name = "llAirdProonGithub";
            this.llAirdProonGithub.Size = new System.Drawing.Size(63, 17);
            this.llAirdProonGithub.TabIndex = 9;
            this.llAirdProonGithub.TabStop = true;
            this.llAirdProonGithub.Text = "on Github";
            this.llAirdProonGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CheckAirdProonGithub_Click);
            // 
            // llAirdProSdkGitee
            // 
            this.llAirdProSdkGitee.AutoSize = true;
            this.llAirdProSdkGitee.Font = new System.Drawing.Font("Calibri", 10F);
            this.llAirdProSdkGitee.Location = new System.Drawing.Point(403, 124);
            this.llAirdProSdkGitee.Name = "llAirdProSdkGitee";
            this.llAirdProSdkGitee.Size = new System.Drawing.Size(56, 17);
            this.llAirdProSdkGitee.TabIndex = 10;
            this.llAirdProSdkGitee.TabStop = true;
            this.llAirdProSdkGitee.Text = "on Gitee";
            this.llAirdProSdkGitee.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProSDKGitee_Click);
            // 
            // llAirdProSDKGithub
            // 
            this.llAirdProSDKGithub.AutoSize = true;
            this.llAirdProSDKGithub.Font = new System.Drawing.Font("Calibri", 10F);
            this.llAirdProSDKGithub.Location = new System.Drawing.Point(479, 124);
            this.llAirdProSDKGithub.Name = "llAirdProSDKGithub";
            this.llAirdProSDKGithub.Size = new System.Drawing.Size(63, 17);
            this.llAirdProSDKGithub.TabIndex = 11;
            this.llAirdProSDKGithub.TabStop = true;
            this.llAirdProSDKGithub.Text = "on Github";
            this.llAirdProSDKGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProSDKGithub_Click);
            // 
            // llAirdProPaper
            // 
            this.llAirdProPaper.AutoSize = true;
            this.llAirdProPaper.Font = new System.Drawing.Font("Calibri", 10F);
            this.llAirdProPaper.Location = new System.Drawing.Point(511, 159);
            this.llAirdProPaper.Name = "llAirdProPaper";
            this.llAirdProPaper.Size = new System.Drawing.Size(34, 17);
            this.llAirdProPaper.TabIndex = 12;
            this.llAirdProPaper.TabStop = true;
            this.llAirdProPaper.Text = "here";
            this.llAirdProPaper.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProPaperCitation_Click);
            // 
            // lblCompanyInfo
            // 
            this.lblCompanyInfo.AutoSize = true;
            this.lblCompanyInfo.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyInfo.Location = new System.Drawing.Point(30, 135);
            this.lblCompanyInfo.Name = "lblCompanyInfo";
            this.lblCompanyInfo.Size = new System.Drawing.Size(94, 19);
            this.lblCompanyInfo.TabIndex = 13;
            this.lblCompanyInfo.Text = "CompanyInfo";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AirdPro.Properties.Resources.AirdProLogoBlack;
            this.pictureBox1.Location = new System.Drawing.Point(21, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(120, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblOPName
            // 
            this.lblOPName.AutoSize = true;
            this.lblOPName.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblOPName.Location = new System.Drawing.Point(18, 237);
            this.lblOPName.Name = "lblOPName";
            this.lblOPName.Size = new System.Drawing.Size(127, 17);
            this.lblOPName.TabIndex = 14;
            this.lblOPName.Text = "Operation System :    ";
            // 
            // lblCPUName
            // 
            this.lblCPUName.AutoSize = true;
            this.lblCPUName.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblCPUName.Location = new System.Drawing.Point(18, 277);
            this.lblCPUName.Name = "lblCPUName";
            this.lblCPUName.Size = new System.Drawing.Size(50, 17);
            this.lblCPUName.TabIndex = 15;
            this.lblCPUName.Text = "CPU :    ";
            // 
            // lblPhysicMemoryName
            // 
            this.lblPhysicMemoryName.AutoSize = true;
            this.lblPhysicMemoryName.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblPhysicMemoryName.Location = new System.Drawing.Point(18, 317);
            this.lblPhysicMemoryName.Name = "lblPhysicMemoryName";
            this.lblPhysicMemoryName.Size = new System.Drawing.Size(122, 17);
            this.lblPhysicMemoryName.TabIndex = 16;
            this.lblPhysicMemoryName.Text = "Physical Memory :    ";
            // 
            // lblComupterInfoName
            // 
            this.lblComupterInfoName.AutoSize = true;
            this.lblComupterInfoName.BackColor = System.Drawing.Color.Transparent;
            this.lblComupterInfoName.Font = new System.Drawing.Font("Calibri", 12F);
            this.lblComupterInfoName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblComupterInfoName.Location = new System.Drawing.Point(17, 206);
            this.lblComupterInfoName.Name = "lblComupterInfoName";
            this.lblComupterInfoName.Size = new System.Drawing.Size(174, 19);
            this.lblComupterInfoName.TabIndex = 17;
            this.lblComupterInfoName.Text = "Device Specifications :     ";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("Calibri", 10F);
            this.lbl1.Location = new System.Drawing.Point(403, 47);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(101, 17);
            this.lbl1.TabIndex = 18;
            this.lbl1.Text = "Find AirdPro (C#)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F);
            this.label1.Location = new System.Drawing.Point(403, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Find AirdPro-SDK (JAVA)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10F);
            this.label2.Location = new System.Drawing.Point(403, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 17);
            this.label2.TabIndex = 20;
            this.label2.Text = "Cite AirdPro Paper";
            // 
            // HelpAboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(585, 371);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.lblComupterInfoName);
            this.Controls.Add(this.lblPhysicMemoryName);
            this.Controls.Add(this.lblCPUName);
            this.Controls.Add(this.lblOPName);
            this.Controls.Add(this.lblCompanyInfo);
            this.Controls.Add(this.llAirdProPaper);
            this.Controls.Add(this.llAirdProSDKGithub);
            this.Controls.Add(this.llAirdProSdkGitee);
            this.Controls.Add(this.llAirdProonGithub);
            this.Controls.Add(this.llAirdProGitee);
            this.Controls.Add(this.llLicense);
            this.Controls.Add(this.lblOpVersion);
            this.Controls.Add(this.lblPhysicMemory);
            this.Controls.Add(this.lblCPUInfo);
            this.Controls.Add(this.lblSoftwareVersion);
            this.Controls.Add(this.pictureBox1);
            this.Name = "HelpAboutForm";
            this.ShowIcon = false;
            this.Text = "About AirdPro";
            this.Load += new System.EventHandler(this.HelpAboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblSoftwareVersion;
        private System.Windows.Forms.Label lblCPUInfo;
        private System.Windows.Forms.Label lblPhysicMemory;
        private System.Windows.Forms.Label lblOpVersion;
        private System.Windows.Forms.LinkLabel llLicense;
        private System.Windows.Forms.LinkLabel llAirdProGitee;
        private System.Windows.Forms.LinkLabel llAirdProonGithub;
        private System.Windows.Forms.LinkLabel llAirdProSdkGitee;
        private System.Windows.Forms.LinkLabel llAirdProSDKGithub;
        private System.Windows.Forms.LinkLabel llAirdProPaper;
        private System.Windows.Forms.Label lblCompanyInfo;
        private System.Windows.Forms.Label lblOPName;
        private System.Windows.Forms.Label lblCPUName;
        private System.Windows.Forms.Label lblPhysicMemoryName;
        private System.Windows.Forms.Label lblComupterInfoName;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}