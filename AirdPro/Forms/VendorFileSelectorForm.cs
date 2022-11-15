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
using System.Windows.Forms;
using AirdPro.Constants;
using ThermoFisher.CommonCore.Data;
using AirdPro.Storage;
using System.Collections.Generic;
using System.IO;
using AirdPro.Domains;
using AirdPro.Properties;
using AirdPro.Storage.Config;
using AirdPro.Utils;
using AirdSDK.Enums;

namespace AirdPro.Forms
{
    public partial class VendorFileSelectorForm : Form, Observer<Dictionary<string, ConversionConfig>>
    {
        public VendorFileSelectorForm()
        {
            InitializeComponent();
            Program.conversionConfigHandler.attach(this);
        }

        private void CustomPathForm_Load(object sender, EventArgs e)
        {
            tbPaths.Text = string.Empty;
            rbAuto.Checked = true;
            betterFolderBrowser.Multiselect = true;
            tbOutputPath.Text = Settings.Default.LastOutputPath;
            cbConfig.SelectedIndex = 0;
        }

        public void clearInfos()
        {
            this.tbPaths.Text = string.Empty;
            for (int i = 0; i < gBoxMode.Controls.Count; i++)
            {
                var cb = gBoxMode.Controls[i] as RadioButton;
                if (cb.Checked)
                {
                    cb.Checked = false;
                }
            }
        }

        private bool addToList()
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

            var paths = tbPaths.Text;
            if (paths.IsNullOrEmpty())
            {
                MessageBox.Show(MessageInfo.Input_Your_Own_Paths_First);
                return false;
            }

            string[] pathList = paths.Split(Const.Change_Line.ToCharArray());

            foreach (string path in pathList)
            {
                if (config.autoExplorer)
                {
                    List<ConversionConfig> configList = config.buildExplorerConfigs(
                        airdType.Equals(AirdType.DDA_PASEF)
                        || airdType.Equals(AirdType.DIA_PASEF)
                        || airdType.Equals(AirdType.PRM_PASEF) || airdType.Equals(JobInfo.AutoType));
                    //如果是探索模式,则会额外增加一个以文件名称命名的文件夹的名称用于存储该文件的所有内核压缩模式
                    string fileName = FileNameUtil.parseFileName(path).Replace("-", "_");
                    string newOutputPath = Path.Combine(outputPath, fileName);
                    for (var i = 0; i < configList.Count; i++)
                    {
                        Program.conversionForm.addFile(path, newOutputPath, airdType, configList[i]);
                    }
                }
                else
                {
                    Program.conversionForm.addFile(path, outputPath, airdType, (ConversionConfig) config.Clone());
                }
            }

            return true;
        }

        private void btnAddAndContinue_Click(object sender, EventArgs e)
        {
            bool addResult = addToList();
            if (addResult)
            {
                clearInfos();
            }
        }

        private void btnAddAndClose_Click(object sender, EventArgs e)
        {
            bool addResult = addToList();
            if (addResult)
            {
                clearInfos();
                Hide();
            }
        }

        private void btnFileSelector_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    tbPaths.Text = tbPaths.Text + filePath + Const.Change_Line;
                    Settings.Default.LastOpenPath = filePath;
                }
                Settings.Default.Save();
            }
        }


        private void btnFolderSelector_Click(object sender, EventArgs e)
        {
            betterFolderBrowser.RootFolder = Settings.Default.LastOpenPath;
            if (betterFolderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var filePath in betterFolderBrowser.SelectedPaths)
                {
                    tbPaths.Text = tbPaths.Text + filePath + Const.Change_Line;
                    Settings.Default.LastOpenPath = filePath;
                }
                Settings.Default.Save();
            }
        }

        //选择已有参数，或者重新编辑参数，并将参数应用于选中的单个或一批文件
        private void btnCreateConfigs_Click(object sender, EventArgs e)
        {
            ConversionConfigListForm configListForm = new ConversionConfigListForm();
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

        private void VendorFileSelectorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.conversionConfigHandler.detach(this);
        }
    }
}