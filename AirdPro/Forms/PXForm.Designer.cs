﻿using System.Windows.Forms;

namespace AirdPro.Repository
{
    partial class PXForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PXForm));
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblConfigFolder = new System.Windows.Forms.Label();
            this.tbConfigFolder = new System.Windows.Forms.TextBox();
            this.btnChangeConfigFolder = new System.Windows.Forms.Button();
            this.lblRepos = new System.Windows.Forms.Label();
            this.tbRepos = new System.Windows.Forms.TextBox();
            this.btnChangeRepos = new System.Windows.Forms.Button();
            this.projectListView = new System.Windows.Forms.DataGridView();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Identifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Repos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Species = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Instrument = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Publication = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LabHead = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Announce = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Keywords = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnUrl = new System.Windows.Forms.Button();
            this.lblLoading = new System.Windows.Forms.Label();
            this.tbPXD = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDownloadLinks = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.projectListView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnLoad.Location = new System.Drawing.Point(1876, 14);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(224, 34);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load From Web";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblConfigFolder
            // 
            this.lblConfigFolder.AutoSize = true;
            this.lblConfigFolder.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblConfigFolder.Location = new System.Drawing.Point(20, 18);
            this.lblConfigFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigFolder.Name = "lblConfigFolder";
            this.lblConfigFolder.Size = new System.Drawing.Size(92, 21);
            this.lblConfigFolder.TabIndex = 1;
            this.lblConfigFolder.Text = "Config File";
            // 
            // tbConfigFolder
            // 
            this.tbConfigFolder.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbConfigFolder.Location = new System.Drawing.Point(123, 14);
            this.tbConfigFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbConfigFolder.Name = "tbConfigFolder";
            this.tbConfigFolder.Size = new System.Drawing.Size(547, 29);
            this.tbConfigFolder.TabIndex = 2;
            // 
            // btnChangeConfigFolder
            // 
            this.btnChangeConfigFolder.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnChangeConfigFolder.Location = new System.Drawing.Point(681, 14);
            this.btnChangeConfigFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChangeConfigFolder.Name = "btnChangeConfigFolder";
            this.btnChangeConfigFolder.Size = new System.Drawing.Size(112, 34);
            this.btnChangeConfigFolder.TabIndex = 3;
            this.btnChangeConfigFolder.Text = "Modify";
            this.btnChangeConfigFolder.UseVisualStyleBackColor = true;
            this.btnChangeConfigFolder.Click += new System.EventHandler(this.btnChangeConfigFolder_Click);
            // 
            // lblRepos
            // 
            this.lblRepos.AutoSize = true;
            this.lblRepos.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblRepos.Location = new System.Drawing.Point(826, 18);
            this.lblRepos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRepos.Name = "lblRepos";
            this.lblRepos.Size = new System.Drawing.Size(93, 21);
            this.lblRepos.TabIndex = 4;
            this.lblRepos.Text = "Local Repo";
            // 
            // tbRepos
            // 
            this.tbRepos.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbRepos.Location = new System.Drawing.Point(933, 15);
            this.tbRepos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbRepos.Name = "tbRepos";
            this.tbRepos.Size = new System.Drawing.Size(532, 29);
            this.tbRepos.TabIndex = 5;
            // 
            // btnChangeRepos
            // 
            this.btnChangeRepos.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnChangeRepos.Location = new System.Drawing.Point(1492, 12);
            this.btnChangeRepos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChangeRepos.Name = "btnChangeRepos";
            this.btnChangeRepos.Size = new System.Drawing.Size(112, 39);
            this.btnChangeRepos.TabIndex = 6;
            this.btnChangeRepos.Text = "Modify";
            this.btnChangeRepos.UseVisualStyleBackColor = true;
            this.btnChangeRepos.Click += new System.EventHandler(this.btnChangeRepos_Click);
            // 
            // projectListView
            // 
            this.projectListView.AllowUserToAddRows = false;
            this.projectListView.AllowUserToDeleteRows = false;
            this.projectListView.AllowUserToOrderColumns = true;
            this.projectListView.AllowUserToResizeRows = false;
            this.projectListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectListView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.projectListView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.projectListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.projectListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Title,
            this.Identifier,
            this.Repos,
            this.Species,
            this.Instrument,
            this.Publication,
            this.LabHead,
            this.Announce,
            this.Keywords});
            this.projectListView.Location = new System.Drawing.Point(8, 98);
            this.projectListView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.projectListView.Name = "projectListView";
            this.projectListView.RowHeadersWidth = 62;
            this.projectListView.RowTemplate.Height = 23;
            this.projectListView.Size = new System.Drawing.Size(2086, 984);
            this.projectListView.TabIndex = 7;
            this.projectListView.VirtualMode = true;
            // 
            // Title
            // 
            this.Title.DataPropertyName = "Title";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Title.DefaultCellStyle = dataGridViewCellStyle2;
            this.Title.FillWeight = 228.4264F;
            this.Title.HeaderText = "Title";
            this.Title.MinimumWidth = 8;
            this.Title.Name = "Title";
            // 
            // Identifier
            // 
            this.Identifier.DataPropertyName = "Identifier";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Identifier.DefaultCellStyle = dataGridViewCellStyle3;
            this.Identifier.FillWeight = 67.53138F;
            this.Identifier.HeaderText = "Identifier";
            this.Identifier.MinimumWidth = 8;
            this.Identifier.Name = "Identifier";
            // 
            // Repos
            // 
            this.Repos.DataPropertyName = "Repos";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Repos.DefaultCellStyle = dataGridViewCellStyle4;
            this.Repos.FillWeight = 62.21808F;
            this.Repos.HeaderText = "Repo";
            this.Repos.MinimumWidth = 8;
            this.Repos.Name = "Repos";
            // 
            // Species
            // 
            this.Species.DataPropertyName = "Species";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Species.DefaultCellStyle = dataGridViewCellStyle5;
            this.Species.FillWeight = 100.1048F;
            this.Species.HeaderText = "Species";
            this.Species.MinimumWidth = 8;
            this.Species.Name = "Species";
            // 
            // Instrument
            // 
            this.Instrument.DataPropertyName = "Instrument";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Instrument.DefaultCellStyle = dataGridViewCellStyle6;
            this.Instrument.FillWeight = 104.9039F;
            this.Instrument.HeaderText = "Instrument";
            this.Instrument.MinimumWidth = 8;
            this.Instrument.Name = "Instrument";
            // 
            // Publication
            // 
            this.Publication.DataPropertyName = "Publication";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Publication.DefaultCellStyle = dataGridViewCellStyle7;
            this.Publication.FillWeight = 64.53849F;
            this.Publication.HeaderText = "Publication";
            this.Publication.MinimumWidth = 8;
            this.Publication.Name = "Publication";
            // 
            // LabHead
            // 
            this.LabHead.DataPropertyName = "LabHead";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.LabHead.DefaultCellStyle = dataGridViewCellStyle8;
            this.LabHead.FillWeight = 69.94061F;
            this.LabHead.HeaderText = "Lab Head";
            this.LabHead.MinimumWidth = 8;
            this.LabHead.Name = "LabHead";
            // 
            // Announce
            // 
            this.Announce.DataPropertyName = "Announce";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Announce.DefaultCellStyle = dataGridViewCellStyle9;
            this.Announce.FillWeight = 66.85007F;
            this.Announce.HeaderText = "Date";
            this.Announce.MinimumWidth = 8;
            this.Announce.Name = "Announce";
            // 
            // Keywords
            // 
            this.Keywords.DataPropertyName = "Keywords";
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.Keywords.DefaultCellStyle = dataGridViewCellStyle10;
            this.Keywords.FillWeight = 135.4862F;
            this.Keywords.HeaderText = "Keywords";
            this.Keywords.MinimumWidth = 8;
            this.Keywords.Name = "Keywords";
            // 
            // tbSearch
            // 
            this.tbSearch.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbSearch.Location = new System.Drawing.Point(18, 54);
            this.tbSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(756, 29);
            this.tbSearch.TabIndex = 8;
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnSearch.Location = new System.Drawing.Point(784, 52);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(165, 38);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnReset.Location = new System.Drawing.Point(958, 52);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(116, 39);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblResult.Location = new System.Drawing.Point(1872, 58);
            this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(70, 21);
            this.lblResult.TabIndex = 11;
            this.lblResult.Text = "Records";
            // 
            // btnUrl
            // 
            this.btnUrl.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnUrl.Location = new System.Drawing.Point(1083, 54);
            this.btnUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUrl.Name = "btnUrl";
            this.btnUrl.Size = new System.Drawing.Size(111, 38);
            this.btnUrl.TabIndex = 13;
            this.btnUrl.Text = "Web";
            this.btnUrl.UseVisualStyleBackColor = true;
            this.btnUrl.Click += new System.EventHandler(this.btnUrl_Click);
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.lblLoading.Location = new System.Drawing.Point(1653, 20);
            this.lblLoading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(66, 21);
            this.lblLoading.TabIndex = 14;
            this.lblLoading.Text = "Loaded";
            // 
            // tbPXD
            // 
            this.tbPXD.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.tbPXD.Location = new System.Drawing.Point(1570, 56);
            this.tbPXD.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPXD.Name = "tbPXD";
            this.tbPXD.Size = new System.Drawing.Size(272, 29);
            this.tbPXD.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.label1.Location = new System.Drawing.Point(1492, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 21);
            this.label1.TabIndex = 17;
            this.label1.Text = "PXD ID:";
            // 
            // btnDownloadLinks
            // 
            this.btnDownloadLinks.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnDownloadLinks.Location = new System.Drawing.Point(1203, 52);
            this.btnDownloadLinks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDownloadLinks.Name = "btnDownloadLinks";
            this.btnDownloadLinks.Size = new System.Drawing.Size(264, 38);
            this.btnDownloadLinks.TabIndex = 18;
            this.btnDownloadLinks.Text = "Download Links";
            this.btnDownloadLinks.UseVisualStyleBackColor = true;
            this.btnDownloadLinks.Click += new System.EventHandler(this.btnDownloadLinks_Click);
            // 
            // PXForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2110, 1094);
            this.Controls.Add(this.btnDownloadLinks);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPXD);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.btnUrl);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.projectListView);
            this.Controls.Add(this.btnChangeRepos);
            this.Controls.Add(this.tbRepos);
            this.Controls.Add(this.lblRepos);
            this.Controls.Add(this.btnChangeConfigFolder);
            this.Controls.Add(this.tbConfigFolder);
            this.Controls.Add(this.lblConfigFolder);
            this.Controls.Add(this.btnLoad);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PXForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProteomeXchange";
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.projectListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnDownloadLinks;

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblConfigFolder;
        private System.Windows.Forms.TextBox tbConfigFolder;
        private System.Windows.Forms.Button btnChangeConfigFolder;
        private System.Windows.Forms.Label lblRepos;
        private System.Windows.Forms.TextBox tbRepos;
        private System.Windows.Forms.Button btnChangeRepos;
        private System.Windows.Forms.DataGridView projectListView;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnUrl;
        private Label lblLoading;
        private System.Windows.Forms.TextBox tbPXD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Identifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Repos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Species;
        private System.Windows.Forms.DataGridViewTextBoxColumn Instrument;
        private System.Windows.Forms.DataGridViewTextBoxColumn Publication;
        private System.Windows.Forms.DataGridViewTextBoxColumn LabHead;
        private System.Windows.Forms.DataGridViewTextBoxColumn Announce;
        private System.Windows.Forms.DataGridViewTextBoxColumn Keywords;
    }
}

