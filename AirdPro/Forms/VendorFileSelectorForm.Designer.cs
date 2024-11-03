
namespace AirdPro.Forms
{
    partial class VendorFileSelectorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VendorFileSelectorForm));
            this.rbDDA = new System.Windows.Forms.RadioButton();
            this.gbAcquisitionMode = new System.Windows.Forms.GroupBox();
            this.rbMRM = new System.Windows.Forms.RadioButton();
            this.rbPRM = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.rbDDAPasef = new System.Windows.Forms.RadioButton();
            this.rbDIAPasef = new System.Windows.Forms.RadioButton();
            this.rbDIA = new System.Windows.Forms.RadioButton();
            this.comboBox_scan_sequence = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_scan_pattern = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_scan_direction = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pixel_z = new System.Windows.Forms.NumericUpDown();
            this.pixel_x = new System.Windows.Forms.NumericUpDown();
            this.pixel_y = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_file_organisation = new System.Windows.Forms.ComboBox();
            this.rbVendor = new System.Windows.Forms.RadioButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnEditConfigs = new System.Windows.Forms.Button();
            this.lblConfigOutputPath = new System.Windows.Forms.Label();
            this.btnConfigChooseFolder = new System.Windows.Forms.Button();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.cbConfig = new System.Windows.Forms.ComboBox();
            this.lblConvertConfig = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.btnFileRefresh = new System.Windows.Forms.Button();
            this.btnPin = new System.Windows.Forms.Button();
            this.btnUnpin = new System.Windows.Forms.Button();
            this.btnTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.imgBtnAdd = new HZH_Controls.Controls.UCBtnImg();
            this.imgBtnClose = new HZH_Controls.Controls.UCBtnImg();
            this.imgBtnPublish = new HZH_Controls.Controls.UCBtnImg();
            this.mSIFileOrganisationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.gbMsiFormat = new System.Windows.Forms.GroupBox();
            this.rbImzML = new System.Windows.Forms.RadioButton();
            this.rbMzML = new System.Windows.Forms.RadioButton();
            this.cbMSI = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbMsiConfig = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbPixelSizeY = new System.Windows.Forms.TextBox();
            this.tbPixelSizeX = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pnlSpectraLocationFile = new System.Windows.Forms.Panel();
            this.btnLocationUpload = new System.Windows.Forms.Button();
            this.tbLocationFilePath = new System.Windows.Forms.TextBox();
            this.msFileViews = new AirdPro.FolderFileBrowser();
            this.gbAcquisitionMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_x)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSIFileOrganisationBindingSource)).BeginInit();
            this.gbMsiFormat.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbMsiConfig.SuspendLayout();
            this.pnlSpectraLocationFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbDDA
            // 
            this.rbDDA.AutoSize = true;
            this.rbDDA.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbDDA.Location = new System.Drawing.Point(12, 76);
            this.rbDDA.Margin = new System.Windows.Forms.Padding(4);
            this.rbDDA.Name = "rbDDA";
            this.rbDDA.Size = new System.Drawing.Size(76, 28);
            this.rbDDA.TabIndex = 1;
            this.rbDDA.Text = "DDA";
            this.rbDDA.UseVisualStyleBackColor = true;
            // 
            // gbAcquisitionMode
            // 
            this.gbAcquisitionMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAcquisitionMode.Controls.Add(this.rbMRM);
            this.gbAcquisitionMode.Controls.Add(this.rbPRM);
            this.gbAcquisitionMode.Controls.Add(this.rbAuto);
            this.gbAcquisitionMode.Controls.Add(this.rbDDAPasef);
            this.gbAcquisitionMode.Controls.Add(this.rbDIAPasef);
            this.gbAcquisitionMode.Controls.Add(this.rbDIA);
            this.gbAcquisitionMode.Controls.Add(this.rbDDA);
            this.gbAcquisitionMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbAcquisitionMode.Location = new System.Drawing.Point(1885, 152);
            this.gbAcquisitionMode.Margin = new System.Windows.Forms.Padding(4);
            this.gbAcquisitionMode.Name = "gbAcquisitionMode";
            this.gbAcquisitionMode.Padding = new System.Windows.Forms.Padding(4);
            this.gbAcquisitionMode.Size = new System.Drawing.Size(241, 295);
            this.gbAcquisitionMode.TabIndex = 2;
            this.gbAcquisitionMode.TabStop = false;
            this.gbAcquisitionMode.Text = "Acquisition Mode";
            // 
            // rbMRM
            // 
            this.rbMRM.AutoSize = true;
            this.rbMRM.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbMRM.Location = new System.Drawing.Point(12, 256);
            this.rbMRM.Margin = new System.Windows.Forms.Padding(4);
            this.rbMRM.Name = "rbMRM";
            this.rbMRM.Size = new System.Drawing.Size(83, 28);
            this.rbMRM.TabIndex = 9;
            this.rbMRM.Text = "MRM";
            this.rbMRM.UseVisualStyleBackColor = true;
            // 
            // rbPRM
            // 
            this.rbPRM.AutoSize = true;
            this.rbPRM.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbPRM.Location = new System.Drawing.Point(12, 220);
            this.rbPRM.Margin = new System.Windows.Forms.Padding(4);
            this.rbPRM.Name = "rbPRM";
            this.rbPRM.Size = new System.Drawing.Size(76, 28);
            this.rbPRM.TabIndex = 8;
            this.rbPRM.Text = "PRM";
            this.rbPRM.UseVisualStyleBackColor = true;
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Checked = true;
            this.rbAuto.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbAuto.Location = new System.Drawing.Point(12, 40);
            this.rbAuto.Margin = new System.Windows.Forms.Padding(4);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(77, 28);
            this.rbAuto.TabIndex = 7;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Auto";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // rbDDAPasef
            // 
            this.rbDDAPasef.AutoSize = true;
            this.rbDDAPasef.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbDDAPasef.Location = new System.Drawing.Point(12, 112);
            this.rbDDAPasef.Margin = new System.Windows.Forms.Padding(4);
            this.rbDDAPasef.Name = "rbDDAPasef";
            this.rbDDAPasef.Size = new System.Drawing.Size(138, 28);
            this.rbDDAPasef.TabIndex = 6;
            this.rbDDAPasef.Text = "DDA_PASEF";
            this.rbDDAPasef.UseVisualStyleBackColor = true;
            // 
            // rbDIAPasef
            // 
            this.rbDIAPasef.AutoSize = true;
            this.rbDIAPasef.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbDIAPasef.Location = new System.Drawing.Point(12, 184);
            this.rbDIAPasef.Margin = new System.Windows.Forms.Padding(4);
            this.rbDIAPasef.Name = "rbDIAPasef";
            this.rbDIAPasef.Size = new System.Drawing.Size(129, 28);
            this.rbDIAPasef.TabIndex = 5;
            this.rbDIAPasef.Text = "DIA_PASEF";
            this.rbDIAPasef.UseVisualStyleBackColor = true;
            // 
            // rbDIA
            // 
            this.rbDIA.AutoSize = true;
            this.rbDIA.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbDIA.Location = new System.Drawing.Point(13, 148);
            this.rbDIA.Margin = new System.Windows.Forms.Padding(4);
            this.rbDIA.Name = "rbDIA";
            this.rbDIA.Size = new System.Drawing.Size(67, 28);
            this.rbDIA.TabIndex = 2;
            this.rbDIA.Text = "DIA";
            this.rbDIA.UseVisualStyleBackColor = true;
            // 
            // comboBox_scan_sequence
            // 
            this.comboBox_scan_sequence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_scan_sequence.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_scan_sequence.FormattingEnabled = true;
            this.comboBox_scan_sequence.Location = new System.Drawing.Point(8, 388);
            this.comboBox_scan_sequence.Name = "comboBox_scan_sequence";
            this.comboBox_scan_sequence.Size = new System.Drawing.Size(227, 32);
            this.comboBox_scan_sequence.TabIndex = 143;
            this.comboBox_scan_sequence.Tag = "";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(9, 361);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 24);
            this.label7.TabIndex = 142;
            this.label7.Text = "Scan Sequence";
            // 
            // comboBox_scan_pattern
            // 
            this.comboBox_scan_pattern.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_scan_pattern.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_scan_pattern.FormattingEnabled = true;
            this.comboBox_scan_pattern.Location = new System.Drawing.Point(7, 450);
            this.comboBox_scan_pattern.Name = "comboBox_scan_pattern";
            this.comboBox_scan_pattern.Size = new System.Drawing.Size(228, 32);
            this.comboBox_scan_pattern.TabIndex = 141;
            this.comboBox_scan_pattern.Tag = "";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(8, 423);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 24);
            this.label6.TabIndex = 140;
            this.label6.Text = "Scan Pattern";
            // 
            // comboBox_scan_direction
            // 
            this.comboBox_scan_direction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_scan_direction.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_scan_direction.FormattingEnabled = true;
            this.comboBox_scan_direction.Location = new System.Drawing.Point(5, 326);
            this.comboBox_scan_direction.Name = "comboBox_scan_direction";
            this.comboBox_scan_direction.Size = new System.Drawing.Size(230, 32);
            this.comboBox_scan_direction.TabIndex = 139;
            this.comboBox_scan_direction.Tag = "";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(6, 299);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 24);
            this.label5.TabIndex = 138;
            this.label5.Text = "Scan Direction";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(9, 259);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 24);
            this.label4.TabIndex = 19;
            this.label4.Text = "pixels in z";
            // 
            // pixel_z
            // 
            this.pixel_z.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pixel_z.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pixel_z.Location = new System.Drawing.Point(109, 257);
            this.pixel_z.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.pixel_z.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pixel_z.Name = "pixel_z";
            this.pixel_z.Size = new System.Drawing.Size(126, 31);
            this.pixel_z.TabIndex = 20;
            this.pixel_z.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pixel_x
            // 
            this.pixel_x.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pixel_x.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pixel_x.Location = new System.Drawing.Point(109, 179);
            this.pixel_x.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.pixel_x.Name = "pixel_x";
            this.pixel_x.Size = new System.Drawing.Size(126, 31);
            this.pixel_x.TabIndex = 18;
            // 
            // pixel_y
            // 
            this.pixel_y.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pixel_y.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pixel_y.Location = new System.Drawing.Point(109, 218);
            this.pixel_y.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.pixel_y.Name = "pixel_y";
            this.pixel_y.Size = new System.Drawing.Size(126, 31);
            this.pixel_y.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(9, 220);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 24);
            this.label3.TabIndex = 15;
            this.label3.Text = "pixels in y";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(9, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "pixels in x";
            // 
            // comboBox_file_organisation
            // 
            this.comboBox_file_organisation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_file_organisation.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_file_organisation.FormattingEnabled = true;
            this.comboBox_file_organisation.Location = new System.Drawing.Point(7, 62);
            this.comboBox_file_organisation.Name = "comboBox_file_organisation";
            this.comboBox_file_organisation.Size = new System.Drawing.Size(228, 32);
            this.comboBox_file_organisation.TabIndex = 13;
            this.comboBox_file_organisation.Tag = "";
            // 
            // rbVendor
            // 
            this.rbVendor.AutoSize = true;
            this.rbVendor.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbVendor.Location = new System.Drawing.Point(20, 110);
            this.rbVendor.Margin = new System.Windows.Forms.Padding(4);
            this.rbVendor.Name = "rbVendor";
            this.rbVendor.Size = new System.Drawing.Size(95, 28);
            this.rbVendor.TabIndex = 10;
            this.rbVendor.Text = "vendor";
            this.rbVendor.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Vendor Files|*.wiff;*.raw;*.mzML;*mzXML";
            this.openFileDialog.Multiselect = true;
            // 
            // btnEditConfigs
            // 
            this.btnEditConfigs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEditConfigs.Location = new System.Drawing.Point(431, 21);
            this.btnEditConfigs.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditConfigs.Name = "btnEditConfigs";
            this.btnEditConfigs.Size = new System.Drawing.Size(112, 32);
            this.btnEditConfigs.TabIndex = 6;
            this.btnEditConfigs.Text = "Browser";
            this.btnEditConfigs.UseVisualStyleBackColor = true;
            this.btnEditConfigs.Click += new System.EventHandler(this.BtnCreateConfigs_Click);
            // 
            // lblConfigOutputPath
            // 
            this.lblConfigOutputPath.AutoSize = true;
            this.lblConfigOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOutputPath.Location = new System.Drawing.Point(565, 24);
            this.lblConfigOutputPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigOutputPath.Name = "lblConfigOutputPath";
            this.lblConfigOutputPath.Size = new System.Drawing.Size(121, 24);
            this.lblConfigOutputPath.TabIndex = 119;
            this.lblConfigOutputPath.Text = "Output Path:";
            // 
            // btnConfigChooseFolder
            // 
            this.btnConfigChooseFolder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConfigChooseFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(1003, 21);
            this.btnConfigChooseFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfigChooseFolder.Name = "btnConfigChooseFolder";
            this.btnConfigChooseFolder.Size = new System.Drawing.Size(112, 32);
            this.btnConfigChooseFolder.TabIndex = 120;
            this.btnConfigChooseFolder.Text = "Browser";
            this.btnConfigChooseFolder.UseVisualStyleBackColor = true;
            this.btnConfigChooseFolder.Click += new System.EventHandler(this.BtnConfigChooseFolder_Click);
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbOutputPath.Location = new System.Drawing.Point(694, 22);
            this.tbOutputPath.Margin = new System.Windows.Forms.Padding(4);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(301, 31);
            this.tbOutputPath.TabIndex = 118;
            // 
            // cbConfig
            // 
            this.cbConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfig.FormattingEnabled = true;
            this.cbConfig.Location = new System.Drawing.Point(193, 21);
            this.cbConfig.Margin = new System.Windows.Forms.Padding(4);
            this.cbConfig.Name = "cbConfig";
            this.cbConfig.Size = new System.Drawing.Size(230, 32);
            this.cbConfig.TabIndex = 121;
            this.cbConfig.SelectionChangeCommitted += new System.EventHandler(this.CbConfig_SelectionChangeCommitted);
            // 
            // lblConvertConfig
            // 
            this.lblConvertConfig.AutoSize = true;
            this.lblConvertConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConvertConfig.Location = new System.Drawing.Point(14, 24);
            this.lblConvertConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConvertConfig.Name = "lblConvertConfig";
            this.lblConvertConfig.Size = new System.Drawing.Size(171, 24);
            this.lblConvertConfig.TabIndex = 122;
            this.lblConvertConfig.Text = "Conversion Config:";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 81);
            this.label1.TabIndex = 123;
            this.label1.Text = "If your acquisition method is PRM, select the PRM option directly!";
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "DirectoryClose16x16.png");
            this.imgList.Images.SetKeyName(1, "DirectoryOpen16x16.png");
            this.imgList.Images.SetKeyName(2, "Spectrum16x16.png");
            // 
            // btnFileRefresh
            // 
            this.btnFileRefresh.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFileRefresh.BackgroundImage")));
            this.btnFileRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFileRefresh.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFileRefresh.Location = new System.Drawing.Point(15, 64);
            this.btnFileRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnFileRefresh.Name = "btnFileRefresh";
            this.btnFileRefresh.Size = new System.Drawing.Size(45, 45);
            this.btnFileRefresh.TabIndex = 132;
            this.btnTooltip.SetToolTip(this.btnFileRefresh, "refresh the selected folder");
            this.btnFileRefresh.UseVisualStyleBackColor = true;
            this.btnFileRefresh.Click += new System.EventHandler(this.BtnFileRefresh_Click);
            // 
            // btnPin
            // 
            this.btnPin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPin.BackgroundImage")));
            this.btnPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPin.Location = new System.Drawing.Point(69, 64);
            this.btnPin.Margin = new System.Windows.Forms.Padding(4);
            this.btnPin.Name = "btnPin";
            this.btnPin.Size = new System.Drawing.Size(45, 45);
            this.btnPin.TabIndex = 133;
            this.btnTooltip.SetToolTip(this.btnPin, "Pin the selected folder");
            this.btnPin.UseVisualStyleBackColor = true;
            this.btnPin.Click += new System.EventHandler(this.BtnPin_Click);
            // 
            // btnUnpin
            // 
            this.btnUnpin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUnpin.BackgroundImage")));
            this.btnUnpin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUnpin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUnpin.Location = new System.Drawing.Point(123, 64);
            this.btnUnpin.Margin = new System.Windows.Forms.Padding(4);
            this.btnUnpin.Name = "btnUnpin";
            this.btnUnpin.Size = new System.Drawing.Size(45, 45);
            this.btnUnpin.TabIndex = 134;
            this.btnTooltip.SetToolTip(this.btnUnpin, "Unpin the selected folder");
            this.btnUnpin.UseVisualStyleBackColor = true;
            this.btnUnpin.Click += new System.EventHandler(this.BtnUnpin_Click);
            // 
            // imgBtnAdd
            // 
            this.imgBtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.imgBtnAdd.BackColor = System.Drawing.Color.White;
            this.imgBtnAdd.BtnBackColor = System.Drawing.Color.White;
            this.imgBtnAdd.BtnFont = new System.Drawing.Font("微软雅黑", 9F);
            this.imgBtnAdd.BtnForeColor = System.Drawing.Color.Black;
            this.imgBtnAdd.BtnText = " Add";
            this.imgBtnAdd.ConerRadius = 1;
            this.imgBtnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgBtnAdd.EnabledMouseEffect = true;
            this.imgBtnAdd.FillColor = System.Drawing.Color.White;
            this.imgBtnAdd.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.imgBtnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.imgBtnAdd.Image = ((System.Drawing.Image)(resources.GetObject("imgBtnAdd.Image")));
            this.imgBtnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.imgBtnAdd.ImageFontIcons = ((object)(resources.GetObject("imgBtnAdd.ImageFontIcons")));
            this.imgBtnAdd.IsRadius = true;
            this.imgBtnAdd.IsShowRect = true;
            this.imgBtnAdd.IsShowTips = false;
            this.imgBtnAdd.Location = new System.Drawing.Point(1885, 1236);
            this.imgBtnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnAdd.Name = "imgBtnAdd";
            this.imgBtnAdd.RectColor = System.Drawing.Color.Silver;
            this.imgBtnAdd.RectWidth = 1;
            this.imgBtnAdd.Size = new System.Drawing.Size(241, 57);
            this.imgBtnAdd.TabIndex = 135;
            this.imgBtnAdd.TabStop = false;
            this.imgBtnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.imgBtnAdd.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.imgBtnAdd.TipsText = "";
            this.imgBtnAdd.BtnClick += new System.EventHandler(this.ImgBtnAdd_BtnClick);
            // 
            // imgBtnClose
            // 
            this.imgBtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.imgBtnClose.BackColor = System.Drawing.Color.White;
            this.imgBtnClose.BtnBackColor = System.Drawing.Color.White;
            this.imgBtnClose.BtnFont = new System.Drawing.Font("微软雅黑", 9F);
            this.imgBtnClose.BtnForeColor = System.Drawing.Color.Black;
            this.imgBtnClose.BtnText = "Close";
            this.imgBtnClose.ConerRadius = 1;
            this.imgBtnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgBtnClose.EnabledMouseEffect = true;
            this.imgBtnClose.FillColor = System.Drawing.Color.White;
            this.imgBtnClose.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.imgBtnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.imgBtnClose.Image = ((System.Drawing.Image)(resources.GetObject("imgBtnClose.Image")));
            this.imgBtnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.imgBtnClose.ImageFontIcons = ((object)(resources.GetObject("imgBtnClose.ImageFontIcons")));
            this.imgBtnClose.IsRadius = true;
            this.imgBtnClose.IsShowRect = true;
            this.imgBtnClose.IsShowTips = false;
            this.imgBtnClose.Location = new System.Drawing.Point(1885, 1305);
            this.imgBtnClose.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnClose.Name = "imgBtnClose";
            this.imgBtnClose.RectColor = System.Drawing.Color.Silver;
            this.imgBtnClose.RectWidth = 1;
            this.imgBtnClose.Size = new System.Drawing.Size(241, 57);
            this.imgBtnClose.TabIndex = 136;
            this.imgBtnClose.TabStop = false;
            this.imgBtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.imgBtnClose.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.imgBtnClose.TipsText = "";
            this.imgBtnClose.BtnClick += new System.EventHandler(this.ImgBtnClose_BtnClick);
            // 
            // imgBtnPublish
            // 
            this.imgBtnPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.imgBtnPublish.BackColor = System.Drawing.Color.White;
            this.imgBtnPublish.BtnBackColor = System.Drawing.Color.White;
            this.imgBtnPublish.BtnFont = new System.Drawing.Font("微软雅黑", 9F);
            this.imgBtnPublish.BtnForeColor = System.Drawing.Color.Black;
            this.imgBtnPublish.BtnText = "Publish";
            this.imgBtnPublish.ConerRadius = 1;
            this.imgBtnPublish.Cursor = System.Windows.Forms.Cursors.Hand;
            this.imgBtnPublish.EnabledMouseEffect = true;
            this.imgBtnPublish.FillColor = System.Drawing.Color.White;
            this.imgBtnPublish.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.imgBtnPublish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.imgBtnPublish.Image = ((System.Drawing.Image)(resources.GetObject("imgBtnPublish.Image")));
            this.imgBtnPublish.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.imgBtnPublish.ImageFontIcons = ((object)(resources.GetObject("imgBtnPublish.ImageFontIcons")));
            this.imgBtnPublish.IsRadius = true;
            this.imgBtnPublish.IsShowRect = true;
            this.imgBtnPublish.IsShowTips = false;
            this.imgBtnPublish.Location = new System.Drawing.Point(1885, 1167);
            this.imgBtnPublish.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnPublish.Name = "imgBtnPublish";
            this.imgBtnPublish.RectColor = System.Drawing.Color.Silver;
            this.imgBtnPublish.RectWidth = 1;
            this.imgBtnPublish.Size = new System.Drawing.Size(241, 57);
            this.imgBtnPublish.TabIndex = 137;
            this.imgBtnPublish.TabStop = false;
            this.imgBtnPublish.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.imgBtnPublish.TipsColor = System.Drawing.Color.Purple;
            this.imgBtnPublish.TipsText = "Publish To Redis";
            this.imgBtnPublish.BtnClick += new System.EventHandler(this.ImgBtnPublish_BtnClick);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(9, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(157, 24);
            this.label8.TabIndex = 144;
            this.label8.Text = "File Organisation";
            // 
            // gbMsiFormat
            // 
            this.gbMsiFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMsiFormat.Controls.Add(this.rbImzML);
            this.gbMsiFormat.Controls.Add(this.rbMzML);
            this.gbMsiFormat.Controls.Add(this.rbVendor);
            this.gbMsiFormat.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbMsiFormat.Location = new System.Drawing.Point(1885, 454);
            this.gbMsiFormat.Name = "gbMsiFormat";
            this.gbMsiFormat.Size = new System.Drawing.Size(241, 143);
            this.gbMsiFormat.TabIndex = 138;
            this.gbMsiFormat.TabStop = false;
            this.gbMsiFormat.Text = "MSI Format";
            this.gbMsiFormat.Visible = false;
            // 
            // rbImzML
            // 
            this.rbImzML.AutoSize = true;
            this.rbImzML.Checked = true;
            this.rbImzML.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbImzML.Location = new System.Drawing.Point(20, 38);
            this.rbImzML.Margin = new System.Windows.Forms.Padding(4);
            this.rbImzML.Name = "rbImzML";
            this.rbImzML.Size = new System.Drawing.Size(93, 28);
            this.rbImzML.TabIndex = 12;
            this.rbImzML.TabStop = true;
            this.rbImzML.Text = "imzML";
            this.rbImzML.UseVisualStyleBackColor = true;
            // 
            // rbMzML
            // 
            this.rbMzML.AutoSize = true;
            this.rbMzML.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbMzML.Location = new System.Drawing.Point(20, 74);
            this.rbMzML.Margin = new System.Windows.Forms.Padding(4);
            this.rbMzML.Name = "rbMzML";
            this.rbMzML.Size = new System.Drawing.Size(88, 28);
            this.rbMzML.TabIndex = 11;
            this.rbMzML.Text = "mzML";
            this.rbMzML.UseVisualStyleBackColor = true;
            // 
            // cbMSI
            // 
            this.cbMSI.AutoSize = true;
            this.cbMSI.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbMSI.Location = new System.Drawing.Point(1138, 24);
            this.cbMSI.Name = "cbMSI";
            this.cbMSI.Size = new System.Drawing.Size(69, 28);
            this.cbMSI.TabIndex = 139;
            this.cbMSI.Text = "MSI";
            this.cbMSI.UseVisualStyleBackColor = true;
            this.cbMSI.CheckedChanged += new System.EventHandler(this.CBoxMSI_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1883, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(241, 81);
            this.panel1.TabIndex = 141;
            // 
            // gbMsiConfig
            // 
            this.gbMsiConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMsiConfig.Controls.Add(this.label9);
            this.gbMsiConfig.Controls.Add(this.tbPixelSizeY);
            this.gbMsiConfig.Controls.Add(this.tbPixelSizeX);
            this.gbMsiConfig.Controls.Add(this.label10);
            this.gbMsiConfig.Controls.Add(this.label8);
            this.gbMsiConfig.Controls.Add(this.label2);
            this.gbMsiConfig.Controls.Add(this.pixel_z);
            this.gbMsiConfig.Controls.Add(this.label6);
            this.gbMsiConfig.Controls.Add(this.pixel_x);
            this.gbMsiConfig.Controls.Add(this.comboBox_file_organisation);
            this.gbMsiConfig.Controls.Add(this.comboBox_scan_sequence);
            this.gbMsiConfig.Controls.Add(this.comboBox_scan_pattern);
            this.gbMsiConfig.Controls.Add(this.label4);
            this.gbMsiConfig.Controls.Add(this.comboBox_scan_direction);
            this.gbMsiConfig.Controls.Add(this.pixel_y);
            this.gbMsiConfig.Controls.Add(this.label5);
            this.gbMsiConfig.Controls.Add(this.label7);
            this.gbMsiConfig.Controls.Add(this.label3);
            this.gbMsiConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gbMsiConfig.Location = new System.Drawing.Point(1885, 611);
            this.gbMsiConfig.Name = "gbMsiConfig";
            this.gbMsiConfig.Size = new System.Drawing.Size(241, 494);
            this.gbMsiConfig.TabIndex = 142;
            this.gbMsiConfig.TabStop = false;
            this.gbMsiConfig.Text = "MSI Config";
            this.gbMsiConfig.Visible = false;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(8, 108);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 24);
            this.label9.TabIndex = 149;
            this.label9.Text = "pixe size x";
            // 
            // tbPixelSizeY
            // 
            this.tbPixelSizeY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPixelSizeY.Location = new System.Drawing.Point(109, 140);
            this.tbPixelSizeY.Name = "tbPixelSizeY";
            this.tbPixelSizeY.Size = new System.Drawing.Size(98303, 47);
            this.tbPixelSizeY.TabIndex = 148;
            this.tbPixelSizeY.Text = "1";
            // 
            // tbPixelSizeX
            // 
            this.tbPixelSizeX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPixelSizeX.Location = new System.Drawing.Point(109, 101);
            this.tbPixelSizeX.Name = "tbPixelSizeX";
            this.tbPixelSizeX.Size = new System.Drawing.Size(98303, 47);
            this.tbPixelSizeX.TabIndex = 147;
            this.tbPixelSizeX.Text = "1";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(9, 143);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 24);
            this.label10.TabIndex = 146;
            this.label10.Text = "pixe size y";
            // 
            // pnlSpectraLocationFile
            // 
            this.pnlSpectraLocationFile.Controls.Add(this.btnLocationUpload);
            this.pnlSpectraLocationFile.Controls.Add(this.tbLocationFilePath);
            this.pnlSpectraLocationFile.Location = new System.Drawing.Point(1213, 20);
            this.pnlSpectraLocationFile.Name = "pnlSpectraLocationFile";
            this.pnlSpectraLocationFile.Size = new System.Drawing.Size(595, 38);
            this.pnlSpectraLocationFile.TabIndex = 145;
            this.pnlSpectraLocationFile.Visible = false;
            // 
            // btnLocationUpload
            // 
            this.btnLocationUpload.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnLocationUpload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLocationUpload.Location = new System.Drawing.Point(384, 3);
            this.btnLocationUpload.Margin = new System.Windows.Forms.Padding(4);
            this.btnLocationUpload.Name = "btnLocationUpload";
            this.btnLocationUpload.Size = new System.Drawing.Size(207, 32);
            this.btnLocationUpload.TabIndex = 146;
            this.btnLocationUpload.Text = "Spectra Location File";
            this.btnLocationUpload.UseVisualStyleBackColor = true;
            this.btnLocationUpload.Click += new System.EventHandler(this.btnLocationUpload_Click);
            // 
            // tbLocationFilePath
            // 
            this.tbLocationFilePath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbLocationFilePath.Location = new System.Drawing.Point(5, 5);
            this.tbLocationFilePath.Margin = new System.Windows.Forms.Padding(4);
            this.tbLocationFilePath.Name = "tbLocationFilePath";
            this.tbLocationFilePath.Size = new System.Drawing.Size(371, 31);
            this.tbLocationFilePath.TabIndex = 145;
            // 
            // msFileViews
            // 
            this.msFileViews.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msFileViews.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.msFileViews.Location = new System.Drawing.Point(9, 114);
            this.msFileViews.Margin = new System.Windows.Forms.Padding(6);
            this.msFileViews.Name = "msFileViews";
            this.msFileViews.Size = new System.Drawing.Size(1865, 1259);
            this.msFileViews.TabIndex = 125;
            // 
            // VendorFileSelectorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(2137, 1382);
            this.Controls.Add(this.pnlSpectraLocationFile);
            this.Controls.Add(this.gbMsiConfig);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbMSI);
            this.Controls.Add(this.gbMsiFormat);
            this.Controls.Add(this.imgBtnPublish);
            this.Controls.Add(this.imgBtnClose);
            this.Controls.Add(this.imgBtnAdd);
            this.Controls.Add(this.btnUnpin);
            this.Controls.Add(this.btnPin);
            this.Controls.Add(this.btnFileRefresh);
            this.Controls.Add(this.msFileViews);
            this.Controls.Add(this.lblConvertConfig);
            this.Controls.Add(this.cbConfig);
            this.Controls.Add(this.lblConfigOutputPath);
            this.Controls.Add(this.btnConfigChooseFolder);
            this.Controls.Add(this.tbOutputPath);
            this.Controls.Add(this.btnEditConfigs);
            this.Controls.Add(this.gbAcquisitionMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VendorFileSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File/Folder Selector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VendorFileSelectorForm_FormClosing);
            this.Load += new System.EventHandler(this.VendorFileSelectorForm_Load);
            this.gbAcquisitionMode.ResumeLayout(false);
            this.gbAcquisitionMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_x)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSIFileOrganisationBindingSource)).EndInit();
            this.gbMsiFormat.ResumeLayout(false);
            this.gbMsiFormat.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.gbMsiConfig.ResumeLayout(false);
            this.gbMsiConfig.PerformLayout();
            this.pnlSpectraLocationFile.ResumeLayout(false);
            this.pnlSpectraLocationFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private HZH_Controls.Controls.UCBtnImg imgBtnPublish;

        private System.Windows.Forms.Button btnPin;

        private System.Windows.Forms.Button btnUnpin;

        #endregion
        private System.Windows.Forms.RadioButton rbDDA;
        private System.Windows.Forms.GroupBox gbAcquisitionMode;
        private System.Windows.Forms.RadioButton rbDIA;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RadioButton rbDDAPasef;
        private System.Windows.Forms.RadioButton rbDIAPasef;
        private System.Windows.Forms.Button btnEditConfigs;
        private System.Windows.Forms.Label lblConfigOutputPath;
        private System.Windows.Forms.Button btnConfigChooseFolder;
        public System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.ComboBox cbConfig;
        private System.Windows.Forms.Label lblConvertConfig;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton rbPRM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imgList;
        private AirdPro.FolderFileBrowser msFileViews;
        private System.Windows.Forms.RadioButton rbMRM;
        private System.Windows.Forms.Button btnFileRefresh;
        private System.Windows.Forms.ToolTip btnTooltip;
        private HZH_Controls.Controls.UCBtnImg imgBtnAdd;
        private HZH_Controls.Controls.UCBtnImg imgBtnClose;
        private System.Windows.Forms.RadioButton rbVendor;
        private System.Windows.Forms.ComboBox comboBox_file_organisation;
        private System.Windows.Forms.BindingSource mSIFileOrganisationBindingSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown pixel_y;
        private System.Windows.Forms.NumericUpDown pixel_x;
        private System.Windows.Forms.NumericUpDown pixel_z;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_scan_pattern;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_scan_direction;
        private System.Windows.Forms.ComboBox comboBox_scan_sequence;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox gbMsiFormat;
        private System.Windows.Forms.RadioButton rbImzML;
        private System.Windows.Forms.RadioButton rbMzML;
        private System.Windows.Forms.CheckBox cbMSI;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbMsiConfig;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbPixelSizeY;
        private System.Windows.Forms.TextBox tbPixelSizeX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel pnlSpectraLocationFile;
        private System.Windows.Forms.Button btnLocationUpload;
        public System.Windows.Forms.TextBox tbLocationFilePath;
    }
}