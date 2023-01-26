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
using AirdPro.Converters;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Utils;
using AirdSDK.Beans;
using AirdSDK.Compressor;
using pwiz.CLI.msdata;
using pwiz.CLI.tradata;

namespace AirdPro.Algorithms
{
    public class CoreComp : ICompressor
    {
        private static readonly object locker = new object();

        public CoreComp(Converter converter) : base(converter)
        {
        }

        public override void compressMS1(Converter converter, BlockIndex index)
        {
            if (multiThread)
            {
                Hashtable ms1Table = Hashtable.Synchronized(new Hashtable());
                int process = 0;

                //使用多线程处理数据提取与压缩
                Parallel.For(0, converter.ms1List.Count, (i, ParallelLoopState) =>
                {
                    Interlocked.Increment(ref process);
                    converter.jobInfo.log(null, Tag.progress(Tag.MS1, process, converter.ms1List.Count));
                    MsIndex ms1Index = converter.ms1List[i];
                    TempScan ts = new TempScan(ms1Index);
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
                    converter.jobInfo.log(null, Tag.progress(Tag.MS1, i, converter.ms1List.Count));
                    MsIndex ms1Index = converter.ms1List[i];
                    TempScan ts = new TempScan(ms1Index);
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

        public override void compressMS2(Converter converter, List<MsIndex> ms2List, BlockIndex index)
        {
            if (multiThread)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, ms2List.Count, (i, ParallelLoopState) =>
                {
                    MsIndex ms2Index = ms2List[i];
                    TempScan ts = new TempScan(ms2Index);
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
                    TempScan ts = new TempScan(ms2Index);
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

        public override void compress(Chromatogram chromatogram, TempScanChroma ts)
        {
            double[] rtData = chromatogram.getTimeArray().data.Storage();
            double[] intData = chromatogram.getIntensityArray().data.Storage();
            var size = rtData.Length;
            if (size == 0)
            {
                ts.rtArrayBytes = new byte[0];
                ts.intArrayBytes = new byte[0];
                return;
            }

            int[] rtArray = new int[size];
            int[] intensityArray = new int[size];
            for (int t = 0; t < size; t++)
            {
                rtArray[t] = DataUtil.fetchRt(rtArray[t]);
                intensityArray[t] = DataUtil.fetchIntensity(intData[t], 1);
            }

            byte[] compressedRtArray = rtByteComp4Chroma.encode(ByteTrans.intToByte(rtIntComp4Chroma.encode(rtArray)));
            byte[] compressedIntArray = intByteComp4Chroma.encode(ByteTrans.intToByte(intIntComp4Chroma.encode(intensityArray)));

            ts.rtArrayBytes = compressedRtArray;
            ts.intArrayBytes = compressedIntArray;
        }

        public override void compress(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
            var size = mzData.Length;
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
                mzArray[j] = DataUtil.fetchMz(mzData[t], mzPrecision);
                // intensityArray[j] = Convert.ToInt32(Math.Log(intData[t]) / Math.Log(2) * 100);
                intensityArray[j] = DataUtil.fetchIntensity(intData[t], intensityPrecision);
                j++;
            }

            int[] mzSubArray = new int[j];
            Array.Copy(mzArray, mzSubArray, j);
            int[] intensitySubArray = new int[j];
            Array.Copy(intensityArray, intensitySubArray, j);

            byte[] compressedMzArray = ComboComp.encode(mzIntComp, mzByteComp, mzSubArray);
            byte[] compressedIntArray = ComboComp.encode(intIntComp, intByteComp, intensitySubArray);

            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
        }

        public override void compressMobility(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
            double[] mobiData = DataUtil.getMobilityData(spectrum);

            var size = mzData.Length;
            if (size == 0)
            {
                ts.mzArrayBytes = new byte[0];
                ts.intArrayBytes = new byte[0];
                ts.mobilityArrayBytes = new byte[0];
                return;
            }

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
                mzArray[i] = DataUtil.fetchMz(dataArray[i].mz, mzPrecision);
                intensityArray[i] = DataUtil.fetchIntensity(dataArray[i].intensity, intensityPrecision);
                mobilityNoArray[i] = dataArray[i].mobilityNo;
            }

            byte[] compressedMzArray = ComboComp.encode(mzIntComp, mzByteComp, mzArray);
            byte[] compressedIntArray = ComboComp.encode(intIntComp, intByteComp, intensityArray);
            byte[] compressedMobilityArray = ComboComp.encode(mobiIntComp, mobiByteComp, mobilityNoArray);
            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
            ts.mobilityArrayBytes = compressedMobilityArray;
        }
    }


}