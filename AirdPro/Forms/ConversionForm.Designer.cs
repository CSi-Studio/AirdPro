﻿/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Forms
{
    partial class ConversionForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversionForm));
            this.container = new System.Windows.Forms.SplitContainer();
            this.label9 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPX = new System.Windows.Forms.Button();
            this.btnMetaboLights = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnMainView = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnHelp = new System.Windows.Forms.Button();
            this.label112 = new System.Windows.Forms.Label();
            this.btnRedisSetting = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lvFileList = new System.Windows.Forms.ListView();
            this.headerJobId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerConfigName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerScene = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerCentroid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPrecision = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerCompressor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerIgnoreZero = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerSuffix = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contentMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.rerun = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new System.Windows.Forms.Button();
            this.cbAutoExe = new System.Windows.Forms.CheckBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCleanFinished = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSetting = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tbJobInfo = new System.Windows.Forms.TextBox();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.logTimer = new System.Windows.Forms.Timer(this.components);
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.timerTaskScan = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.container)).BeginInit();
            this.container.Panel1.SuspendLayout();
            this.container.Panel2.SuspendLayout();
            this.container.SuspendLayout();
            this.contentMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            resources.ApplyResources(this.container, "container");
            this.container.Name = "container";
            // 
            // container.Panel1
            // 
            this.container.Panel1.Controls.Add(this.label9);
            this.container.Panel1.Controls.Add(this.btnDelete);
            this.container.Panel1.Controls.Add(this.btnPX);
            this.container.Panel1.Controls.Add(this.btnMetaboLights);
            this.container.Panel1.Controls.Add(this.label4);
            this.container.Panel1.Controls.Add(this.btnMainView);
            this.container.Panel1.Controls.Add(this.label12);
            this.container.Panel1.Controls.Add(this.btnHelp);
            this.container.Panel1.Controls.Add(this.label112);
            this.container.Panel1.Controls.Add(this.btnRedisSetting);
            this.container.Panel1.Controls.Add(this.label11);
            this.container.Panel1.Controls.Add(this.label8);
            this.container.Panel1.Controls.Add(this.label7);
            this.container.Panel1.Controls.Add(this.label6);
            this.container.Panel1.Controls.Add(this.lvFileList);
            this.container.Panel1.Controls.Add(this.btnImport);
            this.container.Panel1.Controls.Add(this.cbAutoExe);
            this.container.Panel1.Controls.Add(this.btnConvert);
            this.container.Panel1.Controls.Add(this.label5);
            this.container.Panel1.Controls.Add(this.btnCleanFinished);
            this.container.Panel1.Controls.Add(this.label3);
            this.container.Panel1.Controls.Add(this.btnSetting);
            this.container.Panel1.Controls.Add(this.label2);
            this.container.Panel1.Controls.Add(this.label1);
            // 
            // container.Panel2
            // 
            this.container.Panel2.Controls.Add(this.splitContainer1);
            resources.ApplyResources(this.container.Panel2, "container.Panel2");
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Name = "label9";
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Name = "btnDelete";
            this.tooltip.SetToolTip(this.btnDelete, resources.GetString("btnDelete.ToolTip"));
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPX
            // 
            resources.ApplyResources(this.btnPX, "btnPX");
            this.btnPX.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPX.FlatAppearance.BorderSize = 0;
            this.btnPX.Name = "btnPX";
            this.tooltip.SetToolTip(this.btnPX, resources.GetString("btnPX.ToolTip"));
            this.btnPX.UseVisualStyleBackColor = true;
            this.btnPX.Click += new System.EventHandler(this.btnPX_Click);
            // 
            // btnMetaboLights
            // 
            resources.ApplyResources(this.btnMetaboLights, "btnMetaboLights");
            this.btnMetaboLights.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnMetaboLights.FlatAppearance.BorderSize = 0;
            this.btnMetaboLights.Name = "btnMetaboLights";
            this.tooltip.SetToolTip(this.btnMetaboLights, resources.GetString("btnMetaboLights.ToolTip"));
            this.btnMetaboLights.UseVisualStyleBackColor = true;
            this.btnMetaboLights.Click += new System.EventHandler(this.btnMetaboLights_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Name = "label4";
            // 
            // btnMainView
            // 
            resources.ApplyResources(this.btnMainView, "btnMainView");
            this.btnMainView.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnMainView.FlatAppearance.BorderSize = 0;
            this.btnMainView.Name = "btnMainView";
            this.tooltip.SetToolTip(this.btnMainView, resources.GetString("btnMainView.ToolTip"));
            this.btnMainView.UseVisualStyleBackColor = true;
            this.btnMainView.Click += new System.EventHandler(this.btnMainView_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label12.Name = "label12";
            // 
            // btnHelp
            // 
            resources.ApplyResources(this.btnHelp, "btnHelp");
            this.btnHelp.BackColor = System.Drawing.SystemColors.Window;
            this.btnHelp.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.Name = "btnHelp";
            this.tooltip.SetToolTip(this.btnHelp, resources.GetString("btnHelp.ToolTip"));
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // label112
            // 
            resources.ApplyResources(this.label112, "label112");
            this.label112.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label112.Name = "label112";
            // 
            // btnRedisSetting
            // 
            resources.ApplyResources(this.btnRedisSetting, "btnRedisSetting");
            this.btnRedisSetting.BackColor = System.Drawing.SystemColors.Window;
            this.btnRedisSetting.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRedisSetting.FlatAppearance.BorderSize = 0;
            this.btnRedisSetting.Name = "btnRedisSetting";
            this.tooltip.SetToolTip(this.btnRedisSetting, resources.GetString("btnRedisSetting.ToolTip"));
            this.btnRedisSetting.UseVisualStyleBackColor = false;
            this.btnRedisSetting.Click += new System.EventHandler(this.btnRedisSetting_Click);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label11.Name = "label11";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Name = "label6";
            // 
            // lvFileList
            // 
            this.lvFileList.AllowColumnReorder = true;
            resources.ApplyResources(this.lvFileList, "lvFileList");
            this.lvFileList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerJobId,
            this.headerFilePath,
            this.headerType,
            this.headerConfigName,
            this.headerScene,
            this.headerCentroid,
            this.headerProgress,
            this.headerPrecision,
            this.headerCompressor,
            this.headerIgnoreZero,
            this.headerSuffix,
            this.headerOutput});
            this.lvFileList.ContextMenuStrip = this.contentMenu;
            this.lvFileList.FullRowSelect = true;
            this.lvFileList.GridLines = true;
            this.lvFileList.HideSelection = false;
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.ShowGroups = false;
            this.lvFileList.ShowItemToolTips = true;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            this.lvFileList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvFileList_ColumnClick);
            this.lvFileList.SelectedIndexChanged += new System.EventHandler(this.lvFileList_SelectedIndexChanged);
            this.lvFileList.DoubleClick += new System.EventHandler(this.lvFileList_DoubleClick);
            this.lvFileList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvFileList_KeyDown);
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
            // headerConfigName
            // 
            resources.ApplyResources(this.headerConfigName, "headerConfigName");
            // 
            // headerScene
            // 
            resources.ApplyResources(this.headerScene, "headerScene");
            // 
            // headerCentroid
            // 
            resources.ApplyResources(this.headerCentroid, "headerCentroid");
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
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedItems);
            // 
            // btnImport
            // 
            resources.ApplyResources(this.btnImport, "btnImport");
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnImport.FlatAppearance.BorderSize = 0;
            this.btnImport.Name = "btnImport";
            this.tooltip.SetToolTip(this.btnImport, resources.GetString("btnImport.ToolTip"));
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cbAutoExe
            // 
            resources.ApplyResources(this.cbAutoExe, "cbAutoExe");
            this.cbAutoExe.Name = "cbAutoExe";
            this.cbAutoExe.UseVisualStyleBackColor = true;
            this.cbAutoExe.CheckedChanged += new System.EventHandler(this.cbAutoExe_CheckedChanged);
            // 
            // btnConvert
            // 
            resources.ApplyResources(this.btnConvert, "btnConvert");
            this.btnConvert.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.Name = "btnConvert";
            this.tooltip.SetToolTip(this.btnConvert, resources.GetString("btnConvert.ToolTip"));
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Name = "label5";
            // 
            // btnCleanFinished
            // 
            resources.ApplyResources(this.btnCleanFinished, "btnCleanFinished");
            this.btnCleanFinished.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCleanFinished.FlatAppearance.BorderSize = 0;
            this.btnCleanFinished.Name = "btnCleanFinished";
            this.tooltip.SetToolTip(this.btnCleanFinished, resources.GetString("btnCleanFinished.ToolTip"));
            this.btnCleanFinished.UseVisualStyleBackColor = true;
            this.btnCleanFinished.Click += new System.EventHandler(this.btnCleanFinished_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Name = "label3";
            // 
            // btnSetting
            // 
            resources.ApplyResources(this.btnSetting, "btnSetting");
            this.btnSetting.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.Name = "btnSetting";
            this.tooltip.SetToolTip(this.btnSetting, resources.GetString("btnSetting.ToolTip"));
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Name = "label1";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tbJobInfo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbConsole);
            // 
            // tbJobInfo
            // 
            resources.ApplyResources(this.tbJobInfo, "tbJobInfo");
            this.tbJobInfo.BackColor = System.Drawing.SystemColors.Window;
            this.tbJobInfo.ForeColor = System.Drawing.SystemColors.InfoText;
            this.tbJobInfo.Name = "tbJobInfo";
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
            this.logTimer.Tick += new System.EventHandler(this.logTimer_Tick);
            // 
            // ofd
            // 
            resources.ApplyResources(this.ofd, "ofd");
            this.ofd.Multiselect = true;
            // 
            // timerTaskScan
            // 
            this.timerTaskScan.Interval = 2000;
            this.timerTaskScan.Tick += new System.EventHandler(this.timerTaskScan_Tick);
            // 
            // ConversionForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.container);
            this.DoubleBuffered = true;
            this.Name = "ConversionForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConversionForm_FormClosing);
            this.Load += new System.EventHandler(this.ProproForm_Load);
            this.container.Panel1.ResumeLayout(false);
            this.container.Panel1.PerformLayout();
            this.container.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.container)).EndInit();
            this.container.ResumeLayout(false);
            this.contentMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer splitContainer1;

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label9;

        private System.Windows.Forms.Button btnMetaboLights;

        private System.Windows.Forms.Button btnPX;

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;

        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.ColumnHeader headerCentroid;

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
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.ColumnHeader headerPrecision;
        private System.Windows.Forms.ContextMenuStrip contentMenu;
        private System.Windows.Forms.ToolStripMenuItem rerun;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader headerJobId;
        private System.Windows.Forms.ColumnHeader headerIgnoreZero;
        private System.Windows.Forms.ColumnHeader headerSuffix;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCleanFinished;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer timerTaskScan;
        private System.Windows.Forms.CheckBox cbAutoExe;
        private System.Windows.Forms.ColumnHeader headerScene;
        private System.Windows.Forms.TextBox tbJobInfo;
        private System.Windows.Forms.ColumnHeader headerConfigName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label112;
        public System.Windows.Forms.Button btnRedisSetting;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMainView;
    }
}

