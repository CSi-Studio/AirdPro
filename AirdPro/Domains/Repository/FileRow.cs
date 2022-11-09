using System;
using System.Threading;

namespace AirdPro.Repository.ProteomeXchange
{
    public class FileRow
    {
        public int retryTimes = 3;

        public string prefix = "";

        public CancellationTokenSource tokenSource = new CancellationTokenSource();
        public string remotePath { get; set; }
        public long fileSize { get; set; }
        public IProgress<string> fileSizeLabel { get; set; }
        public string fileName { get; set; }
        public string localPath { get; set; }
        public IProgress<string> status { get; set; }

        public FileRow()
        {
            this.fileSizeLabel = new Progress<string>();
            this.status = new Progress<string>();
        }
    }
}