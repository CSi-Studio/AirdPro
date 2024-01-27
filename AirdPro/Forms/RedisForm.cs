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
            if (RedisClient.GetInstance().Check())
            {
                RedisClient.GetInstance().Disconnect();
                UpdateRedisStatus(false);
            }
            else
            {
                ConnectToRedis();
            }
        }

        private void UpdateRedisStatus(bool connected)
        {
            if (connected)
            {
                Program.conversionForm.btnRedisSetting.BackgroundImage = ResourceUtil.ReadImage("Menu.Redis.png");
                btnConnect.Text = "Disconnect";
                lblStatus.BackColor = Color.Green;
            }
            else
            {
                Program.conversionForm.btnRedisSetting.BackgroundImage = ResourceUtil.ReadImage("Menu.Redis_Disconnected.png");
                btnConnect.Text = "Connect";
                lblStatus.BackColor = Color.Red;
            }
        }

        private void redisTimer_Tick(object sender, EventArgs e)
        {
            //开始消费消息时停止时钟遍历
            redisTimer.Stop();
            HeartBeat();
            RedisClient.GetInstance().Consume();
            redisTimer.Start();
        }

        private void HeartBeat()
        {
            UpdateRedisStatus(RedisClient.GetInstance().Check());
            
        }
        
        private void ConnectToRedis()
        {
            if (tbRedisHost.Text == null || tbRedisHost.Text.IsEmpty())
            {
                redisTimer.Enabled = false;
                MessageBox.Show(Constants.Tag.Redis_Host_Cannot_Be_Empty);
                return;
            }

            if (tbRedisPort.Text == null || tbRedisPort.IsEmpty())
            {
                tbRedisPort.Text = "6379";
            }

            RedisClient.GetInstance().Connect(tbRedisHost.Text, int.Parse(tbRedisPort.Text), 
                tbRedisUsername.Text, tbRedisPassword.Text);
            
            if (RedisClient.GetInstance().Check())
            {
                redisTimer.Enabled = true;
                UpdateRedisStatus(true);
            }
            else
            {
                MessageBox.Show(Constants.Tag.Connect_Failed_Please_Check_The_Redis_Host_And_Port);
                redisTimer.Enabled = false;
                UpdateRedisStatus(false);
            }
        }
    }
}