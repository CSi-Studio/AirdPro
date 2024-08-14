using AirdPro.IMSRawDataCompress.datamodel.sql;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace AirdPro.Forms
{
    public partial class BrukerIMMainForm : Form
    {
        private bool isImportRunning;
        TDFImportTask tdfImportTask;
        public BrukerIMMainForm()
        {
            InitializeComponent();
        }

        private void BtnSelectIMS_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new ();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolderPath = folderBrowserDialog.SelectedPath;
                LbFileNames.Items.Clear(); // 清空ListBox
                // 选中的文件就是 .d 文件夹
                if (selectedFolderPath.EndsWith(".d"))
                {
                    LbFileNames.Items.Add(selectedFolderPath);
                }
                // 继续查找当前文件夹下的所有 .d 后缀的子文件夹
                string[] dFiles = Directory.GetDirectories(selectedFolderPath, "*.d", SearchOption.AllDirectories);
                if (dFiles.Length != 0)
                {
                    foreach (string filePath in dFiles)
                    {
                        LbFileNames.Items.Add(filePath); // 将文件夹名添加到ListBox中
                    }
                }
                if (LbFileNames.Items.Count == 0)
                {
                    MessageBox.Show("No Bruker vendor file exists in the current folder.", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                LbFileNames.SelectedIndex = 0; //默认选第一个
            }
        }        

        private void BtnClear_Click(object sender, EventArgs e)
        {
            LbFileNames.Items.Clear();
        }
  
        private void LBFileNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            String fileName = LbFileNames.SelectedItem.ToString();
            LbPwizImport.Items.Clear();
            LbTdfImport.Items.Clear();
        }

        private void BtnPwizImport_Click(object sender, EventArgs e)
        {

        }

        private void BtnTDFImport_Click(object sender, EventArgs e)
        {
            if (LbFileNames.SelectedItem == null)
            {
                MessageBox.Show("Please select a Bruker vendor file.", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string fileName = LbFileNames.SelectedItem.ToString();
            ImportData(fileName);

        }

        private void BtnConvertToAird_Click(object sender, EventArgs e)
        {

        }

        private void ImportData(String fileName)
        {
            LbTdfImport.Items.Clear();
            tdfImportTask = new TDFImportTask();

            isImportRunning = true;
            Thread updateThread = new Thread(ShowDetail);
            updateThread.Start();           
            tdfImportTask.Run(fileName);
            isImportRunning = false;
        }

        void ShowDetail()
        {
            while (isImportRunning)
            {
                if (tdfImportTask.Messages != null)
                {
                    while (tdfImportTask.Messages.Count > 0)
                    {
                        if (LbTdfImport.InvokeRequired)
                        {
                            LbTdfImport.Invoke(new Action(() =>
                            {
                                // 更新ListBox
                                LbTdfImport.Items.Add(tdfImportTask.Messages[0]);
                            }));
                        }
                        else
                        {
                            // 更新ListBox
                            LbTdfImport.Items.Add(tdfImportTask.Messages[0]);
                        }
                        tdfImportTask.Messages.RemoveAt(0);
                    }
                    Thread.Sleep(10);
                }
            }
        }


    }
}
