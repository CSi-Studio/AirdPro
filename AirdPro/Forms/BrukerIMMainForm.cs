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

        private void btnAllBrukerFiles_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new ();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolderPath = folderBrowserDialog.SelectedPath;
                lbFileNames.Items.Clear(); // 清空ListBox
                                           // 选中的文件就是 .d 文件夹
                if (selectedFolderPath.EndsWith(".d"))
                {
                    lbFileNames.Items.Add(selectedFolderPath);
                }
                // 继续查找当前文件夹下的所有 .d 后缀的子文件夹
                string[] dFiles = Directory.GetDirectories(selectedFolderPath, "*.d", SearchOption.AllDirectories);
                if (dFiles.Length != 0)
                {
                    foreach (string filePath in dFiles)
                    {
                        lbFileNames.Items.Add(filePath); // 将文件夹名添加到ListBox中
                    }
                }
                if (lbFileNames.Items.Count == 0)
                {
                    MessageBox.Show("No Bruker vendor file exists in the current folder.", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                lbFileNames.SelectedIndex = 0; //默认选第一个
            }
        }        

        private void btnClear_Click(object sender, EventArgs e)
        {
            lbFileNames.Items.Clear();
        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            string fileName = lbFileNames.SelectedItem.ToString();
            importData(fileName);
        }

        private void btnDataView_Click(object sender, EventArgs e)
        {
            if (lbFileNames.Items.Count == 0)
            {
                MessageBox.Show("Please select a MS file that you want to view first.", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            new DataOverviewForm().ShowDialog();
        }

        private void btnIMDataView_Click(object sender, EventArgs e)
        {
            if (lbFileNames.Items.Count == 0)
            {
                MessageBox.Show("Please select a Bruker ion moblity MS file that you want to view first.", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            new IonMoblityDataOverviewForm().ShowDialog();
        }


        private void btnConvertToAird_Click(object sender, EventArgs e)
        {
            if (lbFileNames.Items.Count == 0)
            {
                MessageBox.Show("Please select a MS file you want to convert first.", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }
  
        private void lbFileNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            String fileName = lbFileNames.SelectedItem.ToString();
        }  

        private void importData(String fileName)
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
            readMetadata(tdfFile);

            // 读取帧数据
            readFrameData(tdfBinFile);
        }

        private void readFrameData(FileInfo tdfBinFile)
        {
            setDescription("Initializing SQL...");
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={tdfBinFile.FullName}"))
                {
                    setDescription($"Establishing SQL connection to {tdfBinFile.Name}");

                    lock (typeof(SQLiteConnection))
                    {
                        try
                        {
                            connection.Open();

                            setDescription($"Reading metadata for {tdfBinFile.Name}");


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

        private void readMetadata(FileInfo tdfFile)
        {
            setDescription("Initializing SQL...");
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={tdfFile.FullName};Version=3;"))
                {
                    setDescription($"Establishing SQL connection to {tdfFile.Name}");

                    lock (typeof(SQLiteConnection))
                    {
                        try
                        {
                            connection.Open();

                            setDescription($"Reading metadata for {tdfFile.Name}");
                            TDFMetaDataTable metaDataTable = new TDFMetaDataTable();
                            metaDataTable.ExecuteQuery(connection);

                            //测试代码：用于在界面上显示已经从表中读取到的数据
                            int colCount = metaDataTable.getColumns().Count;
                            int rowCount = metaDataTable.getColumns()[0].Count;
                            lbFileNames.Items.Add("table name: " + metaDataTable.getTable() + ", record count:" + rowCount + ", entry header: " + metaDataTable.getEntryHeader());
                            String colNames = "rownum";
                            for (int j = 0; j < colCount; j++)
                            {
                                colNames += "\t" + metaDataTable.getColumns()[j].GetColumnName();
                            }
                            lbFileNames.Items.Add(colNames);
                            String oneRow = "";
                            for (int i = 0; i < rowCount; i++)
                            {
                                oneRow = (i+1) + "";
                                for (int j = 0; j < colCount; j++)
                                {
                                    oneRow += "\t" + metaDataTable.getColumns()[j][i];
                                }
                                lbFileNames.Items.Add(oneRow);
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

        private void setDescription(String desc)
        {
            description = desc;
        }

    }
}
