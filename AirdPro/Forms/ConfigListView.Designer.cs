namespace AirdPro.Forms
{
    partial class ConfigListView
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
            this.headerConfigName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerOperator = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerPrecision = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerCompressor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvConfigList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // headerConfigName
            // 
            this.headerConfigName.Text = "Config Name";
            this.headerConfigName.Width = 200;
            // 
            // headerOperator
            // 
            this.headerOperator.Text = "Operator";
            this.headerOperator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.headerOperator.Width = 120;
            // 
            // headerPrecision
            // 
            this.headerPrecision.Text = "Precision";
            this.headerPrecision.Width = 150;
            // 
            // headerCompressor
            // 
            this.headerCompressor.Text = "Compressor";
            this.headerCompressor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.headerCompressor.Width = 100;
            // 
            // headerOutput
            // 
            this.headerOutput.Text = "Output";
            this.headerOutput.Width = 250;
            // 
            // lvConfigList
            // 
            this.lvConfigList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvConfigList.BackColor = System.Drawing.SystemColors.Window;
            this.lvConfigList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerConfigName,
            this.headerOperator,
            this.headerPrecision,
            this.headerCompressor,
            this.headerOutput});
            this.lvConfigList.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lvConfigList.FullRowSelect = true;
            this.lvConfigList.GridLines = true;
            this.lvConfigList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvConfigList.HideSelection = false;
            this.lvConfigList.Location = new System.Drawing.Point(12, 5);
            this.lvConfigList.Name = "lvConfigList";
            this.lvConfigList.ShowGroups = false;
            this.lvConfigList.ShowItemToolTips = true;
            this.lvConfigList.Size = new System.Drawing.Size(815, 475);
            this.lvConfigList.TabIndex = 8;
            this.lvConfigList.UseCompatibleStateImageBehavior = false;
            this.lvConfigList.View = System.Windows.Forms.View.Details;
            // 
            // ConfigListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 482);
            this.Controls.Add(this.lvConfigList);
            this.Name = "ConfigListView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader headerConfigName;
        private System.Windows.Forms.ColumnHeader headerOperator;
        private System.Windows.Forms.ColumnHeader headerPrecision;
        private System.Windows.Forms.ColumnHeader headerCompressor;
        private System.Windows.Forms.ColumnHeader headerOutput;
        public System.Windows.Forms.ListView lvConfigList;
    }
}