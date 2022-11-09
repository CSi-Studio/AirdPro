namespace AirdPro.Repository
{
    partial class RepositoryGuiderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepositoryGuiderForm));
            this.lblPX = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnML = new System.Windows.Forms.Button();
            this.btnPX = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPX
            // 
            this.lblPX.AutoSize = true;
            this.lblPX.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPX.Location = new System.Drawing.Point(71, 120);
            this.lblPX.Name = "lblPX";
            this.lblPX.Size = new System.Drawing.Size(194, 27);
            this.lblPX.TabIndex = 1;
            this.lblPX.Text = "Proteome Xchange";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(495, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 27);
            this.label1.TabIndex = 3;
            this.label1.Text = "MetaboLights";
            // 
            // btnML
            // 
            this.btnML.Image = ((System.Drawing.Image)(resources.GetObject("btnML.Image")));
            this.btnML.Location = new System.Drawing.Point(374, 12);
            this.btnML.Name = "btnML";
            this.btnML.Size = new System.Drawing.Size(414, 105);
            this.btnML.TabIndex = 4;
            this.btnML.UseVisualStyleBackColor = true;
            this.btnML.Click += new System.EventHandler(this.btnML_Click);
            // 
            // btnPX
            // 
            this.btnPX.Image = global::AirdPro.Properties.Resources.Proteomexchange;
            this.btnPX.Location = new System.Drawing.Point(12, 12);
            this.btnPX.Name = "btnPX";
            this.btnPX.Size = new System.Drawing.Size(322, 105);
            this.btnPX.TabIndex = 0;
            this.btnPX.UseVisualStyleBackColor = true;
            this.btnPX.Click += new System.EventHandler(this.btnPX_Click);
            // 
            // RepositoryGuiderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 176);
            this.Controls.Add(this.btnML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPX);
            this.Controls.Add(this.btnPX);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RepositoryGuiderForm";
            this.Text = "Repositories";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPX;
        private System.Windows.Forms.Label lblPX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnML;
    }
}