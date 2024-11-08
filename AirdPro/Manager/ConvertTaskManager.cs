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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains;
using AirdPro.Forms;
using AirdPro.Redis;
using AirdSDK.Enums;
using static AirdPro.Constants.ProcessingStatus;

namespace AirdPro.Asyncs
{
    internal class ConvertTaskManager
    {
        public static ConvertTaskManager Instance;

        public Queue<JobInfo> JobQueue = new();

        //存放全部的Job信息,用于根据JobId判定当前的Job是否已经存在
        public Hashtable JobTable = new();

        //存放已经完成转换的JobInfo,不管是否转换成功
        public Hashtable FinishedTable = new();

        public static ConvertTaskManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new ConvertTaskManager();
            }

            return Instance;
        }


        //加入一个新的转换任务,如果该任务已经在转换完毕的列表内,则将其重新放入待转换队列重新转换
        public void PushJob(JobInfo job)
        {
            if (FinishedTable.ContainsKey(job.jobId))
            {
                FinishedTable.Remove(job.jobId);
            }

            if (!JobTable.Contains(job.jobId))
            {
                JobQueue.Enqueue(job);
                JobTable.Add(job.jobId, job);
            }
        }

        //将一个任务置为已完成状态
        public void FinishedJob(JobInfo jobInfo)
        {
            JobTable.Remove(jobInfo.jobId);
            FinishedTable.Add(jobInfo.jobId, jobInfo);
        }

        //删除一个任务
        public void RemoveJob(JobInfo jobInfo)
        {
            jobInfo.tokenSource.Cancel();
            JobTable.Remove(jobInfo.jobId);
            FinishedTable.Remove(jobInfo.jobId);
        }

        public void Run()
        {
            while (true)
            {
                 //如果队列中没有待执行的任务,那么进行休眠当前进程两秒
                 if (JobQueue.Count == 0)
                 {
                     return;
                 }

                 JobInfo jobInfo = null;
                 try
                 {
                     jobInfo = JobQueue.Dequeue();
                 }
                 catch
                 {
                     // ignored
                 }

                 if (jobInfo == null)
                 {
                     return;
                 }
                 
                 if (!JobTable.Contains(jobInfo.jobId))
                 {
                     continue;
                 }

                 RunJob(jobInfo);
                 // Application.DoEvents();
            }
        }

        public void Clear()
        {
            JobQueue.Clear();
            JobTable.Clear();
        }

        public void RunJob(JobInfo jobInfo)
        {
            jobInfo.threadId = Thread.CurrentThread.ManagedThreadId;
            while (jobInfo.retryTimes > 0)
            {
                try
                {
                    jobInfo.SetStatus(RUNNING);
                    Converter converter = null;
                    if (jobInfo.format.Equals(FileFormat.TDMS))
                    {
                        converter = new TdmsConverter();
                    }
                    else if (jobInfo.format.Equals(FileFormat.imzML))
                    {
                        if (jobInfo.type != AcquisitionMethod.DDA)
                        {
                            jobInfo.type = AcquisitionMethod.DDA;
                        }
                        converter = new ImzMLConverter();
                    }
                    else if (jobInfo.msiConfig!= null)
                    {
                        converter = new MSIConverter();
                    }
                    else
                    {
                        converter = new PwizConverter();
                    }
                 
                    converter.Init(jobInfo);
                    converter.DoConvert();
                    jobInfo.SetStatus(FINISHED);
                    break;
                }
                catch (Exception ex)
                {
                    jobInfo.Log(ex.ToString(), Status.Error);
                    jobInfo.retryTimes--;
                    if (jobInfo.retryTimes > 0)
                    {
                        jobInfo.Log(Tag.Retrying_Left_Retry_Times + jobInfo.retryTimes);
                    }
                    else
                    {
                        jobInfo.SetStatus(ERROR);
                    }
                }
            }

            FinishedJob(jobInfo);
            if (jobInfo.fromRedis)
            {
                Program.redisForm.Invoke((Action)(() =>
                {
                    RedisManager.Instance.RemoveConvertingJob(jobInfo.remoteId);
                    RedisForm.JobUnderConsuming = false;
                }));
            }
        }
    }
}