/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
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

        //任务状态,不用于界面展示,界面展示的字段使用的是progress
        public string status;

        //文件的输出路径
        public string outputPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        //用于转换的参数
        public ConversionConfig config;

        //DIA,PRM,DDA. see AirdType
        public string type;

        public IProgress<string> typeLabel;

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
        public List<Log> logs = new List<Log>();

        //任务运行时产生的进度信息
        private IProgress<string> progress;

        //任务运行时产生的组合压缩,在使用动态决策器时有效
        private IProgress<string> compressor;

        //任务的线程ID,当未分配线程ID时为-1
        public int threadId = -1;

        //分配一个线程终止用的token
        public CancellationTokenSource tokenSource = new CancellationTokenSource();

        //出现异常错误的时候进行重试的次数,每一个job会被自动重试2次
        public int retryTimes = 3;

        public bool refreshReport = true;

        public JobInfo(string inputPath, string outputPath, string type, ConversionConfig config)
        {
            jobId = inputPath + config.GetHashCode();
            this.inputPath = inputPath;
            this.type = type;
            this.outputPath = outputPath;
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
                "",
                status,
                config.getMzPrecisionStr(),
                getCompressorStr(),
                config.ignoreZeroIntensity.ToString(),
                config.suffix,
                outputPath
            };
            ListViewItem item = new ListViewItem(itemInfo);
            typeLabel = new Progress<string>((typeLabel) =>
            {
                item.SubItems[ItemName.TYPE].Text = typeLabel;
            });
            progress = new Progress<string>((progressValue) =>
            {
                item.SubItems[ItemName.PROGRESS].Text = progressValue;
            });
            compressor = new Progress<string>((compressor) =>
            {
                item.SubItems[ItemName.COMPRESSOR].Text = compressor;
            });

            item.ToolTipText = outputPath;
            item.Tag = this;
            return item;
        }

        public JobInfo log(string content)
        {
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLogs.WriteInfo(content, true);
            Debug.WriteLine(content);
            return this;
        }

        public void setStatus(string status)
        {
            this.status = status;
            progress.Report(status);
        }

        public void setType(string type)
        {
            this.type = type;
            typeLabel.Report(type);
        }

        public void setCombination(string combination)
        {
            compressor.Report(combination);
        }

        public JobInfo log(string content, string status)
        {
            if (refreshReport)
            {
                progress.Report(status);
                refreshReport = false;
            }

            if (content != null)
            {
                Log log = new Log(DateTime.Now, content);
                logs.Add(log);
                AppLogs.WriteInfo(content, true);
                Debug.WriteLine(content);
            }

            return this;
        }

        public void logError(string content)
        {
            progress.Report(Status.Error);
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLogs.WriteError(content, true);
            Debug.WriteLine(content);
            throw new Exception(content);
        }

        public string getJsonInfo()
        {
            string jobInfo = Tag.Empty;
            jobInfo += Tag.Input_Path + inputPath + Const.Change_Line;
            jobInfo += Tag.Output_Path + outputPath + Const.Change_Line;
            jobInfo += Tag.Aird_File_Name + airdFileName + Const.Change_Line;
            jobInfo += Tag.Aird_File_Path + airdFilePath + Const.Change_Line;
            jobInfo += Tag.Aird_Json_File_Path + airdJsonFilePath + Const.Change_Line;
            jobInfo += Tag.Ignore_Zero_Intensity + config.ignoreZeroIntensity + Const.Change_Line;
            jobInfo += Tag.Suffix + config.suffix + Const.Change_Line;
            jobInfo += Tag.Thread_Id + threadId + Const.Change_Line;
            jobInfo += Tag.Thread_Accelerate + config.threadAccelerate + Const.Change_Line;
            jobInfo += Tag.Mz_Precision + config.getMzPrecisionStr() + Const.Change_Line;
            jobInfo += Tag.Compressor + getCompressorStr() + Const.Change_Line;
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
            if (config.autoDesicion)
            {
                return Tag.Auto_Decision;
            }

            if (ionMobility)
            {
                return config.mzIntComp + Const.Dash + config.mzByteComp + Const.Dash + config.intIntComp + Const.Dash +
                       config.intByteComp +
                       Const.Dash + config.mobiIntComp + Const.Dash +
                       config.mobiByteComp;
            }
            else
            {
                return config.mzIntComp + Const.Dash + config.mzByteComp + Const.Dash + config.intIntComp + Const.Dash +
                       config.intByteComp;
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