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
using AirdPro.Converters;
using AirdPro.DomainsCore.Aird;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Compress;
using pwiz.CLI.msdata;
using pwiz.CLI.util;

namespace AirdPro.Algorithms
{
    public class ZDPD : ICompressor
    {
        public ZDPD(IConverter converter) : base(converter)
        {
        }

        public override void compressMS1(IConverter converter, BlockIndex index)
        {
            MsIndex[] ms1List = converter.ms1List.ToArray();
            if (multiThread)
            {
                Hashtable ms1Table = Hashtable.Synchronized(new Hashtable());
                int process = 0;
                //使用多线程处理数据提取与压缩
                Parallel.For(0, converter.ms1List.Count, (i, ParallelLoopState) =>
                {
                    Interlocked.Increment(ref process);
                    converter.jobInfo.log(null, "MS1:" + process + "/" + converter.ms1List.Count);
                    MsIndex ms1Index = ms1List[i];
                    TempScan ts = new TempScan(ms1Index.num, ms1Index.rt, ms1Index.tic, ms1Index.cvList);
                    if (converter.jobInfo.ionMobility)
                    {
                        compressMobility(converter.spectrumList, ms1Index, ts);
                    }
                    else
                    {
                        compress(converter.spectrumList.spectrum(ts.num, true), ts);
                    }
                    ms1Table.Add(i, ts);
                });
                converter.writeToFile(ms1Table, index);
            }
            else
            {
                for (int i = 0; i < converter.ms1List.Count; i++)
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + converter.ms1List.Count);
                    MsIndex ms1Index = ms1List[i];
                    TempScan ts = new TempScan(ms1Index.num, ms1Index.rt, ms1Index.tic, ms1Index.cvList);
                    if (converter.jobInfo.ionMobility)
                    {
                        compressMobility(converter.spectrumList, ms1Index, ts);
                    }
                    else
                    {
                        compress(converter.spectrumList.spectrum(ts.num, true), ts);
                    }
                    converter.addToIndex(index, ts);
                }
            }
        }

        public override void compressMS2(IConverter converter, List<MsIndex> ms2List, BlockIndex index)
        {
            if (multiThread)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, ms2List.Count, (i, ParallelLoopState) =>
                {
                    MsIndex ms2Index = ms2List[i];
                    TempScan ts = new TempScan(ms2Index.num, ms2Index.rt, ms2Index.tic, ms2Index.cvList);
                    if (converter.jobInfo.ionMobility)
                    {
                        compressMobility(converter.spectrumList, ms2Index, ts);
                    }
                    else
                    {
                        compress(converter.spectrumList.spectrum(ts.num, true), ts);
                    }
                    table.Add(i, ts);
                });
                converter.writeToFile(table, index);
            }
            else
            {
                foreach (MsIndex ms2Index in ms2List)
                {
                    TempScan ts = new TempScan(ms2Index.num, ms2Index.rt, ms2Index.tic, ms2Index.cvList);
                    // compress(converter.spectrumList.spectrum(ms2Index.num, true), ts);
                    if (converter.jobInfo.ionMobility)
                    {
                        compressMobility(converter.spectrumList, ms2Index, ts);
                    }
                    else
                    {
                        compress(converter.spectrumList.spectrum(ts.num, true), ts);
                    }
                    converter.addToIndex(index, ts);
                }
            }
        }

        public override void compress(Spectrum spectrum, TempScan ts)
        {
            BinaryDataDouble mzData = spectrum.getMZArray().data;
            BinaryDataDouble intData = spectrum.getIntensityArray().data;
            var size = mzData.Count;
            if (size == 0)
            {
                return;
            }

            int[] mzArray = new int[size];
            float[] intensityArray = new float[size];
            int j = 0;
            for (int t = 0; t < size; t++)
            {
                if (ignoreZero && intData[t] == 0) continue;
                mzArray[j] = Convert.ToInt32(mzData[t] * mzPrecision);
                intensityArray[j] = Convert.ToSingle(Math.Round(intData[t], 1)); //精确到小数点后一位
                j++;
            }

            int[] mzSubArray = new int[j];
            Array.Copy(mzArray, mzSubArray, j);
            float[] intensitySubArray = new float[j];
            Array.Copy(intensityArray, intensitySubArray, j);

            int[] compressedMzSubArray = BinPacking.encode(mzSubArray);
            byte[] compressedIntArray = Zlib.encode(intensitySubArray);

            ts.mzArrayBytes = Zlib.encode(compressedMzSubArray);
            ts.intArrayBytes = compressedIntArray;
        }

        public void compressMobility(SpectrumList spectrumList, MsIndex msIndex, TempScan ts)
        {
            List<int> scanNums = msIndex.scanNums;
            Spectrum frame = new Spectrum();
            List<TimsData> dataList = new List<TimsData>();
            int totalSize = 0;
            for (var i = 0; i < scanNums.Count; i++)
            {
                float mobility = msIndex.mobilities[i];
                Spectrum spectrum = spectrumList.spectrum(i, true);
                BinaryDataDouble mzData = spectrum.getMZArray().data;
                BinaryDataDouble intData = spectrum.getIntensityArray().data;
                var size = mzData.Count;
                if (size == 0)
                {
                    continue;
                }
                totalSize += size;
                for (int t = 0; t < size; t++)
                {
                    dataList.Add(new TimsData(mobility, mzData[t], intData[t]));
                }

                dataList.Sort(delegate(TimsData x, TimsData y)
                {
                    if (x.mz > y.mz)
                        return 1;
                    else
                        return -1;
                });
            }

            int[] mzArray = new int[totalSize];
            float[] intensityArray = new float[totalSize];
            float[] mobilityArray = new float[totalSize];
            
            for (int i = 0; i < totalSize; i++)
            {
                mzArray[i] = Convert.ToInt32(dataList[i].mz * mzPrecision);
                intensityArray[i] = Convert.ToSingle(Math.Round(dataList[i].intensity, 1)); //精确到小数点后一位
                mobilityArray[i] = Convert.ToInt32(dataList[i].mobility * mzPrecision); //精确到小数点后一位
            }

            int[] compressedMzArray = BinPacking.encode(mzArray);
            byte[] compressedIntArray = Zlib.encode(intensityArray);
            byte[] compressedMobilityArray = Zlib.encode(mobilityArray);

            ts.mzArrayBytes = Zlib.encode(compressedMzArray);
            ts.intArrayBytes = compressedIntArray;
            ts.mobilityArrayBytes = compressedMobilityArray;
        }
    }
}