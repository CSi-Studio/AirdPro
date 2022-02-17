/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AirdPro.Constants;

namespace AirdPro.Domains.Convert
{ 
    public class JobInfo
    {
        //以C:/data/plasma.wiff为例

        //C:/data/plasma.wiff,作为job的ID存在
        public string jobId;
        //用于转换的参数
        public JobParams jobParams;
        //DIA-Swath,PRM,DDA. see ExperimentType
        public string type;
        //文件的格式,全部大写: WIFF, RAW
        public string format;
        //C:/data/plasma.wiff
        public string inputFilePath;
        //转换后的文件输出路径 D://aird
        public string outputFolderPath;
        //文件本名 plasma
        public string airdFileName;
        //例如: D://aird
        public string airdFilePath;
        //例如: D://aird/plasma.json
        public string airdJsonFilePath;
        //任务运行时产生的日志
        public List<Log> logs = new List<Log>();
        //任务运行时产生的进度信息
        private IProgress<string> progress;
        //任务的线程名称
        public string threadId;
        //出现异常错误的时候进行重试的次数,每一个job会被自动重试2次
        public int retryTimes = 3;

        public bool refreshReport = true;

        public CancellationTokenSource cancellationTokenSource;

        public JobInfo()
        {
        }

        public JobInfo(string inputFilePath, string outputFolderPath, 
            string type, JobParams jobParams, ListViewItem item)
        {
            this.jobId = inputFilePath;
            this.inputFilePath = inputFilePath;
            this.outputFolderPath = outputFolderPath;
            this.type = type;
            // 二代压缩算法StackZDPD目前不支持COMMON和SCANNING_SWATH模式
            if (type.Equals(AirdType.COMMON) || type.Equals(AirdType.SCANNING_SWATH))
            {
                jobParams.airdAlgorithm = 1;  
            }
            this.jobParams = jobParams;
            format = Path.GetExtension(inputFilePath).Replace(".","").ToUpper();
            airdFileName = FileNameUtil.buildOutputFileName(inputFilePath);
            airdFilePath = Path.Combine(outputFolderPath, airdFileName + jobParams.suffix + ".aird");
            airdJsonFilePath = Path.Combine(outputFolderPath, airdFileName + jobParams.suffix + ".json");
            this.cancellationTokenSource = new CancellationTokenSource();
            this.progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[3].Text = progressValue;
            });
            item.SubItems[2].Text = jobParams.getAirdAlgorithmStr();
            item.SubItems[4].Text = outputFolderPath;
        }

        public JobInfo log(string content)
        {
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            Console.Out.WriteLine(content);
            return this;
        }

        public void setStatus(string status)
        {
            progress.Report(status);
        }

        public JobInfo log(string content, string progressReport)
        {
            if (refreshReport)
            {
                progress.Report(progressReport);
                refreshReport = false;
            }
            
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
            jobInfo += "ignoreZeroIntensity:" + jobParams.ignoreZeroIntensity + "\r\n";
            jobInfo += "suffix:" + jobParams.suffix + "\r\n";
            jobInfo += "threadId:" + threadId + "\r\n";
            jobInfo += "ThreadAccelerate:" + jobParams.threadAccelerate + "\r\n";
            jobInfo += "mzPrecision:" + jobParams.mzPrecision + "\r\n";
            jobInfo += "compressor:" + (jobParams.airdAlgorithm == 1 ? "ZDPD" : ("Stack-ZDPD:" + (Math.Pow(2, jobParams.digit))) + " Layers\r\n");
            return jobInfo;
        }
    }
}
