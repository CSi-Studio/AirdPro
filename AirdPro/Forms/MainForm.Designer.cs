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
            this.tabs = new System.Windows.Forms.TabControl();
            this.panelContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chartPanel1 = new AirdPro.Controls.ChartPanel();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.fileTreeContext.SuspendLayout();
            this.tabs.SuspendLayout();
            this.panelContext.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.menu.Size = new System.Drawing.Size(2980, 32);
            this.menu.TabIndex = 16;
            this.menu.Text = "menuStrip1";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRepositoryToolStripMenuItem});
            this.filesToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Files;
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(88, 28);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // openRepositoryToolStripMenuItem
            // 
            this.openRepositoryToolStripMenuItem.Name = "openRepositoryToolStripMenuItem";
            this.openRepositoryToolStripMenuItem.Size = new System.Drawing.Size(256, 34);
            this.openRepositoryToolStripMenuItem.Text = "Open Repository";
            this.openRepositoryToolStripMenuItem.Click += new System.EventHandler(this.openRepositoryToolStripMenuItem_Click);
            // 
            // startConversionToolStripMenuItem
            // 
            this.startConversionToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Conversion;
            this.startConversionToolStripMenuItem.Name = "startConversionToolStripMenuItem";
            this.startConversionToolStripMenuItem.Size = new System.Drawing.Size(145, 28);
            this.startConversionToolStripMenuItem.Text = "Conversion";
            this.startConversionToolStripMenuItem.Click += new System.EventHandler(this.startConversionToolStripMenuItem_Click);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalSettingToolStripMenuItem});
            this.settingToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Setting;
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(112, 28);
            this.settingToolStripMenuItem.Text = "Setting";
            // 
            // globalSettingToolStripMenuItem
            // 
            this.globalSettingToolStripMenuItem.Name = "globalSettingToolStripMenuItem";
            this.globalSettingToolStripMenuItem.Size = new System.Drawing.Size(233, 34);
            this.globalSettingToolStripMenuItem.Text = "Global Setting";
            this.globalSettingToolStripMenuItem.Click += new System.EventHandler(this.globalSettingToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Image = global::AirdPro.Properties.Resources.Help;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(91, 28);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(164, 34);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // mainContainer
            // 
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mainContainer.Location = new System.Drawing.Point(0, 32);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.fileTree);
            this.mainContainer.Panel1MinSize = 20;
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.tabs);
            this.mainContainer.Size = new System.Drawing.Size(2980, 1560);
            this.mainContainer.SplitterDistance = 577;
            this.mainContainer.TabIndex = 17;
            // 
            // fileTree
            // 
            this.fileTree.ContextMenuStrip = this.fileTreeContext;
            this.fileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileTree.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fileTree.Location = new System.Drawing.Point(0, 0);
            this.fileTree.Name = "fileTree";
            this.fileTree.Size = new System.Drawing.Size(577, 1560);
            this.fileTree.TabIndex = 0;
            this.fileTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.fileTree_NodeMouseClick);
            // 
            // fileTreeContext
            // 
            this.fileTreeContext.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.fileTreeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemRefresh});
            this.fileTreeContext.Name = "fileTreeContext";
            this.fileTreeContext.Size = new System.Drawing.Size(145, 34);
            // 
            // itemRefresh
            // 
            this.itemRefresh.Name = "itemRefresh";
            this.itemRefresh.Size = new System.Drawing.Size(144, 30);
            this.itemRefresh.Text = "Refresh";
            this.itemRefresh.Click += new System.EventHandler(this.itemRefresh_Click);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.ContextMenuStrip = this.panelContext;
            this.tabs.Controls.Add(this.tabPage1);
            this.tabs.Location = new System.Drawing.Point(3, 4);
            this.tabs.Margin = new System.Windows.Forms.Padding(4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(2395, 1553);
            this.tabs.TabIndex = 0;
            // 
            // panelContext
            // 
            this.panelContext.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.panelContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuClose});
            this.panelContext.Name = "panelContext";
            this.panelContext.Size = new System.Drawing.Size(127, 34);
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(126, 30);
            this.menuClose.Text = "Close";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chartPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(2387, 1516);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chartPanel1
            // 
            this.chartPanel1.Location = new System.Drawing.Point(15, 16);
            this.chartPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chartPanel1.Name = "chartPanel1";
            this.chartPanel1.Size = new System.Drawing.Size(1944, 1306);
            this.chartPanel1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2980, 1592);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.tabs.ResumeLayout(false);
            this.panelContext.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.ContextMenuStrip panelContext;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.TabPage tabPage1;
        private Controls.ChartPanel chartPanel1;
    }
}