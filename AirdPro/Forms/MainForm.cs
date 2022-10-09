using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AirdPro.Controls;
using AirdPro.Properties;
using AirdSDK.Compressor;
using AirdSDK.Constants;

namespace AirdPro.Forms
{
    public partial class MainForm : Form
    {
        private AboutForm aboutForm = new AboutForm();
        private GlobalSettingForm globalSettingForm;
        private HashSet<string> airdFiles = new HashSet<string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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
                else if (node.Tag.ToString().EndsWith(SuffixConst.AIRD))
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
                FileInfo airdFile = new FileInfo(path);
                TabPage tabPage = new TabPage(airdFile.Name);
                tabPage.Tag = path;
                AirdPanel airdPanel = new AirdPanel();
                airdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                              | System.Windows.Forms.AnchorStyles.Left)
                                                                             | System.Windows.Forms.AnchorStyles.Right)));
                tabPage.Controls.Add(airdPanel);
                tabs.Controls.Add(tabPage);
            }
        }

        private void menuClose_Click(object sender, EventArgs e)
        {
            TabPage page = tabs.SelectedTab;
            airdFiles.Remove(page.Tag.ToString());
            tabs.TabPages.RemoveAt(tabs.SelectedIndex);
        }
    }
}