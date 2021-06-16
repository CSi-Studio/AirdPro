/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains.Convert;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static AirdPro.Constants.ProcessingStatus;

namespace AirdPro.Asyncs
{
    internal class ConvertTaskManager
    {

        public static ConvertTaskManager instance;

        public ConvertTaskManager() { }

        public static ConvertTaskManager getInstance()
        {
            if (instance == null)
            {
                instance = new ConvertTaskManager();
            }

            return instance;
        }

        //需要进行处理的Job
        private Queue<JobInfo> jobQueue = new Queue<JobInfo>();
        TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        //存放全部的Job信息,用于根据JobId判定当前的Job是否已经存在
        public Hashtable jobTable = new Hashtable();
        //当某一个Job执行异常的时候会存储入本队列中
        public Hashtable errorJob = new Hashtable();

        public void pushJob(JobInfo job)
        {
            if (errorJob.Contains(job.jobId))
            {
                errorJob.Remove(job.jobId);
            }
            if (!jobTable.Contains(job.jobId))
            {
                jobQueue.Enqueue(job);
                jobTable.Add(job.jobId, job);
            }
        }

        public void clear()
        {
            jobQueue.Clear();
            jobTable.Clear();
        }

        public void run()
        {
            Boolean again = true; //本次run执行完毕以后是否立即再执行一轮

            while (again)
            {
                //如果队列中没有待执行的任务,那么进行休眠当前进程两秒
                if (jobQueue.Count == 0)
                {
                    again = false;
                    return;
                }
                JobInfo jobInfo = null;
                try
                {
                    jobInfo = jobQueue.Dequeue();
                }
                catch { }

                if (jobInfo == null)
                {
                    again = false;
                    return;
                };

                fac.StartNew(() =>
                {
                    Console.Out.WriteLine("Start Convert");
                    jobInfo.threadId = "ThreadId:" + Thread.CurrentThread.ManagedThreadId;
                    while (jobInfo.retryTimes > 0)
                    {
                        try
                        {
                            jobInfo.setStatus(RUNNING);
                            tryConvert(jobInfo);
                        }
                        catch (Exception ex)
                        {
                            jobInfo.log(ex.ToString(), "Error");
                            jobInfo.retryTimes--;
                            if (jobInfo.retryTimes > 0)
                            {
                                jobInfo.log("Retrying...left retry times:" + jobInfo.retryTimes);
                            }
                            else
                            {
                                jobInfo.setStatus(ERROR);
                                errorJob.Add(jobInfo.jobId, jobInfo);
                            }
                        }
                    }
                });
            }
        }

        public void tryConvert(JobInfo jobInfo)
        {
            
            if (jobInfo.type.Equals(AirdType.DIA_SWATH))
            {
                new SWATH(jobInfo).doConvert();
            }
            else if (jobInfo.type.Equals(AirdType.PRM))
            {
                new PRM(jobInfo).doConvert();
            }
            else if (jobInfo.type.Equals(AirdType.SCANNING_SWATH))
            {
                new ScanningSWATH(jobInfo).doConvert();
            }
            else if (jobInfo.type.Equals(AirdType.DDA))
            {
                new DDA(jobInfo).doConvert();
            }
            else if (jobInfo.type.Equals(AirdType.COMMON))
            {
                new Common(jobInfo).doConvert();
            }
            jobInfo.setStatus(FINISHED);
        }
    }
}