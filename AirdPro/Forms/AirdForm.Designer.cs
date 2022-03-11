/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Forms
{
    partial class AirdForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AirdForm));
            this.container = new System.Windows.Forms.SplitContainer();
            this.lvFileList = new System.Windows.Forms.ListView();
            this.headerJobId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPrecision = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerCompressor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerIgnoreZero = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerSuffix = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.rerun = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConfig = new System.Windows.Forms.MenuStrip();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanFinishedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.configList = new System.Windows.Forms.ToolStripMenuItem();
            this.globalSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compouterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblImport = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblRedisMessageCenter = new System.Windows.Forms.Label();
            this.btnRedisDisconnect = new System.Windows.Forms.Button();
            this.lblRedisStatus = new System.Windows.Forms.Label();
            this.btnRedisConnect = new System.Windows.Forms.Button();
            this.lblConvert = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.logTimer = new System.Windows.Forms.Timer(this.components);
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.ttAlgorithm = new System.Windows.Forms.ToolTip(this.components);
            this.redisConsumer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.container)).BeginInit();
            this.container.Panel1.SuspendLayout();
            this.container.Panel2.SuspendLayout();
            this.container.SuspendLayout();
            this.contentMenu.SuspendLayout();
            this.menuConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            resources.ApplyResources(this.container, "container");
            this.container.Name = "container";
            // 
            // container.Panel1
            // 
            this.container.Panel1.Controls.Add(this.lvFileList);
            this.container.Panel1.Controls.Add(this.menuConfig);
            // 
            // container.Panel2
            // 
            this.container.Panel2.Controls.Add(this.lblImport);
            this.container.Panel2.Controls.Add(this.btnImport);
            this.container.Panel2.Controls.Add(this.lblRedisMessageCenter);
            this.container.Panel2.Controls.Add(this.btnRedisDisconnect);
            this.container.Panel2.Controls.Add(this.lblRedisStatus);
            this.container.Panel2.Controls.Add(this.btnRedisConnect);
            this.container.Panel2.Controls.Add(this.lblConvert);
            this.container.Panel2.Controls.Add(this.btnConvert);
            this.container.Panel2.Controls.Add(this.tbConsole);
            resources.ApplyResources(this.container.Panel2, "container.Panel2");
            // 
            // lvFileList
            // 
            this.lvFileList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerJobId,
            this.headerFilePath,
            this.headerType,
            this.headerProgress,
            this.headerPrecision,
            this.headerCompressor,
            this.headerIgnoreZero,
            this.headerSuffix,
            this.headerOutput});
            this.lvFileList.ContextMenuStrip = this.contentMenu;
            resources.ApplyResources(this.lvFileList, "lvFileList");
            this.lvFileList.FullRowSelect = true;
            this.lvFileList.GridLines = true;
            this.lvFileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileList.HideSelection = false;
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.ShowGroups = false;
            this.lvFileList.ShowItemToolTips = true;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            this.lvFileList.SelectedIndexChanged += new System.EventHandler(this.lvFileList_SelectedIndexChanged);
            this.lvFileList.DoubleClick += new System.EventHandler(this.lvFileList_DoubleClick);
            // 
            // headerJobId
            // 
            resources.ApplyResources(this.headerJobId, "headerJobId");
            // 
            // headerFilePath
            // 
            resources.ApplyResources(this.headerFilePath, "headerFilePath");
            // 
            // headerType
            // 
            resources.ApplyResources(this.headerType, "headerType");
            // 
            // headerProgress
            // 
            resources.ApplyResources(this.headerProgress, "headerProgress");
            // 
            // headerPrecision
            // 
            resources.ApplyResources(this.headerPrecision, "headerPrecision");
            // 
            // headerCompressor
            // 
            resources.ApplyResources(this.headerCompressor, "headerCompressor");
            // 
            // headerIgnoreZero
            // 
            resources.ApplyResources(this.headerIgnoreZero, "headerIgnoreZero");
            // 
            // headerSuffix
            // 
            resources.ApplyResources(this.headerSuffix, "headerSuffix");
            // 
            // headerOutput
            // 
            resources.ApplyResources(this.headerOutput, "headerOutput");
            // 
            // contentMenu
            // 
            this.contentMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contentMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rerun,
            this.removeToolStripMenuItem});
            this.contentMenu.Name = "contentMenu";
            resources.ApplyResources(this.contentMenu, "contentMenu");
            // 
            // rerun
            // 
            this.rerun.Name = "rerun";
            resources.ApplyResources(this.rerun, "rerun");
            this.rerun.Click += new System.EventHandler(this.rerun_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            resources.ApplyResources(this.removeToolStripMenuItem, "removeToolStripMenuItem");
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // menuConfig
            // 
            resources.ApplyResources(this.menuConfig, "menuConfig");
            this.menuConfig.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuConfig.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem,
            this.editToolStripMenuItem,
            this.menuConfiguration,
            this.helpToolStripMenuItem});
            this.menuConfig.Name = "menuConfig";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFilesToolStripMenuItem});
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            resources.ApplyResources(this.filesToolStripMenuItem, "filesToolStripMenuItem");
            // 
            // selectFilesToolStripMenuItem
            // 
            this.selectFilesToolStripMenuItem.Name = "selectFilesToolStripMenuItem";
            resources.ApplyResources(this.selectFilesToolStripMenuItem, "selectFilesToolStripMenuItem");
            this.selectFilesToolStripMenuItem.Click += new System.EventHandler(this.selectFilesToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cleanFinishedToolStripMenuItem,
            this.cleanErrorsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // cleanFinishedToolStripMenuItem
            // 
            this.cleanFinishedToolStripMenuItem.Name = "cleanFinishedToolStripMenuItem";
            resources.ApplyResources(this.cleanFinishedToolStripMenuItem, "cleanFinishedToolStripMenuItem");
            this.cleanFinishedToolStripMenuItem.Click += new System.EventHandler(this.cleanFinishedToolStripMenuItem_Click);
            // 
            // cleanErrorsToolStripMenuItem
            // 
            this.cleanErrorsToolStripMenuItem.Name = "cleanErrorsToolStripMenuItem";
            resources.ApplyResources(this.cleanErrorsToolStripMenuItem, "cleanErrorsToolStripMenuItem");
            this.cleanErrorsToolStripMenuItem.Click += new System.EventHandler(this.cleanErrorsToolStripMenuItem_Click);
            // 
            // menuConfiguration
            // 
            this.menuConfiguration.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configList,
            this.globalSettingsToolStripMenuItem});
            this.menuConfiguration.Name = "menuConfiguration";
            resources.ApplyResources(this.menuConfiguration, "menuConfiguration");
            // 
            // configList
            // 
            this.configList.Name = "configList";
            resources.ApplyResources(this.configList, "configList");
            this.configList.Click += new System.EventHandler(this.openConversionConfigListForm);
            // 
            // globalSettingsToolStripMenuItem
            // 
            this.globalSettingsToolStripMenuItem.Name = "globalSettingsToolStripMenuItem";
            resources.ApplyResources(this.globalSettingsToolStripMenuItem, "globalSettingsToolStripMenuItem");
            this.globalSettingsToolStripMenuItem.Click += new System.EventHandler(this.globalSettingsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compouterToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // compouterToolStripMenuItem
            // 
            this.compouterToolStripMenuItem.Name = "compouterToolStripMenuItem";
            resources.ApplyResources(this.compouterToolStripMenuItem, "compouterToolStripMenuItem");
            this.compouterToolStripMenuItem.Click += new System.EventHandler(this.compouterToolStripMenuItem_Click);
            // 
            // lblImport
            // 
            resources.ApplyResources(this.lblImport, "lblImport");
            this.lblImport.Name = "lblImport";
            // 
            // btnImport
            // 
            this.btnImport.BackgroundImage = global::AirdPro.Properties.Resources.Import;
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.FlatAppearance.BorderSize = 0;
            this.btnImport.Name = "btnImport";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblRedisMessageCenter
            // 
            resources.ApplyResources(this.lblRedisMessageCenter, "lblRedisMessageCenter");
            this.lblRedisMessageCenter.Name = "lblRedisMessageCenter";
            // 
            // btnRedisDisconnect
            // 
            resources.ApplyResources(this.btnRedisDisconnect, "btnRedisDisconnect");
            this.btnRedisDisconnect.FlatAppearance.BorderSize = 0;
            this.btnRedisDisconnect.Name = "btnRedisDisconnect";
            this.btnRedisDisconnect.UseVisualStyleBackColor = true;
            this.btnRedisDisconnect.Click += new System.EventHandler(this.btnRedisDisconnect_Click);
            // 
            // lblRedisStatus
            // 
            resources.ApplyResources(this.lblRedisStatus, "lblRedisStatus");
            this.lblRedisStatus.ForeColor = System.Drawing.Color.Red;
            this.lblRedisStatus.Name = "lblRedisStatus";
            // 
            // btnRedisConnect
            // 
            this.btnRedisConnect.BackgroundImage = global::AirdPro.Properties.Resources.DisConnect;
            resources.ApplyResources(this.btnRedisConnect, "btnRedisConnect");
            this.btnRedisConnect.FlatAppearance.BorderSize = 0;
            this.btnRedisConnect.Name = "btnRedisConnect";
            this.btnRedisConnect.UseVisualStyleBackColor = true;
            this.btnRedisConnect.Click += new System.EventHandler(this.btnRedisConnect_Click);
            // 
            // lblConvert
            // 
            resources.ApplyResources(this.lblConvert, "lblConvert");
            this.lblConvert.Name = "lblConvert";
            // 
            // btnConvert
            // 
            this.btnConvert.BackgroundImage = global::AirdPro.Properties.Resources.Convert;
            resources.ApplyResources(this.btnConvert, "btnConvert");
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // tbConsole
            // 
            resources.ApplyResources(this.tbConsole, "tbConsole");
            this.tbConsole.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.tbConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.tbConsole.Name = "tbConsole";
            // 
            // logTimer
            // 
            this.logTimer.Enabled = true;
            this.logTimer.Tick += new System.EventHandler(this.logTimer_Tick);
            // 
            // ofd
            // 
            resources.ApplyResources(this.ofd, "ofd");
            this.ofd.Multiselect = true;
            // 
            // redisConsumer
            // 
            this.redisConsumer.Interval = 3000;
            this.redisConsumer.Tick += new System.EventHandler(this.redisConsumer_Tick);
            // 
            // AirdForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.container);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuConfig;
            this.Name = "AirdForm";
            this.Load += new System.EventHandler(this.ProproForm_Load);
            this.container.Panel1.ResumeLayout(false);
            this.container.Panel1.PerformLayout();
            this.container.Panel2.ResumeLayout(false);
            this.container.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.container)).EndInit();
            this.container.ResumeLayout(false);
            this.contentMenu.ResumeLayout(false);
            this.menuConfig.ResumeLayout(false);
            this.menuConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.SplitContainer container;
        private System.Windows.Forms.TextBox tbConsole;
        private System.Windows.Forms.ColumnHeader headerFilePath;
        private System.Windows.Forms.ColumnHeader headerProgress;
        private System.Windows.Forms.Timer logTimer;
        private System.Windows.Forms.ColumnHeader headerType;
        public System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.ColumnHeader headerOutput;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ColumnHeader headerCompressor;
        private System.Windows.Forms.ToolTip ttAlgorithm;
        private System.Windows.Forms.ColumnHeader headerPrecision;
        private System.Windows.Forms.MenuStrip menuConfig;
        private System.Windows.Forms.ToolStripMenuItem menuConfiguration;
        private System.Windows.Forms.ToolStripMenuItem configList;
        private System.Windows.Forms.ToolStripMenuItem globalSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFilesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contentMenu;
        private System.Windows.Forms.ToolStripMenuItem rerun;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanFinishedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanErrorsToolStripMenuItem;
        private System.Windows.Forms.Label lblConvert;
        private System.Windows.Forms.ColumnHeader headerJobId;
        private System.Windows.Forms.ColumnHeader headerIgnoreZero;
        private System.Windows.Forms.ColumnHeader headerSuffix;
        private System.Windows.Forms.Timer redisConsumer;
        private System.Windows.Forms.Button btnRedisConnect;
        private System.Windows.Forms.Label lblRedisStatus;
        private System.Windows.Forms.Button btnRedisDisconnect;
        private System.Windows.Forms.Label lblRedisMessageCenter;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compouterToolStripMenuItem;
        private System.Windows.Forms.Label lblImport;
        private System.Windows.Forms.Button btnImport;
    }
}

