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
using AirdPro.Domains.Job;
using AirdPro.Redis;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class GlobalSettingForm : Form
    {
        private GlobalConfigHandler configHandler;
        public GlobalSettingForm()
        {
            InitializeComponent();
            configHandler = new GlobalConfigHandler();
            updateWithConfig(configHandler.config);
        }

        private void updateWithConfig(GlobalConfig config)
        {
            this.tbLastOpenPath.Text = config.lastOpenPath;
            this.tbRedisHost.Text = config.redisHost;
            this.tbRedisPort.Text = config.redisPort;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (tbRedisHost.Text.IsNullOrEmpty())
            {
                timerConsumer.Enabled = false;
                MessageBox.Show("LIMS IP不能为空,监听器暂时关闭");
                return;
            }
            bool initResult = RedisClient.getInstance().connect(tbRedisHost.Text);
            if (initResult)
            {
                lblConnectStatus.Text = "Connected";
                lblConnectStatus.ForeColor = Color.Green;
                timerConsumer.Enabled = true;
            }
            else
            {
                MessageBox.Show("连接失败,请检查LIMS IP字符串是否配置正确,监听器暂时关闭");
                lblConnectStatus.Text = "Not Connect";
                lblConnectStatus.ForeColor = Color.Red;
                timerConsumer.Enabled = false;
            }
        }

        private void timerConsumer_Tick(object sender, EventArgs e)
        {
            RedisClient.getInstance().consume();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            timerConsumer.Enabled = false;
            RedisClient.getInstance().disconnect();
            lblConnectStatus.Text = "Not Connect";
            lblConnectStatus.ForeColor = Color.Red;
        }

        private void btnConfigChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                tbLastOpenPath.Text = fbd.SelectedPath;
            }
        }

        private void GlobalSettingForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            configHandler.saveConfig(new GlobalConfig(tbLastOpenPath.Text, tbRedisHost.Text, tbRedisPort.Text));
            this.Hide();
        }
    }
}
