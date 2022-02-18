
namespace AirdPro.Forms
{
    partial class FileFolderSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileFolderSelector));
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.gBoxMode = new System.Windows.Forms.GroupBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnFileSelector = new System.Windows.Forms.Button();
            this.tbPaths = new System.Windows.Forms.TextBox();
            this.betterFolderBrowser = new WK.Libraries.BetterFolderBrowserNS.BetterFolderBrowser(this.components);
            this.btnFolderSelector = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.gBoxMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton1.Location = new System.Drawing.Point(8, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(52, 21);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "DDA";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // gBoxMode
            // 
            this.gBoxMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gBoxMode.Controls.Add(this.radioButton6);
            this.gBoxMode.Controls.Add(this.radioButton5);
            this.gBoxMode.Controls.Add(this.radioButton4);
            this.gBoxMode.Controls.Add(this.radioButton3);
            this.gBoxMode.Controls.Add(this.radioButton2);
            this.gBoxMode.Controls.Add(this.radioButton1);
            this.gBoxMode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gBoxMode.Location = new System.Drawing.Point(713, 3);
            this.gBoxMode.Name = "gBoxMode";
            this.gBoxMode.Size = new System.Drawing.Size(152, 197);
            this.gBoxMode.TabIndex = 2;
            this.gBoxMode.TabStop = false;
            this.gBoxMode.Text = "Acquisition Mode";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton4.Location = new System.Drawing.Point(8, 161);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(79, 21);
            this.radioButton4.TabIndex = 4;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Common";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton3.Location = new System.Drawing.Point(8, 139);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(53, 21);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "PRM";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton2.Location = new System.Drawing.Point(8, 76);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 21);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "DIA";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(712, 408);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(152, 38);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnFileSelector
            // 
            this.btnFileSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileSelector.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFileSelector.Location = new System.Drawing.Point(712, 355);
            this.btnFileSelector.Name = "btnFileSelector";
            this.btnFileSelector.Size = new System.Drawing.Size(152, 39);
            this.btnFileSelector.TabIndex = 4;
            this.btnFileSelector.Text = "Select Files";
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
            this.tbPaths.Size = new System.Drawing.Size(705, 443);
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
            this.btnFolderSelector.Location = new System.Drawing.Point(712, 315);
            this.btnFolderSelector.Name = "btnFolderSelector";
            this.btnFolderSelector.Size = new System.Drawing.Size(152, 39);
            this.btnFolderSelector.TabIndex = 5;
            this.btnFolderSelector.Text = "Select Folders";
            this.btnFolderSelector.UseVisualStyleBackColor = true;
            this.btnFolderSelector.Click += new System.EventHandler(this.btnFolderSelector_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Vendor Files|*.wiff;*raw";
            this.openFileDialog.Multiselect = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton5.Location = new System.Drawing.Point(8, 100);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(87, 21);
            this.radioButton5.TabIndex = 5;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "DIA_PASEF";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton6.Location = new System.Drawing.Point(8, 44);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(92, 21);
            this.radioButton6.TabIndex = 6;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "DDA_PASEF";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // FileFolderSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 450);
            this.Controls.Add(this.btnFolderSelector);
            this.Controls.Add(this.btnFileSelector);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gBoxMode);
            this.Controls.Add(this.tbPaths);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FileFolderSelector";
            this.Text = "File/Folder Selector";
            this.Load += new System.EventHandler(this.CustomPathForm_Load);
            this.gBoxMode.ResumeLayout(false);
            this.gBoxMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox gBoxMode;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnFileSelector;
        private System.Windows.Forms.TextBox tbPaths;
        private WK.Libraries.BetterFolderBrowserNS.BetterFolderBrowser betterFolderBrowser;
        private System.Windows.Forms.Button btnFolderSelector;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
    }
}