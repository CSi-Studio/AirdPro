﻿/*
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
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Storage.Config;
using AirdSDK.Enums;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Domains
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
        public bool ionMobility = false;

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
        public List<Stat> logs = new List<Stat>();

        //任务运行时产生的进度信息
        private IProgress<string> progress;

        //任务的线程名称
        public string threadId;

        //出现异常错误的时候进行重试的次数,每一个job会被自动重试2次
        public int retryTimes = 3;

        public bool refreshReport = true;

        public JobInfo(string inputPath, string outputPath, string type, ConversionConfig config)
        {
            jobId = inputPath + config.GetHashCode();
            this.inputPath = inputPath;
            this.type = type;
            this.outputPath = outputPath;
            // 二代压缩算法StackZDPD目前不支持COMMON模式
            if (type.Equals(AirdType.COMMON) && config.stack)
            {
                throw new Exception("Stack Layer Algorithm is not support for COMMON mode");
            }

            this.config = config;
            format = Path.GetExtension(inputPath).Replace(".", "").ToUpper();
            airdFileName = FileNameUtil.parseFileName(inputPath);
            airdFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".aird");
            airdJsonFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".json");
            status = ProcessingStatus.WAITING;
        }

        public ListViewItem buildItem()
        {
            string[] itemInfo = new string[]
            {
                getJobId(),
                inputPath,
                type,
                status,
                config.getMzPrecisionStr(),
                getCompressorStr(),
                config.ignoreZeroIntensity.ToString(),
                config.suffix,
                outputPath
            };
            ListViewItem item = new ListViewItem(itemInfo);
            progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[ItemName.PROGRESS].Text = progressValue;
            });
            item.ToolTipText = outputPath;
            item.Tag = this;
            return item;
        }

        public JobInfo log(string content)
        {
            Stat log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLogs.WriteInfo(content, true);
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
                Stat log = new Log(DateTime.Now, content);
                logs.Add(log);
                AppLogs.WriteInfo(content, true);
                Console.Out.WriteLine(content);
            }

            return this;
        }

        public void logError(string content)
        {
            progress.Report("Error");
            Stat log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLogs.WriteError(content, true);
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
            jobInfo += "mzPrecision:" + config.getMzPrecisionStr() + "\r\n";
            jobInfo += "compressor:" + getCompressorStr() + "\r\n";
            return jobInfo;
        }

        public string getJobId()
        {
            if (jobId.IsNullOrEmpty())
            {
                jobId = (inputPath + outputPath + getCompressorStr() + config.getMzPrecisionStr() +
                         config.ignoreZeroIntensity).GetHashCode() + "";
            }

            return jobId;
        }

        public string getCompressorStr()
        {
            if (ionMobility)
            {
                return config.mzIntComp + "_" + config.mzByteComp + "_" + config.intIntComp + "_" + config.intByteComp +
                       "_" + config.mobiIntComp + "_" +
                       config.mobiByteComp;
            }
            else
            {
                return config.mzIntComp + "_" + config.mzByteComp + "_" + config.intIntComp + "_" + config.intByteComp;
            }
        }

        public void refreshItem(ListViewItem item)
        {
            item.SubItems[ItemName.JOB_ID].Text = jobId;
            item.SubItems[ItemName.INPUT_PATH].Text = inputPath;
            item.SubItems[ItemName.TYPE].Text = type;
            item.SubItems[ItemName.PRECISION].Text = config.getMzPrecisionStr();
            item.SubItems[ItemName.COMPRESSOR].Text = getCompressorStr();
            item.SubItems[ItemName.IGNORE_ZERO].Text = config.ignoreZeroIntensity.ToString();
            item.SubItems[ItemName.SUFFIX].Text = config.suffix;
            item.SubItems[ItemName.OUTPUT_PATH].Text = outputPath;
        }
    }
}