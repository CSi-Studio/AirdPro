using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirdPro.Redis;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class GlobalSettingForm : Form
    {
        public GlobalSettingForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (tbHostAndPort.Text.IsNullOrEmpty())
            {
                timerConsumer.Enabled = false;
                MessageBox.Show("LIMS IP不能为空,监听器暂时关闭");
                return;
            }
            bool initResult = RedisClient.getInstance().connect(tbHostAndPort.Text);
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
    }
}
