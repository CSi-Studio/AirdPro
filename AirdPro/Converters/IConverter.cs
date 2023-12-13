using AirdPro.Domains;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirdPro.Algorithms;
using AirdPro.Constants;
using AirdSDK.Compressor;

namespace AirdPro.Converters
{
    public abstract class IConverter
    {
        public JobInfo jobInfo;
        protected Stopwatch stopwatch = new Stopwatch();
        public FileStream airdStream;
        public FileStream airdJsonStream;
        
        protected long fileSize; //厂商文件大小
        public long startPosition = 0; //文件指针
        protected int totalSize = 0; //总计的谱图数目
        protected int totalChroma = 0; //总计的色谱数目
        
        public void start()
        {
            stopwatch.Start();
            jobInfo.log(Tag.Ready_To_Start, Status.Starting);
            AppLogs.WriteInfo(Tag.BaseInfo + jobInfo.getJsonInfo(), true);
        }
        
        public void initDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdFilePath));
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdJsonFilePath));
        }
        
        public abstract void init(JobInfo jobInfo);
        
        public abstract void doConvert();
    }
}
