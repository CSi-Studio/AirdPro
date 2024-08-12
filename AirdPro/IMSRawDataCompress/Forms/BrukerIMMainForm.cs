using AirdPro.IMSRawDataCompress.datamodel.sql;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AirdPro.Forms
{
    public partial class BrukerIMMainForm : Form
    {
        private String description;
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
            string fileName = LbFileNames.SelectedItem.ToString();
            ImportData(fileName);
        }

        private void BtnConvertToAird_Click(object sender, EventArgs e)
        {

        }

        private void ImportData(String fileName)
        {
            // 检查文件夹是否存在
            if (!Directory.Exists(fileName))
            {
                throw new DirectoryNotFoundException($"The directory '{fileName}' does not exist.");
            }

            // 定义文件过滤器
            string tdfFilter = Path.Combine(fileName, "*.tdf");
            string tdfBinFilter = Path.Combine(fileName, "*.tdf_bin");

            // 获取匹配的文件
            FileInfo tdfFile = Directory.EnumerateFiles(fileName, "*.tdf").Select(f => new FileInfo(f)).FirstOrDefault();
            FileInfo tdfBinFile = Directory.EnumerateFiles(fileName, "*.tdf_bin").Select(f => new FileInfo(f)).FirstOrDefault();

            // 检查是否找到了文件
            if (tdfFile == null || tdfBinFile == null)
            {
                throw new FileNotFoundException("Could not find both .tdf and .tdf_bin files in the specified directory.");
            }

            // 读取元数据
            ReadMetadata(tdfFile);

            // 读取帧数据
            ReadFrameData(tdfBinFile);
        }

        private void ReadMetadata(FileInfo tdfFile)
        {
            SetDescription("Initializing SQL...");
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={tdfFile.FullName};Version=3;"))
                {
                    SetDescription($"Establishing SQL connection to {tdfFile.Name}");

                    lock (typeof(SQLiteConnection))
                    {
                        try
                        {
                            connection.Open();

                            SetDescription($"Reading metadata for {tdfFile.Name}");
                            TDFMetaDataTable metaDataTable = new TDFMetaDataTable();
                            metaDataTable.ExecuteQuery(connection);

                            //测试代码：用于在界面上显示已经从表中读取到的数据
                            int colCount = metaDataTable.getColumns().Count;
                            int rowCount = metaDataTable.getColumns()[0].Count;
                            LbFileNames.Items.Add("table name: " + metaDataTable.getTable() + ", record count:" + rowCount + ", entry header: " + metaDataTable.getEntryHeader());
                            String colNames = "rownum";
                            for (int j = 0; j < colCount; j++)
                            {
                                colNames += "\t" + metaDataTable.getColumns()[j].GetColumnName();
                            }
                            LbFileNames.Items.Add(colNames);
                            String oneRow = "";
                            for (int i = 0; i < rowCount; i++)
                            {
                                oneRow = (i + 1) + "";
                                for (int j = 0; j < colCount; j++)
                                {
                                    oneRow += "\t" + metaDataTable.getColumns()[j][i];
                                }
                                LbFileNames.Items.Add(oneRow);
                            }

                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReadFrameData(FileInfo tdfBinFile)
        {
            SetDescription("Initializing SQL...");
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={tdfBinFile.FullName}"))
                {
                    SetDescription($"Establishing SQL connection to {tdfBinFile.Name}");

                    lock (typeof(SQLiteConnection))
                    {
                        try
                        {
                            connection.Open();

                            SetDescription($"Reading metadata for {tdfBinFile.Name}");


                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SetDescription(String desc)
        {
            description = desc;
        }

  
    }
}
