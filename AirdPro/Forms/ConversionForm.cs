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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using AirdPro.Domains;
using AirdPro.Properties;
using AirdPro.Storage.Config;
using AirdSDK.Utils;
using ThermoFisher.CommonCore.Data;
using System.ComponentModel;
using AirdPro.Repository;
using AirdPro.Utils;
using AirdSDK.Constants;
using Newtonsoft.Json;

namespace AirdPro.Forms
{
    public partial class ConversionForm : Form
    {
        ArrayList jobIdList = new();
        BackgroundWorker bw;

        public ConversionForm()
        {
            InitializeComponent();
        }

        private void ConversionForm_Load(object sender, EventArgs e)
        {
            this.Text = SoftwareInfo.GetVersion() + Const.Dash + string.Join(SymbolConst.COMMA ,NetworkUtil.GetHostIpList());
            initJobsFromStorage();
            bw = new BackgroundWorker();
            bw.DoWork += (sender, e) => ConvertTaskManager.GetInstance().Run();
            // 创建一个ListViewSorter对象
            FileListSorter sorter = new FileListSorter();
            lvFileList.ListViewItemSorter = sorter;
            listViewJobInfo.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void initJobsFromStorage()
        {
            string jobInfoListJson = Settings.Default.JobInfoList;
            List<JobInfo> jobInfoList = JsonConvert.DeserializeObject<List<JobInfo>>(jobInfoListJson);
            foreach (var jobInfo in jobInfoList)
            {
                jobInfo.Reset();
                ListViewItem item = jobInfo.BuildItem();
                if (!ConvertTaskManager.GetInstance().JobTable.Contains(jobInfo.jobId))
                {
                    Program.conversionForm.lvFileList.Items.Add(item);
                    ConvertTaskManager.GetInstance().PushJob(jobInfo);
                }
            }
        }

        private void PrintJobInfo()
        {
            if (lvFileList.SelectedItems.Count != 0)
            {
                ListViewItem selectedItem = lvFileList.SelectedItems[lvFileList.SelectedItems.Count - 1];
                JobInfo jobInfo = (JobInfo)selectedItem.Tag;
                // 清空现有数据
                listViewJobInfo.Items.Clear();
                Dictionary<string, string> dict = jobInfo.GetJobDict();
                // 添加新数据
                foreach (var kvp in dict)
                {
                    ListViewItem item = new ListViewItem(kvp.Key);
                    item.SubItems.Add(kvp.Value);
                    listViewJobInfo.Items.Add(item);
                }
                
                // 调整列宽以适应内容
                listViewJobInfo.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
           
        }
        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (lvFileList.Items.Count == 0)
            {
                MessageBox.Show(Constants.Tag.No_File_Is_Selected);
                return;
            }

            DoConvert();
        }

        public void DoConvert()
        {
            if (lvFileList.Items.Count == 0)
            {
                return;
            }

            foreach (ListViewItem item in lvFileList.Items)
            {
                JobInfo jobInfo = (JobInfo)item.Tag;
                if (!ConvertTaskManager.GetInstance().FinishedTable.ContainsKey(jobInfo.jobId))
                {
                    item.Tag = jobInfo;
                    ConvertTaskManager.GetInstance().PushJob(jobInfo);
                }
            }

            try
            {
                if (!bw.IsBusy)
                {
                    bw.RunWorkerAsync();
                }

                Application.DoEvents();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        public void AddFile(string inputPath, string outputPath, string type, ConversionConfig config)
        {
            if (!inputPath.IsNullOrEmpty())
            {
                JobInfo jobInfo = new JobInfo(inputPath, outputPath, type, config);
                ListViewItem item = jobInfo.BuildItem();

                lvFileList.Items.Add(item);
                jobIdList.Add(jobInfo.jobId);
            }
        }

        private void RemoveFile(ListViewItem fileItem)
        {
            JobInfo jobInfo = (JobInfo)fileItem.Tag;
            if (jobInfo.threadId != -1 
                && !jobInfo.status.Equals(Status.Finished) 
                && !jobInfo.status.Equals(Status.Waiting)
                && !jobInfo.status.Equals(Status.Error))
            {
                MessageBox.Show(Constants.Tag.Cannot_Be_Deleted_When_Running);
                return;
            }

            jobIdList.Remove(fileItem.Text);
            fileItem.Remove();
            ConvertTaskManager.GetInstance().RemoveJob(jobInfo);
        }

        private void lvFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListView)sender).SelectedItems.Count == 1)
            {
                PrintJobInfo();
                PrintLog();
            }
        }

