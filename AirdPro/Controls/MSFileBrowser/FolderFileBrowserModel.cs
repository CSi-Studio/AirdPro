using System;
using System.Collections.Generic;


using Aga.Controls.Tree;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Threading;
using AirdPro.Constants;

namespace AirdPro
{
	public class FolderFileBrowserModel: ITreeModel
    {
        private BackgroundWorker worker;
		private List<BaseItem> itemsToRead;
		private Dictionary<string, List<BaseItem>> cache = new Dictionary<string, List<BaseItem>>();

		public FolderFileBrowserModel()
		{
			itemsToRead = new List<BaseItem>(); 
			worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.DoWork += new DoWorkEventHandler(ReadFilesProperties);
			worker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged);
		}

		void ReadFilesProperties(object sender, DoWorkEventArgs e)
		{
			while(itemsToRead.Count > 0)
			{
				BaseItem item = itemsToRead[0];
				itemsToRead.RemoveAt(0);

				// Thread.Sleep(50); //emulate time consuming operation
				if (item is FolderItem)
				{
					DirectoryInfo info = new DirectoryInfo(item.ItemPath);
					item.Date = info.CreationTime;
				}
				else if (item is FileItem)
				{
					FileInfo info = new FileInfo(item.ItemPath);
					item.Size = info.Length;
					item.Date = info.CreationTime;
					if (info.Extension.ToLower() == ".ico")
					{
						Icon icon = new Icon(item.ItemPath);
						item.Icon = icon.ToBitmap();
					}
					else if (info.Extension.ToLower() == ".bmp")
					{
						item.Icon = new Bitmap(item.ItemPath);
					}
				}
				worker.ReportProgress(0, item);
			}
		}

		void ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			OnNodesChanged(e.UserState as BaseItem);
		}

		private TreePath GetPath(BaseItem item)
		{
			if (item == null)
				return TreePath.Empty;
			else
			{
				Stack<object> stack = new Stack<object>();
				while (item != null)
				{
					stack.Push(item);
					item = item.Parent;
				}
				return new TreePath(stack.ToArray());
			}
		}

		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
			List<BaseItem> items = null;
			if (treePath.IsEmpty())
			{
				if (cache.ContainsKey("ROOT"))
					items = cache["ROOT"];
				else
				{
					items = new List<BaseItem>();
					cache.Add("ROOT", items);
                    foreach (string str in Environment.GetLogicalDrives())
                    {
						DirectoryInfo dir = new DirectoryInfo(str);
                        RootItem root = new RootItem(str, this);
                        root.Date = dir.CreationTime;
                        items.Add(root);
                    }
                }
			}
			else
			{
				BaseItem parent = treePath.LastNode as BaseItem;
				if (parent != null && !parent.MSFile)
				{
					if (cache.ContainsKey(parent.ItemPath))
						items = cache[parent.ItemPath];
					else
					{
						items = new List<BaseItem>();
						try
						{
                            foreach (string str in Directory.GetDirectories(parent.ItemPath))
                            {
                                FolderItem item = new FolderItem(str, parent, this);
                                if (str.ToLower().EndsWith(FileFormat.DotD.ToLower()) || str.ToLower().EndsWith(FileFormat.DotRAW.ToLower()))
                                {
                                    item.MSFile = true;
                                }

                                items.Add(item);
                            }
								
							foreach (string str in Directory.GetFiles(parent.ItemPath))
							{
								FileItem item = new FileItem(str, parent, this);
								string extension = Path.GetExtension(str);
                                if (FileFormat.DotWIFF.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotRAW.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotmzML.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotmzXML.ToLower().Equals(extension.ToLower()))
                                {
                                    item.MSFile = true;
                                    items.Add(item);
                                }
                                
							}
						}
						catch (Exception)
						{
							return null;
						}
						cache.Add(parent.ItemPath, items);
						itemsToRead.AddRange(items);
						if (!worker.IsBusy)
							worker.RunWorkerAsync();
					}
				}
			}
			return items;
		}

		public bool IsLeaf(TreePath treePath)
		{
			return treePath.LastNode is FileItem;
		}

		public event EventHandler<TreeModelEventArgs> NodesChanged;
		internal void OnNodesChanged(BaseItem item)
		{
			if (NodesChanged != null)
			{
				TreePath path = GetPath(item.Parent);
				NodesChanged(this, new TreeModelEventArgs(path, new object[] { item }));
			}
		}

		public event EventHandler<TreeModelEventArgs> NodesInserted;
		public event EventHandler<TreeModelEventArgs> NodesRemoved;
		public event EventHandler<TreePathEventArgs> StructureChanged;
		public void OnStructureChanged()
		{
			if (StructureChanged != null)
				StructureChanged(this, new TreePathEventArgs());
		}
	}
}
