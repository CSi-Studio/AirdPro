namespace AirdPro.Repository
{
    partial class DownloadDetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadDetailForm));
            this.lvFileList = new System.Windows.Forms.ListView();
            this.headerRemotePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerLocalPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAsync = new System.Windows.Forms.Button();
            this.tbRemote = new System.Windows.Forms.TextBox();
            this.tbHome = new System.Windows.Forms.TextBox();
            this.lblDowload = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvFileList
            // 
            this.lvFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerRemotePath,
            this.headerFileName,
            this.headerLocalPath,
            this.headerStatus,
            this.headerSize});
            this.lvFileList.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lvFileList.GridLines = true;
            this.lvFileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileList.HideSelection = false;
            this.lvFileList.Location = new System.Drawing.Point(0, 61);
            this.lvFileList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.Size = new System.Drawing.Size(967, 648);
            this.lvFileList.TabIndex = 0;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            // 
            // headerRemotePath
            // 
            this.headerRemotePath.Text = "Remote";
            this.headerRemotePath.Width = 200;
            // 
            // headerFileName
            // 
            this.headerFileName.Text = "File Name";
            this.headerFileName.Width = 300;
            // 
            // headerLocalPath
            // 
            this.headerLocalPath.Text = "Local";
            this.headerLocalPath.Width = 200;
            // 
            // headerStatus
            // 
            this.headerStatus.Text = "Status";
            this.headerStatus.Width = 150;
            // 
            // headerSize
            // 
            this.headerSize.Text = "File Size";
            this.headerSize.Width = 200;
            // 
            // btnAsync
            // 
            this.btnAsync.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnAsync.Location = new System.Drawing.Point(835, 3);
            this.btnAsync.Name = "btnAsync";
            this.btnAsync.Size = new System.Drawing.Size(127, 26);
            this.btnAsync.TabIndex = 2;
            this.btnAsync.Text = "Start Async";
            this.btnAsync.UseVisualStyleBackColor = true;
            this.btnAsync.Click += new System.EventHandler(this.btnAsync_Click);
            // 
            // tbRemote
            // 
            this.tbRemote.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbRemote.Location = new System.Drawing.Point(97, 6);
            this.tbRemote.Name = "tbRemote";
            this.tbRemote.ReadOnly = true;
            this.tbRemote.Size = new System.Drawing.Size(726, 22);
            this.tbRemote.TabIndex = 3;
            // 
            // tbHome
            // 
            this.tbHome.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbHome.Location = new System.Drawing.Point(97, 34);
            this.tbHome.Name = "tbHome";
            this.tbHome.ReadOnly = true;
            this.tbHome.Size = new System.Drawing.Size(726, 22);
            this.tbHome.TabIndex = 4;
            // 
            // lblDowload
            // 
            this.lblDowload.AutoSize = true;
            this.lblDowload.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblDowload.Location = new System.Drawing.Point(5, 11);
            this.lblDowload.Name = "lblDowload";
            this.lblDowload.Size = new System.Drawing.Size(61, 16);
            this.lblDowload.TabIndex = 5;
            this.lblDowload.Text = "Download";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label1.Location = new System.Drawing.Point(5, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "HomePage";
            // 
            // DownloadDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 707);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDowload);
            this.Controls.Add(this.tbHome);
            this.Controls.Add(this.tbRemote);
            this.Controls.Add(this.btnAsync);
            this.Controls.Add(this.lvFileList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DownloadDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Async File List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DetailForm_FormClosed);
            this.Load += new System.EventHandler(this.DetailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.ColumnHeader headerRemotePath;
        private System.Windows.Forms.ColumnHeader headerFileName;
        private System.Windows.Forms.ColumnHeader headerLocalPath;
        private System.Windows.Forms.ColumnHeader headerStatus;
        private System.Windows.Forms.ColumnHeader headerSize;
        private System.Windows.Forms.Button btnAsync;
        private System.Windows.Forms.TextBox tbRemote;
        private System.Windows.Forms.TextBox tbHome;
        private System.Windows.Forms.Label lblDowload;
        private System.Windows.Forms.Label label1;
    }
}