
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
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
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
            this.msFileViews = new AirdPro.FolderFileBrowser();
            this.btnPin = new System.Windows.Forms.Button();
            this.btnUnpin = new System.Windows.Forms.Button();
            this.btnTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.imgBtnAdd = new HZH_Controls.Controls.UCBtnImg();
            this.imgBtnClose = new HZH_Controls.Controls.UCBtnImg();
            this.gBoxMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton1.Location = new System.Drawing.Point(8, 51);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(52, 21);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "DDA";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // gBoxMode
            // 
            this.gBoxMode.Controls.Add(this.radioButton4);
            this.gBoxMode.Controls.Add(this.radioButton3);
            this.gBoxMode.Controls.Add(this.rbAuto);
            this.gBoxMode.Controls.Add(this.radioButton6);
            this.gBoxMode.Controls.Add(this.radioButton5);
            this.gBoxMode.Controls.Add(this.radioButton2);
            this.gBoxMode.Controls.Add(this.radioButton1);
            this.gBoxMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gBoxMode.Location = new System.Drawing.Point(604, 128);
            this.gBoxMode.Name = "gBoxMode";
            this.gBoxMode.Size = new System.Drawing.Size(142, 206);
            this.gBoxMode.TabIndex = 2;
            this.gBoxMode.TabStop = false;
            this.gBoxMode.Text = "Acquisition Mode";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton4.Location = new System.Drawing.Point(8, 177);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(58, 21);
            this.radioButton4.TabIndex = 9;
            this.radioButton4.Text = "MRM";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton3.Location = new System.Drawing.Point(8, 151);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(53, 21);
            this.radioButton3.TabIndex = 8;
            this.radioButton3.Text = "PRM";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.Checked = true;
            this.rbAuto.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbAuto.Location = new System.Drawing.Point(8, 27);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(53, 21);
            this.rbAuto.TabIndex = 7;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Auto";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton6.Location = new System.Drawing.Point(8, 77);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(92, 21);
            this.radioButton6.TabIndex = 6;
            this.radioButton6.Text = "DDA_PASEF";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton5.Location = new System.Drawing.Point(8, 126);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(87, 21);
            this.radioButton5.TabIndex = 5;
            this.radioButton5.Text = "DIA_PASEF";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton2.Location = new System.Drawing.Point(8, 102);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 21);
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
            this.btnEditConfigs.Location = new System.Drawing.Point(292, 12);
            this.btnEditConfigs.Name = "btnEditConfigs";
            this.btnEditConfigs.Size = new System.Drawing.Size(75, 26);
            this.btnEditConfigs.TabIndex = 6;
            this.btnEditConfigs.Text = "Browser";
            this.btnEditConfigs.UseVisualStyleBackColor = true;
            this.btnEditConfigs.Click += new System.EventHandler(this.btnCreateConfigs_Click);
            // 
            // lblConfigOutputPath
            // 
            this.lblConfigOutputPath.AutoSize = true;
            this.lblConfigOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOutputPath.Location = new System.Drawing.Point(384, 17);
            this.lblConfigOutputPath.Name = "lblConfigOutputPath";
            this.lblConfigOutputPath.Size = new System.Drawing.Size(80, 17);
            this.lblConfigOutputPath.TabIndex = 119;
            this.lblConfigOutputPath.Text = "Output Path:";
            // 
            // btnConfigChooseFolder
            // 
            this.btnConfigChooseFolder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConfigChooseFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(670, 14);
            this.btnConfigChooseFolder.Name = "btnConfigChooseFolder";
            this.btnConfigChooseFolder.Size = new System.Drawing.Size(75, 26);
            this.btnConfigChooseFolder.TabIndex = 120;
            this.btnConfigChooseFolder.Text = "Browser";
            this.btnConfigChooseFolder.UseVisualStyleBackColor = true;
            this.btnConfigChooseFolder.Click += new System.EventHandler(this.btnConfigChooseFolder_Click);
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbOutputPath.Location = new System.Drawing.Point(462, 14);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(202, 23);
            this.tbOutputPath.TabIndex = 118;
            // 
            // cbConfig
            // 
            this.cbConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfig.FormattingEnabled = true;
            this.cbConfig.Location = new System.Drawing.Point(131, 12);
            this.cbConfig.Name = "cbConfig";
            this.cbConfig.Size = new System.Drawing.Size(155, 25);
            this.cbConfig.TabIndex = 121;
            // 
            // lblConvertConfig
            // 
            this.lblConvertConfig.AutoSize = true;
            this.lblConvertConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConvertConfig.Location = new System.Drawing.Point(9, 16);
            this.lblConvertConfig.Name = "lblConvertConfig";
            this.lblConvertConfig.Size = new System.Drawing.Size(118, 17);
            this.lblConvertConfig.TabIndex = 122;
            this.lblConvertConfig.Text = "Conversion Config:";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(606, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 75);
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
            this.btnFileRefresh.Location = new System.Drawing.Point(10, 43);
            this.btnFileRefresh.Name = "btnFileRefresh";
            this.btnFileRefresh.Size = new System.Drawing.Size(30, 30);
            this.btnFileRefresh.TabIndex = 132;
            this.btnTooltip.SetToolTip(this.btnFileRefresh, "refresh the selected folder");
            this.btnFileRefresh.UseVisualStyleBackColor = true;
            this.btnFileRefresh.Click += new System.EventHandler(this.btnFileRefresh_Click);
            // 
            // msFileViews
            // 
            this.msFileViews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.msFileViews.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.msFileViews.Location = new System.Drawing.Point(6, 76);
            this.msFileViews.Margin = new System.Windows.Forms.Padding(4);
            this.msFileViews.Name = "msFileViews";
            this.msFileViews.Size = new System.Drawing.Size(592, 354);
            this.msFileViews.TabIndex = 125;
            // 
            // btnPin
            // 
            this.btnPin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPin.BackgroundImage")));
            this.btnPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPin.Location = new System.Drawing.Point(46, 43);
            this.btnPin.Name = "btnPin";
            this.btnPin.Size = new System.Drawing.Size(30, 30);
            this.btnPin.TabIndex = 133;
            this.btnTooltip.SetToolTip(this.btnPin, "Pin the selected folder");
            this.btnPin.UseVisualStyleBackColor = true;
            this.btnPin.Click += new System.EventHandler(this.btnPin_Click);
            // 
            // btnUnpin
            // 
            this.btnUnpin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnUnpin.BackgroundImage")));
            this.btnUnpin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnUnpin.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUnpin.Location = new System.Drawing.Point(82, 43);
            this.btnUnpin.Name = "btnUnpin";
            this.btnUnpin.Size = new System.Drawing.Size(30, 30);
            this.btnUnpin.TabIndex = 134;
            this.btnTooltip.SetToolTip(this.btnUnpin, "Unpin the selected folder");
            this.btnUnpin.UseVisualStyleBackColor = true;
            this.btnUnpin.Click += new System.EventHandler(this.btnUnpin_Click);
            // 
            // imgBtnAdd
            // 
            this.imgBtnAdd.BackColor = System.Drawing.Color.Transparent;
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
            this.imgBtnAdd.Location = new System.Drawing.Point(603, 337);
            this.imgBtnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnAdd.Name = "imgBtnAdd";
            this.imgBtnAdd.RectColor = System.Drawing.Color.Silver;
            this.imgBtnAdd.RectWidth = 1;
            this.imgBtnAdd.Size = new System.Drawing.Size(142, 38);
            this.imgBtnAdd.TabIndex = 135;
            this.imgBtnAdd.TabStop = false;
            this.imgBtnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.imgBtnAdd.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.imgBtnAdd.TipsText = "";
            this.imgBtnAdd.BtnClick += new System.EventHandler(this.imgBtnAdd_BtnClick);
            // 
            // imgBtnClose
            // 
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
            this.imgBtnClose.Location = new System.Drawing.Point(603, 380);
            this.imgBtnClose.Margin = new System.Windows.Forms.Padding(0);
            this.imgBtnClose.Name = "imgBtnClose";
            this.imgBtnClose.RectColor = System.Drawing.Color.Silver;
            this.imgBtnClose.RectWidth = 1;
            this.imgBtnClose.Size = new System.Drawing.Size(142, 38);
            this.imgBtnClose.TabIndex = 136;
            this.imgBtnClose.TabStop = false;
            this.imgBtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.imgBtnClose.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.imgBtnClose.TipsText = "";
            this.imgBtnClose.BtnClick += new System.EventHandler(this.imgBtnClose_BtnClick);
            // 
            // VendorFileSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 432);
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
            this.Name = "VendorFileSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File/Folder Selector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VendorFileSelectorForm_FormClosing);
            this.Load += new System.EventHandler(this.VendorFileSelectorForm_Load);
            this.gBoxMode.ResumeLayout(false);
            this.gBoxMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
    }
}