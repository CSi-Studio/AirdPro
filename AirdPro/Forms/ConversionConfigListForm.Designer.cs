namespace AirdPro.Forms
{
    partial class ConversionConfigListForm
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
            this.components = new System.ComponentModel.Container();
            this.headerConfigList = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvConfigList = new System.Windows.Forms.ListView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveToLocal = new System.Windows.Forms.Button();
            this.tbNameConfig = new System.Windows.Forms.TextBox();
            this.lblNameConfig = new System.Windows.Forms.Label();
            this.cbConfigStack = new System.Windows.Forms.CheckBox();
            this.lblConfigIntCompA = new System.Windows.Forms.Label();
            this.configIntByteComp = new System.Windows.Forms.ComboBox();
            this.lblConfigMzByteComp = new System.Windows.Forms.Label();
            this.configMzByteComp = new System.Windows.Forms.ComboBox();
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
            this.btnApplyNow = new System.Windows.Forms.Button();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerConfigList
            // 
            this.headerConfigList.Text = "Config List";
            this.headerConfigList.Width = 208;
            // 
            // lvConfigList
            // 
            this.lvConfigList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvConfigList.BackColor = System.Drawing.SystemColors.Window;
            this.lvConfigList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerConfigList});
            this.lvConfigList.ContextMenuStrip = this.contextMenu;
            this.lvConfigList.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lvConfigList.FullRowSelect = true;
            this.lvConfigList.GridLines = true;
            this.lvConfigList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvConfigList.HideSelection = false;
            this.lvConfigList.Location = new System.Drawing.Point(1, 3);
            this.lvConfigList.Name = "lvConfigList";
            this.lvConfigList.ShowGroups = false;
            this.lvConfigList.ShowItemToolTips = true;
            this.lvConfigList.Size = new System.Drawing.Size(213, 355);
            this.lvConfigList.TabIndex = 8;
            this.lvConfigList.UseCompatibleStateImageBehavior = false;
            this.lvConfigList.View = System.Windows.Forms.View.Details;
            this.lvConfigList.SelectedIndexChanged += new System.EventHandler(this.lvConfigList_SelectedIndexChanged);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(114, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // btnSaveToLocal
            // 
            this.btnSaveToLocal.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveToLocal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveToLocal.Location = new System.Drawing.Point(356, 316);
            this.btnSaveToLocal.Name = "btnSaveToLocal";
            this.btnSaveToLocal.Size = new System.Drawing.Size(116, 38);
            this.btnSaveToLocal.TabIndex = 138;
            this.btnSaveToLocal.Text = "Save";
            this.btnSaveToLocal.UseVisualStyleBackColor = true;
            this.btnSaveToLocal.Click += new System.EventHandler(this.btnSaveToLocal_Click);
            // 
            // tbNameConfig
            // 
            this.tbNameConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbNameConfig.Location = new System.Drawing.Point(311, 5);
            this.tbNameConfig.Name = "tbNameConfig";
            this.tbNameConfig.Size = new System.Drawing.Size(185, 23);
            this.tbNameConfig.TabIndex = 137;
            // 
            // lblNameConfig
            // 
            this.lblNameConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNameConfig.AutoSize = true;
            this.lblNameConfig.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblNameConfig.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNameConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNameConfig.Location = new System.Drawing.Point(226, 9);
            this.lblNameConfig.Name = "lblNameConfig";
            this.lblNameConfig.Size = new System.Drawing.Size(76, 16);
            this.lblNameConfig.TabIndex = 136;
            this.lblNameConfig.Text = "Config Name";
            // 
            // cbConfigStack
            // 
            this.cbConfigStack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfigStack.AutoSize = true;
            this.cbConfigStack.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigStack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigStack.Location = new System.Drawing.Point(260, 166);
            this.cbConfigStack.Name = "cbConfigStack";
            this.cbConfigStack.Size = new System.Drawing.Size(93, 21);
            this.cbConfigStack.TabIndex = 135;
            this.cbConfigStack.Text = "Stack Layer";
            this.cbConfigStack.UseVisualStyleBackColor = true;
            this.cbConfigStack.CheckedChanged += new System.EventHandler(this.cbConfigStack_CheckedChanged);
            // 
            // lblConfigIntCompA
            // 
            this.lblConfigIntCompA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigIntCompA.AutoSize = true;
            this.lblConfigIntCompA.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigIntCompA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigIntCompA.Location = new System.Drawing.Point(220, 281);
            this.lblConfigIntCompA.Name = "lblConfigIntCompA";
            this.lblConfigIntCompA.Size = new System.Drawing.Size(158, 17);
            this.lblConfigIntCompA.TabIndex = 134;
            this.lblConfigIntCompA.Text = "intensity byte compressor";
            // 
            // configIntByteComp
            // 
            this.configIntByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configIntByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.configIntByteComp.FormattingEnabled = true;
            this.configIntByteComp.Location = new System.Drawing.Point(384, 278);
            this.configIntByteComp.Name = "configIntByteComp";
            this.configIntByteComp.Size = new System.Drawing.Size(82, 25);
            this.configIntByteComp.TabIndex = 133;
            // 
            // lblConfigMzByteComp
            // 
            this.lblConfigMzByteComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigMzByteComp.AutoSize = true;
            this.lblConfigMzByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigMzByteComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzByteComp.Location = new System.Drawing.Point(233, 255);
            this.lblConfigMzByteComp.Name = "lblConfigMzByteComp";
            this.lblConfigMzByteComp.Size = new System.Drawing.Size(133, 17);
            this.lblConfigMzByteComp.TabIndex = 132;
            this.lblConfigMzByteComp.Text = "m/z byte compressor";
            // 
            // configMzByteComp
            // 
            this.configMzByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configMzByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.configMzByteComp.FormattingEnabled = true;
            this.configMzByteComp.Location = new System.Drawing.Point(384, 251);
            this.configMzByteComp.Name = "configMzByteComp";
            this.configMzByteComp.Size = new System.Drawing.Size(82, 25);
            this.configMzByteComp.TabIndex = 131;
            // 
            // cbConfigStackLayers
            // 
            this.cbConfigStackLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigStackLayers.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigStackLayers.FormattingEnabled = true;
            this.cbConfigStackLayers.Items.AddRange(new object[] {
            "32",
            "64",
            "128",
            "256",
            "512",
            "1024"});
            this.cbConfigStackLayers.Location = new System.Drawing.Point(384, 164);
            this.cbConfigStackLayers.Name = "cbConfigStackLayers";
            this.cbConfigStackLayers.Size = new System.Drawing.Size(82, 25);
            this.cbConfigStackLayers.TabIndex = 129;
            this.cbConfigStackLayers.Visible = false;
            // 
            // lblConfigMzIntComp
            // 
            this.lblConfigMzIntComp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigMzIntComp.AutoSize = true;
            this.lblConfigMzIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigMzIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzIntComp.Location = new System.Drawing.Point(244, 228);
            this.lblConfigMzIntComp.Name = "lblConfigMzIntComp";
            this.lblConfigMzIntComp.Size = new System.Drawing.Size(122, 17);
            this.lblConfigMzIntComp.TabIndex = 128;
            this.lblConfigMzIntComp.Text = "m/z int compressor";
            // 
            // configMzIntComp
            // 
            this.configMzIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.configMzIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.configMzIntComp.FormattingEnabled = true;
            this.configMzIntComp.Location = new System.Drawing.Point(384, 224);
            this.configMzIntComp.Name = "configMzIntComp";
            this.configMzIntComp.Size = new System.Drawing.Size(82, 25);
            this.configMzIntComp.TabIndex = 127;
            // 
            // lblConfigOperator
            // 
            this.lblConfigOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigOperator.AutoSize = true;
            this.lblConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOperator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOperator.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOperator.Location = new System.Drawing.Point(233, 52);
            this.lblConfigOperator.Name = "lblConfigOperator";
            this.lblConfigOperator.Size = new System.Drawing.Size(62, 17);
            this.lblConfigOperator.TabIndex = 126;
            this.lblConfigOperator.Text = "Operator";
            // 
            // tbConfigOperator
            // 
            this.tbConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigOperator.Location = new System.Drawing.Point(311, 52);
            this.tbConfigOperator.Name = "tbConfigOperator";
            this.tbConfigOperator.Size = new System.Drawing.Size(183, 23);
            this.tbConfigOperator.TabIndex = 125;
            // 
            // lblConfigFileNameTag
            // 
            this.lblConfigFileNameTag.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigFileNameTag.AutoSize = true;
            this.lblConfigFileNameTag.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigFileNameTag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigFileNameTag.Location = new System.Drawing.Point(233, 81);
            this.lblConfigFileNameTag.Name = "lblConfigFileNameTag";
            this.lblConfigFileNameTag.Size = new System.Drawing.Size(62, 17);
            this.lblConfigFileNameTag.TabIndex = 124;
            this.lblConfigFileNameTag.Text = "File Suffix";
            // 
            // tbConfigFileNameSuffix
            // 
            this.tbConfigFileNameSuffix.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFileNameSuffix.Location = new System.Drawing.Point(311, 81);
            this.tbConfigFileNameSuffix.Name = "tbConfigFileNameSuffix";
            this.tbConfigFileNameSuffix.Size = new System.Drawing.Size(185, 23);
            this.tbConfigFileNameSuffix.TabIndex = 123;
            // 
            // lblDp
            // 
            this.lblDp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDp.AutoSize = true;
            this.lblDp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDp.Location = new System.Drawing.Point(448, 197);
            this.lblDp.Name = "lblDp";
            this.lblDp.Size = new System.Drawing.Size(24, 17);
            this.lblDp.TabIndex = 122;
            this.lblDp.Text = "dp";
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMzPrecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMzPrecision.Location = new System.Drawing.Point(283, 198);
            this.lblMzPrecision.Name = "lblMzPrecision";
            this.lblMzPrecision.Size = new System.Drawing.Size(30, 17);
            this.lblMzPrecision.TabIndex = 121;
            this.lblMzPrecision.Text = "m/z";
            // 
            // cbConfigMzPrecision
            // 
            this.cbConfigMzPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigMzPrecision.FormattingEnabled = true;
            this.cbConfigMzPrecision.Items.AddRange(new object[] {
            "4",
            "5",
            "6"});
            this.cbConfigMzPrecision.Location = new System.Drawing.Point(384, 194);
            this.cbConfigMzPrecision.Name = "cbConfigMzPrecision";
            this.cbConfigMzPrecision.Size = new System.Drawing.Size(58, 25);
            this.cbConfigMzPrecision.TabIndex = 120;
            // 
            // cbConfigThreadAccelerate
            // 
            this.cbConfigThreadAccelerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfigThreadAccelerate.AutoSize = true;
            this.cbConfigThreadAccelerate.Checked = true;
            this.cbConfigThreadAccelerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigThreadAccelerate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigThreadAccelerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigThreadAccelerate.Location = new System.Drawing.Point(382, 112);
            this.cbConfigThreadAccelerate.Name = "cbConfigThreadAccelerate";
            this.cbConfigThreadAccelerate.Size = new System.Drawing.Size(112, 21);
            this.cbConfigThreadAccelerate.TabIndex = 119;
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
            this.cbConfigIsZeroIntensityIgnore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigIsZeroIntensityIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigIsZeroIntensityIgnore.Location = new System.Drawing.Point(220, 112);
            this.cbConfigIsZeroIntensityIgnore.Name = "cbConfigIsZeroIntensityIgnore";
            this.cbConfigIsZeroIntensityIgnore.Size = new System.Drawing.Size(149, 21);
            this.cbConfigIsZeroIntensityIgnore.TabIndex = 118;
            this.cbConfigIsZeroIntensityIgnore.Text = "Ignore Zero Intensity";
            this.cbConfigIsZeroIntensityIgnore.UseVisualStyleBackColor = true;
            this.cbConfigIsZeroIntensityIgnore.CheckedChanged += new System.EventHandler(this.cbConfigIsZeroIntensityIgnore_CheckedChanged);
            // 
            // btnApplyNow
            // 
            this.btnApplyNow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApplyNow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnApplyNow.Location = new System.Drawing.Point(228, 316);
            this.btnApplyNow.Name = "btnApplyNow";
            this.btnApplyNow.Size = new System.Drawing.Size(116, 38);
            this.btnApplyNow.TabIndex = 139;
            this.btnApplyNow.Text = "Apply";
            this.btnApplyNow.UseVisualStyleBackColor = true;
            this.btnApplyNow.Click += new System.EventHandler(this.btnApplyNow_Click);
            // 
            // ConversionConfigListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 359);
            this.Controls.Add(this.btnApplyNow);
            this.Controls.Add(this.btnSaveToLocal);
            this.Controls.Add(this.tbNameConfig);
            this.Controls.Add(this.lblNameConfig);
            this.Controls.Add(this.cbConfigStack);
            this.Controls.Add(this.lblConfigIntCompA);
            this.Controls.Add(this.configIntByteComp);
            this.Controls.Add(this.lblConfigMzByteComp);
            this.Controls.Add(this.configMzByteComp);
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
            this.Controls.Add(this.lvConfigList);
            this.Name = "ConversionConfigListForm";
            this.ShowIcon = false;
            this.Text = "Conversion Config List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConversionConfigListForm_FormClosed);
            this.Load += new System.EventHandler(this.ConversionConfigListForm_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader headerConfigList;
        public System.Windows.Forms.ListView lvConfigList;
        private System.Windows.Forms.Button btnSaveToLocal;
        private System.Windows.Forms.Label lblNameConfig;
        private System.Windows.Forms.Label lblConfigIntCompA;
        private System.Windows.Forms.Label lblConfigMzByteComp;
        private System.Windows.Forms.Label lblConfigMzIntComp;
        private System.Windows.Forms.Label lblConfigOperator;
        private System.Windows.Forms.Label lblConfigFileNameTag;
        private System.Windows.Forms.Label lblDp;
        private System.Windows.Forms.Label lblMzPrecision;
        private System.Windows.Forms.Button btnApplyNow;
        public System.Windows.Forms.TextBox tbNameConfig;
        public System.Windows.Forms.TextBox tbConfigFileNameSuffix;
        public System.Windows.Forms.TextBox tbConfigOperator;
        public System.Windows.Forms.CheckBox cbConfigIsZeroIntensityIgnore;
        public System.Windows.Forms.CheckBox cbConfigThreadAccelerate;
        public System.Windows.Forms.ComboBox cbConfigMzPrecision;
        public System.Windows.Forms.ComboBox configMzIntComp;
        public System.Windows.Forms.ComboBox configMzByteComp;
        public System.Windows.Forms.ComboBox configIntByteComp;
        public System.Windows.Forms.CheckBox cbConfigStack;
        public System.Windows.Forms.ComboBox cbConfigStackLayers;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}