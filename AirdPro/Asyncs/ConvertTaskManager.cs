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
using AirdPro.Algorithms;
using Compress;
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
            ICompressor comp = null;
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

            if (!jobInfo.jobParams.stack)
            {
                comp = new CoreComp(converter);
            }
            else
            {
                comp = new StackComp(converter);
                comp.mobilityByteComp = new Zlib();
            }

            switch (jobInfo.jobParams.mzIntComp)
            {
                case IntCompType.IBP:
                    comp.mzIntComp = new IntegratedBinPacking();
                    break;
                case IntCompType.IVB:
                    comp.mzIntComp = new IntegratedVarByte();
                    break;
            }

            switch (jobInfo.jobParams.mzByteComp)
            {
                case ByteCompType.Zlib:
                    comp.mzByteComp = new Zlib();
                    break;
                case ByteCompType.Zstd:
                    comp.mzByteComp = new Zstd();
                    break;
                case ByteCompType.Snappy:
                    comp.mzByteComp = new Snappier();
                    break;
                case ByteCompType.Brotli:
                    comp.mzByteComp = new Brotlier();
                    break;
            }

            switch (jobInfo.jobParams.intByteComp)
            {
                case ByteCompType.Zlib:
                    comp.intByteComp = new Zlib();
                    break;
                case ByteCompType.Zstd:
                    comp.intByteComp = new Zstd();
                    break;
                case ByteCompType.Snappy:
                    comp.intByteComp = new Snappier();
                    break;
                case ByteCompType.Brotli:
                    comp.intByteComp = new Brotlier();
                    break;
            }

            converter.compressor = comp;
            converter.doConvert();
            jobInfo.setStatus(FINISHED);
        }
    }
}