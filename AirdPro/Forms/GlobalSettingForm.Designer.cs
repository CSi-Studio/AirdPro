namespace AirdPro.Forms
{
    partial class GlobalSettingForm
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
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblIP = new System.Windows.Forms.Label();
            this.tbHostAndPort = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblConnectStatus = new System.Windows.Forms.Label();
            this.timerConsumer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDisconnect.Location = new System.Drawing.Point(346, 6);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(103, 27);
            this.btnDisconnect.TabIndex = 30;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lblIP
            // 
            this.lblIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIP.AutoSize = true;
            this.lblIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIP.Location = new System.Drawing.Point(12, 13);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(35, 12);
            this.lblIP.TabIndex = 29;
            this.lblIP.Text = "Redis";
            // 
            // tbHostAndPort
            // 
            this.tbHostAndPort.Location = new System.Drawing.Point(53, 9);
            this.tbHostAndPort.Name = "tbHostAndPort";
            this.tbHostAndPort.Size = new System.Drawing.Size(176, 21);
            this.tbHostAndPort.TabIndex = 28;
            this.tbHostAndPort.Text = "192.168.31.88:6379";
            // 
            // btnConnect
            // 
            this.btnConnect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConnect.Location = new System.Drawing.Point(235, 6);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(105, 26);
            this.btnConnect.TabIndex = 27;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblConnectStatus
            // 
            this.lblConnectStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectStatus.AutoSize = true;
            this.lblConnectStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConnectStatus.Location = new System.Drawing.Point(12, 34);
            this.lblConnectStatus.Name = "lblConnectStatus";
            this.lblConnectStatus.Size = new System.Drawing.Size(71, 12);
            this.lblConnectStatus.TabIndex = 31;
            this.lblConnectStatus.Text = "Not Connect";
            // 
            // timerConsumer
            // 
            this.timerConsumer.Interval = 3000;
            this.timerConsumer.Tick += new System.EventHandler(this.timerConsumer_Tick);
            // 
            // GlobalSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 159);
            this.Controls.Add(this.lblConnectStatus);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.tbHostAndPort);
            this.Controls.Add(this.btnConnect);
            this.Name = "GlobalSettingForm";
            this.Text = "Global Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbHostAndPort;
        private System.Windows.Forms.Button btnConnect;
        public System.Windows.Forms.Label lblConnectStatus;
        private System.Windows.Forms.Timer timerConsumer;
    }
}