using AirdPro.Properties;
using AirdSDK.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AirdPro.Domains.View;
using AirdSDK.Beans;
using AirdSDK.Beans.Common;
using AirdSDK.Enums;
using AirdSDK.Parser;
using CV = AirdSDK.Beans.CV;
using AirdPro.Repository;
using AirdPro.Constants;
using AirdPro.Utils;

namespace AirdPro.Forms
{
    public partial class MainForm : Form
    {
        private AboutForm aboutForm = new AboutForm();
        private GlobalSettingForm globalSettingForm;
        private RepositoryGuiderForm repositoryGuiderForm;
        private HashSet<string> airdFiles = new HashSet<string>();
        private FileInfo airdFile;
        private FileInfo indexFile;
        private AirdInfo airdInfo;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text ="AirdPro " + SoftwareInfo.getVersion() + Const.Dash + NetworkUtil.getHostIP();
            updateFileTree();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutForm == null || aboutForm.IsDisposed)
            {
                aboutForm = new AboutForm();
            }

            aboutForm.Show();
        }

        private void globalSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalSettingForm == null || globalSettingForm.IsDisposed)
            {
                globalSettingForm = new GlobalSettingForm();
            }

            globalSettingForm.Show();
        }

        private void startConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.conversionForm.Visible = true;
        }

        private void openRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Settings.Default.repository = fbd.SelectedPath;
                Settings.Default.Save();
                fileTree.Nodes.Clear();
                addDirectory(null, fbd.SelectedPath, 5);
                fileTree.TopNode.Expand();
            }
        }

        //单独增加某一个仓库路径，maxDeep为最大遍历文件夹深度，最深为5层
        private void addDirectory(TreeNode parent, string directoryPath, int maxDeep)
        {
            if (Directory.Exists(directoryPath))
            {
                
                TreeNode node = new TreeNode();
                DirectoryInfo directory = new DirectoryInfo(directoryPath);

                node.Text = directory.Name;
                node.Tag = directory.FullName;
                if (parent == null)
                {
                    node.Text = directory.FullName;
                    fileTree.Nodes.Add(node);
                    node.Expand();
                }
                else
                {
                    parent.Nodes.Add(node);
                }
                
                addFiles(node, directory.GetFiles());

                maxDeep--;
                if (maxDeep < 0)
                {
                    return;
                }
                DirectoryInfo[] directories = directory.GetDirectories();
                if (directories.Length > 0)
                {
                    foreach (DirectoryInfo directoryInfo in directories)
                    {
                        addDirectory(node, directoryInfo.FullName, maxDeep);
                    }
                }
            }
        }

        private void addFiles(TreeNode parent, FileInfo[] files)
        {
            List<TreeNode> nodeList = new List<TreeNode>();
            foreach (FileInfo file in files)
            {
                string extension = Path.GetExtension(file.FullName);
                if (extension.Equals(SuffixConst.AIRD))
                {
                    TreeNode node = new TreeNode();
                    node.Text = file.Name;
                    node.Tag = file.FullName;
                    nodeList.Add(node);
                }
                
            }

            parent.Nodes.AddRange(nodeList.ToArray());
        }

        //完整更新整个FileTree
        private void updateFileTree() 
        {
            if (!String.IsNullOrEmpty(Settings.Default.repository))
            {
                addDirectory(null, Settings.Default.repository, 5);
            }
            if(fileTree.Nodes.Count > 0)
            {
                fileTree.TopNode.Expand();
            }
        }

        private void fileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //鼠标左击
            if (e.Button == MouseButtons.Left)
            {
                TreeNode node = e.Node;
                if (node.Nodes.Count > 0)
                {
                    if (node.IsExpanded)
                    {
                        node.Collapse();
                    }
                    else
                    {
                        node.Expand();
                    }
                }
            }
            
        }

        private void fileTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //鼠标左击
            if (e.Button == MouseButtons.Left)
            {
                TreeNode node = e.Node;
                if (node.Nodes.Count == 0 && node.Tag.ToString().EndsWith(SuffixConst.AIRD))
                {
                    renderAird(node.Tag.ToString());
                }
            }
        }

        //刷新列表
        private void itemRefresh_Click(object sender, EventArgs e)
        {
            fileTree.Nodes.Clear();
            updateFileTree();
        }

        private void renderAird(string path)
        {
            if (!airdFiles.Contains(path))
            {
                airdFile = new FileInfo(path);
                if (!airdFile.Exists)
                {
                    MessageBox.Show("This File is not exist!");
                    return;
                }
                string indexFilePath = AirdScanUtil.getIndexPathByAirdPath(path);
                indexFile = new FileInfo(indexFilePath);
                airdInfo = AirdScanUtil.loadAirdInfo(indexFile);
                List<SpectrumRow> spectra = parseAsSpectra(airdInfo);
                spectraDataGrids.DataSource = spectra;

                for (var i = 0; i < spectra.Count; i++)
                {
                    ticChart.Series[0].Points.AddXY(Math.Round(spectra[i].RT, 2), spectra[i].TotalIons);
                    basePeakChart.Series[0].Points.AddXY(Math.Round(spectra[i].BasePeakMz, 5),
                        Math.Round(spectra[i].BasePeakIntensity, 0));
                }

                ticChart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0e+0}";
                basePeakChart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0e+0}";
                spectrumChart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0e+0}";
                lblAirdInfo.Text = path;
            }
        }

        private List<SpectrumRow> parseAsSpectra(AirdInfo airdInfo)
        {
            List<SpectrumRow> rowList = new List<SpectrumRow>();
            List<BlockIndex> indexList = airdInfo.indexList;
            for (var i = 0; i < indexList.Count; i++)
            {
                BlockIndex index = indexList[i];
                if (index.level.Equals(1))
                {
                    for (var k = 0; k < index.nums.Count; k++)
                    {
                        SpectrumRow row = new SpectrumRow();
                        row.Polarity = airdInfo.polarity;
                        row.Energy = airdInfo.energy;
                        row.Activator = airdInfo.activator;
                        row.Scan = index.nums[k] + 1;
                        row.ScanType = airdInfo.msType;
                        row.ParentScan = null;
                        row.MSn = 1;
                        row.RT = index.rts[k];
                        row.BasePeakIntensity = index.basePeakIntensities[k];
                        row.BasePeakMz = index.basePeakMzs[k];
                        row.TotalIons = index.tics[k];
                        row.FilterString = getFilterString(index.cvList[k]);
                        rowList.Add(row);
                    }
                }
                else if (index.level.Equals(2))
                {
                    for (int k = 0; k < index.nums.Count; k++)
                    {
                        SpectrumRow row = new SpectrumRow();
                        row.Polarity = airdInfo.polarity;
                        row.Energy = airdInfo.energy;
                        row.Activator = airdInfo.activator;
                        row.Scan = index.nums[k] + 1;
                        row.ScanType = airdInfo.msType;
                        row.ParentScan = index.num;
                        row.MSn = 2;
                        row.RT = index.rts[k];
                        row.Precursor = index.getWindowRange().start + "-" + index.getWindowRange().end;
                        row.BasePeakIntensity = index.basePeakIntensities[k];
                        row.BasePeakMz = index.basePeakMzs[k];
                        row.TotalIons = index.tics[k];
                        row.FilterString = getFilterString(index.cvList[k]);
                        rowList.Add(row);
                    }
                }
            }

            rowList = rowList.OrderBy(o => o.Scan).ToList();
            return rowList;
        }

        private string getFilterString(List<CV> cvList)
        {
            for (var i = 0; i < cvList.Count; i++)
            {
                if (cvList[i].cvid.Equals("1000512:filter string"))
                {
                    return cvList[i].value.ToString();
                }
            }

            return null;
        }

        private void spectraDataGrids_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SpectrumRow row = spectraDataGrids.SelectedRows[0].DataBoundItem as SpectrumRow;
            if (airdInfo.type.Equals(AirdType.DDA))
            {
                DDAParser parser = new DDAParser(indexFile.FullName, airdInfo);
                Spectrum spectrum = parser.getSpectrumByNum(row.Scan);
                spectrumChart.Series[0].Points.Clear();
                for (var i = 0; i < spectrum.mzs.Length; i++)
                {
                    spectrumChart.Series[0].Points.AddXY(Math.Round(spectrum.mzs[i], 3), Math.Round(spectrum.ints[i], 1));
                }
            }
        }

        private void repositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (repositoryGuiderForm == null || repositoryGuiderForm.IsDisposed)
            {
                repositoryGuiderForm = new RepositoryGuiderForm();
            }

            repositoryGuiderForm.Show();
        }
    }
}