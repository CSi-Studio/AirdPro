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
            this.container = new System.Windows.Forms.SplitContainer();
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvServers = new System.Windows.Forms.ListView();
            this.colIP = new System.Windows.Forms.ColumnHeader();
            this.tbServerInfo = new System.Windows.Forms.TextBox();
            this.lvJobs = new System.Windows.Forms.ListView();
            this.colJobId = new System.Windows.Forms.ColumnHeader();
            this.colType = new System.Windows.Forms.ColumnHeader();
            this.colInput = new System.Windows.Forms.ColumnHeader();
            this.colOutput = new System.Windows.Forms.ColumnHeader();
            this.colConsumeIP = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.container)).BeginInit();
            this.container.Panel1.SuspendLayout();
            this.container.Panel2.SuspendLayout();
            this.container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // heartBeatTimer
            // 
            this.heartBeatTimer.Interval = 3000;
            this.heartBeatTimer.Tick += new System.EventHandler(this.redisTimer_Tick);
            // 
            // container
            // 
            this.container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.container.Location = new System.Drawing.Point(0, 0);
            this.container.Name = "container";
            this.container.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // container.Panel1
            // 
            this.container.Panel1.Controls.Add(this.btnConsume);
            this.container.Panel1.Controls.Add(this.btnClearServerCache);
            this.container.Panel1.Controls.Add(this.lblPassword);
            this.container.Panel1.Controls.Add(this.lblUser);
            this.container.Panel1.Controls.Add(this.lblPort);
            this.container.Panel1.Controls.Add(this.lblIP);
            this.container.Panel1.Controls.Add(this.lblStatus);
            this.container.Panel1.Controls.Add(this.btnConnect);
            this.container.Panel1.Controls.Add(this.tbRedisPassword);
            this.container.Panel1.Controls.Add(this.tbRedisUsername);
            this.container.Panel1.Controls.Add(this.tbRedisHost);
            this.container.Panel1.Controls.Add(this.tbRedisPort);
            this.container.Panel1.Controls.Add(this.btnSave);
            // 
            // container.Panel2
            // 
            this.container.Panel2.Controls.Add(this.splitContainer1);
            this.container.Size = new System.Drawing.Size(978, 621);
            this.container.SplitterDistance = 68;
            this.container.TabIndex = 138;
            // 
            // btnConsume
            // 
            this.btnConsume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConsume.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnConsume.FlatAppearance.BorderSize = 0;
            this.btnConsume.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConsume.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConsume.Location = new System.Drawing.Point(744, 40);
            this.btnConsume.Name = "btnConsume";
            this.btnConsume.Size = new System.Drawing.Size(231, 26);
            this.btnConsume.TabIndex = 155;
            this.btnConsume.Text = "Start Consuming Task";
            this.btnConsume.UseVisualStyleBackColor = true;
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
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(8, 33);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(970, 5);
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
            this.tbRedisHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
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
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvJobs);
            this.splitContainer1.Size = new System.Drawing.Size(978, 549);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvServers);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tbServerInfo);
            this.splitContainer2.Size = new System.Drawing.Size(205, 549);
            this.splitContainer2.SplitterDistance = 312;
            this.splitContainer2.TabIndex = 0;
            // 
            // lvServers
            // 
            this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colIP });
            this.lvServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvServers.HideSelection = false;
            this.lvServers.LabelEdit = true;
            this.lvServers.Location = new System.Drawing.Point(0, 0);
            this.lvServers.MultiSelect = false;
            this.lvServers.Name = "lvServers";
            this.lvServers.Size = new System.Drawing.Size(205, 312);
            this.lvServers.TabIndex = 0;
            this.lvServers.UseCompatibleStateImageBehavior = false;
            this.lvServers.View = System.Windows.Forms.View.Details;
            this.lvServers.SelectedIndexChanged += new System.EventHandler(this.listViewServers_SelectedIndexChanged);
            // 
            // colIP
            // 
            this.colIP.Text = "IP";
            this.colIP.Width = 200;
            // 
            // tbServerInfo
            // 
            this.tbServerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerInfo.Location = new System.Drawing.Point(3, 3);
            this.tbServerInfo.Multiline = true;
            this.tbServerInfo.Name = "tbServerInfo";
            this.tbServerInfo.ReadOnly = true;
            this.tbServerInfo.Size = new System.Drawing.Size(199, 227);
            this.tbServerInfo.TabIndex = 0;
            // 
            // lvJobs
            // 
            this.lvJobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colJobId, this.colType, this.colInput, this.colOutput, this.colConsumeIP });
            this.lvJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvJobs.FullRowSelect = true;
            this.lvJobs.HideSelection = false;
            this.lvJobs.LabelEdit = true;
            this.lvJobs.Location = new System.Drawing.Point(0, 0);
            this.lvJobs.Name = "lvJobs";
            this.lvJobs.Size = new System.Drawing.Size(769, 549);
            this.lvJobs.TabIndex = 0;
            this.lvJobs.UseCompatibleStateImageBehavior = false;
            this.lvJobs.View = System.Windows.Forms.View.Details;
            // 
            // colJobId
            // 
            this.colJobId.Text = "JobId";
            this.colJobId.Width = 112;
            // 
            // colType
            // 
            this.colType.Text = "Type";
            this.colType.Width = 89;
            // 
            // colInput
            // 
            this.colInput.Text = "Input Path";
            this.colInput.Width = 129;
            // 
            // colOutput
            // 
            this.colOutput.Text = "Output Path";
            this.colOutput.Width = 161;
            // 
            // colConsumeIP
            // 
            this.colConsumeIP.Text = "Consumer IP";
            this.colConsumeIP.Width = 154;
            // 
            // RedisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(978, 621);
            this.Controls.Add(this.container);
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
            this.container.Panel1.ResumeLayout(false);
            this.container.Panel1.PerformLayout();
            this.container.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.container)).EndInit();
            this.container.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ColumnHeader colType;

        private System.Windows.Forms.ColumnHeader colOutput;

        private System.Windows.Forms.Button btnConsume;

        private System.Windows.Forms.TextBox tbServerInfo;

        private System.Windows.Forms.Timer heartBeatTimer;

        #endregion
        private System.Windows.Forms.SplitContainer container;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbRedisPassword;
        private System.Windows.Forms.TextBox tbRedisUsername;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbRedisHost;
        private System.Windows.Forms.TextBox tbRedisPort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvServers;
        private System.Windows.Forms.ColumnHeader colIP;
        private System.Windows.Forms.ListView lvJobs;
        private System.Windows.Forms.ColumnHeader colJobId;
        private System.Windows.Forms.ColumnHeader colInput;
        private System.Windows.Forms.ColumnHeader colConsumeIP;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnClearServerCache;
    }
}