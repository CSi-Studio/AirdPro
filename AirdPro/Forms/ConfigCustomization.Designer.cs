namespace AirdPro.Forms
{
    partial class ConfigCustomization
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
            this.lblConfigOutputPath = new System.Windows.Forms.Label();
            this.btnConfigChooseFolder = new System.Windows.Forms.Button();
            this.tbConfigFolderPath = new System.Windows.Forms.TextBox();
            this.lblDp = new System.Windows.Forms.Label();
            this.lblMzPrecision = new System.Windows.Forms.Label();
            this.cbConfigMzPrecision = new System.Windows.Forms.ComboBox();
            this.cbConfigThreadAccelerate = new System.Windows.Forms.CheckBox();
            this.cbConfigIsZeroIntensityIgnore = new System.Windows.Forms.CheckBox();
            this.cbConfigStack = new System.Windows.Forms.CheckBox();
            this.lblConfigIntCompA = new System.Windows.Forms.Label();
            this.configIntByteComp = new System.Windows.Forms.ComboBox();
            this.lblConfigMzByteComp = new System.Windows.Forms.Label();
            this.configMzByteComp = new System.Windows.Forms.ComboBox();
            this.lblConfigStackLayers = new System.Windows.Forms.Label();
            this.cbConfigStackLayers = new System.Windows.Forms.ComboBox();
            this.lblConfigMzIntComp = new System.Windows.Forms.Label();
            this.configMzIntComp = new System.Windows.Forms.ComboBox();
            this.btnConfigDisconnect = new System.Windows.Forms.Button();
            this.lblConfigIP = new System.Windows.Forms.Label();
            this.tbConfigHostAndPort = new System.Windows.Forms.TextBox();
            this.btnConfigConnect = new System.Windows.Forms.Button();
            this.lblConfigOperator = new System.Windows.Forms.Label();
            this.tbConfigOperator = new System.Windows.Forms.TextBox();
            this.lblConfigFileNameTag = new System.Windows.Forms.Label();
            this.tbConfigFileNameSuffix = new System.Windows.Forms.TextBox();
            this.lblNameConfig = new System.Windows.Forms.Label();
            this.tbNameConfig = new System.Windows.Forms.TextBox();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblConfigOutputPath
            // 
            this.lblConfigOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigOutputPath.AutoSize = true;
            this.lblConfigOutputPath.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblConfigOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOutputPath.Location = new System.Drawing.Point(32, 20);
            this.lblConfigOutputPath.Name = "lblConfigOutputPath";
            this.lblConfigOutputPath.Size = new System.Drawing.Size(75, 16);
            this.lblConfigOutputPath.TabIndex = 8;
            this.lblConfigOutputPath.Text = "Output Path:";
            // 
            // btnConfigChooseFolder
            // 
            this.btnConfigChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigChooseFolder.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnConfigChooseFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(440, 37);
            this.btnConfigChooseFolder.Name = "btnConfigChooseFolder";
            this.btnConfigChooseFolder.Size = new System.Drawing.Size(75, 26);
            this.btnConfigChooseFolder.TabIndex = 9;
            this.btnConfigChooseFolder.Text = "Browser";
            this.btnConfigChooseFolder.UseVisualStyleBackColor = true;
            this.btnConfigChooseFolder.Click += new System.EventHandler(this.btnConfigChooseFolder_Click);
            // 
            // tbConfigFolderPath
            // 
            this.tbConfigFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConfigFolderPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFolderPath.Location = new System.Drawing.Point(33, 40);
            this.tbConfigFolderPath.Name = "tbConfigFolderPath";
            this.tbConfigFolderPath.Size = new System.Drawing.Size(401, 23);
            this.tbConfigFolderPath.TabIndex = 7;
            // 
            // lblDp
            // 
            this.lblDp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDp.AutoSize = true;
            this.lblDp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDp.Location = new System.Drawing.Point(519, 86);
            this.lblDp.Name = "lblDp";
            this.lblDp.Size = new System.Drawing.Size(17, 12);
            this.lblDp.TabIndex = 43;
            this.lblDp.Text = "dp";
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMzPrecision.Location = new System.Drawing.Point(424, 85);
            this.lblMzPrecision.Name = "lblMzPrecision";
            this.lblMzPrecision.Size = new System.Drawing.Size(23, 12);
            this.lblMzPrecision.TabIndex = 42;
            this.lblMzPrecision.Text = "m/z";
            // 
            // cbConfigMzPrecision
            // 
            this.cbConfigMzPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigMzPrecision.FormattingEnabled = true;
            this.cbConfigMzPrecision.Items.AddRange(new object[] {
            "3",
            "4",
            "5",
            "6"});
            this.cbConfigMzPrecision.Location = new System.Drawing.Point(460, 82);
            this.cbConfigMzPrecision.Name = "cbConfigMzPrecision";
            this.cbConfigMzPrecision.Size = new System.Drawing.Size(55, 20);
            this.cbConfigMzPrecision.TabIndex = 41;
            // 
            // cbConfigThreadAccelerate
            // 
            this.cbConfigThreadAccelerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfigThreadAccelerate.AutoSize = true;
            this.cbConfigThreadAccelerate.Checked = true;
            this.cbConfigThreadAccelerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigThreadAccelerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigThreadAccelerate.Location = new System.Drawing.Point(263, 86);
            this.cbConfigThreadAccelerate.Name = "cbConfigThreadAccelerate";
            this.cbConfigThreadAccelerate.Size = new System.Drawing.Size(108, 16);
            this.cbConfigThreadAccelerate.TabIndex = 40;
            this.cbConfigThreadAccelerate.Text = "Multithreading";
            this.cbConfigThreadAccelerate.UseVisualStyleBackColor = true;
            // 
            // cbConfigIsZeroIntensityIgnore
            // 
            this.cbConfigIsZeroIntensityIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfigIsZeroIntensityIgnore.AutoSize = true;
            this.cbConfigIsZeroIntensityIgnore.Checked = true;
            this.cbConfigIsZeroIntensityIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigIsZeroIntensityIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigIsZeroIntensityIgnore.Location = new System.Drawing.Point(34, 86);
            this.cbConfigIsZeroIntensityIgnore.Name = "cbConfigIsZeroIntensityIgnore";
            this.cbConfigIsZeroIntensityIgnore.Size = new System.Drawing.Size(150, 16);
            this.cbConfigIsZeroIntensityIgnore.TabIndex = 39;
            this.cbConfigIsZeroIntensityIgnore.Text = "Ignore Zero Intensity";
            this.cbConfigIsZeroIntensityIgnore.UseVisualStyleBackColor = true;
            this.cbConfigIsZeroIntensityIgnore.CheckedChanged += new System.EventHandler(this.cbConfigIsZeroIntensityIgnore_CheckedChanged);
            // 
            // cbConfigStack
            // 
            this.cbConfigStack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfigStack.AutoSize = true;
            this.cbConfigStack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigStack.Location = new System.Drawing.Point(35, 203);
            this.cbConfigStack.Name = "cbConfigStack";
            this.cbConfigStack.Size = new System.Drawing.Size(54, 16);
            this.cbConfigStack.TabIndex = 60;
            this.cbConfigStack.Text = "Stack";
            this.cbConfigStack.UseVisualStyleBackColor = true;
            // 
            // lblConfigIntCompA
            // 
            this.lblConfigIntCompA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigIntCompA.AutoSize = true;
            this.lblConfigIntCompA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigIntCompA.Location = new System.Drawing.Point(30, 164);
            this.lblConfigIntCompA.Name = "lblConfigIntCompA";
            this.lblConfigIntCompA.Size = new System.Drawing.Size(155, 12);
            this.lblConfigIntCompA.TabIndex = 59;
            this.lblConfigIntCompA.Text = "intensity byte compressor";
            // 
            // configIntByteComp
            // 
            this.configIntByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configIntByteComp.FormattingEnabled = true;
            this.configIntByteComp.Location = new System.Drawing.Point(194, 161);
            this.configIntByteComp.Name = "configIntByteComp";
            this.configIntByteComp.Size = new System.Drawing.Size(82, 20);
            this.configIntByteComp.TabIndex = 58;
            // 
            // lblConfigMzByteComp
            // 
            this.lblConfigMzByteComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigMzByteComp.AutoSize = true;
            this.lblConfigMzByteComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzByteComp.Location = new System.Drawing.Point(270, 127);
            this.lblConfigMzByteComp.Name = "lblConfigMzByteComp";
            this.lblConfigMzByteComp.Size = new System.Drawing.Size(119, 12);
            this.lblConfigMzByteComp.TabIndex = 57;
            this.lblConfigMzByteComp.Text = "m/z byte compressor";
            // 
            // configMzByteComp
            // 
            this.configMzByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configMzByteComp.FormattingEnabled = true;
            this.configMzByteComp.Location = new System.Drawing.Point(409, 124);
            this.configMzByteComp.Name = "configMzByteComp";
            this.configMzByteComp.Size = new System.Drawing.Size(77, 20);
            this.configMzByteComp.TabIndex = 56;
            // 
            // lblConfigStackLayers
            // 
            this.lblConfigStackLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigStackLayers.AutoSize = true;
            this.lblConfigStackLayers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigStackLayers.Location = new System.Drawing.Point(127, 203);
            this.lblConfigStackLayers.Name = "lblConfigStackLayers";
            this.lblConfigStackLayers.Size = new System.Drawing.Size(41, 12);
            this.lblConfigStackLayers.TabIndex = 55;
            this.lblConfigStackLayers.Text = "Layers";
            // 
            // cbConfigStackLayers
            // 
            this.cbConfigStackLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigStackLayers.FormattingEnabled = true;
            this.cbConfigStackLayers.Items.AddRange(new object[] {
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024"});
            this.cbConfigStackLayers.Location = new System.Drawing.Point(181, 199);
            this.cbConfigStackLayers.Name = "cbConfigStackLayers";
            this.cbConfigStackLayers.Size = new System.Drawing.Size(84, 20);
            this.cbConfigStackLayers.TabIndex = 54;
            // 
            // lblConfigMzIntComp
            // 
            this.lblConfigMzIntComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigMzIntComp.AutoSize = true;
            this.lblConfigMzIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzIntComp.Location = new System.Drawing.Point(32, 127);
            this.lblConfigMzIntComp.Name = "lblConfigMzIntComp";
            this.lblConfigMzIntComp.Size = new System.Drawing.Size(113, 12);
            this.lblConfigMzIntComp.TabIndex = 53;
            this.lblConfigMzIntComp.Text = "m/z int compressor";
            // 
            // configMzIntComp
            // 
            this.configMzIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configMzIntComp.FormattingEnabled = true;
            this.configMzIntComp.Location = new System.Drawing.Point(160, 124);
            this.configMzIntComp.Name = "configMzIntComp";
            this.configMzIntComp.Size = new System.Drawing.Size(82, 20);
            this.configMzIntComp.TabIndex = 52;
            // 
            // btnConfigDisconnect
            // 
            this.btnConfigDisconnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigDisconnect.Location = new System.Drawing.Point(383, 271);
            this.btnConfigDisconnect.Name = "btnConfigDisconnect";
            this.btnConfigDisconnect.Size = new System.Drawing.Size(103, 27);
            this.btnConfigDisconnect.TabIndex = 51;
            this.btnConfigDisconnect.Text = "Disconnect";
            this.btnConfigDisconnect.UseVisualStyleBackColor = true;
            // 
            // lblConfigIP
            // 
            this.lblConfigIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigIP.AutoSize = true;
            this.lblConfigIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigIP.Location = new System.Drawing.Point(30, 277);
            this.lblConfigIP.Name = "lblConfigIP";
            this.lblConfigIP.Size = new System.Drawing.Size(35, 12);
            this.lblConfigIP.TabIndex = 50;
            this.lblConfigIP.Text = "Redis";
            // 
            // tbConfigHostAndPort
            // 
            this.tbConfigHostAndPort.Location = new System.Drawing.Point(86, 274);
            this.tbConfigHostAndPort.Name = "tbConfigHostAndPort";
            this.tbConfigHostAndPort.Size = new System.Drawing.Size(176, 21);
            this.tbConfigHostAndPort.TabIndex = 49;
            this.tbConfigHostAndPort.Text = "192.168.31.88:6379";
            // 
            // btnConfigConnect
            // 
            this.btnConfigConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigConnect.Location = new System.Drawing.Point(272, 272);
            this.btnConfigConnect.Name = "btnConfigConnect";
            this.btnConfigConnect.Size = new System.Drawing.Size(105, 26);
            this.btnConfigConnect.TabIndex = 48;
            this.btnConfigConnect.Text = "Connect";
            this.btnConfigConnect.UseVisualStyleBackColor = true;
            // 
            // lblConfigOperator
            // 
            this.lblConfigOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigOperator.AutoSize = true;
            this.lblConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOperator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOperator.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOperator.Location = new System.Drawing.Point(314, 235);
            this.lblConfigOperator.Name = "lblConfigOperator";
            this.lblConfigOperator.Size = new System.Drawing.Size(62, 17);
            this.lblConfigOperator.TabIndex = 47;
            this.lblConfigOperator.Text = "Operator";
            // 
            // tbConfigOperator
            // 
            this.tbConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigOperator.Location = new System.Drawing.Point(382, 232);
            this.tbConfigOperator.Name = "tbConfigOperator";
            this.tbConfigOperator.Size = new System.Drawing.Size(104, 23);
            this.tbConfigOperator.TabIndex = 46;
            // 
            // lblConfigFileNameTag
            // 
            this.lblConfigFileNameTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigFileNameTag.AutoSize = true;
            this.lblConfigFileNameTag.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigFileNameTag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigFileNameTag.Location = new System.Drawing.Point(30, 235);
            this.lblConfigFileNameTag.Name = "lblConfigFileNameTag";
            this.lblConfigFileNameTag.Size = new System.Drawing.Size(62, 17);
            this.lblConfigFileNameTag.TabIndex = 45;
            this.lblConfigFileNameTag.Text = "File Suffix";
            // 
            // tbConfigFileNameSuffix
            // 
            this.tbConfigFileNameSuffix.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFileNameSuffix.Location = new System.Drawing.Point(110, 232);
            this.tbConfigFileNameSuffix.Name = "tbConfigFileNameSuffix";
            this.tbConfigFileNameSuffix.Size = new System.Drawing.Size(185, 23);
            this.tbConfigFileNameSuffix.TabIndex = 44;
            // 
            // lblNameConfig
            // 
            this.lblNameConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNameConfig.AutoSize = true;
            this.lblNameConfig.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblNameConfig.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNameConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNameConfig.Location = new System.Drawing.Point(29, 328);
            this.lblNameConfig.Name = "lblNameConfig";
            this.lblNameConfig.Size = new System.Drawing.Size(107, 16);
            this.lblNameConfig.TabIndex = 61;
            this.lblNameConfig.Text = "Name Your Config:";
            // 
            // tbNameConfig
            // 
            this.tbNameConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbNameConfig.Location = new System.Drawing.Point(142, 324);
            this.tbNameConfig.Name = "tbNameConfig";
            this.tbNameConfig.Size = new System.Drawing.Size(185, 23);
            this.tbNameConfig.TabIndex = 62;
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveConfig.Location = new System.Drawing.Point(370, 316);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(116, 38);
            this.btnSaveConfig.TabIndex = 63;
            this.btnSaveConfig.Text = "Save";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // ConfigCustomization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 371);
            this.Controls.Add(this.btnSaveConfig);
            this.Controls.Add(this.tbNameConfig);
            this.Controls.Add(this.lblNameConfig);
            this.Controls.Add(this.cbConfigStack);
            this.Controls.Add(this.lblConfigIntCompA);
            this.Controls.Add(this.configIntByteComp);
            this.Controls.Add(this.lblConfigMzByteComp);
            this.Controls.Add(this.configMzByteComp);
            this.Controls.Add(this.lblConfigStackLayers);
            this.Controls.Add(this.cbConfigStackLayers);
            this.Controls.Add(this.lblConfigMzIntComp);
            this.Controls.Add(this.configMzIntComp);
            this.Controls.Add(this.btnConfigDisconnect);
            this.Controls.Add(this.lblConfigIP);
            this.Controls.Add(this.tbConfigHostAndPort);
            this.Controls.Add(this.btnConfigConnect);
            this.Controls.Add(this.lblConfigOperator);
            this.Controls.Add(this.tbConfigOperator);
            this.Controls.Add(this.lblConfigFileNameTag);
            this.Controls.Add(this.tbConfigFileNameSuffix);
            this.Controls.Add(this.lblDp);
            this.Controls.Add(this.lblMzPrecision);
            this.Controls.Add(this.cbConfigMzPrecision);
            this.Controls.Add(this.cbConfigThreadAccelerate);
            this.Controls.Add(this.cbConfigIsZeroIntensityIgnore);
            this.Controls.Add(this.lblConfigOutputPath);
            this.Controls.Add(this.btnConfigChooseFolder);
            this.Controls.Add(this.tbConfigFolderPath);
            this.Name = "ConfigCustomization";
            this.Text = "ConfigCustomization";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConfigOutputPath;
        private System.Windows.Forms.Button btnConfigChooseFolder;
        private System.Windows.Forms.TextBox tbConfigFolderPath;
        private System.Windows.Forms.Label lblDp;
        private System.Windows.Forms.Label lblMzPrecision;
        private System.Windows.Forms.ComboBox cbConfigMzPrecision;
        private System.Windows.Forms.CheckBox cbConfigThreadAccelerate;
        private System.Windows.Forms.CheckBox cbConfigIsZeroIntensityIgnore;
        private System.Windows.Forms.CheckBox cbConfigStack;
        private System.Windows.Forms.Label lblConfigIntCompA;
        private System.Windows.Forms.ComboBox configIntByteComp;
        private System.Windows.Forms.Label lblConfigMzByteComp;
        private System.Windows.Forms.ComboBox configMzByteComp;
        private System.Windows.Forms.Label lblConfigStackLayers;
        private System.Windows.Forms.ComboBox cbConfigStackLayers;
        private System.Windows.Forms.Label lblConfigMzIntComp;
        private System.Windows.Forms.ComboBox configMzIntComp;
        private System.Windows.Forms.Button btnConfigDisconnect;
        private System.Windows.Forms.Label lblConfigIP;
        private System.Windows.Forms.TextBox tbConfigHostAndPort;
        private System.Windows.Forms.Button btnConfigConnect;
        private System.Windows.Forms.Label lblConfigOperator;
        private System.Windows.Forms.TextBox tbConfigOperator;
        private System.Windows.Forms.Label lblConfigFileNameTag;
        private System.Windows.Forms.TextBox tbConfigFileNameSuffix;
        private System.Windows.Forms.Label lblNameConfig;
        private System.Windows.Forms.TextBox tbNameConfig;
        private System.Windows.Forms.Button btnSaveConfig;
    }
}