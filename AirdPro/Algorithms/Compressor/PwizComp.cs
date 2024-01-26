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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains;
using AirdPro.Domains.Common;
using AirdPro.Utils;
using AirdSDK.Beans;
using AirdSDK.Beans.Common;
using AirdSDK.Compressor;
using AirdSDK.Enums;
using AirdSDK.Utils;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra.Storage;
using pwiz.CLI.msdata;
using Spectrum = pwiz.CLI.msdata.Spectrum;
using Control = MathNet.Numerics.Control;

namespace AirdPro.Algorithms.Compressor
{
    public class PwizComp(Converter converter) : ICompressor(converter)
    {
        private static readonly object Locker = new object();

        public override void CompressMS1(PwizConverter converter, BlockIndex index)
        {
            //仅当面向Search的Aird模式下有效
            ConcurrentDictionary<double, TempSpectrum> msDictionary =
                new ConcurrentDictionary<double, TempSpectrum>();
            ConcurrentBag<TempSpectrum> spectra = new ConcurrentBag<TempSpectrum>();
            
            Hashtable ms1Table = Hashtable.Synchronized(new Hashtable());
            int process = 0;

            //使用多线程处理数据提取与压缩
            //Step1. MS1数据预处理
            Parallel.For(0, converter.Ms1List.Count, i =>
            {
                Interlocked.Increment(ref process);
                converter.JobInfo.log(null, Tag.progress(Tag.MS1, process, converter.Ms1List.Count));
                MsIndex ms1Index = converter.Ms1List[i];
                TempScan ts = new TempScan(ms1Index);
                Spectrum spectrum = null;
                try
                {
                    //部分情况下读取Spectrum是不能并行的，需要加锁以避免异步读取错误
                    lock (Locker)
                    {
                        spectrum = converter.SpectrumList.spectrum(ts.num, true);
                    }

                    switch (converter.JobInfo.config.scene)
                    {
                        case "Computation":
                            if (converter.JobInfo.ionMobility)
                            {
                                CompressMobility(spectrum, ts);
                            }
                            else
                            {
                                Compress(spectrum, ts);
                            }

                            break;

                        case "Search":
                            //面向搜索场景下暂时还不支持离子淌度文件
                            spectra.Add(ReadSpectrum(spectrum));
                            // msDictionary[ts.rt] = ReadSpectrum(spectrum);
                            break;
                    }

                    ms1Table.Add(i, ts);
                }
                finally
                {
                    spectrum?.Dispose();
                }
            });
            converter.WriteToFile(ms1Table, index);

            //如果是面向搜索的格式转换，则msRowTable不为空，准备启动行矩阵向列矩阵转换的过程
            if (converter.JobInfo.config.IsSearch())
            {
                ColumnIndex columnIndex = new ColumnIndex();
                columnIndex.level = 1;
                ConcurrentDictionary<int, ByteColumn> compressedColumns = null;
                
                compressedColumns = CompressAsColumnMatrix(converter, spectra, columnIndex);
                converter.WriteColumnData(compressedColumns, columnIndex);
            }
        }

        public override void CompressMS2(PwizConverter converter, List<MsIndex> ms2List, BlockIndex index)
        {
            //仅当面向Search的Aird模式下有效
            ConcurrentDictionary<double, TempSpectrum> msDictionary =
                new ConcurrentDictionary<double, TempSpectrum>();
            ConcurrentBag<TempSpectrum> spectra = new ConcurrentBag<TempSpectrum>();
            
            Hashtable table = Hashtable.Synchronized(new Hashtable());
            //使用多线程处理数据提取与压缩
            Parallel.For(0, ms2List.Count, (i) =>
            {
                MsIndex ms2Index = ms2List[i];
                TempScan ts = new TempScan(ms2Index);
                Spectrum spectrum = null;
                try
                {
                    lock (Locker)
                    {
                        spectrum = converter.SpectrumList.spectrum(ts.num, true);
                    }

                    switch (converter.JobInfo.config.scene)
                    {
                        case "Computation":
                            if (converter.JobInfo.ionMobility)
                            {
                                CompressMobility(spectrum, ts);
                            }
                            else
                            {
                                Compress(spectrum, ts);
                            }

                            break;

                        case "Search":
                            switch (converter.JobInfo.type)
                            {
                                case AcquisitionMethod.DIA:
                                    spectra.Add(ReadSpectrum(spectrum));
                                    // msDictionary[ts.rt] = ReadSpectrum(spectrum);
                                    break;
                                case AcquisitionMethod.DDA:
                                    Compress(spectrum, ts);
                                    break;
                            }

                            break;
                    }

                    table.Add(i, ts);
                }
                finally
                {
                    spectrum?.Dispose();
                }
            });
            converter.WriteToFile(table, index);

            //如果是面向搜索引擎的格式转换，则msRowTable不为空，准备启动行矩阵向列矩阵转换的过程
            if (converter.JobInfo.config.IsSearch() &&
                converter.JobInfo.type.Equals(AcquisitionMethod.DIA))
            {
                ColumnIndex columnIndex = new ColumnIndex();
                columnIndex.level = 2;
                columnIndex.range = index.getWindowRange();
                ConcurrentDictionary<int, ByteColumn> compressedColumns = null;
               
                compressedColumns = CompressAsColumnMatrix(converter, spectra, columnIndex);
                converter.WriteColumnData(compressedColumns, columnIndex);
            }
        }

