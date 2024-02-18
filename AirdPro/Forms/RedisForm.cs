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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AirdPro.Domains;
using AirdPro.Properties;
using AirdPro.Redis;
using AirdPro.Utils;
using HZH_Controls;
using HZH_Controls.Controls;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace AirdPro.Forms
{
    public partial class RedisForm : Form
    {
        public static bool JobUnderConsuming = false; //当前是否有远程任务正在执行

        public RedisForm()
        {
            InitializeComponent();
            consumeTimer.Interval = RedisManager.ConsumeInterval;
            heartBeatTimer.Interval = RedisManager.HeartBeatInterval;
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
            Visible = false;
        }

        //保存Redis配置项字段
        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.Default.RedisHost = tbRedisHost.Text;
            Settings.Default.RedisPort = tbRedisPort.Text;
            Settings.Default.RedisUsername = tbRedisUsername.Text;
            Settings.Default.RedisPassword = tbRedisPassword.Text;
            Settings.Default.Save();
        }

        //当重新连接Redis时,会启动Redis任务消费功能
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (RedisManager.Instance.Check())
            {
                RedisManager.Instance.Disconnect();
                JobUnderConsuming = false;
            }
            else
            {
                ConnectToRedis();
            }
        }

        //更新页面状态机
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
                Program.conversionForm.btnRedisSetting.BackgroundImage =
                    ResourceUtil.ReadImage("Menu.Redis_Disconnected.png");
                btnConnect.Text = "Connect";
                lblStatus.BackColor = Color.Red;
            }
        }

        /**
         * 心跳功能,用于更新本节点在服务器端的活跃时间戳,同时也获取当前局域网内所有的AirdPro节点
         */
        private void HeartBeat()
        {
            UpdateRedisStatus(RedisManager.Instance.Check());
            if (!RedisManager.Instance.Check()) return;
            RedisManager.Instance.RegisterOrUpdate();
        }

        //更新服务节点列表
        public void LoadServers()
        {
            Dictionary<string, ClientInfo> serverMap = RedisManager.Instance.GetServerMap();
            lvServers.Items.Clear();
            foreach (var kv in serverMap)
            {
                ListViewItem item = new ListViewItem(kv.Key);
                ClientInfo info = kv.Value;
                item.SubItems.Add(string.Join(";", info.IPList));
                item.SubItems.Add(info.ServerName);
                item.SubItems.Add(info.OpVersion);
                item.SubItems.Add(info.CpuInfo);
                item.SubItems.Add(info.PhysicMemory);
                item.SubItems.Add(info.AirdProVersion);
                item.SubItems.Add(info.ConsumingJob ? "ON" : "OFF");
                lvServers.Items.Add(item);
            }
        }

        //更新远程任务列表
        private void LoadJobs()
        {
            List<RemoteConvertJob> jobList = RedisManager.Instance.GetTodoJobs();
            List<RemoteConvertJob> jobListUnderConverting = RedisManager.Instance.GetConvertingJobs();
            jobList.AddRange(jobListUnderConverting);
            lvJobs.Items.Clear();
            foreach (RemoteConvertJob remoteJob in jobList)
            {
                ListViewItem item = new ListViewItem(remoteJob.remoteId);
                item.SubItems.Add(remoteJob.type);
                item.SubItems.Add(remoteJob.scene);
                FileInfo info = new FileInfo(remoteJob.sourcePath);
                item.SubItems.Add(info.Name);
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

            RedisManager.Instance.Connect(tbRedisHost.Text, int.Parse(tbRedisPort.Text),
                tbRedisUsername.Text, tbRedisPassword.Text);

            if (RedisManager.Instance.Check())
            {
                UpdateRedisStatus(true);
                RedisManager.Instance.RegisterOrUpdate();
                LoadServers();
                LoadJobs();
            }
            else
            {
                MessageBox.Show(Constants.Tag.Connect_Failed_Please_Check_The_Redis_Host_And_Port);
                UpdateRedisStatus(false);
            }
        }

        private void btnRefreshJobList_Click(object sender, EventArgs e)
        {
            LoadJobs();
            LoadServers();
        }

        //执行异步线程,如果当前没有相关的任务,则开始消费一个
        public void Consume()
        {
            if (!JobUnderConsuming)
            {
                RedisManager.Instance.Consume();
            }
        }

        //全局任务消费开关
        private void switchConsumeJob_CheckedChanged(object sender, EventArgs e)
        {
            UCSwitch switcher = (UCSwitch)sender;
            RedisManager.GlobalConsumeJobSwitch = switcher.Checked;
            lblSwitch.Text = "Consume " + (switcher.Checked ? "On" : "Off");
        }

        private void consumeTimer_Tick(object sender, EventArgs e)
        {
            Consume();
        }

        private void openConsumeSwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> serverIps = new List<string>();
            foreach (ListViewItem selectedItem in lvServers.SelectedItems)
            {
                serverIps.Add(selectedItem.SubItems[0].Text);
            }

            RedisManager.Instance.OpenConsume(serverIps);
        }

        private void closeConsumeSwitchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> serverIps = new List<string>();
            foreach (ListViewItem selectedItem in lvServers.SelectedItems)
            {
                serverIps.Add(selectedItem.SubItems[0].Text);
            }

            RedisManager.Instance.CloseConsume(serverIps);
        }

        private void heartBeatTimer_Tick(object sender, EventArgs e)
        {
            HeartBeat();
        }

        private void btnClearRedisCache_Click(object sender, EventArgs e)
        {
            RedisManager.Instance.ClearServerCache();
            LoadServers();
        }

        private void btnClearTempFiles_Click(object sender, EventArgs e)
        {
            AirdProFileUtil.ClearLocalTempFiles();
        }
    }
}