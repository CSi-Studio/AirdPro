namespace AirdPro.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRepositoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startConversionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.fileTree = new System.Windows.Forms.TreeView();
            this.fileTreeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.spectraDataGrids = new System.Windows.Forms.DataGridView();
            this.lblAirdInfo = new System.Windows.Forms.Label();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.fileTreeContext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectraDataGrids)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem,
            this.startConversionToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menu.Size = new System.Drawing.Size(1509, 30);
            this.menu.TabIndex = 16;
            this.menu.Text = "menuStrip1";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRepositoryToolStripMenuItem});
            this.filesToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Files;
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(69, 28);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // openRepositoryToolStripMenuItem
            // 
            this.openRepositoryToolStripMenuItem.Name = "openRepositoryToolStripMenuItem";
            this.openRepositoryToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.openRepositoryToolStripMenuItem.Text = "Open Repository";
            this.openRepositoryToolStripMenuItem.Click += new System.EventHandler(this.openRepositoryToolStripMenuItem_Click);
            // 
            // startConversionToolStripMenuItem
            // 
            this.startConversionToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Conversion;
            this.startConversionToolStripMenuItem.Name = "startConversionToolStripMenuItem";
            this.startConversionToolStripMenuItem.Size = new System.Drawing.Size(109, 28);
            this.startConversionToolStripMenuItem.Text = "Conversion";
            this.startConversionToolStripMenuItem.Click += new System.EventHandler(this.startConversionToolStripMenuItem_Click);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalSettingToolStripMenuItem});
            this.settingToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Setting;
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(84, 28);
            this.settingToolStripMenuItem.Text = "Setting";
            // 
            // globalSettingToolStripMenuItem
            // 
            this.globalSettingToolStripMenuItem.Name = "globalSettingToolStripMenuItem";
            this.globalSettingToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.globalSettingToolStripMenuItem.Text = "Global Setting";
            this.globalSettingToolStripMenuItem.Click += new System.EventHandler(this.globalSettingToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Help;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(71, 28);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // mainContainer
            // 
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainContainer.Location = new System.Drawing.Point(0, 30);
            this.mainContainer.Margin = new System.Windows.Forms.Padding(2);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.fileTree);
            this.mainContainer.Panel1MinSize = 20;
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.lblAirdInfo);
            this.mainContainer.Panel2.Controls.Add(this.spectraDataGrids);
            this.mainContainer.Size = new System.Drawing.Size(1509, 828);
            this.mainContainer.SplitterDistance = 291;
            this.mainContainer.SplitterWidth = 3;
            this.mainContainer.TabIndex = 17;
            // 
            // fileTree
            // 
            this.fileTree.ContextMenuStrip = this.fileTreeContext;
            this.fileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileTree.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fileTree.Location = new System.Drawing.Point(0, 0);
            this.fileTree.Margin = new System.Windows.Forms.Padding(2);
            this.fileTree.Name = "fileTree";
            this.fileTree.Size = new System.Drawing.Size(291, 828);
            this.fileTree.TabIndex = 0;
            this.fileTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.fileTree_NodeMouseClick);
            this.fileTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.fileTree_NodeMouseDoubleClick);
            // 
            // fileTreeContext
            // 
            this.fileTreeContext.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.fileTreeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemRefresh});
            this.fileTreeContext.Name = "fileTreeContext";
            this.fileTreeContext.Size = new System.Drawing.Size(121, 26);
            // 
            // itemRefresh
            // 
            this.itemRefresh.Name = "itemRefresh";
            this.itemRefresh.Size = new System.Drawing.Size(120, 22);
            this.itemRefresh.Text = "Refresh";
            this.itemRefresh.Click += new System.EventHandler(this.itemRefresh_Click);
            // 
            // spectraDataGrids
            // 
            this.spectraDataGrids.AllowUserToOrderColumns = true;
            this.spectraDataGrids.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spectraDataGrids.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.spectraDataGrids.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.spectraDataGrids.Location = new System.Drawing.Point(3, 34);
            this.spectraDataGrids.Name = "spectraDataGrids";
            this.spectraDataGrids.RowTemplate.Height = 23;
            this.spectraDataGrids.Size = new System.Drawing.Size(1209, 794);
            this.spectraDataGrids.TabIndex = 0;
            // 
            // lblAirdInfo
            // 
            this.lblAirdInfo.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblAirdInfo.Location = new System.Drawing.Point(3, 4);
            this.lblAirdInfo.Name = "lblAirdInfo";
            this.lblAirdInfo.Size = new System.Drawing.Size(1209, 27);
            this.lblAirdInfo.TabIndex = 1;
            this.lblAirdInfo.Text = "AirdInfo";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1509, 858);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "MainForm";
            this.Text = "AirdPro";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.fileTreeContext.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spectraDataGrids)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem globalSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startConversionToolStripMenuItem;
        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.ToolStripMenuItem openRepositoryToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip fileTreeContext;
        private System.Windows.Forms.ToolStripMenuItem itemRefresh;
        private System.Windows.Forms.DataGridView spectraDataGrids;
        private System.Windows.Forms.Label lblAirdInfo;
    }
}