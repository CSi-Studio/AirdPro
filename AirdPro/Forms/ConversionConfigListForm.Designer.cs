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
            this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvConfigList = new System.Windows.Forms.ListView();
            this.mzPrecision = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerAuto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgsForList = new System.Windows.Forms.ImageList(this.components);
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
            this.cbRtIntComp = new System.Windows.Forms.ComboBox();
            this.lblConfigRt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbRtByteComp = new System.Windows.Forms.ComboBox();
            this.cbAutoExplore = new System.Windows.Forms.CheckBox();
            this.tableDeciderWeight = new System.Windows.Forms.TableLayoutPanel();
            this.cbCSWeight = new System.Windows.Forms.ComboBox();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblWeightValue = new System.Windows.Forms.Label();
            this.cbDTWeight = new System.Windows.Forms.ComboBox();
            this.cbCTWeight = new System.Windows.Forms.ComboBox();
            this.lblCompSize = new System.Windows.Forms.Label();
            this.lblCompTime = new System.Windows.Forms.Label();
            this.lblDecompTime = new System.Windows.Forms.Label();
            this.tbSpectraToPredict = new System.Windows.Forms.TextBox();
            this.lblSelectSpectraCount = new System.Windows.Forms.Label();
            this.lblScene = new System.Windows.Forms.Label();
            this.cbScene = new System.Windows.Forms.ComboBox();
            this.tabs = new HZH_Controls.Controls.TabControlExt();
            this.pageComputation = new System.Windows.Forms.TabPage();
            this.pageSearchEngine = new System.Windows.Forms.TabPage();
            this.pageStorage = new System.Windows.Forms.TabPage();
            this.lblSceneConfig = new System.Windows.Forms.Label();
            this.cbCompressedIndex = new System.Windows.Forms.CheckBox();
            this.contextMenu.SuspendLayout();
            this.tableAutoDecision.SuspendLayout();
            this.tableDeciderWeight.SuspendLayout();
            this.tabs.SuspendLayout();
            this.pageComputation.SuspendLayout();
            this.pageStorage.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerName
            // 
            this.headerName.Text = "Config Name";
            this.headerName.Width = 180;
            // 
            // lvConfigList
            // 
            this.lvConfigList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvConfigList.BackColor = System.Drawing.SystemColors.Window;
            this.lvConfigList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerName,
            this.mzPrecision,
            this.headerAuto});
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
            this.lvConfigList.Size = new System.Drawing.Size(332, 645);
            this.lvConfigList.SmallImageList = this.imgsForList;
            this.lvConfigList.TabIndex = 8;
            this.lvConfigList.UseCompatibleStateImageBehavior = false;
            this.lvConfigList.View = System.Windows.Forms.View.Details;
            this.lvConfigList.SelectedIndexChanged += new System.EventHandler(this.lvConfigList_SelectedIndexChanged);
            // 
            // mzPrecision
            // 
            this.mzPrecision.Text = "mz";
            this.mzPrecision.Width = 50;
            // 
            // headerAuto
            // 
            this.headerAuto.Text = "Auto Decision";
            this.headerAuto.Width = 100;
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
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
            // imgsForList
            // 
            this.imgsForList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgsForList.ImageStream")));
            this.imgsForList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgsForList.Images.SetKeyName(0, "Computation.png");
            this.imgsForList.Images.SetKeyName(1, "SearchEngine.png");
            // 
            // btnSaveToLocal
            // 
            this.btnSaveToLocal.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveToLocal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveToLocal.Location = new System.Drawing.Point(982, 599);
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
            this.tbNameConfig.Location = new System.Drawing.Point(424, 5);
            this.tbNameConfig.Name = "tbNameConfig";
            this.tbNameConfig.Size = new System.Drawing.Size(213, 23);
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
            this.lblNameConfig.Location = new System.Drawing.Point(342, 9);
            this.lblNameConfig.Name = "lblNameConfig";
            this.lblNameConfig.Size = new System.Drawing.Size(76, 16);
            this.lblNameConfig.TabIndex = 136;
            this.lblNameConfig.Text = "Config Name";
            // 
            // cbConfigStack
            // 
            this.cbConfigStack.AutoSize = true;
            this.cbConfigStack.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigStack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigStack.Location = new System.Drawing.Point(8, 8);
            this.cbConfigStack.Name = "cbConfigStack";
            this.cbConfigStack.Size = new System.Drawing.Size(93, 21);
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
            this.lblConfigIntIntComp.Location = new System.Drawing.Point(15, 68);
            this.lblConfigIntIntComp.Name = "lblConfigIntIntComp";
            this.lblConfigIntIntComp.Size = new System.Drawing.Size(55, 17);
            this.lblConfigIntIntComp.TabIndex = 134;
            this.lblConfigIntIntComp.Text = "intensity";
            // 
            // cbIntIntComp
            // 
            this.cbIntIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbIntIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIntIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbIntIntComp.FormattingEnabled = true;
            this.cbIntIntComp.Location = new System.Drawing.Point(99, 64);
            this.cbIntIntComp.Name = "cbIntIntComp";
            this.cbIntIntComp.Size = new System.Drawing.Size(82, 25);
            this.cbIntIntComp.TabIndex = 133;
            // 
            // cbMzByteComp
            // 
            this.cbMzByteComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMzByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMzByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMzByteComp.FormattingEnabled = true;
            this.cbMzByteComp.Location = new System.Drawing.Point(229, 33);
            this.cbMzByteComp.Name = "cbMzByteComp";
            this.cbMzByteComp.Size = new System.Drawing.Size(82, 25);
            this.cbMzByteComp.TabIndex = 131;
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
            this.cbConfigStackLayers.Location = new System.Drawing.Point(128, 4);
            this.cbConfigStackLayers.Name = "cbConfigStackLayers";
            this.cbConfigStackLayers.Size = new System.Drawing.Size(82, 25);
            this.cbConfigStackLayers.TabIndex = 129;
            // 
            // lblConfigMzIntComp
            // 
            this.lblConfigMzIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigMzIntComp.AutoSize = true;
            this.lblConfigMzIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigMzIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzIntComp.Location = new System.Drawing.Point(28, 37);
            this.lblConfigMzIntComp.Name = "lblConfigMzIntComp";
            this.lblConfigMzIntComp.Size = new System.Drawing.Size(30, 17);
            this.lblConfigMzIntComp.TabIndex = 128;
            this.lblConfigMzIntComp.Text = "m/z";
            // 
            // cbMzIntComp
            // 
            this.cbMzIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMzIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMzIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMzIntComp.FormattingEnabled = true;
            this.cbMzIntComp.Location = new System.Drawing.Point(99, 33);
            this.cbMzIntComp.Name = "cbMzIntComp";
            this.cbMzIntComp.Size = new System.Drawing.Size(82, 25);
            this.cbMzIntComp.TabIndex = 127;
            // 
            // lblConfigOperator
            // 
            this.lblConfigOperator.AutoSize = true;
            this.lblConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOperator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOperator.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOperator.Location = new System.Drawing.Point(580, 70);
            this.lblConfigOperator.Name = "lblConfigOperator";
            this.lblConfigOperator.Size = new System.Drawing.Size(62, 17);
            this.lblConfigOperator.TabIndex = 126;
            this.lblConfigOperator.Text = "Operator";
            // 
            // tbConfigOperator
            // 
            this.tbConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigOperator.Location = new System.Drawing.Point(648, 67);
            this.tbConfigOperator.Name = "tbConfigOperator";
            this.tbConfigOperator.Size = new System.Drawing.Size(155, 23);
            this.tbConfigOperator.TabIndex = 125;
            // 
            // lblConfigFileNameTag
            // 
            this.lblConfigFileNameTag.AutoSize = true;
            this.lblConfigFileNameTag.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigFileNameTag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigFileNameTag.Location = new System.Drawing.Point(342, 70);
            this.lblConfigFileNameTag.Name = "lblConfigFileNameTag";
            this.lblConfigFileNameTag.Size = new System.Drawing.Size(62, 17);
            this.lblConfigFileNameTag.TabIndex = 124;
            this.lblConfigFileNameTag.Text = "File Suffix";
            // 
            // tbConfigFileNameSuffix
            // 
            this.tbConfigFileNameSuffix.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFileNameSuffix.Location = new System.Drawing.Point(410, 67);
            this.tbConfigFileNameSuffix.Name = "tbConfigFileNameSuffix";
            this.tbConfigFileNameSuffix.Size = new System.Drawing.Size(155, 23);
            this.tbConfigFileNameSuffix.TabIndex = 123;
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMzPrecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMzPrecision.Location = new System.Drawing.Point(345, 40);
            this.lblMzPrecision.Name = "lblMzPrecision";
            this.lblMzPrecision.Size = new System.Drawing.Size(111, 17);
            this.lblMzPrecision.TabIndex = 121;
            this.lblMzPrecision.Text = "m/z precision(dp)";
            // 
            // cbConfigMzPrecision
            // 
            this.cbConfigMzPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigMzPrecision.FormattingEnabled = true;
            this.cbConfigMzPrecision.Items.AddRange(new object[] {
            "3",
            "4",
            "5",
            "6"});
            this.cbConfigMzPrecision.Location = new System.Drawing.Point(468, 37);
            this.cbConfigMzPrecision.Name = "cbConfigMzPrecision";
            this.cbConfigMzPrecision.Size = new System.Drawing.Size(82, 25);
            this.cbConfigMzPrecision.TabIndex = 120;
            // 
            // cbConfigThreadAccelerate
            // 
            this.cbConfigThreadAccelerate.AutoSize = true;
            this.cbConfigThreadAccelerate.Checked = true;
            this.cbConfigThreadAccelerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigThreadAccelerate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigThreadAccelerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigThreadAccelerate.Location = new System.Drawing.Point(561, 40);
            this.cbConfigThreadAccelerate.Name = "cbConfigThreadAccelerate";
            this.cbConfigThreadAccelerate.Size = new System.Drawing.Size(112, 21);
            this.cbConfigThreadAccelerate.TabIndex = 119;
            this.cbConfigThreadAccelerate.Text = "Multithreading";
            this.cbConfigThreadAccelerate.UseVisualStyleBackColor = true;
            // 
            // cbConfigIsZeroIntensityIgnore
            // 
            this.cbConfigIsZeroIntensityIgnore.AutoSize = true;
            this.cbConfigIsZeroIntensityIgnore.Checked = true;
            this.cbConfigIsZeroIntensityIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigIsZeroIntensityIgnore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigIsZeroIntensityIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigIsZeroIntensityIgnore.Location = new System.Drawing.Point(679, 40);
            this.cbConfigIsZeroIntensityIgnore.Name = "cbConfigIsZeroIntensityIgnore";
            this.cbConfigIsZeroIntensityIgnore.Size = new System.Drawing.Size(149, 21);
            this.cbConfigIsZeroIntensityIgnore.TabIndex = 118;
            this.cbConfigIsZeroIntensityIgnore.Text = "Ignore Zero Intensity";
            this.cbConfigIsZeroIntensityIgnore.UseVisualStyleBackColor = true;
            this.cbConfigIsZeroIntensityIgnore.CheckedChanged += new System.EventHandler(this.cbConfigIsZeroIntensityIgnore_CheckedChanged);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApply.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnApply.Location = new System.Drawing.Point(861, 599);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(116, 38);
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
            this.cbIntByteComp.Location = new System.Drawing.Point(229, 64);
            this.cbIntByteComp.Name = "cbIntByteComp";
            this.cbIntByteComp.Size = new System.Drawing.Size(82, 25);
            this.cbIntByteComp.TabIndex = 142;
            // 
            // cbMobiByteComp
            // 
            this.cbMobiByteComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMobiByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMobiByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMobiByteComp.FormattingEnabled = true;
            this.cbMobiByteComp.Location = new System.Drawing.Point(229, 95);
            this.cbMobiByteComp.Name = "cbMobiByteComp";
            this.cbMobiByteComp.Size = new System.Drawing.Size(82, 25);
            this.cbMobiByteComp.TabIndex = 144;
            // 
            // cbMobiIntComp
            // 
            this.cbMobiIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMobiIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMobiIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMobiIntComp.FormattingEnabled = true;
            this.cbMobiIntComp.Location = new System.Drawing.Point(99, 95);
            this.cbMobiIntComp.Name = "cbMobiIntComp";
            this.cbMobiIntComp.Size = new System.Drawing.Size(82, 25);
            this.cbMobiIntComp.TabIndex = 140;
            // 
            // lblConfigMobiIntComp
            // 
            this.lblConfigMobiIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigMobiIntComp.AutoSize = true;
            this.lblConfigMobiIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigMobiIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMobiIntComp.Location = new System.Drawing.Point(16, 99);
            this.lblConfigMobiIntComp.Name = "lblConfigMobiIntComp";
            this.lblConfigMobiIntComp.Size = new System.Drawing.Size(54, 17);
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
            this.cbAutoDecision.Location = new System.Drawing.Point(12, 40);
            this.cbAutoDecision.Name = "cbAutoDecision";
            this.cbAutoDecision.Size = new System.Drawing.Size(107, 21);
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
            this.lblIntegerPurpose.Location = new System.Drawing.Point(89, 6);
            this.lblIntegerPurpose.Name = "lblIntegerPurpose";
            this.lblIntegerPurpose.Size = new System.Drawing.Size(103, 17);
            this.lblIntegerPurpose.TabIndex = 147;
            this.lblIntegerPurpose.Text = "Integer-Purpose";
            // 
            // lblGeneralPurpose
            // 
            this.lblGeneralPurpose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblGeneralPurpose.AutoSize = true;
            this.lblGeneralPurpose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblGeneralPurpose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblGeneralPurpose.Location = new System.Drawing.Point(217, 6);
            this.lblGeneralPurpose.Name = "lblGeneralPurpose";
            this.lblGeneralPurpose.Size = new System.Drawing.Size(106, 17);
            this.lblGeneralPurpose.TabIndex = 148;
            this.lblGeneralPurpose.Text = "General-Purpose";
            // 
            // tableAutoDecision
            // 
            this.tableAutoDecision.ColumnCount = 3;
            this.tableAutoDecision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAutoDecision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAutoDecision.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableAutoDecision.Controls.Add(this.cbRtIntComp, 0, 4);
            this.tableAutoDecision.Controls.Add(this.lblConfigRt, 0, 4);
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
            this.tableAutoDecision.Controls.Add(this.cbRtByteComp, 2, 4);
            this.tableAutoDecision.Location = new System.Drawing.Point(12, 67);
            this.tableAutoDecision.Name = "tableAutoDecision";
            this.tableAutoDecision.RowCount = 5;
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableAutoDecision.Size = new System.Drawing.Size(345, 160);
            this.tableAutoDecision.TabIndex = 151;
            // 
            // cbRtIntComp
            // 
            this.cbRtIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbRtIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRtIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbRtIntComp.FormattingEnabled = true;
            this.cbRtIntComp.Location = new System.Drawing.Point(99, 129);
            this.cbRtIntComp.Name = "cbRtIntComp";
            this.cbRtIntComp.Size = new System.Drawing.Size(82, 25);
            this.cbRtIntComp.TabIndex = 153;
            // 
            // lblConfigRt
            // 
            this.lblConfigRt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigRt.AutoSize = true;
            this.lblConfigRt.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigRt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigRt.Location = new System.Drawing.Point(31, 133);
            this.lblConfigRt.Name = "lblConfigRt";
            this.lblConfigRt.Size = new System.Drawing.Size(23, 17);
            this.lblConfigRt.TabIndex = 150;
            this.lblConfigRt.Text = "RT";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 17);
            this.label4.TabIndex = 149;
            this.label4.Text = "Compressor";
            // 
            // cbRtByteComp
            // 
            this.cbRtByteComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbRtByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRtByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbRtByteComp.FormattingEnabled = true;
            this.cbRtByteComp.Location = new System.Drawing.Point(229, 129);
            this.cbRtByteComp.Name = "cbRtByteComp";
            this.cbRtByteComp.Size = new System.Drawing.Size(82, 25);
            this.cbRtByteComp.TabIndex = 152;
            // 
            // cbAutoExplore
            // 
            this.cbAutoExplore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoExplore.AutoSize = true;
            this.cbAutoExplore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbAutoExplore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbAutoExplore.Location = new System.Drawing.Point(12, 238);
            this.cbAutoExplore.Name = "cbAutoExplore";
            this.cbAutoExplore.Size = new System.Drawing.Size(102, 21);
            this.cbAutoExplore.TabIndex = 153;
            this.cbAutoExplore.Text = "Auto Explore";
            this.cbAutoExplore.UseVisualStyleBackColor = true;
            // 
            // tableDeciderWeight
            // 
            this.tableDeciderWeight.ColumnCount = 2;
            this.tableDeciderWeight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableDeciderWeight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableDeciderWeight.Controls.Add(this.cbCSWeight, 1, 1);
            this.tableDeciderWeight.Controls.Add(this.lblWeight, 0, 0);
            this.tableDeciderWeight.Controls.Add(this.lblWeightValue, 1, 0);
            this.tableDeciderWeight.Controls.Add(this.cbDTWeight, 1, 3);
            this.tableDeciderWeight.Controls.Add(this.cbCTWeight, 1, 2);
            this.tableDeciderWeight.Controls.Add(this.lblCompSize, 0, 1);
            this.tableDeciderWeight.Controls.Add(this.lblCompTime, 0, 2);
            this.tableDeciderWeight.Controls.Add(this.lblDecompTime, 0, 3);
            this.tableDeciderWeight.Location = new System.Drawing.Point(378, 67);
            this.tableDeciderWeight.Name = "tableDeciderWeight";
            this.tableDeciderWeight.RowCount = 4;
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.Size = new System.Drawing.Size(345, 125);
            this.tableDeciderWeight.TabIndex = 152;
            // 
            // cbCSWeight
            // 
            this.cbCSWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbCSWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCSWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCSWeight.FormattingEnabled = true;
            this.cbCSWeight.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbCSWeight.Location = new System.Drawing.Point(200, 33);
            this.cbCSWeight.Name = "cbCSWeight";
            this.cbCSWeight.Size = new System.Drawing.Size(82, 25);
            this.cbCSWeight.TabIndex = 127;
            // 
            // lblWeight
            // 
            this.lblWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWeight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWeight.Location = new System.Drawing.Point(28, 6);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(80, 17);
            this.lblWeight.TabIndex = 149;
            this.lblWeight.Text = "Compressor";
            // 
            // lblWeightValue
            // 
            this.lblWeightValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblWeightValue.AutoSize = true;
            this.lblWeightValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWeightValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWeightValue.Location = new System.Drawing.Point(216, 6);
            this.lblWeightValue.Name = "lblWeightValue";
            this.lblWeightValue.Size = new System.Drawing.Size(49, 17);
            this.lblWeightValue.TabIndex = 147;
            this.lblWeightValue.Text = "Weight";
            // 
            // cbDTWeight
            // 
            this.cbDTWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbDTWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDTWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbDTWeight.FormattingEnabled = true;
            this.cbDTWeight.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbDTWeight.Location = new System.Drawing.Point(200, 96);
            this.cbDTWeight.Name = "cbDTWeight";
            this.cbDTWeight.Size = new System.Drawing.Size(82, 25);
            this.cbDTWeight.TabIndex = 140;
            // 
            // cbCTWeight
            // 
            this.cbCTWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbCTWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCTWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCTWeight.FormattingEnabled = true;
            this.cbCTWeight.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbCTWeight.Location = new System.Drawing.Point(200, 64);
            this.cbCTWeight.Name = "cbCTWeight";
            this.cbCTWeight.Size = new System.Drawing.Size(82, 25);
            this.cbCTWeight.TabIndex = 133;
            // 
            // lblCompSize
            // 
            this.lblCompSize.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCompSize.AutoSize = true;
            this.lblCompSize.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCompSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCompSize.Location = new System.Drawing.Point(12, 37);
            this.lblCompSize.Name = "lblCompSize";
            this.lblCompSize.Size = new System.Drawing.Size(112, 17);
            this.lblCompSize.TabIndex = 128;
            this.lblCompSize.Text = "Compression Size";
            // 
            // lblCompTime
            // 
            this.lblCompTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCompTime.AutoSize = true;
            this.lblCompTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCompTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCompTime.Location = new System.Drawing.Point(10, 68);
            this.lblCompTime.Name = "lblCompTime";
            this.lblCompTime.Size = new System.Drawing.Size(117, 17);
            this.lblCompTime.TabIndex = 134;
            this.lblCompTime.Text = "Compression Time";
            // 
            // lblDecompTime
            // 
            this.lblDecompTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDecompTime.AutoSize = true;
            this.lblDecompTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDecompTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDecompTime.Location = new System.Drawing.Point(3, 100);
            this.lblDecompTime.Name = "lblDecompTime";
            this.lblDecompTime.Size = new System.Drawing.Size(131, 17);
            this.lblDecompTime.TabIndex = 141;
            this.lblDecompTime.Text = "Decompression Time";
            // 
            // tbSpectraToPredict
            // 
            this.tbSpectraToPredict.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbSpectraToPredict.Location = new System.Drawing.Point(122, 8);
            this.tbSpectraToPredict.Name = "tbSpectraToPredict";
            this.tbSpectraToPredict.Size = new System.Drawing.Size(155, 23);
            this.tbSpectraToPredict.TabIndex = 137;
            this.tbSpectraToPredict.Text = "50";
            // 
            // lblSelectSpectraCount
            // 
            this.lblSelectSpectraCount.AutoSize = true;
            this.lblSelectSpectraCount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectSpectraCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSelectSpectraCount.Location = new System.Drawing.Point(9, 11);
            this.lblSelectSpectraCount.Name = "lblSelectSpectraCount";
            this.lblSelectSpectraCount.Size = new System.Drawing.Size(107, 17);
            this.lblSelectSpectraCount.TabIndex = 136;
            this.lblSelectSpectraCount.Text = "SpectraToPredict";
            // 
            // lblScene
            // 
            this.lblScene.AutoSize = true;
            this.lblScene.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScene.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblScene.Location = new System.Drawing.Point(645, 8);
            this.lblScene.Name = "lblScene";
            this.lblScene.Size = new System.Drawing.Size(42, 17);
            this.lblScene.TabIndex = 155;
            this.lblScene.Text = "Scene";
            // 
            // cbScene
            // 
            this.cbScene.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScene.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbScene.FormattingEnabled = true;
            this.cbScene.Items.AddRange(new object[] {
            "Computation",
            "SearchEngine"});
            this.cbScene.Location = new System.Drawing.Point(693, 5);
            this.cbScene.Name = "cbScene";
            this.cbScene.Size = new System.Drawing.Size(125, 25);
            this.cbScene.TabIndex = 154;
            // 
            // tabs
            // 
            this.tabs.CloseBtnColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(85)))), ((int)(((byte)(51)))));
            this.tabs.Controls.Add(this.pageComputation);
            this.tabs.Controls.Add(this.pageSearchEngine);
            this.tabs.Controls.Add(this.pageStorage);
            this.tabs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabs.HeadSelectedBackColor = System.Drawing.Color.Blue;
            this.tabs.IsShowCloseBtn = false;
            this.tabs.ItemSize = new System.Drawing.Size(0, 50);
            this.tabs.Location = new System.Drawing.Point(345, 189);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(756, 404);
            this.tabs.TabIndex = 156;
            this.tabs.UncloseTabIndexs = null;
            // 
            // pageComputation
            // 
            this.pageComputation.Controls.Add(this.cbCompressedIndex);
            this.pageComputation.Controls.Add(this.cbAutoDecision);
            this.pageComputation.Controls.Add(this.tableAutoDecision);
            this.pageComputation.Controls.Add(this.cbAutoExplore);
            this.pageComputation.Controls.Add(this.tableDeciderWeight);
            this.pageComputation.Controls.Add(this.tbSpectraToPredict);
            this.pageComputation.Controls.Add(this.lblSelectSpectraCount);
            this.pageComputation.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageComputation.Location = new System.Drawing.Point(4, 54);
            this.pageComputation.Name = "pageComputation";
            this.pageComputation.Padding = new System.Windows.Forms.Padding(3);
            this.pageComputation.Size = new System.Drawing.Size(748, 346);
            this.pageComputation.TabIndex = 0;
            this.pageComputation.Text = "Computation";
            this.pageComputation.UseVisualStyleBackColor = true;
            // 
            // pageSearchEngine
            // 
            this.pageSearchEngine.Location = new System.Drawing.Point(4, 54);
            this.pageSearchEngine.Name = "pageSearchEngine";
            this.pageSearchEngine.Padding = new System.Windows.Forms.Padding(3);
            this.pageSearchEngine.Size = new System.Drawing.Size(475, 401);
            this.pageSearchEngine.TabIndex = 1;
            this.pageSearchEngine.Text = "SearchEngine";
            this.pageSearchEngine.UseVisualStyleBackColor = true;
            // 
            // pageStorage
            // 
            this.pageStorage.Controls.Add(this.cbConfigStackLayers);
            this.pageStorage.Controls.Add(this.cbConfigStack);
            this.pageStorage.Location = new System.Drawing.Point(4, 54);
            this.pageStorage.Name = "pageStorage";
            this.pageStorage.Size = new System.Drawing.Size(475, 401);
            this.pageStorage.TabIndex = 2;
            this.pageStorage.Text = "Storage";
            this.pageStorage.UseVisualStyleBackColor = true;
            // 
            // lblSceneConfig
            // 
            this.lblSceneConfig.AutoSize = true;
            this.lblSceneConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblSceneConfig.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSceneConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSceneConfig.Location = new System.Drawing.Point(345, 169);
            this.lblSceneConfig.Name = "lblSceneConfig";
            this.lblSceneConfig.Size = new System.Drawing.Size(206, 17);
            this.lblSceneConfig.TabIndex = 157;
            this.lblSceneConfig.Text = "Configuration for Different Scenes";
            // 
            // cbCompressedIndex
            // 
            this.cbCompressedIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCompressedIndex.AutoSize = true;
            this.cbCompressedIndex.Checked = true;
            this.cbCompressedIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCompressedIndex.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCompressedIndex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbCompressedIndex.Location = new System.Drawing.Point(12, 265);
            this.cbCompressedIndex.Name = "cbCompressedIndex";
            this.cbCompressedIndex.Size = new System.Drawing.Size(137, 21);
            this.cbCompressedIndex.TabIndex = 154;
            this.cbCompressedIndex.Text = "Compressed Index";
            this.cbCompressedIndex.UseVisualStyleBackColor = true;
            // 
            // ConversionConfigListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 649);
            this.Controls.Add(this.lblSceneConfig);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.lblScene);
            this.Controls.Add(this.cbScene);
            this.Controls.Add(this.tbConfigFileNameSuffix);
            this.Controls.Add(this.lblMzPrecision);
            this.Controls.Add(this.tbConfigOperator);
            this.Controls.Add(this.cbConfigMzPrecision);
            this.Controls.Add(this.lblConfigOperator);
            this.Controls.Add(this.cbConfigThreadAccelerate);
            this.Controls.Add(this.cbConfigIsZeroIntensityIgnore);
            this.Controls.Add(this.lblConfigFileNameTag);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnSaveToLocal);
            this.Controls.Add(this.tbNameConfig);
            this.Controls.Add(this.lblNameConfig);
            this.Controls.Add(this.lvConfigList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConversionConfigListForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conversion Config List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConversionConfigListForm_FormClosed);
            this.Load += new System.EventHandler(this.ConversionConfigListForm_Load);
            this.contextMenu.ResumeLayout(false);
            this.tableAutoDecision.ResumeLayout(false);
            this.tableAutoDecision.PerformLayout();
            this.tableDeciderWeight.ResumeLayout(false);
            this.tableDeciderWeight.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.pageComputation.ResumeLayout(false);
            this.pageComputation.PerformLayout();
            this.pageStorage.ResumeLayout(false);
            this.pageStorage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColumnHeader headerName;
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
        public System.Windows.Forms.CheckBox cbAutoExplore;
        private System.Windows.Forms.TableLayoutPanel tableDeciderWeight;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblWeightValue;
        private System.Windows.Forms.Label lblCompSize;
        private System.Windows.Forms.Label lblCompTime;
        private System.Windows.Forms.Label lblDecompTime;
        public System.Windows.Forms.ComboBox cbCSWeight;
        public System.Windows.Forms.ComboBox cbDTWeight;
        public System.Windows.Forms.ComboBox cbCTWeight;
        public System.Windows.Forms.ComboBox cbRtIntComp;
        private System.Windows.Forms.Label lblConfigRt;
        public System.Windows.Forms.ComboBox cbRtByteComp;
        private System.Windows.Forms.Label lblSelectSpectraCount;
        public System.Windows.Forms.TextBox tbSpectraToPredict;
        private System.Windows.Forms.Label lblScene;
        public System.Windows.Forms.ComboBox cbScene;
        private System.Windows.Forms.ImageList imgsForList;
        private HZH_Controls.Controls.TabControlExt tabs;
        private System.Windows.Forms.TabPage pageComputation;
        private System.Windows.Forms.TabPage pageSearchEngine;
        private System.Windows.Forms.TabPage pageStorage;
        private System.Windows.Forms.ColumnHeader mzPrecision;
        private System.Windows.Forms.ColumnHeader headerAuto;
        private System.Windows.Forms.Label lblSceneConfig;
        public System.Windows.Forms.CheckBox cbCompressedIndex;
    }
}