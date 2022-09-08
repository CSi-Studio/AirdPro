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
            this.lblConversionCenter = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblConversionCenter
            // 
            this.lblConversionCenter.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblConversionCenter.Location = new System.Drawing.Point(20, 199);
            this.lblConversionCenter.Name = "lblConversionCenter";
            this.lblConversionCenter.Size = new System.Drawing.Size(155, 33);
            this.lblConversionCenter.TabIndex = 15;
            this.lblConversionCenter.Text = " Conversion Center";
            this.lblConversionCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnImport
            // 
            this.btnImport.BackgroundImage = global::AirdPro.Properties.Resources.ConversionCenter;
            this.btnImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnImport.FlatAppearance.BorderSize = 0;
            this.btnImport.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.btnImport.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnImport.Location = new System.Drawing.Point(49, 96);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 100);
            this.btnImport.TabIndex = 14;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 723);
            this.Controls.Add(this.lblConversionCenter);
            this.Controls.Add(this.btnImport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MSPro";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblConversionCenter;
    }
}