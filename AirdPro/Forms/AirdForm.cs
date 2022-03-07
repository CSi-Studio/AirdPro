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
using ThermoFisher.CommonCore.Data;
using AirdPro.Utils;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using AirdPro.Forms.Config;

namespace AirdPro.Forms
{
    public partial class AirdForm : Form
    {
        ArrayList currentFiles = new ArrayList();
        FileFolderSelector customPathForm;
        ConfigCustomization configCustomization;
        ConfigListView configListView;
        public string hostIP;
        public AirdForm()
        {
            InitializeComponent();
            ConfigSubject configSubject = new ConfigParamsSubject();
            ConfigListView configListView = new ConfigListView();
            configSubject.addObserver(new ConfigNotifyEventHandler(configListView.Receive));
            configSubject.publishConfigInfo();
            
        }
       
        private void ProproForm_Load(object sender, EventArgs e)
        {
            creatADefaultConfig();
            this.Text = SoftwareInfo.getVersion() + " - " + getHostIP();
            this.cbMzPrecision.SelectedIndex = 1; //默认选择精确到小数点后4位的精度
           
            foreach (string intCompType in Enum.GetNames(typeof(IntCompType)))
            {
                this.mzIntComp.Items.Add(intCompType);
            }
            foreach (string byteCompType in Enum.GetNames(typeof(ByteCompType)))
            {
                this.mzByteComp.Items.Add(byteCompType);
                this.intByteComp.Items.Add(byteCompType);
            }

            this.mzIntComp.SelectedItem = IntCompType.IBP.ToString(); //mz数组默认选择IBP的压缩内核
            this.mzByteComp.SelectedItem = ByteCompType.Zlib.ToString(); //mz数组默认选择Zlib的压缩内核
            this.intByteComp.SelectedItem = ByteCompType.Zlib.ToString(); //intensity数组默认选择Zlib的压缩内核
            this.cbStackLayers.SelectedItem = "256"; //当使用Stack Layer堆叠的时候,默认堆叠256层
            this.tbFolderPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            this.tbOperator.Text = Environment.UserName;
            RedisClient.getInstance();
            ConvertTaskManager.getInstance().run();
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
                    JobParams jobParams = new  JobParams
                    {
                        ignoreZeroIntensity = cbIsZeroIntensityIgnore.Checked,
                        threadAccelerate = cbThreadAccelerate.Checked,
                        suffix = tbFileNameSuffix.Text,
                        creator = tbOperator.Text,
                        mzPrecision = (int)Math.Pow(10, int.Parse(cbMzPrecision.Text)),
                        stack = cbStack.Checked,  // 是否使用stack layer压缩算法
                        mzIntComp = (IntCompType)Enum.Parse(typeof(IntCompType),mzIntComp.SelectedItem.ToString()),
                        mzByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), mzByteComp.SelectedItem.ToString()),
                        intByteComp = (ByteCompType)Enum.Parse(typeof(ByteCompType), intByteComp.SelectedItem.ToString()),
                        digit = (int)Math.Log(int.Parse(cbStackLayers.SelectedItem.ToString()), 2),
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
        public void addFile(string fileName, string expType)
        {
            if (fileName != "" && !currentFiles.Contains(fileName))
            {
                ListViewItem item = new ListViewItem(new string[]{fileName, expType, "Waiting","","",""});
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
                lblFileSelectedInfo.Text = currentFiles.Count + " files are selected";
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
                    if (item.SubItems[2].Text.Equals("Finished"))
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
                customPathForm = new FileFolderSelector(this);
            }
            customPathForm.clearInfos();
            customPathForm.Show();
        }

        //查看列表选中对象的详细参数
        private void lvFileList_DoubleClick(object sender, EventArgs e)
        {
            int index = lvFileList.FocusedItem.Index; //获取选中Item的索引值
            JobParams jobParams = new JobParams();
            JobDetailForm details = new JobDetailForm();
            if(lvFileList.SelectedItems.Count == 0 ) //判断选中的不为0
            {
                return;
            }
            else
            {
                details.tbInputFilePath.Text = lvFileList.Items[index].SubItems[0].Text;
                details.tbFileName.Text = FileNameUtil.buildOutputFileName(details.tbInputFilePath.Text);
                details.tbFileType.Text = lvFileList.Items[index].SubItems[1].Text;
                details.tbOutputFilePath.Text = lvFileList.Items[index].SubItems[5].Text;
                if(cbIsZeroIntensityIgnore.Checked)
                {
                    details.tbZeroIntensity.Text = "Ignore";
                }
                else
                {
                    details.tbZeroIntensity.Text = "Not Ignore";
                }
                if(cbThreadAccelerate.Checked)
                {
                    details.tbMultithreading.Text = "True";
                }
                else
                {
                    details.tbMultithreading.Text = "False";
                }
                details.tbMzPrecision.Text = lvFileList.Items[index].SubItems[3].Text;
                details.tbMzIntCompressor.Text = Convert.ToString(jobParams.mzIntComp);
                details.tbMzByteCompressor.Text = Convert.ToString(jobParams.mzByteComp);
                details.tbIntensityByteCompressor.Text = Convert.ToString(jobParams.intByteComp);
                if (cbStack.Checked)
                {
                    details.tbStackStatus.Text = "Open";
                    details.tbStackLayers.Text = cbStackLayers.Text;
                }
                else
                {
                    details.tbStackStatus.Text = "Not Open";
                }
                
                details.tbFileSuffix.Text = tbFileNameSuffix.Text;
                details.tbOperator.Text = tbOperator.Text;
                details.Show();
            }
        }

        //程序启动后自动保存一份默认参数配置，且不可被覆盖更改
        public FileStream defaultConfigStream;
        public void creatADefaultConfig()
        {
            string str = Environment.CurrentDirectory;
            string defaultConfigPath = Path.Combine(str, "DefaultConfig.json");
            if (File.Exists(defaultConfigPath))
            {
               
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(defaultConfigPath));
                JobParams jobParams = new JobParams();
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                { NullValueHandling = NullValueHandling.Ignore };
                String defaultConfigStr = JsonConvert.SerializeObject(jobParams, jsonSetting);
                byte[] defaultConfigBytes = Encoding.Default.GetBytes(defaultConfigStr);
                using (defaultConfigStream = new FileStream(defaultConfigPath, FileMode.OpenOrCreate))
                {
                    defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
                }
            }  
        }

        //打开定制化参数窗口
        private void ConfigCustom(object sender, EventArgs e)
        {
            if(configCustomization == null)
            {
                configCustomization = new ConfigCustomization();
            }
            
            configCustomization.Show();
        }
        //获取本地IP的方法
        public string getHostIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    hostIP = Convert.ToString(ip);
                    return hostIP;
                }
            }
            return null;
        }

        //打开输入的定制化参数列表
        private void ConfigList(object sender, EventArgs e)
        {
            if (configListView == null || configListView.IsDisposed)
            {
                configListView = new ConfigListView();
            }
            configListView.Show();
        }
    }
}
