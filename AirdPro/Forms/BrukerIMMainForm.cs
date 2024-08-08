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
                // 查找所有 .d 后缀的文件夹
                string[] dFiles = Directory.GetDirectories(selectedFolderPath, "*.d", SearchOption.AllDirectories);
                if (dFiles.Length == 0)
                {
                    MessageBox.Show("No Bruker vendor files were found in the selected folder.", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (string filePath in dFiles)
                {
                    lbFileNames.Items.Add(filePath); // 将文件夹名添加到ListBox中
                }
                lbFileNames.SelectedIndex = 0;
            }
        }        

        private void btnClear_Click(object sender, EventArgs e)
        {
            lbFileNames.Items.Clear();
            //关闭sqlite

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
            importData(fileName);
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
                using (var connection = new SQLiteConnection($"Data Source={tdfFile.FullName}"))
                {
                    setDescription($"Establishing SQL connection to {tdfFile.Name}");

                    lock (typeof(SQLiteConnection))
                    {
                        try
                        {
                            connection.Open();

                            setDescription($"Reading metadata for {tdfFile.Name}");
                            

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
