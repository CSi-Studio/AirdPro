using System;
using System.ComponentModel;
using System.Windows.Forms;

using Aga.Controls.Tree.NodeControls;
using Aga.Controls.Tree;

namespace AirdPro
{
	public partial class FolderFileBrowser : UserControl
	{
		private class ToolTipProvider: IToolTipProvider
		{
			public string GetToolTip(TreeNodeAdv node, NodeControl nodeControl)
			{
				if (node.Tag is RootItem)
					return null;
				else
					return (node.Tag as BaseItem).Name;
			}
		}

		public FolderFileBrowser()
		{
			InitializeComponent();
            name.ToolTipProvider = new ToolTipProvider();
			name.EditorShowing += new CancelEventHandler(_name_EditorShowing);
            files.Model = new SortedTreeModel(new FolderFileBrowserModel());
        }

		void _name_EditorShowing(object sender, CancelEventArgs e)
		{
            if (files.CurrentNode.Tag is RootItem)
            {
                e.Cancel = true;
            }
		}

		private void _treeView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				NodeControlInfo info = files.GetNodeControlInfoAt(e.Location);
                if (info.Control != null && info.Control is NodeCheckBox)
                {
                    BaseItem item = info.Node.Tag as BaseItem;
                    item.IsChecked = !item.IsChecked;
					Console.WriteLine(info.Bounds);
				}
			}
		}

		private void _treeView_ColumnClicked(object sender, TreeColumnEventArgs e)
		{
            TreeColumn clicked = e.Column;
            if (clicked.SortOrder == SortOrder.Ascending)
                clicked.SortOrder = SortOrder.Descending;
            else
                clicked.SortOrder = SortOrder.Ascending;

            (files.Model as SortedTreeModel).Comparer = new FolderFileItemSorter(clicked.Header, clicked.SortOrder);
		}

        private void _treeView_NodeMouseDoubleClick(object sender, TreeNodeAdvMouseEventArgs e)
		{
			Console.WriteLine(e.Node);
		}
	}
}