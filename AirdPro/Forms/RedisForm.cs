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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AirdPro.Properties;
using AirdPro.Redis;
using AirdPro.Utils;
using HZH_Controls;
using StackExchange.Redis;

namespace AirdPro.Forms
{
    public partial class RedisForm : Form
    {
        public RedisForm()
        {
            InitializeComponent();
            redisTimer.Interval = RedisClient.HeartBeatTime * 1000;
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

        /**
         * 更新页面状态机
         */
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

        /**
         * Redis现调机,用于消费转换任务,完成心跳,保持链接状态
         */
        private void redisTimer_Tick(object sender, EventArgs e)
        {
            //开始消费消息时停止时钟遍历
            redisTimer.Stop();
            HeartBeat();
            RedisClient.GetInstance().Consume();
            redisTimer.Start();
        }

        /**
         * 心跳功能,用于更新本节点在服务器端的活跃时间戳,同时也获取当前局域网内所有的AirdPro节点
         */
        private void HeartBeat()
        {
            UpdateRedisStatus(RedisClient.GetInstance().Check());
            RedisClient.GetInstance().RegisterOrUpdate();
            List<string> servers = RedisClient.GetInstance().GetServerList();
            listViewServers.Items.Clear();
            for (var i = 0; i < servers.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageKey = "AirdPro";
                item.Text = servers[i];
                listViewServers.Items.Add(item);
            }
            listViewServers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
                HeartBeat();
            }
            else
            {
                MessageBox.Show(Constants.Tag.Connect_Failed_Please_Check_The_Redis_Host_And_Port);
                redisTimer.Enabled = false;
                UpdateRedisStatus(false);
            }
        }

        /**
         * 查看各节点的服务器配置
         */
        private void listViewServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewServers.SelectedItems.Count == 1)
            {
                string ip = listViewServers.SelectedItems[0].Text;
                Dictionary<string, string> dict = RedisClient.GetInstance().GetServerInfo(ip);
                tbServerInfo.Text = "";
                foreach (var kv in dict)
                {
                    tbServerInfo.Text += kv.Key + ":" + kv.Value + "\r\n";
                }
            }
            else
            {
                tbServerInfo.Text = "";
            }
        }

        private void btnClearServerCache_Click(object sender, EventArgs e)
        {
            RedisClient.GetInstance().ClearServerCache();
        }
    }
}