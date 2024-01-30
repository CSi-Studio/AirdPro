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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Properties;
using AirdPro.Redis;
using AirdPro.Utils;
using HZH_Controls;
using Newtonsoft.Json;
using StackExchange.Redis;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace AirdPro.Forms
{
    public partial class RedisForm : Form
    {
        public RedisForm()
        {
            InitializeComponent();
            heartBeatTimer.Interval = RedisClient.HeartBeatTime * 1000;
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
            if (RedisClient.Instance.Check())
            {
                RedisClient.Instance.Disconnect();
                consumeTimer.Stop();
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
            HeartBeat();
        }

        /**
         * 心跳功能,用于更新本节点在服务器端的活跃时间戳,同时也获取当前局域网内所有的AirdPro节点
         */
        private void HeartBeat()
        {
            UpdateRedisStatus(RedisClient.Instance.Check());
            if (!RedisClient.Instance.Check()) return;
            RedisClient.Instance.RegisterOrUpdate();
            LoadServers();
            LoadJobs();
        }

        private void LoadServers()
        {
            List<string> servers = RedisClient.Instance.GetServerList();
            lvServers.Items.Clear();
            for (var i = 0; i < servers.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageKey = "AirdPro";
                item.Text = servers[i];
                lvServers.Items.Add(item);
            }
        }

        private void LoadJobs()
        {
            List<RemoteConvertJob> jobList = RedisClient.Instance.GetTodoJobs();
            List<RemoteConvertJob> jobListUnderConverting = RedisClient.Instance.GetConvertingJobs();
            jobList.AddRange(jobListUnderConverting);
            lvJobs.Items.Clear();
            foreach (RemoteConvertJob remoteJob in jobList)
            {
                ListViewItem item = new ListViewItem(remoteJob.remoteId);
                item.SubItems.Add(remoteJob.type);
                item.SubItems.Add(remoteJob.scene);
                item.SubItems.Add(remoteJob.sourcePath);
                item.SubItems.Add(remoteJob.targetPath);
                item.SubItems.Add(remoteJob.consumeIP);
                item.SubItems.Add(remoteJob.consumeTime);
                item.Tag = remoteJob;
                lvJobs.Items.Add(item);
            }
        }
        
        private void ConnectToRedis()
        {
            if (tbRedisHost.Text == null || tbRedisHost.Text.IsEmpty())
            {
                MessageBox.Show(Constants.Tag.Redis_Host_Cannot_Be_Empty);
                return;
            }

            if (tbRedisPort.Text == null || tbRedisPort.IsEmpty())
            {
                tbRedisPort.Text = "6379";
            }

            RedisClient.Instance.Connect(tbRedisHost.Text, int.Parse(tbRedisPort.Text), 
                tbRedisUsername.Text, tbRedisPassword.Text);
            
            if (RedisClient.Instance.Check())
            {
                HeartBeat();
                LoadJobs();
            }
            else
            {
                MessageBox.Show(Constants.Tag.Connect_Failed_Please_Check_The_Redis_Host_And_Port);
                UpdateRedisStatus(false);
            }
        }

        /**
         * 查看各节点的服务器配置
         */
        private void listViewServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvServers.SelectedItems.Count == 1)
            {
                string ip = lvServers.SelectedItems[0].Text;
                Dictionary<string, string> dict = RedisClient.Instance.GetServerInfo(ip);
                tbConsole.Text = "IP:"+ ip + "\r\n";
                foreach (var kv in dict)
                {
                    tbConsole.Text += kv.Key + ":" + kv.Value + "\r\n";
                }
            }
            else
            {
                tbConsole.Text = "";
            }
        }

        private void btnClearServerCache_Click(object sender, EventArgs e)
        {
            RedisClient.Instance.ClearServerCache();
            LoadServers();
        }

        private void btnConsume_Click(object sender, EventArgs e)
        {
            RedisClient.Instance.Consume();
        }

        private void btnRefreshJobList_Click(object sender, EventArgs e)
        {
            LoadJobs();
        }

        private void lvJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvJobs.SelectedItems.Count == 1)
            {
                RemoteConvertJob job = (RemoteConvertJob)lvJobs.SelectedItems[0].Tag;
                tbConsole.Text = JsonConvert.SerializeObject(job);
            }
        }

        private void consumeTimer_Tick(object sender, EventArgs e)
        {
            RedisClient.Instance.Consume();
        }
    }
}