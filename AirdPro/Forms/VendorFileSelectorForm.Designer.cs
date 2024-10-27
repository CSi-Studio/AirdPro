
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
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.gBoxMode = new System.Windows.Forms.GroupBox();
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
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
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
            this.msFileViews = new AirdPro.FolderFileBrowser();
            this.gBoxMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_x)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSIFileOrganisationBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton1.Location = new System.Drawing.Point(12, 76);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(76, 28);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "DDA";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // gBoxMode
            // 
            this.gBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gBoxMode.Controls.Add(this.comboBox_scan_sequence);
            this.gBoxMode.Controls.Add(this.label7);
            this.gBoxMode.Controls.Add(this.comboBox_scan_pattern);
            this.gBoxMode.Controls.Add(this.label6);
            this.gBoxMode.Controls.Add(this.comboBox_scan_direction);
            this.gBoxMode.Controls.Add(this.label5);
            this.gBoxMode.Controls.Add(this.label4);
            this.gBoxMode.Controls.Add(this.pixel_z);
            this.gBoxMode.Controls.Add(this.pixel_x);
            this.gBoxMode.Controls.Add(this.pixel_y);
            this.gBoxMode.Controls.Add(this.label3);
            this.gBoxMode.Controls.Add(this.label2);
            this.gBoxMode.Controls.Add(this.comboBox_file_organisation);
            this.gBoxMode.Controls.Add(this.radioButton8);
            this.gBoxMode.Controls.Add(this.radioButton4);
            this.gBoxMode.Controls.Add(this.radioButton3);
            this.gBoxMode.Controls.Add(this.radioButton7);
            this.gBoxMode.Controls.Add(this.rbAuto);
            this.gBoxMode.Controls.Add(this.radioButton6);
            this.gBoxMode.Controls.Add(this.radioButton5);
            this.gBoxMode.Controls.Add(this.radioButton2);
            this.gBoxMode.Controls.Add(this.radioButton1);
            this.gBoxMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gBoxMode.Location = new System.Drawing.Point(1296, 192);
            this.gBoxMode.Margin = new System.Windows.Forms.Padding(4);
            this.gBoxMode.Name = "gBoxMode";
            this.gBoxMode.Padding = new System.Windows.Forms.Padding(4);
            this.gBoxMode.Size = new System.Drawing.Size(213, 758);
            this.gBoxMode.TabIndex = 2;
            this.gBoxMode.TabStop = false;
            this.gBoxMode.Text = "Acquisition Mode";
            // 
            // comboBox_scan_sequence
            // 
            this.comboBox_scan_sequence.FormattingEnabled = true;
            this.comboBox_scan_sequence.Location = new System.Drawing.Point(7, 574);
            this.comboBox_scan_sequence.Name = "comboBox_scan_sequence";
            this.comboBox_scan_sequence.Size = new System.Drawing.Size(199, 32);
            this.comboBox_scan_sequence.TabIndex = 143;
            this.comboBox_scan_sequence.Tag = "";
            this.comboBox_scan_sequence.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 547);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(138, 24);
            this.label7.TabIndex = 142;
            this.label7.Text = "Scan Sequence";
            this.label7.Visible = false;
            // 
            // comboBox_scan_pattern
            // 
            this.comboBox_scan_pattern.FormattingEnabled = true;
            this.comboBox_scan_pattern.Location = new System.Drawing.Point(7, 636);
            this.comboBox_scan_pattern.Name = "comboBox_scan_pattern";
            this.comboBox_scan_pattern.Size = new System.Drawing.Size(199, 32);
            this.comboBox_scan_pattern.TabIndex = 141;
            this.comboBox_scan_pattern.Tag = "";
            this.comboBox_scan_pattern.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 609);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 24);
            this.label6.TabIndex = 140;
            this.label6.Text = "Scan Pattern";
            this.label6.Visible = false;
            // 
            // comboBox_scan_direction
            // 
            this.comboBox_scan_direction.FormattingEnabled = true;
            this.comboBox_scan_direction.Location = new System.Drawing.Point(7, 512);
            this.comboBox_scan_direction.Name = "comboBox_scan_direction";
            this.comboBox_scan_direction.Size = new System.Drawing.Size(199, 32);
            this.comboBox_scan_direction.TabIndex = 139;
            this.comboBox_scan_direction.Tag = "";
            this.comboBox_scan_direction.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 485);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 24);
            this.label5.TabIndex = 138;
            this.label5.Text = "Scan Direction";
            this.label5.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 720);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 24);
            this.label4.TabIndex = 19;
            this.label4.Text = "pixels in z";
            this.label4.Visible = false;
            // 
            // pixel_z
            // 
            this.pixel_z.Location = new System.Drawing.Point(107, 718);
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
            this.pixel_z.Size = new System.Drawing.Size(99, 31);
            this.pixel_z.TabIndex = 20;
            this.pixel_z.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pixel_z.Visible = false;
            // 
            // pixel_x
            // 
            this.pixel_x.Location = new System.Drawing.Point(107, 414);
            this.pixel_x.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.pixel_x.Name = "pixel_x";
            this.pixel_x.Size = new System.Drawing.Size(99, 31);
            this.pixel_x.TabIndex = 18;
            this.pixel_x.Visible = false;
            // 
            // pixel_y
            // 
            this.pixel_y.Location = new System.Drawing.Point(107, 451);
            this.pixel_y.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.pixel_y.Name = "pixel_y";
            this.pixel_y.Size = new System.Drawing.Size(99, 31);
            this.pixel_y.TabIndex = 17;
            this.pixel_y.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 450);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 24);
            this.label3.TabIndex = 15;
            this.label3.Text = "pixels in y";
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 417);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "pixels in x";
            this.label2.Visible = false;
            // 
            // comboBox_file_organisation
            // 
            this.comboBox_file_organisation.FormattingEnabled = true;
            this.comboBox_file_organisation.Location = new System.Drawing.Point(7, 373);
            this.comboBox_file_organisation.Name = "comboBox_file_organisation";
            this.comboBox_file_organisation.Size = new System.Drawing.Size(199, 32);
            this.comboBox_file_organisation.TabIndex = 13;
            this.comboBox_file_organisation.Tag = "";
            this.comboBox_file_organisation.Visible = false;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton8.Location = new System.Drawing.Point(12, 338);
            this.radioButton8.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(108, 28);
            this.radioButton8.TabIndex = 11;
            this.radioButton8.Text = "DIA_MSI";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton4.Location = new System.Drawing.Point(12, 266);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(83, 28);
            this.radioButton4.TabIndex = 9;
            this.radioButton4.Text = "MRM";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton3.Location = new System.Drawing.Point(12, 226);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(76, 28);
            this.radioButton3.TabIndex = 8;
            this.radioButton3.Text = "PRM";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton7.Location = new System.Drawing.Point(12, 302);
            this.radioButton7.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(117, 28);
            this.radioButton7.TabIndex = 10;
            this.radioButton7.Text = "DDA_MSI";
            this.radioButton7.UseVisualStyleBackColor = true;
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
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton6.Location = new System.Drawing.Point(12, 116);
            this.radioButton6.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(138, 28);
            this.radioButton6.TabIndex = 6;
            this.radioButton6.Text = "DDA_PASEF";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton5.Location = new System.Drawing.Point(12, 189);
            this.radioButton5.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(129, 28);
            this.radioButton5.TabIndex = 5;
            this.radioButton5.Text = "DIA_PASEF";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton2.Location = new System.Drawing.Point(12, 153);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(67, 28);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "DIA";
            this.radioButton2.UseVisualStyleBackColor = true;
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
            this.btnEditConfigs.Location = new System.Drawing.Point(438, 18);
            this.btnEditConfigs.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditConfigs.Name = "btnEditConfigs";
            this.btnEditConfigs.Size = new System.Drawing.Size(112, 39);
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
            this.lblConfigOutputPath.Location = new System.Drawing.Point(576, 26);
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
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(1005, 21);
            this.btnConfigChooseFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfigChooseFolder.Name = "btnConfigChooseFolder";
            this.btnConfigChooseFolder.Size = new System.Drawing.Size(112, 39);
            this.btnConfigChooseFolder.TabIndex = 120;
            this.btnConfigChooseFolder.Text = "Browser";
            this.btnConfigChooseFolder.UseVisualStyleBackColor = true;
            this.btnConfigChooseFolder.Click += new System.EventHandler(this.BtnConfigChooseFolder_Click);
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbOutputPath.Location = new System.Drawing.Point(693, 21);
            this.tbOutputPath.Margin = new System.Windows.Forms.Padding(4);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(301, 31);
            this.tbOutputPath.TabIndex = 118;
            // 
            // cbConfig
            // 
            this.cbConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfig.FormattingEnabled = true;
            this.cbConfig.Location = new System.Drawing.Point(196, 18);
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
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(1299, 75);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 112);
            this.label1.TabIndex = 123;
            this.label1.Text = "If your acquisition method is PRM,\r\nselect the PRM option directly!";
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
            this.imgBtnAdd.Location = new System.Drawing.Point(1296, 1059);
            this.imgBtnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnAdd.Name = "imgBtnAdd";
            this.imgBtnAdd.RectColor = System.Drawing.Color.Silver;
            this.imgBtnAdd.RectWidth = 1;
            this.imgBtnAdd.Size = new System.Drawing.Size(213, 57);
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
            this.imgBtnClose.Location = new System.Drawing.Point(1296, 1125);
            this.imgBtnClose.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnClose.Name = "imgBtnClose";
            this.imgBtnClose.RectColor = System.Drawing.Color.Silver;
            this.imgBtnClose.RectWidth = 1;
            this.imgBtnClose.Size = new System.Drawing.Size(213, 57);
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
            this.imgBtnPublish.Location = new System.Drawing.Point(1296, 990);
            this.imgBtnPublish.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnPublish.Name = "imgBtnPublish";
            this.imgBtnPublish.RectColor = System.Drawing.Color.Silver;
            this.imgBtnPublish.RectWidth = 1;
            this.imgBtnPublish.Size = new System.Drawing.Size(213, 57);
            this.imgBtnPublish.TabIndex = 137;
            this.imgBtnPublish.TabStop = false;
            this.imgBtnPublish.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.imgBtnPublish.TipsColor = System.Drawing.Color.Purple;
            this.imgBtnPublish.TipsText = "Publish To Redis";
            this.imgBtnPublish.BtnClick += new System.EventHandler(this.ImgBtnPublish_BtnClick);
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
            this.msFileViews.Size = new System.Drawing.Size(1278, 1074);
            this.msFileViews.TabIndex = 125;
            // 
            // VendorFileSelectorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1522, 1191);
            this.Controls.Add(this.imgBtnPublish);
            this.Controls.Add(this.imgBtnClose);
            this.Controls.Add(this.imgBtnAdd);
            this.Controls.Add(this.btnUnpin);
            this.Controls.Add(this.btnPin);
            this.Controls.Add(this.btnFileRefresh);
            this.Controls.Add(this.msFileViews);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblConvertConfig);
            this.Controls.Add(this.cbConfig);
            this.Controls.Add(this.lblConfigOutputPath);
            this.Controls.Add(this.btnConfigChooseFolder);
            this.Controls.Add(this.tbOutputPath);
            this.Controls.Add(this.btnEditConfigs);
            this.Controls.Add(this.gBoxMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "VendorFileSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File/Folder Selector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VendorFileSelectorForm_FormClosing);
            this.Load += new System.EventHandler(this.VendorFileSelectorForm_Load);
            this.gBoxMode.ResumeLayout(false);
            this.gBoxMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_x)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixel_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mSIFileOrganisationBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private HZH_Controls.Controls.UCBtnImg imgBtnPublish;

        private System.Windows.Forms.Button btnPin;

        private System.Windows.Forms.Button btnUnpin;

        #endregion
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox gBoxMode;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Button btnEditConfigs;
        private System.Windows.Forms.Label lblConfigOutputPath;
        private System.Windows.Forms.Button btnConfigChooseFolder;
        public System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.ComboBox cbConfig;
        private System.Windows.Forms.Label lblConvertConfig;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imgList;
        private AirdPro.FolderFileBrowser msFileViews;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Button btnFileRefresh;
        private System.Windows.Forms.ToolTip btnTooltip;
        private HZH_Controls.Controls.UCBtnImg imgBtnAdd;
        private HZH_Controls.Controls.UCBtnImg imgBtnClose;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
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
    }
}