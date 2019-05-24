using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using pwiz.CLI.msdata;
using Propro.Structs;
using Propro.Utils;

namespace Propro.Domains
{
    public class ConvertJobInfo
    {
        //以C:/data/plasma.wiff为例

        //C:/data/plasma.wiff,作为job的ID存在
        public string jobId;
        //0:DIA-Swath,1:PRM,2:Fill Info. see ExperimentType
        public string type;
        //C:/data/plasma.wiff
        public string inputFilePath;
        //D://aird
        public string outputFolderPath;
        //plasma
        public string airdFileName;
        //D://aird
        public string airdFilePath;
        //D://aird/plasma.json
        public string airdJsonFilePath;
        //mz精确到小数点后三位
        public int mzPrecision = 1000;
        //intensity精确到小数点后一位
        public int intensityPrecision = 10;
        //忽略intensity为0的数据
        public Boolean ignoreZeroIntensity = true;

        public List<Log> logs = new List<Log>();
        public IProgress<string> progress;

        //任务的线程名称
        public string threadId;
        //额外的文件后缀名称
        public string suffix;

        public CancellationTokenSource cancellationTokenSource;

        public ConvertJobInfo(string inputFilePath, string outputFolderPath, string type, ListViewItem item)
        {
            this.jobId = inputFilePath;
            this.inputFilePath = inputFilePath;
            this.outputFolderPath = outputFolderPath;
            this.type = type;
            airdFileName = FileNameUtil.buildOutputFileName(inputFilePath);
            airdFilePath = Path.Combine(outputFolderPath, airdFileName + suffix + ".aird");
            airdJsonFilePath = Path.Combine(outputFolderPath, airdFileName + suffix + ".json");
            this.cancellationTokenSource = new CancellationTokenSource();
            this.progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[2].Text = progressValue;
            });
        }

        public ConvertJobInfo(string inputFilePath, string outputFolderPath, string type, int mzPrecision,
            int intensityPrecision, Boolean ignoreZeroIntensity,string suffix, ListViewItem item)
        {
            this.jobId = inputFilePath;
            this.inputFilePath = inputFilePath;
            this.outputFolderPath = outputFolderPath;
            this.mzPrecision = mzPrecision;
            this.intensityPrecision = intensityPrecision;
            this.ignoreZeroIntensity = ignoreZeroIntensity;
            this.type = type;
            this.suffix = suffix;
            airdFileName = FileNameUtil.buildOutputFileName(inputFilePath);
            airdFilePath = Path.Combine(outputFolderPath, airdFileName + suffix + ".aird");
            airdJsonFilePath = Path.Combine(outputFolderPath, airdFileName + suffix + ".json");
            this.cancellationTokenSource = new CancellationTokenSource();
            this.progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[2].Text = progressValue;
            });
        }

        public void addLog(string content)
        {
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
        }

        public string getJsonInfo()
        {
            string jobInfo = "";
            jobInfo += "inputFilePath:" + inputFilePath + "\r\n";
            jobInfo += "outputFolderPath:" + outputFolderPath + "\r\n";
            jobInfo += "airdFileName:" + airdFileName + "\r\n";
            jobInfo += "airdFilePath:" + airdFilePath + "\r\n";
            jobInfo += "airdJsonFilePath:" + airdJsonFilePath + "\r\n";
            jobInfo += "mzPrecision:" + mzPrecision + "\r\n";
            jobInfo += "intensityPrecision:" + intensityPrecision + "\r\n";
            jobInfo += "ignoreZeroIntensity:" + ignoreZeroIntensity + "\r\n";
            jobInfo += "suffix:" + suffix + "\r\n";
            jobInfo += "threadId:" + threadId + "\r\n";
          
            return jobInfo;
        }
    }
}
