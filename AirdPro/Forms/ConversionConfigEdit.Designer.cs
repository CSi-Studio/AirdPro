namespace AirdPro.Forms
{
    partial class ConversionConfigEdit
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.tbNameConfig = new System.Windows.Forms.TextBox();
            this.lblNameConfig = new System.Windows.Forms.Label();
            this.cbConfigStack = new System.Windows.Forms.CheckBox();
            this.lblConfigIntCompA = new System.Windows.Forms.Label();
            this.configIntByteComp = new System.Windows.Forms.ComboBox();
            this.lblConfigMzByteComp = new System.Windows.Forms.Label();
            this.configMzByteComp = new System.Windows.Forms.ComboBox();
            this.lblConfigStackLayers = new System.Windows.Forms.Label();
            this.cbConfigStackLayers = new System.Windows.Forms.ComboBox();
            this.lblConfigMzIntComp = new System.Windows.Forms.Label();
            this.configMzIntComp = new System.Windows.Forms.ComboBox();
            this.lblConfigOperator = new System.Windows.Forms.Label();
            this.tbConfigOperator = new System.Windows.Forms.TextBox();
            this.lblConfigFileNameTag = new System.Windows.Forms.Label();
            this.tbConfigFileNameSuffix = new System.Windows.Forms.TextBox();
            this.lblDp = new System.Windows.Forms.Label();
            this.lblMzPrecision = new System.Windows.Forms.Label();
            this.cbConfigMzPrecision = new System.Windows.Forms.ComboBox();
            this.cbConfigThreadAccelerate = new System.Windows.Forms.CheckBox();
            this.cbConfigIsZeroIntensityIgnore = new System.Windows.Forms.CheckBox();
            this.lblConfigOutputPath = new System.Windows.Forms.Label();
            this.btnConfigChooseFolder = new System.Windows.Forms.Button();
            this.tbConfigFolderPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveConfig.Location = new System.Drawing.Point(325, 312);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(116, 38);
            this.btnSaveConfig.TabIndex = 91;
            this.btnSaveConfig.Text = "Save";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            // 
            // tbNameConfig
            // 
            this.tbNameConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbNameConfig.Location = new System.Drawing.Point(94, 27);
            this.tbNameConfig.Name = "tbNameConfig";
            this.tbNameConfig.Size = new System.Drawing.Size(185, 23);
            this.tbNameConfig.TabIndex = 90;
            // 
            // lblNameConfig
            // 
            this.lblNameConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNameConfig.AutoSize = true;
            this.lblNameConfig.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblNameConfig.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNameConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNameConfig.Location = new System.Drawing.Point(9, 31);
            this.lblNameConfig.Name = "lblNameConfig";
            this.lblNameConfig.Size = new System.Drawing.Size(79, 16);
            this.lblNameConfig.TabIndex = 89;
            this.lblNameConfig.Text = "Config Name:";
            // 
            // cbConfigStack
            // 
            this.cbConfigStack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfigStack.AutoSize = true;
            this.cbConfigStack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigStack.Location = new System.Drawing.Point(28, 297);
            this.cbConfigStack.Name = "cbConfigStack";
            this.cbConfigStack.Size = new System.Drawing.Size(54, 16);
            this.cbConfigStack.TabIndex = 88;
            this.cbConfigStack.Text = "Stack";
            this.cbConfigStack.UseVisualStyleBackColor = true;
            // 
            // lblConfigIntCompA
            // 
            this.lblConfigIntCompA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigIntCompA.AutoSize = true;
            this.lblConfigIntCompA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigIntCompA.Location = new System.Drawing.Point(23, 253);
            this.lblConfigIntCompA.Name = "lblConfigIntCompA";
            this.lblConfigIntCompA.Size = new System.Drawing.Size(155, 12);
            this.lblConfigIntCompA.TabIndex = 87;
            this.lblConfigIntCompA.Text = "intensity byte compressor";
            // 
            // configIntByteComp
            // 
            this.configIntByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configIntByteComp.FormattingEnabled = true;
            this.configIntByteComp.Location = new System.Drawing.Point(187, 250);
            this.configIntByteComp.Name = "configIntByteComp";
            this.configIntByteComp.Size = new System.Drawing.Size(82, 20);
            this.configIntByteComp.TabIndex = 86;
            // 
            // lblConfigMzByteComp
            // 
            this.lblConfigMzByteComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigMzByteComp.AutoSize = true;
            this.lblConfigMzByteComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzByteComp.Location = new System.Drawing.Point(263, 216);
            this.lblConfigMzByteComp.Name = "lblConfigMzByteComp";
            this.lblConfigMzByteComp.Size = new System.Drawing.Size(119, 12);
            this.lblConfigMzByteComp.TabIndex = 85;
            this.lblConfigMzByteComp.Text = "m/z byte compressor";
            // 
            // configMzByteComp
            // 
            this.configMzByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configMzByteComp.FormattingEnabled = true;
            this.configMzByteComp.Location = new System.Drawing.Point(402, 213);
            this.configMzByteComp.Name = "configMzByteComp";
            this.configMzByteComp.Size = new System.Drawing.Size(77, 20);
            this.configMzByteComp.TabIndex = 84;
            // 
            // lblConfigStackLayers
            // 
            this.lblConfigStackLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigStackLayers.AutoSize = true;
            this.lblConfigStackLayers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigStackLayers.Location = new System.Drawing.Point(120, 297);
            this.lblConfigStackLayers.Name = "lblConfigStackLayers";
            this.lblConfigStackLayers.Size = new System.Drawing.Size(41, 12);
            this.lblConfigStackLayers.TabIndex = 83;
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
            this.cbConfigStackLayers.Location = new System.Drawing.Point(174, 293);
            this.cbConfigStackLayers.Name = "cbConfigStackLayers";
            this.cbConfigStackLayers.Size = new System.Drawing.Size(84, 20);
            this.cbConfigStackLayers.TabIndex = 82;
            // 
            // lblConfigMzIntComp
            // 
            this.lblConfigMzIntComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigMzIntComp.AutoSize = true;
            this.lblConfigMzIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzIntComp.Location = new System.Drawing.Point(25, 216);
            this.lblConfigMzIntComp.Name = "lblConfigMzIntComp";
            this.lblConfigMzIntComp.Size = new System.Drawing.Size(113, 12);
            this.lblConfigMzIntComp.TabIndex = 81;
            this.lblConfigMzIntComp.Text = "m/z int compressor";
            // 
            // configMzIntComp
            // 
            this.configMzIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configMzIntComp.FormattingEnabled = true;
            this.configMzIntComp.Location = new System.Drawing.Point(153, 213);
            this.configMzIntComp.Name = "configMzIntComp";
            this.configMzIntComp.Size = new System.Drawing.Size(82, 20);
            this.configMzIntComp.TabIndex = 80;
            // 
            // lblConfigOperator
            // 
            this.lblConfigOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigOperator.AutoSize = true;
            this.lblConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOperator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOperator.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOperator.Location = new System.Drawing.Point(308, 133);
            this.lblConfigOperator.Name = "lblConfigOperator";
            this.lblConfigOperator.Size = new System.Drawing.Size(62, 17);
            this.lblConfigOperator.TabIndex = 75;
            this.lblConfigOperator.Text = "Operator";
            // 
            // tbConfigOperator
            // 
            this.tbConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigOperator.Location = new System.Drawing.Point(376, 130);
            this.tbConfigOperator.Name = "tbConfigOperator";
            this.tbConfigOperator.Size = new System.Drawing.Size(104, 23);
            this.tbConfigOperator.TabIndex = 74;
            // 
            // lblConfigFileNameTag
            // 
            this.lblConfigFileNameTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigFileNameTag.AutoSize = true;
            this.lblConfigFileNameTag.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigFileNameTag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigFileNameTag.Location = new System.Drawing.Point(24, 133);
            this.lblConfigFileNameTag.Name = "lblConfigFileNameTag";
            this.lblConfigFileNameTag.Size = new System.Drawing.Size(62, 17);
            this.lblConfigFileNameTag.TabIndex = 73;
            this.lblConfigFileNameTag.Text = "File Suffix";
            // 
            // tbConfigFileNameSuffix
            // 
            this.tbConfigFileNameSuffix.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFileNameSuffix.Location = new System.Drawing.Point(104, 130);
            this.tbConfigFileNameSuffix.Name = "tbConfigFileNameSuffix";
            this.tbConfigFileNameSuffix.Size = new System.Drawing.Size(185, 23);
            this.tbConfigFileNameSuffix.TabIndex = 72;
            // 
            // lblDp
            // 
            this.lblDp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDp.AutoSize = true;
            this.lblDp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDp.Location = new System.Drawing.Point(462, 175);
            this.lblDp.Name = "lblDp";
            this.lblDp.Size = new System.Drawing.Size(17, 12);
            this.lblDp.TabIndex = 71;
            this.lblDp.Text = "dp";
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMzPrecision.Location = new System.Drawing.Point(367, 174);
            this.lblMzPrecision.Name = "lblMzPrecision";
            this.lblMzPrecision.Size = new System.Drawing.Size(23, 12);
            this.lblMzPrecision.TabIndex = 70;
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
            this.cbConfigMzPrecision.Location = new System.Drawing.Point(403, 171);
            this.cbConfigMzPrecision.Name = "cbConfigMzPrecision";
            this.cbConfigMzPrecision.Size = new System.Drawing.Size(55, 20);
            this.cbConfigMzPrecision.TabIndex = 69;
            // 
            // cbConfigThreadAccelerate
            // 
            this.cbConfigThreadAccelerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfigThreadAccelerate.AutoSize = true;
            this.cbConfigThreadAccelerate.Checked = true;
            this.cbConfigThreadAccelerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigThreadAccelerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigThreadAccelerate.Location = new System.Drawing.Point(206, 175);
            this.cbConfigThreadAccelerate.Name = "cbConfigThreadAccelerate";
            this.cbConfigThreadAccelerate.Size = new System.Drawing.Size(108, 16);
            this.cbConfigThreadAccelerate.TabIndex = 68;
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
            this.cbConfigIsZeroIntensityIgnore.Location = new System.Drawing.Point(27, 175);
            this.cbConfigIsZeroIntensityIgnore.Name = "cbConfigIsZeroIntensityIgnore";
            this.cbConfigIsZeroIntensityIgnore.Size = new System.Drawing.Size(150, 16);
            this.cbConfigIsZeroIntensityIgnore.TabIndex = 67;
            this.cbConfigIsZeroIntensityIgnore.Text = "Ignore Zero Intensity";
            this.cbConfigIsZeroIntensityIgnore.UseVisualStyleBackColor = true;
            // 
            // lblConfigOutputPath
            // 
            this.lblConfigOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigOutputPath.AutoSize = true;
            this.lblConfigOutputPath.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblConfigOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOutputPath.Location = new System.Drawing.Point(11, 83);
            this.lblConfigOutputPath.Name = "lblConfigOutputPath";
            this.lblConfigOutputPath.Size = new System.Drawing.Size(75, 16);
            this.lblConfigOutputPath.TabIndex = 65;
            this.lblConfigOutputPath.Text = "Output Path:";
            // 
            // btnConfigChooseFolder
            // 
            this.btnConfigChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigChooseFolder.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnConfigChooseFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(402, 79);
            this.btnConfigChooseFolder.Name = "btnConfigChooseFolder";
            this.btnConfigChooseFolder.Size = new System.Drawing.Size(75, 26);
            this.btnConfigChooseFolder.TabIndex = 66;
            this.btnConfigChooseFolder.Text = "Browser";
            this.btnConfigChooseFolder.UseVisualStyleBackColor = true;
            // 
            // tbConfigFolderPath
            // 
            this.tbConfigFolderPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConfigFolderPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFolderPath.Location = new System.Drawing.Point(92, 79);
            this.tbConfigFolderPath.Name = "tbConfigFolderPath";
            this.tbConfigFolderPath.Size = new System.Drawing.Size(298, 23);
            this.tbConfigFolderPath.TabIndex = 64;
            // 
            // ConversionConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Name = "ConversionConfigEdit";
            this.Size = new System.Drawing.Size(507, 353);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.TextBox tbNameConfig;
        private System.Windows.Forms.Label lblNameConfig;
        private System.Windows.Forms.CheckBox cbConfigStack;
        private System.Windows.Forms.Label lblConfigIntCompA;
        private System.Windows.Forms.ComboBox configIntByteComp;
        private System.Windows.Forms.Label lblConfigMzByteComp;
        private System.Windows.Forms.ComboBox configMzByteComp;
        private System.Windows.Forms.Label lblConfigStackLayers;
        private System.Windows.Forms.ComboBox cbConfigStackLayers;
        private System.Windows.Forms.Label lblConfigMzIntComp;
        private System.Windows.Forms.ComboBox configMzIntComp;
        private System.Windows.Forms.Label lblConfigOperator;
        private System.Windows.Forms.TextBox tbConfigOperator;
        private System.Windows.Forms.Label lblConfigFileNameTag;
        private System.Windows.Forms.TextBox tbConfigFileNameSuffix;
        private System.Windows.Forms.Label lblDp;
        private System.Windows.Forms.Label lblMzPrecision;
        private System.Windows.Forms.ComboBox cbConfigMzPrecision;
        private System.Windows.Forms.CheckBox cbConfigThreadAccelerate;
        private System.Windows.Forms.CheckBox cbConfigIsZeroIntensityIgnore;
        private System.Windows.Forms.Label lblConfigOutputPath;
        private System.Windows.Forms.Button btnConfigChooseFolder;
        private System.Windows.Forms.TextBox tbConfigFolderPath;
    }
}
