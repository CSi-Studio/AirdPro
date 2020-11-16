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
            this.btnAddCommonFiles = new System.Windows.Forms.Button();
            this.btnAddDDAFiles = new System.Windows.Forms.Button();
            this.btnChooseSSwathFiles = new System.Windows.Forms.Button();
            this.btnAddPRMFiles = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblFileSelectedInfo = new System.Windows.Forms.Label();
            this.lvFileList = new System.Windows.Forms.ListView();
            this.headerFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerExpType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDeleteFiles = new System.Windows.Forms.Button();
            this.btnAddSWATHFiles = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblOperator = new System.Windows.Forms.Label();
            this.tbOperator = new System.Windows.Forms.TextBox();
            this.lblMzPrecision = new System.Windows.Forms.Label();
            this.cbMzPrecision = new System.Windows.Forms.ComboBox();
            this.cbThreadAccelerate = new System.Windows.Forms.CheckBox();
            this.cbLog2 = new System.Windows.Forms.CheckBox();
            this.lblFileNameTag = new System.Windows.Forms.Label();
            this.tbFileNameSuffix = new System.Windows.Forms.TextBox();
            this.cbIsZeroIntensityIgnore = new System.Windows.Forms.CheckBox();
            this.lblConsole = new System.Windows.Forms.Label();
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChooseFolder = new System.Windows.Forms.Button();
            this.tbFolderPath = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
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
            this.container.Panel1.Controls.Add(this.btnAddCommonFiles);
            this.container.Panel1.Controls.Add(this.btnAddDDAFiles);
            this.container.Panel1.Controls.Add(this.btnChooseSSwathFiles);
            this.container.Panel1.Controls.Add(this.btnAddPRMFiles);
            this.container.Panel1.Controls.Add(this.btnClear);
            this.container.Panel1.Controls.Add(this.lblFileSelectedInfo);
            this.container.Panel1.Controls.Add(this.lvFileList);
            this.container.Panel1.Controls.Add(this.btnDeleteFiles);
            this.container.Panel1.Controls.Add(this.btnAddSWATHFiles);
            this.container.Panel1.Controls.Add(this.btnConvert);
            // 
            // container.Panel2
            // 
            this.container.Panel2.Controls.Add(this.lblOperator);
            this.container.Panel2.Controls.Add(this.tbOperator);
            this.container.Panel2.Controls.Add(this.lblMzPrecision);
            this.container.Panel2.Controls.Add(this.cbMzPrecision);
            this.container.Panel2.Controls.Add(this.cbThreadAccelerate);
            this.container.Panel2.Controls.Add(this.cbLog2);
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
            // btnAddCommonFiles
            // 
            resources.ApplyResources(this.btnAddCommonFiles, "btnAddCommonFiles");
            this.btnAddCommonFiles.Name = "btnAddCommonFiles";
            this.btnAddCommonFiles.UseVisualStyleBackColor = true;
            this.btnAddCommonFiles.Click += new System.EventHandler(this.btnAddCommonFiles_Click);
            // 
            // btnAddDDAFiles
            // 
            resources.ApplyResources(this.btnAddDDAFiles, "btnAddDDAFiles");
            this.btnAddDDAFiles.Name = "btnAddDDAFiles";
            this.btnAddDDAFiles.UseVisualStyleBackColor = true;
            this.btnAddDDAFiles.Click += new System.EventHandler(this.btnChooseDDAFile_Click);
            // 
            // btnChooseSSwathFiles
            // 
            resources.ApplyResources(this.btnChooseSSwathFiles, "btnChooseSSwathFiles");
            this.btnChooseSSwathFiles.Name = "btnChooseSSwathFiles";
            this.btnChooseSSwathFiles.UseVisualStyleBackColor = true;
            this.btnChooseSSwathFiles.Click += new System.EventHandler(this.btnChooseSSwathFiles_Click);
            // 
            // btnAddPRMFiles
            // 
            resources.ApplyResources(this.btnAddPRMFiles, "btnAddPRMFiles");
            this.btnAddPRMFiles.Name = "btnAddPRMFiles";
            this.btnAddPRMFiles.UseVisualStyleBackColor = true;
            this.btnAddPRMFiles.Click += new System.EventHandler(this.btnChoosePRMFiles_Click);
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
            // 
            // lvFileList
            // 
            resources.ApplyResources(this.lvFileList, "lvFileList");
            this.lvFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerFilePath,
            this.headerExpType,
            this.headerProgress});
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
            // headerProgress
            // 
            resources.ApplyResources(this.headerProgress, "headerProgress");
            // 
            // btnDeleteFiles
            // 
            resources.ApplyResources(this.btnDeleteFiles, "btnDeleteFiles");
            this.btnDeleteFiles.Name = "btnDeleteFiles";
            this.btnDeleteFiles.UseVisualStyleBackColor = true;
            this.btnDeleteFiles.Click += new System.EventHandler(this.btnDeleteFiles_Click);
            // 
            // btnAddSWATHFiles
            // 
            resources.ApplyResources(this.btnAddSWATHFiles, "btnAddSWATHFiles");
            this.btnAddSWATHFiles.Name = "btnAddSWATHFiles";
            this.btnAddSWATHFiles.UseVisualStyleBackColor = true;
            this.btnAddSWATHFiles.Click += new System.EventHandler(this.btnChooseFiles_Click);
            // 
            // btnConvert
            // 
            resources.ApplyResources(this.btnConvert, "btnConvert");
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
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
            resources.GetString("cbMzPrecision.Items2")});
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
            // cbLog2
            // 
            resources.ApplyResources(this.cbLog2, "cbLog2");
            this.cbLog2.Name = "cbLog2";
            this.cbLog2.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.Button btnAddSWATHFiles;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox tbFolderPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChooseFolder;
        private System.Windows.Forms.ListView lvFileList;
        private System.Windows.Forms.Button btnDeleteFiles;
        private System.Windows.Forms.SplitContainer container;
        private System.Windows.Forms.Label lblFileSelectedInfo;
        private System.Windows.Forms.TextBox tbConsole;
        private System.Windows.Forms.Label lblConsole;
        private System.Windows.Forms.ColumnHeader headerFilePath;
        private System.Windows.Forms.ColumnHeader headerProgress;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAddPRMFiles;
        private System.Windows.Forms.ColumnHeader headerExpType;
        private System.Windows.Forms.CheckBox cbIsZeroIntensityIgnore;
        private System.Windows.Forms.Label lblFileNameTag;
        private System.Windows.Forms.TextBox tbFileNameSuffix;
        private System.Windows.Forms.CheckBox cbLog2;
        private System.Windows.Forms.CheckBox cbThreadAccelerate;
        private System.Windows.Forms.Button btnChooseSSwathFiles;
        private System.Windows.Forms.Button btnAddDDAFiles;
        private System.Windows.Forms.ComboBox cbMzPrecision;
        private System.Windows.Forms.Label lblMzPrecision;
        private System.Windows.Forms.Label lblOperator;
        private System.Windows.Forms.TextBox tbOperator;
        private System.Windows.Forms.Button btnAddCommonFiles;
    }
}

