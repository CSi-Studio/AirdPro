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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalSettingForm));
            this.lblIP = new System.Windows.Forms.Label();
            this.tbRedisHost = new System.Windows.Forms.TextBox();
            this.lblConfigOutputPath = new System.Windows.Forms.Label();
            this.btnConfigChooseFolder = new System.Windows.Forms.Button();
            this.tbLastOpenPath = new System.Windows.Forms.TextBox();
            this.tbRedisPort = new System.Windows.Forms.TextBox();
            this.lblMaohao = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblMaxTasks = new System.Windows.Forms.Label();
            this.numMaxTasks = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // lblIP
            // 
            this.lblIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblIP.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblIP.Location = new System.Drawing.Point(13, 9);
            this.lblIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(43, 17);
            this.lblIP.TabIndex = 29;
            this.lblIP.Text = "Redis:";
            // 
            // tbRedisHost
            // 
            this.tbRedisHost.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisHost.Location = new System.Drawing.Point(64, 6);
            this.tbRedisHost.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisHost.Name = "tbRedisHost";
            this.tbRedisHost.Size = new System.Drawing.Size(140, 23);
            this.tbRedisHost.TabIndex = 28;
            this.tbRedisHost.Text = "127.0.0.1";
            // 
            // lblConfigOutputPath
            // 
            this.lblConfigOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfigOutputPath.AutoSize = true;
            this.lblConfigOutputPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConfigOutputPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblConfigOutputPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblConfigOutputPath.Location = new System.Drawing.Point(13, 37);
            this.lblConfigOutputPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfigOutputPath.Name = "lblConfigOutputPath";
            this.lblConfigOutputPath.Size = new System.Drawing.Size(99, 17);
            this.lblConfigOutputPath.TabIndex = 119;
            this.lblConfigOutputPath.Text = "Last Open Path:";
            // 
            // btnConfigChooseFolder
            // 
            this.btnConfigChooseFolder.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnConfigChooseFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfigChooseFolder.Location = new System.Drawing.Point(368, 34);
            this.btnConfigChooseFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfigChooseFolder.Name = "btnConfigChooseFolder";
            this.btnConfigChooseFolder.Size = new System.Drawing.Size(76, 24);
            this.btnConfigChooseFolder.TabIndex = 120;
            this.btnConfigChooseFolder.Text = "Browser";
            this.btnConfigChooseFolder.UseVisualStyleBackColor = true;
            this.btnConfigChooseFolder.Click += new System.EventHandler(this.btnConfigChooseFolder_Click);
            // 
            // tbLastOpenPath
            // 
            this.tbLastOpenPath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tbLastOpenPath.Location = new System.Drawing.Point(120, 34);
            this.tbLastOpenPath.Margin = new System.Windows.Forms.Padding(4);
            this.tbLastOpenPath.Name = "tbLastOpenPath";
            this.tbLastOpenPath.Size = new System.Drawing.Size(240, 23);
            this.tbLastOpenPath.TabIndex = 118;
            // 
            // tbRedisPort
            // 
            this.tbRedisPort.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbRedisPort.Location = new System.Drawing.Point(223, 6);
            this.tbRedisPort.Margin = new System.Windows.Forms.Padding(4);
            this.tbRedisPort.Name = "tbRedisPort";
            this.tbRedisPort.Size = new System.Drawing.Size(66, 23);
            this.tbRedisPort.TabIndex = 121;
            this.tbRedisPort.Text = "6379";
            // 
            // lblMaohao
            // 
            this.lblMaohao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaohao.AutoSize = true;
            this.lblMaohao.Font = new System.Drawing.Font("微软雅黑", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMaohao.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMaohao.Location = new System.Drawing.Point(208, 10);
            this.lblMaohao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaohao.Name = "lblMaohao";
            this.lblMaohao.Size = new System.Drawing.Size(10, 14);
            this.lblMaohao.TabIndex = 122;
            this.lblMaohao.Text = ":";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 8F);
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(362, 111);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 29);
            this.btnSave.TabIndex = 123;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblMaxTasks
            // 
            this.lblMaxTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaxTasks.AutoSize = true;
            this.lblMaxTasks.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMaxTasks.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMaxTasks.Location = new System.Drawing.Point(13, 66);
            this.lblMaxTasks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxTasks.Name = "lblMaxTasks";
            this.lblMaxTasks.Size = new System.Drawing.Size(234, 17);
            this.lblMaxTasks.TabIndex = 125;
            this.lblMaxTasks.Text = "Max Tasks(Program Restart Required):";
            // 
            // numMaxTasks
            // 
            this.numMaxTasks.Location = new System.Drawing.Point(254, 64);
            this.numMaxTasks.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numMaxTasks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxTasks.Name = "numMaxTasks";
            this.numMaxTasks.Size = new System.Drawing.Size(70, 23);
            this.numMaxTasks.TabIndex = 126;
            this.numMaxTasks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // GlobalSettingForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 152);
            this.Controls.Add(this.numMaxTasks);
            this.Controls.Add(this.lblMaxTasks);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblMaohao);
            this.Controls.Add(this.tbRedisPort);
            this.Controls.Add(this.lblConfigOutputPath);
            this.Controls.Add(this.btnConfigChooseFolder);
            this.Controls.Add(this.tbLastOpenPath);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.tbRedisHost);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GlobalSettingForm";
            this.ShowIcon = false;
            this.Text = "Global Settings";
            this.Load += new System.EventHandler(this.GlobalSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTasks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.TextBox tbRedisHost;
        private System.Windows.Forms.Label lblConfigOutputPath;
        private System.Windows.Forms.Button btnConfigChooseFolder;
        public System.Windows.Forms.TextBox tbLastOpenPath;
        private System.Windows.Forms.TextBox tbRedisPort;
        private System.Windows.Forms.Label lblMaohao;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblMaxTasks;
        private System.Windows.Forms.NumericUpDown numMaxTasks;
    }
}