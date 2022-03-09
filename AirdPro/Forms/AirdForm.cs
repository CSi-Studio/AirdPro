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
using AirdPro.Domains.Convert;
using AirdPro.Domains.Job;
using AirdPro.Redis;
using AirdPro.Utils;
using System;
using System.Collections;
using System.Windows.Forms;

namespace AirdPro.Forms
{
    public partial class AirdForm : Form
    {
        ArrayList currentFiles = new ArrayList();
        VendorFileSelectorForm fileSelector;
        ConversionConfigListForm conversionConfigListForm;
        private GlobalSettingForm globalSettingForm;
        public string lastOpenPath;

        public AirdForm()
        {
            InitializeComponent();
        }

        public void applyNowConfig(ConversionConfig conversionConfig)
        {
            tbFolderPath.Text = conversionConfig.outputPath;
            cbIsZeroIntensityIgnore.Checked = conversionConfig.ignoreZeroIntensity;
            cbThreadAccelerate.Checked = conversionConfig.threadAccelerate;
            cbMzPrecision.Text = conversionConfig.mzPrecision.ToString();
            mzIntComp.Text = conversionConfig.mzIntComp.ToString();
            mzByteComp.Text = conversionConfig.mzByteComp.ToString();
            intByteComp.Text = conversionConfig.intByteComp.ToString();
            if (conversionConfig.stack)
            {
                cbStack.Checked = true;
                cbStackLayers.Text = Math.Pow(2, conversionConfig.digit).ToString();
            }
            else
            {
                cbStack.Checked = false;
            }

            tbFileNameSuffix.Text = conversionConfig.suffix;
            tbOperator.Text = conversionConfig.creator;
        }
       
        private void ProproForm_Load(object sender, EventArgs e)
        {
            this.Text = SoftwareInfo.getVersion() + " - " + NetworkUtil.getHostIP();
            this.cbMzPrecision.SelectedIndex = 1; //默认选择精确到小数点后5位的精度
           
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
                    ConversionConfig conversionConfig = new  ConversionConfig
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
                        outputPath = tbFolderPath.Text
                    };
                    item.Tag = conversionConfig;
                    JobInfo jobInfo = new JobInfo(item.SubItems[0].Text, 
                        item.SubItems[1].Text, conversionConfig, item);

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
            if (fileName != "" )
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

        private void btnClearError_Click(object sender, EventArgs e)
        {
            ConvertTaskManager.getInstance().errorJob.Clear();
        }

        private void btnCustomerPath_Click(object sender, EventArgs e)
        {
            
        }

        //查看列表选中对象的详细参数
        private void lvFileList_DoubleClick(object sender, EventArgs e)
        {
            int index = lvFileList.FocusedItem.Index; //获取选中Item的索引值
            ConversionConfig conversionConfig = new ConversionConfig();
            conversionConfig = (ConversionConfig)lvFileList.Items[index].Tag;
            ConversionConfigListForm conversionConfigListForm =
                new ConversionConfigListForm(this);
            if (lvFileList.SelectedItems.Count == 0 ) //判断选中的不为0
            {
                return;
            }
            else
            {
                conversionConfigListForm.tbConfigFolderPath.Text = conversionConfig.outputPath;
                conversionConfigListForm.tbConfigFileNameSuffix.Text = conversionConfig.suffix;
                conversionConfigListForm.tbConfigOperator.Text = conversionConfig.creator;
                conversionConfigListForm.cbConfigIsZeroIntensityIgnore.Checked = conversionConfig.ignoreZeroIntensity;
                conversionConfigListForm.cbConfigThreadAccelerate.Checked = conversionConfig.threadAccelerate;
                conversionConfigListForm.cbConfigMzPrecision.Text = Math.Log10(conversionConfig.mzPrecision).ToString();
                conversionConfigListForm.configMzIntComp.Text = conversionConfig.mzIntComp.ToString();
                conversionConfigListForm.configMzByteComp.Text = conversionConfig.mzByteComp.ToString();
                conversionConfigListForm.configIntByteComp.Text = conversionConfig.intByteComp.ToString();
                if (conversionConfig.stack)
                {
                    conversionConfigListForm.cbConfigStack.Checked = true;
                    conversionConfigListForm.cbConfigStackLayers.Text = Math.Pow(2, conversionConfig.digit).ToString();
                }
                else
                {
                    conversionConfigListForm.cbConfigStack.Checked = false;
                }
                conversionConfigListForm.Show();
            }
        }

        //打开输入的定制化参数列表
        private void openConversionConfigListForm(object sender, EventArgs e)
        {
            if (conversionConfigListForm == null || conversionConfigListForm.IsDisposed)
            {
                conversionConfigListForm = new ConversionConfigListForm(this);
                Program.conversionConfigHandler.attach(conversionConfigListForm);
            }
            conversionConfigListForm.Show();
           
        }

        private void selectFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileSelector == null || fileSelector.IsDisposed)
            {
                fileSelector = new VendorFileSelectorForm(this);
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
    }
}
