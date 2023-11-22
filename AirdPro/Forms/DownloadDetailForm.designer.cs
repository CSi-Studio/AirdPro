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
            this.headerRemotePath = new System.Windows.Forms.ColumnHeader();
            this.headerFileName = new System.Windows.Forms.ColumnHeader();
            this.headerLocalPath = new System.Windows.Forms.ColumnHeader();
            this.headerStatus = new System.Windows.Forms.ColumnHeader();
            this.headerSize = new System.Windows.Forms.ColumnHeader();
            this.headerFileType = new System.Windows.Forms.ColumnHeader();
            this.btnDownload = new System.Windows.Forms.Button();
            this.tbRemote = new System.Windows.Forms.TextBox();
            this.tbHome = new System.Windows.Forms.TextBox();
            this.lblDowload = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbMzData = new System.Windows.Forms.CheckBox();
            this.lblSkip = new System.Windows.Forms.Label();
            this.cbMzML = new System.Windows.Forms.CheckBox();
            this.cbMzXML = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvFileList
            // 
            this.lvFileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.headerRemotePath, this.headerFileName, this.headerLocalPath, this.headerStatus, this.headerSize, this.headerFileType });
            this.lvFileList.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lvFileList.GridLines = true;
            this.lvFileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileList.HideSelection = false;
            this.lvFileList.Location = new System.Drawing.Point(0, 90);
            this.lvFileList.Margin = new System.Windows.Forms.Padding(2);
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.Size = new System.Drawing.Size(842, 619);
            this.lvFileList.TabIndex = 0;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            // 
            // headerRemotePath
            // 
            this.headerRemotePath.Text = "Remote";
            this.headerRemotePath.Width = 100;
            // 
            // headerFileName
            // 
            this.headerFileName.Text = "File Name";
            this.headerFileName.Width = 300;
            // 
            // headerLocalPath
            // 
            this.headerLocalPath.Text = "Local";
            this.headerLocalPath.Width = 100;
            // 
            // headerStatus
            // 
            this.headerStatus.Text = "Status";
            this.headerStatus.Width = 100;
            // 
            // headerSize
            // 
            this.headerSize.Text = "File Size";
            this.headerSize.Width = 150;
            // 
            // headerFileType
            // 
            this.headerFileType.Text = "Type";
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnDownload.Location = new System.Drawing.Point(707, 4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(127, 26);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // tbRemote
            // 
            this.tbRemote.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbRemote.Location = new System.Drawing.Point(97, 6);
            this.tbRemote.Name = "tbRemote";
            this.tbRemote.ReadOnly = true;
            this.tbRemote.Size = new System.Drawing.Size(604, 22);
            this.tbRemote.TabIndex = 3;
            // 
            // tbHome
            // 
            this.tbHome.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbHome.Location = new System.Drawing.Point(97, 34);
            this.tbHome.Name = "tbHome";
            this.tbHome.ReadOnly = true;
            this.tbHome.Size = new System.Drawing.Size(501, 22);
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
            // cbMzData
            // 
            this.cbMzData.Checked = true;
            this.cbMzData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMzData.Location = new System.Drawing.Point(97, 63);
            this.cbMzData.Name = "cbMzData";
            this.cbMzData.Size = new System.Drawing.Size(62, 24);
            this.cbMzData.TabIndex = 7;
            this.cbMzData.Text = "mzData";
            this.cbMzData.UseVisualStyleBackColor = true;
            // 
            // lblSkip
            // 
            this.lblSkip.AutoSize = true;
            this.lblSkip.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblSkip.Location = new System.Drawing.Point(5, 66);
            this.lblSkip.Name = "lblSkip";
            this.lblSkip.Size = new System.Drawing.Size(69, 16);
            this.lblSkip.TabIndex = 8;
            this.lblSkip.Text = "Skip Format";
            // 
            // cbMzML
            // 
            this.cbMzML.Checked = true;
            this.cbMzML.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMzML.Location = new System.Drawing.Point(165, 63);
            this.cbMzML.Name = "cbMzML";
            this.cbMzML.Size = new System.Drawing.Size(62, 24);
            this.cbMzML.TabIndex = 9;
            this.cbMzML.Text = "mzML";
            this.cbMzML.UseVisualStyleBackColor = true;
            // 
            // cbMzXML
            // 
            this.cbMzXML.Checked = true;
            this.cbMzXML.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMzXML.Location = new System.Drawing.Point(223, 63);
            this.cbMzXML.Name = "cbMzXML";
            this.cbMzXML.Size = new System.Drawing.Size(62, 24);
            this.cbMzXML.TabIndex = 10;
            this.cbMzXML.Text = "mzXML";
            this.cbMzXML.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label2.Location = new System.Drawing.Point(604, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Download Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblStatus.Location = new System.Drawing.Point(709, 38);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 16);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Waiting";
            // 
            // DownloadDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 707);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbMzXML);
            this.Controls.Add(this.cbMzML);
            this.Controls.Add(this.lblSkip);
            this.Controls.Add(this.cbMzData);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDowload);
            this.Controls.Add(this.tbHome);
            this.Controls.Add(this.tbRemote);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lvFileList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DownloadDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Async File List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DetailForm_FormClosed);
            this.Load += new System.EventHandler(this.DetailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.ColumnHeader headerFileType;

        private System.Windows.Forms.CheckBox cbMzML;
        private System.Windows.Forms.CheckBox cbMzXML;

        private System.Windows.Forms.CheckBox cbMzData;
        private System.Windows.Forms.Label lblSkip;

        #endregion

        private System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.ColumnHeader headerRemotePath;
        private System.Windows.Forms.ColumnHeader headerFileName;
        private System.Windows.Forms.ColumnHeader headerLocalPath;
        private System.Windows.Forms.ColumnHeader headerStatus;
        private System.Windows.Forms.ColumnHeader headerSize;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox tbRemote;
        private System.Windows.Forms.TextBox tbHome;
        private System.Windows.Forms.Label lblDowload;
        private System.Windows.Forms.Label label1;
    }
}