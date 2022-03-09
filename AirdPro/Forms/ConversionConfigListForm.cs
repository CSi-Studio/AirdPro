using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using AirdPro.Storage;
using AirdPro.Domains.Job;
using AirdPro.Constants;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace AirdPro.Forms
{
    public partial class ConversionConfigListForm : Form, Observer<Dictionary<string, ConversionConfig>>
    {
        private ConversionConfig config = new ConversionConfig();
        public AirdForm airdForm;
        private ConversionConfigHandler handler;
        public ConversionConfigListForm(ConversionConfigHandler handler,AirdForm airdForm)
        {
            InitializeComponent();
            this.handler = handler;
            this.airdForm = airdForm;
            this.initConfigInfo();
        }


        private void ConversionConfigListForm_Load(object sender, EventArgs e)
        {
           
           
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
                    configEntry.Value.outputPath
                });
                lvConfigList.Items.Add(item);
            }
        }

        private void btnConfigChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                tbConfigFolderPath.Text = fbd.SelectedPath;
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
            setConfigInfo();
            handler.saveConfig(tbNameConfig.Text, config);
        }

        //设置所有参数
        private void setConfigInfo()
        {
            config.mzPrecision = (int)Math.Pow(10, int.Parse(cbConfigMzPrecision.Text));
            config.outputPath = tbConfigFolderPath.Text;
            config.ignoreZeroIntensity = cbConfigIsZeroIntensityIgnore.Checked;
            config.threadAccelerate = cbConfigThreadAccelerate.Checked;

            config.mzIntComp = (IntCompType)Enum.Parse(typeof(IntCompType), configMzIntComp.SelectedItem.ToString());
            config.mzByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), configMzByteComp.SelectedItem.ToString());
            config.intByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), configIntByteComp.SelectedItem.ToString());
            if (cbConfigStack.Checked)
            {
                config.stack = true;
                config.digit = (int)Math.Log(int.Parse(cbConfigStackLayers.Text), 2);
            }
            else
            {
                config.stack = false;
            }
            config.suffix = tbConfigFileNameSuffix.Text;
            config.creator = tbConfigOperator.Text;
            
        }

        public void initConfigInfo()
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
            this.cbConfigMzPrecision.SelectedIndex = 1;
            this.configMzIntComp.SelectedItem = IntCompType.IBP.ToString(); //mz数组默认选择IBP的压缩内核
            this.configMzByteComp.SelectedItem = ByteCompType.Zlib.ToString(); //mz数组默认选择Zlib的压缩内核
            this.configIntByteComp.SelectedItem = ByteCompType.Zlib.ToString(); //intensity数组默认选择Zlib的压缩内核
            this.cbConfigStackLayers.SelectedItem = "256"; //当使用Stack Layer堆叠的时候,默认堆叠256层
            this.tbConfigFolderPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.tbConfigOperator.Text = Environment.UserName;
        }

        //不存储进内存，直接应用于当前文件
        private void btnApplyNow_Click(object sender, EventArgs e)
        {
            setConfigInfo();
            airdForm.applyNowConfig(config);
            this.Hide();
        }

        //双击查看指定参数
        private void configList_DoubleClick(object sender, EventArgs e)
        {
            Dictionary<string, ConversionConfig> configMap = new Dictionary<string, ConversionConfig>();
            int index = lvConfigList.FocusedItem.Index; //获取选中Item的索引值
            string configName = lvConfigList.Items[index].SubItems[0].Text;
            using (StreamReader file = File.OpenText(Path.Combine(Environment.CurrentDirectory, "ConversionConfig.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                configMap = (Dictionary<string, ConversionConfig>) serializer.Deserialize(file,
                        typeof(Dictionary<string, ConversionConfig>));
            }

            foreach (var configEntry in configMap)
            {
                if (configEntry.Key == configName)
                {
                    tbNameConfig.Text = configEntry.Key;
                    tbConfigFolderPath.Text = configEntry.Value.outputPath;
                    tbConfigFileNameSuffix.Text = configEntry.Value.suffix;
                    tbConfigOperator.Text = configEntry.Value.creator;
                    cbConfigIsZeroIntensityIgnore.Checked = configEntry.Value.ignoreZeroIntensity;
                    cbConfigThreadAccelerate.Checked = configEntry.Value.threadAccelerate;
                    cbConfigMzPrecision.Text = configEntry.Value.mzPrecision.ToString();
                    configMzIntComp.Text = configEntry.Value.mzIntComp.ToString();
                    configMzByteComp.Text = configEntry.Value.mzByteComp.ToString();
                    configIntByteComp.Text = configEntry.Value.intByteComp.ToString();
                    if (configEntry.Value.stack)
                    {
                        cbConfigStack.Checked = true;
                        cbConfigStackLayers.Text = Convert.ToString(Math.Pow(2, configEntry.Value.digit));
                    }
                    else
                    {
                        cbConfigStack.Checked = false;
                    }
                }
            }
        }

        //移除自定义ConfigList中的选中项
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvConfigList.SelectedItems.Count != 0)
            {
                ListView.SelectedListViewItemCollection configs = this.lvConfigList.SelectedItems;//获取所有选中的Items集合
                foreach (ListViewItem item in configs)
                {
                    string configName = item.SubItems[0].Text;
                    handler.removeConfig(configName);
                }
            }
            else
            {
                int index = lvConfigList.FocusedItem.Index;//获取指定当个项的索引值
                string configName = lvConfigList.Items[index].SubItems[0].Text;
                handler.removeConfig(configName);
            }
            
        }
    }
}
