using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Windows.Forms;
using AirdPro.Async;
using AirdPro.Repository.ProteomeXchange;

namespace AirdPro.Repository
{
    public partial class DownloadDetailForm : Form
    {
        public string identifier;
        public string localDirectory;
        public string remoteUrl;

        private List<FileRow> fileList = new List<FileRow>();
        private DownloadTaskManager manager;
        public DownloadDetailForm(string identifier, string homeUrl, string remoteUrl, string localDirectory, List<string> remotes, List<string> locals)
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            manager = new DownloadTaskManager(this);
            this.identifier = identifier;
            this.localDirectory = localDirectory;
            this.remoteUrl = remoteUrl;
            tbRemote.Text = remoteUrl;
            tbHome.Text = homeUrl;
            init(remotes, locals);
        }

        public void init(List<string> remotes, List<string> locals)
        {
            Text = "Repository:" + identifier;
            for (var i = 0; i < remotes.Count; i++)
            {
                FileRow row = buildItem(remotes[i], locals[i], String.Empty);
                manager.pushJob(row);
            }
        }

        public FileRow buildItem(string remotePath, string localPath, string prefix)
        {
            string fileName = Path.GetFileName(remotePath);
            FileRow row = new FileRow();
            ListViewItem item = buildListViewItem(row, fileName, remotePath, localPath, prefix);

            fileList.Add(row);
            lvFileList.Items.Add(item);
            
            return row;
        }

        private ListViewItem buildListViewItem(FileRow row, string fileName, string remotePath, string localPath, string prefix)
        {
            row.remotePath = Path.Combine(remotePath);
            row.fileName = Path.GetFileName(fileName);
            row.localPath = Path.Combine(localPath);
            row.prefix = prefix;
            string[] itemInfo = new string[]
            {
                row.remotePath,
                row.fileName,
                row.localPath,
                "Wait",
                "Fetching"
            };
            ListViewItem item = new ListViewItem(itemInfo);
            Progress<string> status = new Progress<string>((progressValue) =>
            {
                if (item!= null)
                {
                    item.SubItems[3].Text = progressValue;
                }
            });
            Progress<string> fileSizeLabel = new Progress<string>((label) =>
            {
                if (item != null)
                {
                    item.SubItems[4].Text = label;
                }
            });
            row.status = status;
            row.fileSizeLabel = fileSizeLabel;

            return item;
        }

        private void btnAsync_Click(object sender, EventArgs e)
        {
            manager.run();
        }

        private void DetailForm_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void DetailForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // manager.close();
        }
    }
}
