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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversionConfigListForm));
            this.headerConfigList = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvConfigList = new System.Windows.Forms.ListView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveToLocal = new System.Windows.Forms.Button();
            this.tbNameConfig = new System.Windows.Forms.TextBox();
            this.lblNameConfig = new System.Windows.Forms.Label();
            this.cbConfigStack = new System.Windows.Forms.CheckBox();
            this.lblConfigIntIntComp = new System.Windows.Forms.Label();
            this.cbIntIntComp = new System.Windows.Forms.ComboBox();
            this.cbMzByteComp = new System.Windows.Forms.ComboBox();
            this.cbConfigStackLayers = new System.Windows.Forms.ComboBox();
            this.lblConfigMzIntComp = new System.Windows.Forms.Label();
            this.cbMzIntComp = new System.Windows.Forms.ComboBox();
            this.lblConfigOperator = new System.Windows.Forms.Label();
            this.tbConfigOperator = new System.Windows.Forms.TextBox();
            this.lblConfigFileNameTag = new System.Windows.Forms.Label();
            this.tbConfigFileNameSuffix = new System.Windows.Forms.TextBox();
            this.lblMzPrecision = new System.Windows.Forms.Label();
            this.cbConfigMzPrecision = new System.Windows.Forms.ComboBox();
            this.cbConfigThreadAccelerate = new System.Windows.Forms.CheckBox();
            this.cbConfigIsZeroIntensityIgnore = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.cbIntByteComp = new System.Windows.Forms.ComboBox();
            this.cbMobiByteComp = new System.Windows.Forms.ComboBox();
            this.cbMobiIntComp = new System.Windows.Forms.ComboBox();
            this.lblConfigMobiIntComp = new System.Windows.Forms.Label();
            this.cbAutoDecision = new System.Windows.Forms.CheckBox();
            this.lblIntegerPurpose = new System.Windows.Forms.Label();
            this.lblGeneralPurpose = new System.Windows.Forms.Label();
            this.tableAutoDecision = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbAutoExplore = new System.Windows.Forms.CheckBox();
            this.contextMenu.SuspendLayout();
            this.tableAutoDecision.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.lvConfigList.Location = new System.Drawing.Point(2, 4);
            this.lvConfigList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lvConfigList.Name = "lvConfigList";
            this.lvConfigList.ShowGroups = false;
            this.lvConfigList.ShowItemToolTips = true;
            this.lvConfigList.Size = new System.Drawing.Size(318, 736);
            this.lvConfigList.TabIndex = 8;
            this.lvConfigList.UseCompatibleStateImageBehavior = false;
            this.lvConfigList.View = System.Windows.Forms.View.Details;
            this.lvConfigList.SelectedIndexChanged += new System.EventHandler(this.lvConfigList_SelectedIndexChanged);
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(137, 34);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(136, 30);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // btnSaveToLocal
            // 
            this.btnSaveToLocal.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveToLocal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveToLocal.Location = new System.Drawing.Point(598, 669);
            this.btnSaveToLocal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveToLocal.Name = "btnSaveToLocal";
            this.btnSaveToLocal.Size = new System.Drawing.Size(174, 57);
            this.btnSaveToLocal.TabIndex = 138;
            this.btnSaveToLocal.Text = "Save";
            this.btnSaveToLocal.UseVisualStyleBackColor = true;
            this.btnSaveToLocal.Click += new System.EventHandler(this.btnSaveToLocal_Click);
            // 
            // tbNameConfig
            // 
            this.tbNameConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbNameConfig.Location = new System.Drawing.Point(466, 18);
            this.tbNameConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbNameConfig.Name = "tbNameConfig";
            this.tbNameConfig.Size = new System.Drawing.Size(276, 31);
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
            this.lblNameConfig.Location = new System.Drawing.Point(339, 24);
            this.lblNameConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNameConfig.Name = "lblNameConfig";
            this.lblNameConfig.Size = new System.Drawing.Size(112, 21);
            this.lblNameConfig.TabIndex = 136;
            this.lblNameConfig.Text = "Config Name";
            // 
            // cbConfigStack
            // 
            this.cbConfigStack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbConfigStack.AutoSize = true;
            this.cbConfigStack.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigStack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigStack.Location = new System.Drawing.Point(19, 156);
            this.cbConfigStack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbConfigStack.Name = "cbConfigStack";
            this.cbConfigStack.Size = new System.Drawing.Size(133, 28);
            this.cbConfigStack.TabIndex = 135;
            this.cbConfigStack.Text = "Stack Layer";
            this.cbConfigStack.UseVisualStyleBackColor = true;
            this.cbConfigStack.CheckedChanged += new System.EventHandler(this.cbConfigStack_CheckedChanged);
            // 
            // lblConfigIntIntComp
            // 
            this.lblConfigIntIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigIntIntComp.AutoSize = true;
            this.lblConfigIntIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigIntIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigIntIntComp.Location = new System.Drawing.Point(18, 93);
            this.lblConfigIntIntComp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigIntIntComp.Name = "lblConfigIntIntComp";
            this.lblConfigIntIntComp.Size = new System.Drawing.Size(84, 24);
            this.lblConfigIntIntComp.TabIndex = 134;
            this.lblConfigIntIntComp.Text = "intensity";
            // 
            // cbIntIntComp
            // 
            this.cbIntIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbIntIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIntIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbIntIntComp.FormattingEnabled = true;
            this.cbIntIntComp.Location = new System.Drawing.Point(139, 89);
            this.cbIntIntComp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbIntIntComp.Name = "cbIntIntComp";
            this.cbIntIntComp.Size = new System.Drawing.Size(121, 32);
            this.cbIntIntComp.TabIndex = 133;
            // 
            // cbMzByteComp
            // 
            this.cbMzByteComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMzByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMzByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMzByteComp.FormattingEnabled = true;
            this.cbMzByteComp.Location = new System.Drawing.Point(314, 49);
            this.cbMzByteComp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMzByteComp.Name = "cbMzByteComp";
            this.cbMzByteComp.Size = new System.Drawing.Size(121, 32);
            this.cbMzByteComp.TabIndex = 131;
            // 
            // cbConfigStackLayers
            // 
            this.cbConfigStackLayers.Anchor = System.Windows.Forms.AnchorStyles.Left;
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
            this.cbConfigStackLayers.Location = new System.Drawing.Point(175, 154);
            this.cbConfigStackLayers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbConfigStackLayers.Name = "cbConfigStackLayers";
            this.cbConfigStackLayers.Size = new System.Drawing.Size(121, 32);
            this.cbConfigStackLayers.TabIndex = 129;
            // 
            // lblConfigMzIntComp
            // 
            this.lblConfigMzIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigMzIntComp.AutoSize = true;
            this.lblConfigMzIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigMzIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzIntComp.Location = new System.Drawing.Point(38, 53);
            this.lblConfigMzIntComp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigMzIntComp.Name = "lblConfigMzIntComp";
            this.lblConfigMzIntComp.Size = new System.Drawing.Size(44, 24);
            this.lblConfigMzIntComp.TabIndex = 128;
            this.lblConfigMzIntComp.Text = "m/z";
            // 
            // cbMzIntComp
            // 
            this.cbMzIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMzIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMzIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMzIntComp.FormattingEnabled = true;
            this.cbMzIntComp.Location = new System.Drawing.Point(139, 49);
            this.cbMzIntComp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMzIntComp.Name = "cbMzIntComp";
            this.cbMzIntComp.Size = new System.Drawing.Size(121, 32);
            this.cbMzIntComp.TabIndex = 127;
            // 
            // lblConfigOperator
            // 
            this.lblConfigOperator.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigOperator.AutoSize = true;
            this.lblConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOperator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOperator.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOperator.Location = new System.Drawing.Point(41, 7);
            this.lblConfigOperator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigOperator.Name = "lblConfigOperator";
            this.lblConfigOperator.Size = new System.Drawing.Size(89, 24);
            this.lblConfigOperator.TabIndex = 126;
            this.lblConfigOperator.Text = "Operator";
            // 
            // tbConfigOperator
            // 
            this.tbConfigOperator.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigOperator.Location = new System.Drawing.Point(175, 4);
            this.tbConfigOperator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbConfigOperator.Name = "tbConfigOperator";
            this.tbConfigOperator.Size = new System.Drawing.Size(230, 31);
            this.tbConfigOperator.TabIndex = 125;
            // 
            // lblConfigFileNameTag
            // 
            this.lblConfigFileNameTag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigFileNameTag.AutoSize = true;
            this.lblConfigFileNameTag.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigFileNameTag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigFileNameTag.Location = new System.Drawing.Point(39, 46);
            this.lblConfigFileNameTag.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigFileNameTag.Name = "lblConfigFileNameTag";
            this.lblConfigFileNameTag.Size = new System.Drawing.Size(92, 24);
            this.lblConfigFileNameTag.TabIndex = 124;
            this.lblConfigFileNameTag.Text = "File Suffix";
            // 
            // tbConfigFileNameSuffix
            // 
            this.tbConfigFileNameSuffix.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbConfigFileNameSuffix.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFileNameSuffix.Location = new System.Drawing.Point(175, 43);
            this.tbConfigFileNameSuffix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbConfigFileNameSuffix.Name = "tbConfigFileNameSuffix";
            this.tbConfigFileNameSuffix.Size = new System.Drawing.Size(230, 31);
            this.tbConfigFileNameSuffix.TabIndex = 123;
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMzPrecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMzPrecision.Location = new System.Drawing.Point(4, 213);
            this.lblMzPrecision.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMzPrecision.Name = "lblMzPrecision";
            this.lblMzPrecision.Size = new System.Drawing.Size(163, 24);
            this.lblMzPrecision.TabIndex = 121;
            this.lblMzPrecision.Text = "m/z precision(dp)";
            // 
            // cbConfigMzPrecision
            // 
            this.cbConfigMzPrecision.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbConfigMzPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigMzPrecision.FormattingEnabled = true;
            this.cbConfigMzPrecision.Items.AddRange(new object[] {
            "4",
            "5",
            "6"});
            this.cbConfigMzPrecision.Location = new System.Drawing.Point(175, 209);
            this.cbConfigMzPrecision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbConfigMzPrecision.Name = "cbConfigMzPrecision";
            this.cbConfigMzPrecision.Size = new System.Drawing.Size(121, 32);
            this.cbConfigMzPrecision.TabIndex = 120;
            // 
            // cbConfigThreadAccelerate
            // 
            this.cbConfigThreadAccelerate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbConfigThreadAccelerate.AutoSize = true;
            this.cbConfigThreadAccelerate.Checked = true;
            this.cbConfigThreadAccelerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigThreadAccelerate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigThreadAccelerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigThreadAccelerate.Location = new System.Drawing.Point(175, 118);
            this.cbConfigThreadAccelerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbConfigThreadAccelerate.Name = "cbConfigThreadAccelerate";
            this.cbConfigThreadAccelerate.Size = new System.Drawing.Size(167, 28);
            this.cbConfigThreadAccelerate.TabIndex = 119;
            this.cbConfigThreadAccelerate.Text = "Multithreading";
            this.cbConfigThreadAccelerate.UseVisualStyleBackColor = true;
            // 
            // cbConfigIsZeroIntensityIgnore
            // 
            this.cbConfigIsZeroIntensityIgnore.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbConfigIsZeroIntensityIgnore.AutoSize = true;
            this.cbConfigIsZeroIntensityIgnore.Checked = true;
            this.cbConfigIsZeroIntensityIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigIsZeroIntensityIgnore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigIsZeroIntensityIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigIsZeroIntensityIgnore.Location = new System.Drawing.Point(175, 82);
            this.cbConfigIsZeroIntensityIgnore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbConfigIsZeroIntensityIgnore.Name = "cbConfigIsZeroIntensityIgnore";
            this.cbConfigIsZeroIntensityIgnore.Size = new System.Drawing.Size(215, 28);
            this.cbConfigIsZeroIntensityIgnore.TabIndex = 118;
            this.cbConfigIsZeroIntensityIgnore.Text = "Ignore Zero Intensity";
            this.cbConfigIsZeroIntensityIgnore.UseVisualStyleBackColor = true;
            this.cbConfigIsZeroIntensityIgnore.CheckedChanged += new System.EventHandler(this.cbConfigIsZeroIntensityIgnore_CheckedChanged);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApply.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnApply.Location = new System.Drawing.Point(358, 669);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(174, 57);
            this.btnApply.TabIndex = 139;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cbIntByteComp
            // 
            this.cbIntByteComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbIntByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIntByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbIntByteComp.FormattingEnabled = true;
            this.cbIntByteComp.Location = new System.Drawing.Point(314, 89);
            this.cbIntByteComp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbIntByteComp.Name = "cbIntByteComp";
            this.cbIntByteComp.Size = new System.Drawing.Size(121, 32);
            this.cbIntByteComp.TabIndex = 142;
            // 
            // cbMobiByteComp
            // 
            this.cbMobiByteComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMobiByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMobiByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMobiByteComp.FormattingEnabled = true;
            this.cbMobiByteComp.Location = new System.Drawing.Point(314, 139);
            this.cbMobiByteComp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMobiByteComp.Name = "cbMobiByteComp";
            this.cbMobiByteComp.Size = new System.Drawing.Size(121, 32);
            this.cbMobiByteComp.TabIndex = 144;
            // 
            // cbMobiIntComp
            // 
            this.cbMobiIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMobiIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMobiIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMobiIntComp.FormattingEnabled = true;
            this.cbMobiIntComp.Location = new System.Drawing.Point(139, 139);
            this.cbMobiIntComp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbMobiIntComp.Name = "cbMobiIntComp";
            this.cbMobiIntComp.Size = new System.Drawing.Size(121, 32);
            this.cbMobiIntComp.TabIndex = 140;
            // 
            // lblConfigMobiIntComp
            // 
            this.lblConfigMobiIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigMobiIntComp.AutoSize = true;
            this.lblConfigMobiIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigMobiIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMobiIntComp.Location = new System.Drawing.Point(19, 143);
            this.lblConfigMobiIntComp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigMobiIntComp.Name = "lblConfigMobiIntComp";
            this.lblConfigMobiIntComp.Size = new System.Drawing.Size(82, 24);
            this.lblConfigMobiIntComp.TabIndex = 141;
            this.lblConfigMobiIntComp.Text = "mobility";
            // 
            // cbAutoDecision
            // 
            this.cbAutoDecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoDecision.AutoSize = true;
            this.cbAutoDecision.Checked = true;
            this.cbAutoDecision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoDecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbAutoDecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbAutoDecision.Location = new System.Drawing.Point(474, 366);
            this.cbAutoDecision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAutoDecision.Name = "cbAutoDecision";
            this.cbAutoDecision.Size = new System.Drawing.Size(156, 28);
            this.cbAutoDecision.TabIndex = 146;
            this.cbAutoDecision.Text = "Auto Decision";
            this.cbAutoDecision.UseVisualStyleBackColor = true;
            this.cbAutoDecision.CheckedChanged += new System.EventHandler(this.cbAutoDecision_CheckedChanged);
            // 
            // lblIntegerPurpose
            // 
            this.lblIntegerPurpose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblIntegerPurpose.AutoSize = true;
            this.lblIntegerPurpose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIntegerPurpose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIntegerPurpose.Location = new System.Drawing.Point(125, 10);
            this.lblIntegerPurpose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIntegerPurpose.Name = "lblIntegerPurpose";
            this.lblIntegerPurpose.Size = new System.Drawing.Size(150, 24);
            this.lblIntegerPurpose.TabIndex = 147;
            this.lblIntegerPurpose.Text = "Integer-Purpose";
            // 
            // lblGeneralPurpose
            // 
            this.lblGeneralPurpose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblGeneralPurpose.AutoSize = true;
            this.lblGeneralPurpose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGeneralPurpose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblGeneralPurpose.Location = new System.Drawing.Point(298, 10);
            this.lblGeneralPurpose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeneralPurpose.Name = "lblGeneralPurpose";
            this.lblGeneralPurpose.Size = new System.Drawing.Size(154, 24);
            this.lblGeneralPurpose.TabIndex = 148;
            this.lblGeneralPurpose.Text = "General-Purpose";
            // 
            // tableAutoDecision
            // 
            this.tableAutoDecision.ColumnCount = 3;
            this.tableAutoDecision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAutoDecision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAutoDecision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAutoDecision.Controls.Add(this.cbMzIntComp, 1, 1);
            this.tableAutoDecision.Controls.Add(this.label4, 0, 0);
            this.tableAutoDecision.Controls.Add(this.lblIntegerPurpose, 1, 0);
            this.tableAutoDecision.Controls.Add(this.lblGeneralPurpose, 2, 0);
            this.tableAutoDecision.Controls.Add(this.cbMobiByteComp, 2, 3);
            this.tableAutoDecision.Controls.Add(this.cbMzByteComp, 2, 1);
            this.tableAutoDecision.Controls.Add(this.cbIntByteComp, 2, 2);
            this.tableAutoDecision.Controls.Add(this.cbMobiIntComp, 1, 3);
            this.tableAutoDecision.Controls.Add(this.cbIntIntComp, 1, 2);
            this.tableAutoDecision.Controls.Add(this.lblConfigMzIntComp, 0, 1);
            this.tableAutoDecision.Controls.Add(this.lblConfigIntIntComp, 0, 2);
            this.tableAutoDecision.Controls.Add(this.lblConfigMobiIntComp, 0, 3);
            this.tableAutoDecision.Location = new System.Drawing.Point(330, 406);
            this.tableAutoDecision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableAutoDecision.Name = "tableAutoDecision";
            this.tableAutoDecision.RowCount = 4;
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.Size = new System.Drawing.Size(471, 186);
            this.tableAutoDecision.TabIndex = 151;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(4, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 24);
            this.label4.TabIndex = 149;
            this.label4.Text = "Compressor";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblConfigOperator, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbConfigOperator, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblConfigFileNameTag, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbConfigFileNameSuffix, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbConfigStack, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbConfigStackLayers, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbConfigMzPrecision, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblMzPrecision, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.cbConfigThreadAccelerate, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbConfigIsZeroIntensityIgnore, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(332, 87);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(470, 261);
            this.tableLayoutPanel1.TabIndex = 152;
            // 
            // cbAutoExplore
            // 
            this.cbAutoExplore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoExplore.AutoSize = true;
            this.cbAutoExplore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbAutoExplore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbAutoExplore.Location = new System.Drawing.Point(330, 615);
            this.cbAutoExplore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbAutoExplore.Name = "cbAutoExplore";
            this.cbAutoExplore.Size = new System.Drawing.Size(147, 28);
            this.cbAutoExplore.TabIndex = 153;
            this.cbAutoExplore.Text = "Auto Explore";
            this.cbAutoExplore.UseVisualStyleBackColor = true;
            // 
            // ConversionConfigListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 744);
            this.Controls.Add(this.cbAutoExplore);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableAutoDecision);
            this.Controls.Add(this.cbAutoDecision);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnSaveToLocal);
            this.Controls.Add(this.tbNameConfig);
            this.Controls.Add(this.lblNameConfig);
            this.Controls.Add(this.lvConfigList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ConversionConfigListForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conversion Config List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConversionConfigListForm_FormClosed);
            this.Load += new System.EventHandler(this.ConversionConfigListForm_Load);
            this.contextMenu.ResumeLayout(false);
            this.tableAutoDecision.ResumeLayout(false);
            this.tableAutoDecision.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader headerConfigList;
        public System.Windows.Forms.ListView lvConfigList;
        private System.Windows.Forms.Button btnSaveToLocal;
        private System.Windows.Forms.Label lblNameConfig;
        private System.Windows.Forms.Label lblConfigIntIntComp;
        private System.Windows.Forms.Label lblConfigMzIntComp;
        private System.Windows.Forms.Label lblConfigOperator;
        private System.Windows.Forms.Label lblConfigFileNameTag;
        private System.Windows.Forms.Label lblMzPrecision;
        private System.Windows.Forms.Button btnApply;
        public System.Windows.Forms.TextBox tbNameConfig;
        public System.Windows.Forms.TextBox tbConfigFileNameSuffix;
        public System.Windows.Forms.TextBox tbConfigOperator;
        public System.Windows.Forms.CheckBox cbConfigIsZeroIntensityIgnore;
        public System.Windows.Forms.CheckBox cbConfigThreadAccelerate;
        public System.Windows.Forms.ComboBox cbConfigMzPrecision;
        public System.Windows.Forms.ComboBox cbMzIntComp;
        public System.Windows.Forms.ComboBox cbMzByteComp;
        public System.Windows.Forms.ComboBox cbIntIntComp;
        public System.Windows.Forms.CheckBox cbConfigStack;
        public System.Windows.Forms.ComboBox cbConfigStackLayers;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        public System.Windows.Forms.ComboBox cbIntByteComp;
        public System.Windows.Forms.ComboBox cbMobiByteComp;
        public System.Windows.Forms.ComboBox cbMobiIntComp;
        private System.Windows.Forms.Label lblConfigMobiIntComp;
        public System.Windows.Forms.CheckBox cbAutoDecision;
        private System.Windows.Forms.Label lblIntegerPurpose;
        private System.Windows.Forms.Label lblGeneralPurpose;
        private System.Windows.Forms.TableLayoutPanel tableAutoDecision;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public System.Windows.Forms.CheckBox cbAutoExplore;
    }
}