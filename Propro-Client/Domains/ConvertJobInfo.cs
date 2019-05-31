using Propro.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Propro.Domains
{
    public class ConvertJobInfo
    {
        //以C:/data/plasma.wiff为例

        //C:/data/plasma.wiff,作为job的ID存在
        public string jobId;
        //0:DIA-Swath,1:PRM,2:Fill Info. see ExperimentType
        public string type;
        //当前任务的状态
        public string status = "Ready";
        //文件的格式,全部大写: WIFF, RAW
        public string format;
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
        //忽略intensity为0的数据
        public Boolean ignoreZeroIntensity = true;
        //对intensity是否求log10以降低精度
        public Boolean log10 = true;

        public List<Log> logs = new List<Log>();
        private IProgress<string> progress;

        //任务的线程名称
        public string threadId;
        //额外的文件后缀名称
        public string suffix;
        //任务的创建人
        public string creator;

        public CancellationTokenSource cancellationTokenSource;

        public ConvertJobInfo(string inputFilePath, string outputFolderPath, string type, Boolean ignoreZeroIntensity, Boolean log10, string suffix, ListViewItem item)
        {
            this.jobId = inputFilePath;
            this.inputFilePath = inputFilePath;
            this.outputFolderPath = outputFolderPath;
            this.ignoreZeroIntensity = ignoreZeroIntensity;
            this.log10 = log10;
            this.type = type;
            this.suffix = suffix;
            format = Path.GetExtension(inputFilePath).Replace(".","").ToUpper();
            airdFileName = FileNameUtil.buildOutputFileName(inputFilePath);
            airdFilePath = Path.Combine(outputFolderPath, airdFileName + suffix + ".aird");
            airdJsonFilePath = Path.Combine(outputFolderPath, airdFileName + suffix + ".json");
            this.cancellationTokenSource = new CancellationTokenSource();
            this.progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[2].Text = progressValue;
            });
        }

        public ConvertJobInfo log(string content)
        {
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            Console.Out.WriteLine(content);
            return this;
        }

        public ConvertJobInfo log(string content, string progressReport)
        {
            progress.Report(progressReport);
            if (content != null)
            {
                Log log = new Log(DateTime.Now, content);
                logs.Add(log);
                Console.Out.WriteLine(content);
            }
           
            return this;
        }

        public void logError(string content)
        {
            progress.Report("Error");
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            Console.Out.WriteLine(content);
            throw new Exception(content);
        }

        public string getJsonInfo()
        {
            string jobInfo = "";
            jobInfo += "inputFilePath:" + inputFilePath + "\r\n";
            jobInfo += "outputFolderPath:" + outputFolderPath + "\r\n";
            jobInfo += "airdFileName:" + airdFileName + "\r\n";
            jobInfo += "airdFilePath:" + airdFilePath + "\r\n";
            jobInfo += "airdJsonFilePath:" + airdJsonFilePath + "\r\n";
            jobInfo += "ignoreZeroIntensity:" + ignoreZeroIntensity + "\r\n";
            jobInfo += "suffix:" + suffix + "\r\n";
            jobInfo += "threadId:" + threadId + "\r\n";
          
            return jobInfo;
        }
    }
}
