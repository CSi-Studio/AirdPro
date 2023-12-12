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
        public ICompressor compressor;
        
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
        
        public void initCompressor()
        {
            ICompressor comp = jobInfo.config.stack ? new StackComp(this) : new CoreComp(this);
            //探索模式和非自动决策模式,会在此处初始化指定的压缩内核
            if (!jobInfo.config.autoDesicion)
            {
                if (jobInfo.ionMobility)
                {
                    comp.mobiIntComp = IntComp.build(jobInfo.config.mobiIntComp);
                    comp.mobiByteComp = ByteComp.build(jobInfo.config.mobiByteComp);
                }

                comp.mzIntComp = SortedIntComp.build(jobInfo.config.mzIntComp);
                comp.mzByteComp = ByteComp.build(jobInfo.config.mzByteComp);

                comp.intIntComp = IntComp.build(jobInfo.config.intIntComp);
                comp.intByteComp = ByteComp.build(jobInfo.config.intByteComp);
            }

            this.compressor = comp;
        }
        
        public abstract void init(JobInfo jobInfo);
        
        public abstract void doConvert();
    }
}
