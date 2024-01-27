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
            this.redisTimer = new System.Windows.Forms.Timer(this.components);
            this.container = new System.Windows.Forms.SplitContainer();
            this.lblMessageNum = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.tbRedisPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbRedisUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.tbRedisHost = new System.Windows.Forms.TextBox();
            this.tbRedisPort = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewServers = new System.Windows.Forms.ListView();
            this.colIP = new System.Windows.Forms.ColumnHeader();
            this.colInfo = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listViewJobs = new System.Windows.Forms.ListView();
            this.colJobId = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.colConsumeIP = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.container)).BeginInit();
            this.container.Panel1.SuspendLayout();
            this.container.Panel2.SuspendLayout();
            this.container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // redisTimer
            // 
            this.redisTimer.Interval = 5000;
            this.redisTimer.Tick += new System.EventHandler(this.redisTimer_Tick);
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
            this.container.Panel1.Controls.Add(this.lblMessageNum);
            this.container.Panel1.Controls.Add(this.label4);
            this.container.Panel1.Controls.Add(this.lblStatus);
            this.container.Panel1.Controls.Add(this.btnConnect);
            this.container.Panel1.Controls.Add(this.tbRedisPassword);
            this.container.Panel1.Controls.Add(this.label3);
            this.container.Panel1.Controls.Add(this.tbRedisUsername);
            this.container.Panel1.Controls.Add(this.label2);
            this.container.Panel1.Controls.Add(this.label1);
            this.container.Panel1.Controls.Add(this.lblIP);
            this.container.Panel1.Controls.Add(this.tbRedisHost);
            this.container.Panel1.Controls.Add(this.tbRedisPort);
            this.container.Panel1.Controls.Add(this.btnSave);
            // 
            // container.Panel2
            // 
            this.container.Panel2.Controls.Add(this.splitContainer1);
            this.container.Size = new System.Drawing.Size(978, 621);
            this.container.SplitterDistance = 72;
            this.container.TabIndex = 138;
            // 
            // lblMessageNum
            // 
            this.lblMessageNum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessageNum.AutoSize = true;
            this.lblMessageNum.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMessageNum.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMessageNum.Location = new System.Drawing.Point(125, 39);
            this.lblMessageNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessageNum.Name = "lblMessageNum";
            this.lblMessageNum.Size = new System.Drawing.Size(15, 17);
            this.lblMessageNum.TabIndex = 149;
            this.lblMessageNum.Text = "0";
            this.lblMessageNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(5, 39);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 17);
            this.label4.TabIndex = 148;
            this.label4.Text = "Consumed Jobs:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(5, 59);
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
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(575, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 144;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbRedisUsername
            // 
            this.tbRedisUsername.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisUsername.Location = new System.Drawing.Point(449, 6);
            this.tbRedisUsername.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisUsername.Name = "tbRedisUsername";
            this.tbRedisUsername.Size = new System.Drawing.Size(122, 23);
            this.tbRedisUsername.TabIndex = 143;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(378, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 142;
            this.label2.Text = "Username";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(240, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 141;
            this.label1.Text = "Redis Port";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIP
            // 
            this.lblIP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIP.Location = new System.Drawing.Point(13, 10);
            this.lblIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(71, 17);
            this.lblIP.TabIndex = 138;
            this.lblIP.Text = "Redis Host";
            this.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbRedisHost
            // 
            this.tbRedisHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.tbRedisHost.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisHost.Location = new System.Drawing.Point(92, 7);
            this.tbRedisHost.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisHost.Name = "tbRedisHost";
            this.tbRedisHost.Size = new System.Drawing.Size(140, 23);
            this.tbRedisHost.TabIndex = 137;
            this.tbRedisHost.Text = "127.0.0.1";
            // 
            // tbRedisPort
            // 
            this.tbRedisPort.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisPort.Location = new System.Drawing.Point(316, 7);
            this.tbRedisPort.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisPort.Name = "tbRedisPort";
            this.tbRedisPort.Size = new System.Drawing.Size(58, 23);
            this.tbRedisPort.TabIndex = 139;
            this.tbRedisPort.Text = "6379";
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
            this.splitContainer1.Panel1.Controls.Add(this.listViewServers);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewJobs);
            this.splitContainer1.Size = new System.Drawing.Size(978, 545);
            this.splitContainer1.SplitterDistance = 326;
            this.splitContainer1.TabIndex = 0;
            // 
            // listViewServers
            // 
            this.listViewServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colIP, this.colInfo });
            this.listViewServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewServers.HideSelection = false;
            this.listViewServers.LargeImageList = this.imageList1;
            this.listViewServers.Location = new System.Drawing.Point(0, 0);
            this.listViewServers.Name = "listViewServers";
            this.listViewServers.Size = new System.Drawing.Size(326, 545);
            this.listViewServers.TabIndex = 0;
            this.listViewServers.UseCompatibleStateImageBehavior = false;
            // 
            // colIP
            // 
            this.colIP.Text = "IP";
            this.colIP.Width = 200;
            // 
            // colInfo
            // 
            this.colInfo.Text = "Info";
            this.colInfo.Width = 200;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "AirdPro");
            // 
            // listViewJobs
            // 
            this.listViewJobs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.colJobId, this.colStatus, this.colConsumeIP });
            this.listViewJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewJobs.HideSelection = false;
            this.listViewJobs.Location = new System.Drawing.Point(0, 0);
            this.listViewJobs.Name = "listViewJobs";
            this.listViewJobs.Size = new System.Drawing.Size(648, 545);
            this.listViewJobs.TabIndex = 0;
            this.listViewJobs.UseCompatibleStateImageBehavior = false;
            // 
            // colJobId
            // 
            this.colJobId.Text = "JobId";
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            // 
            // colConsumeIP
            // 
            this.colConsumeIP.Text = "Consumer";
            // 
            // RedisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 621);
            this.Controls.Add(this.container);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RedisForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Redis";
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
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ImageList imageList1;

        private System.Windows.Forms.Timer redisTimer;

        #endregion
        private System.Windows.Forms.SplitContainer container;
        public System.Windows.Forms.Label lblMessageNum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox tbRedisPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbRedisUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbRedisHost;
        private System.Windows.Forms.TextBox tbRedisPort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listViewServers;
        private System.Windows.Forms.ColumnHeader colIP;
        private System.Windows.Forms.ColumnHeader colInfo;
        private System.Windows.Forms.ListView listViewJobs;
        private System.Windows.Forms.ColumnHeader colJobId;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colConsumeIP;
    }
}