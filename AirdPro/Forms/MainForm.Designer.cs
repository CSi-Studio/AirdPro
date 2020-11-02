/*
 * Copyright (c) 2020 Propro Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuConvert = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToAirdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCloud = new System.Windows.Forms.ToolStripMenuItem();
            this.aliyunOSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuConvert,
            this.menuUpload,
            this.menuCloud,
            this.menuSettings,
            this.menuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuConvert
            // 
            this.menuConvert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertToAirdToolStripMenuItem});
            this.menuConvert.Name = "menuConvert";
            this.menuConvert.Size = new System.Drawing.Size(65, 21);
            this.menuConvert.Text = "Convert";
            // 
            // convertToAirdToolStripMenuItem
            // 
            this.convertToAirdToolStripMenuItem.Name = "convertToAirdToolStripMenuItem";
            this.convertToAirdToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.convertToAirdToolStripMenuItem.Text = "Convert To Aird";
            this.convertToAirdToolStripMenuItem.Click += new System.EventHandler(this.convertToAirdToolStripMenuItem_Click);
            // 
            // menuUpload
            // 
            this.menuUpload.Name = "menuUpload";
            this.menuUpload.Size = new System.Drawing.Size(63, 21);
            this.menuUpload.Text = "Upload";
            // 
            // menuCloud
            // 
            this.menuCloud.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aliyunOSSToolStripMenuItem});
            this.menuCloud.Name = "menuCloud";
            this.menuCloud.Size = new System.Drawing.Size(54, 21);
            this.menuCloud.Text = "Cloud";
            // 
            // aliyunOSSToolStripMenuItem
            // 
            this.aliyunOSSToolStripMenuItem.Name = "aliyunOSSToolStripMenuItem";
            this.aliyunOSSToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aliyunOSSToolStripMenuItem.Text = "Aliyun-OSS";
            // 
            // menuSettings
            // 
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(66, 21);
            this.menuSettings.Text = "Settings";
            this.menuSettings.Click += new System.EventHandler(this.helpToolStripMenuItem2_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(47, 21);
            this.menuHelp.Text = "Help";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuConvert;
        private System.Windows.Forms.ToolStripMenuItem menuUpload;
        private System.Windows.Forms.ToolStripMenuItem menuCloud;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem aliyunOSSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToAirdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
    }
}