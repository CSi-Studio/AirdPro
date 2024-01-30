using System.Diagnostics;
using System.IO;
using AirdPro.Constants;
using AirdPro.Domains;

namespace AirdPro.Converters
{
    public abstract class Converter
    {
        public JobInfo JobInfo; //转换任务的基本信息
        protected readonly Stopwatch Stopwatch = new(); //全局的计时器
        protected FileStream AirdStream; //最终输出的Aird文件
        protected FileStream AirdJsonStream; //最终输出的Aird索引文件
        protected FileStream AirdColumnJsonStream; //最终输出的Aird-Slice索引文件
        
        protected long FileSize; //厂商文件大小
        protected long StartPosition = 0; //文件指针
        protected int TotalSpectraCount = 0; //总计的谱图数目
        protected int TotalChromaCount = 0; //总计的色谱数目

        /**
         * 开始转换前，需要启动计时器，并且开始记录日志
         */
        protected void Start()
        {
            Stopwatch.Start();
            JobInfo.Log(Tag.Ready_To_Start, Status.Starting);
            AppLogs.WriteInfo(Tag.BaseInfo + JobInfo.GetJsonInfo(), true);
        }

        /**
         * 初始化Aird文件对应的文件
         */
        protected void InitDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(JobInfo.airdFilePath) ?? string.Empty);
            Directory.CreateDirectory(Path.GetDirectoryName(JobInfo.airdJsonFilePath) ?? string.Empty);
        }
        
        public abstract void Init(JobInfo jobInfo);
        
        public abstract void DoConvert();
    }
}
