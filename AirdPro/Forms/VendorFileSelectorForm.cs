/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
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
using AirdPro.Domains.Convert;
using System.Collections.Generic;

namespace AirdPro.Forms
{
    public partial class VendorFileSelectorForm : Form, Observer<Dictionary<string, ConversionConfig>>
    {
        public VendorFileSelectorForm()
        {
            InitializeComponent();
            Program.conversionConfigHandler.attach(this);
            betterFolderBrowser.Multiselect = true;
            tbConfigFolderPath.Text = Program.globalConfigHandler.config.defaultOpenPath;
            cbConfig.SelectedIndex = 0;
        }

        private void CustomPathForm_Load(object sender, EventArgs e)
        {
            tbPaths.Text = string.Empty;
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            string expType = null;
            for (int i = 0; i < gBoxMode.Controls.Count; i++)
            {
                var cb = gBoxMode.Controls[i] as RadioButton;
                if (cb.Checked)
                {
                    expType = cb.Text;
                }
            }

            if (expType == null)
            {
                MessageBox.Show("Choose one acquisition mode first!");
                return;
            }

            if (cbConfig.SelectedText.IsNullOrEmpty())
            {
                MessageBox.Show("Choose one conversion config first!");
                return;
            }

            var paths = tbPaths.Text;

            if (paths.IsNullOrEmpty())
            {
                MessageBox.Show("Input your own paths first!");
                return;
            }
            var pathList = paths.Split(Const.Change_Line.ToCharArray());

            foreach (var path in pathList)
            {
                Program.airdForm.addFile(path, expType, Program.conversionConfigHandler.configMap[cbConfig.SelectedText]);
            }
            
            this.Hide();
        }

        private void btnFileSelector_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    tbPaths.Text = tbPaths.Text + filePath + Const.Change_Line;
                }
            }
        }


        private void btnFolderSelector_Click(object sender, EventArgs e)
        {
            betterFolderBrowser.RootFolder = Program.globalConfigHandler.config.defaultOpenPath;
            if (betterFolderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var filePath in betterFolderBrowser.SelectedPaths)
                {
                    tbPaths.Text = tbPaths.Text + filePath + Const.Change_Line;
                }
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

        public void applyNowFileSelector(string configName, ConversionConfig config)
        {
            tbPaths.Text = tbPaths.Text + configName + Const.Change_Line;
            tbPaths.Text = tbPaths.Text + config.creator + Const.Change_Line;
            tbPaths.Text = tbPaths.Text + config.mzPrecision + Const.Change_Line;
        }

        private void btnConfigChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = tbConfigFolderPath.Text;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                tbConfigFolderPath.Text = fbd.SelectedPath;
            }
        }

        private void VendorFileSelectorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.conversionConfigHandler.detach(this);
        }
    }
}
