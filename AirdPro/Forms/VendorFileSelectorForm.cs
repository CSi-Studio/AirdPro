﻿/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Aga.Controls.Tree;
using AirdPro.Constants;
using AirdPro.Properties;
using AirdPro.Storage;
using AirdPro.Storage.Config;
using AirdPro.Utils;
using AirdSDK.Utils;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class VendorFileSelectorForm : Form, Observer<Dictionary<string, ConversionConfig>>
    {
        private ConversionConfigListForm configListForm;

        public VendorFileSelectorForm()
        {
            InitializeComponent();
        }

        private void VendorFileSelectorForm_Load(object sender, EventArgs e)
        {
            if (!Program.conversionConfigHandler.contains(this))
            {
                Program.conversionConfigHandler.attach(this);
            }
            rbAuto.Checked = true;
            tbOutputPath.Text = Settings.Default.LastOutputPath;
            cbConfig.SelectedIndex = 0;
        }

        public void clearInfos()
        {
            msFileViews.files.ClearSelection();
        }
        
        /**
         * mirrorConvert if use mirror conversion,
         * if true,AirdPro will scan the selected files into target output file with same directory structure.
         */
        private bool addToList(bool mirrorConvert)
        {
            string airdType = null;
            for (int i = 0; i < gBoxMode.Controls.Count; i++)
            {
                var cb = gBoxMode.Controls[i] as RadioButton;
                if (cb.Checked)
                {
                    airdType = cb.Text;
                }
            }

            if (airdType == null)
            {
                MessageBox.Show(MessageInfo.Choose_One_Acquisition_Mode_First);
                return false;
            }

            if (cbConfig.SelectedItem == null && !cbConfig.Text.IsNullOrEmpty())
            {
                cbConfig.SelectedItem = cbConfig.Text;
            }
            
            if (cbConfig.SelectedItem == null || cbConfig.SelectedItem.ToString().IsNullOrEmpty() ||
                !Program.conversionConfigHandler.configMap.ContainsKey(cbConfig.SelectedItem.ToString()))
            {
                MessageBox.Show(MessageInfo.Choose_One_Conversion_Config_First);
                return false;
            }

            ConversionConfig config = Program.conversionConfigHandler
                .configMap[cbConfig.SelectedItem.ToString()];

            string outputPath = tbOutputPath.Text;
            if (outputPath.IsNullOrEmpty())
            {
                MessageBox.Show(MessageInfo.Set_Your_Output_Path_First);
                return false;
            }
            
            var selectedNodes = msFileViews.files.SelectedNodes;
            if (selectedNodes.IsNullOrEmpty())
            {
                MessageBox.Show(MessageInfo.Select_Files_First);
                return false;
            }
            
            List<string> filePathList = new List<string>();
            string sourceFolder = "";
            if (mirrorConvert)
            {
                if (selectedNodes.Count != 1)
                {
                    MessageBox.Show(MessageInfo.Only_One_Source_Folder_Can_Be_Selected_In_Mirror_Conversion_Mode);
                    return false;
                }

                var selectedNode = selectedNodes[0];
                BaseItem item = selectedNode.Tag as BaseItem;
                if (item.MSFile)
                {
                    MessageBox.Show(MessageInfo.Only_Source_Folder_Can_Be_Selected_In_Mirror_Conversion_Mode);
                    return false;
                }

                sourceFolder = item.ItemPath;
                filePathList.AddRange(AirdProFileUtil.scan(item.ItemPath));
            }
            else
            {
                foreach (TreeNodeAdv node in selectedNodes)
                {
                    BaseItem item = node.Tag as BaseItem;
                    if (item.MSFile) //如果是质谱文件则直接导入
                    {
                        filePathList.Add(item.ItemPath);
                    }
                    else //如果是文件夹并且不是质谱文件,则直接扫描该文件夹下第一层的所有质谱文件
                    {
                        filePathList.AddRange(AirdProFileUtil.scan(item.ItemPath));
                    }
                }
            }
            
            foreach (string path in filePathList)
            {
                if (mirrorConvert) //如果不是镜像转换,则需要将源路径的文件夹结构也同时拷贝
                {
                    Program.conversionForm.addFile(path, Path.GetDirectoryName(path.Replace(sourceFolder, outputPath))
                        , airdType, (ConversionConfig)config.Clone());
                }
                else //如果不是镜像转换,则直接转换至指定文件夹位置即可
                {
                    Program.conversionForm.addFile(path, outputPath, airdType, (ConversionConfig)config.Clone());
                }
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            clearInfos();
            Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool addResult = addToList(false);
            if (addResult)
            {
                clearInfos();
            }
        }

        //选择已有参数，或者重新编辑参数，并将参数应用于选中的单个或一批文件
        private void btnCreateConfigs_Click(object sender, EventArgs e)
        {
            if (this.configListForm == null || this.configListForm.IsDisposed)
            {
                configListForm = new ConversionConfigListForm();
            }

            configListForm.Show();
        }

        public void update(Dictionary<string, ConversionConfig> configMap)
        {
            cbConfig.Items.Clear();
            foreach (var configEntry in configMap)
            {
                cbConfig.Items.Add(configEntry.Key);
            }
        }

        private void btnConfigChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbOutputPath.Text;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Settings.Default.LastOutputPath = fbd.SelectedPath;
                Settings.Default.Save();
                tbOutputPath.Text = fbd.SelectedPath;
            }
        }

        private void btnAddClose_Click(object sender, EventArgs e)
        {
            bool addResult = addToList(false);
            if (addResult)
            {
                clearInfos();
                Hide();
            }
        }

        private void btnMirrorScan_Click(object sender, EventArgs e)
        {
            bool addResult = addToList(true);
            if (addResult)
            {
                clearInfos();
            }
        }

        private void VendorFileSelectorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void btnFileRefresh_Click(object sender, EventArgs e)
        {
            TreeViewAdv treeViewAdv = msFileViews.files;
            SortedTreeModel model = treeViewAdv.Model as SortedTreeModel;
            FolderFileBrowserModel innerModel = model.InnerModel as FolderFileBrowserModel;
            if (treeViewAdv.SelectedNodes.IsNullOrEmpty())
            {
                innerModel.clearCache();
            }
            else
            {
                for (var i = 0; i < treeViewAdv.SelectedNodes.Count; i++)
                {
                    innerModel.clearCache(treeViewAdv.GetPath(treeViewAdv.SelectedNodes[i]));
                }
            }
        }

        private void btnFileRefresh_Click_1(object sender, EventArgs e)
        {
            TreeViewAdv treeViewAdv = msFileViews.files;
            SortedTreeModel model = treeViewAdv.Model as SortedTreeModel;
            FolderFileBrowserModel innerModel = model.InnerModel as FolderFileBrowserModel;
            if (treeViewAdv.SelectedNodes.IsNullOrEmpty())
            {
                innerModel.clearCache();
            }
            else
            {
                for (var i = 0; i < treeViewAdv.SelectedNodes.Count; i++)
                {
                    innerModel.clearCache(treeViewAdv.GetPath(treeViewAdv.SelectedNodes[i]));
                }
            }
        }

        //将某个文件目录收藏至根目录下
        private void btnPin_Click(object sender, EventArgs e)
        {
            bool alert = false;
            FolderFileBrowserModel innerModel = msFileViews.getInnerModel();
            List<string> addedPaths = new List<string>();
            for (var i = 0; i < msFileViews.files.SelectedNodes.Count; i++)
            {
                TreeNodeAdv node = msFileViews.files.SelectedNodes[i];
                if (node.Tag is RootItem)
                {
                    continue;
                }

                if (node.Tag is FileItem)
                {
                    alert = true;
                }
                
                FolderItem folderItem = node.Tag as FolderItem;
                RootItem rootItem = msFileViews.getInnerModel().buildRoot(folderItem.ItemPath);
                string path = innerModel.AddRootItemToCache(rootItem);
                if (path != null)
                {
                    addedPaths.Add(path);
                    addToSettingStorage(addedPaths);
                }
            }

            if (addedPaths.Count > 0)
            {
                innerModel.OnStructureChanged(null);
            }
          
            if (alert)
            {
                MessageBox.Show("Files cannot be pinned");
            }
        }

        private void btnUnpin_Click(object sender, EventArgs e)
        {
            FolderFileBrowserModel innerModel = msFileViews.getInnerModel();
            List<string> removedPaths = new List<string>();
            for (var i = 0; i < msFileViews.files.SelectedNodes.Count; i++)
            {
                TreeNodeAdv node = msFileViews.files.SelectedNodes[i];
                if (node.Tag is FileItem || node.Tag is FolderItem)
                {
                    continue;
                }

                RootItem rootItem = node.Tag as RootItem;
                string removedPath = innerModel.RemoveRootItemFromCache(rootItem);
                if (removedPath != null)
                {
                    removedPaths.Add(removedPath);
                }
            }

            if (removedPaths.Count > 0)
            {
                removeFromSettingStorage(removedPaths);
                innerModel.OnStructureChanged(null);
            }
        }

        public void addToSettingStorage(List<string> addedPaths)
        {
            string pinPathStr = Settings.Default.PinPathList;
            string[] pinPathArray = pinPathStr.Split(',');
            HashSet<string> pinPathSet = new HashSet<string>(pinPathArray);
            for (var i = 0; i < addedPaths.Count; i++)
            {
                pinPathSet.Add(addedPaths[i]);
            }

            Settings.Default.PinPathList = string.Join(",",pinPathSet);
            Settings.Default.Save();
        }

        public void removeFromSettingStorage(List<string> removedPaths)
        {
            string pinPathStr = Settings.Default.PinPathList;
            string[] pinPathArray = pinPathStr.Split(',');
            HashSet<string> pinPathSet = new HashSet<string>(pinPathArray);
            for (var i = 0; i < removedPaths.Count; i++)
            {
                pinPathSet.Remove(removedPaths[i]);
            }
            Settings.Default.PinPathList = string.Join(",",pinPathSet);
            Settings.Default.Save();
        }
    }
}