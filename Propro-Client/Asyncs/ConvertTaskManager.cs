using Propro.Constants;
using Propro.Domains;
using Propro.Logics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Propro.Asyncs
{
    internal class ConvertTaskManager
    {
        //需要进行处理的Job
        private Queue<ConvertJobInfo> jobQueue = new Queue<ConvertJobInfo>();
        TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));

        //全部的Job信息
        public Hashtable jobTable = new Hashtable();
        public Hashtable errorJob = new Hashtable();

        public void pushJob(ConvertJobInfo job)
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

        public List<ConvertResult> run()
        {
            
            if (jobQueue.Count != 0)
            {
                while (true)
                {
                    ConvertJobInfo jobInfo = null;
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
                            jobInfo.status = "Running";
                            if (jobInfo.type.Equals(ExperimentType.DIA_SWATH))
                            {
                                new DIASwathConverter(jobInfo).doConvert();
                            }
                            else if (jobInfo.type.Equals(ExperimentType.PRM))
                            {
                                new PRMConverter(jobInfo).doConvert();
                            }
                            else if (jobInfo.type.Equals(ExperimentType.SCANNING_SWATH))
                            {
                                new ScanningSwathConvert(jobInfo).doConvert();
                            }
                            else if (jobInfo.type.Equals(ExperimentType.DDA))
                            {
                                new DDAConverter(jobInfo).doConvert();
                            }
                            jobInfo.status = "Finished";
                        }
                        catch (Exception ex)
                        {
                            jobInfo.log(ex.ToString(),"Error");
                            jobInfo.status = "Error";
                            errorJob.Add(jobInfo.jobId, jobInfo);
                            MessageBox.Show(ex.ToString());
                        }
                    });

                }
                
            }

            return null;
        }
    }
}