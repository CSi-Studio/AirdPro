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
using System.Drawing;
using System.IO;
using AirdPro.Utils;

namespace AirdPro
{
    public abstract class BaseItem
    {
        private bool msfile = false;

        public bool MSFile
        {
            get { return msfile; }
            set
            {
                msfile = value;
                if (msfile)
                {
                    this.Icon = ResourceUtil.readImage("Menu.Spectrum16x16.png");
                }
            }
        }

        private string path = "";

        public string ItemPath
        {
            get { return path; }
            set { path = value; }
        }

        private Image icon;

        public Image Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        private long size = 0;

        public long Size
        {
            get { return size; }
            set
            {
                size = value;
                SizeLabel = FileUtil.getSizeLabel(size);
            }
        }

        private string sizeLabel = "";

        public string SizeLabel
        {
            get { return sizeLabel; }
            set { sizeLabel = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public abstract string Name { get; set; }

        private BaseItem parent;

        public BaseItem Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (Owner != null)
                    Owner.OnNodesChanged(this);
            }
        }

        private FolderFileBrowserModel owner;

        public FolderFileBrowserModel Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /*public override bool Equals(object obj)
        {
            if (obj is BaseItem)
                return path.Equals((obj as BaseItem).ItemPath);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return path.GetHashCode();
        }*/

        public override string ToString()
        {
            return path;
        }
    }

    public class RootItem : BaseItem
    {
        public RootItem(string name, FolderFileBrowserModel owner)
        {
            ItemPath = name;
            Owner = owner;
        }

        public override string Name
        {
            get { return ItemPath; }
            set { }
        }
    }

    public class FolderItem : BaseItem
    {
        public override string Name
        {
            get { return Path.GetFileName(ItemPath); }
            set
            {
                string dir = Path.GetDirectoryName(ItemPath);
                string destination = Path.Combine(dir, value);
                Directory.Move(ItemPath, destination);
                ItemPath = destination;
            }
        }

        public FolderItem(string name, BaseItem parent, FolderFileBrowserModel owner)
        {
            ItemPath = name;
            Parent = parent;
            Owner = owner;
        }
    }

    public class FileItem : BaseItem
    {
        public override string Name
        {
            get { return Path.GetFileName(ItemPath); }
            set
            {
                string dir = Path.GetDirectoryName(ItemPath);
                string destination = Path.Combine(dir, value);
                File.Move(ItemPath, destination);
                ItemPath = destination;
            }
        }

        public FileItem(string name, BaseItem parent, FolderFileBrowserModel owner)
        {
            ItemPath = name;
            Parent = parent;
            Owner = owner;
        }
    }
}