        private void logTimer_Tick(object sender, EventArgs e)
        {
            PrintLog();
            refresh();
        }

        private void PrintLog()
        {
            if (lvFileList.SelectedItems.Count != 0)
            {
                ListViewItem item = lvFileList.SelectedItems[lvFileList.SelectedItems.Count - 1];
                string content = Constants.Tag.Empty;
                JobInfo job = (JobInfo)item.Tag;
                for (int i = job.logs.Count - 1; i >= 0; i--)
                {
                    content += job.logs[i].dateTime + " " + job.logs[i].content + Const.Change_Line;
                }
                content += Const.Change_Line;
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
                if (ConvertTaskManager.GetInstance().JobTable[item.Text] != null)
                {
                    JobInfo job = ConvertTaskManager.GetInstance().JobTable[item.Text] as JobInfo;
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
            JobInfo jobInfo = (JobInfo)(item.Tag);
            ConversionConfig config = jobInfo.config;

            if (Program.configListForm == null || Program.configListForm.IsDisposed)
            {
                Program.configListForm = new ConversionConfigListForm(item);
            }

            //以下代码的顺序不能换,必须先Show,再执行showConfig操作
            Program.configListForm.Show();
            Program.configListForm.ShowConfig(Constants.Tag.Empty, config);
            Program.configListForm.BringToFront();
        }

        private void rerun_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFileList.SelectedItems)
            {
                JobInfo jobInfo = (JobInfo)item.Tag;
                if (ConvertTaskManager.GetInstance().FinishedTable.ContainsKey(jobInfo.jobId))
                {
                    ConvertTaskManager.GetInstance().FinishedTable.Remove(jobInfo.jobId);
                    ConvertTaskManager.GetInstance().PushJob(jobInfo);
                    jobInfo.RefreshItem(item);
                }
                else
                {
                    MessageBox.Show(Constants.Tag.Only_Finished_Job_Can_Rerun);
                }

                if (!bw.IsBusy)
                {
                    bw.RunWorkerAsync();
                }
            }
        }

        private void removeSelectedItems(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFileList.SelectedItems)
            {
                JobInfo jobInfo = (JobInfo)item.Tag;
                if (!jobInfo.status.Equals(ProcessingStatus.RUNNING))
                {
                    RemoveFile(item);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (Program.fileSelector == null || Program.fileSelector.IsDisposed)
            {
                Program.fileSelector = new VendorFileSelectorForm();
                Program.fileSelector.Show();
            }

            if (Program.fileSelector.Visible == false)
            {
                Program.fileSelector.ClearInfos();
                Program.fileSelector.Visible = true;
            }
       
            Program.fileSelector.BringToFront();
        }

        private void ConversionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //需要将未完成转换的任务保存到本地
            string jobInfoListStr = JsonConvert.SerializeObject(ConvertTaskManager.GetInstance().JobTable.Values,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Settings.Default.JobInfoList = jobInfoListStr;
            Settings.Default.Save();
            Environment.Exit(0);
        }

        private void btnCleanFinished_Click(object sender, EventArgs e)
        {
            if (lvFileList.Items.Count != 0)
            {
                foreach (ListViewItem item in lvFileList.Items)
                {
                    if (item.SubItems[ItemName.PROGRESS].Text.Equals(Status.Finished) ||
                        item.SubItems[ItemName.PROGRESS].Text.Equals(Status.Error))
                    {
                        item.Remove();
                        jobIdList.Remove(item.SubItems[0].Text);
                        ConvertTaskManager.GetInstance().JobTable.Remove(item.SubItems[0].Text);
                    }
                }
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (Program.configListForm == null || Program.configListForm.IsDisposed)
            {
                Program.configListForm = new ConversionConfigListForm();
            }

            Program.configListForm.Show();
            Program.configListForm.BringToFront();
        }

        private void timerTaskScan_Tick(object sender, EventArgs e)
        {
            DoConvert();
        }

        private void cbAutoExe_CheckedChanged(object sender, EventArgs e)
        {
            timerTaskScan.Enabled = cbAutoExe.Checked;
        }

        private void btnRedisSetting_Click(object sender, EventArgs e)
        {
            if (Program.redisForm == null || Program.redisForm.IsDisposed)
            {
                Program.redisForm = new RedisForm();
                Program.redisForm.Show();
            }
            
            Program.redisForm.Visible = true;
            Program.redisForm.BringToFront();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            if (Program.aboutForm == null || Program.aboutForm.IsDisposed)
            {
                Program.aboutForm = new AboutForm();
                Program.aboutForm.Show();
            }

            Program.aboutForm.Visible = true;
            Program.aboutForm.BringToFront();
        }

        private void btnMainView_Click(object sender, EventArgs e)
        {
            if (Program.mainForm == null || Program.mainForm.IsDisposed)
            {
                Program.mainForm = new MainForm();
            }

            Program.mainForm.Show();
            Program.mainForm.BringToFront();
        }

        private bool IsCtrlA(KeyEventArgs e)
        {
            return (e.Control && e.KeyCode == Keys.A);
        }

        private bool IsDelete(KeyEventArgs e)
        {
            return e.KeyCode == Keys.Delete;
        }

        private void lvFileList_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsCtrlA(e))
            {
                foreach (ListViewItem item in ((ListView)sender).Items)
                {
                    item.Selected = true;
                }

                e.SuppressKeyPress = true; // 阻止控件处理这个按键事件  
            }
            else if (IsDelete(e))
            {
                removeSelectedItems(null, null);
            }
        }

