namespace AirdPro
{
    partial class FolderFileBrowser
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.files = new Aga.Controls.Tree.TreeViewAdv();
            this.FileName = new Aga.Controls.Tree.TreeColumn();
            this.SizeLabel = new Aga.Controls.Tree.TreeColumn();
            this.FileDate = new Aga.Controls.Tree.TreeColumn();
            this.icon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.size = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.date = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this.SuspendLayout();
            // 
            // files
            // 
            this.files.AllowColumnReorder = true;
            this.files.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.files.AutoRowHeight = true;
            this.files.BackColor = System.Drawing.SystemColors.Window;
            this.files.Columns.Add(this.FileName);
            this.files.Columns.Add(this.SizeLabel);
            this.files.Columns.Add(this.FileDate);
            this.files.Cursor = System.Windows.Forms.Cursors.Default;
            this.files.DefaultToolTipProvider = null;
            this.files.DragDropMarkColor = System.Drawing.Color.Black;
            this.files.Font = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 8F);
            this.files.FullRowSelect = true;
            this.files.GridLineStyle = ((Aga.Controls.Tree.GridLineStyle)((Aga.Controls.Tree.GridLineStyle.Horizontal | Aga.Controls.Tree.GridLineStyle.Vertical)));
            this.files.LineColor = System.Drawing.SystemColors.ControlDark;
            this.files.LoadOnDemand = true;
            this.files.Location = new System.Drawing.Point(0, -1);
            this.files.Model = null;
            this.files.Name = "files";
            this.files.NodeControls.Add(this.icon);
            this.files.NodeControls.Add(this.name);
            this.files.NodeControls.Add(this.size);
            this.files.NodeControls.Add(this.date);
            this.files.SelectedNode = null;
            this.files.SelectionMode = Aga.Controls.Tree.TreeSelectionMode.Multi;
            this.files.ShowNodeToolTips = true;
            this.files.Size = new System.Drawing.Size(623, 289);
            this.files.TabIndex = 0;
            this.files.UseColumns = true;
            this.files.NodeMouseDoubleClick += new System.EventHandler<Aga.Controls.Tree.TreeNodeAdvMouseEventArgs>(this._treeView_NodeMouseDoubleClick);
            this.files.ColumnClicked += new System.EventHandler<Aga.Controls.Tree.TreeColumnEventArgs>(this._treeView_ColumnClicked);
            this.files.MouseClick += new System.Windows.Forms.MouseEventHandler(this._treeView_MouseClick);
            // 
            // FileName
            // 
            this.FileName.Header = "Name";
            this.FileName.SortOrder = System.Windows.Forms.SortOrder.None;
            this.FileName.TooltipText = "File name";
            this.FileName.Width = 350;
            // 
            // SizeLabel
            // 
            this.SizeLabel.Header = "Size";
            this.SizeLabel.SortOrder = System.Windows.Forms.SortOrder.None;
            this.SizeLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SizeLabel.TooltipText = "File size";
            this.SizeLabel.Width = 80;
            // 
            // FileDate
            // 
            this.FileDate.Header = "Date";
            this.FileDate.SortOrder = System.Windows.Forms.SortOrder.None;
            this.FileDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileDate.TooltipText = "File date";
            this.FileDate.Width = 150;
            // 
            // icon
            // 
            this.icon.DataPropertyName = "Icon";
            this.icon.LeftMargin = 1;
            this.icon.ParentColumn = this.FileName;
            this.icon.ScaleMode = Aga.Controls.Tree.ImageScaleMode.Clip;
            // 
            // name
            // 
            this.name.DataPropertyName = "Name";
            this.name.IncrementalSearchEnabled = true;
            this.name.LeftMargin = 3;
            this.name.ParentColumn = this.FileName;
            this.name.Trimming = System.Drawing.StringTrimming.EllipsisCharacter;
            this.name.UseCompatibleTextRendering = true;
            // 
            // size
            // 
            this.size.DataPropertyName = "SizeLabel";
            this.size.IncrementalSearchEnabled = true;
            this.size.LeftMargin = 3;
            this.size.ParentColumn = this.SizeLabel;
            this.size.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // date
            // 
            this.date.DataPropertyName = "Date";
            this.date.IncrementalSearchEnabled = true;
            this.date.LeftMargin = 3;
            this.date.ParentColumn = this.FileDate;
            this.date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FolderFileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.files);
            this.Name = "FolderFileBrowser";
            this.Size = new System.Drawing.Size(623, 291);
            this.ResumeLayout(false);

        }

        #endregion
        private Aga.Controls.Tree.NodeControls.NodeStateIcon icon;
        private Aga.Controls.Tree.NodeControls.NodeTextBox name;
        private Aga.Controls.Tree.NodeControls.NodeTextBox size;
        private Aga.Controls.Tree.NodeControls.NodeTextBox date;
        private Aga.Controls.Tree.TreeColumn FileName;
        private Aga.Controls.Tree.TreeColumn SizeLabel;
        private Aga.Controls.Tree.TreeColumn FileDate;
        public Aga.Controls.Tree.TreeViewAdv files;
    }
}
