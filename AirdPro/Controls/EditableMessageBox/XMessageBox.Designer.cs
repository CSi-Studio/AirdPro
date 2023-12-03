using System.ComponentModel;

namespace AirdPro.Controls.EditableMessageBox;

partial class XMessageBox
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
        this.tbContent = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // 
        // tbContent
        // 
        this.tbContent.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tbContent.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.tbContent.Location = new System.Drawing.Point(0, 0);
        this.tbContent.Multiline = true;
        this.tbContent.Name = "tbContent";
        this.tbContent.Size = new System.Drawing.Size(351, 171);
        this.tbContent.TabIndex = 0;
        // 
        // XMessageBox
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(351, 171);
        this.Controls.Add(this.tbContent);
        this.Name = "XMessageBox";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Message";
        this.TopMost = true;
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TextBox tbContent;

    #endregion
}