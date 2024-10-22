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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Aga.Controls.Tree;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Properties;
using AirdPro.Redis;
using AirdPro.Storage;
using AirdPro.Storage.Config;
using AirdPro.Utils;
using AirdSDK.Bean;
using AirdSDK.Enums.Msi;
using AirdSDK.Utils;
using HZH_Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class VendorFileSelectorForm : Form, Observer<Dictionary<string, ConversionConfig>>
    {
        private ConversionConfigListForm configListForm;

        public VendorFileSelectorForm()
        {
            InitializeComponent();
            MsiConfig_Load();
            AddEventHandler();
        }

        private void VendorFileSelectorForm_Load(object sender, EventArgs e)
        {
            if (!Program.conversionConfigHandler.contains(this))
            {
                Program.conversionConfigHandler.attach(this);
            }

            rbAuto.Checked = true;
            tbOutputPath.Text = Settings.Default.LastOutputPath;
            string selectedConfig = Settings.Default.LastSelectedConfig;
            int selectedIndex = cbConfig.Items.IndexOf(selectedConfig);
            if (selectedIndex < 0)
            {
                selectedIndex = 0;
            }
            cbConfig.SelectedIndex = selectedIndex;
        }

        private void MsiConfig_Load()
        {
            comboBox_file_organisation.Items.Clear();
            FieldInfo[] fields = typeof(FileOrganisation).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral)
                {
                    comboBox_file_organisation.Items.Add((string)field.GetValue(null));
                }
            }
            comboBox_file_organisation.SelectedIndex = 0;  // default: row per file

            comboBox_scan_direction.Items.Clear();
            fields = typeof(ScanDirection).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral)
                {
                    comboBox_scan_direction.Items.Add((string)field.GetValue(null));
                }
            }
            comboBox_scan_direction.SelectedIndex = 2; // default: linescam left right

            comboBox_scan_sequence.Items.Clear();
            fields = typeof(ScanSequence).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral)
                {
                    comboBox_scan_sequence.Items.Add((string)field.GetValue(null));
                }
            }
            comboBox_scan_sequence.SelectedIndex = 0; // default: top down

            comboBox_scan_pattern.Items.Clear();
            fields = typeof(ScanPattern).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral && !((string)field.GetValue(null)).Equals(ScanPattern.RANDOM_ACCESS))  //厂商格式不支持随机扫描
                {
                    comboBox_scan_pattern.Items.Add((string)field.GetValue(null));  
                }
            }
            comboBox_scan_pattern.SelectedIndex = 1; // default: fly back
        }

        private void AddEventHandler()
        {
            this.rbAuto.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton1.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton2.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton3.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton4.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton5.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton6.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton7.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
            this.radioButton8.CheckedChanged += new EventHandler(this.Radio_CheckChanged);
        }

        public void ClearInfos()
        {
            msFileViews.files.ClearSelection();
        }

        private string GetAirdType()
        {
            string airdType = null;
            for (int i = 0; i < gBoxMode.Controls.Count; i++)
            {
                var cb = gBoxMode.Controls[i] as RadioButton;
                if (cb != null && cb.Checked)
                {
                    airdType = cb.Text;
                }
            }

            return airdType;
        }

        private List<string> GetInputFilesPath()
        {
            List<string> paths = new List<string>();
            var selectedNodes = msFileViews.files.SelectedNodes;
            if (selectedNodes.IsNullOrEmpty())
            {
                return paths;
            }
            
            foreach (TreeNodeAdv node in selectedNodes)
            {
                BaseItem item = node.Tag as BaseItem;
                if (item.MSFile) //如果是质谱文件则直接导入
                {
                    paths.Add(item.ItemPath);
                }
                else //如果是文件夹并且不是质谱文件,则直接扫描该文件夹下第一层的所有质谱文件
                {
                    List<string> files = AirdProFileUtil.Scan(item.ItemPath);
                    if (files != null)
                    {
                        paths.AddRange(files);
                    }
                }
            }

            return paths;
        }
        
        private bool AddToList(bool local)
        {
            string airdType = GetAirdType();
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

            List<string> filePathList = GetInputFilesPath();
            if (filePathList.IsNullOrEmpty())
            {
                MessageBox.Show(MessageInfo.Select_Files_First);
                return false;
            }

            //
            if (local)
            {
                if (airdType == AirdSDK.Enums.AcquisitionMethod.DDA_MSI || airdType == AirdSDK.Enums.AcquisitionMethod.DDA_MSI)
                {
                    string msi_path = string.Empty;
                    foreach (string path in filePathList)
                    {
                        msi_path += "|" + path;
                    }
                    msi_path = msi_path.Substring(1);
                    int[] pixels = [pixel_x.Value.ToInt(), pixel_y.Value.ToInt()];
                    MsiConfig msiConfig = new MsiConfig();
                    // msiConfig.fileOrganisation
                    switch (comboBox_file_organisation.SelectedIndex)
                    {
                        case 0:
                            msiConfig.fileOrganisation = FileOrganisation.ROW_PER_FILE;
                            break;
                        case 1:
                            msiConfig.fileOrganisation = FileOrganisation.IMAGE_PER_FILE;
                            break;
                        case 2:
                            msiConfig.fileOrganisation = FileOrganisation.SPECTRUM_PER_FILE;
                            break;
                    }
                    // msiConfig.scanDirection
                    switch (comboBox_scan_direction.SelectedIndex)
                    {
                        case 0:
                            msiConfig.scanDirection = ScanDirection.LINESCAN_TOP_DOWN;
                            break;
                        case 1:
                            msiConfig.scanDirection = ScanDirection.LINESCAN_BOTTOM_UP;
                            break;
                        case 2:
                            msiConfig.scanDirection = ScanDirection.LINESCAN_LEFT_RIGHT;
                            break;
                        case 3:
                            msiConfig.scanDirection = ScanDirection.LINESCAN_RIGHT_LEFT;
                            break;
                    }
                    // msiConfig.scanSequence
                    switch (comboBox_scan_sequence.SelectedIndex)
                    {
                        case 0:
                            msiConfig.scanSequence = ScanSequence.TOP_DOWN;
                            break;
                        case 1:
                            msiConfig.scanSequence = ScanSequence.BOTTOM_UP;
                            break;
                        case 2:
                            msiConfig.scanSequence = ScanSequence.LEFT_RIGHT;
                            break;
                        case 3:
                            msiConfig.scanSequence = ScanSequence.RIGHT_LEFT;
                            break;
                    }
                    // msiConfig.scanPattern
                    switch (comboBox_scan_pattern.SelectedIndex)
                    {
                        case 0:
                            msiConfig.scanPattern = ScanPattern.MEANDERING;
                            break;
                        case 1:
                            msiConfig.scanPattern = ScanPattern.FLY_BACK;
                            break;
                        case 2:
                            msiConfig.scanPattern = ScanPattern.RANDOM_ACCESS;
                            break;                        
                    }
                    // maxPixelX, maxPixelY, maxPixelZ
                    msiConfig.maxPixelX = pixel_x.Value.ToInt();
                    msiConfig.maxPixelY = pixel_y.Value.ToInt();
                    msiConfig.maxPixelZ = pixel_z.Value.ToInt();
                   
                    Program.conversionForm.AddFile(msi_path, outputPath, airdType, (ConversionConfig)config.Clone(), msi_path, msiConfig);
                }
                else
                {
                    foreach (string path in filePathList)
                    {
                        Program.conversionForm.AddFile(path, outputPath, airdType, (ConversionConfig)config.Clone());
                    }
                }
            }
            else
            {
                foreach (string path in filePathList)
                {
                    RemoteConvertJob remoteJob = new RemoteConvertJob(path, outputPath, airdType, config);
                    RedisManager.Instance.PublishJob(remoteJob);
                }
            }
            
            return true;
        }

        //选择已有参数，或者重新编辑参数，并将参数应用于选中的单个或一批文件
        private void BtnCreateConfigs_Click(object sender, EventArgs e)
        {
            if (this.configListForm == null || this.configListForm.IsDisposed)
            {
                configListForm = new ConversionConfigListForm();
            }

            configListForm.Show();
            configListForm.BringToFront();
        }

        public void update(Dictionary<string, ConversionConfig> configMap)
        {
            cbConfig.Items.Clear();
            foreach (var configEntry in configMap)
            {
                cbConfig.Items.Add(configEntry.Key);
            }
        }

        private void BtnConfigChooseFolder_Click(object sender, EventArgs e)
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

        private void VendorFileSelectorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void BtnFileRefresh_Click(object sender, EventArgs e)
        {
            TreeViewAdv treeViewAdv = msFileViews.files;
            SortedTreeModel model = treeViewAdv.Model as SortedTreeModel;
            FolderFileBrowserModel innerModel = model.InnerModel as FolderFileBrowserModel;
            if (treeViewAdv.SelectedNodes.IsNullOrEmpty())
            {
                innerModel.ClearCache();
            }
            else
            {
                for (var i = 0; i < treeViewAdv.SelectedNodes.Count; i++)
                {
                    innerModel.ClearCache(treeViewAdv.GetPath(treeViewAdv.SelectedNodes[i]));
                }
            }
        }

        //将某个文件目录收藏至根目录下
        private void BtnPin_Click(object sender, EventArgs e)
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
                    continue;
                }

                FolderItem folderItem = node.Tag as FolderItem;
                RootItem rootItem = msFileViews.getInnerModel().BuildRoot(folderItem.ItemPath);
                string path = innerModel.AddRootItemToCache(rootItem);
                if (path != null)
                {
                    addedPaths.Add(path);
                    AddToSettingStorage(addedPaths);
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

        private void BtnUnpin_Click(object sender, EventArgs e)
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
                RemoveFromSettingStorage(removedPaths);
                innerModel.OnStructureChanged(null);
            }
        }

        public void AddToSettingStorage(List<string> addedPaths)
        {
            string pinPathStr = Settings.Default.PinPathList;
            string[] pinPathArray = pinPathStr.Split(',');
            HashSet<string> pinPathSet = new HashSet<string>(pinPathArray);
            for (var i = 0; i < addedPaths.Count; i++)
            {
                pinPathSet.Add(addedPaths[i]);
            }

            Settings.Default.PinPathList = string.Join(",", pinPathSet);
            Settings.Default.Save();
        }

        public void RemoveFromSettingStorage(List<string> removedPaths)
        {
            string pinPathStr = Settings.Default.PinPathList;
            string[] pinPathArray = pinPathStr.Split(',');
            HashSet<string> pinPathSet = new HashSet<string>(pinPathArray);
            for (var i = 0; i < removedPaths.Count; i++)
            {
                pinPathSet.Remove(removedPaths[i]);
            }

            Settings.Default.PinPathList = string.Join(",", pinPathSet);
            Settings.Default.Save();
        }

        private void ImgBtnAdd_BtnClick(object sender, EventArgs e)
        {
            bool addResult = AddToList(true);
            if (addResult)
            {
                ClearInfos();
            }
        }

        private void ImgBtnClose_BtnClick(object sender, EventArgs e)
        {
            ClearInfos();
            Hide();
        }

        private void CbConfig_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Settings.Default.LastSelectedConfig = cbConfig.SelectedItem.ToString();
            Settings.Default.Save();
        }

        private void ImgBtnPublish_BtnClick(object sender, EventArgs e)
        {
            if (RedisManager.Instance.Check())
            {
                bool addResult = AddToList(false);
                if (addResult)
                {
                    ClearInfos();
                }
            }
            else
            {
                MessageBox.Show("Redis is not connected");
            }
        }


        private void Radio_CheckChanged(object sender, EventArgs e)
        {
            string airdType = GetAirdType();
            if (airdType == AirdSDK.Enums.AcquisitionMethod.DDA_MSI || airdType == AirdSDK.Enums.AcquisitionMethod.DIA_MSI)
            {
                comboBox_file_organisation.Visible = true;
                comboBox_scan_direction.Visible = true;
                comboBox_scan_sequence.Visible = true;
                comboBox_scan_pattern.Visible = true;
                label2.Visible = true;
                pixel_x.Visible = true;
                label3.Visible = true;
                pixel_y.Visible = true;
                //label4.Visible = true;
                //pixel_z.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
            }
            else
            {
                comboBox_file_organisation.Visible = false;
                comboBox_scan_direction.Visible = false;
                comboBox_scan_sequence.Visible = false;
                comboBox_scan_pattern.Visible = false;
                label2.Visible = false;
                pixel_x.Visible = false;
                label3.Visible = false;
                pixel_y.Visible = false;
                //label4.Visible = false;
                //pixel_z.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
            }
        }
    }
}