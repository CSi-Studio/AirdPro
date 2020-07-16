using AirdPro.Constants;
using AirdPro.Domains;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<Result> run()
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
                            if (jobInfo.type.Equals(ExperimentType.DIA_SWATH))
                            {
                                new DIASWATH(jobInfo).doConvert();
                            }
                            else if (jobInfo.type.Equals(ExperimentType.PRM))
                            {
                                new PRM(jobInfo).doConvert();
                            }
                            else if (jobInfo.type.Equals(ExperimentType.SCANNING_SWATH))
                            {
                                new ScanningSWATH(jobInfo).doConvert();
                            }
                            else if (jobInfo.type.Equals(ExperimentType.DDA))
                            {
                                new DDA(jobInfo).doConvert();
                            }
                            jobInfo.status = FINISHED;
                        }
                        catch (Exception ex)
                        {
                            jobInfo.log(ex.ToString(),"Error");
                            jobInfo.status = ERROR;
                            errorJob.Add(jobInfo.jobId, jobInfo);
//                            MessageBox.Show(ex.ToString());
                        }
                    });

                }
                
            }

            return null;
        }
    }
}