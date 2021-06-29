/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
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
using System.Drawing;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using AirdPro.Redis;
using AirdPro.Test;
using ThermoFisher.CommonCore.Data;
using ThermoFisher.CommonCore.Data.Interfaces;

namespace AirdPro.Forms
{
    public partial class AirdForm : Form
    {
        ArrayList currentFiles = new ArrayList(); 
        CustomPathForm customPathForm;
        Folder Folder;
        public AirdForm()
        {
            InitializeComponent();
        }

        private void ProproForm_Load(object sender, EventArgs e)
        {
            this.Text = SoftwareInfo.getVersion();
            this.cbMzPrecision.SelectedIndex = 1; //默认选择精确到小数点后4位的精度
            this.cbAlgorithm.SelectedIndex = 0; //默认选择Aird第一代压缩算法ZDPD
            this.cbStackLayers.SelectedIndex = 3; //默认堆叠256层
            this.tbFolderPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.tbOperator.Text = Environment.UserName;
            RedisClient.getInstance();
            ConvertTaskManager.getInstance().run();
        }

        private void btnChooseFiles_Click(object sender, EventArgs e)
        {
            ofd.Title = "Please Choose DIA/SWATH Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, AirdType.DIA_SWATH);
                }
            }
        }

        private void btnChoosePRMFiles_Click(object sender, EventArgs e)
        {
            ofd.Title = "Please Choose PRM Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, AirdType.PRM);
                }
            }
        }

        private void btnChooseSSwathFiles_Click(object sender, EventArgs e)
        {
            ofd.Title = "Please Choose Scanning SWATH Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, AirdType.SCANNING_SWATH);
                }
            }
        }

        private void btnChooseDDAFile_Click(object sender, EventArgs e)
        {
            ofd.Title = "Please Choose DDA Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, AirdType.DDA);
                }
            }
        }

        private void btnAddCommonFiles_Click(object sender, EventArgs e)
        {
            ofd.Title = "Please Choose Common Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, AirdType.COMMON);
                }
            }
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                tbFolderPath.Text = fbd.SelectedPath;
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (tbFolderPath.Text == "")
            {
                MessageBox.Show("Output folder path is empty!");
                return;
            }

            if (lvFileList.Items.Count == 0)
            {
                MessageBox.Show("No file is selected!");
                return;
            }
            
            foreach (ListViewItem item in lvFileList.Items)
            {
                if (!ConvertTaskManager.getInstance().jobTable.ContainsKey(item.SubItems[0].Text))
                {
                    JobParams jobParams = new JobParams
                    {
                        ignoreZeroIntensity = cbIsZeroIntensityIgnore.Checked,
                        log2 = cbLog2.Checked,
                        threadAccelerate = cbThreadAccelerate.Checked,
                        suffix = tbFileNameSuffix.Text,
                        creator = tbOperator.Text,
                        mzPrecision = Double.Parse(cbMzPrecision.Text),
                        airdAlgorithm = cbAlgorithm.SelectedIndex+1,  // 1:ZDPD, 2:StackZDPD
                        digit = (int)Math.Log(Int32.Parse(cbStackLayers.SelectedItem.ToString()), 2),
                        includeCV = cbIncludingPSICV.Checked
                    };

                    JobInfo jobInfo = new JobInfo(item.SubItems[0].Text, tbFolderPath.Text,
                        item.SubItems[1].Text, jobParams, item);

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

        private void btnDeleteFiles_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvFileList.SelectedItems)
            {
                removeFile(item);
            }
           
        }
        public void addFile(string fileName,string expType)
        {
            if (fileName != "" && !currentFiles.Contains(fileName))
            {
                ListViewItem item = new ListViewItem(new string[]{fileName, expType,"", "Waiting",""});
                item.ToolTipText = fileName;
                lvFileList.Items.Add(item);
                currentFiles.Add(fileName);
                lblFileSelectedInfo.Text = currentFiles.Count + " files selected";
            }
        }

        private void removeFile(ListViewItem fileItem)
        {
            currentFiles.Remove(fileItem.Text);
            if (currentFiles.Count == 0)
            {
                lblFileSelectedInfo.Text = "No file is selected";
            }
            else
            {
                lblFileSelectedInfo.Text = currentFiles.Count + "files are selected";
            }
            fileItem.Remove();
            ConvertTaskManager.getInstance().jobTable.Remove(fileItem.Text);
        }

        private void lvFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            printLog();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            printLog();
            refresh();
        }

        private void printLog()
        {
            if (lvFileList.SelectedItems.Count != 0)
            {
                ListViewItem item = lvFileList.SelectedItems[lvFileList.SelectedItems.Count - 1];
                string content = "";
                JobInfo job = null;
                if (ConvertTaskManager.getInstance().jobTable[item.Text] != null)
                {
                    job = ConvertTaskManager.getInstance().jobTable[item.Text] as JobInfo;
                    
                    for (int i=job.logs.Count-1;i>=0;i--)
                    {
                        content += job.logs[i].dateTime + "   " + job.logs[i].content + "\r\n";
                    }
                    string jobInfo = job.getJsonInfo();
                    content += jobInfo + "\r\n";
                }
                else if (ConvertTaskManager.getInstance().errorJob[item.Text] != null)
                {
                    job = ConvertTaskManager.getInstance().errorJob[item.Text] as JobInfo;

                    for (int i = job.logs.Count - 1; i >= 0; i--)
                    {
                        content += job.logs[i].dateTime + "   " + job.logs[i].content + "\r\n";
                    }
                    string jobInfo = job.getJsonInfo();
                    content += jobInfo + "\r\n";
                }
                else
                {
                    content = "Not start converting!";
                }
                
                tbConsole.Text = content;
            }
            else
            {
                tbConsole.Text = "select item to watch logs";
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (lvFileList.Items.Count != 0)
            {
                foreach (ListViewItem item in lvFileList.Items)
                {
                    if (item.SubItems[3].Text.Equals("Finished"))
                    {
                        item.Remove();
                        currentFiles.Remove(item.SubItems[0].Text);
                        ConvertTaskManager.getInstance().jobTable.Remove(item.SubItems[0].Text);
                    }
                }
            }
        }

        private void cbIsZeroIntensityIgnore_CheckedChanged(object sender, EventArgs e)
        {
            showFileSuffix();
        }

        public void showFileSuffix()
        {
            string suffix = "";

            if (!cbIsZeroIntensityIgnore.Checked)
            {
                suffix += "_with_zero";
            }

            tbFileNameSuffix.Text = suffix;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (tbHostAndPort.Text.IsNullOrEmpty())
            {
                timerConsumer.Enabled = false;
                MessageBox.Show("LIMS IP不能为空,监听器暂时关闭");
                return;
            }
            bool initResult = RedisClient.getInstance().connect(tbHostAndPort.Text);
            if (initResult)
            {
                lblConnectStatus.Text = "Connected";
                lblConnectStatus.ForeColor = Color.Green;
                timerConsumer.Enabled = true;
            }
            else
            {
                MessageBox.Show("连接失败,请检查LIMS IP字符串是否配置正确,监听器暂时关闭");
                lblConnectStatus.Text = "Not Connect";
                lblConnectStatus.ForeColor = Color.Red;
                timerConsumer.Enabled = false;
            }
        }

        //当Redis连接建立的时候持续的监听相关队列的消息
        private void consumer_Tick(object sender, EventArgs e)
        {
           RedisClient.getInstance().consume();
        }

        private void btnClearError_Click(object sender, EventArgs e)
        {
            ConvertTaskManager.getInstance().errorJob.Clear();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            timerConsumer.Enabled = false;
            RedisClient.getInstance().disconnect();
            lblConnectStatus.Text = "Not Connect";
            lblConnectStatus.ForeColor = Color.Red;
        }

        private void btnCustomerPath_Click(object sender, EventArgs e)
        {
            if (customPathForm == null || customPathForm.IsDisposed)
            {
                customPathForm = new CustomPathForm(this);
            }
            customPathForm.clearInfos();
            customPathForm.Show();
        }

        private void cbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox) sender).SelectedIndex == 1)
            {
                lblStackLayers.Visible = true;
                cbStackLayers.Visible = true;
            }
            else
            {
                lblStackLayers.Visible = false;
                cbStackLayers.Visible = false;
            }
        }

        private void lblFileSelectedInfo_Click(object sender, EventArgs e)
        {
            StackZDPDTest.stackZDPD_Test1();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (Folder == null || Folder.IsDisposed)
            {
                Folder = new Folder(this);
            }
            //Form.clearInfos();
            Folder.Show();
        }
    }
}
