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
            this.lblCompanyInfo = new System.Windows.Forms.Label();
            this.lblOPName = new System.Windows.Forms.Label();
            this.lblCPUName = new System.Windows.Forms.Label();
            this.lblPhysicMemoryName = new System.Windows.Forms.Label();
            this.lbl1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDesc = new System.Windows.Forms.Label();
            this.tbPaperZDPD = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSoftwareVersion
            // 
            this.lblSoftwareVersion.AutoSize = true;
            this.lblSoftwareVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoftwareVersion.ForeColor = System.Drawing.Color.DimGray;
            this.lblSoftwareVersion.Location = new System.Drawing.Point(698, 45);
            this.lblSoftwareVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoftwareVersion.Name = "lblSoftwareVersion";
            this.lblSoftwareVersion.Size = new System.Drawing.Size(175, 29);
            this.lblSoftwareVersion.TabIndex = 3;
            this.lblSoftwareVersion.Text = "SoftwareVersion";
            // 
            // lblCPUInfo
            // 
            this.lblCPUInfo.AutoSize = true;
            this.lblCPUInfo.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCPUInfo.Location = new System.Drawing.Point(248, 295);
            this.lblCPUInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCPUInfo.Name = "lblCPUInfo";
            this.lblCPUInfo.Size = new System.Drawing.Size(96, 27);
            this.lblCPUInfo.TabIndex = 4;
            this.lblCPUInfo.Text = "CPU Info";
            // 
            // lblPhysicMemory
            // 
            this.lblPhysicMemory.AutoSize = true;
            this.lblPhysicMemory.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPhysicMemory.Location = new System.Drawing.Point(248, 330);
            this.lblPhysicMemory.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhysicMemory.Name = "lblPhysicMemory";
            this.lblPhysicMemory.Size = new System.Drawing.Size(175, 27);
            this.lblPhysicMemory.TabIndex = 5;
            this.lblPhysicMemory.Text = "Physical Memory";
            // 
            // lblOpVersion
            // 
            this.lblOpVersion.AutoSize = true;
            this.lblOpVersion.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblOpVersion.Location = new System.Drawing.Point(248, 258);
            this.lblOpVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOpVersion.Name = "lblOpVersion";
            this.lblOpVersion.Size = new System.Drawing.Size(118, 27);
            this.lblOpVersion.TabIndex = 6;
            this.lblOpVersion.Text = "OP Version";
            // 
            // llLicense
            // 
            this.llLicense.AutoSize = true;
            this.llLicense.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llLicense.Location = new System.Drawing.Point(10, 446);
            this.llLicense.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llLicense.Name = "llLicense";
            this.llLicense.Size = new System.Drawing.Size(144, 27);
            this.llLicense.TabIndex = 7;
            this.llLicense.TabStop = true;
            this.llLicense.Text = "License Status";
            this.llLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseCheck_Click);
            // 
            // llAirdProGitee
            // 
            this.llAirdProGitee.AutoSize = true;
            this.llAirdProGitee.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProGitee.Location = new System.Drawing.Point(10, 528);
            this.llAirdProGitee.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llAirdProGitee.Name = "llAirdProGitee";
            this.llAirdProGitee.Size = new System.Drawing.Size(92, 27);
            this.llAirdProGitee.TabIndex = 8;
            this.llAirdProGitee.TabStop = true;
            this.llAirdProGitee.Text = "on Gitee";
            this.llAirdProGitee.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProonGitee_Click);
            // 
            // llAirdProonGithub
            // 
            this.llAirdProonGithub.AutoSize = true;
            this.llAirdProonGithub.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProonGithub.Location = new System.Drawing.Point(124, 528);
            this.llAirdProonGithub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llAirdProonGithub.Name = "llAirdProonGithub";
            this.llAirdProonGithub.Size = new System.Drawing.Size(107, 27);
            this.llAirdProonGithub.TabIndex = 9;
            this.llAirdProonGithub.TabStop = true;
            this.llAirdProonGithub.Text = "on Github";
            this.llAirdProonGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CheckAirdProonGithub_Click);
            // 
            // llAirdProSdkGitee
            // 
            this.llAirdProSdkGitee.AutoSize = true;
            this.llAirdProSdkGitee.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProSdkGitee.Location = new System.Drawing.Point(261, 528);
            this.llAirdProSdkGitee.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llAirdProSdkGitee.Name = "llAirdProSdkGitee";
            this.llAirdProSdkGitee.Size = new System.Drawing.Size(92, 27);
            this.llAirdProSdkGitee.TabIndex = 10;
            this.llAirdProSdkGitee.TabStop = true;
            this.llAirdProSdkGitee.Text = "on Gitee";
            this.llAirdProSdkGitee.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProSDKGitee_Click);
            // 
            // llAirdProSDKGithub
            // 
            this.llAirdProSDKGithub.AutoSize = true;
            this.llAirdProSDKGithub.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProSDKGithub.Location = new System.Drawing.Point(375, 528);
            this.llAirdProSDKGithub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.llAirdProSDKGithub.Name = "llAirdProSDKGithub";
            this.llAirdProSDKGithub.Size = new System.Drawing.Size(107, 27);
            this.llAirdProSDKGithub.TabIndex = 11;
            this.llAirdProSDKGithub.TabStop = true;
            this.llAirdProSDKGithub.Text = "on Github";
            this.llAirdProSDKGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProSDKGithub_Click);
            // 
            // lblCompanyInfo
            // 
            this.lblCompanyInfo.AutoSize = true;
            this.lblCompanyInfo.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCompanyInfo.Location = new System.Drawing.Point(12, 836);
            this.lblCompanyInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompanyInfo.Name = "lblCompanyInfo";
            this.lblCompanyInfo.Size = new System.Drawing.Size(142, 27);
            this.lblCompanyInfo.TabIndex = 13;
            this.lblCompanyInfo.Text = "CompanyInfo";
            // 
            // lblOPName
            // 
            this.lblOPName.AutoSize = true;
            this.lblOPName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOPName.Location = new System.Drawing.Point(10, 258);
            this.lblOPName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOPName.Name = "lblOPName";
            this.lblOPName.Size = new System.Drawing.Size(218, 27);
            this.lblOPName.TabIndex = 14;
            this.lblOPName.Text = "Operation System :    ";
            // 
            // lblCPUName
            // 
            this.lblCPUName.AutoSize = true;
            this.lblCPUName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCPUName.Location = new System.Drawing.Point(10, 295);
            this.lblCPUName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCPUName.Name = "lblCPUName";
            this.lblCPUName.Size = new System.Drawing.Size(87, 27);
            this.lblCPUName.TabIndex = 15;
            this.lblCPUName.Text = "CPU :    ";
            // 
            // lblPhysicMemoryName
            // 
            this.lblPhysicMemoryName.AutoSize = true;
            this.lblPhysicMemoryName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPhysicMemoryName.Location = new System.Drawing.Point(10, 330);
            this.lblPhysicMemoryName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPhysicMemoryName.Name = "lblPhysicMemoryName";
            this.lblPhysicMemoryName.Size = new System.Drawing.Size(210, 27);
            this.lblPhysicMemoryName.TabIndex = 16;
            this.lblPhysicMemoryName.Text = "Physical Memory :    ";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lbl1.Location = new System.Drawing.Point(10, 498);
            this.lbl1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(178, 27);
            this.lbl1.TabIndex = 18;
            this.lbl1.Text = "Find AirdPro (C#)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.Location = new System.Drawing.Point(261, 495);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 27);
            this.label1.TabIndex = 19;
            this.label1.Text = "Find Aird-SDK (JAVA, C#, Python)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.Location = new System.Drawing.Point(14, 586);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 27);
            this.label2.TabIndex = 20;
            this.label2.Text = "Cite AirdPro Paper";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.ForeColor = System.Drawing.Color.Black;
            this.lblDesc.Location = new System.Drawing.Point(9, 86);
            this.lblDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(147, 29);
            this.lblDesc.TabIndex = 21;
            this.lblDesc.Text = "SoftwareDesc";
            // 
            // tbPaperZDPD
            // 
            this.tbPaperZDPD.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.tbPaperZDPD.Location = new System.Drawing.Point(15, 621);
            this.tbPaperZDPD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPaperZDPD.Multiline = true;
            this.tbPaperZDPD.Name = "tbPaperZDPD";
            this.tbPaperZDPD.ReadOnly = true;
            this.tbPaperZDPD.Size = new System.Drawing.Size(1123, 208);
            this.tbPaperZDPD.TabIndex = 23;
            this.tbPaperZDPD.Text = "Paper:ZDPD";
            this.tbPaperZDPD.TextChanged += new System.EventHandler(this.tbPaperZDPD_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AirdPro.Properties.Resources.AirdProLogo;
            this.pictureBox1.Location = new System.Drawing.Point(433, 13);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(257, 61);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1158, 878);
            this.Controls.Add(this.tbPaperZDPD);
            this.Controls.Add(this.lblDesc);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.lblPhysicMemoryName);
            this.Controls.Add(this.lblCPUName);
            this.Controls.Add(this.lblOPName);
            this.Controls.Add(this.lblCompanyInfo);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.Text = "About";
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
        private System.Windows.Forms.Label lblCompanyInfo;
        private System.Windows.Forms.Label lblOPName;
        private System.Windows.Forms.Label lblCPUName;
        private System.Windows.Forms.Label lblPhysicMemoryName;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.TextBox tbPaperZDPD;
    }
}