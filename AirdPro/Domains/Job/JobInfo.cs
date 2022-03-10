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
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Domains.Convert
{ 
    public class JobInfo
    {
        //以C:/data/plasma.wiff为例

        //C:/data/plasma.wiff,作为job的ID存在
        private string jobId;
        //任务状态
        public string status;
        //文件的输出路径
        public string outputPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        //用于转换的参数
        public ConversionConfig config;
        //DIA-Swath,PRM,DDA. see AirdType
        public string type;
        //是否是IonMobility文件
        public Boolean ionMobility = false;
        //文件的格式,全部大写: WIFF, RAW. See FileFormat.cs
        public string format;
        //C:/data/plasma.wiff
        public string inputPath;
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

        public JobInfo(string inputPath, string outputPath, string type, ConversionConfig config)
        {
            this.jobId = inputPath + config.GetHashCode();
            this.inputPath = inputPath;
            this.type = type;
            this.outputPath = outputPath;
            // 二代压缩算法StackZDPD目前不支持COMMON模式
            if (type.Equals(AirdType.COMMON) && config.stack)
            {
                throw new Exception("Stack Layer Algorithm is not support for COMMON mode");
            }
            this.config = config;
            format = Path.GetExtension(inputPath).Replace(".","").ToUpper();
            airdFileName = FileNameUtil.buildOutputFileName(inputPath);
            airdFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".aird");
            airdJsonFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".json");
            status = ProcessingStatus.WAITING;
        }

        public void bindItem(ListViewItem item)
        {
            progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[3].Text = progressValue;
            });
            item.ToolTipText = inputPath;
            item.Tag = this;
        }

        public JobInfo log(string content)
        {
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLog.WriteInfo(content,true);
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
                AppLog.WriteInfo(content, true);
                Console.Out.WriteLine(content);
            }
           
            return this;
        }

        public void logError(string content)
        {
            progress.Report("Error");
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLog.WriteError(content, true);
            Console.Out.WriteLine(content);
            throw new Exception(content);
        }

        public string getJsonInfo()
        {
            string jobInfo = "";
            jobInfo += "inpuPath:" + inputPath + "\r\n";
            jobInfo += "outputPath:" + outputPath + "\r\n";
            jobInfo += "airdFileName:" + airdFileName + "\r\n";
            jobInfo += "airdFilePath:" + airdFilePath + "\r\n";
            jobInfo += "airdJsonFilePath:" + airdJsonFilePath + "\r\n";
            jobInfo += "ignoreZeroIntensity:" + config.ignoreZeroIntensity + "\r\n";
            jobInfo += "suffix:" + config.suffix + "\r\n";
            jobInfo += "threadId:" + threadId + "\r\n";
            jobInfo += "ThreadAccelerate:" + config.threadAccelerate + "\r\n";
            jobInfo += "mzPrecision:" + config.mzPrecision + "\r\n";
            // jobInfo += "compressor:" + (jobParams.airdAlgorithm == 1 ? "ZDPD" : ("Stack-ZDPD:" + (Math.Pow(2, jobParams.digit))) + " Layers\r\n");
            return jobInfo;
        }

        public string getJobId()
        {
            if (jobId.IsNullOrEmpty())
            {
                jobId = inputPath.GetHashCode().ToString() +
                        (outputPath + config.getCompressorStr() + config.getMzPrecisionStr() +
                         config.ignoreZeroIntensity).GetHashCode();
            }

            return jobId;
        }

        public ListViewItem buildItem()
        {
            string[] itemInfo = new string[]{
                getJobId(),
                inputPath,
                type,
                status,
                config.getMzPrecisionStr(),
                config.getCompressorStr(),
                config.ignoreZeroIntensity.ToString(),
                config.suffix,
                outputPath
            };
            ListViewItem item = new ListViewItem(itemInfo);
            progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[3].Text = progressValue;
            });
            item.ToolTipText = outputPath;
            item.Tag = this;
            return item;
        }
    }
}
