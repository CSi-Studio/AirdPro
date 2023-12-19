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
        this.tbHome = new System.Windows.Forms.TextBox();
        this.btnListFtpFiles = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.tbFTP = new System.Windows.Forms.TextBox();
        this.tbFrom = new System.Windows.Forms.TextBox();
        this.tbIdentifier = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // 
        // tabControl
        // 
        this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.tabControl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.tabControl.Location = new System.Drawing.Point(3, 131);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new System.Drawing.Size(721, 445);
        this.tabControl.TabIndex = 0;
        // 
        // btnReload
        // 
        this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnReload.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnReload.Location = new System.Drawing.Point(623, 44);
        this.btnReload.Name = "btnReload";
        this.btnReload.Size = new System.Drawing.Size(93, 51);
        this.btnReload.TabIndex = 1;
        this.btnReload.Text = "Load";
        this.btnReload.UseVisualStyleBackColor = true;
        this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
        // 
        // lblTips
        // 
        this.lblTips.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.lblTips.BackColor = System.Drawing.SystemColors.ControlDark;
        this.lblTips.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.lblTips.Location = new System.Drawing.Point(9, 45);
        this.lblTips.Name = "lblTips";
        this.lblTips.Size = new System.Drawing.Size(610, 23);
        this.lblTips.TabIndex = 2;
        this.lblTips.Text = "Tips";
        this.lblTips.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // tbHome
        // 
        this.tbHome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.tbHome.Font = new System.Drawing.Font("微软雅黑", 9F);
        this.tbHome.Location = new System.Drawing.Point(55, 74);
        this.tbHome.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.tbHome.Name = "tbHome";
        this.tbHome.ReadOnly = true;
        this.tbHome.Size = new System.Drawing.Size(564, 23);
        this.tbHome.TabIndex = 3;
        // 
        // btnListFtpFiles
        // 
        this.btnListFtpFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnListFtpFiles.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.btnListFtpFiles.Location = new System.Drawing.Point(624, 101);
        this.btnListFtpFiles.Name = "btnListFtpFiles";
        this.btnListFtpFiles.Size = new System.Drawing.Size(93, 24);
        this.btnListFtpFiles.TabIndex = 4;
        this.btnListFtpFiles.Text = "FTP Files";
        this.btnListFtpFiles.UseVisualStyleBackColor = true;
        this.btnListFtpFiles.Click += new System.EventHandler(this.btnListFtpFiles_Click);
        // 
        // label1
        // 
        this.label1.BackColor = System.Drawing.SystemColors.ButtonFace;
        this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.label1.Location = new System.Drawing.Point(7, 75);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(43, 21);
        this.label1.TabIndex = 5;
        this.label1.Text = "Home";
        this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // label2
        // 
        this.label2.BackColor = System.Drawing.SystemColors.ButtonFace;
        this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.label2.Location = new System.Drawing.Point(17, 103);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(33, 21);
        this.label2.TabIndex = 6;
        this.label2.Text = "FTP";
        this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // tbFTP
        // 
        this.tbFTP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.tbFTP.Font = new System.Drawing.Font("微软雅黑", 9F);
        this.tbFTP.Location = new System.Drawing.Point(55, 103);
        this.tbFTP.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.tbFTP.Name = "tbFTP";
        this.tbFTP.Size = new System.Drawing.Size(565, 23);
        this.tbFTP.TabIndex = 7;
        // 
        // tbFrom
        // 
        this.tbFrom.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.tbFrom.Location = new System.Drawing.Point(9, 8);
        this.tbFrom.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.tbFrom.Name = "tbFrom";
        this.tbFrom.ReadOnly = true;
        this.tbFrom.Size = new System.Drawing.Size(286, 34);
        this.tbFrom.TabIndex = 8;
        // 
        // tbIdentifier
        // 
        this.tbIdentifier.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.tbIdentifier.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.tbIdentifier.Location = new System.Drawing.Point(414, 8);
        this.tbIdentifier.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.tbIdentifier.Name = "tbIdentifier";
        this.tbIdentifier.ReadOnly = true;
        this.tbIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.tbIdentifier.Size = new System.Drawing.Size(304, 34);
        this.tbIdentifier.TabIndex = 9;
        // 
        // DownloadLinksForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(725, 578);
        this.Controls.Add(this.tbIdentifier);
        this.Controls.Add(this.tbFrom);
        this.Controls.Add(this.tbFTP);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.btnListFtpFiles);
        this.Controls.Add(this.tbHome);
        this.Controls.Add(this.lblTips);
        this.Controls.Add(this.btnReload);
        this.Controls.Add(this.tabControl);
        this.Name = "DownloadLinksForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DownloadLinksForm";
        this.Load += new System.EventHandler(this.DownloadLinksForm_Load);
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TextBox tbIdentifier;

    private System.Windows.Forms.TextBox tbFrom;

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox tbFTP;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.TextBox tbHome;
    private System.Windows.Forms.Button btnListFtpFiles;

    private System.Windows.Forms.Label lblTips;

    private System.Windows.Forms.Button btnReload;

    private System.Windows.Forms.TabControl tabControl;

    #endregion
}