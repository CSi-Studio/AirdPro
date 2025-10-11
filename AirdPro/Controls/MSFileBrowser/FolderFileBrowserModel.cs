/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2.
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Aga.Controls.Tree;
using AirdPro.Constants;
using AirdPro.Properties;
using ThermoFisher.CommonCore.Data;

namespace AirdPro
{
    public class FolderFileBrowserModel : ITreeModel
    {
        private readonly BackgroundWorker worker;
        private readonly List<BaseItem> itemsToRead;
        private readonly Dictionary<string, List<BaseItem>> cache = [];
        private HashSet<string> criticalPathList = null;
        public FolderFileBrowserModel()
        {
            itemsToRead = [];
            worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += new DoWorkEventHandler(ReadFilesProperties);
            worker.ProgressChanged += new ProgressChangedEventHandler(ProgressChanged); 
            Init();
        }

        public void Init()
        {
            criticalPathList = new HashSet<string>(Environment.GetLogicalDrives())
            {
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
        }
        
        public RootItem BuildRoot(string path)
        {
            RootItem item = new(path, this);
            return item;
        }

        /**
         * 新增一个Root路径，如果新增成功，则返回该路径，否则返回空
         */
        public string AddRootItemToCache(RootItem item)
        {
            List<BaseItem> items;
            if (cache.ContainsKey("ROOT"))
            {
                items = cache["ROOT"];
            }
            else
            {
                items = [];
            }

            if (!items.Contains(item))
            {
                items.Add(item);
                return item.ItemPath;
            }

            return null;
        }

        /**
         * 删除一个Root路径，如果删除成功，则返回该路径，否则返回空
         */
        public string RemoveRootItemFromCache(RootItem rootItem)
        {
            if (!criticalPathList.Contains(rootItem.ItemPath))
            {
                if (cache.ContainsKey("ROOT"))
                {
                    List<BaseItem> items = cache["ROOT"];
                    items.Remove(rootItem);
                    return rootItem.ItemPath;
                }
            }

            return null;
        }

        void ReadFilesProperties(object sender, DoWorkEventArgs e)
        {
            while (itemsToRead.Count > 0)
            {
                BaseItem item = itemsToRead[0];
                itemsToRead.RemoveAt(0);
                
                if (item is FolderItem)
                {
                    DirectoryInfo info = new(item.ItemPath);
                    item.Date = info.CreationTime;
                }
                else if (item is FileItem)
                {
                    FileInfo info = new(item.ItemPath);
                    item.Size = info.Length;
                    item.Date = info.CreationTime;
                    if (info.Extension.ToLower() == ".ico")
                    {
                        Icon icon = new(item.ItemPath);
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
                Stack<object> stack = new();
                while (item != null)
                {
                    stack.Push(item);
                    item = item.Parent;
                }

                return new TreePath([.. stack]);
            }
        }

        public IEnumerable GetChildren(TreePath treePath)
        {
            List<BaseItem> items = null;
            if (treePath.IsEmpty())
            {
                if (cache.ContainsKey("ROOT"))
                    items = cache["ROOT"];
                else
                {
                    items = [];
                    cache.Add("ROOT", items);
                    
                    RootItem desktop = BuildRoot(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                    items.Add(desktop);

                    string pinPaths = Settings.Default.PinPathList;
                    string[] pinPathArray = pinPaths.Split(',');
                    for (var i = 0; i < pinPathArray.Length; i++)
                    {
                        if (pinPathArray[i].IsNullOrEmpty())
                        {
                            continue;
                        }
                        RootItem pinItem = BuildRoot(pinPathArray[i]);
                        items.Add(pinItem);
                    }
                
                    foreach (string str in Environment.GetLogicalDrives())
                    {
                        try
                        {
                            RootItem item = BuildRoot(str);
                            items.Add(item);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            else
            {
                if (treePath.LastNode is BaseItem parent && !parent.MSFile)
                {
                    if (cache.ContainsKey(parent.ItemPath))
                        items = cache[parent.ItemPath];
                    else
                    {
                        items = [];
                        try
                        {
                            foreach (string str in Directory.GetDirectories(parent.ItemPath))
                            {
                                FolderItem item = new(str, parent, this);
                                if (str.ToLower().EndsWith(FileFormat.DotD.ToLower()) ||
                                    str.ToLower().EndsWith(FileFormat.DotRAW.ToLower()))
                                {
                                    item.MSFile = true;
                                }

                                items.Add(item);
                            }

                            foreach (string str in Directory.GetFiles(parent.ItemPath))
                            {
                                FileItem item = new(str, parent, this);
                                string extension = Path.GetExtension(str);
                                if (FileFormat.DotWIFF.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotWIFF2.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotTDMS.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotRAW.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotmzML.ToLower().Equals(extension.ToLower())
                                    || FileFormat.DotimzML.ToLower().Equals(extension.ToLower())
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

        public void ClearCache(TreePath treePath = null)
        {
            if (treePath == null)
            {
                cache.Clear();
            }
            else
            {
                BaseItem item = treePath.LastNode as BaseItem;
                cache.Remove(item.ItemPath);
            }
            
            OnStructureChanged(treePath);
        }

        public event EventHandler<TreeModelEventArgs> NodesChanged;

        internal void OnNodesChanged(BaseItem item)
        {
            if (NodesChanged != null)
            {
                TreePath path = GetPath(item.Parent);
                NodesChanged(this, new TreeModelEventArgs(path, [item]));
            }
        }

        public event EventHandler<TreeModelEventArgs> NodesInserted;
        public event EventHandler<TreeModelEventArgs> NodesRemoved;
        public event EventHandler<TreePathEventArgs> StructureChanged;

        public void OnStructureChanged(TreePath treePath)
        {
            if (StructureChanged != null)
            {
                if (treePath == null)
                {
                    StructureChanged(this, new TreePathEventArgs());
                }
                else
                {
                    StructureChanged(this, new TreePathEventArgs(treePath));
                }
            
            }
                
        }
    }
}