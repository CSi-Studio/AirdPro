﻿
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
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btnAddAndContinue = new System.Windows.Forms.Button();
            this.btnFileSelector = new System.Windows.Forms.Button();
            this.tbPaths = new System.Windows.Forms.TextBox();
            this.betterFolderBrowser = new WK.Libraries.BetterFolderBrowserNS.BetterFolderBrowser(this.components);
            this.btnFolderSelector = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnEditConfigs = new System.Windows.Forms.Button();
            this.btnAddAndClose = new System.Windows.Forms.Button();
            this.lblConfigOutputPath = new System.Windows.Forms.Label();
            this.btnConfigChooseFolder = new System.Windows.Forms.Button();
            this.tbOutputPath = new System.Windows.Forms.TextBox();
            this.cbConfig = new System.Windows.Forms.ComboBox();
            this.lblConvertConfig = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
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
            this.gBoxMode.Location = new System.Drawing.Point(469, 12);
            this.gBoxMode.Name = "gBoxMode";
            this.gBoxMode.Size = new System.Drawing.Size(134, 175);
            this.gBoxMode.TabIndex = 2;
            this.gBoxMode.TabStop = false;
            this.gBoxMode.Text = "Acquisition Mode";
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
            // btnAddAndContinue
            // 
            this.btnAddAndContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAndContinue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddAndContinue.Location = new System.Drawing.Point(503, 408);
            this.btnAddAndContinue.Name = "btnAddAndContinue";
            this.btnAddAndContinue.Size = new System.Drawing.Size(172, 38);
            this.btnAddAndContinue.TabIndex = 3;
            this.btnAddAndContinue.Text = "Add and Continue";
            this.btnAddAndContinue.UseVisualStyleBackColor = true;
            this.btnAddAndContinue.Click += new System.EventHandler(this.btnAddAndContinue_Click);
            // 
            // btnFileSelector
            // 
            this.btnFileSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileSelector.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFileSelector.Location = new System.Drawing.Point(604, 338);
            this.btnFileSelector.Name = "btnFileSelector";
            this.btnFileSelector.Size = new System.Drawing.Size(250, 39);
            this.btnFileSelector.TabIndex = 4;
            this.btnFileSelector.Text = "Select Files(.wiff, .raw, mzML, mzXML)";
            this.btnFileSelector.UseVisualStyleBackColor = true;
            this.btnFileSelector.Click += new System.EventHandler(this.btnFileSelector_Click);
            // 
            // tbPaths
            // 
            this.tbPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPaths.Location = new System.Drawing.Point(2, 3);
            this.tbPaths.Multiline = true;
            this.tbPaths.Name = "tbPaths";
            this.tbPaths.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbPaths.Size = new System.Drawing.Size(461, 443);
            this.tbPaths.TabIndex = 0;
            // 
            // betterFolderBrowser
            // 
            this.betterFolderBrowser.Multiselect = false;
            this.betterFolderBrowser.RootFolder = "C:";
            this.betterFolderBrowser.Title = "Please select a folder...";
            // 
            // btnFolderSelector
            // 
            this.btnFolderSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderSelector.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFolderSelector.Location = new System.Drawing.Point(604, 293);
            this.btnFolderSelector.Name = "btnFolderSelector";
            this.btnFolderSelector.Size = new System.Drawing.Size(250, 39);
            this.btnFolderSelector.TabIndex = 5;
            this.btnFolderSelector.Text = "Select Folders(.d)";
            this.btnFolderSelector.UseVisualStyleBackColor = true;
            this.btnFolderSelector.Click += new System.EventHandler(this.btnFolderSelector_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Vendor Files|*.wiff;*.raw;*.mzML;*mzXML";
            this.openFileDialog.Multiselect = true;
            // 
            // btnEditConfigs
            // 
            this.btnEditConfigs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditConfigs.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEditConfigs.Location = new System.Drawing.Point(779, 214);
            this.btnEditConfigs.Name = "btnEditConfigs";
            this.btnEditConfigs.Size = new System.Drawing.Size(75, 26);
            this.btnEditConfigs.TabIndex = 6;
            this.btnEditConfigs.Text = "Browser";
            this.btnEditConfigs.UseVisualStyleBackColor = true;
            this.btnEditConfigs.Click += new System.EventHandler(this.btnCreateConfigs_Click);
            // 
            // btnAddAndClose
            // 
            this.btnAddAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAndClose.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddAndClose.Location = new System.Drawing.Point(681, 408);
            this.btnAddAndClose.Name = "btnAddAndClose";
            this.btnAddAndClose.Size = new System.Drawing.Size(181, 38);
            this.btnAddAndClose.TabIndex = 7;
            this.btnAddAndClose.Text = "Add";
            this.btnAddAndClose.UseVisualStyleBackColor = true;
            this.btnAddAndClose.Click += new System.EventHandler(this.btnAddAndClose_Click);
            // 
            // lblConfigOutputPath
            // 
            this.lblConfigOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigOutputPath.AutoSize = true;
            this.lblConfigOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOutputPath.Location = new System.Drawing.Point(480, 254);
            this.lblConfigOutputPath.Name = "lblConfigOutputPath";
            this.lblConfigOutputPath.Size = new System.Drawing.Size(80, 17);
            this.lblConfigOutputPath.TabIndex = 119;
            this.lblConfigOutputPath.Text = "Output Path:";
            // 
            // btnConfigChooseFolder
            // 
            this.btnConfigChooseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigChooseFolder.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnConfigChooseFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(779, 247);
            this.btnConfigChooseFolder.Name = "btnConfigChooseFolder";
            this.btnConfigChooseFolder.Size = new System.Drawing.Size(75, 26);
            this.btnConfigChooseFolder.TabIndex = 120;
            this.btnConfigChooseFolder.Text = "Browser";
            this.btnConfigChooseFolder.UseVisualStyleBackColor = true;
            this.btnConfigChooseFolder.Click += new System.EventHandler(this.btnConfigChooseFolder_Click);
            // 
            // tbOutputPath
            // 
            this.tbOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbOutputPath.Location = new System.Drawing.Point(574, 251);
            this.tbOutputPath.Name = "tbOutputPath";
            this.tbOutputPath.Size = new System.Drawing.Size(199, 23);
            this.tbOutputPath.TabIndex = 118;
            // 
            // cbConfig
            // 
            this.cbConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConfig.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbConfig.FormattingEnabled = true;
            this.cbConfig.Location = new System.Drawing.Point(590, 216);
            this.cbConfig.Name = "cbConfig";
            this.cbConfig.Size = new System.Drawing.Size(183, 25);
            this.cbConfig.TabIndex = 121;
            // 
            // lblConvertConfig
            // 
            this.lblConvertConfig.AutoSize = true;
            this.lblConvertConfig.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConvertConfig.Location = new System.Drawing.Point(468, 220);
            this.lblConvertConfig.Name = "lblConvertConfig";
            this.lblConvertConfig.Size = new System.Drawing.Size(118, 17);
            this.lblConvertConfig.TabIndex = 122;
            this.lblConvertConfig.Text = "Conversion Config:";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.label1.Location = new System.Drawing.Point(617, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 34);
            this.label1.TabIndex = 123;
            this.label1.Text = "If Your Acquisition Method is PRM,\r\nDo not Select the Auto Tag.";
            // 
            // VendorFileSelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblConvertConfig);
            this.Controls.Add(this.cbConfig);
            this.Controls.Add(this.lblConfigOutputPath);
            this.Controls.Add(this.btnConfigChooseFolder);
            this.Controls.Add(this.tbOutputPath);
            this.Controls.Add(this.btnAddAndClose);
            this.Controls.Add(this.btnEditConfigs);
            this.Controls.Add(this.btnFolderSelector);
            this.Controls.Add(this.btnFileSelector);
            this.Controls.Add(this.btnAddAndContinue);
            this.Controls.Add(this.gBoxMode);
            this.Controls.Add(this.tbPaths);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "VendorFileSelectorForm";
            this.Text = "File/Folder Selector";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VendorFileSelectorForm_FormClosed);
            this.Load += new System.EventHandler(this.CustomPathForm_Load);
            this.gBoxMode.ResumeLayout(false);
            this.gBoxMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox gBoxMode;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button btnAddAndContinue;
        private System.Windows.Forms.Button btnFileSelector;
        private System.Windows.Forms.TextBox tbPaths;
        private WK.Libraries.BetterFolderBrowserNS.BetterFolderBrowser betterFolderBrowser;
        private System.Windows.Forms.Button btnFolderSelector;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.Button btnEditConfigs;
        private System.Windows.Forms.Button btnAddAndClose;
        private System.Windows.Forms.Label lblConfigOutputPath;
        private System.Windows.Forms.Button btnConfigChooseFolder;
        public System.Windows.Forms.TextBox tbOutputPath;
        private System.Windows.Forms.ComboBox cbConfig;
        private System.Windows.Forms.Label lblConvertConfig;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label1;
    }
}