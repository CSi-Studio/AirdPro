/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.Asyncs;
using AirdPro.Constants;
using AirdPro.Redis;
using AirdPro.Utils;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using AirdPro.Domains;
using AirdPro.Properties;
using AirdPro.Storage.Config;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class ConversionForm : Form
    {
        ArrayList jobIdList = new();
        VendorFileSelectorForm fileSelector;
        ConversionConfigListForm configListForm;
        GlobalSettingForm globalSettingForm;

        public ConversionForm()
        {
            InitializeComponent();
        }

        private void ProproForm_Load(object sender, EventArgs e)
        {
            this.Text = SoftwareInfo.getVersion() + Const.Dash + NetworkUtil.getHostIP();
            RedisClient.getInstance();
            ConvertTaskManager.getInstance().run();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (lvFileList.Items.Count == 0)
            {
                MessageBox.Show(Constants.Tag.No_File_Is_Selected);
                return;
            }

            foreach (ListViewItem item in lvFileList.Items)
            {
                JobInfo jobInfo = (JobInfo) item.Tag;
                if (!ConvertTaskManager.getInstance().finishedTable.ContainsKey(jobInfo.getJobId()))
                {
                    item.Tag = jobInfo;
                    ConvertTaskManager.getInstance().pushJob(jobInfo);
                }
            }

            try
            {
                ConvertTaskManager.getInstance().run();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        public void addFile(string inputPath, string outputPath, string type, ConversionConfig config)
        {
            if (!inputPath.IsNullOrEmpty())
            {
                JobInfo jobInfo = new JobInfo(inputPath, outputPath, type, config);
                ListViewItem item = jobInfo.buildItem();

                lvFileList.Items.Add(item);
                jobIdList.Add(jobInfo.getJobId());
            }
        }

        private void removeFile(ListViewItem fileItem)
        {
            JobInfo jobInfo = (JobInfo) fileItem.Tag;
            if (jobInfo.threadId != -1 && !jobInfo.status.Equals(Status.Finished))
            {
                MessageBox.Show(Constants.Tag.Cannot_Be_Deleted_When_Running);
                return;
            }

            jobIdList.Remove(fileItem.Text);
            fileItem.Remove();
            ConvertTaskManager.getInstance().removeJob(jobInfo);
        }

        private void lvFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            printLog();
        }

        private void logTimer_Tick(object sender, EventArgs e)
        {
            printLog();
            refresh();
        }

        private void printLog()
        {
            if (lvFileList.SelectedItems.Count != 0)
            {
                ListViewItem item = lvFileList.SelectedItems[lvFileList.SelectedItems.Count - 1];
                string content = Constants.Tag.Empty;
                JobInfo job;
                if (ConvertTaskManager.getInstance().jobTable[item.Text] != null)
                {
                    job = ConvertTaskManager.getInstance().jobTable[item.Text] as JobInfo;

                    for (int i = job.logs.Count - 1; i >= 0; i--)
                    {
                        content += job.logs[i].dateTime + " " + job.logs[i].content + Const.Change_Line;
                    }

                    string jobInfo = job.getJsonInfo();
                    content += jobInfo + Const.Change_Line;
                }
                else if (ConvertTaskManager.getInstance().finishedTable[item.Text] != null)
                {
                    job = ConvertTaskManager.getInstance().finishedTable[item.Text] as JobInfo;

                    for (int i = job.logs.Count - 1; i >= 0; i--)
                    {
                        content += job.logs[i].dateTime + " " + job.logs[i].content + Const.Change_Line;
                    }

                    string jobInfo = job.getJsonInfo();
                    content += jobInfo + Const.Change_Line;
                }
                else
                {
                    content = Constants.Tag.Not_Start_Converting;
                }

                tbConsole.Text = content;
            }
            else
            {
                tbConsole.Text = Constants.Tag.Select_Item_To_Watch_Logs;
            }
        }

        private void refresh()
        {
            foreach (ListViewItem item in lvFileList.Items)
            {
                if (ConvertTaskManager.getInstance().jobTable[item.Text] != null)
                {
                    JobInfo job = ConvertTaskManager.getInstance().jobTable[item.Text] as JobInfo;
                    job.refreshReport = true;
                }
            }
        }

        //查看列表选中对象的详细参数
        private void lvFileList_DoubleClick(object sender, EventArgs e)
        {
            if (lvFileList.SelectedItems.Count == 0) //判断选中的不为0
            {
                return;
            }

            int index = lvFileList.FocusedItem.Index; //获取选中Item的索引值
            ListViewItem item = lvFileList.Items[index];
            JobInfo jobInfo = (JobInfo) (item.Tag);
            ConversionConfig config = jobInfo.config;

            if (configListForm == null || configListForm.IsDisposed)
            {
                configListForm = new ConversionConfigListForm(item);
            }

            //以下代码的顺序不能换,必须先Show,再执行showConfig操作
            configListForm.Show();
            configListForm.showConfig(Constants.Tag.Empty, config);
        }

        private void selectFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileSelector == null || fileSelector.IsDisposed)
            {
                fileSelector = new VendorFileSelectorForm();
            }

            fileSelector.clearInfos();
            fileSelector.Show();
        }

        private void globalSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalSettingForm == null || globalSettingForm.IsDisposed)
            {
                globalSettingForm = new GlobalSettingForm();
            }

            globalSettingForm.Show();
        }

        private void rerun_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFileList.SelectedItems)
            {
                JobInfo jobInfo = (JobInfo) item.Tag;
                if (ConvertTaskManager.getInstance().finishedTable.ContainsKey(jobInfo.getJobId()))
                {
                    ConvertTaskManager.getInstance().finishedTable.Remove(jobInfo.getJobId());
                    ConvertTaskManager.getInstance().pushJob(jobInfo);
                    jobInfo.refreshItem(item);
                }
                else
                {
                    MessageBox.Show(Constants.Tag.Only_Finished_Job_Can_Rerun);
                }

                ConvertTaskManager.getInstance().run();
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFileList.SelectedItems)
            {
                JobInfo jobInfo = (JobInfo) item.Tag;
                if (!jobInfo.status.Equals(ProcessingStatus.RUNNING))
                {
                    removeFile(item);
                }
            }
        }

        private void redisConsumer_Tick(object sender, EventArgs e)
        {
            RedisClient.getInstance().consume();
        }

        public void connectToRedis()
        {
            string connectLink = Settings.Default.RedisHost + ":" + Settings.Default.RedisPort;
            if (connectLink.IsNullOrEmpty())
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

        public void disconnectFromRedis()
        {
            RedisClient.getInstance().disconnect();
            updateRedisStatus(false);
        }

        private void updateRedisStatus(bool connected)
        {
            if (connected)
            {
                btnRedis.BackgroundImage = Properties.Resources.Connected;
                lblRedisStatus.Text = Status.Redis_Connected;
                lblRedisStatus.ForeColor = Color.Green;
            }
            else
            {
                btnRedis.BackgroundImage = Properties.Resources.DisConnect;
                lblRedisStatus.Text = Status.Redis_Not_Connected;
                lblRedisStatus.ForeColor = Color.Red;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (fileSelector == null || fileSelector.IsDisposed)
            {
                fileSelector = new VendorFileSelectorForm();
            }

            fileSelector.clearInfos();
            fileSelector.Show();
        }

        private void ConversionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void btnRedis_Click(object sender, EventArgs e)
        {
            if (RedisClient.getInstance().check())
            {
                disconnectFromRedis();
            }
            else
            {
                connectToRedis();
            }
        }

        private void btnCleanFinished_Click(object sender, EventArgs e)
        {
            if (lvFileList.Items.Count != 0)
            {
                foreach (ListViewItem item in lvFileList.Items)
                {
                    if (item.SubItems[ItemName.PROGRESS].Text.Equals(Status.Finished))
                    {
                        item.Remove();
                        jobIdList.Remove(item.SubItems[0].Text);
                        ConvertTaskManager.getInstance().jobTable.Remove(item.SubItems[0].Text);
                    }
                }
            }
        }

        private void btnCleanErrors_Click(object sender, EventArgs e)
        {
            // ConvertTaskManager.getInstance().finishedTable.Clear();
            if (lvFileList.Items.Count != 0)
            {
                foreach (ListViewItem item in lvFileList.Items)
                {
                    if (item.SubItems[ItemName.PROGRESS].Text.Equals(Status.Error))
                    {
                        item.Remove();
                        jobIdList.Remove(item.SubItems[0].Text);
                        ConvertTaskManager.getInstance().jobTable.Remove(item.SubItems[0].Text);
                    }
                }
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (configListForm == null || configListForm.IsDisposed)
            {
                configListForm = new ConversionConfigListForm();
            }

            configListForm.Show();
        }
    }
}