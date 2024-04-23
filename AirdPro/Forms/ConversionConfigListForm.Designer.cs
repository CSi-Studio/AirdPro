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
            this.lblConfigIntIntComp = new System.Windows.Forms.Label();
            this.cbIntIntComp = new System.Windows.Forms.ComboBox();
            this.cbMzByteComp = new System.Windows.Forms.ComboBox();
            this.lblConfigMzIntComp = new System.Windows.Forms.Label();
            this.cbMzIntComp = new System.Windows.Forms.ComboBox();
            this.lblConfigOperator = new System.Windows.Forms.Label();
            this.tbConfigOperator = new System.Windows.Forms.TextBox();
            this.lblConfigFileNameTag = new System.Windows.Forms.Label();
            this.tbConfigFileNameSuffix = new System.Windows.Forms.TextBox();
            this.lblMzPrecision = new System.Windows.Forms.Label();
            this.cbConfigMzPrecision = new System.Windows.Forms.ComboBox();
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
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbRtIntComp = new System.Windows.Forms.ComboBox();
            this.cbRtByteComp = new System.Windows.Forms.ComboBox();
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
            this.lblEngine = new System.Windows.Forms.Label();
            this.cbCompEngine = new System.Windows.Forms.ComboBox();
            this.cbCompressedIndex = new System.Windows.Forms.CheckBox();
            this.cbConfigIsCentroid = new System.Windows.Forms.CheckBox();
            this.lblMaxTasks = new System.Windows.Forms.Label();
            this.numMaxTasks = new System.Windows.Forms.NumericUpDown();
            this.btnGlobalSettingSave = new System.Windows.Forms.Button();
            this.lblIndexFormat = new System.Windows.Forms.Label();
            this.cbIndexFormat = new System.Windows.Forms.ComboBox();
            this.contextMenu.SuspendLayout();
            this.tableAutoDecision.SuspendLayout();
            this.tableDeciderWeight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTasks)).BeginInit();
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
            this.lvConfigList.Location = new System.Drawing.Point(1, 37);
            this.lvConfigList.Name = "lvConfigList";
            this.lvConfigList.ShowGroups = false;
            this.lvConfigList.ShowItemToolTips = true;
            this.lvConfigList.Size = new System.Drawing.Size(332, 477);
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
            this.btnSaveToLocal.Location = new System.Drawing.Point(800, 469);
            this.btnSaveToLocal.Name = "btnSaveToLocal";
            this.btnSaveToLocal.Size = new System.Drawing.Size(113, 38);
            this.btnSaveToLocal.TabIndex = 138;
            this.btnSaveToLocal.Text = "Save";
            this.btnSaveToLocal.UseVisualStyleBackColor = true;
            this.btnSaveToLocal.Click += new System.EventHandler(this.btnSaveToLocal_Click);
            // 
            // tbNameConfig
            // 
            this.tbNameConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbNameConfig.Location = new System.Drawing.Point(424, 36);
            this.tbNameConfig.Name = "tbNameConfig";
            this.tbNameConfig.Size = new System.Drawing.Size(168, 23);
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
            this.lblNameConfig.Location = new System.Drawing.Point(342, 40);
            this.lblNameConfig.Name = "lblNameConfig";
            this.lblNameConfig.Size = new System.Drawing.Size(76, 16);
            this.lblNameConfig.TabIndex = 136;
            this.lblNameConfig.Text = "Config Name";
            // 
            // lblConfigIntIntComp
            // 
            this.lblConfigIntIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigIntIntComp.AutoSize = true;
            this.lblConfigIntIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigIntIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigIntIntComp.Location = new System.Drawing.Point(13, 68);
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
            this.cbIntIntComp.Location = new System.Drawing.Point(95, 64);
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
            this.cbMzByteComp.Location = new System.Drawing.Point(210, 33);
            this.cbMzByteComp.Name = "cbMzByteComp";
            this.cbMzByteComp.Size = new System.Drawing.Size(82, 25);
            this.cbMzByteComp.TabIndex = 131;
            // 
            // lblConfigMzIntComp
            // 
            this.lblConfigMzIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConfigMzIntComp.AutoSize = true;
            this.lblConfigMzIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigMzIntComp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigMzIntComp.Location = new System.Drawing.Point(26, 37);
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
            this.cbMzIntComp.Location = new System.Drawing.Point(95, 33);
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
            this.lblConfigOperator.Location = new System.Drawing.Point(746, 95);
            this.lblConfigOperator.Name = "lblConfigOperator";
            this.lblConfigOperator.Size = new System.Drawing.Size(62, 17);
            this.lblConfigOperator.TabIndex = 126;
            this.lblConfigOperator.Text = "Operator";
            // 
            // tbConfigOperator
            // 
            this.tbConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigOperator.Location = new System.Drawing.Point(814, 92);
            this.tbConfigOperator.Name = "tbConfigOperator";
            this.tbConfigOperator.Size = new System.Drawing.Size(101, 23);
            this.tbConfigOperator.TabIndex = 125;
            // 
            // lblConfigFileNameTag
            // 
            this.lblConfigFileNameTag.AutoSize = true;
            this.lblConfigFileNameTag.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigFileNameTag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigFileNameTag.Location = new System.Drawing.Point(342, 101);
            this.lblConfigFileNameTag.Name = "lblConfigFileNameTag";
            this.lblConfigFileNameTag.Size = new System.Drawing.Size(62, 17);
            this.lblConfigFileNameTag.TabIndex = 124;
            this.lblConfigFileNameTag.Text = "File Suffix";
            // 
            // tbConfigFileNameSuffix
            // 
            this.tbConfigFileNameSuffix.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFileNameSuffix.Location = new System.Drawing.Point(410, 98);
            this.tbConfigFileNameSuffix.Name = "tbConfigFileNameSuffix";
            this.tbConfigFileNameSuffix.Size = new System.Drawing.Size(155, 23);
            this.tbConfigFileNameSuffix.TabIndex = 123;
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMzPrecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMzPrecision.Location = new System.Drawing.Point(345, 71);
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
            this.cbConfigMzPrecision.Location = new System.Drawing.Point(468, 68);
            this.cbConfigMzPrecision.Name = "cbConfigMzPrecision";
            this.cbConfigMzPrecision.Size = new System.Drawing.Size(97, 25);
            this.cbConfigMzPrecision.TabIndex = 120;
            // 
            // cbConfigIsZeroIntensityIgnore
            // 
            this.cbConfigIsZeroIntensityIgnore.AutoSize = true;
            this.cbConfigIsZeroIntensityIgnore.Checked = true;
            this.cbConfigIsZeroIntensityIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigIsZeroIntensityIgnore.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigIsZeroIntensityIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigIsZeroIntensityIgnore.Location = new System.Drawing.Point(647, 70);
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
            this.btnApply.Location = new System.Drawing.Point(678, 469);
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
            this.cbIntByteComp.Location = new System.Drawing.Point(210, 64);
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
            this.cbMobiByteComp.Location = new System.Drawing.Point(210, 95);
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
            this.cbMobiIntComp.Location = new System.Drawing.Point(95, 95);
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
            this.lblConfigMobiIntComp.Location = new System.Drawing.Point(3, 99);
            this.lblConfigMobiIntComp.Name = "lblConfigMobiIntComp";
            this.lblConfigMobiIntComp.Size = new System.Drawing.Size(76, 17);
            this.lblConfigMobiIntComp.TabIndex = 141;
            this.lblConfigMobiIntComp.Text = "ion mobility";
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
            this.cbAutoDecision.Location = new System.Drawing.Point(342, 216);
            this.cbAutoDecision.Name = "cbAutoDecision";
            this.cbAutoDecision.Size = new System.Drawing.Size(463, 21);
            this.cbAutoDecision.TabIndex = 146;
            this.cbAutoDecision.Text = "Auto Decision(More conversion time but better compression performance)";
            this.cbAutoDecision.UseVisualStyleBackColor = true;
            this.cbAutoDecision.CheckedChanged += new System.EventHandler(this.cbAutoDecision_CheckedChanged);
            // 
            // lblIntegerPurpose
            // 
            this.lblIntegerPurpose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblIntegerPurpose.AutoSize = true;
            this.lblIntegerPurpose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIntegerPurpose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIntegerPurpose.Location = new System.Drawing.Point(85, 6);
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
            this.lblGeneralPurpose.Location = new System.Drawing.Point(198, 6);
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
            this.tableAutoDecision.Controls.Add(this.label1, 0, 4);
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
            this.tableAutoDecision.Controls.Add(this.cbRtIntComp, 1, 4);
            this.tableAutoDecision.Controls.Add(this.cbRtByteComp, 2, 4);
            this.tableAutoDecision.Location = new System.Drawing.Point(342, 276);
            this.tableAutoDecision.Name = "tableAutoDecision";
            this.tableAutoDecision.RowCount = 5;
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
            this.tableAutoDecision.Size = new System.Drawing.Size(312, 155);
            this.tableAutoDecision.TabIndex = 151;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(29, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 17);
            this.label1.TabIndex = 150;
            this.label1.Text = "RT";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(18, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 17);
            this.label4.TabIndex = 149;
            this.label4.Text = "Target";
            // 
            // cbRtIntComp
            // 
            this.cbRtIntComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbRtIntComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRtIntComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbRtIntComp.FormattingEnabled = true;
            this.cbRtIntComp.Location = new System.Drawing.Point(95, 126);
            this.cbRtIntComp.Name = "cbRtIntComp";
            this.cbRtIntComp.Size = new System.Drawing.Size(82, 25);
            this.cbRtIntComp.TabIndex = 151;
            // 
            // cbRtByteComp
            // 
            this.cbRtByteComp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbRtByteComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRtByteComp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbRtByteComp.FormattingEnabled = true;
            this.cbRtByteComp.Location = new System.Drawing.Point(210, 126);
            this.cbRtByteComp.Name = "cbRtByteComp";
            this.cbRtByteComp.Size = new System.Drawing.Size(82, 25);
            this.cbRtByteComp.TabIndex = 152;
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
            this.tableDeciderWeight.Location = new System.Drawing.Point(660, 276);
            this.tableDeciderWeight.Name = "tableDeciderWeight";
            this.tableDeciderWeight.RowCount = 4;
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.Size = new System.Drawing.Size(253, 128);
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
            this.cbCSWeight.Location = new System.Drawing.Point(154, 33);
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
            this.lblWeightValue.Location = new System.Drawing.Point(170, 6);
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
            this.cbDTWeight.Location = new System.Drawing.Point(154, 97);
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
            this.cbCTWeight.Location = new System.Drawing.Point(154, 64);
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
            this.lblDecompTime.Location = new System.Drawing.Point(3, 101);
            this.lblDecompTime.Name = "lblDecompTime";
            this.lblDecompTime.Size = new System.Drawing.Size(131, 17);
            this.lblDecompTime.TabIndex = 141;
            this.lblDecompTime.Text = "Decompression Time";
            // 
            // tbSpectraToPredict
            // 
            this.tbSpectraToPredict.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbSpectraToPredict.Location = new System.Drawing.Point(455, 243);
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
            this.lblSelectSpectraCount.Location = new System.Drawing.Point(342, 246);
            this.lblSelectSpectraCount.Name = "lblSelectSpectraCount";
            this.lblSelectSpectraCount.Size = new System.Drawing.Size(107, 17);
            this.lblSelectSpectraCount.TabIndex = 136;
            this.lblSelectSpectraCount.Text = "SpectraToPredict";
            // 
            // lblEngine
            // 
            this.lblEngine.AutoSize = true;
            this.lblEngine.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblEngine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblEngine.Location = new System.Drawing.Point(598, 40);
            this.lblEngine.Name = "lblEngine";
            this.lblEngine.Size = new System.Drawing.Size(128, 17);
            this.lblEngine.TabIndex = 155;
            this.lblEngine.Text = "Compression Engine";
            // 
            // cbCompEngine
            // 
            this.cbCompEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompEngine.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCompEngine.FormattingEnabled = true;
            this.cbCompEngine.Items.AddRange(new object[] {
            "Row Compression",
            "Column Compression"});
            this.cbCompEngine.Location = new System.Drawing.Point(732, 36);
            this.cbCompEngine.Name = "cbCompEngine";
            this.cbCompEngine.Size = new System.Drawing.Size(183, 25);
            this.cbCompEngine.TabIndex = 154;
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
            this.cbCompressedIndex.Location = new System.Drawing.Point(342, 189);
            this.cbCompressedIndex.Name = "cbCompressedIndex";
            this.cbCompressedIndex.Size = new System.Drawing.Size(137, 21);
            this.cbCompressedIndex.TabIndex = 154;
            this.cbCompressedIndex.Text = "Compressed Index";
            this.cbCompressedIndex.UseVisualStyleBackColor = true;
            // 
            // cbConfigIsCentroid
            // 
            this.cbConfigIsCentroid.AutoSize = true;
            this.cbConfigIsCentroid.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigIsCentroid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigIsCentroid.Location = new System.Drawing.Point(835, 70);
            this.cbConfigIsCentroid.Name = "cbConfigIsCentroid";
            this.cbConfigIsCentroid.Size = new System.Drawing.Size(77, 21);
            this.cbConfigIsCentroid.TabIndex = 158;
            this.cbConfigIsCentroid.Text = "Centroid";
            this.cbConfigIsCentroid.UseVisualStyleBackColor = true;
            this.cbConfigIsCentroid.CheckedChanged += new System.EventHandler(this.cbConfigIsCentroid_CheckedChanged);
            // 
            // lblMaxTasks
            // 
            this.lblMaxTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxTasks.AutoSize = true;
            this.lblMaxTasks.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMaxTasks.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMaxTasks.Location = new System.Drawing.Point(1, 9);
            this.lblMaxTasks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxTasks.Name = "lblMaxTasks";
            this.lblMaxTasks.Size = new System.Drawing.Size(180, 17);
            this.lblMaxTasks.TabIndex = 159;
            this.lblMaxTasks.Text = "Max Tasks (Restart Required)";
            this.lblMaxTasks.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numMaxTasks
            // 
            this.numMaxTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.numMaxTasks.Location = new System.Drawing.Point(188, 8);
            this.numMaxTasks.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numMaxTasks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxTasks.Name = "numMaxTasks";
            this.numMaxTasks.Size = new System.Drawing.Size(54, 21);
            this.numMaxTasks.TabIndex = 160;
            this.numMaxTasks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnGlobalSettingSave
            // 
            this.btnGlobalSettingSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGlobalSettingSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGlobalSettingSave.Location = new System.Drawing.Point(248, 6);
            this.btnGlobalSettingSave.Name = "btnGlobalSettingSave";
            this.btnGlobalSettingSave.Size = new System.Drawing.Size(85, 25);
            this.btnGlobalSettingSave.TabIndex = 161;
            this.btnGlobalSettingSave.Text = "Save";
            this.btnGlobalSettingSave.UseVisualStyleBackColor = true;
            this.btnGlobalSettingSave.Click += new System.EventHandler(this.btnGlobalSettingSave_Click);
            // 
            // lblIndexFormat
            // 
            this.lblIndexFormat.AutoSize = true;
            this.lblIndexFormat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIndexFormat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIndexFormat.Location = new System.Drawing.Point(337, 131);
            this.lblIndexFormat.Name = "lblIndexFormat";
            this.lblIndexFormat.Size = new System.Drawing.Size(85, 17);
            this.lblIndexFormat.TabIndex = 163;
            this.lblIndexFormat.Text = "Index Format";
            // 
            // cbIndexFormat
            // 
            this.cbIndexFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIndexFormat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbIndexFormat.FormattingEnabled = true;
            this.cbIndexFormat.Items.AddRange(new object[] {
            "Json",
            "Protobuf",
            "Both"});
            this.cbIndexFormat.Location = new System.Drawing.Point(428, 127);
            this.cbIndexFormat.Name = "cbIndexFormat";
            this.cbIndexFormat.Size = new System.Drawing.Size(137, 25);
            this.cbIndexFormat.TabIndex = 162;
            // 
            // ConversionConfigListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 515);
            this.Controls.Add(this.lblIndexFormat);
            this.Controls.Add(this.cbIndexFormat);
            this.Controls.Add(this.cbCompressedIndex);
            this.Controls.Add(this.cbAutoDecision);
            this.Controls.Add(this.btnGlobalSettingSave);
            this.Controls.Add(this.tableAutoDecision);
            this.Controls.Add(this.lblMaxTasks);
            this.Controls.Add(this.tableDeciderWeight);
            this.Controls.Add(this.numMaxTasks);
            this.Controls.Add(this.tbSpectraToPredict);
            this.Controls.Add(this.cbConfigIsCentroid);
            this.Controls.Add(this.lblSelectSpectraCount);
            this.Controls.Add(this.lblEngine);
            this.Controls.Add(this.cbCompEngine);
            this.Controls.Add(this.tbConfigFileNameSuffix);
            this.Controls.Add(this.lblMzPrecision);
            this.Controls.Add(this.tbConfigOperator);
            this.Controls.Add(this.cbConfigMzPrecision);
            this.Controls.Add(this.lblConfigOperator);
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
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTasks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbRtIntComp;
        public System.Windows.Forms.ComboBox cbRtByteComp;

        private System.Windows.Forms.Button btnGlobalSettingSave;

        private System.Windows.Forms.Label lblMaxTasks;
        private System.Windows.Forms.NumericUpDown numMaxTasks;

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
        public System.Windows.Forms.ComboBox cbConfigMzPrecision;
        public System.Windows.Forms.ComboBox cbMzIntComp;
        public System.Windows.Forms.ComboBox cbMzByteComp;
        public System.Windows.Forms.ComboBox cbIntIntComp;
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
        private System.Windows.Forms.TableLayoutPanel tableDeciderWeight;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblWeightValue;
        private System.Windows.Forms.Label lblCompSize;
        private System.Windows.Forms.Label lblCompTime;
        private System.Windows.Forms.Label lblDecompTime;
        public System.Windows.Forms.ComboBox cbCSWeight;
        public System.Windows.Forms.ComboBox cbDTWeight;
        public System.Windows.Forms.ComboBox cbCTWeight;
        private System.Windows.Forms.Label lblSelectSpectraCount;
        public System.Windows.Forms.TextBox tbSpectraToPredict;
        private System.Windows.Forms.Label lblEngine;
        public System.Windows.Forms.ComboBox cbCompEngine;
        private System.Windows.Forms.ImageList imgsForList;
        private System.Windows.Forms.ColumnHeader mzPrecision;
        private System.Windows.Forms.ColumnHeader headerAuto;
        public System.Windows.Forms.CheckBox cbCompressedIndex;
        public System.Windows.Forms.CheckBox cbConfigIsCentroid;
        private System.Windows.Forms.Label lblIndexFormat;
        public System.Windows.Forms.ComboBox cbIndexFormat;
    }
}