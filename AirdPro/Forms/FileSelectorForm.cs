using System;
using System.IO;
using System.Windows.Forms;

namespace AirdPro.Forms
{
    public partial class FileSelectorForm : Form
    {
        public string path;
        public bool multiSelect;
        public FileSelectorForm(string path, bool multiSelect)
        {
            InitializeComponent();
            this.path = path;
            this.multiSelect = multiSelect;
        }

        private void FileSelectorForm_Load(object sender, EventArgs e)
        {
            string[] drives = Environment.GetLogicalDrives();
            for (var i = 0; i < drives.Length; i++)
            {
                TreeNode node = new TreeNode();
                node.Text = drives[i];
                node.ImageIndex = 0;
                tvFiles.Nodes.Add(node);
            }
        }
    }
}
