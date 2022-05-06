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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirdPro.Algorithms;
using AirdPro.Domains;
using AirdSDK.Compressor;
using AirdSDK.Enums;
using static AirdPro.Constants.ProcessingStatus;

namespace AirdPro.Asyncs
{
    internal class ConvertTaskManager
    {
        public static ConvertTaskManager instance;

        public ConvertTaskManager()
        {
        }

        public static ConvertTaskManager getInstance()
        {
            if (instance == null)
            {
                instance = new ConvertTaskManager();
            }

            return instance;
        }

        //需要进行处理的Job
        Queue<JobInfo> jobQueue = new Queue<JobInfo>();
        TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        //存放全部的Job信息,用于根据JobId判定当前的Job是否已经存在
        public Hashtable jobTable = new Hashtable();
        public Hashtable finishedTable = new Hashtable();

        //加入一个新的转换任务
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

        public void clear()
        {
            jobQueue.Clear();
            jobTable.Clear();
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

                fac.StartNew(() =>
                {
                    jobInfo.threadId = "ThreadId:" + Thread.CurrentThread.ManagedThreadId;
                    while (jobInfo.retryTimes > 0)
                    {
                        try
                        {
                            jobInfo.setStatus(RUNNING);
                            tryConvert(jobInfo);
                            jobInfo.retryTimes = 0; //成功转换以后不需要在重试
                        }
                        catch (Exception ex)
                        {
                            jobInfo.log(ex.ToString(), Status.Error);
                            jobInfo.retryTimes--;
                            if (jobInfo.retryTimes > 0)
                            {
                                jobInfo.log("Retrying...left retry times:" + jobInfo.retryTimes);
                            }
                            else
                            {
                                jobInfo.setStatus(ERROR);
                            }
                        }
                    }

                    jobTable.Remove(jobInfo.getJobId());
                    finishedTable.Add(jobInfo.getJobId(), jobInfo);
                });
            }
        }

        public void tryConvert(JobInfo jobInfo)
        {
            ICompressor comp;
            IConverter converter = null;
            if (jobInfo.type.Equals(AirdType.DIA_SWATH))
            {
                converter = new DIA(jobInfo);
            }
            else if (jobInfo.type.Equals(AirdType.PRM))
            {
                converter = new PRM(jobInfo);
            }
            else if (jobInfo.type.Equals(AirdType.SCANNING_SWATH))
            {
                converter = new ScanningSWATH(jobInfo);
            }
            else if (jobInfo.type.Equals(AirdType.DDA))
            {
                converter = new DDA(jobInfo);
            }
            else if (jobInfo.type.Equals(AirdType.COMMON))
            {
                converter = new Common(jobInfo);
            }
            else if (jobInfo.type.Equals(AirdType.DDA_PASEF))
            {
                jobInfo.ionMobility = true;
                converter = new DDAPasef(jobInfo);
            }
            else if (jobInfo.type.Equals(AirdType.DIA_PASEF))
            {
                jobInfo.ionMobility = true;
                converter = new DIAPasef(jobInfo);
            }

            comp = jobInfo.config.stack ? new StackComp(converter) : new CoreComp(converter);

            //探索模式和非自动决策模式,会在此处初始化指定的压缩内核
            if (jobInfo.config.autoExplorer || !jobInfo.config.autoDesicion)
            {
                if (jobInfo.ionMobility)
                {
                    comp.mobiIntComp = IntComp.build(jobInfo.config.mobiIntComp);
                    comp.mobiByteComp = ByteComp.build(jobInfo.config.mobiByteComp);
                }

                comp.mzIntComp = SortedIntComp.build(jobInfo.config.mzIntComp);
                comp.mzByteComp = ByteComp.build(jobInfo.config.mzByteComp);

                comp.intIntComp = IntComp.build(jobInfo.config.intIntComp);
                comp.intByteComp = ByteComp.build(jobInfo.config.intByteComp);
            }

            converter.compressor = comp;
            converter.doConvert();
            jobInfo.setStatus(FINISHED);
        }
    }
}