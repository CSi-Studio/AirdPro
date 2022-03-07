namespace AirdPro.Forms
{
    partial class JobDetailForm
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
            this.lblInputFilePath = new System.Windows.Forms.Label();
            this.tbInputFilePath = new System.Windows.Forms.TextBox();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.lblFileName = new System.Windows.Forms.Label();
            this.tbFileType = new System.Windows.Forms.TextBox();
            this.lblFileType = new System.Windows.Forms.Label();
            this.tbOutputFilePath = new System.Windows.Forms.TextBox();
            this.lblOutputFilePath = new System.Windows.Forms.Label();
            this.tbZeroIntensity = new System.Windows.Forms.TextBox();
            this.lblIsIgnoreZeroIntensity = new System.Windows.Forms.Label();
            this.tbMultithreading = new System.Windows.Forms.TextBox();
            this.lblMultithreading = new System.Windows.Forms.Label();
            this.tbMzPrecision = new System.Windows.Forms.TextBox();
            this.lblMzPrecision = new System.Windows.Forms.Label();
            this.tbMzIntCompressor = new System.Windows.Forms.TextBox();
            this.lblMzIntCompressor = new System.Windows.Forms.Label();
            this.tbMzByteCompressor = new System.Windows.Forms.TextBox();
            this.lblMzByteCompressor = new System.Windows.Forms.Label();
            this.tbIntensityByteCompressor = new System.Windows.Forms.TextBox();
            this.lblIntensityByteCompressor = new System.Windows.Forms.Label();
            this.tbStackLayers = new System.Windows.Forms.TextBox();
            this.lblStackLayers = new System.Windows.Forms.Label();
            this.tbFileSuffix = new System.Windows.Forms.TextBox();
            this.lblFileSuffix = new System.Windows.Forms.Label();
            this.tbOperator = new System.Windows.Forms.TextBox();
            this.lblOperator = new System.Windows.Forms.Label();
            this.tbStackStatus = new System.Windows.Forms.TextBox();
            this.lblStackStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblInputFilePath
            // 
            this.lblInputFilePath.AutoSize = true;
            this.lblInputFilePath.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblInputFilePath.Location = new System.Drawing.Point(423, 221);
            this.lblInputFilePath.Name = "lblInputFilePath";
            this.lblInputFilePath.Size = new System.Drawing.Size(114, 20);
            this.lblInputFilePath.TabIndex = 0;
            this.lblInputFilePath.Text = "Input File Path :";
            // 
            // tbInputFilePath
            // 
            this.tbInputFilePath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbInputFilePath.Location = new System.Drawing.Point(538, 221);
            this.tbInputFilePath.Name = "tbInputFilePath";
            this.tbInputFilePath.ReadOnly = true;
            this.tbInputFilePath.Size = new System.Drawing.Size(250, 23);
            this.tbInputFilePath.TabIndex = 1;
            // 
            // tbFileName
            // 
            this.tbFileName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbFileName.Location = new System.Drawing.Point(88, 41);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.ReadOnly = true;
            this.tbFileName.Size = new System.Drawing.Size(127, 23);
            this.tbFileName.TabIndex = 3;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblFileName.Location = new System.Drawing.Point(4, 41);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(83, 20);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "File Name :";
            // 
            // tbFileType
            // 
            this.tbFileType.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbFileType.Location = new System.Drawing.Point(88, 86);
            this.tbFileType.Name = "tbFileType";
            this.tbFileType.ReadOnly = true;
            this.tbFileType.Size = new System.Drawing.Size(127, 23);
            this.tbFileType.TabIndex = 5;
            // 
            // lblFileType
            // 
            this.lblFileType.AutoSize = true;
            this.lblFileType.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblFileType.Location = new System.Drawing.Point(12, 86);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(75, 20);
            this.lblFileType.TabIndex = 4;
            this.lblFileType.Text = "File Type :";
            // 
            // tbOutputFilePath
            // 
            this.tbOutputFilePath.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbOutputFilePath.Location = new System.Drawing.Point(139, 221);
            this.tbOutputFilePath.Name = "tbOutputFilePath";
            this.tbOutputFilePath.ReadOnly = true;
            this.tbOutputFilePath.Size = new System.Drawing.Size(250, 23);
            this.tbOutputFilePath.TabIndex = 7;
            // 
            // lblOutputFilePath
            // 
            this.lblOutputFilePath.AutoSize = true;
            this.lblOutputFilePath.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblOutputFilePath.Location = new System.Drawing.Point(12, 221);
            this.lblOutputFilePath.Name = "lblOutputFilePath";
            this.lblOutputFilePath.Size = new System.Drawing.Size(126, 20);
            this.lblOutputFilePath.TabIndex = 6;
            this.lblOutputFilePath.Text = "Output File Path :";
            // 
            // tbZeroIntensity
            // 
            this.tbZeroIntensity.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbZeroIntensity.Location = new System.Drawing.Point(337, 41);
            this.tbZeroIntensity.Name = "tbZeroIntensity";
            this.tbZeroIntensity.ReadOnly = true;
            this.tbZeroIntensity.Size = new System.Drawing.Size(127, 23);
            this.tbZeroIntensity.TabIndex = 9;
            // 
            // lblIsIgnoreZeroIntensity
            // 
            this.lblIsIgnoreZeroIntensity.AutoSize = true;
            this.lblIsIgnoreZeroIntensity.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblIsIgnoreZeroIntensity.Location = new System.Drawing.Point(230, 41);
            this.lblIsIgnoreZeroIntensity.Name = "lblIsIgnoreZeroIntensity";
            this.lblIsIgnoreZeroIntensity.Size = new System.Drawing.Size(108, 20);
            this.lblIsIgnoreZeroIntensity.TabIndex = 8;
            this.lblIsIgnoreZeroIntensity.Text = "Zero Intensity :";
            // 
            // tbMultithreading
            // 
            this.tbMultithreading.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMultithreading.Location = new System.Drawing.Point(337, 86);
            this.tbMultithreading.Name = "tbMultithreading";
            this.tbMultithreading.ReadOnly = true;
            this.tbMultithreading.Size = new System.Drawing.Size(127, 23);
            this.tbMultithreading.TabIndex = 11;
            // 
            // lblMultithreading
            // 
            this.lblMultithreading.AutoSize = true;
            this.lblMultithreading.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblMultithreading.Location = new System.Drawing.Point(220, 86);
            this.lblMultithreading.Name = "lblMultithreading";
            this.lblMultithreading.Size = new System.Drawing.Size(118, 20);
            this.lblMultithreading.TabIndex = 10;
            this.lblMultithreading.Text = "Multithreading :";
            // 
            // tbMzPrecision
            // 
            this.tbMzPrecision.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMzPrecision.Location = new System.Drawing.Point(337, 131);
            this.tbMzPrecision.Name = "tbMzPrecision";
            this.tbMzPrecision.ReadOnly = true;
            this.tbMzPrecision.Size = new System.Drawing.Size(127, 23);
            this.tbMzPrecision.TabIndex = 13;
            // 
            // lblMzPrecision
            // 
            this.lblMzPrecision.AutoSize = true;
            this.lblMzPrecision.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblMzPrecision.Location = new System.Drawing.Point(237, 131);
            this.lblMzPrecision.Name = "lblMzPrecision";
            this.lblMzPrecision.Size = new System.Drawing.Size(101, 20);
            this.lblMzPrecision.TabIndex = 12;
            this.lblMzPrecision.Text = "mz Precision :";
            // 
            // tbMzIntCompressor
            // 
            this.tbMzIntCompressor.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMzIntCompressor.Location = new System.Drawing.Point(659, 41);
            this.tbMzIntCompressor.Name = "tbMzIntCompressor";
            this.tbMzIntCompressor.ReadOnly = true;
            this.tbMzIntCompressor.Size = new System.Drawing.Size(127, 23);
            this.tbMzIntCompressor.TabIndex = 15;
            // 
            // lblMzIntCompressor
            // 
            this.lblMzIntCompressor.AutoSize = true;
            this.lblMzIntCompressor.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblMzIntCompressor.Location = new System.Drawing.Point(518, 41);
            this.lblMzIntCompressor.Name = "lblMzIntCompressor";
            this.lblMzIntCompressor.Size = new System.Drawing.Size(141, 20);
            this.lblMzIntCompressor.TabIndex = 14;
            this.lblMzIntCompressor.Text = "mz int Compressor :";
            // 
            // tbMzByteCompressor
            // 
            this.tbMzByteCompressor.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMzByteCompressor.Location = new System.Drawing.Point(659, 86);
            this.tbMzByteCompressor.Name = "tbMzByteCompressor";
            this.tbMzByteCompressor.ReadOnly = true;
            this.tbMzByteCompressor.Size = new System.Drawing.Size(127, 23);
            this.tbMzByteCompressor.TabIndex = 17;
            // 
            // lblMzByteCompressor
            // 
            this.lblMzByteCompressor.AutoSize = true;
            this.lblMzByteCompressor.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblMzByteCompressor.Location = new System.Drawing.Point(507, 86);
            this.lblMzByteCompressor.Name = "lblMzByteCompressor";
            this.lblMzByteCompressor.Size = new System.Drawing.Size(152, 20);
            this.lblMzByteCompressor.TabIndex = 16;
            this.lblMzByteCompressor.Text = "mz byte Compressor :";
            // 
            // tbIntensityByteCompressor
            // 
            this.tbIntensityByteCompressor.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbIntensityByteCompressor.Location = new System.Drawing.Point(659, 131);
            this.tbIntensityByteCompressor.Name = "tbIntensityByteCompressor";
            this.tbIntensityByteCompressor.ReadOnly = true;
            this.tbIntensityByteCompressor.Size = new System.Drawing.Size(127, 23);
            this.tbIntensityByteCompressor.TabIndex = 19;
            // 
            // lblIntensityByteCompressor
            // 
            this.lblIntensityByteCompressor.AutoSize = true;
            this.lblIntensityByteCompressor.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblIntensityByteCompressor.Location = new System.Drawing.Point(470, 131);
            this.lblIntensityByteCompressor.Name = "lblIntensityByteCompressor";
            this.lblIntensityByteCompressor.Size = new System.Drawing.Size(189, 20);
            this.lblIntensityByteCompressor.TabIndex = 18;
            this.lblIntensityByteCompressor.Text = "Intensity Byte Compressor :";
            // 
            // tbStackLayers
            // 
            this.tbStackLayers.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbStackLayers.Location = new System.Drawing.Point(659, 176);
            this.tbStackLayers.Name = "tbStackLayers";
            this.tbStackLayers.ReadOnly = true;
            this.tbStackLayers.Size = new System.Drawing.Size(127, 23);
            this.tbStackLayers.TabIndex = 21;
            // 
            // lblStackLayers
            // 
            this.lblStackLayers.AutoSize = true;
            this.lblStackLayers.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblStackLayers.Location = new System.Drawing.Point(562, 176);
            this.lblStackLayers.Name = "lblStackLayers";
            this.lblStackLayers.Size = new System.Drawing.Size(97, 20);
            this.lblStackLayers.TabIndex = 20;
            this.lblStackLayers.Text = "Stack Layers :";
            // 
            // tbFileSuffix
            // 
            this.tbFileSuffix.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbFileSuffix.Location = new System.Drawing.Point(88, 131);
            this.tbFileSuffix.Name = "tbFileSuffix";
            this.tbFileSuffix.ReadOnly = true;
            this.tbFileSuffix.Size = new System.Drawing.Size(127, 23);
            this.tbFileSuffix.TabIndex = 23;
            // 
            // lblFileSuffix
            // 
            this.lblFileSuffix.AutoSize = true;
            this.lblFileSuffix.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblFileSuffix.Location = new System.Drawing.Point(6, 131);
            this.lblFileSuffix.Name = "lblFileSuffix";
            this.lblFileSuffix.Size = new System.Drawing.Size(81, 20);
            this.lblFileSuffix.TabIndex = 22;
            this.lblFileSuffix.Text = "File Suffix :";
            // 
            // tbOperator
            // 
            this.tbOperator.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbOperator.Location = new System.Drawing.Point(88, 176);
            this.tbOperator.Name = "tbOperator";
            this.tbOperator.ReadOnly = true;
            this.tbOperator.Size = new System.Drawing.Size(127, 23);
            this.tbOperator.TabIndex = 25;
            // 
            // lblOperator
            // 
            this.lblOperator.AutoSize = true;
            this.lblOperator.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblOperator.Location = new System.Drawing.Point(12, 176);
            this.lblOperator.Name = "lblOperator";
            this.lblOperator.Size = new System.Drawing.Size(76, 20);
            this.lblOperator.TabIndex = 24;
            this.lblOperator.Text = "Operator :";
            // 
            // tbStackStatus
            // 
            this.tbStackStatus.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbStackStatus.Location = new System.Drawing.Point(337, 176);
            this.tbStackStatus.Name = "tbStackStatus";
            this.tbStackStatus.ReadOnly = true;
            this.tbStackStatus.Size = new System.Drawing.Size(127, 23);
            this.tbStackStatus.TabIndex = 27;
            // 
            // lblStackStatus
            // 
            this.lblStackStatus.AutoSize = true;
            this.lblStackStatus.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblStackStatus.Location = new System.Drawing.Point(241, 176);
            this.lblStackStatus.Name = "lblStackStatus";
            this.lblStackStatus.Size = new System.Drawing.Size(97, 20);
            this.lblStackStatus.TabIndex = 26;
            this.lblStackStatus.Text = "Stack Status :";
            // 
            // JobDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 278);
            this.Controls.Add(this.tbStackStatus);
            this.Controls.Add(this.lblStackStatus);
            this.Controls.Add(this.tbOperator);
            this.Controls.Add(this.lblOperator);
            this.Controls.Add(this.tbFileSuffix);
            this.Controls.Add(this.lblFileSuffix);
            this.Controls.Add(this.tbStackLayers);
            this.Controls.Add(this.lblStackLayers);
            this.Controls.Add(this.tbIntensityByteCompressor);
            this.Controls.Add(this.lblIntensityByteCompressor);
            this.Controls.Add(this.tbMzByteCompressor);
            this.Controls.Add(this.lblMzByteCompressor);
            this.Controls.Add(this.tbMzIntCompressor);
            this.Controls.Add(this.lblMzIntCompressor);
            this.Controls.Add(this.tbMzPrecision);
            this.Controls.Add(this.lblMzPrecision);
            this.Controls.Add(this.tbMultithreading);
            this.Controls.Add(this.lblMultithreading);
            this.Controls.Add(this.tbZeroIntensity);
            this.Controls.Add(this.lblIsIgnoreZeroIntensity);
            this.Controls.Add(this.tbOutputFilePath);
            this.Controls.Add(this.lblOutputFilePath);
            this.Controls.Add(this.tbFileType);
            this.Controls.Add(this.lblFileType);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.tbInputFilePath);
            this.Controls.Add(this.lblInputFilePath);
            this.Name = "JobDetailForm";
            this.Text = "Job Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInputFilePath;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblFileType;
        private System.Windows.Forms.Label lblOutputFilePath;
        private System.Windows.Forms.Label lblIsIgnoreZeroIntensity;
        private System.Windows.Forms.Label lblMultithreading;
        private System.Windows.Forms.Label lblMzPrecision;
        private System.Windows.Forms.Label lblMzIntCompressor;
        private System.Windows.Forms.Label lblMzByteCompressor;
        private System.Windows.Forms.Label lblIntensityByteCompressor;
        private System.Windows.Forms.Label lblStackLayers;
        private System.Windows.Forms.Label lblFileSuffix;
        private System.Windows.Forms.Label lblOperator;
        public System.Windows.Forms.TextBox tbInputFilePath;
        public System.Windows.Forms.TextBox tbOperator;
        public System.Windows.Forms.TextBox tbFileName;
        public System.Windows.Forms.TextBox tbFileType;
        public System.Windows.Forms.TextBox tbOutputFilePath;
        public System.Windows.Forms.TextBox tbZeroIntensity;
        public System.Windows.Forms.TextBox tbMultithreading;
        public System.Windows.Forms.TextBox tbMzPrecision;
        public System.Windows.Forms.TextBox tbMzIntCompressor;
        public System.Windows.Forms.TextBox tbMzByteCompressor;
        public System.Windows.Forms.TextBox tbIntensityByteCompressor;
        public System.Windows.Forms.TextBox tbStackLayers;
        public System.Windows.Forms.TextBox tbStackStatus;
        private System.Windows.Forms.Label lblStackStatus;
        public System.Windows.Forms.TextBox tbFileSuffix;
    }
}