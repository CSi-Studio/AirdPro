using System;
using System.Collections.Generic;
using AirdPro.Domains;
using System.Diagnostics;
using System.IO;

namespace AirdPro.Converters
{
    internal class TdmsConverter : IConverter
    {
        public TdmsConverter()
        {
        }

        public override void init(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            initCompressor();
        }

        public override void doConvert()
        {
            try
            {
                start();
                initDirectory();
                long size = 0;
                
                using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
                {
                    using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                    {
                        using (var tdms = new NationalInstruments.Tdms.File(jobInfo.inputPath))
                        {
                            tdms.Open();
                            foreach (var group in tdms)
                            {
                                Console.WriteLine("Group: {0}", group.Name);
                                int iter = 0;
                                
                                foreach (var channel in group)
                                {
                                    if (iter % 2 == 0)
                                    {
                                        List<double> dataList = new List<double>(channel.GetData<double>());
                                    }
                                    else
                                    {
                                        List<float> dataList = new List<float>(channel.GetData<float>());
                                    }

                                    iter++;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                jobInfo.log(ee.Message);
            }
        }
    }
}