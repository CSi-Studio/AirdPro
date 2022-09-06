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
using AirdPro.Storage.Config;

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
            this.numMaxTasks.Value = config.maxTasks;
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
            Program.globalConfigHandler.saveConfig(
                new GlobalConfig(
                    tbLastOpenPath.Text,
                    tbRedisHost.Text,
                    tbRedisPort.Text,
                    (int) numMaxTasks.Value));
            this.Hide();
        }
    }
}