/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirdPro.Converters;
using AirdPro.DomainsCore.Aird;
using AirdPro.Utils;
using pwiz.CLI.msdata;
using pwiz.CLI.util;

namespace AirdPro.Algorithms
{
    public class StackZDPD : ICompressor
    {
        public StackZDPD(IConverter converter) : base(converter){}

        override 
        public void compressMS1(IConverter converter, BlockIndex index)
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
                    List<long> tics = new List<long>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= converter.ms1List.Count)
                        {
                            break;
                        }
                        MsIndex scanIndex = converter.ms1List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        tics.Add(scanIndex.tic);
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, cvs);

                    compress(spectrumGroup, ts);
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
                    List<long> tics = new List<long>();
                    List<List<CV>> cvs = new List<List<CV>>();
                    List<Spectrum> spectrumGroup = new List<Spectrum>();
                    for (int k = 0; k < layers; k++)
                    {
                        int realNum = i * layers + k;
                        if (realNum >= converter.ms1List.Count)
                        {
                            break;
                        }
                        MsIndex scanIndex = converter.ms1List[realNum];
                        rts.Add(scanIndex.rt);
                        nums.Add(scanIndex.num);
                        tics.Add(scanIndex.tic);
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }

                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, cvs);
                    compress(spectrumGroup, ts);
                    converter.addToIndex(index, ts);
                }
            }
        }

        override
        public void compressMS2(IConverter converter, List<MsIndex> ms2List, BlockIndex index)
        {
            int layers = (int)Math.Pow(2, converter.jobInfo.jobParams.digit); //计算堆叠层数
            int iter = ms2List.Count % layers == 0 ? (ms2List.Count / layers) : (ms2List.Count / layers + 1); //计算循环周期

            if (converter.jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, iter, (i, ParallelLoopState) =>
                {
                    List<float> rts = new List<float>();
                    List<int> nums = new List<int>();
                    List<long> tics = new List<long>();
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
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }
                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, cvs);
                    compress(spectrumGroup, ts);
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
                    List<long> tics = new List<long>();
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
                        cvs.Add(scanIndex.cvList);
                        spectrumGroup.Add(converter.spectrumList.spectrum(scanIndex.num, true));
                    }
                    TempScanSZDPD ts = new TempScanSZDPD(nums, rts, tics, cvs);
                    compress(spectrumGroup, ts);
                    converter.addToIndex(index, ts);
                }
            }
        }

        public override void compress(Spectrum spectrum, TempScan ts)
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
                    mzListGroup.Add(new int[] { 0 });
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

            Layers layers = StackLayer.encode(mzListGroup, mzListGroup.Count == Math.Pow(2, digit));
            // List<int[]> temp = StackCompressUtil.stackDecode(layers);
            //使用SZDPD对mz进行压缩
            ts.mzArrayBytes = layers.mzArray;
            ts.tagArrayBytes = layers.tagArray;
            ts.intArrayBytes = Zlib.encode(intListAllGroup.ToArray());
        }
    }
}