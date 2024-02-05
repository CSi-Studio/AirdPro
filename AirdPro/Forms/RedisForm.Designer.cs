namespace AirdPro.Forms
{
    partial class RedisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RedisForm));
            this.mainView = new System.Windows.Forms.SplitContainer();
            this.lblSwitch = new System.Windows.Forms.Label();
            this.switchConsumeJob = new HZH_Controls.Controls.UCSwitch();
            this.btnRefreshJobList = new System.Windows.Forms.Button();
            this.btnClearRedisCache = new System.Windows.Forms.Button();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tbRedisPassword = new System.Windows.Forms.TextBox();
            this.tbRedisUsername = new System.Windows.Forms.TextBox();
            this.tbRedisHost = new System.Windows.Forms.TextBox();
            this.tbRedisPort = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.mainOperation = new System.Windows.Forms.SplitContainer();
            this.lvServers = new System.Windows.Forms.ListView();
            this.colId = new System.Windows.Forms.ColumnHeader();
            this.colIP = new System.Windows.Forms.ColumnHeader();
            this.colServerName = new System.Windows.Forms.ColumnHeader();
            this.colOSVersion = new System.Windows.Forms.ColumnHeader();
            this.colCPU = new System.Windows.Forms.ColumnHeader();
            this.colMemory = new System.Windows.Forms.ColumnHeader();
            this.colVersion = new System.Windows.Forms.ColumnHeader();
            this.colConsumingJob = new System.Windows.Forms.ColumnHeader();
            this.listMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openConsumeSwitchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeConsumeSwitchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvJobs = new System.Windows.Forms.ListView();
            this.colJobId = new System.Windows.Forms.ColumnHeader();
            this.colType = new System.Windows.Forms.ColumnHeader();
            this.colScene = new System.Windows.Forms.ColumnHeader();
            this.colFile = new System.Windows.Forms.ColumnHeader();
            this.colInput = new System.Windows.Forms.ColumnHeader();
            this.colOutput = new System.Windows.Forms.ColumnHeader();
            this.colConsumeIP = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.consumeTimer = new System.Windows.Forms.Timer(this.components);
            this.heartBeatTimer = new System.Windows.Forms.Timer(this.components);
            this.btnClearTempFiles = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mainView)).BeginInit();
            this.mainView.Panel1.SuspendLayout();
            this.mainView.Panel2.SuspendLayout();
            this.mainView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainOperation)).BeginInit();
            this.mainOperation.Panel1.SuspendLayout();
            this.mainOperation.Panel2.SuspendLayout();
            this.mainOperation.SuspendLayout();
            this.listMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainView
            // 
            this.mainView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainView.Location = new System.Drawing.Point(0, 0);
            this.mainView.Name = "mainView";
            this.mainView.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainView.Panel1
            // 
            this.mainView.Panel1.Controls.Add(this.btnClearTempFiles);
            this.mainView.Panel1.Controls.Add(this.lblSwitch);
            this.mainView.Panel1.Controls.Add(this.switchConsumeJob);
            this.mainView.Panel1.Controls.Add(this.btnRefreshJobList);
            this.mainView.Panel1.Controls.Add(this.btnClearRedisCache);
            this.mainView.Panel1.Controls.Add(this.lblPassword);
            this.mainView.Panel1.Controls.Add(this.lblUser);
            this.mainView.Panel1.Controls.Add(this.lblPort);
            this.mainView.Panel1.Controls.Add(this.lblIP);
            this.mainView.Panel1.Controls.Add(this.lblStatus);
            this.mainView.Panel1.Controls.Add(this.btnConnect);
            this.mainView.Panel1.Controls.Add(this.tbRedisPassword);
            this.mainView.Panel1.Controls.Add(this.tbRedisUsername);
            this.mainView.Panel1.Controls.Add(this.tbRedisHost);
            this.mainView.Panel1.Controls.Add(this.tbRedisPort);
            this.mainView.Panel1.Controls.Add(this.btnSave);
            // 
            // mainView.Panel2
            // 
            this.mainView.Panel2.Controls.Add(this.mainOperation);
            this.mainView.Size = new System.Drawing.Size(978, 621);
            this.mainView.SplitterDistance = 68;
            this.mainView.TabIndex = 138;
            // 
            // lblSwitch
            // 
            this.lblSwitch.AutoSize = true;
            this.lblSwitch.Location = new System.Drawing.Point(864, 45);
            this.lblSwitch.Name = "lblSwitch";
            this.lblSwitch.Size = new System.Drawing.Size(84, 17);
            this.lblSwitch.TabIndex = 159;
            this.lblSwitch.Text = "Consume Off";
            // 
            // switchConsumeJob
            // 
            this.switchConsumeJob.BackColor = System.Drawing.Color.Transparent;
            this.switchConsumeJob.Checked = false;
            this.switchConsumeJob.FalseColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(189)))), ((int)(((byte)(189)))));
            this.switchConsumeJob.FalseTextColr = System.Drawing.Color.White;
            this.switchConsumeJob.Location = new System.Drawing.Point(804, 41);
            this.switchConsumeJob.Name = "switchConsumeJob";
            this.switchConsumeJob.Size = new System.Drawing.Size(54, 23);
            this.switchConsumeJob.SwitchType = HZH_Controls.Controls.SwitchType.Ellipse;
            this.switchConsumeJob.TabIndex = 158;
            this.switchConsumeJob.Texts = new string[] { "Consume Job Switch" };
            this.switchConsumeJob.TrueColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.switchConsumeJob.TrueTextColr = System.Drawing.Color.White;
            this.switchConsumeJob.CheckedChanged += new System.EventHandler(this.switchConsumeJob_CheckedChanged);
            // 
            // btnRefreshJobList
            // 
            this.btnRefreshJobList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefreshJobList.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRefreshJobList.FlatAppearance.BorderSize = 0;
            this.btnRefreshJobList.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefreshJobList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefreshJobList.Location = new System.Drawing.Point(718, 40);
            this.btnRefreshJobList.Name = "btnRefreshJobList";
            this.btnRefreshJobList.Size = new System.Drawing.Size(80, 26);
            this.btnRefreshJobList.TabIndex = 156;
            this.btnRefreshJobList.Text = "Refresh";
            this.btnRefreshJobList.UseVisualStyleBackColor = true;
            this.btnRefreshJobList.Click += new System.EventHandler(this.btnRefreshJobList_Click);
            // 
            // btnClearRedisCache
            // 
            this.btnClearRedisCache.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClearRedisCache.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearRedisCache.FlatAppearance.BorderSize = 0;
            this.btnClearRedisCache.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnClearRedisCache.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClearRedisCache.Location = new System.Drawing.Point(8, 40);
            this.btnClearRedisCache.Name = "btnClearRedisCache";
            this.btnClearRedisCache.Size = new System.Drawing.Size(152, 26);
            this.btnClearRedisCache.TabIndex = 154;
            this.btnClearRedisCache.Text = "Clear Redis Cache";
            this.btnClearRedisCache.UseVisualStyleBackColor = true;
            this.btnClearRedisCache.Click += new System.EventHandler(this.btnClearRedisCache_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(482, 7);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(67, 20);
            this.lblPassword.TabIndex = 153;
            this.lblPassword.Text = "Password";
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(302, 7);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(44, 20);
            this.lblUser.TabIndex = 152;
            this.lblUser.Text = "User";
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(196, 9);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(44, 20);
            this.lblPort.TabIndex = 151;
            this.lblPort.Text = "Port";
            // 
            // lblIP
            // 
            this.lblIP.Location = new System.Drawing.Point(12, 9);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(30, 20);
            this.lblIP.TabIndex = 150;
            this.lblIP.Text = "IP";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(8, 33);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1880, 5);
            this.lblStatus.TabIndex = 147;
            // 
            // btnConnect
            // 
            this.btnConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConnect.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConnect.Location = new System.Drawing.Point(781, 4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(94, 26);
            this.btnConnect.TabIndex = 146;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // tbRedisPassword
            // 
            this.tbRedisPassword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisPassword.Location = new System.Drawing.Point(556, 6);
            this.tbRedisPassword.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisPassword.Name = "tbRedisPassword";
            this.tbRedisPassword.Size = new System.Drawing.Size(130, 23);
            this.tbRedisPassword.TabIndex = 145;
            // 
            // tbRedisUsername
            // 
            this.tbRedisUsername.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisUsername.Location = new System.Drawing.Point(353, 6);
            this.tbRedisUsername.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisUsername.Name = "tbRedisUsername";
            this.tbRedisUsername.Size = new System.Drawing.Size(122, 23);
            this.tbRedisUsername.TabIndex = 143;
            // 
            // tbRedisHost
            // 
            this.tbRedisHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.tbRedisHost.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisHost.Location = new System.Drawing.Point(49, 6);
            this.tbRedisHost.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisHost.Name = "tbRedisHost";
            this.tbRedisHost.Size = new System.Drawing.Size(140, 23);
            this.tbRedisHost.TabIndex = 137;
            // 
            // tbRedisPort
            // 
            this.tbRedisPort.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisPort.Location = new System.Drawing.Point(237, 6);
            this.tbRedisPort.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisPort.Name = "tbRedisPort";
            this.tbRedisPort.Size = new System.Drawing.Size(58, 23);
            this.tbRedisPort.TabIndex = 139;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(882, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 26);
            this.btnSave.TabIndex = 140;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // mainOperation
            // 
            this.mainOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainOperation.Location = new System.Drawing.Point(0, 0);
            this.mainOperation.Name = "mainOperation";
            this.mainOperation.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainOperation.Panel1
            // 
            this.mainOperation.Panel1.Controls.Add(this.lvServers);
            // 
            // mainOperation.Panel2
            // 
            this.mainOperation.Panel2.Controls.Add(this.lvJobs);
            this.mainOperation.Size = new System.Drawing.Size(978, 549);
            this.mainOperation.SplitterDistance = 189;
            this.mainOperation.TabIndex = 0;
            // 
            // lvServers
            // 
            this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colId, this.colIP, this.colServerName, this.colOSVersion, this.colCPU, this.colMemory, this.colVersion, this.colConsumingJob });
            this.lvServers.ContextMenuStrip = this.listMenu;
            this.lvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvServers.FullRowSelect = true;
            this.lvServers.HideSelection = false;
            this.lvServers.Location = new System.Drawing.Point(0, 0);
            this.lvServers.MultiSelect = false;
            this.lvServers.Name = "lvServers";
            this.lvServers.Size = new System.Drawing.Size(978, 189);
            this.lvServers.TabIndex = 0;
            this.lvServers.UseCompatibleStateImageBehavior = false;
            this.lvServers.View = System.Windows.Forms.View.Details;
            // 
            // colId
            // 
            this.colId.Text = "ID";
            // 
            // colIP
            // 
            this.colIP.Text = "IP";
            this.colIP.Width = 101;
            // 
            // colServerName
            // 
            this.colServerName.Text = "Server Name";
            this.colServerName.Width = 123;
            // 
            // colOSVersion
            // 
            this.colOSVersion.Text = "OS";
            this.colOSVersion.Width = 185;
            // 
            // colCPU
            // 
            this.colCPU.Text = "CPU";
            this.colCPU.Width = 256;
            // 
            // colMemory
            // 
            this.colMemory.Text = "Memory";
            this.colMemory.Width = 69;
            // 
            // colVersion
            // 
            this.colVersion.Text = "AirdPro";
            this.colVersion.Width = 78;
            // 
            // colConsumingJob
            // 
            this.colConsumingJob.Text = "Consuming Job";
            this.colConsumingJob.Width = 102;
            // 
            // listMenu
            // 
            this.listMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.openConsumeSwitchToolStripMenuItem, this.closeConsumeSwitchToolStripMenuItem });
            this.listMenu.Name = "listMenu";
            this.listMenu.Size = new System.Drawing.Size(207, 48);
            // 
            // openConsumeSwitchToolStripMenuItem
            // 
            this.openConsumeSwitchToolStripMenuItem.Name = "openConsumeSwitchToolStripMenuItem";
            this.openConsumeSwitchToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.openConsumeSwitchToolStripMenuItem.Text = "Open Consume Switch";
            this.openConsumeSwitchToolStripMenuItem.Click += new System.EventHandler(this.openConsumeSwitchToolStripMenuItem_Click);
            // 
            // closeConsumeSwitchToolStripMenuItem
            // 
            this.closeConsumeSwitchToolStripMenuItem.Name = "closeConsumeSwitchToolStripMenuItem";
            this.closeConsumeSwitchToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.closeConsumeSwitchToolStripMenuItem.Text = "Close Consume Switch";
            this.closeConsumeSwitchToolStripMenuItem.Click += new System.EventHandler(this.closeConsumeSwitchToolStripMenuItem_Click);
            // 
            // lvJobs
            // 
            this.lvJobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colJobId, this.colType, this.colScene, this.colFile, this.colInput, this.colOutput, this.colConsumeIP, this.colTime });
            this.lvJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvJobs.FullRowSelect = true;
            this.lvJobs.HideSelection = false;
            this.lvJobs.Location = new System.Drawing.Point(0, 0);
            this.lvJobs.Name = "lvJobs";
            this.lvJobs.Size = new System.Drawing.Size(978, 356);
            this.lvJobs.TabIndex = 0;
            this.lvJobs.UseCompatibleStateImageBehavior = false;
            this.lvJobs.View = System.Windows.Forms.View.Details;
            // 
            // colJobId
            // 
            this.colJobId.Text = "JobId";
            this.colJobId.Width = 50;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 50;
            // 
            // colScene
            // 
            this.colScene.Text = "Scene";
            this.colScene.Width = 80;
            // 
            // colFile
            // 
            this.colFile.Text = "File";
            this.colFile.Width = 278;
            // 
            // colInput
            // 
            this.colInput.Text = "Input Path";
            this.colInput.Width = 149;
            // 
            // colOutput
            // 
            this.colOutput.Text = "Output Path";
            this.colOutput.Width = 142;
            // 
            // colConsumeIP
            // 
            this.colConsumeIP.Text = "IP";
            this.colConsumeIP.Width = 120;
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            this.colTime.Width = 150;
            // 
            // consumeTimer
            // 
            this.consumeTimer.Enabled = true;
            this.consumeTimer.Tick += new System.EventHandler(this.consumeTimer_Tick);
            // 
            // heartBeatTimer
            // 
            this.heartBeatTimer.Enabled = true;
            this.heartBeatTimer.Interval = 5000;
            this.heartBeatTimer.Tick += new System.EventHandler(this.heartBeatTimer_Tick);
            // 
            // btnClearTempFiles
            // 
            this.btnClearTempFiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClearTempFiles.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearTempFiles.FlatAppearance.BorderSize = 0;
            this.btnClearTempFiles.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnClearTempFiles.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClearTempFiles.Location = new System.Drawing.Point(166, 40);
            this.btnClearTempFiles.Name = "btnClearTempFiles";
            this.btnClearTempFiles.Size = new System.Drawing.Size(152, 26);
            this.btnClearTempFiles.TabIndex = 160;
            this.btnClearTempFiles.Text = "Clear Temp Files";
            this.btnClearTempFiles.UseVisualStyleBackColor = true;
            this.btnClearTempFiles.Click += new System.EventHandler(this.btnClearTempFiles_Click);
            // 
            // RedisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(978, 621);
            this.Controls.Add(this.mainView);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(15, 15);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RedisForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RedisForm_FormClosing);
            this.Load += new System.EventHandler(this.RedisForm_Load);
            this.mainView.Panel1.ResumeLayout(false);
            this.mainView.Panel1.PerformLayout();
            this.mainView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainView)).EndInit();
            this.mainView.ResumeLayout(false);
            this.mainOperation.Panel1.ResumeLayout(false);
            this.mainOperation.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainOperation)).EndInit();
            this.mainOperation.ResumeLayout(false);
            this.listMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnClearTempFiles;

        private System.Windows.Forms.Timer heartBeatTimer;

        private System.Windows.Forms.ColumnHeader colFile;

        private System.Windows.Forms.ToolStripMenuItem openConsumeSwitchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeConsumeSwitchToolStripMenuItem;

        private System.Windows.Forms.ContextMenuStrip listMenu;

        private System.Windows.Forms.Timer consumeTimer;

        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colOSVersion;

        private System.Windows.Forms.ColumnHeader colConsumingJob;

        private System.Windows.Forms.ColumnHeader colCPU;
        private System.Windows.Forms.ColumnHeader colMemory;

        private System.Windows.Forms.ColumnHeader colServerName;
        private System.Windows.Forms.ColumnHeader colVersion;

        private System.Windows.Forms.ColumnHeader colTime;

        private System.Windows.Forms.Button btnRefreshJobList;

        private System.Windows.Forms.ColumnHeader colScene;

        private System.Windows.Forms.ColumnHeader colType;

        private System.Windows.Forms.ColumnHeader colOutput;

        #endregion
        private System.Windows.Forms.SplitContainer mainView;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbRedisPassword;
        private System.Windows.Forms.TextBox tbRedisUsername;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbRedisHost;
        private System.Windows.Forms.TextBox tbRedisPort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SplitContainer mainOperation;
        private System.Windows.Forms.ListView lvServers;
        private System.Windows.Forms.ColumnHeader colIP;
        private System.Windows.Forms.ListView lvJobs;
        private System.Windows.Forms.ColumnHeader colJobId;
        private System.Windows.Forms.ColumnHeader colInput;
        private System.Windows.Forms.ColumnHeader colConsumeIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnClearRedisCache;
        public HZH_Controls.Controls.UCSwitch switchConsumeJob;
        private System.Windows.Forms.Label lblSwitch;
    }
}