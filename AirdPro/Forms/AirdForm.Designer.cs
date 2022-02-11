﻿/*
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
            this.btnSelectVendorFiles = new System.Windows.Forms.Button();
            this.btnClearError = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblFileSelectedInfo = new System.Windows.Forms.Label();
            this.lvFileList = new System.Windows.Forms.ListView();
            this.headerFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerExpType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerCompressor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDeleteFiles = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.cbIncludingPSICV = new System.Windows.Forms.CheckBox();
            this.lblStackLayers = new System.Windows.Forms.Label();
            this.cbStackLayers = new System.Windows.Forms.ComboBox();
            this.lblAirdAlgorithm = new System.Windows.Forms.Label();
            this.cbAlgorithm = new System.Windows.Forms.ComboBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblConnectStatus = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.tbHostAndPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblOperator = new System.Windows.Forms.Label();
            this.tbOperator = new System.Windows.Forms.TextBox();
            this.lblMzPrecision = new System.Windows.Forms.Label();
            this.cbMzPrecision = new System.Windows.Forms.ComboBox();
            this.cbThreadAccelerate = new System.Windows.Forms.CheckBox();
            this.lblFileNameTag = new System.Windows.Forms.Label();
            this.tbFileNameSuffix = new System.Windows.Forms.TextBox();
            this.cbIsZeroIntensityIgnore = new System.Windows.Forms.CheckBox();
            this.lblConsole = new System.Windows.Forms.Label();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.tbFolderPath = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.timerConsumer = new System.Windows.Forms.Timer(this.components);
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.ttAlgorithm = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.container)).BeginInit();
            this.container.Panel1.SuspendLayout();
            this.container.Panel2.SuspendLayout();
            this.container.SuspendLayout();
            this.SuspendLayout();
            // 
            // container
            // 
            resources.ApplyResources(this.container, "container");
            this.container.Name = "container";
            // 
            // container.Panel1
            // 
            this.container.Panel1.Controls.Add(this.btnSelectVendorFiles);
            this.container.Panel1.Controls.Add(this.btnClearError);
            this.container.Panel1.Controls.Add(this.btnClear);
            this.container.Panel1.Controls.Add(this.lblFileSelectedInfo);
            this.container.Panel1.Controls.Add(this.lvFileList);
            this.container.Panel1.Controls.Add(this.btnDeleteFiles);
            this.container.Panel1.Controls.Add(this.btnConvert);
            // 
            // container.Panel2
            // 
            this.container.Panel2.Controls.Add(this.cbIncludingPSICV);
            this.container.Panel2.Controls.Add(this.lblStackLayers);
            this.container.Panel2.Controls.Add(this.cbStackLayers);
            this.container.Panel2.Controls.Add(this.lblAirdAlgorithm);
            this.container.Panel2.Controls.Add(this.cbAlgorithm);
            this.container.Panel2.Controls.Add(this.btnDisconnect);
            this.container.Panel2.Controls.Add(this.lblConnectStatus);
            this.container.Panel2.Controls.Add(this.lblIP);
            this.container.Panel2.Controls.Add(this.tbHostAndPort);
            this.container.Panel2.Controls.Add(this.btnConnect);
            this.container.Panel2.Controls.Add(this.lblOperator);
            this.container.Panel2.Controls.Add(this.tbOperator);
            this.container.Panel2.Controls.Add(this.lblMzPrecision);
            this.container.Panel2.Controls.Add(this.cbMzPrecision);
            this.container.Panel2.Controls.Add(this.cbThreadAccelerate);
            this.container.Panel2.Controls.Add(this.lblFileNameTag);
            this.container.Panel2.Controls.Add(this.tbFileNameSuffix);
            this.container.Panel2.Controls.Add(this.cbIsZeroIntensityIgnore);
            this.container.Panel2.Controls.Add(this.lblConsole);
            this.container.Panel2.Controls.Add(this.tbConsole);
            this.container.Panel2.Controls.Add(this.label1);
            this.container.Panel2.Controls.Add(this.btnChooseFolder);
            this.container.Panel2.Controls.Add(this.tbFolderPath);
            resources.ApplyResources(this.container.Panel2, "container.Panel2");
            // 
            // btnSelectVendorFiles
            // 
            resources.ApplyResources(this.btnSelectVendorFiles, "btnSelectVendorFiles");
            this.btnSelectVendorFiles.Name = "btnSelectVendorFiles";
            this.btnSelectVendorFiles.UseVisualStyleBackColor = true;
            this.btnSelectVendorFiles.Click += new System.EventHandler(this.btnCustomerPath_Click);
            // 
            // btnClearError
            // 
            resources.ApplyResources(this.btnClearError, "btnClearError");
            this.btnClearError.Name = "btnClearError";
            this.btnClearError.UseVisualStyleBackColor = true;
            this.btnClearError.Click += new System.EventHandler(this.btnClearError_Click);
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblFileSelectedInfo
            // 
            resources.ApplyResources(this.lblFileSelectedInfo, "lblFileSelectedInfo");
            this.lblFileSelectedInfo.Name = "lblFileSelectedInfo";
            this.lblFileSelectedInfo.Click += new System.EventHandler(this.lblFileSelectedInfo_Click);
            // 
            // lvFileList
            // 
            resources.ApplyResources(this.lvFileList, "lvFileList");
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerFilePath,
            this.headerExpType,
            this.headerCompressor,
            this.headerProgress,
            this.headerOutput});
            this.lvFileList.FullRowSelect = true;
            this.lvFileList.GridLines = true;
            this.lvFileList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvFileList.HideSelection = false;
            this.lvFileList.Name = "lvFileList";
            this.lvFileList.ShowItemToolTips = true;
            this.lvFileList.UseCompatibleStateImageBehavior = false;
            this.lvFileList.View = System.Windows.Forms.View.Details;
            this.lvFileList.SelectedIndexChanged += new System.EventHandler(this.lvFileList_SelectedIndexChanged);
            // 
            // headerFilePath
            // 
            resources.ApplyResources(this.headerFilePath, "headerFilePath");
            // 
            // headerExpType
            // 
            resources.ApplyResources(this.headerExpType, "headerExpType");
            // 
            // headerCompressor
            // 
            resources.ApplyResources(this.headerCompressor, "headerCompressor");
            // 
            // headerProgress
            // 
            resources.ApplyResources(this.headerProgress, "headerProgress");
            // 
            // headerOutput
            // 
            resources.ApplyResources(this.headerOutput, "headerOutput");
            // 
            // btnDeleteFiles
            // 
            resources.ApplyResources(this.btnDeleteFiles, "btnDeleteFiles");
            this.btnDeleteFiles.Name = "btnDeleteFiles";
            this.btnDeleteFiles.UseVisualStyleBackColor = true;
            this.btnDeleteFiles.Click += new System.EventHandler(this.btnDeleteFiles_Click);
            // 
            // btnConvert
            // 
            resources.ApplyResources(this.btnConvert, "btnConvert");
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // cbIncludingPSICV
            // 
            resources.ApplyResources(this.cbIncludingPSICV, "cbIncludingPSICV");
            this.cbIncludingPSICV.Checked = true;
            this.cbIncludingPSICV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludingPSICV.Name = "cbIncludingPSICV";
            this.cbIncludingPSICV.UseVisualStyleBackColor = true;
            // 
            // lblStackLayers
            // 
            resources.ApplyResources(this.lblStackLayers, "lblStackLayers");
            this.lblStackLayers.Name = "lblStackLayers";
            // 
            // cbStackLayers
            // 
            this.cbStackLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStackLayers.FormattingEnabled = true;
            this.cbStackLayers.Items.AddRange(new object[] {
            resources.GetString("cbStackLayers.Items"),
            resources.GetString("cbStackLayers.Items1"),
            resources.GetString("cbStackLayers.Items2"),
            resources.GetString("cbStackLayers.Items3"),
            resources.GetString("cbStackLayers.Items4"),
            resources.GetString("cbStackLayers.Items5")});
            resources.ApplyResources(this.cbStackLayers, "cbStackLayers");
            this.cbStackLayers.Name = "cbStackLayers";
            // 
            // lblAirdAlgorithm
            // 
            resources.ApplyResources(this.lblAirdAlgorithm, "lblAirdAlgorithm");
            this.lblAirdAlgorithm.Name = "lblAirdAlgorithm";
            // 
            // cbAlgorithm
            // 
            this.cbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlgorithm.FormattingEnabled = true;
            this.cbAlgorithm.Items.AddRange(new object[] {
            resources.GetString("cbAlgorithm.Items"),
            resources.GetString("cbAlgorithm.Items1"),
            resources.GetString("cbAlgorithm.Items2")});
            resources.ApplyResources(this.cbAlgorithm, "cbAlgorithm");
            this.cbAlgorithm.Name = "cbAlgorithm";
            this.ttAlgorithm.SetToolTip(this.cbAlgorithm, resources.GetString("cbAlgorithm.ToolTip"));
            this.cbAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cbAlgorithm_SelectedIndexChanged);
            // 
            // btnDisconnect
            // 
            resources.ApplyResources(this.btnDisconnect, "btnDisconnect");
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lblConnectStatus
            // 
            resources.ApplyResources(this.lblConnectStatus, "lblConnectStatus");
            this.lblConnectStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectStatus.Name = "lblConnectStatus";
            // 
            // lblIP
            // 
            resources.ApplyResources(this.lblIP, "lblIP");
            this.lblIP.Name = "lblIP";
            // 
            // tbHostAndPort
            // 
            resources.ApplyResources(this.tbHostAndPort, "tbHostAndPort");
            this.tbHostAndPort.Name = "tbHostAndPort";
            // 
            // btnConnect
            // 
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblOperator
            // 
            resources.ApplyResources(this.lblOperator, "lblOperator");
            this.lblOperator.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblOperator.Name = "lblOperator";
            // 
            // tbOperator
            // 
            resources.ApplyResources(this.tbOperator, "tbOperator");
            this.tbOperator.Name = "tbOperator";
            // 
            // lblMzPrecision
            // 
            resources.ApplyResources(this.lblMzPrecision, "lblMzPrecision");
            this.lblMzPrecision.Name = "lblMzPrecision";
            // 
            // cbMzPrecision
            // 
            resources.ApplyResources(this.cbMzPrecision, "cbMzPrecision");
            this.cbMzPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMzPrecision.FormattingEnabled = true;
            this.cbMzPrecision.Items.AddRange(new object[] {
            resources.GetString("cbMzPrecision.Items"),
            resources.GetString("cbMzPrecision.Items1"),
            resources.GetString("cbMzPrecision.Items2"),
            resources.GetString("cbMzPrecision.Items3")});
            this.cbMzPrecision.Name = "cbMzPrecision";
            // 
            // cbThreadAccelerate
            // 
            resources.ApplyResources(this.cbThreadAccelerate, "cbThreadAccelerate");
            this.cbThreadAccelerate.Checked = true;
            this.cbThreadAccelerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbThreadAccelerate.Name = "cbThreadAccelerate";
            this.cbThreadAccelerate.UseVisualStyleBackColor = true;
            // 
            // lblFileNameTag
            // 
            resources.ApplyResources(this.lblFileNameTag, "lblFileNameTag");
            this.lblFileNameTag.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFileNameTag.Name = "lblFileNameTag";
            // 
            // tbFileNameSuffix
            // 
            resources.ApplyResources(this.tbFileNameSuffix, "tbFileNameSuffix");
            this.tbFileNameSuffix.Name = "tbFileNameSuffix";
            // 
            // cbIsZeroIntensityIgnore
            // 
            resources.ApplyResources(this.cbIsZeroIntensityIgnore, "cbIsZeroIntensityIgnore");
            this.cbIsZeroIntensityIgnore.Checked = true;
            this.cbIsZeroIntensityIgnore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIsZeroIntensityIgnore.Name = "cbIsZeroIntensityIgnore";
            this.cbIsZeroIntensityIgnore.UseVisualStyleBackColor = true;
            this.cbIsZeroIntensityIgnore.CheckedChanged += new System.EventHandler(this.cbIsZeroIntensityIgnore_CheckedChanged);
            // 
            // lblConsole
            // 
            resources.ApplyResources(this.lblConsole, "lblConsole");
            this.lblConsole.Name = "lblConsole";
            // 
            // tbConsole
            // 
            resources.ApplyResources(this.tbConsole, "tbConsole");
            this.tbConsole.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.tbConsole.ForeColor = System.Drawing.SystemColors.Window;
            this.tbConsole.Name = "tbConsole";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Name = "label1";
            // 
            // btnChooseFolder
            // 
            resources.ApplyResources(this.btnChooseFolder, "btnChooseFolder");
            this.btnChooseFolder.Name = "btnChooseFolder";
            this.btnChooseFolder.UseVisualStyleBackColor = true;
            this.btnChooseFolder.Click += new System.EventHandler(this.btnChooseFolder_Click);
            // 
            // tbFolderPath
            // 
            resources.ApplyResources(this.tbFolderPath, "tbFolderPath");
            this.tbFolderPath.Name = "tbFolderPath";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // timerConsumer
            // 
            this.timerConsumer.Interval = 3000;
            this.timerConsumer.Tick += new System.EventHandler(this.consumer_Tick);
            // 
            // ofd
            // 
            resources.ApplyResources(this.ofd, "ofd");
            this.ofd.Multiselect = true;
            // 
            // AirdForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.container);
            this.DoubleBuffered = true;
            this.Name = "AirdForm";
            this.Load += new System.EventHandler(this.ProproForm_Load);
            this.container.Panel1.ResumeLayout(false);
            this.container.Panel2.ResumeLayout(false);
            this.container.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.container)).EndInit();
            this.container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox tbFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.Button btnDeleteFiles;
        private System.Windows.Forms.SplitContainer container;
        private System.Windows.Forms.Label lblFileSelectedInfo;
        private System.Windows.Forms.TextBox tbConsole;
        private System.Windows.Forms.Label lblConsole;
        private System.Windows.Forms.ColumnHeader headerFilePath;
        private System.Windows.Forms.ColumnHeader headerProgress;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ColumnHeader headerExpType;
        private System.Windows.Forms.CheckBox cbIsZeroIntensityIgnore;
        private System.Windows.Forms.Label lblFileNameTag;
        private System.Windows.Forms.TextBox tbFileNameSuffix;
        private System.Windows.Forms.CheckBox cbThreadAccelerate;
        private System.Windows.Forms.ComboBox cbMzPrecision;
        private System.Windows.Forms.Label lblMzPrecision;
        private System.Windows.Forms.Label lblOperator;
        private System.Windows.Forms.TextBox tbOperator;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbHostAndPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Timer timerConsumer;
        public System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.Button btnDisconnect;
        public System.Windows.Forms.Label lblConnectStatus;
        private System.Windows.Forms.Button btnClearError;
        private System.Windows.Forms.ColumnHeader headerOutput;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.Button btnSelectVendorFiles;
        private System.Windows.Forms.Label lblAirdAlgorithm;
        private System.Windows.Forms.ComboBox cbAlgorithm;
        private System.Windows.Forms.Label lblStackLayers;
        private System.Windows.Forms.ComboBox cbStackLayers;
        private System.Windows.Forms.ColumnHeader headerCompressor;
        private System.Windows.Forms.ToolTip ttAlgorithm;
        private System.Windows.Forms.CheckBox cbIncludingPSICV;
    }
}

