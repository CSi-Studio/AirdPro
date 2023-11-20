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
            this.lblIP = new System.Windows.Forms.Label();
            this.tbRedisHost = new System.Windows.Forms.TextBox();
            this.tbRedisPort = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRedisUsername = new System.Windows.Forms.TextBox();
            this.tbRedisPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.redisConsumer = new System.Windows.Forms.Timer(this.components);
            this.lblStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblIP
            // 
            this.lblIP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIP.Location = new System.Drawing.Point(1, 7);
            this.lblIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(71, 17);
            this.lblIP.TabIndex = 29;
            this.lblIP.Text = "Redis Host";
            this.lblIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbRedisHost
            // 
            this.tbRedisHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.tbRedisHost.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisHost.Location = new System.Drawing.Point(80, 4);
            this.tbRedisHost.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisHost.Name = "tbRedisHost";
            this.tbRedisHost.Size = new System.Drawing.Size(140, 23);
            this.tbRedisHost.TabIndex = 28;
            this.tbRedisHost.Text = "127.0.0.1";
            // 
            // tbRedisPort
            // 
            this.tbRedisPort.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisPort.Location = new System.Drawing.Point(304, 4);
            this.tbRedisPort.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisPort.Name = "tbRedisPort";
            this.tbRedisPort.Size = new System.Drawing.Size(58, 23);
            this.tbRedisPort.TabIndex = 121;
            this.tbRedisPort.Text = "6379";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(454, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 26);
            this.btnSave.TabIndex = 123;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(228, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 127;
            this.label1.Text = "Redis Port";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(5, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 128;
            this.label2.Text = "Username";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbRedisUsername
            // 
            this.tbRedisUsername.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisUsername.Location = new System.Drawing.Point(80, 35);
            this.tbRedisUsername.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisUsername.Name = "tbRedisUsername";
            this.tbRedisUsername.Size = new System.Drawing.Size(135, 23);
            this.tbRedisUsername.TabIndex = 129;
            // 
            // tbRedisPassword
            // 
            this.tbRedisPassword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisPassword.Location = new System.Drawing.Point(304, 35);
            this.tbRedisPassword.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisPassword.Name = "tbRedisPassword";
            this.tbRedisPassword.Size = new System.Drawing.Size(130, 23);
            this.tbRedisPassword.TabIndex = 131;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(232, 38);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 130;
            this.label3.Text = "Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnConnect
            // 
            this.btnConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConnect.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConnect.Location = new System.Drawing.Point(454, 33);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(94, 26);
            this.btnConnect.TabIndex = 133;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // redisConsumer
            // 
            this.redisConsumer.Interval = 3000;
            this.redisConsumer.Tick += new System.EventHandler(this.redisConsumer_Tick);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.BackColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(5, 62);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(543, 5);
            this.lblStatus.TabIndex = 134;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(4, 80);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 17);
            this.label4.TabIndex = 135;
            this.label4.Text = "Consumed Jobs:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(113, 80);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 17);
            this.label5.TabIndex = 136;
            this.label5.Text = "0";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RedisForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 175);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.tbRedisPassword);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbRedisUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.tbRedisHost);
            this.Controls.Add(this.tbRedisPort);
            this.Controls.Add(this.btnSave);
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
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Timer redisConsumer;

        private System.Windows.Forms.Button btnConnect;

        #endregion
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbRedisHost;
        private System.Windows.Forms.TextBox tbRedisPort;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbRedisUsername;
        private System.Windows.Forms.TextBox tbRedisPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatus;
    }
}