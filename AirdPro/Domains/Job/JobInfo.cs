/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AirdPro.Constants;
using AirdPro.Storage.Config;
using AirdSDK.Enums;
using AirdSDK.Utils;
using Newtonsoft.Json;
using ThermoFisher.CommonCore.Data;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace AirdPro.Domains
{
    public class JobInfo
    {
        public const string AutoType = "Auto";

        //这个任务是否来自于Redis
        public bool fromRedis = false;

        public string remoteId;
        
        //使用NextId作为自增函数
        public string jobId;

        //任务状态,不用于界面展示,界面展示的字段使用的是progress
        public string status;

        //文件的输出路径
        public string outputPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        //用于转换的参数
        public ConversionConfig config;

        //DIA,PRM,DDA. see AcquisitionMethod
        public string type;
        
        [JsonIgnore]
        public IProgress<string> typeLabel;

        //是否是IonMobility文件
        public bool ionMobility = false;

        //文件的格式,全部大写: WIFF, RAW. See FileFormat.cs
        public string format;
        
        //C:/data/plasma.wiff
        public string inputPath;

        //文件本名
        public string airdFileName;

        //例如: D://aird
        public string airdFilePath;

        //例如: D://aird/plasma.json
        public string airdJsonFilePath;

        //例如： D://aird/plasma.cjson
        public string airdColumnJsonFilePath;

        //例如:  D://aird/plasma.proto
        public string airdColumnProtoFilePath;

        //任务运行时产生的日志
        [JsonIgnore]
        public List<Log> logs = new();

        //任务运行时产生的进度信息
        [JsonIgnore]
        private IProgress<string> progress;

        //任务运行时产生的组合压缩,在使用动态决策器时有效
        [JsonIgnore]
        private IProgress<string> compressor;

        //任务的线程ID,当未分配线程ID时为-1
        public int threadId = -1;

        //分配一个线程终止用的token
        [JsonIgnore]
        public CancellationTokenSource tokenSource = new ();

        //出现异常错误的时候进行重试的次数,每一个job会被自动重试2次
        public int retryTimes = 3;

        //用于表示是否刷新日志界面的字段
        public bool refreshReport = true;

        //用于全局自增的id字段
        public static int id = 0;
        
        //产生全局唯一且自增的jobId
        public static string NextId()
        {
            return Interlocked.Increment(ref id)+"";
        }

        //本构造函数不能删除,可以避免在JSON反序列化的时候调用下面的有参构造函数,从而提前调用NextId()的自增函数
        public JobInfo()
        {
        }

        public JobInfo(string inputPath, string outputPath, string type, ConversionConfig config)
        {
            jobId = NextId();
            this.inputPath = inputPath;
            this.type = type;
            this.outputPath = outputPath;
            this.config = config;
            format = Path.GetExtension(inputPath).Replace(".", "").ToUpper();
            airdFileName = FileNameUtil.parseFileName(inputPath);
            airdFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".aird");
            airdJsonFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".json");
            airdColumnJsonFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".cjson");
            airdColumnProtoFilePath = Path.Combine(outputPath, airdFileName + config.suffix + ".index");
            status = ProcessingStatus.WAITING;
        }

        public ListViewItem BuildItem()
        {
            string[] itemInfo = new string[]
            {
                jobId,
                inputPath,
                type,
                config.configName,
                config.engine + "",
                config.centroid.ToString(),
                status,
                config.GetMzPrecisionStr(),
                GetCompressorStr(),
                config.ignoreZeroIntensity.ToString(),
                config.suffix,
                outputPath
            };
            ListViewItem item = new ListViewItem(itemInfo);
            typeLabel = new Progress<string>((typeLabel) => { item.SubItems[ItemName.TYPE].Text = typeLabel; });
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

        public JobInfo Log(string content)
        {
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLogs.WriteInfo(content, true);
            return this;
        }

        public void SetStatus(string status)
        {
            this.status = status;
            progress.Report(status);
        }

        public void SetType(string type)
        {
            this.type = type;
            typeLabel.Report(type);
        }

        public void SetCombination(string combination)
        {
            compressor.Report(combination);
        }

        public JobInfo Log(string content, string status)
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
            }

            return this;
        }

        public void LogError(string content)
        {
            progress.Report(Status.Error);
            Log log = new Log(DateTime.Now, content);
            logs.Add(log);
            AppLogs.WriteError(content, true);
            Debug.WriteLine(content);
            throw new Exception(content);
        }

        public string GetJsonInfo()
        {
            string jobInfo = Tag.Empty;
            jobInfo += Tag.ConfigName + config.configName + Const.Change_Line;
            jobInfo += Tag.Engine + config.engine + Const.Change_Line;
            jobInfo += Tag.Input_Path + inputPath + Const.Change_Line;
            jobInfo += Tag.Output_Path + outputPath + Const.Change_Line;
            jobInfo += Tag.Aird_File_Name + airdFileName + Const.Change_Line;
            jobInfo += Tag.Aird_File_Path + airdFilePath + Const.Change_Line;
            jobInfo += Tag.Aird_Json_File_Path + airdJsonFilePath + Const.Change_Line;
            jobInfo += Tag.Aird_Column_Json_File_Path + airdColumnJsonFilePath + Const.Change_Line;
            jobInfo += Tag.Ignore_Zero_Intensity + config.ignoreZeroIntensity + Const.Change_Line;
            jobInfo += Tag.Suffix + config.suffix + Const.Change_Line;
            jobInfo += Tag.Thread_Id + threadId + Const.Change_Line;
            jobInfo += Tag.Mz_Precision + config.GetMzPrecisionStr() + Const.Change_Line;
            jobInfo += Tag.Compressor + GetCompressorStr() + Const.Change_Line;
            if (config.autoDesicion)
            {
                jobInfo += config.spectraToPredict + " spectra for prediction" + Const.Change_Line;
                jobInfo += "size:ct:dt=" + config.compressionSizeWeight + ":" + config.compressionTimeWeight + ":" +
                           config.decompressionTimeWeight + Const.Change_Line;
            }

            return jobInfo;
        }

        public Dictionary<string, string> GetJobDict()
        {
            Dictionary<string, string> dict = new();
            dict.Add(Tag.ConfigName, config.configName);
            dict.Add(Tag.Engine, config.engine+"");
            dict.Add(Tag.Input_Path, inputPath);
            dict.Add(Tag.Output_Path, outputPath);
            dict.Add(Tag.Aird_File_Name, airdFileName);
            dict.Add(Tag.Aird_File_Path, airdFilePath);
            dict.Add(Tag.Aird_Json_File_Path, airdJsonFilePath);
            if (config.engine.Equals(AirdEngine.ColumnCompression))
            {
                dict.Add(Tag.Aird_Column_Json_File_Path, airdColumnJsonFilePath);
            }
            dict.Add(Tag.Ignore_Zero_Intensity, config.ignoreZeroIntensity+"");
            dict.Add(Tag.Suffix, config.suffix);
            dict.Add(Tag.Thread_Id, threadId+"");
            dict.Add(Tag.Mz_Precision, config.GetMzPrecisionStr());
            dict.Add(Tag.Compressor, GetCompressorStr());

            return dict;
        }

        public string GetUniqueId()
        {
            return inputPath + outputPath + GetCompressorStr() + config.GetMzPrecisionStr() +
                   config.ignoreZeroIntensity;
        }

        public string GetCompressorStr()
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

        public void RefreshItem(ListViewItem item)
        {
            item.SubItems[ItemName.JOB_ID].Text = jobId;
            item.SubItems[ItemName.INPUT_PATH].Text = inputPath;
            item.SubItems[ItemName.TYPE].Text = type;
            item.SubItems[ItemName.PRECISION].Text = config.GetMzPrecisionStr();
            item.SubItems[ItemName.COMPRESSOR].Text = GetCompressorStr();
            item.SubItems[ItemName.IGNORE_ZERO].Text = config.ignoreZeroIntensity.ToString();
            item.SubItems[ItemName.SUFFIX].Text = config.suffix;
            item.SubItems[ItemName.OUTPUT_PATH].Text = outputPath;
        }

        public void Reset()
        {
            jobId = NextId();
            status = ProcessingStatus.WAITING;
            logs = new List<Log>();
        }
    }
}