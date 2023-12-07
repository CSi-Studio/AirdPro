using System.ComponentModel;

namespace AirdPro.Forms;

partial class DownloadLinksForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
        this.tabControl = new System.Windows.Forms.TabControl();
        this.btnReload = new System.Windows.Forms.Button();
        this.lblTips = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // tabControl
        // 
        this.tabControl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.tabControl.Location = new System.Drawing.Point(3, 35);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new System.Drawing.Size(795, 413);
        this.tabControl.TabIndex = 0;
        // 
        // btnReload
        // 
        this.btnReload.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnReload.Location = new System.Drawing.Point(713, 6);
        this.btnReload.Name = "btnReload";
        this.btnReload.Size = new System.Drawing.Size(75, 23);
        this.btnReload.TabIndex = 1;
        this.btnReload.Text = "Reload";
        this.btnReload.UseVisualStyleBackColor = true;
        this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
        // 
        // lblTips
        // 
        this.lblTips.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.lblTips.Location = new System.Drawing.Point(3, 6);
        this.lblTips.Name = "lblTips";
        this.lblTips.Size = new System.Drawing.Size(695, 23);
        this.lblTips.TabIndex = 2;
        this.lblTips.Text = "Tips";
        this.lblTips.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // DownloadLinksForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.lblTips);
        this.Controls.Add(this.btnReload);
        this.Controls.Add(this.tabControl);
        this.Name = "DownloadLinksForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DownloadLinksForm";
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Label lblTips;

    private System.Windows.Forms.Button btnReload;

    private System.Windows.Forms.TabControl tabControl;

    #endregion
}