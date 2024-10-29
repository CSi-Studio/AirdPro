using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains.Common;
using AirdPro.Domains;
using AirdSDK.Compressor;
using AirdSDK.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirdSDK.Beans;
using Spectrum = AirdPro.csimzMLParser.mzml.Spectrum;
using AirdPro.Utils;
using DataUtil = AirdPro.Utils.imzml.DataUtil;

namespace AirdPro.Algorithms.Compressor
{
    public class ImzMLComp
    {
        private static readonly object Locker = new object();
        public int MzPrecision = 100000;
        public bool IgnoreZero = true;
        public SortedIntComp MzIntComp;
        public ByteComp MzByteComp;
        public IntComp IntIntComp;
        public ByteComp IntByteComp;
        public IntComp MobiIntComp;
        public ByteComp MobiByteComp;
        public SortedIntComp RtIntComp4Chroma;
        public ByteComp RtByteComp4Chroma;

        public Dictionary<double, int> MobiDict;
        public int IntensityPrecision = 1;
        public int RtPrecision = 100000;

        public ImzMLComp(Converter converter)
        {
            MzPrecision = converter.JobInfo.config.mzPrecision;
            IgnoreZero = converter.JobInfo.config.ignoreZeroIntensity;
        }

        public void CompressMS1(ImzMLConverter converter, BlockIndex index)
        {
            //仅当面向Search的Aird模式下有效
            ConcurrentBag<TempSpectrum> spectra = new ConcurrentBag<TempSpectrum>();

            Hashtable ms1Table = Hashtable.Synchronized(new Hashtable());
            int process = 0;

            //使用多线程处理数据提取与压缩
            //Step1. MS1数据预处理
            Parallel.For(0, converter.Ms1List.Count, i =>
            {
                Interlocked.Increment(ref process);
                converter.JobInfo.Log(null, Tag.progress(Tag.MS1, process, converter.Ms1List.Count));
                MsIndex ms1Index = converter.Ms1List[i];
                TempScan ts = new TempScan(ms1Index);
                Spectrum spectrum = null;
                try
                {
                    //部分情况下读取Spectrum是不能并行的，需要加锁以避免异步读取错误
                    lock (Locker)
                    {
                        spectrum = converter.spectrumList.GetSpectrum(ts.num);
                    }

                    switch (converter.JobInfo.config.engine)
                    {
                        case (int)AirdEngine.RowCompression:
                            if (converter.JobInfo.ionMobility)
                            {
                                CompressMobility(spectrum, ts);
                            }
                            else
                            {
                                Compress(spectrum, ts);
                            }

                            break;

                        case (int)AirdEngine.ColumnCompression:
                            TempSpectrum tempSpectrum = ReadSpectrum(spectrum);
                            tempSpectrum.rt = ts.rt;
                            spectra.Add(tempSpectrum); //面向搜索场景下暂时还不支持离子淌度文件
                            break;
                    }

                    ms1Table.Add(i, ts);
                }
                finally
                {
                    spectrum = null;
                }
            });
            converter.WriteToFile(ms1Table, index);

            
        }

        public void Compress(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.GetMzArray();
            double[] intData = spectrum.GetIntensityArray();

            var size = mzData.Length;
            if (size == 0)
            {
                ts.mzArrayBytes = Array.Empty<byte>();
                ts.intArrayBytes = Array.Empty<byte>();
                return;
            }

            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int j = 0;
            for (int t = 0; t < size; t++)
            {
                if (IgnoreZero && intData[t] == 0) continue;
                mzArray[j] = DataUtil.FetchMz(mzData[t], MzPrecision);
                intensityArray[j] = DataUtil.FetchIntensity(intData[t], IntensityPrecision);
                j++;
            }

            int[] mzSubArray = new int[j];
            Array.Copy(mzArray, mzSubArray, j);
            int[] intensitySubArray = new int[j];
            Array.Copy(intensityArray, intensitySubArray, j);

            byte[] compressedMzArray = null;
            byte[] compressedIntArray = null;

            if (mzSubArray.Length == 0)
            {
                compressedMzArray = new byte[0];
            }
            else
            {
                compressedMzArray = MzByteComp.encode(AirdProUtil.IntToByte(MzIntComp.encode(mzSubArray)));
            }

            if (intensitySubArray.Length == 0)
            {
                compressedIntArray = new byte[0];
            }
            else
            {
                compressedIntArray = IntByteComp.encode(AirdProUtil.IntToByte(IntIntComp.encode(intensitySubArray)));
            }

            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
        }

        public TempSpectrum ReadSpectrum(Spectrum spectrum)
        {
            double[] mzData = spectrum.GetMzArray();
            double[] intData = spectrum.GetIntensityArray();
            var size = mzData.Length;
            if (size == 0)
            {
                return new TempSpectrum(Array.Empty<int>(), Array.Empty<int>());
            }

            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int j = 0;
            int lastIndex = 0;
            int lastMz = 0;
            for (int t = 0; t < size; t++)
            {
                if (IgnoreZero && intData[t] == 0) continue;
                int currentMz = DataUtil.FetchMz(mzData[t], MzPrecision);
                if (lastMz == currentMz)
                {
                    intensityArray[lastIndex] += DataUtil.FetchIntensity(intData[t], IntensityPrecision);
                    continue;
                }
                else
                {
                    mzArray[j] = currentMz;
                    intensityArray[j] = DataUtil.FetchIntensity(intData[t], IntensityPrecision);
                    lastIndex = t;
                    lastMz = currentMz;
                }

                j++;
            }

            int[] mzSubArray = new int[j];
            Array.Copy(mzArray, mzSubArray, j);
            int[] intensitySubArray = new int[j];
            Array.Copy(intensityArray, intensitySubArray, j);
            return new TempSpectrum(mzSubArray, intensitySubArray);
        }

        public void CompressMobility(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.GetMzArray();
            double[] intData = spectrum.GetIntensityArray();
            double[] mobiData = DataUtil.GetMobilityData(spectrum);
            

            var size = mzData.Length;
            if (size == 0)
            {
                ts.mzArrayBytes = Array.Empty<byte>();
                ts.intArrayBytes = Array.Empty<byte>();
                ts.mobilityArrayBytes = Array.Empty<byte>();
                return;
            }

            List<TimsData> dataList = new List<TimsData>();
            for (int t = 0; t < size; t++)
            {
                dataList.Add(new TimsData(MobiDict[mobiData[t]], mzData[t], intData[t]));
            }

            List<TimsData> sortedDataList = dataList.OrderBy(d => d.mz).ThenBy(d => d.mobilityNo).ToList();

            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int[] mobilityNoArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                mzArray[i] = DataUtil.FetchMz(sortedDataList[i].mz, MzPrecision);
                intensityArray[i] = DataUtil.FetchIntensity(sortedDataList[i].intensity, IntensityPrecision);
                mobilityNoArray[i] = sortedDataList[i].mobilityNo;
            }

            byte[] compressedMzArray = ComboComp.encode(MzIntComp, MzByteComp, mzArray);
            byte[] compressedIntArray = ComboComp.encode(IntIntComp, IntByteComp, intensityArray);
            byte[] compressedMobilityArray = ComboComp.encode(MobiIntComp, MobiByteComp, mobilityNoArray);
            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
            ts.mobilityArrayBytes = compressedMobilityArray;
        }
        
    }
}
