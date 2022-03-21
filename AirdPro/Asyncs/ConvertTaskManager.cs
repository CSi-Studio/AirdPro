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
using System.Collections.Concurrent;
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
                catch { }

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
                            jobInfo.log(ex.ToString(), "Error");
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

            if (jobInfo.config.stack)
            {
                comp = new StackComp(converter);
                comp.mobiByteComp = new ZstdWrapper();
            }
            else
            {
                comp = new CoreComp(converter);
                switch (jobInfo.config.mobiIntComp)
                {
                    case IntCompType.VB:
                        comp.mobiIntComp = new VarByteWrapper();
                        break;
                    case IntCompType.BP:
                        comp.mobiIntComp = new BinPackingWrapper();
                        break;
                }

                switch (jobInfo.config.mobiByteComp)
                {
                    case ByteCompType.Zlib:
                        comp.mobiByteComp = new ZlibWrapper();
                        break;
                    case ByteCompType.Zstd:
                        comp.mobiByteComp = new ZstdWrapper();
                        break;
                    case ByteCompType.Snappy:
                        comp.mobiByteComp = new SnappyWrapper();
                        break;
                    case ByteCompType.Brotli:
                        comp.mobiByteComp = new BrotliWrapper();
                        break;
                }
            }

            switch (jobInfo.config.mzIntComp)
            {
                case IntCompType.IBP:
                    comp.mzIntComp = new IntegratedBinPackingWrapper();
                    break;
                case IntCompType.IVB:
                    comp.mzIntComp = new IntegratedVarByteWrapper();
                    break;
            }

            switch (jobInfo.config.mzByteComp)
            {
                case ByteCompType.Zlib:
                    comp.mzByteComp = new ZlibWrapper();
                    break;
                case ByteCompType.Zstd:
                    comp.mzByteComp = new ZstdWrapper();
                    break;
                case ByteCompType.Snappy:
                    comp.mzByteComp = new SnappyWrapper();
                    break;
                case ByteCompType.Brotli:
                    comp.mzByteComp = new BrotliWrapper();
                    break;
            }

            switch (jobInfo.config.intIntComp)
            {
                case IntCompType.VB:
                    comp.intIntComp = new VarByteWrapper();
                    break;
                case IntCompType.BP:
                    comp.intIntComp = new BinPackingWrapper();
                    break;
            }

            switch (jobInfo.config.intByteComp)
            {
                case ByteCompType.Zlib:
                    comp.intByteComp = new ZlibWrapper();
                    break;
                case ByteCompType.Zstd:
                    comp.intByteComp = new ZstdWrapper();
                    break;
                case ByteCompType.Snappy:
                    comp.intByteComp = new SnappyWrapper();
                    break;
                case ByteCompType.Brotli:
                    comp.intByteComp = new BrotliWrapper();
                    break;
            }

            converter.compressor = comp;
            converter.doConvert();
            jobInfo.setStatus(FINISHED);
        }
    }
}