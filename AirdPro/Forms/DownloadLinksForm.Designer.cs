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
        this.cbMzXML = new System.Windows.Forms.CheckBox();
        this.lblFilter = new System.Windows.Forms.Label();
        this.cbMzML = new System.Windows.Forms.CheckBox();
        this.cbMzData = new System.Windows.Forms.CheckBox();
        this.SuspendLayout();
        // 
        // tabControl
        // 
        this.tabControl.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        this.tabControl.Location = new System.Drawing.Point(3, 35);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new System.Drawing.Size(794, 413);
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
        // cbMzXML
        // 
        this.cbMzXML.Location = new System.Drawing.Point(84, 6);
        this.cbMzXML.Name = "cbMzXML";
        this.cbMzXML.Size = new System.Drawing.Size(66, 24);
        this.cbMzXML.TabIndex = 2;
        this.cbMzXML.Text = "mzXML";
        this.cbMzXML.UseVisualStyleBackColor = true;
        // 
        // lblFilter
        // 
        this.lblFilter.Font = new System.Drawing.Font("宋体", 9F);
        this.lblFilter.Location = new System.Drawing.Point(3, 6);
        this.lblFilter.Name = "lblFilter";
        this.lblFilter.Size = new System.Drawing.Size(75, 25);
        this.lblFilter.TabIndex = 3;
        this.lblFilter.Text = "Filter:";
        this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // cbMzML
        // 
        this.cbMzML.Location = new System.Drawing.Point(144, 6);
        this.cbMzML.Name = "cbMzML";
        this.cbMzML.Size = new System.Drawing.Size(66, 24);
        this.cbMzML.TabIndex = 4;
        this.cbMzML.Text = "mzML";
        this.cbMzML.UseVisualStyleBackColor = true;
        // 
        // cbMzData
        // 
        this.cbMzData.Location = new System.Drawing.Point(199, 6);
        this.cbMzData.Name = "cbMzData";
        this.cbMzData.Size = new System.Drawing.Size(66, 24);
        this.cbMzData.TabIndex = 5;
        this.cbMzData.Text = "mzData";
        this.cbMzData.UseVisualStyleBackColor = true;
        // 
        // DownloadLinksForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.cbMzData);
        this.Controls.Add(this.cbMzML);
        this.Controls.Add(this.lblFilter);
        this.Controls.Add(this.cbMzXML);
        this.Controls.Add(this.btnReload);
        this.Controls.Add(this.tabControl);
        this.Name = "DownloadLinksForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "DownloadLinksForm";
        this.Load += new System.EventHandler(this.DownloadLinksForm_Load);
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.CheckBox cbMzXML;
    private System.Windows.Forms.CheckBox cbMzData;

    private System.Windows.Forms.CheckBox cbMzML;
    private System.Windows.Forms.Label lblFilter;

    private System.Windows.Forms.Button btnReload;

    private System.Windows.Forms.TabControl tabControl;

    #endregion
}