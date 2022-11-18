
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
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.betterFolderBrowser = new WK.Libraries.BetterFolderBrowserNS.BetterFolderBrowser(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnEditConfigs = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblConfigOutputPath = new System.Windows.Forms.Label();
            this.btnConfigChooseFolder = new System.Windows.Forms.Button();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.cbConfig = new System.Windows.Forms.ComboBox();
            this.lblConvertConfig = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.btnAdd = new System.Windows.Forms.Button();
            this.msFileViews = new AirdPro.FolderFileBrowser();
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
            this.gBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gBoxMode.Controls.Add(this.radioButton3);
            this.gBoxMode.Controls.Add(this.rbAuto);
            this.gBoxMode.Controls.Add(this.radioButton6);
            this.gBoxMode.Controls.Add(this.radioButton5);
            this.gBoxMode.Controls.Add(this.radioButton2);
            this.gBoxMode.Controls.Add(this.radioButton1);
            this.gBoxMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gBoxMode.Location = new System.Drawing.Point(646, 86);
            this.gBoxMode.Name = "gBoxMode";
            this.gBoxMode.Size = new System.Drawing.Size(208, 181);
            this.gBoxMode.TabIndex = 2;
            this.gBoxMode.TabStop = false;
            this.gBoxMode.Text = "Acquisition Mode";
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
            // betterFolderBrowser
            // 
            this.betterFolderBrowser.Multiselect = false;
            this.betterFolderBrowser.RootFolder = "C:";
            this.betterFolderBrowser.Title = "Please select a folder...";
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
            this.btnEditConfigs.Location = new System.Drawing.Point(320, 12);
            this.btnEditConfigs.Name = "btnEditConfigs";
            this.btnEditConfigs.Size = new System.Drawing.Size(75, 26);
            this.btnEditConfigs.TabIndex = 6;
            this.btnEditConfigs.Text = "Browser";
            this.btnEditConfigs.UseVisualStyleBackColor = true;
            this.btnEditConfigs.Click += new System.EventHandler(this.btnCreateConfigs_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(646, 357);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(205, 38);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblConfigOutputPath
            // 
            this.lblConfigOutputPath.AutoSize = true;
            this.lblConfigOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOutputPath.Location = new System.Drawing.Point(421, 15);
            this.lblConfigOutputPath.Name = "lblConfigOutputPath";
            this.lblConfigOutputPath.Size = new System.Drawing.Size(80, 17);
            this.lblConfigOutputPath.TabIndex = 119;
            this.lblConfigOutputPath.Text = "Output Path:";
            // 
            // btnConfigChooseFolder
            // 
            this.btnConfigChooseFolder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConfigChooseFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(785, 10);
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
            this.tbOutputPath.Location = new System.Drawing.Point(507, 12);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(272, 23);
            this.tbOutputPath.TabIndex = 118;
            // 
            // cbConfig
            // 
            this.cbConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfig.FormattingEnabled = true;
            this.cbConfig.Location = new System.Drawing.Point(131, 12);
            this.cbConfig.Name = "cbConfig";
            this.cbConfig.Size = new System.Drawing.Size(183, 25);
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
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(643, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 34);
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
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(646, 313);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(205, 38);
            this.btnAdd.TabIndex = 127;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // msFileViews
            // 
            this.msFileViews.Location = new System.Drawing.Point(6, 44);
            this.msFileViews.Name = "msFileViews";
            this.msFileViews.Size = new System.Drawing.Size(625, 351);
            this.msFileViews.TabIndex = 125;
            // 
            // VendorFileSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 407);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.msFileViews);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblConvertConfig);
            this.Controls.Add(this.cbConfig);
            this.Controls.Add(this.lblConfigOutputPath);
            this.Controls.Add(this.btnConfigChooseFolder);
            this.Controls.Add(this.tbOutputPath);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEditConfigs);
            this.Controls.Add(this.gBoxMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VendorFileSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "File/Folder Selector";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VendorFileSelectorForm_FormClosed);
            this.Load += new System.EventHandler(this.VendorFileSelectorForm_Load);
            this.gBoxMode.ResumeLayout(false);
            this.gBoxMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox gBoxMode;
        private System.Windows.Forms.RadioButton radioButton2;
        private WK.Libraries.BetterFolderBrowserNS.BetterFolderBrowser betterFolderBrowser;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Button btnEditConfigs;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblConfigOutputPath;
        private System.Windows.Forms.Button btnConfigChooseFolder;
        public System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.ComboBox cbConfig;
        private System.Windows.Forms.Label lblConvertConfig;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imgList;
        private FolderFileBrowser msFileViews;
        private System.Windows.Forms.Button btnAdd;
    }
}