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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Compress;
using CSharpFastPFOR.Port;
using pwiz.CLI.msdata;
using pwiz.CLI.util;
using ThermoFisher.CommonCore.Data.Interfaces;

namespace AirdPro.Algorithms
{
    public class CoreComp : ICompressor
    {
        private static readonly object locker = new object();
        private static readonly object locker2 = new object();
        public CoreComp(IConverter converter) : base(converter)
        {
        }
        ConcurrentDictionary<float, int> mobiDict = new ConcurrentDictionary<float, int>();
        private int id=-1;
        public override void compressMS1(IConverter converter, BlockIndex index)
        {
            if (multiThread)
            {
                Hashtable ms1Table = Hashtable.Synchronized(new Hashtable());
                int process = 0;
                
                //使用多线程处理数据提取与压缩
                Parallel.For(0, converter.ms1List.Count, (i, ParallelLoopState) =>
                {
                    Interlocked.Increment(ref process);
                    converter.jobInfo.log(null, "MS1:" + process + "/" + converter.ms1List.Count); 
                    MsIndex ms1Index = converter.ms1List[i];
                    TempScan ts = new TempScan(ms1Index.num, ms1Index.rt, ms1Index.tic, ms1Index.cvList);
                    Spectrum spectrum;
                    lock (locker)
                    {
                        spectrum = converter.spectrumList.spectrum(ts.num, true);
                    }

                    if (converter.jobInfo.ionMobility)
                    {
                        compressMobility(spectrum, ts);
                    }
                    else
                    {
                        compress(spectrum, ts);
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
                    MsIndex ms1Index = converter.ms1List[i];
                    TempScan ts = new TempScan(ms1Index.num, ms1Index.rt, ms1Index.tic, ms1Index.cvList);
                    using (var spectrum = converter.spectrumList.spectrum(ts.num, true))
                    {
                        if (converter.jobInfo.ionMobility)
                        {
                            HashSet<int> mobilities = compressMobility(spectrum, ts);
                            lock (locker2)
                            {
                                totalMobilities.UnionWith(mobilities);
                            }
                        }
                        else
                        {
                            compress(spectrum, ts);
                        }
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
                    Spectrum spectrum;
                    lock (locker)
                    {
                        spectrum = converter.spectrumList.spectrum(ts.num, true);
                    }

                    if (converter.jobInfo.ionMobility)
                    {
                        compressMobility(spectrum, ts);
                    }
                    else
                    {
                        compress(spectrum, ts);
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
                    using (var spectrum = converter.spectrumList.spectrum(ts.num, true))
                    {
                        if (converter.jobInfo.ionMobility)
                        {
                            compressMobility(spectrum, ts);
                        }
                        else
                        {
                            compress(spectrum, ts);
                        }
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
                ts.mzArrayBytes = new byte[0];
                ts.intArrayBytes = new byte[0];
                return;
            }

            int[] mzArray = new int[size];
            float[] intensityArray = new float[size];
            int j = 0;
            for (int t = 0; t < size; t++)
            {
                if (ignoreZero && intData[t] == 0) continue;
                mzArray[j] = Convert.ToInt32(mzData[t] * mzPrecision);
                intensityArray[j] = getIntensity(intData[t]);
                j++;
            }

            int[] mzSubArray = new int[j];
            Array.Copy(mzArray, mzSubArray, j);
            float[] intensitySubArray = new float[j];
            Array.Copy(intensityArray, intensitySubArray, j);

            int[] compressedMzSubArray = mzIntComp.encode(mzSubArray);
            byte[] compressedIntArray = intByteComp.encode(ByteTrans.floatToByte(intensitySubArray));

            ts.mzArrayBytes = mzByteComp.encode(ByteTrans.intToByte(compressedMzSubArray));
            ts.intArrayBytes = compressedIntArray;
        }

        public HashSet<float> compressMobility(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
            double[] mobiData = spectrum.getArrayByCVID(pwiz.CLI.cv.CVID.MS_mean_ion_mobility_drift_time_array)?.data
                                    .Storage() ??
                                spectrum.getArrayByCVID(pwiz.CLI.cv.CVID.MS_mean_inverse_reduced_ion_mobility_array)
                                    ?.data.Storage() ??
                                spectrum.getArrayByCVID(pwiz.CLI.cv.CVID.MS_raw_ion_mobility_array)?.data.Storage() ??
                                spectrum.getArrayByCVID(pwiz.CLI.cv.CVID.MS_raw_inverse_reduced_ion_mobility_array)
                                    ?.data.Storage();


            var size = mzData.Length;
            TimsData[] dataArray = new TimsData[size];
            for (int t = 0; t < size; t++)
            {
                dataArray[t] = new TimsData(mobiData[t], mzData[t], intData[t]);
            }
            Array.Sort(dataArray, (p1, p2) => p1.mz.CompareTo(p2.mz));
            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            float[] mobilityArray = new float[size];
            for (int i = 0; i < size; i++)
            {
                mzArray[i] = Convert.ToInt32(dataArray[i].mz * mzPrecision);
                intensityArray[i] = getIntensity(dataArray[i].intensity);
                mobilityArray[i] = (float) (dataArray[i].mobility);
            }

            byte[] compressedMzArray = mzByteComp.encode(ByteTrans.intToByte(mzIntComp.encode(mzArray)));
            byte[] compressedIntArray = intByteComp.encode(ByteTrans.intToByte(intIntComp.encode(intensityArray)));
            byte[] compressedMobilityArray = mobiByteComp.encode(ByteTrans.floatToByte(mobilityArray));

            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
            ts.mobilityArrayBytes = compressedMobilityArray;

            return new HashSet<float>(mobilityArray);
        }

        private int getIntensity(double target)
        {
            int result;
            try
            {
                result = Convert.ToInt32(Math.Round(target)); //精确到小数点后一位
            }
            catch (Exception e)
            {
                //超出Integer可以表达的最大值,使用-log2进行转换
                result = -Convert.ToInt32(Math.Log(target) / Math.Log(2) * 10000);
                Console.WriteLine("出现一个超级值:" + target + ",转换后为:" + result);
            }

            return result;
        }
    }
}