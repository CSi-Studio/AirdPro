using System;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using AirdPro.Constants;
using AirdPro.Domains.Job;


namespace AirdPro.Forms
{
    public partial class ConversionConfigForm : Form
    {
        private ConversionConfigHandler handler;
        public ConversionConfigForm(ConversionConfigHandler handler)
        {
            InitializeComponent();
            this.handler = handler;
          
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
            this.Hide();
        }

        //TODO 俊杰
        public void readConfigInfo(ConversionConfig config)
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
        //设置所有参数
        public void setConfigInfo()
        {
            ConversionConfig config = new ConversionConfig();
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
            handler.saveConfig(tbNameConfig.Text, config);
        }

        private void ConversionConfigForm_Load(object sender, EventArgs e)
        {

        }
    }
}
