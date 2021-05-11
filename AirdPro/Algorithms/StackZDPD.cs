using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirdPro.Converters;
using AirdPro.Domains.Aird;
using AirdPro.Domains.Convert;
using pwiz.CLI.data;
using pwiz.CLI.msdata;

namespace AirdPro.Algorithms
{
    public class StackZDPD : IZDPD
    {
        public StackZDPD(IConverter converter) : base(converter){}

        override 
        public void compressMS1(BlockIndex index)
        {
            int layers = (int)Math.Pow(2, converter.jobInfo.jobParams.digit); //计算堆叠层数
            int iter = converter.ms1List.Count % layers == 0 ? (converter.ms1List.Count / layers) : (converter.ms1List.Count / layers + 1); //计算循环周期
            if (converter.jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, iter, (i, ParallelLoopState) =>
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + iter);
                    List<float> rts = new List<float>();
                    List<int> nums = new List<int>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= converter.ms1List.Count)
                        {
                            break;
                        }
                        TempIndex scanIndex = converter.ms1List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        if (converter.jobInfo.jobParams.includeCV)
                        {
                            cvs.Add(scanIndex.cvList);
                        }
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = cvs;
                    }
                    converter.compress(spectrumGroup, ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                for (int i = 0; i < iter; i++)
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + iter);

                    List<float> rts = new List<float>();
                    List<int> nums = new List<int>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= converter.ms1List.Count)
                        {
                            break;
                        }
                        TempIndex scanIndex = converter.ms1List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        if (converter.jobInfo.jobParams.includeCV)
                        {
                            cvs.Add(scanIndex.cvList);
                        }
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = cvs;
                    }
                    converter.compress(spectrumGroup, ts);
                    converter.addToIndex(index, ts);
                }
            }
        }

        override
        public void compressMS2(List<TempIndex> tempIndexList, BlockIndex index)
        {
            int layers = (int)Math.Pow(2, converter.jobInfo.jobParams.digit); //计算堆叠层数
            int iter = tempIndexList.Count % layers == 0 ? (tempIndexList.Count / layers) : (tempIndexList.Count / layers + 1); //计算循环周期

            if (converter.jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, iter, (i, ParallelLoopState) =>
                {
                    List<float> rts = new List<float>();
                    List<int> nums = new List<int>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= tempIndexList.Count)
                        {
                            break;
                        }
                        TempIndex scanIndex = tempIndexList[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        if (converter.jobInfo.jobParams.includeCV)
                        {
                            cvs.Add(scanIndex.cvList);
                        }
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }
                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = cvs;
                    }
                    converter.compress(spectrumGroup, ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                for (int i = 0; i < iter; i++)
                {
                    List<float> rts = new List<float>();
                    List<int> nums = new List<int>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= tempIndexList.Count)
                        {
                            break;
                        }
                        TempIndex scanIndex = tempIndexList[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        if (converter.jobInfo.jobParams.includeCV)
                        {
                            cvs.Add(scanIndex.cvList);
                        }
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }
                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = cvs;
                    }
                    converter.compress(spectrumGroup, ts);
                    converter.addToIndex(index, ts);
                }
            }
        }
    }
}