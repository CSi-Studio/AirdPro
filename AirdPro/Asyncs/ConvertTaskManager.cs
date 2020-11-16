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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirdPro.Converters;
using AirdPro.Domains.Convert;
using static AirdPro.Constants.ProcessingStatus;

namespace AirdPro.Asyncs
{
    internal class ConvertTaskManager
    {
        //需要进行处理的Job
        private Queue<JobInfo> jobQueue = new Queue<JobInfo>();
        TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        //全部的Job信息
        public Hashtable jobTable = new Hashtable();
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
                jobTable.Add(job.jobId,job);
            }
        }

        public void clear()
        {
            jobQueue.Clear();
            jobTable.Clear();
        }

        public void run()
        {
            
            if (jobQueue.Count != 0)
            {
                while (true)
                {
                    JobInfo jobInfo = null;
                    try
                    {
                        jobInfo = jobQueue.Dequeue();
                    }
                    catch {}

                    if (jobInfo == null) break;
                    
                    fac.StartNew(() =>
                    {
                        try
                        {
                            Console.Out.WriteLine("Start Convert");
                            jobInfo.threadId = "ThreadId:" + Thread.CurrentThread.ManagedThreadId;
                            jobInfo.status = RUNNING;
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
                            jobInfo.status = FINISHED;
                        }
                        catch (Exception ex)
                        {
                            jobInfo.log(ex.ToString(),"Error");
                            jobInfo.status = ERROR;
                            errorJob.Add(jobInfo.jobId, jobInfo);
                        }
                    });

                }
                
            }
        }
    }
}