namespace AirdPro.Repository
{
    partial class ReposWizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReposWizardForm));
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
            this.lblPX.Location = new System.Drawing.Point(106, 180);
            this.lblPX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPX.Name = "lblPX";
            this.lblPX.Size = new System.Drawing.Size(289, 39);
            this.lblPX.TabIndex = 1;
            this.lblPX.Text = "Proteome Xchange";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(742, 180);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "MetaboLights";
            // 
            // btnML
            // 
            this.btnML.Image = ((System.Drawing.Image)(resources.GetObject("btnML.Image")));
            this.btnML.Location = new System.Drawing.Point(561, 18);
            this.btnML.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnML.Name = "btnML";
            this.btnML.Size = new System.Drawing.Size(621, 158);
            this.btnML.TabIndex = 4;
            this.btnML.UseVisualStyleBackColor = true;
            this.btnML.Click += new System.EventHandler(this.btnML_Click);
            // 
            // btnPX
            // 
            this.btnPX.Image = global::AirdPro.Properties.Resources.Proteomexchange;
            this.btnPX.Location = new System.Drawing.Point(18, 18);
            this.btnPX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPX.Name = "btnPX";
            this.btnPX.Size = new System.Drawing.Size(483, 158);
            this.btnPX.TabIndex = 0;
            this.btnPX.UseVisualStyleBackColor = true;
            this.btnPX.Click += new System.EventHandler(this.btnPX_Click);
            // 
            // ReposWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 264);
            this.Controls.Add(this.btnML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPX);
            this.Controls.Add(this.btnPX);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ReposWizardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
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