        public override void Compress(Chromatogram chromatogram, TempScanChroma ts)
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
                rtArray[t] = DataUtil.FetchRt(rtData[t]);
                intensityArray[t] = DataUtil.FetchIntensity(intData[t], 1);
            }

            byte[] compressedRtArray = RtByteComp4Chroma.encode(ByteTrans.intToByte(RtIntComp4Chroma.encode(rtArray)));
            byte[] compressedIntArray =
                IntByteComp4Chroma.encode(ByteTrans.intToByte(IntIntComp4Chroma.encode(intensityArray)));

            ts.rtArrayBytes = compressedRtArray;
            ts.intArrayBytes = compressedIntArray;
        }

        public override void Compress(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
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
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
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

        public override void CompressMobility(Spectrum spectrum, TempScan ts)
        {
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
            double[] mobiData = DataUtil.GetMobilityData(spectrum);

            var size = mzData.Length;
            if (size == 0)
            {
                ts.mzArrayBytes = Array.Empty<byte>();
                ts.intArrayBytes = Array.Empty<byte>();
                ts.mobilityArrayBytes = Array.Empty<byte>();
                return;
            }

            TimsData[] dataArray = new TimsData[size];
            for (int t = 0; t < size; t++)
            {
                dataArray[t] = new TimsData(MobiDict[mobiData[t]], mzData[t], intData[t]);
            }

            Array.Sort(dataArray, (p1, p2) => p1.mz.CompareTo(p2.mz));
            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int[] mobilityNoArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                mzArray[i] = DataUtil.FetchMz(dataArray[i].mz, MzPrecision);
                intensityArray[i] = DataUtil.FetchIntensity(dataArray[i].intensity, IntensityPrecision);
                mobilityNoArray[i] = dataArray[i].mobilityNo;
            }

            byte[] compressedMzArray = ComboComp.encode(MzIntComp, MzByteComp, mzArray);
            byte[] compressedIntArray = ComboComp.encode(IntIntComp, IntByteComp, intensityArray);
            byte[] compressedMobilityArray = ComboComp.encode(MobiIntComp, MobiByteComp, mobilityNoArray);
            ts.mzArrayBytes = compressedMzArray;
            ts.intArrayBytes = compressedIntArray;
            ts.mobilityArrayBytes = compressedMobilityArray;
        }

        /**
         * 将按光谱(即按行)存储的模式改为按列存储
         * 第一代野鸡算法，转换速度慢
         */
        public ConcurrentDictionary<int, ByteColumn> CompressAsColumnMatrix(PwizConverter converter,
            ConcurrentBag<TempSpectrum> spectra, ColumnIndex columnIndex)
        {
            converter.JobInfo.log(null, "Column Compressing");
            //矩阵横坐标
            // List<double> rts = rowTable.Keys.ToList();
            List<int> rtsInt = [];
            int totalPoints = 0;
            foreach (var spectrum in spectra)
            {
                rtsInt.Add((int)Math.Round(spectrum.rt * 1000));
                totalPoints += spectrum.mzs.Length;
            }
            
            HashSet<int> mzsSet = new HashSet<int>(totalPoints);
            foreach (TempSpectrum spectrum in spectra)
            {
                mzsSet.UnionWith(spectrum.mzs);
            }

            int[] totalMzs = mzsSet.ToArray();
        
            converter.JobInfo.log("合计光谱图" + spectra.Count + "张,不同质荷比共：" + totalMzs.Length + "个");
            converter.JobInfo.log("质荷比范围:" + totalMzs[0] + "-" + totalMzs[totalMzs.Length - 1]);
            converter.JobInfo.log("总计包含有效点数:" + totalPoints);

            long totalSize = 0;
            int step = 1;
            ConcurrentDictionary<int, ByteColumn> treeColumnCompressed = new ConcurrentDictionary<int, ByteColumn>();
            ConcurrentDictionary<int, Slice> treeColumn = new ConcurrentDictionary<int, Slice>();
            
            bool fastMode = converter.JobInfo.config.fastMode;
            foreach (var spectrum in spectra)
            {
                int currentStep = Interlocked.Increment(ref step);
                converter.JobInfo.log(null, Tag.percentage(Tag.Column_Trans, currentStep, spectra.Count));
                for (var i = 0; i < spectrum.mzs.Length; i++)
                {
                    treeColumn.GetOrAdd(spectrum.mzs[i], new Slice()).Add(spectrum.mzs[i], spectrum.intensities[i]);
                }
            }

            step = 0;
            // 并行化处理质荷比
            Parallel.ForEach(totalMzs, mz =>
            {
                int currentStep = Interlocked.Increment(ref step);
            
                if (currentStep % 100000 == 0)
                {
                    converter.JobInfo.log(null, Tag.percentage(Tag.Column_Compress, currentStep, totalMzs.Length));
                }

                Slice slice = treeColumn[mz];
                int length = slice.indexIdList.Count;
                byte[] compressedIndexIds = null;
                byte[] compressedInts = null;
                if (length > 4)
                {
                    if (fastMode)
                    {
                        compressedIndexIds =
                            AirdProUtil.IntToByte(
                                new IntegratedVarByteWrapper().encode(ArrayUtil.toIntArray(slice.indexIdList)));
                        compressedInts =
                            AirdProUtil.IntToByte(new VarByteWrapper().encode(ArrayUtil.toIntArray(slice.intensityList)));
                    }
                    else
                    {
                        compressedIndexIds =
                            new ZstdWrapper().encode(
                                AirdProUtil.IntToByte(
                                    new IntegratedVarByteWrapper().encode(ArrayUtil.toIntArray(slice.indexIdList))));
                        compressedInts = new ZstdWrapper().encode(
                            AirdProUtil.IntToByte(new VarByteWrapper().encode(ArrayUtil.toIntArray(slice.intensityList))));
                    }
                }
                else
                {
                    compressedIndexIds = ByteTrans.intToByte(ArrayUtil.toIntArray(slice.indexIdList));
                    compressedInts = ByteTrans.intToByte(ArrayUtil.toIntArray(slice.intensityList));
                }
            
                treeColumnCompressed[mz] = new ByteColumn(compressedIndexIds, compressedInts);
            
            });

            columnIndex.mzs = totalMzs;
            columnIndex.rts = rtsInt.ToArray();
            return treeColumnCompressed;
        }

        /**
         * 使用Math.NET中的稀疏矩阵进行数据初始化与横纵列转换，
         * 同时本算法支持多线程计算，速度更快
         * 第二代算法，转换速度快，比第一代快5-10倍
         *
         * 返回值中key为转化为整型的mz,value为压缩以后得数组
         */
        public ConcurrentDictionary<int, ByteColumn> CompressAsColumnMatrix(PwizConverter converter,
            ConcurrentDictionary<double, TempSpectrum> rowTable, ColumnIndex columnIndex)
        {
            var dict = rowTable.OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.Value);
            converter.JobInfo.log(null, columnIndex.toString() + "Compressing");
            Stopwatch stopwatch = Stopwatch.StartNew();

            //矩阵横坐标
            double[] rts = dict.Keys.ToArray();
            int[] rtsInt = new int[rts.Length];
            for (var i = 0; i < rts.Length; i++)
            {
                rtsInt[i] = (int)Math.Round(rts[i] * 1000);
            }

            TempSpectrum[] spectra = dict.Values.ToArray();
            int totalPoints = spectra.Sum(spectrum => spectrum.mzs.Length);
            HashSet<int> mzsSet = new HashSet<int>(totalPoints);
            foreach (TempSpectrum spectrum in spectra)
            {
                mzsSet.UnionWith(spectrum.mzs);
            }

            int[] totalMzs = mzsSet.ToArray();
            Dictionary<int, int> mzIndexDict = new Dictionary<int, int>();
            for (var i = 0; i < totalMzs.Length; i++)
            {
                mzIndexDict[totalMzs[i]] = i;
            }

            converter.JobInfo.log("m/z merge time: " + stopwatch.Elapsed.TotalSeconds + "s");
            converter.JobInfo.log("Total Spectra: " + dict.Count);
            converter.JobInfo.log("Total Diff m/z: " + totalMzs.Length);
            converter.JobInfo.log("mz range: " + totalMzs[0] * 1.0 / converter.Compressor.MzPrecision + "-" +
                                  totalMzs[totalMzs.Length - 1] * 1.0 / converter.Compressor.MzPrecision);
            SparseMatrix matrix = new SparseMatrix(rts.Length, mzsSet.Count);

            stopwatch.Restart();
            converter.JobInfo.log("Start Init Matrix");
            int iter = 0;
            Control.UseNativeMKL();
            try
            {
                foreach (TempSpectrum spectrum in spectra)
                {
                    int i = 0;
                    while (i < spectrum.mzs.Length)
                    {
                        int j = i + 1;
                        double intensitySum = spectrum.intensities[i];
                        while (j < spectrum.mzs.Length && spectrum.mzs[j] == spectrum.mzs[i])
                        {
                            intensitySum += spectrum.intensities[j];
                            j++;
                        }

                        matrix[iter, mzIndexDict[spectrum.mzs[i]]] =
                            DataUtil.FetchIntensity(intensitySum, converter.Compressor.IntensityPrecision);
                        i = j;
                    }

                    iter++;
                    converter.JobInfo.log(null, Tag.progress(columnIndex.toString(), iter, spectra.Length));
                }
            }
            catch (Exception e)
            {
                converter.JobInfo.log(e.Message);
                throw e;
            }

            converter.JobInfo.log("Init Matrix: " + stopwatch.Elapsed.TotalSeconds + " s");
            converter.JobInfo.log("Column Index Finished", "Column Index Finished");

            ConcurrentDictionary<int, ByteColumn> treeColumn = new ConcurrentDictionary<int, ByteColumn>();

            int progress = 0;
            bool fastMode = converter.JobInfo.config.fastMode;
            Parallel.For(0, matrix.ColumnCount, (i, ParallelLoopState) =>
            {
                Interlocked.Increment(ref progress);
                if (progress % 100000 == 0)
                {
                    converter.JobInfo.setStatus(columnIndex.toString() +
                                                (progress * 100.0 / matrix.ColumnCount).ToString("F1") + "%");
                }

                MathNet.Numerics.LinearAlgebra.Vector<Complex> column = matrix.Column(i);
                SparseVectorStorage<Complex> storage = (SparseVectorStorage<Complex>)(column.Storage);

                int[] spectraIds = new int[storage.ValueCount];
                int[] ints = new int[storage.ValueCount];

                int loop = 0;
                foreach (var valueTuple in storage.EnumerateNonZeroIndexed())
                {
                    spectraIds[loop] = valueTuple.Item1;
                    ints[loop] =
                        DataUtil.FetchIntensity(valueTuple.Item2.Real, converter.Compressor.IntensityPrecision);
                    loop++;
                }

                //如果使用组合压缩，最小的压缩byte也需要17个byte,当列中的点小于4个时默认不压缩
                int length = spectraIds.Length;
                byte[] compressedIndexIds = null;
                byte[] compressedInts = null;
                if (length > 4)
                {
                    if (fastMode)
                    {
                        compressedIndexIds = AirdProUtil.IntToByte(new IntegratedVarByteWrapper().encode(spectraIds));
                        compressedInts = AirdProUtil.IntToByte(new VarByteWrapper().encode(ints));
                    }
                    else
                    {
                        compressedIndexIds =
                            new ZstdWrapper().encode(
                                AirdProUtil.IntToByte(new IntegratedVarByteWrapper().encode(spectraIds)));
                        compressedInts = new ZstdWrapper().encode(
                            AirdProUtil.IntToByte(new VarByteWrapper().encode(ints)));
                    }
                }
                else
                {
                    compressedIndexIds = ByteTrans.intToByte(spectraIds);
                    compressedInts = ByteTrans.intToByte(ints);
                }

                treeColumn[totalMzs[i]] = new ByteColumn(compressedIndexIds, compressedInts);
            });

            converter.JobInfo.log("NonZero Points:" + matrix.NonZerosCount);
            converter.JobInfo.log("Column Count:" + matrix.ColumnCount + ";Row Count:" + matrix.RowCount);

            columnIndex.mzs = totalMzs.ToArray();
            columnIndex.rts = rtsInt.ToArray();

            return treeColumn;
        }
    }
}