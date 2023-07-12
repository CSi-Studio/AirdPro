﻿/*
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
using System.Threading.Tasks;
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains;
using AirdPro.Properties;
using static AirdPro.Constants.ProcessingStatus;

namespace AirdPro.Asyncs
{
    internal class ConvertTaskManager
    {
        public static ConvertTaskManager instance;

       // public TaskFactory fac = null;

        public Queue<JobInfo> jobQueue = new Queue<JobInfo>();

        //存放全部的Job信息,用于根据JobId判定当前的Job是否已经存在
        public Hashtable jobTable = new Hashtable();

        //存放已经完成转换的JobInfo,不管是否转换成功
        public Hashtable finishedTable = new Hashtable();

        public ConvertTaskManager()
        {
            //fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(Settings.Default.MaxConversionTasks));
        }

        public static ConvertTaskManager getInstance()
        {
            if (instance == null)
            {
                instance = new ConvertTaskManager();
            }

            return instance;
        }


        //加入一个新的转换任务,如果该任务已经在转换完毕的列表内,则将其重新放入待转换队列重新转换
        public void pushJob(JobInfo job)
        {
            if (finishedTable.ContainsKey(job.getJobId()))
            {
                finishedTable.Remove(job.getJobId());
            }

            if (!jobTable.Contains(job.getJobId()))
            {
                jobQueue.Enqueue(job);
                jobTable.Add(job.getJobId(), job);
            }
        }

        //将一个任务置为已完成状态
        public void finishedJob(JobInfo jobInfo)
        {
            jobTable.Remove(jobInfo.getJobId());
            finishedTable.Add(jobInfo.getJobId(), jobInfo);
        }

        //删除一个任务
        public void removeJob(JobInfo jobInfo)
        {
            jobInfo.tokenSource.Cancel();
            jobTable.Remove(jobInfo.getJobId());
            finishedTable.Remove(jobInfo.getJobId());
        }

        public void run()
        {
            while (true)
            {
                //如果队列中没有待执行的任务,那么进行休眠当前进程两秒
                if (jobQueue.Count == 0)
                {
                    return;
                }

                JobInfo jobInfo = null;
                try
                {
                    jobInfo = jobQueue.Dequeue();
                }
                catch
                {
                }

                if (jobInfo == null)
                {
                    return;
                }

                 runJob(jobInfo);
                 Application.DoEvents();
             
                // fac.StartNew(() => runJob(jobInfo), jobInfo.tokenSource.Token);
            }
        }

        public void clear()
        {
            jobQueue.Clear();
            jobTable.Clear();
        }

        public void runJob(JobInfo jobInfo)
        {
            jobInfo.threadId = Thread.CurrentThread.ManagedThreadId;
            while (jobInfo.retryTimes > 0)
            {
                try
                {
                    jobInfo.setStatus(RUNNING);
                    Converter converter = new Converter(jobInfo);
                    converter.doConvert();
                    jobInfo.setStatus(FINISHED);
                    break;
                }
                catch (Exception ex)
                {
                    jobInfo.log(ex.ToString(), Status.Error);
                    jobInfo.retryTimes--;
                    if (jobInfo.retryTimes > 0)
                    {
                        jobInfo.log(Tag.Retrying_Left_Retry_Times + jobInfo.retryTimes);
                    }
                    else
                    {
                        jobInfo.setStatus(ERROR);
                    }
                }
            }

            finishedJob(jobInfo);
        }
    }
}