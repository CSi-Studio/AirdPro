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
using AirdPro.Storage.Config;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class GlobalSettingForm : Form
    {
        public GlobalSettingForm()
        {
            InitializeComponent();
            updateWithConfig(Program.globalConfigHandler.config);
        }

        private void updateWithConfig(GlobalConfig config)
        {
            this.tbLastOpenPath.Text = config.defaultOpenPath;
            this.tbRedisHost.Text = config.redisHost;
            this.tbRedisPort.Text = config.redisPort;
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
            Program.globalConfigHandler.saveConfig(new GlobalConfig(tbLastOpenPath.Text, tbRedisHost.Text, tbRedisPort.Text));
            this.Hide();
        }
    }
}
