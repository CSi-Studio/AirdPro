/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

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
using AirdPro.Repository;
using AirdPro.Constants;
using AirdSDK.Utils;
using AutoUpdaterDotNET;
using pwiz.CLI.msdata;
using Spectrum = AirdSDK.Beans.Common.Spectrum;

namespace AirdPro.Forms
{
    public partial class MainForm : Form
    {
       

        private ReposWizardForm reposWizardForm;
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
            // RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            // key.SetValue("AirdPro", Application.ExecutablePath);
            this.Text = "AirdPro " + SoftwareInfo.getVersion() + Const.Dash + NetworkUtil.getHostIP();
            updateFileTree();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.aboutForm == null || Program.aboutForm.IsDisposed)
            {
                Program.aboutForm = new AboutForm();
            }

            Program.aboutForm.Show();
        }

        private void globalSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.redisForm == null || Program.redisForm.IsDisposed)
            {
                Program.redisForm = new RedisForm();
                Program.redisForm.Show();
            }

            Program.redisForm.Visible = true;

        }

        private void openRepositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                Settings.Default.Repository = fbd.SelectedPath;
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
            if (!String.IsNullOrEmpty(Settings.Default.Repository))
            {
                addDirectory(null, Settings.Default.Repository, 5);
            }

            if (fileTree.Nodes.Count > 0)
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
                if (airdInfo == null)
                {
                    MessageBox.Show("AirdInfo解码错误,请检查Aird文件是否异常");
                    return;
                }

                MRMParser mrmParser = new MRMParser(indexFilePath);
                List<MrmPair> srms = mrmParser.getAllSrmPairs();
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
                        SpectrumRow row = parseFromIndexAndAirdInfo(airdInfo, index, k);
                        row.ParentScan = null;
                        row.MSn = 1;
                        rowList.Add(row);
                    }
                }
                else if (index.level.Equals(2))
                {
                    for (int k = 0; k < index.nums.Count; k++)
                    {
                        SpectrumRow row = parseFromIndexAndAirdInfo(airdInfo, index, k);
                        row.ParentScan = index.num;
                        row.MSn = 2;
                        row.Precursor = index.getWindowRange().mz+"("+index.getWindowRange().start + "-" + index.getWindowRange().end+")";
                        rowList.Add(row);
                    }
                }
            }

            rowList = rowList.OrderBy(o => o.Scan).ToList();
            return rowList;
        }

        private SpectrumRow parseFromIndexAndAirdInfo(AirdInfo airdInfo, BlockIndex index, int k)
        {
            SpectrumRow row = new SpectrumRow();
            row.Polarity = (index.polarities == null || index.polarities.Count == 0)
                ? airdInfo.polarity
                : index.polarities[k];
            row.Energy = (index.energies == null || index.energies.Count == 0) ? airdInfo.energy : index.energies[k];
            row.Activator = (index.activators == null || index.activators.Count == 0)
                ? airdInfo.activator
                : index.activators[k];
            row.Scan = index.nums[k] + 1;
            row.ScanType = (index.msTypes == null || index.msTypes.Count == 0) ? airdInfo.msType : index.msTypes[k];
            row.RT = index.rts[k];
            row.BasePeakIntensity = index.basePeakIntensities[k];
            row.BasePeakMz = index.basePeakMzs[k];
            row.TotalIons = index.tics[k];
            row.FilterString = (index.filterStrings == null || index.filterStrings.Count == 0)
                ? airdInfo.filterString
                : index.filterStrings[k];
            return row;
        }

        private void spectraDataGrids_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SpectrumRow row = spectraDataGrids.SelectedRows[0].DataBoundItem as SpectrumRow;
            BaseParser parser = null;
            switch (airdInfo.type)
            {
                case AcquisitionMethod.DDA:
                    parser = new DDAParser(indexFile.FullName, airdInfo);
                    break;
                case AcquisitionMethod.DIA:
                    parser = new DIAParser(indexFile.FullName, airdInfo);
                    break;
                case AcquisitionMethod.PRM:
                    parser = new PRMParser(indexFile.FullName, airdInfo);
                    break;
                case AcquisitionMethod.DDA_PASEF:
                    parser = new DDAParser(indexFile.FullName, airdInfo);
                    break;
                case AcquisitionMethod.DIA_PASEF:
                    parser = new DIAParser(indexFile.FullName, airdInfo);
                    break;
                case AcquisitionMethod.MRM:
                    parser = new MRMParser(indexFile.FullName, airdInfo);
                    break;
            }

            Spectrum spectrum = parser.getSpectrumByNum(row.Scan - 1);

            IList<double> mzArray = spectrum.mzs;
            IList<double> intensityArray = spectrum.ints;

            //PASEF格式在展示上做一个合并,将相同mz的项合并为同一项
            if (airdInfo.mobiInfo != null && airdInfo.mobiInfo.type != null)
            {
                var uniqueMz = new List<double>(mzArray.Count);
                var summedIntensity = new List<double>(mzArray.Count);
                uniqueMz.Add(mzArray[0]);
                summedIntensity.Add(intensityArray[0]);
                for (int i = 1; i < mzArray.Count; ++i)
                {
                    if (mzArray[i] == uniqueMz[uniqueMz.Count - 1])
                        summedIntensity[uniqueMz.Count - 1] += intensityArray[i];
                    else
                    {
                        uniqueMz.Add(mzArray[i]);
                        summedIntensity.Add(intensityArray[i]);
                    }
                }

                mzArray = uniqueMz;
                intensityArray = summedIntensity;
            }

            spectrumChart.Series[0].Points.Clear();
            for (var i = 0; i < mzArray.Count; i++)
            {
                spectrumChart.Series[0].Points.AddXY(Math.Round(mzArray[i], 3), Math.Round(intensityArray[i], 1));
            }
        }

        private void repositoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reposWizardForm == null || reposWizardForm.IsDisposed)
            {
                reposWizardForm = new ReposWizardForm();
            }

            reposWizardForm.Show();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoUpdater.Start("https://airdpro.oss-cn-hangzhou.aliyuncs.com/Version.xml");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if (e.CloseReason == CloseReason.UserClosing)
            // {
            //     e.Cancel = true;
            //     this.ShowInTaskbar = false;
            //     this.Hide();
            // }

            e.Cancel = true;
            this.Visible = false;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                notifyMenu.Show();
            }

            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void startConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.conversionForm.Visible = true;
        }

        private void conversionSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.configListForm == null || Program.configListForm.IsDisposed)
            {
                Program.configListForm = new ConversionConfigListForm();
            }

            Program.configListForm.Show();
        }
    }
}