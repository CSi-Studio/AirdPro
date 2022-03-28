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
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using pwiz.CLI.util;

namespace AirdPro.Algorithms
{
    public class CoreComp : ICompressor
    {
        private static readonly object locker = new object();
        public CoreComp(IConverter converter) : base(converter)
        {
        }

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
            int[] intensityArray = new int[size];
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
            int[] intensitySubArray = new int[j];
            Array.Copy(intensityArray, intensitySubArray, j);

            byte[] compressedMzArray = mzByteComp.encode(ByteTrans.intToByte(mzIntComp.encode(mzSubArray)));
            byte[] compressedIntArray = intByteComp.encode(ByteTrans.intToByte(intIntComp.encode(intensitySubArray)));
            
            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
        }

        public void compressMobility(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
            double[] mobiData = IConverter.getMobilityData(spectrum);

            var size = mzData.Length;
            TimsData[] dataArray = new TimsData[size];
            for (int t = 0; t < size; t++)
            {
                dataArray[t] = new TimsData(mobiDict[mobiData[t]], mzData[t], intData[t]);
            }
            Array.Sort(dataArray, (p1, p2) => p1.mz.CompareTo(p2.mz));
            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int[] mobilityNoArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                mzArray[i] = Convert.ToInt32(dataArray[i].mz * mzPrecision);
                intensityArray[i] = getIntensity(dataArray[i].intensity);
                mobilityNoArray[i] = dataArray[i].mobilityNo;
            }

            byte[] compressedMzArray = mzByteComp.encode(ByteTrans.intToByte(mzIntComp.encode(mzArray)));
            byte[] compressedIntArray = intByteComp.encode(ByteTrans.intToByte(intIntComp.encode(intensityArray)));
            byte[] compressedMobilityArray = mobiByteComp.encode(ByteTrans.intToByte(mobiIntComp.encode(mobilityNoArray)));

            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
            ts.mobilityArrayBytes = compressedMobilityArray;
        }

        private int getIntensity(double target)
        {
            int result;
            try
            {
                result = Convert.ToInt32(Math.Round(target*intensityPrecision)); //精确到小数点后一位
            }
            catch (Exception e)
            {
                //超出Integer可以表达的最大值,使用-log2进行转换,保留5位有效数字
                result = -Convert.ToInt32(Math.Log(target* intensityPrecision) / Math.Log(2) * 100000);
                Console.WriteLine("A Huge Integer Appears:" + target + ", after conversion:" + result);
            }

            return result;
        }
    }
}