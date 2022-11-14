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
using AirdPro.Properties;
using AirdPro.Storage.Config;

namespace AirdPro.Forms
{
    public partial class GlobalSettingForm : Form
    {
        public GlobalSettingForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.Default.MaxTasks = (int)numMaxTasks.Value;
            Settings.Default.RedisHost = tbRedisHost.Text;
            Settings.Default.RedisPort = tbRedisPort.Text;
            Settings.Default.RedisUsername = tbRedisUsername.Text;
            Settings.Default.RedisPassword = tbRedisPassword.Text;
            Settings.Default.Save();
            this.Hide();
        }

        private void GlobalSettingForm_Load(object sender, EventArgs e)
        {
            numMaxTasks.Text = Settings.Default.MaxTasks.ToString();
            tbRedisHost.Text = Settings.Default.RedisHost;
            tbRedisPort.Text = Settings.Default.RedisPort;
            tbRedisUsername.Text = Settings.Default.RedisUsername;
            tbRedisPassword.Text = Settings.Default.RedisPassword;
        }
    }
}