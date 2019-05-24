using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Propro.Constants;
using Propro.Domains;
using Propro.Logics;

namespace Propro.Asyncs
{
    internal class ConvertTaskManager
    {
        //需要进行处理的Job
        private Queue<ConvertJobInfo> jobQueue = new Queue<ConvertJobInfo>();
        TaskFactory fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(10));

        //全部的Job信息
        public Hashtable jobTable = new Hashtable();

        public void pushJob(ConvertJobInfo job)
        {
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
                    catch
                    {
                    }

                    if (jobInfo == null)
                    {
                        break;
                    }
                    
                    fac.StartNew(() =>
                    {
                        try
                        {
                            Console.Out.WriteLine("Start Convert");
                            jobInfo.threadId = "ThreadId:" + Thread.CurrentThread.ManagedThreadId;
                            if (jobInfo.type.Equals(ExperimentType.DIA_SWATH))
                            {
                                new DIASwathConverter().doConvert(jobInfo);
                            }
                            else if (jobInfo.type.Equals(ExperimentType.PRM))
                            {
                                new PRMConverter().doConvert(jobInfo);
                            }
                            else if (jobInfo.type.Equals(ExperimentType.FILL_INFO))
                            {
                                new FillInfoConverter().doConvert(jobInfo);
                            }
                            else if (jobInfo.type.Equals(ExperimentType.TEST_READ_SPEED))
                            {
                                new TestLinearRead().doConvert(jobInfo);
                            }
                           
                        }
                        catch (Exception ex)
                        {
                            jobInfo.addLog(ex.ToString());
                            MessageBox.Show(ex.ToString());
                        }
                    });

                }
                
            }

            return null;
        }
    }
}