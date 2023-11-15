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
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using AirdPro.Properties;
using AirdPro.Redis;
using AirdPro.Utils;
using HZH_Controls;

namespace AirdPro.Forms
{
    public partial class RedisForm : Form
    {
        public RedisForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.Default.RedisHost = tbRedisHost.Text;
            Settings.Default.RedisPort = tbRedisPort.Text;
            Settings.Default.RedisUsername = tbRedisUsername.Text;
            Settings.Default.RedisPassword = tbRedisPassword.Text;
            Settings.Default.Save();
        }

        private void RedisForm_Load(object sender, EventArgs e)
        {
            tbRedisHost.Text = Settings.Default.RedisHost;
            tbRedisPort.Text = Settings.Default.RedisPort;
            tbRedisUsername.Text = Settings.Default.RedisUsername;
            tbRedisPassword.Text = Settings.Default.RedisPassword;
        }

        private void RedisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (RedisClient.getInstance().check())
            {
                RedisClient.getInstance().disconnect();
                updateRedisStatus(false);
            }
            else
            {
                connectToRedis();
            }
        }

        private void updateRedisStatus(bool connected)
        {
            if (connected)
            {
                Program.conversionForm.btnRedisSetting.BackgroundImage = ResourceUtil.readImage("Menu.Redis.png");
                this.btnConnect.Text = "Connected";
            }
            else
            {
                Program.conversionForm.btnRedisSetting.BackgroundImage = ResourceUtil.readImage("Menu.Redis_Disconnected.png");
                this.btnConnect.Text = "Disconnected";
            }
        }

        private void redisConsumer_Tick(object sender, EventArgs e)
        {
            redisConsumer.Stop();
            RedisClient.getInstance().consume();
            redisConsumer.Start();
        }
        
        public void connectToRedis()
        {
            string connectLink = Settings.Default.RedisHost + ":" + Settings.Default.RedisPort;
            if (connectLink == null || connectLink.IsEmpty())
            {
                redisConsumer.Enabled = false;
                MessageBox.Show(Constants.Tag.Redis_Host_Cannot_Be_Empty);
                return;
            }

            bool initResult = RedisClient.getInstance().connect(connectLink);
            if (initResult)
            {
                redisConsumer.Enabled = true;
                updateRedisStatus(true);
            }
            else
            {
                MessageBox.Show(Constants.Tag.Connect_Failed_Please_Check_The_Redis_Host_And_Port);
                redisConsumer.Enabled = false;
                updateRedisStatus(false);
            }
        }
    }
}