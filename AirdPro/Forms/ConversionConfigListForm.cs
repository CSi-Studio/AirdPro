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
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Properties;
using AirdPro.Storage;
using AirdPro.Storage.Config;
using AirdSDK.Enums;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class ConversionConfigListForm : Form, Observer<Dictionary<string, ConversionConfig>>
    {
        private ListViewItem item;

        public ConversionConfigListForm()
        {
            InitializeComponent();
            Program.conversionConfigHandler.attach(this);
            btnApply.Visible = false;
            btnSaveToLocal.Visible = true;
            tableAutoDecision.Enabled = !cbAutoDecision.Checked;
        }

        public ConversionConfigListForm(ListViewItem item)
        {
            InitializeComponent();
            this.item = item;
            btnApply.Visible = true;
            btnSaveToLocal.Visible = false;
            tableAutoDecision.Enabled = !cbAutoDecision.Checked;
        }

        private void ConversionConfigListForm_Load(object sender, EventArgs e)
        {
            foreach (string sortedIntCompType in Enum.GetNames(typeof(SortedIntCompType)))
            {
                cbMzIntComp.Items.Add(sortedIntCompType);
                cbRtIntComp.Items.Add(sortedIntCompType);
            }
            
            foreach (string intCompType in Enum.GetNames(typeof(IntCompType)))
            {
                cbIntIntComp.Items.Add(intCompType);
                cbMobiIntComp.Items.Add(intCompType);
            }

            foreach (string byteCompType in Enum.GetNames(typeof(ByteCompType)))
            {
                cbMzByteComp.Items.Add(byteCompType);
                cbIntByteComp.Items.Add(byteCompType);
                cbMobiByteComp.Items.Add(byteCompType);
                cbRtByteComp.Items.Add(byteCompType);
            }

            ShowConfig("", new ConversionConfig());
            numMaxTasks.Text = Settings.Default.MaxConversionTasks.ToString();
        }

        public void update(Dictionary<string, ConversionConfig> configMap)
        {
            lvConfigList.Items.Clear();
            foreach (var configEntry in configMap)
            {
                ListViewItem item = new ListViewItem(new string[]
                {
                    configEntry.Key, configEntry.Value.GetMzPrecisionStr(), configEntry.Value.autoDesicion + ""
                });
                if (configEntry.Value.engine.Equals(AirdEngine.RowCompression))
                {
                    item.ImageIndex = 0;
                }
                else if (configEntry.Value.engine.Equals(AirdEngine.ColumnCompression))
                {
                    item.ImageIndex = 1;
                }

                lvConfigList.Items.Add(item);
            }
        }

        private void cbConfigIsZeroIntensityIgnore_CheckedChanged(object sender, EventArgs e)
        {
            string suffix = "";

            if (!cbConfigIsZeroIntensityIgnore.Checked)
            {
                suffix += "_with_zero";
            }

            tbConfigFileNameSuffix.Text = suffix;
        }

        private void cbConfigIsCentroid_CheckedChanged(object sender, EventArgs e)
        {
        }

        //设置所有参数
        private ConversionConfig BuildConfigInfo()
        {
            ConversionConfig config = new ConversionConfig();
            config.mzPrecision = (int)Math.Pow(10, int.Parse(cbConfigMzPrecision.Text));
            config.ignoreZeroIntensity = cbConfigIsZeroIntensityIgnore.Checked;
            config.engine = cbCompEngine.SelectedIndex;
            config.configName = tbNameConfig.Text;
            config.indexFormat = cbIndexFormat.SelectedIndex;
            //如果不是自动决策的,则会使用配置的组合压缩器
            if (!cbAutoDecision.Checked)
            {
                config.mzIntComp =
                    (SortedIntCompType)Enum.Parse(typeof(SortedIntCompType), cbMzIntComp.SelectedItem.ToString());
                config.mzByteComp =
                    (ByteCompType)Enum.Parse(typeof(ByteCompType), cbMzByteComp.SelectedItem.ToString());
                config.intIntComp = (IntCompType)Enum.Parse(typeof(IntCompType), cbIntIntComp.SelectedItem.ToString());
                config.intByteComp =
                    (ByteCompType)Enum.Parse(typeof(ByteCompType), cbIntByteComp.SelectedItem.ToString());
                config.mobiIntComp =
                    (IntCompType)Enum.Parse(typeof(IntCompType), cbMobiIntComp.SelectedItem.ToString());
                config.mobiByteComp =
                    (ByteCompType)Enum.Parse(typeof(ByteCompType), cbMobiByteComp.SelectedItem.ToString());
                config.rtIntComp =  (SortedIntCompType)Enum.Parse(typeof(SortedIntCompType), cbRtIntComp.SelectedItem.ToString());
                config.rtByteComp =
                    (ByteCompType)Enum.Parse(typeof(ByteCompType), cbRtByteComp.SelectedItem.ToString());
            }
            
            config.compressedIndex = cbCompressedIndex.Checked;
            config.suffix = tbConfigFileNameSuffix.Text;
            config.creator = tbConfigOperator.Text;
            config.autoDesicion = cbAutoDecision.Checked;
            try
            {
                config.spectraToPredict = int.Parse(tbSpectraToPredict.Text);
            }
            catch (Exception e)
            {
                config.spectraToPredict = 50;
                tbSpectraToPredict.Text = "50";
            }

            config.compressionSizeWeight = int.Parse(cbCSWeight.Text);
            config.compressionTimeWeight = int.Parse(cbCTWeight.Text);
            config.decompressionTimeWeight = int.Parse(cbDTWeight.Text);
           
            //Filter字段
            config.noMS1 = cbNoMS1.Checked;
            config.noMS2 = cbNoMS2.Checked;
            return config;
        }

        //保存文件到本地
        private void btnSaveToLocal_Click(object sender, EventArgs e)
        {
            if (tbNameConfig.Text.IsNullOrEmpty())
            {
                MessageBox.Show(MessageInfo.Config_Name_Cannot_Be_Empty);
                return;
            }

            ConversionConfig config = BuildConfigInfo();
            Program.conversionConfigHandler.saveConfig(tbNameConfig.Text, config);
            Settings.Default.MaxConversionTasks = (int)numMaxTasks.Value;
        }


        //不存储进内存，直接应用于当前文件
        private void btnApply_Click(object sender, EventArgs e)
        {
            ConversionConfig config = BuildConfigInfo();
            JobInfo jobInfo = (JobInfo)(item.Tag);
            jobInfo.config = config;
            if (jobInfo.status.Equals(ProcessingStatus.RUNNING))
            {
                MessageBox.Show(MessageInfo.Running_Job_Cannot_Be_Modified);
            }
            else
            {
                jobInfo.RefreshItem(item);
            }

            Hide();
        }

        private void lvConfigList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvConfigList.SelectedItems.Count == 1)
            {
                string configName = lvConfigList.SelectedItems[0].Text;
                ShowConfig(configName, Program.conversionConfigHandler.configMap[configName]);
            }
        }

        public void ShowConfig(string name, ConversionConfig config)
        {
            tbNameConfig.Text = name;
            cbCompEngine.SelectedIndex = config.engine;
            tbConfigFileNameSuffix.Text = config.suffix;
            tbConfigOperator.Text = config.creator;
            cbConfigIsZeroIntensityIgnore.Checked = config.ignoreZeroIntensity;
            cbConfigMzPrecision.SelectedItem = ((int)Math.Log10(config.mzPrecision)).ToString();
            cbMzIntComp.SelectedItem = config.mzIntComp.ToString();
            cbMzByteComp.SelectedItem = config.mzByteComp.ToString();
            cbIntIntComp.SelectedItem = config.intIntComp.ToString();
            cbIntByteComp.SelectedItem = config.intByteComp.ToString();
            cbMobiIntComp.SelectedItem = config.mobiIntComp.ToString();
            cbMobiByteComp.SelectedItem = config.mobiByteComp.ToString();
            cbRtIntComp.SelectedItem = config.rtIntComp.ToString();
            cbRtByteComp.SelectedItem = config.rtByteComp.ToString();
            
            tableAutoDecision.Enabled = !config.autoDesicion;
            cbAutoDecision.Checked = config.autoDesicion;
            cbCompressedIndex.Checked = config.compressedIndex;
            tbSpectraToPredict.Text = config.spectraToPredict + "";
            cbCSWeight.Text = config.compressionSizeWeight + "";
            cbCTWeight.Text = config.compressionTimeWeight + "";
            cbDTWeight.Text = config.decompressionTimeWeight + "";
            cbIndexFormat.SelectedIndex = config.indexFormat;
            cbNoMS1.Checked = config.noMS1;
            cbNoMS2.Checked = config.noMS2;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvConfigList.SelectedItems.Count != 0)
            {
                ListView.SelectedListViewItemCollection items = this.lvConfigList.SelectedItems; //获取所有选中的Items集合
                List<string> configNames = new List<string>();
                foreach (ListViewItem item in items)
                {
                    string configName = item.SubItems[0].Text;
                    configNames.Add(configName);
                }

                Program.conversionConfigHandler.removeConfig(configNames);
            }
        }

        private void ConversionConfigListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.conversionConfigHandler.detach(this);
        }

        private void cbAutoDecision_CheckedChanged(object sender, EventArgs e)
        {
            tableAutoDecision.Enabled = !cbAutoDecision.Checked;
            tableDeciderWeight.Enabled = cbAutoDecision.Checked;
            tbSpectraToPredict.Enabled = cbAutoDecision.Checked;
        }

        private void btnGlobalSettingSave_Click(object sender, EventArgs e)
        {
            Settings.Default.MaxConversionTasks = (int)numMaxTasks.Value;
        }
    }
}