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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend11 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title11 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea12 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend12 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title12 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend10 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title10 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRepositoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startConversionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.globalSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conversionSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.fileTree = new System.Windows.Forms.TreeView();
            this.fileTreeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabTIC = new System.Windows.Forms.TabPage();
            this.ticChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabBasePeak = new System.Windows.Forms.TabPage();
            this.basePeakChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.spectrumChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblAirdInfo = new System.Windows.Forms.Label();
            this.spectraDataGrids = new System.Windows.Forms.DataGridView();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.fileTreeContext.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabTIC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).BeginInit();
            this.tabBasePeak.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraDataGrids)).BeginInit();
            this.notifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem,
            this.startConversionToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.imageViewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(6, 2, 0, 2);
            this.menu.Size = new System.Drawing.Size(2264, 32);
            this.menu.TabIndex = 16;
            this.menu.Text = "menuStrip1";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRepositoryToolStripMenuItem});
            this.filesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("filesToolStripMenuItem.Image")));
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(88, 28);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // openRepositoryToolStripMenuItem
            // 
            this.openRepositoryToolStripMenuItem.Name = "openRepositoryToolStripMenuItem";
            this.openRepositoryToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.openRepositoryToolStripMenuItem.Text = "Open Repository";
            this.openRepositoryToolStripMenuItem.Click += new System.EventHandler(this.openRepositoryToolStripMenuItem_Click);
            // 
            // startConversionToolStripMenuItem
            // 
            this.startConversionToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("startConversionToolStripMenuItem.Image")));
            this.startConversionToolStripMenuItem.Name = "startConversionToolStripMenuItem";
            this.startConversionToolStripMenuItem.Size = new System.Drawing.Size(145, 28);
            this.startConversionToolStripMenuItem.Text = "Conversion";
            this.startConversionToolStripMenuItem.Click += new System.EventHandler(this.startConversionToolStripMenuItem_Click);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalSettingToolStripMenuItem,
            this.conversionSettingToolStripMenuItem});
            this.settingToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("settingToolStripMenuItem.Image")));
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(112, 28);
            this.settingToolStripMenuItem.Text = "Setting";
            // 
            // globalSettingToolStripMenuItem
            // 
            this.globalSettingToolStripMenuItem.Name = "globalSettingToolStripMenuItem";
            this.globalSettingToolStripMenuItem.Size = new System.Drawing.Size(272, 34);
            this.globalSettingToolStripMenuItem.Text = "Global Setting";
            this.globalSettingToolStripMenuItem.Click += new System.EventHandler(this.globalSettingToolStripMenuItem_Click);
            // 
            // conversionSettingToolStripMenuItem
            // 
            this.conversionSettingToolStripMenuItem.Name = "conversionSettingToolStripMenuItem";
            this.conversionSettingToolStripMenuItem.Size = new System.Drawing.Size(272, 34);
            this.conversionSettingToolStripMenuItem.Text = "Conversion Setting";
            this.conversionSettingToolStripMenuItem.Click += new System.EventHandler(this.conversionSettingToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripMenuItem.Image")));
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
            this.mainContainer.Panel2.Controls.Add(this.spectrumChart);
            this.mainContainer.Panel2.Controls.Add(this.lblAirdInfo);
            this.mainContainer.Panel2.Controls.Add(this.spectraDataGrids);
            this.mainContainer.Size = new System.Drawing.Size(2264, 1255);
            this.mainContainer.SplitterDistance = 432;
            this.mainContainer.TabIndex = 17;
            // 
            // fileTree
            // 
            this.fileTree.ContextMenuStrip = this.fileTreeContext;
            this.fileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileTree.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fileTree.Location = new System.Drawing.Point(0, 0);
            this.fileTree.Name = "fileTree";
            this.fileTree.Size = new System.Drawing.Size(432, 1255);
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
            this.tabs.Controls.Add(this.tabTIC);
            this.tabs.Controls.Add(this.tabBasePeak);
            this.tabs.Location = new System.Drawing.Point(10, 51);
            this.tabs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(924, 598);
            this.tabs.TabIndex = 5;
            // 
            // tabTIC
            // 
            this.tabTIC.Controls.Add(this.ticChart);
            this.tabTIC.Location = new System.Drawing.Point(4, 33);
            this.tabTIC.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabTIC.Name = "tabTIC";
            this.tabTIC.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabTIC.Size = new System.Drawing.Size(916, 561);
            this.tabTIC.TabIndex = 0;
            this.tabTIC.Text = "TIC";
            this.tabTIC.UseVisualStyleBackColor = true;
            // 
            // ticChart
            // 
            this.ticChart.BorderlineWidth = 0;
            chartArea11.AxisX.MajorGrid.Enabled = false;
            chartArea11.AxisY.LabelStyle.Format = "\"#,,M\"";
            chartArea11.AxisY.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            chartArea11.BorderWidth = 0;
            chartArea11.IsSameFontSizeForAllAxes = true;
            chartArea11.Name = "ChartAreaTIC";
            this.ticChart.ChartAreas.Add(chartArea11);
            legend11.Alignment = System.Drawing.StringAlignment.Center;
            legend11.DockedToChartArea = "ChartAreaTIC";
            legend11.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend11.Enabled = false;
            legend11.Name = "LegendTIC";
            this.ticChart.Legends.Add(legend11);
            this.ticChart.Location = new System.Drawing.Point(0, 4);
            this.ticChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ticChart.Name = "ticChart";
            series11.BorderWidth = 0;
            series11.ChartArea = "ChartAreaTIC";
            series11.Legend = "LegendTIC";
            series11.MarkerBorderWidth = 0;
            series11.Name = "data";
            this.ticChart.Series.Add(series11);
            this.ticChart.Size = new System.Drawing.Size(912, 549);
            this.ticChart.TabIndex = 2;
            this.ticChart.Text = "TIC";
            title11.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title11.Name = "titleTIC";
            title11.Text = "TIC";
            this.ticChart.Titles.Add(title11);
            // 
            // tabBasePeak
            // 
            this.tabBasePeak.Controls.Add(this.basePeakChart);
            this.tabBasePeak.Location = new System.Drawing.Point(4, 33);
            this.tabBasePeak.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabBasePeak.Name = "tabBasePeak";
            this.tabBasePeak.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabBasePeak.Size = new System.Drawing.Size(916, 561);
            this.tabBasePeak.TabIndex = 1;
            this.tabBasePeak.Text = "Base Peak";
            this.tabBasePeak.UseVisualStyleBackColor = true;
            // 
            // basePeakChart
            // 
            this.basePeakChart.BorderlineWidth = 0;
            chartArea12.AxisX.MajorGrid.Enabled = false;
            chartArea12.BorderWidth = 0;
            chartArea12.IsSameFontSizeForAllAxes = true;
            chartArea12.Name = "ChartAreaBasePeak";
            this.basePeakChart.ChartAreas.Add(chartArea12);
            legend12.Alignment = System.Drawing.StringAlignment.Center;
            legend12.DockedToChartArea = "ChartAreaBasePeak";
            legend12.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend12.Enabled = false;
            legend12.Name = "LegendBasePeak";
            this.basePeakChart.Legends.Add(legend12);
            this.basePeakChart.Location = new System.Drawing.Point(0, 0);
            this.basePeakChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.basePeakChart.Name = "basePeakChart";
            series12.BorderWidth = 0;
            series12.ChartArea = "ChartAreaBasePeak";
            series12.Legend = "LegendBasePeak";
            series12.MarkerBorderWidth = 0;
            series12.Name = "data";
            this.basePeakChart.Series.Add(series12);
            this.basePeakChart.Size = new System.Drawing.Size(912, 554);
            this.basePeakChart.TabIndex = 3;
            this.basePeakChart.Text = "TIC";
            title12.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title12.Name = "titleBasePeak";
            title12.Text = "Base Peak";
            this.basePeakChart.Titles.Add(title12);
            // 
            // spectrumChart
            // 
            this.spectrumChart.BorderlineWidth = 0;
            chartArea10.AxisX.MajorGrid.Enabled = false;
            chartArea10.BorderWidth = 0;
            chartArea10.IsSameFontSizeForAllAxes = true;
            chartArea10.Name = "ChartAreaSpectrum";
            this.spectrumChart.ChartAreas.Add(chartArea10);
            legend10.Alignment = System.Drawing.StringAlignment.Center;
            legend10.DockedToChartArea = "ChartAreaSpectrum";
            legend10.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend10.Enabled = false;
            legend10.Name = "LegendSpectrum";
            this.spectrumChart.Legends.Add(legend10);
            this.spectrumChart.Location = new System.Drawing.Point(944, 62);
            this.spectrumChart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spectrumChart.Name = "spectrumChart";
            series10.BorderWidth = 0;
            series10.ChartArea = "ChartAreaSpectrum";
            series10.Legend = "LegendSpectrum";
            series10.MarkerBorderWidth = 0;
            series10.Name = "data";
            this.spectrumChart.Series.Add(series10);
            this.spectrumChart.Size = new System.Drawing.Size(861, 588);
            this.spectrumChart.TabIndex = 4;
            this.spectrumChart.Text = "TIC";
            title10.Font = new System.Drawing.Font("微软雅黑", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title10.Name = "titleSpectrum";
            title10.Text = "Spectrum";
            this.spectrumChart.Titles.Add(title10);
            // 
            // lblAirdInfo
            // 
            this.lblAirdInfo.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblAirdInfo.Location = new System.Drawing.Point(4, 6);
            this.lblAirdInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAirdInfo.Name = "lblAirdInfo";
            this.lblAirdInfo.Size = new System.Drawing.Size(1814, 40);
            this.lblAirdInfo.TabIndex = 1;
            this.lblAirdInfo.Text = "AirdInfo";
            this.lblAirdInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // spectraDataGrids
            // 
            this.spectraDataGrids.AllowUserToOrderColumns = true;
            this.spectraDataGrids.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spectraDataGrids.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.spectraDataGrids.ColumnHeadersHeight = 34;
            this.spectraDataGrids.Location = new System.Drawing.Point(4, 658);
            this.spectraDataGrids.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.spectraDataGrids.MultiSelect = false;
            this.spectraDataGrids.Name = "spectraDataGrids";
            this.spectraDataGrids.ReadOnly = true;
            this.spectraDataGrids.RowHeadersWidth = 62;
            this.spectraDataGrids.RowTemplate.Height = 23;
            this.spectraDataGrids.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.spectraDataGrids.Size = new System.Drawing.Size(1831, 592);
            this.spectraDataGrids.TabIndex = 0;
            this.spectraDataGrids.VirtualMode = true;
            this.spectraDataGrids.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.spectraDataGrids_MouseDoubleClick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "AirdPro";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // notifyMenu
            // 
            this.notifyMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.notifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.notifyMenu.Name = "notifyMenu";
            this.notifyMenu.Size = new System.Drawing.Size(119, 34);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(118, 30);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // imageViewToolStripMenuItem
            // 
            this.imageViewToolStripMenuItem.Name = "imageViewToolStripMenuItem";
            this.imageViewToolStripMenuItem.Size = new System.Drawing.Size(126, 28);
            this.imageViewToolStripMenuItem.Text = "Image View";
            this.imageViewToolStripMenuItem.Click += new System.EventHandler(this.imageViewToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2264, 1287);
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AirdPro";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.fileTreeContext.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tabTIC.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).EndInit();
            this.tabBasePeak.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraDataGrids)).EndInit();
            this.notifyMenu.ResumeLayout(false);
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
        private System.Windows.Forms.DataVisualization.Charting.Chart ticChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart basePeakChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart spectrumChart;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabTIC;
        private System.Windows.Forms.TabPage tabBasePeak;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyMenu;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conversionSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageViewToolStripMenuItem;
    }
}