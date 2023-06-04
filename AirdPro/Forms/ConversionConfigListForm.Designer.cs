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
            this.headerName = new System.Windows.Forms.ColumnHeader();
            this.lvConfigList = new System.Windows.Forms.ListView();
            this.mzPrecision = new System.Windows.Forms.ColumnHeader();
            this.headerAuto = new System.Windows.Forms.ColumnHeader();
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
            this.label4 = new System.Windows.Forms.Label();
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
            this.cbCompressedIndex = new System.Windows.Forms.CheckBox();
            this.pageSearchEngine = new System.Windows.Forms.TabPage();
            this.cbFastReadMode = new System.Windows.Forms.CheckBox();
            this.pageStorage = new System.Windows.Forms.TabPage();
            this.lblSceneConfig = new System.Windows.Forms.Label();
            this.cbConfigIsCentroid = new System.Windows.Forms.CheckBox();
            this.contextMenu.SuspendLayout();
            this.tableAutoDecision.SuspendLayout();
            this.tableDeciderWeight.SuspendLayout();
            this.tabs.SuspendLayout();
            this.pageComputation.SuspendLayout();
            this.pageSearchEngine.SuspendLayout();
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
            this.lvConfigList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.lvConfigList.BackColor = System.Drawing.SystemColors.Window;
            this.lvConfigList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.headerName, this.mzPrecision, this.headerAuto });
            this.lvConfigList.ContextMenuStrip = this.contextMenu;
            this.lvConfigList.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lvConfigList.FullRowSelect = true;
            this.lvConfigList.GridLines = true;
            this.lvConfigList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvConfigList.HideSelection = false;
            this.lvConfigList.Location = new System.Drawing.Point(2, 4);
            this.lvConfigList.Margin = new System.Windows.Forms.Padding(4);
            this.lvConfigList.Name = "lvConfigList";
            this.lvConfigList.ShowGroups = false;
            this.lvConfigList.ShowItemToolTips = true;
            this.lvConfigList.Size = new System.Drawing.Size(496, 892);
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
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.deleteToolStripMenuItem });
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(137, 32);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(136, 28);
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
            this.btnSaveToLocal.Location = new System.Drawing.Point(1254, 830);
            this.btnSaveToLocal.Margin = new System.Windows.Forms.Padding(4);
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
            this.tbNameConfig.Location = new System.Drawing.Point(636, 8);
            this.tbNameConfig.Margin = new System.Windows.Forms.Padding(4);
            this.tbNameConfig.Name = "tbNameConfig";
            this.tbNameConfig.Size = new System.Drawing.Size(318, 31);
            this.tbNameConfig.TabIndex = 137;
            // 
            // lblNameConfig
            // 
            this.lblNameConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNameConfig.AutoSize = true;
            this.lblNameConfig.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblNameConfig.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNameConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNameConfig.Location = new System.Drawing.Point(513, 14);
            this.lblNameConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNameConfig.Name = "lblNameConfig";
            this.lblNameConfig.Size = new System.Drawing.Size(112, 21);
            this.lblNameConfig.TabIndex = 136;
            this.lblNameConfig.Text = "Config Name";
            // 
            // cbConfigStack
            // 
            this.cbConfigStack.AutoSize = true;
            this.cbConfigStack.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigStack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigStack.Location = new System.Drawing.Point(12, 12);
            this.cbConfigStack.Margin = new System.Windows.Forms.Padding(4);
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
            this.lblConfigIntIntComp.Location = new System.Drawing.Point(4, 93);
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
            this.cbIntIntComp.Location = new System.Drawing.Point(110, 89);
            this.cbIntIntComp.Margin = new System.Windows.Forms.Padding(4);
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
            this.cbMzByteComp.Location = new System.Drawing.Point(286, 49);
            this.cbMzByteComp.Margin = new System.Windows.Forms.Padding(4);
            this.cbMzByteComp.Name = "cbMzByteComp";
            this.cbMzByteComp.Size = new System.Drawing.Size(121, 32);
            this.cbMzByteComp.TabIndex = 131;
            // 
            // cbConfigStackLayers
            // 
            this.cbConfigStackLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigStackLayers.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigStackLayers.FormattingEnabled = true;
            this.cbConfigStackLayers.Items.AddRange(new object[] { "32", "64", "128", "256", "512", "1024" });
            this.cbConfigStackLayers.Location = new System.Drawing.Point(192, 6);
            this.cbConfigStackLayers.Margin = new System.Windows.Forms.Padding(4);
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
            this.lblConfigMzIntComp.Location = new System.Drawing.Point(24, 53);
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
            this.cbMzIntComp.Location = new System.Drawing.Point(110, 49);
            this.cbMzIntComp.Margin = new System.Windows.Forms.Padding(4);
            this.cbMzIntComp.Name = "cbMzIntComp";
            this.cbMzIntComp.Size = new System.Drawing.Size(121, 32);
            this.cbMzIntComp.TabIndex = 127;
            // 
            // lblConfigOperator
            // 
            this.lblConfigOperator.AutoSize = true;
            this.lblConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOperator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOperator.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOperator.Location = new System.Drawing.Point(870, 105);
            this.lblConfigOperator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigOperator.Name = "lblConfigOperator";
            this.lblConfigOperator.Size = new System.Drawing.Size(89, 24);
            this.lblConfigOperator.TabIndex = 126;
            this.lblConfigOperator.Text = "Operator";
            // 
            // tbConfigOperator
            // 
            this.tbConfigOperator.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigOperator.Location = new System.Drawing.Point(972, 100);
            this.tbConfigOperator.Margin = new System.Windows.Forms.Padding(4);
            this.tbConfigOperator.Name = "tbConfigOperator";
            this.tbConfigOperator.Size = new System.Drawing.Size(230, 31);
            this.tbConfigOperator.TabIndex = 125;
            // 
            // lblConfigFileNameTag
            // 
            this.lblConfigFileNameTag.AutoSize = true;
            this.lblConfigFileNameTag.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigFileNameTag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigFileNameTag.Location = new System.Drawing.Point(513, 105);
            this.lblConfigFileNameTag.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigFileNameTag.Name = "lblConfigFileNameTag";
            this.lblConfigFileNameTag.Size = new System.Drawing.Size(92, 24);
            this.lblConfigFileNameTag.TabIndex = 124;
            this.lblConfigFileNameTag.Text = "File Suffix";
            // 
            // tbConfigFileNameSuffix
            // 
            this.tbConfigFileNameSuffix.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbConfigFileNameSuffix.Location = new System.Drawing.Point(615, 100);
            this.tbConfigFileNameSuffix.Margin = new System.Windows.Forms.Padding(4);
            this.tbConfigFileNameSuffix.Name = "tbConfigFileNameSuffix";
            this.tbConfigFileNameSuffix.Size = new System.Drawing.Size(230, 31);
            this.tbConfigFileNameSuffix.TabIndex = 123;
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMzPrecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMzPrecision.Location = new System.Drawing.Point(518, 60);
            this.lblMzPrecision.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMzPrecision.Name = "lblMzPrecision";
            this.lblMzPrecision.Size = new System.Drawing.Size(163, 24);
            this.lblMzPrecision.TabIndex = 121;
            this.lblMzPrecision.Text = "m/z precision(dp)";
            // 
            // cbConfigMzPrecision
            // 
            this.cbConfigMzPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConfigMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigMzPrecision.FormattingEnabled = true;
            this.cbConfigMzPrecision.Items.AddRange(new object[] { "3", "4", "5", "6" });
            this.cbConfigMzPrecision.Location = new System.Drawing.Point(702, 56);
            this.cbConfigMzPrecision.Margin = new System.Windows.Forms.Padding(4);
            this.cbConfigMzPrecision.Name = "cbConfigMzPrecision";
            this.cbConfigMzPrecision.Size = new System.Drawing.Size(121, 32);
            this.cbConfigMzPrecision.TabIndex = 120;
            // 
            // cbConfigThreadAccelerate
            // 
            this.cbConfigThreadAccelerate.AutoSize = true;
            this.cbConfigThreadAccelerate.Checked = true;
            this.cbConfigThreadAccelerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbConfigThreadAccelerate.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigThreadAccelerate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigThreadAccelerate.Location = new System.Drawing.Point(842, 60);
            this.cbConfigThreadAccelerate.Margin = new System.Windows.Forms.Padding(4);
            this.cbConfigThreadAccelerate.Name = "cbConfigThreadAccelerate";
            this.cbConfigThreadAccelerate.Size = new System.Drawing.Size(167, 28);
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
            this.cbConfigIsZeroIntensityIgnore.Location = new System.Drawing.Point(1018, 60);
            this.cbConfigIsZeroIntensityIgnore.Margin = new System.Windows.Forms.Padding(4);
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
            this.btnApply.Location = new System.Drawing.Point(1072, 830);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4);
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
            this.cbIntByteComp.Location = new System.Drawing.Point(286, 89);
            this.cbIntByteComp.Margin = new System.Windows.Forms.Padding(4);
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
            this.cbMobiByteComp.Location = new System.Drawing.Point(286, 132);
            this.cbMobiByteComp.Margin = new System.Windows.Forms.Padding(4);
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
            this.cbMobiIntComp.Location = new System.Drawing.Point(110, 132);
            this.cbMobiIntComp.Margin = new System.Windows.Forms.Padding(4);
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
            this.lblConfigMobiIntComp.Location = new System.Drawing.Point(5, 136);
            this.lblConfigMobiIntComp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigMobiIntComp.Name = "lblConfigMobiIntComp";
            this.lblConfigMobiIntComp.Size = new System.Drawing.Size(82, 24);
            this.lblConfigMobiIntComp.TabIndex = 141;
            this.lblConfigMobiIntComp.Text = "mobility";
            // 
            // cbAutoDecision
            // 
            this.cbAutoDecision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAutoDecision.AutoSize = true;
            this.cbAutoDecision.Checked = true;
            this.cbAutoDecision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoDecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbAutoDecision.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbAutoDecision.Location = new System.Drawing.Point(18, 110);
            this.cbAutoDecision.Margin = new System.Windows.Forms.Padding(4);
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
            this.lblIntegerPurpose.Location = new System.Drawing.Point(96, 10);
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
            this.lblGeneralPurpose.Location = new System.Drawing.Point(270, 10);
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
            this.tableAutoDecision.Location = new System.Drawing.Point(18, 150);
            this.tableAutoDecision.Margin = new System.Windows.Forms.Padding(4);
            this.tableAutoDecision.Name = "tableAutoDecision";
            this.tableAutoDecision.RowCount = 4;
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableAutoDecision.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableAutoDecision.Size = new System.Drawing.Size(444, 172);
            this.tableAutoDecision.TabIndex = 151;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(13, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 24);
            this.label4.TabIndex = 149;
            this.label4.Text = "Target";
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
            this.tableDeciderWeight.Location = new System.Drawing.Point(495, 150);
            this.tableDeciderWeight.Margin = new System.Windows.Forms.Padding(4);
            this.tableDeciderWeight.Name = "tableDeciderWeight";
            this.tableDeciderWeight.RowCount = 4;
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableDeciderWeight.Size = new System.Drawing.Size(380, 172);
            this.tableDeciderWeight.TabIndex = 152;
            // 
            // cbCSWeight
            // 
            this.cbCSWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbCSWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCSWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCSWeight.FormattingEnabled = true;
            this.cbCSWeight.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
            this.cbCSWeight.Location = new System.Drawing.Point(228, 49);
            this.cbCSWeight.Margin = new System.Windows.Forms.Padding(4);
            this.cbCSWeight.Name = "cbCSWeight";
            this.cbCSWeight.Size = new System.Drawing.Size(121, 32);
            this.cbCSWeight.TabIndex = 127;
            // 
            // lblWeight
            // 
            this.lblWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWeight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWeight.Location = new System.Drawing.Point(42, 10);
            this.lblWeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(113, 24);
            this.lblWeight.TabIndex = 149;
            this.lblWeight.Text = "Compressor";
            // 
            // lblWeightValue
            // 
            this.lblWeightValue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblWeightValue.AutoSize = true;
            this.lblWeightValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWeightValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblWeightValue.Location = new System.Drawing.Point(252, 10);
            this.lblWeightValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWeightValue.Name = "lblWeightValue";
            this.lblWeightValue.Size = new System.Drawing.Size(73, 24);
            this.lblWeightValue.TabIndex = 147;
            this.lblWeightValue.Text = "Weight";
            // 
            // cbDTWeight
            // 
            this.cbDTWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbDTWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDTWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbDTWeight.FormattingEnabled = true;
            this.cbDTWeight.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
            this.cbDTWeight.Location = new System.Drawing.Point(228, 132);
            this.cbDTWeight.Margin = new System.Windows.Forms.Padding(4);
            this.cbDTWeight.Name = "cbDTWeight";
            this.cbDTWeight.Size = new System.Drawing.Size(121, 32);
            this.cbDTWeight.TabIndex = 140;
            // 
            // cbCTWeight
            // 
            this.cbCTWeight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbCTWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCTWeight.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCTWeight.FormattingEnabled = true;
            this.cbCTWeight.Items.AddRange(new object[] { "1", "2", "3", "4", "5" });
            this.cbCTWeight.Location = new System.Drawing.Point(228, 89);
            this.cbCTWeight.Margin = new System.Windows.Forms.Padding(4);
            this.cbCTWeight.Name = "cbCTWeight";
            this.cbCTWeight.Size = new System.Drawing.Size(121, 32);
            this.cbCTWeight.TabIndex = 133;
            // 
            // lblCompSize
            // 
            this.lblCompSize.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCompSize.AutoSize = true;
            this.lblCompSize.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCompSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCompSize.Location = new System.Drawing.Point(18, 53);
            this.lblCompSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompSize.Name = "lblCompSize";
            this.lblCompSize.Size = new System.Drawing.Size(161, 24);
            this.lblCompSize.TabIndex = 128;
            this.lblCompSize.Text = "Compression Size";
            // 
            // lblCompTime
            // 
            this.lblCompTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCompTime.AutoSize = true;
            this.lblCompTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCompTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCompTime.Location = new System.Drawing.Point(14, 93);
            this.lblCompTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCompTime.Name = "lblCompTime";
            this.lblCompTime.Size = new System.Drawing.Size(169, 24);
            this.lblCompTime.TabIndex = 134;
            this.lblCompTime.Text = "Compression Time";
            // 
            // lblDecompTime
            // 
            this.lblDecompTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDecompTime.AutoSize = true;
            this.lblDecompTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDecompTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDecompTime.Location = new System.Drawing.Point(4, 136);
            this.lblDecompTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDecompTime.Name = "lblDecompTime";
            this.lblDecompTime.Size = new System.Drawing.Size(190, 24);
            this.lblDecompTime.TabIndex = 141;
            this.lblDecompTime.Text = "Decompression Time";
            // 
            // tbSpectraToPredict
            // 
            this.tbSpectraToPredict.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbSpectraToPredict.Location = new System.Drawing.Point(183, 12);
            this.tbSpectraToPredict.Margin = new System.Windows.Forms.Padding(4);
            this.tbSpectraToPredict.Name = "tbSpectraToPredict";
            this.tbSpectraToPredict.Size = new System.Drawing.Size(230, 31);
            this.tbSpectraToPredict.TabIndex = 137;
            this.tbSpectraToPredict.Text = "50";
            // 
            // lblSelectSpectraCount
            // 
            this.lblSelectSpectraCount.AutoSize = true;
            this.lblSelectSpectraCount.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectSpectraCount.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSelectSpectraCount.Location = new System.Drawing.Point(14, 16);
            this.lblSelectSpectraCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectSpectraCount.Name = "lblSelectSpectraCount";
            this.lblSelectSpectraCount.Size = new System.Drawing.Size(157, 24);
            this.lblSelectSpectraCount.TabIndex = 136;
            this.lblSelectSpectraCount.Text = "SpectraToPredict";
            // 
            // lblScene
            // 
            this.lblScene.AutoSize = true;
            this.lblScene.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblScene.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblScene.Location = new System.Drawing.Point(968, 12);
            this.lblScene.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScene.Name = "lblScene";
            this.lblScene.Size = new System.Drawing.Size(60, 24);
            this.lblScene.TabIndex = 155;
            this.lblScene.Text = "Scene";
            // 
            // cbScene
            // 
            this.cbScene.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScene.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbScene.FormattingEnabled = true;
            this.cbScene.Items.AddRange(new object[] { "Computation", "Search" });
            this.cbScene.Location = new System.Drawing.Point(1040, 8);
            this.cbScene.Margin = new System.Windows.Forms.Padding(4);
            this.cbScene.Name = "cbScene";
            this.cbScene.Size = new System.Drawing.Size(186, 32);
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
            this.tabs.Location = new System.Drawing.Point(508, 214);
            this.tabs.Margin = new System.Windows.Forms.Padding(4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(916, 606);
            this.tabs.TabIndex = 156;
            this.tabs.UncloseTabIndexs = null;
            // 
            // pageComputation
            // 
            this.pageComputation.Controls.Add(this.cbCompressedIndex);
            this.pageComputation.Controls.Add(this.cbAutoDecision);
            this.pageComputation.Controls.Add(this.tableAutoDecision);
            this.pageComputation.Controls.Add(this.tableDeciderWeight);
            this.pageComputation.Controls.Add(this.tbSpectraToPredict);
            this.pageComputation.Controls.Add(this.lblSelectSpectraCount);
            this.pageComputation.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageComputation.Location = new System.Drawing.Point(4, 54);
            this.pageComputation.Margin = new System.Windows.Forms.Padding(4);
            this.pageComputation.Name = "pageComputation";
            this.pageComputation.Padding = new System.Windows.Forms.Padding(4);
            this.pageComputation.Size = new System.Drawing.Size(908, 548);
            this.pageComputation.TabIndex = 0;
            this.pageComputation.Text = "Computation";
            this.pageComputation.UseVisualStyleBackColor = true;
            // 
            // cbCompressedIndex
            // 
            this.cbCompressedIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCompressedIndex.AutoSize = true;
            this.cbCompressedIndex.Checked = true;
            this.cbCompressedIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCompressedIndex.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbCompressedIndex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbCompressedIndex.Location = new System.Drawing.Point(18, 66);
            this.cbCompressedIndex.Margin = new System.Windows.Forms.Padding(4);
            this.cbCompressedIndex.Name = "cbCompressedIndex";
            this.cbCompressedIndex.Size = new System.Drawing.Size(195, 28);
            this.cbCompressedIndex.TabIndex = 154;
            this.cbCompressedIndex.Text = "Compressed Index";
            this.cbCompressedIndex.UseVisualStyleBackColor = true;
            // 
            // pageSearchEngine
            // 
            this.pageSearchEngine.Controls.Add(this.cbFastReadMode);
            this.pageSearchEngine.Location = new System.Drawing.Point(4, 54);
            this.pageSearchEngine.Margin = new System.Windows.Forms.Padding(4);
            this.pageSearchEngine.Name = "pageSearchEngine";
            this.pageSearchEngine.Padding = new System.Windows.Forms.Padding(4);
            this.pageSearchEngine.Size = new System.Drawing.Size(908, 548);
            this.pageSearchEngine.TabIndex = 1;
            this.pageSearchEngine.Text = "Search";
            this.pageSearchEngine.UseVisualStyleBackColor = true;
            // 
            // cbFastReadMode
            // 
            this.cbFastReadMode.Checked = true;
            this.cbFastReadMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFastReadMode.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbFastReadMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbFastReadMode.Location = new System.Drawing.Point(8, 8);
            this.cbFastReadMode.Margin = new System.Windows.Forms.Padding(4);
            this.cbFastReadMode.Name = "cbFastReadMode";
            this.cbFastReadMode.Size = new System.Drawing.Size(336, 36);
            this.cbFastReadMode.TabIndex = 159;
            this.cbFastReadMode.Text = "Fast Reading Mode(Bigger Size)";
            this.cbFastReadMode.UseVisualStyleBackColor = true;
            // 
            // pageStorage
            // 
            this.pageStorage.Controls.Add(this.cbConfigStackLayers);
            this.pageStorage.Controls.Add(this.cbConfigStack);
            this.pageStorage.Location = new System.Drawing.Point(4, 54);
            this.pageStorage.Margin = new System.Windows.Forms.Padding(4);
            this.pageStorage.Name = "pageStorage";
            this.pageStorage.Size = new System.Drawing.Size(908, 548);
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
            this.lblSceneConfig.Location = new System.Drawing.Point(508, 184);
            this.lblSceneConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSceneConfig.Name = "lblSceneConfig";
            this.lblSceneConfig.Size = new System.Drawing.Size(302, 24);
            this.lblSceneConfig.TabIndex = 157;
            this.lblSceneConfig.Text = "Configuration for Different Scenes";
            // 
            // cbConfigIsCentroid
            // 
            this.cbConfigIsCentroid.AutoSize = true;
            this.cbConfigIsCentroid.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfigIsCentroid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbConfigIsCentroid.Location = new System.Drawing.Point(1244, 60);
            this.cbConfigIsCentroid.Margin = new System.Windows.Forms.Padding(4);
            this.cbConfigIsCentroid.Name = "cbConfigIsCentroid";
            this.cbConfigIsCentroid.Size = new System.Drawing.Size(111, 28);
            this.cbConfigIsCentroid.TabIndex = 158;
            this.cbConfigIsCentroid.Text = "Centroid";
            this.cbConfigIsCentroid.UseVisualStyleBackColor = true;
            this.cbConfigIsCentroid.CheckedChanged += new System.EventHandler(this.cbConfigIsCentroid_CheckedChanged);
            // 
            // ConversionConfigListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1458, 900);
            this.Controls.Add(this.cbConfigIsCentroid);
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
            this.Margin = new System.Windows.Forms.Padding(4);
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
            this.pageSearchEngine.ResumeLayout(false);
            this.pageStorage.ResumeLayout(false);
            this.pageStorage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public System.Windows.Forms.CheckBox cbFastReadMode;

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
        public System.Windows.Forms.CheckBox cbConfigIsCentroid;
    }
}