/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains;
using AirdSDK.Beans;
using AirdSDK.Compressor;
using pwiz.CLI.msdata;
using pwiz.CLI.util;

namespace AirdPro.Algorithms
{
    public class StackComp : ICompressor
    {
        public StackComp(Converter converter) : base(converter)
        {
        }

        public override void compressMS1(Converter converter, BlockIndex index)
        {
            int layers = (int) Math.Pow(2, digit); //计算堆叠层数
            int iter = converter.ms1List.Count % layers == 0
                ? (converter.ms1List.Count / layers)
                : (converter.ms1List.Count / layers + 1); //计算循环周期
            MsIndex[] ms1List = converter.ms1List.ToArray();
            if (multiThread)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                int process = 0;
                //使用多线程处理数据提取与压缩
                Parallel.For(0, iter, (i, ParallelLoopState) =>
                {
                    Interlocked.Increment(ref process);
                    converter.jobInfo.log(null, Tag.progress(Tag.MS1, process, iter));
                    List<double> rts = new List<double>();
                    List<int> nums = new List<int>();
                    List<long> tics = new List<long>();
                    List<double> basePeakIntensities = new List<double>();
                    List<double> basePeakMzs = new List<double>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= ms1List.Length)
                        {
                            break;
                        }

                        MsIndex scanIndex = ms1List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        tics.Add(scanIndex.tic);
                        basePeakIntensities.Add(scanIndex.basePeakIntensity);
                        basePeakMzs.Add(scanIndex.basePeakMz);
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, basePeakIntensities, basePeakMzs, cvs);

                    compress(spectrumGroup, ts);
                    table.Add(i, ts);
                });
                converter.writeToFile(table, index);
            }
            else
            {
                for (int i = 0; i < iter; i++)
                {
                    converter.jobInfo.log(null, Tag.progress(Tag.MS1, i, iter));

                    List<double> rts = new List<double>();
                    List<int> nums = new List<int>();
                    List<long> tics = new List<long>();
                    List<double> basePeakIntensities = new List<double>();
                    List<double> basePeakMzs = new List<double>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= converter.ms1List.Count)
                        {
                            break;
                        }

                        MsIndex scanIndex = ms1List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        tics.Add(scanIndex.tic);
                        basePeakIntensities.Add(scanIndex.basePeakIntensity);
                        basePeakMzs.Add(scanIndex.basePeakMz);
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, basePeakIntensities, basePeakMzs, cvs);
                    compress(spectrumGroup, ts);
                    converter.addToIndex(index, ts);
                }
            }
        }

        public override void compressMS2(Converter converter, List<MsIndex> ms2List, BlockIndex index)
        {
            int layers = (int) Math.Pow(2, digit); //计算堆叠层数
            int iter = ms2List.Count % layers == 0 ? (ms2List.Count / layers) : (ms2List.Count / layers + 1); //计算循环周期

            if (multiThread)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, iter, (i, ParallelLoopState) =>
                {
                    List<double> rts = new List<double>();
                    List<int> nums = new List<int>();
                    List<long> tics = new List<long>();
                    List<double> basePeakIntensities = new List<double>();
                    List<double> basePeakMzs = new List<double>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= ms2List.Count)
                        {
                            break;
                        }

                        MsIndex scanIndex = ms2List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        tics.Add(scanIndex.tic);
                        basePeakIntensities.Add(scanIndex.basePeakIntensity);
                        basePeakMzs.Add(scanIndex.basePeakMz);
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, basePeakIntensities, basePeakMzs, cvs);
                    compress(spectrumGroup, ts);
                    table.Add(i, ts);
                });
                converter.writeToFile(table, index);
            }
            else
            {
                for (int i = 0; i < iter; i++)
                {
                    List<double> rts = new List<double>();
                    List<int> nums = new List<int>();
                    List<long> tics = new List<long>();
                    List<double> basePeakIntensities = new List<double>();
                    List<double> basePeakMzs = new List<double>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= ms2List.Count)
                        {
                            break;
                        }

                        MsIndex scanIndex = ms2List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        tics.Add(scanIndex.tic);
                        basePeakIntensities.Add(scanIndex.basePeakIntensity);
                        basePeakMzs.Add(scanIndex.basePeakMz);
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, basePeakIntensities, basePeakMzs, cvs);
                    compress(spectrumGroup, ts);
                    converter.addToIndex(index, ts);
                }
            }
        }

        public override void compress(Spectrum spectrum, TempScan ts)
        {
            throw new NotImplementedException();
        }

        public override void compressMobility(Spectrum spectrum, TempScan ts)
        {
            throw new NotImplementedException();
        }

        //Compress for Stack-ZDPD
        public void compress(List<Spectrum> spectrumGroup, TempScanSZDPD ts)
        {
            List<int[]> mzListGroup = new List<int[]>();
            List<float> intListAllGroup = new List<float>(); //Intensity数组会直接合并为一个数组

            for (int i = 0; i < spectrumGroup.Count; i++)
            {
                BinaryDataDouble mzData = spectrumGroup[i].getMZArray().data;
                BinaryDataDouble intData = spectrumGroup[i].getIntensityArray().data;
                List<float> intensityList = new List<float>();
                var dataCount = mzData.Count;
                int[] mzArray = new int[dataCount];
                int j = 0;
                for (int t = 0; t < mzData.Count; t++)
                {
                    if (ignoreZero && intData[t] == 0) continue;
                    mzArray[j] = Convert.ToInt32(mzData[t] * mzPrecision);
                    intensityList.Add(Convert.ToSingle(Math.Round(intData[t], 1))); //精确到小数点后一位
                    j++;
                }

                //空光谱的情况下会填充一个mz=0,intensity=0的点
                if (j == 0)
                {
                    mzListGroup.Add(new int[] {0});
                    intensityList.Add(0);
                }
                else
                {
                    int[] mzSubArray = new int[j];
                    Array.Copy(mzArray, mzSubArray, j);
                    mzListGroup.Add(mzSubArray);
                }

                //说明是一帧空光谱,那么直接在Aird文件中抹除这一帧的信息
                intListAllGroup.AddRange(intensityList);
            }

            // Layers layers = StackLayer.encode(mzListGroup, mzListGroup.Count == Math.Pow(2, digit));
            Layers layers = StackLayer.encode(mzListGroup, mzIntComp, intByteComp);
            // List<int[]> temp = StackCompressUtil.stackDecode(layers);
            //使用SZDPD对mz进行压缩
            ts.mzArrayBytes = layers.mzArray;
            ts.tagArrayBytes = layers.tagArray;
            ts.intArrayBytes = intByteComp.encode(ByteTrans.floatToByte(intListAllGroup.ToArray()));
        }
    }
}