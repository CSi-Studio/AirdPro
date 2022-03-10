using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using AirdPro.Storage;
using AirdPro.Domains.Job;
using AirdPro.Constants;
using Newtonsoft.Json;
using System.IO;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class ConversionConfigListForm : Form, Observer<Dictionary<string, ConversionConfig>>
    {
        public VendorFileSelectorForm vendor = null;

        public ConversionConfigListForm()
        {
            InitializeComponent();
            Program.conversionConfigHandler.attach(this);
            btnApplyNow.Visible = false;
        }

        public ConversionConfigListForm(VendorFileSelectorForm vendor)
        {
            InitializeComponent();
            this.vendor = vendor;
            btnApplyNow.Visible = true;
        }

        private void ConversionConfigListForm_Load(object sender, EventArgs e)
        {
            foreach (string intCompType in Enum.GetNames(typeof(IntCompType)))
            {
                configMzIntComp.Items.Add(intCompType);
            }

            foreach (string byteCompType in Enum.GetNames(typeof(ByteCompType)))
            {
                configMzByteComp.Items.Add(byteCompType);
                configIntByteComp.Items.Add(byteCompType);
            }
            showConfig("", new ConversionConfig());
        }

        public void update(Dictionary<string, ConversionConfig> configMap)
        {
            lvConfigList.Items.Clear();
            foreach (var configEntry in configMap)
            {
                ListViewItem item = new ListViewItem(new string[]
                {
                    configEntry.Key,
                    configEntry.Value.creator,
                    configEntry.Value.mzPrecision.ToString(),
                    configEntry.Value.getCompressorStr(),
                });
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

        //保存文件到本地
        private void btnSaveToLocal_Click(object sender, EventArgs e)
        {
            if (tbNameConfig.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Config Name Cannot Be Empty!");
                return;
            }
            ConversionConfig config = buildConfigInfo();
            Program.conversionConfigHandler.saveConfig(tbNameConfig.Text, config);
            MessageBox.Show("Save Config Successful!");
        }

        //设置所有参数
        private ConversionConfig buildConfigInfo()
        {
            ConversionConfig config = new ConversionConfig();
            config.mzPrecision = (int) Math.Pow(10, int.Parse(cbConfigMzPrecision.Text));
            config.ignoreZeroIntensity = cbConfigIsZeroIntensityIgnore.Checked;
            config.threadAccelerate = cbConfigThreadAccelerate.Checked;

            config.mzIntComp = (IntCompType) Enum.Parse(typeof(IntCompType), configMzIntComp.SelectedItem.ToString());
            config.mzByteComp =
                (ByteCompType) Enum.Parse(typeof(ByteCompType), configMzByteComp.SelectedItem.ToString());
            config.intByteComp =
                (ByteCompType) Enum.Parse(typeof(ByteCompType), configIntByteComp.SelectedItem.ToString());
            if (cbConfigStack.Checked)
            {
                config.stack = true;
                config.digit = (int) Math.Log(int.Parse(cbConfigStackLayers.Text), 2);
            }
            else
            {
                config.stack = false;
            }

            config.suffix = tbConfigFileNameSuffix.Text;
            config.creator = tbConfigOperator.Text;
            return config;
        }

        //不存储进内存，直接应用于当前文件
        private void btnApplyNow_Click(object sender, EventArgs e)
        {
            ConversionConfig config = buildConfigInfo();
            Program.airdForm.applyNowConfig(config);
            vendor.applyNowFileSelector(tbNameConfig.Text, config);
            this.Hide();
        }

        private void lvConfigList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvConfigList.SelectedItems.Count == 1)
            {
                string configName = lvConfigList.SelectedItems[0].Text;
                showConfig(configName, Program.conversionConfigHandler.configMap[configName]);
            }
        }

        public void showConfig(string name, ConversionConfig config)
        {
            tbNameConfig.Text = name;
            tbConfigFileNameSuffix.Text = config.suffix;
            tbConfigOperator.Text = config.creator;
            cbConfigIsZeroIntensityIgnore.Checked = config.ignoreZeroIntensity;
            cbConfigThreadAccelerate.Checked = config.threadAccelerate;
            cbConfigMzPrecision.Text = ((int) Math.Log10(config.mzPrecision)).ToString();
            configMzIntComp.Text = config.mzIntComp.ToString();
            configMzByteComp.Text = config.mzByteComp.ToString();
            configIntByteComp.Text = config.intByteComp.ToString();
            if (config.stack)
            {
                cbConfigStack.Checked = true;
                cbConfigStackLayers.Text = Convert.ToString(Math.Pow(2, config.digit));
            }
            else
            {
                cbConfigStack.Checked = false;
            }
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

        private void lblConfigOutputPath_Click(object sender, EventArgs e)
        {

        }

        private void tbConfigFolderPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConversionConfigListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.conversionConfigHandler.detach(this);
        }
    }
}