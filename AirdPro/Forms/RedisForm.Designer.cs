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
            this.heartBeatTimer = new System.Windows.Forms.Timer(this.components);
            this.mainView = new System.Windows.Forms.SplitContainer();
            this.btnRefreshJobList = new System.Windows.Forms.Button();
            this.btnConsume = new System.Windows.Forms.Button();
            this.btnClearServerCache = new System.Windows.Forms.Button();
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
            this.mainList = new System.Windows.Forms.SplitContainer();
            this.lvServers = new System.Windows.Forms.ListView();
            this.colIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbConsole = new System.Windows.Forms.TextBox();
            this.lvJobs = new System.Windows.Forms.ListView();
            this.colJobId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colScene = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colConsumeIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.mainView)).BeginInit();
            this.mainView.Panel1.SuspendLayout();
            this.mainView.Panel2.SuspendLayout();
            this.mainView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainOperation)).BeginInit();
            this.mainOperation.Panel1.SuspendLayout();
            this.mainOperation.Panel2.SuspendLayout();
            this.mainOperation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainList)).BeginInit();
            this.mainList.Panel1.SuspendLayout();
            this.mainList.Panel2.SuspendLayout();
            this.mainList.SuspendLayout();
            this.SuspendLayout();
            // 
            // heartBeatTimer
            // 
            this.heartBeatTimer.Enabled = true;
            this.heartBeatTimer.Interval = 3000;
            this.heartBeatTimer.Tick += new System.EventHandler(this.redisTimer_Tick);
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
            this.mainView.Panel1.Controls.Add(this.btnRefreshJobList);
            this.mainView.Panel1.Controls.Add(this.btnConsume);
            this.mainView.Panel1.Controls.Add(this.btnClearServerCache);
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
            // btnRefreshJobList
            // 
            this.btnRefreshJobList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefreshJobList.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRefreshJobList.FlatAppearance.BorderSize = 0;
            this.btnRefreshJobList.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRefreshJobList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefreshJobList.Location = new System.Drawing.Point(732, 40);
            this.btnRefreshJobList.Name = "btnRefreshJobList";
            this.btnRefreshJobList.Size = new System.Drawing.Size(80, 26);
            this.btnRefreshJobList.TabIndex = 156;
            this.btnRefreshJobList.Text = "Refresh";
            this.btnRefreshJobList.UseVisualStyleBackColor = true;
            this.btnRefreshJobList.Click += new System.EventHandler(this.btnRefreshJobList_Click);
            // 
            // btnConsume
            // 
            this.btnConsume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConsume.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnConsume.FlatAppearance.BorderSize = 0;
            this.btnConsume.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConsume.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConsume.Location = new System.Drawing.Point(818, 40);
            this.btnConsume.Name = "btnConsume";
            this.btnConsume.Size = new System.Drawing.Size(157, 26);
            this.btnConsume.TabIndex = 155;
            this.btnConsume.Text = "Start Consuming Task";
            this.btnConsume.UseVisualStyleBackColor = true;
            this.btnConsume.Click += new System.EventHandler(this.btnConsume_Click);
            // 
            // btnClearServerCache
            // 
            this.btnClearServerCache.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnClearServerCache.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClearServerCache.FlatAppearance.BorderSize = 0;
            this.btnClearServerCache.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnClearServerCache.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClearServerCache.Location = new System.Drawing.Point(8, 40);
            this.btnClearServerCache.Name = "btnClearServerCache";
            this.btnClearServerCache.Size = new System.Drawing.Size(152, 26);
            this.btnClearServerCache.TabIndex = 154;
            this.btnClearServerCache.Text = "Clear Server Cache";
            this.btnClearServerCache.UseVisualStyleBackColor = true;
            this.btnClearServerCache.Click += new System.EventHandler(this.btnClearServerCache_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(532, 7);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(105, 20);
            this.lblPassword.TabIndex = 153;
            this.lblPassword.Text = "Password";
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(352, 9);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(44, 20);
            this.lblUser.TabIndex = 152;
            this.lblUser.Text = "User";
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(221, 9);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(44, 20);
            this.lblPort.TabIndex = 151;
            this.lblPort.Text = "Port";
            // 
            // lblIP
            // 
            this.lblIP.Location = new System.Drawing.Point(12, 9);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(55, 20);
            this.lblIP.TabIndex = 150;
            this.lblIP.Text = "Host IP";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.tbRedisPassword.Location = new System.Drawing.Point(644, 6);
            this.tbRedisPassword.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisPassword.Name = "tbRedisPassword";
            this.tbRedisPassword.Size = new System.Drawing.Size(130, 23);
            this.tbRedisPassword.TabIndex = 145;
            // 
            // tbRedisUsername
            // 
            this.tbRedisUsername.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisUsername.Location = new System.Drawing.Point(403, 6);
            this.tbRedisUsername.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisUsername.Name = "tbRedisUsername";
            this.tbRedisUsername.Size = new System.Drawing.Size(122, 23);
            this.tbRedisUsername.TabIndex = 143;
            // 
            // tbRedisHost
            // 
            this.tbRedisHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tbRedisHost.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisHost.Location = new System.Drawing.Point(74, 7);
            this.tbRedisHost.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisHost.Name = "tbRedisHost";
            this.tbRedisHost.Size = new System.Drawing.Size(140, 23);
            this.tbRedisHost.TabIndex = 137;
            // 
            // tbRedisPort
            // 
            this.tbRedisPort.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisPort.Location = new System.Drawing.Point(272, 6);
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
            this.mainOperation.Panel1.Controls.Add(this.mainList);
            // 
            // mainOperation.Panel2
            // 
            this.mainOperation.Panel2.Controls.Add(this.tbConsole);
            this.mainOperation.Size = new System.Drawing.Size(978, 549);
            this.mainOperation.SplitterDistance = 367;
            this.mainOperation.TabIndex = 0;
            // 
            // mainList
            // 
            this.mainList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainList.Location = new System.Drawing.Point(0, 0);
            this.mainList.Name = "mainList";
            // 
            // mainList.Panel1
            // 
            this.mainList.Panel1.Controls.Add(this.lvServers);
            // 
            // mainList.Panel2
            // 
            this.mainList.Panel2.Controls.Add(this.lvJobs);
            this.mainList.Size = new System.Drawing.Size(978, 367);
            this.mainList.SplitterDistance = 154;
            this.mainList.TabIndex = 0;
            // 
            // lvServers
            // 
            this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIP});
            this.lvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvServers.HideSelection = false;
            this.lvServers.Location = new System.Drawing.Point(0, 0);
            this.lvServers.MultiSelect = false;
            this.lvServers.Name = "lvServers";
            this.lvServers.Size = new System.Drawing.Size(154, 367);
            this.lvServers.TabIndex = 0;
            this.lvServers.UseCompatibleStateImageBehavior = false;
            this.lvServers.View = System.Windows.Forms.View.Details;
            this.lvServers.SelectedIndexChanged += new System.EventHandler(this.listViewServers_SelectedIndexChanged);
            // 
            // colIP
            // 
            this.colIP.Text = "IP";
            this.colIP.Width = 400;
            // 
            // tbConsole
            // 
            this.tbConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConsole.Location = new System.Drawing.Point(3, 3);
            this.tbConsole.Multiline = true;
            this.tbConsole.Name = "tbConsole";
            this.tbConsole.ReadOnly = true;
            this.tbConsole.Size = new System.Drawing.Size(972, 172);
            this.tbConsole.TabIndex = 0;
            // 
            // lvJobs
            // 
            this.lvJobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colJobId,
            this.colType,
            this.colScene,
            this.colInput,
            this.colOutput,
            this.colConsumeIP,
            this.colTime});
            this.lvJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvJobs.FullRowSelect = true;
            this.lvJobs.HideSelection = false;
            this.lvJobs.Location = new System.Drawing.Point(0, 0);
            this.lvJobs.Name = "lvJobs";
            this.lvJobs.Size = new System.Drawing.Size(820, 367);
            this.lvJobs.TabIndex = 0;
            this.lvJobs.UseCompatibleStateImageBehavior = false;
            this.lvJobs.View = System.Windows.Forms.View.Details;
            this.lvJobs.SelectedIndexChanged += new System.EventHandler(this.lvJobs_SelectedIndexChanged);
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
            // colInput
            // 
            this.colInput.Text = "Input Path";
            this.colInput.Width = 200;
            // 
            // colOutput
            // 
            this.colOutput.Text = "Output Path";
            this.colOutput.Width = 120;
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
            this.mainOperation.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainOperation)).EndInit();
            this.mainOperation.ResumeLayout(false);
            this.mainList.Panel1.ResumeLayout(false);
            this.mainList.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainList)).EndInit();
            this.mainList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.ColumnHeader colTime;

        private System.Windows.Forms.Button btnRefreshJobList;

        private System.Windows.Forms.ColumnHeader colScene;

        private System.Windows.Forms.ColumnHeader colType;

        private System.Windows.Forms.ColumnHeader colOutput;

        private System.Windows.Forms.Button btnConsume;

        private System.Windows.Forms.TextBox tbConsole;

        private System.Windows.Forms.Timer heartBeatTimer;

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
        private System.Windows.Forms.SplitContainer mainList;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnClearServerCache;
    }
}