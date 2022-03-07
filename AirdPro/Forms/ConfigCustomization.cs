using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using AirdPro.Constants;
using System.IO;
using Newtonsoft.Json;
using AirdPro.Forms;
using AirdPro.Forms.Config;


namespace AirdPro.Forms
{
    public partial class ConfigCustomization : Form
    {
        //定义所有参数以ConfigInfo类保存
        [Serializable]
        public class ConfigInfo
        {
            public string configName { get; set; }
            public string creator { get; set; }
            public string configMzPrecision { get; set; }
            public string configCompressor { get; set; }
            public string configOutputPath { get; set; }
            public bool ignoreZeroIntensity { get; set; }
            public bool threadAccelerate { get; set; }
            public IntCompType mzIntComp { get; set; }
            public ByteCompType mzByteComp { get; set; }
            public ByteCompType intByteComp { get; set; }
            public bool stack { get; set; }
            public int digit { get; set; }
            public string suffix { get; set; }
        }
        ConfigInfo configInfo = new ConfigInfo();
        public ConfigCustomization()
        {
            InitializeComponent();
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

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            setConfigInfo();
            ConfigSubject configSubject =  new ConfigParamsSubject(configInfo);
            ConfigSubject configSubject1 = new ConfigParamsSubject(configInfo.configName, configInfo.creator, configInfo.configMzPrecision,
                                                             configInfo.configCompressor, configInfo.configOutputPath);
            this.Hide();
        }
        //设置所有参数
        public void setConfigInfo()
        {
            configInfo.configMzPrecision = Convert.ToString(Math.Pow(10, int.Parse(cbConfigMzPrecision.Text)));
            configInfo.configOutputPath = tbConfigFolderPath.Text;
            if (cbConfigIsZeroIntensityIgnore.Checked)
            {
                configInfo.ignoreZeroIntensity = true;
            }
            else
            {
                configInfo.ignoreZeroIntensity = false;
            }
            if (cbConfigThreadAccelerate.Checked)
            {
                configInfo.threadAccelerate = true;
            }
            else
            {
                configInfo.threadAccelerate = false;
            }
            configInfo.mzIntComp = (IntCompType)Enum.Parse(typeof(IntCompType), configMzIntComp.SelectedItem.ToString());
            configInfo.mzByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), configMzByteComp.SelectedItem.ToString());
            configInfo.intByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), configIntByteComp.SelectedItem.ToString());
            if (cbConfigStack.Checked)
            {
                configInfo.stack = true;
                configInfo.digit = (int)Math.Log(int.Parse(cbConfigStackLayers.Text), 2);
            }
            else
            {
                configInfo.stack = false;
            }
            configInfo.suffix = tbConfigFileNameSuffix.Text;
            configInfo.creator = tbConfigOperator.Text;
            configInfo.configName = tbNameConfig.Text;
            configInfo.configCompressor = configMzIntComp.SelectedItem.ToString() + "|" + configMzByteComp.SelectedItem.ToString() + "|"
                                          + configIntByteComp.SelectedItem.ToString();
        }
    }
}
