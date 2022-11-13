using System.Windows.Forms;

namespace AirdPro.Repository
{
    partial class MLForm
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MLForm));
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblConfigFolder = new System.Windows.Forms.Label();
            this.tbConfigFolder = new System.Windows.Forms.TextBox();
            this.btnChangeConfigFolder = new System.Windows.Forms.Button();
            this.lblRepos = new System.Windows.Forms.Label();
            this.tbRepos = new System.Windows.Forms.TextBox();
            this.btnChangeRepos = new System.Windows.Forms.Button();
            this.projectListView = new System.Windows.Forms.DataGridView();
            this.Identifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.btnDetail = new System.Windows.Forms.Button();
            this.btnUrl = new System.Windows.Forms.Button();
            this.lblLoading = new System.Windows.Forms.Label();
            this.btnFastLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.projectListView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoad.Location = new System.Drawing.Point(2040, 12);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(224, 44);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load From Web";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblConfigFolder
            // 
            this.lblConfigFolder.AutoSize = true;
            this.lblConfigFolder.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigFolder.Location = new System.Drawing.Point(20, 18);
            this.lblConfigFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigFolder.Name = "lblConfigFolder";
            this.lblConfigFolder.Size = new System.Drawing.Size(135, 31);
            this.lblConfigFolder.TabIndex = 1;
            this.lblConfigFolder.Text = "Config File";
            // 
            // tbConfigFolder
            // 
            this.tbConfigFolder.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbConfigFolder.Location = new System.Drawing.Point(188, 12);
            this.tbConfigFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbConfigFolder.Name = "tbConfigFolder";
            this.tbConfigFolder.Size = new System.Drawing.Size(685, 39);
            this.tbConfigFolder.TabIndex = 2;
            // 
            // btnChangeConfigFolder
            // 
            this.btnChangeConfigFolder.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChangeConfigFolder.Location = new System.Drawing.Point(879, 10);
            this.btnChangeConfigFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChangeConfigFolder.Name = "btnChangeConfigFolder";
            this.btnChangeConfigFolder.Size = new System.Drawing.Size(112, 46);
            this.btnChangeConfigFolder.TabIndex = 3;
            this.btnChangeConfigFolder.Text = "Modify";
            this.btnChangeConfigFolder.UseVisualStyleBackColor = true;
            this.btnChangeConfigFolder.Click += new System.EventHandler(this.btnChangeConfigFolder_Click);
            // 
            // lblRepos
            // 
            this.lblRepos.AutoSize = true;
            this.lblRepos.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRepos.Location = new System.Drawing.Point(1040, 16);
            this.lblRepos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRepos.Name = "lblRepos";
            this.lblRepos.Size = new System.Drawing.Size(139, 31);
            this.lblRepos.TabIndex = 4;
            this.lblRepos.Text = "Local Repo";
            // 
            // tbRepos
            // 
            this.tbRepos.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRepos.Location = new System.Drawing.Point(1202, 12);
            this.tbRepos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbRepos.Name = "tbRepos";
            this.tbRepos.Size = new System.Drawing.Size(685, 39);
            this.tbRepos.TabIndex = 5;
            // 
            // btnChangeRepos
            // 
            this.btnChangeRepos.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChangeRepos.Location = new System.Drawing.Point(1894, 10);
            this.btnChangeRepos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChangeRepos.Name = "btnChangeRepos";
            this.btnChangeRepos.Size = new System.Drawing.Size(112, 46);
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.projectListView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.projectListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.projectListView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Identifier,
            this.Title,
            this.Repos,
            this.Species,
            this.Instrument,
            this.Publication,
            this.LabHead,
            this.Announce,
            this.Keywords});
            this.projectListView.Location = new System.Drawing.Point(14, 124);
            this.projectListView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.projectListView.Name = "projectListView";
            this.projectListView.RowHeadersWidth = 62;
            this.projectListView.RowTemplate.Height = 23;
            this.projectListView.Size = new System.Drawing.Size(2415, 1026);
            this.projectListView.TabIndex = 7;
            this.projectListView.VirtualMode = true;
            // 
            // Identifier
            // 
            this.Identifier.DataPropertyName = "Identifier";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Identifier.DefaultCellStyle = dataGridViewCellStyle2;
            this.Identifier.FillWeight = 67.53138F;
            this.Identifier.HeaderText = "Identifier";
            this.Identifier.MinimumWidth = 8;
            this.Identifier.Name = "Identifier";
            // 
            // Title
            // 
            this.Title.DataPropertyName = "Title";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Title.DefaultCellStyle = dataGridViewCellStyle3;
            this.Title.FillWeight = 228.4264F;
            this.Title.HeaderText = "Title";
            this.Title.MinimumWidth = 8;
            this.Title.Name = "Title";
            // 
            // Repos
            // 
            this.Repos.DataPropertyName = "Repos";
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Repos.DefaultCellStyle = dataGridViewCellStyle4;
            this.Repos.FillWeight = 62.21808F;
            this.Repos.HeaderText = "Repo";
            this.Repos.MinimumWidth = 8;
            this.Repos.Name = "Repos";
            // 
            // Species
            // 
            this.Species.DataPropertyName = "Species";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Species.DefaultCellStyle = dataGridViewCellStyle5;
            this.Species.FillWeight = 100.1048F;
            this.Species.HeaderText = "Species";
            this.Species.MinimumWidth = 8;
            this.Species.Name = "Species";
            // 
            // Instrument
            // 
            this.Instrument.DataPropertyName = "Instrument";
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Instrument.DefaultCellStyle = dataGridViewCellStyle6;
            this.Instrument.FillWeight = 104.9039F;
            this.Instrument.HeaderText = "Instrument";
            this.Instrument.MinimumWidth = 8;
            this.Instrument.Name = "Instrument";
            // 
            // Publication
            // 
            this.Publication.DataPropertyName = "Publication";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Publication.DefaultCellStyle = dataGridViewCellStyle7;
            this.Publication.FillWeight = 64.53849F;
            this.Publication.HeaderText = "Publication";
            this.Publication.MinimumWidth = 8;
            this.Publication.Name = "Publication";
            // 
            // LabHead
            // 
            this.LabHead.DataPropertyName = "LabHead";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.LabHead.DefaultCellStyle = dataGridViewCellStyle8;
            this.LabHead.FillWeight = 69.94061F;
            this.LabHead.HeaderText = "Lab Head";
            this.LabHead.MinimumWidth = 8;
            this.LabHead.Name = "LabHead";
            // 
            // Announce
            // 
            this.Announce.DataPropertyName = "Announce";
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Announce.DefaultCellStyle = dataGridViewCellStyle9;
            this.Announce.FillWeight = 66.85007F;
            this.Announce.HeaderText = "Date";
            this.Announce.MinimumWidth = 8;
            this.Announce.Name = "Announce";
            // 
            // Keywords
            // 
            this.Keywords.DataPropertyName = "Keywords";
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Keywords.DefaultCellStyle = dataGridViewCellStyle10;
            this.Keywords.FillWeight = 135.4862F;
            this.Keywords.HeaderText = "Keyword";
            this.Keywords.MinimumWidth = 8;
            this.Keywords.Name = "Keywords";
            // 
            // tbSearch
            // 
            this.tbSearch.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbSearch.Location = new System.Drawing.Point(18, 66);
            this.tbSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(1207, 44);
            this.tbSearch.TabIndex = 8;
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnSearch.Location = new System.Drawing.Point(1232, 66);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(160, 50);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnReset.Location = new System.Drawing.Point(1400, 66);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(116, 50);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblResult
            // 
            this.lblResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblResult.Location = new System.Drawing.Point(2176, 75);
            this.lblResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(106, 31);
            this.lblResult.TabIndex = 11;
            this.lblResult.Text = "Records";
            // 
            // btnDetail
            // 
            this.btnDetail.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnDetail.Location = new System.Drawing.Point(1524, 66);
            this.btnDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(111, 50);
            this.btnDetail.TabIndex = 12;
            this.btnDetail.Text = "Detail";
            this.btnDetail.UseVisualStyleBackColor = true;
            this.btnDetail.Click += new System.EventHandler(this.btnDetail_Click);
            // 
            // btnUrl
            // 
            this.btnUrl.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnUrl.Location = new System.Drawing.Point(1644, 66);
            this.btnUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUrl.Name = "btnUrl";
            this.btnUrl.Size = new System.Drawing.Size(111, 50);
            this.btnUrl.TabIndex = 13;
            this.btnUrl.Text = "Web";
            this.btnUrl.UseVisualStyleBackColor = true;
            this.btnUrl.Click += new System.EventHandler(this.btnUrl_Click);
            // 
            // lblLoading
            // 
            this.lblLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoading.AutoSize = true;
            this.lblLoading.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLoading.Location = new System.Drawing.Point(1786, 78);
            this.lblLoading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(98, 31);
            this.lblLoading.TabIndex = 15;
            this.lblLoading.Text = "Loaded";
            // 
            // btnFastLoad
            // 
            this.btnFastLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFastLoad.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFastLoad.Location = new System.Drawing.Point(2272, 14);
            this.btnFastLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFastLoad.Name = "btnFastLoad";
            this.btnFastLoad.Size = new System.Drawing.Size(150, 44);
            this.btnFastLoad.TabIndex = 16;
            this.btnFastLoad.Text = "Fast Load";
            this.btnFastLoad.UseVisualStyleBackColor = true;
            this.btnFastLoad.Click += new System.EventHandler(this.btnFastLoad_Click);
            // 
            // MLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2440, 1162);
            this.Controls.Add(this.btnFastLoad);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.btnUrl);
            this.Controls.Add(this.btnDetail);
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
            this.Name = "MLForm";
            this.Text = "MetaboLights";
            this.Load += new System.EventHandler(this.mainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.projectListView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
        private System.Windows.Forms.Button btnDetail;
        private System.Windows.Forms.Button btnUrl;
        private Label lblLoading;
        private Button btnFastLoad;
        private DataGridViewTextBoxColumn Identifier;
        private DataGridViewTextBoxColumn Title;
        private DataGridViewTextBoxColumn Repos;
        private DataGridViewTextBoxColumn Species;
        private DataGridViewTextBoxColumn Instrument;
        private DataGridViewTextBoxColumn Publication;
        private DataGridViewTextBoxColumn LabHead;
        private DataGridViewTextBoxColumn Announce;
        private DataGridViewTextBoxColumn Keywords;
    }
}