        private void btnMetaboLights_Click(object sender, EventArgs e)
        {
            if (Program.mlForm == null || Program.mlForm.IsDisposed)
            {
                Program.mlForm = new MLForm();
            }

            Program.mlForm.Show();
            Program.mlForm.BringToFront();
        }

        private void btnPX_Click(object sender, EventArgs e)
        {
            if (Program.pxForm == null || Program.pxForm.IsDisposed)
            {
                Program.pxForm = new PXForm();
            }

            Program.pxForm.Show();
            Program.pxForm.BringToFront();
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFileList.SelectedItems)
            {
                JobInfo jobInfo = (JobInfo)item.Tag;
                if (!jobInfo.status.Equals(ProcessingStatus.RUNNING))
                {
                    RemoveFile(item);
                }
            }
        }

        private void lvFileList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvFileList.Sort();
            FileListSorter.isAscending = !FileListSorter.isAscending;
        }

        private void btnDownloadJobList_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, string>> jobList = new List<Dictionary<string, string>>();
            foreach (ListViewItem item in lvFileList.Items)
            {
                JobInfo jobInfo = item.Tag as JobInfo;
                jobList.Add(jobInfo.GetJobDict());
            }

            string result = JsonConvert.SerializeObject(jobList);
            var popup = new CustomPopup();
            popup.Text = "Job Info List";
            popup.content.Text = result;
            popup.ShowDialog();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            int totalSpectra = 139054;
            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            dict.Add(10, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 10));
            dict.Add(20, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 20));
            dict.Add(50, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 50));
            dict.Add(100, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 100));
            dict.Add(200, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 200));
            dict.Add(500, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 500));
            dict.Add(1000, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 1000));
            dict.Add(2000, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 2000));
            dict.Add(5000, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 5000));
            dict.Add(10000, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 10000));
            dict.Add(20000, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 20000));
            dict.Add(50000, SpeedTestUtil.GenerateUniqueRandomNumbers(0, totalSpectra, 50000));
            Console.WriteLine("10,20,50,100,200,500,1000,2000,5000,10000,20000,50000");
            SpeedTestUtil.Test("D:\\Aird2.0\\numpress\\18.mzML", dict);
            Console.WriteLine();
            SpeedTestUtil.Test("D:\\Aird2.0\\numpress\\18.mzMLb", dict);
            Console.WriteLine();
            SpeedTestUtil.Test("D:\\Aird2.0\\18.mzMLb", dict);
            Console.WriteLine();
            SpeedTestUtil.Test("D:\\Aird2.0\\18.mzML", dict);
            Console.WriteLine();
            SpeedTestUtil.Test("D:\\Aird2.0\\Vendor\\18.raw", dict);
            Console.WriteLine();
        }
    }
}