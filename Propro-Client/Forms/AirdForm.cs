using AirdPro.Asyncs;
using AirdPro.Constants;
using System;
using System.Collections;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using Alea;

namespace AirdPro.Forms
{
    public partial class AirdForm : Form
    {
        ArrayList currentFiles = new ArrayList();
        ConvertTaskManager convertTaskManager = new ConvertTaskManager();
      
        public AirdForm()
        {
            InitializeComponent();
        }

        private void ProproForm_Load(object sender, EventArgs e)
        {
            this.Text = SoftwareVersion.getVersion();
            this.cbMzPrecision.SelectedIndex = 1;
        }

        private void btnChooseFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "Vendor Format|*.wiff;*.raw";
            ofd.Title = "Please Choose Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, ExperimentType.DIA_SWATH);
                }
            }
        }

        private void btnChoosePRMFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "Vendor Format|*.wiff;*.raw";
            ofd.Title = "Please Choose Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, ExperimentType.PRM);
                }
            }
        }

        private void BtnChooseSSwathFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "Vendor Format|*.wiff;";
            ofd.Title = "Please Choose Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, ExperimentType.SCANNING_SWATH);
                }
            }
        }

        private void btnChooseDDAFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;
            ofd.Filter = "Vendor Format|*.wiff;*.raw";
            ofd.Title = "Please Choose Vendor File to Convert";

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var fileName in ofd.FileNames)
                {
                    addFile(fileName, ExperimentType.DDA);
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
                JobParams jobParams = new JobParams();
                jobParams.ignoreZeroIntensity = cbIsZeroIntensityIgnore.Checked;
                jobParams.log2 = cbLog2.Checked;
                jobParams.threadAccelerate = cbThreadAccelerate.Checked;
                jobParams.suffix = tbFileNameSuffix.Text;
                jobParams.creator = tbOperator.Text;
                jobParams.mzPrecision = Double.Parse(cbMzPrecision.Text);
                JobInfo jobInfo = new JobInfo(item.SubItems[0].Text, tbFolderPath.Text,
                    item.SubItems[1].Text, jobParams, item);
                convertTaskManager.pushJob(jobInfo);
            }

            try
            {
                convertTaskManager.run();
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

        private void addFile(string fileName,string expType)
        {
            if (fileName != "" && !currentFiles.Contains(fileName))
            {
                ListViewItem item = new ListViewItem(new string[]{fileName, expType, "Waiting"});
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
            convertTaskManager.jobTable.Remove(fileItem.Text);
        }

        private void lvFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            printLog();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            printLog();
        }

        private void printLog()
        {
            if (lvFileList.SelectedItems.Count != 0)
            {
                ListViewItem item = lvFileList.SelectedItems[lvFileList.SelectedItems.Count - 1];
                string content = "";
                JobInfo job = null;
                if (convertTaskManager.jobTable[item.Text] != null)
                {
                    job = convertTaskManager.jobTable[item.Text] as JobInfo;
                    
                    for (int i=job.logs.Count-1;i>=0;i--)
                    {
                        content += job.logs[i].dateTime + "   " + job.logs[i].content + "\r\n";
                    }
                    string jobInfo = job.getJsonInfo();
                    content += jobInfo + "\r\n";
                }
                else if (convertTaskManager.errorJob[item.Text] != null)
                {
                    job = convertTaskManager.errorJob[item.Text] as JobInfo;

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
                        convertTaskManager.jobTable.Remove(item.SubItems[0].Text);
                    }
                }
            }
        }

        private void cbIsZeroIntensityIgnore_CheckedChanged(object sender, EventArgs e)
        {
            showFileSuffix();
        }

        private void cbMzPrecision_SelectedIndexChanged(object sender, EventArgs e)
        {
            showFileSuffix();
        }

        private void cbIntensityPrecision_SelectedIndexChanged(object sender, EventArgs e)
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

        private void btnCheckGPU_Click(object sender, EventArgs e)
        {
            var devices = Device.Devices;
            string message = "";
            foreach (var device in devices)
            {
                message += "Name:" + device.Name+ "\r\n";
                message += "Cores:" + device.Cores + "\r\n";
                message += "Memory:" + device.TotalMemory/1024/1024/1024+ "GB\r\n";
            } 

            MessageBox.Show(message);

        }
    }
}
