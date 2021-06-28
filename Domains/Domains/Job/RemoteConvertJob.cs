using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Domains.Job
{
    public class RemoteConvertJob
    {
        public string sourcePath;
        public string targetPath;
        public string type;
        public double mzPrecision;
        //目前远程任务默认为第一代压缩算法
        public int airdAlgorithm = 1;
        public int digit = 8;

        public String getAirdAlgorithmStr()
        {
            return airdAlgorithm == 1 ? "ZDPD" : ("Stack-ZDPD:" + Math.Pow(2,digit).ToString() + " Layers");
        }
    }
}
