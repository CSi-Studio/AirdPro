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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
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
            this.pbCheckUpdate = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSoftwareVersion
            // 
            this.lblSoftwareVersion.AutoSize = true;
            this.lblSoftwareVersion.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSoftwareVersion.ForeColor = System.Drawing.Color.DimGray;
            this.lblSoftwareVersion.Location = new System.Drawing.Point(465, 30);
            this.lblSoftwareVersion.Name = "lblSoftwareVersion";
            this.lblSoftwareVersion.Size = new System.Drawing.Size(114, 19);
            this.lblSoftwareVersion.TabIndex = 3;
            this.lblSoftwareVersion.Text = "SoftwareVersion";
            // 
            // lblCPUInfo
            // 
            this.lblCPUInfo.AutoSize = true;
            this.lblCPUInfo.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCPUInfo.Location = new System.Drawing.Point(165, 197);
            this.lblCPUInfo.Name = "lblCPUInfo";
            this.lblCPUInfo.Size = new System.Drawing.Size(68, 20);
            this.lblCPUInfo.TabIndex = 4;
            this.lblCPUInfo.Text = "CPU Info";
            // 
            // lblPhysicMemory
            // 
            this.lblPhysicMemory.AutoSize = true;
            this.lblPhysicMemory.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPhysicMemory.Location = new System.Drawing.Point(165, 220);
            this.lblPhysicMemory.Name = "lblPhysicMemory";
            this.lblPhysicMemory.Size = new System.Drawing.Size(123, 20);
            this.lblPhysicMemory.TabIndex = 5;
            this.lblPhysicMemory.Text = "Physical Memory";
            // 
            // lblOpVersion
            // 
            this.lblOpVersion.AutoSize = true;
            this.lblOpVersion.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblOpVersion.Location = new System.Drawing.Point(165, 172);
            this.lblOpVersion.Name = "lblOpVersion";
            this.lblOpVersion.Size = new System.Drawing.Size(83, 20);
            this.lblOpVersion.TabIndex = 6;
            this.lblOpVersion.Text = "OP Version";
            // 
            // llLicense
            // 
            this.llLicense.AutoSize = true;
            this.llLicense.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llLicense.Location = new System.Drawing.Point(7, 297);
            this.llLicense.Name = "llLicense";
            this.llLicense.Size = new System.Drawing.Size(103, 20);
            this.llLicense.TabIndex = 7;
            this.llLicense.TabStop = true;
            this.llLicense.Text = "License Status";
            this.llLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseCheck_Click);
            // 
            // llAirdProGitee
            // 
            this.llAirdProGitee.AutoSize = true;
            this.llAirdProGitee.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProGitee.Location = new System.Drawing.Point(7, 352);
            this.llAirdProGitee.Name = "llAirdProGitee";
            this.llAirdProGitee.Size = new System.Drawing.Size(66, 20);
            this.llAirdProGitee.TabIndex = 8;
            this.llAirdProGitee.TabStop = true;
            this.llAirdProGitee.Text = "on Gitee";
            this.llAirdProGitee.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProonGitee_Click);
            // 
            // llAirdProonGithub
            // 
            this.llAirdProonGithub.AutoSize = true;
            this.llAirdProonGithub.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProonGithub.Location = new System.Drawing.Point(83, 352);
            this.llAirdProonGithub.Name = "llAirdProonGithub";
            this.llAirdProonGithub.Size = new System.Drawing.Size(77, 20);
            this.llAirdProonGithub.TabIndex = 9;
            this.llAirdProonGithub.TabStop = true;
            this.llAirdProonGithub.Text = "on Github";
            this.llAirdProonGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CheckAirdProonGithub_Click);
            // 
            // llAirdProSdkGitee
            // 
            this.llAirdProSdkGitee.AutoSize = true;
            this.llAirdProSdkGitee.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProSdkGitee.Location = new System.Drawing.Point(174, 352);
            this.llAirdProSdkGitee.Name = "llAirdProSdkGitee";
            this.llAirdProSdkGitee.Size = new System.Drawing.Size(66, 20);
            this.llAirdProSdkGitee.TabIndex = 10;
            this.llAirdProSdkGitee.TabStop = true;
            this.llAirdProSdkGitee.Text = "on Gitee";
            this.llAirdProSdkGitee.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProSDKGitee_Click);
            // 
            // llAirdProSDKGithub
            // 
            this.llAirdProSDKGithub.AutoSize = true;
            this.llAirdProSDKGithub.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.llAirdProSDKGithub.Location = new System.Drawing.Point(250, 352);
            this.llAirdProSDKGithub.Name = "llAirdProSDKGithub";
            this.llAirdProSDKGithub.Size = new System.Drawing.Size(77, 20);
            this.llAirdProSDKGithub.TabIndex = 11;
            this.llAirdProSDKGithub.TabStop = true;
            this.llAirdProSDKGithub.Text = "on Github";
            this.llAirdProSDKGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AirdProSDKGithub_Click);
            // 
            // lblCompanyInfo
            // 
            this.lblCompanyInfo.AutoSize = true;
            this.lblCompanyInfo.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCompanyInfo.Location = new System.Drawing.Point(8, 557);
            this.lblCompanyInfo.Name = "lblCompanyInfo";
            this.lblCompanyInfo.Size = new System.Drawing.Size(100, 20);
            this.lblCompanyInfo.TabIndex = 13;
            this.lblCompanyInfo.Text = "CompanyInfo";
            // 
            // lblOPName
            // 
            this.lblOPName.AutoSize = true;
            this.lblOPName.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOPName.Location = new System.Drawing.Point(7, 172);
            this.lblOPName.Name = "lblOPName";
            this.lblOPName.Size = new System.Drawing.Size(151, 20);
            this.lblOPName.TabIndex = 14;
            this.lblOPName.Text = "Operation System :    ";
            // 
            // lblCPUName
            // 
            this.lblCPUName.AutoSize = true;
            this.lblCPUName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblCPUName.Location = new System.Drawing.Point(7, 197);
            this.lblCPUName.Name = "lblCPUName";
            this.lblCPUName.Size = new System.Drawing.Size(60, 20);
            this.lblCPUName.TabIndex = 15;
            this.lblCPUName.Text = "CPU :    ";
            // 
            // lblPhysicMemoryName
            // 
            this.lblPhysicMemoryName.AutoSize = true;
            this.lblPhysicMemoryName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblPhysicMemoryName.Location = new System.Drawing.Point(7, 220);
            this.lblPhysicMemoryName.Name = "lblPhysicMemoryName";
            this.lblPhysicMemoryName.Size = new System.Drawing.Size(146, 20);
            this.lblPhysicMemoryName.TabIndex = 16;
            this.lblPhysicMemoryName.Text = "Physical Memory :    ";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lbl1.Location = new System.Drawing.Point(7, 332);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(125, 20);
            this.lbl1.TabIndex = 18;
            this.lbl1.Text = "Find AirdPro (C#)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.Location = new System.Drawing.Point(174, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "Find Aird-SDK (JAVA, C#, Python)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.Location = new System.Drawing.Point(9, 391);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "Cite AirdPro Paper";
            // 
            // lblDesc
            // 
            this.lblDesc.AutoSize = true;
            this.lblDesc.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.ForeColor = System.Drawing.Color.Black;
            this.lblDesc.Location = new System.Drawing.Point(6, 57);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(98, 19);
            this.lblDesc.TabIndex = 21;
            this.lblDesc.Text = "SoftwareDesc";
            // 
            // tbPaperZDPD
            // 
            this.tbPaperZDPD.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.tbPaperZDPD.Location = new System.Drawing.Point(10, 414);
            this.tbPaperZDPD.Multiline = true;
            this.tbPaperZDPD.Name = "tbPaperZDPD";
            this.tbPaperZDPD.ReadOnly = true;
            this.tbPaperZDPD.Size = new System.Drawing.Size(750, 140);
            this.tbPaperZDPD.TabIndex = 23;
            this.tbPaperZDPD.Text = "Paper:ZDPD";
            this.tbPaperZDPD.TextChanged += new System.EventHandler(this.tbPaperZDPD_TextChanged);
            // 
            // pbCheckUpdate
            // 
            this.pbCheckUpdate.Image = global::AirdPro.Properties.Resources.AirdProLogo;
            this.pbCheckUpdate.Location = new System.Drawing.Point(289, 9);
            this.pbCheckUpdate.Name = "pbCheckUpdate";
            this.pbCheckUpdate.Size = new System.Drawing.Size(171, 41);
            this.pbCheckUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCheckUpdate.TabIndex = 0;
            this.pbCheckUpdate.TabStop = false;
            this.pbCheckUpdate.Click += new System.EventHandler(this.pbCheckUpdate_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(772, 585);
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
            this.Controls.Add(this.pbCheckUpdate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.Text = "About";
            this.Load += new System.EventHandler(this.HelpAboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCheckUpdate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCheckUpdate;
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