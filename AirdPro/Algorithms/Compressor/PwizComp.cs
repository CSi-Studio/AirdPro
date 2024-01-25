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
            ConcurrentDictionary<double, TempSpectrum> msDictionary = new ConcurrentDictionary<double, TempSpectrum>();
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
                    
                    if (converter.JobInfo.ionMobility)
                    {
                        CompressMobility(spectrum, ts);
                    }
                    else
                    {
                        //使用行式存储
                        if (converter.JobInfo.config.IsComputation())
                        {
                            Compress(spectrum, ts);
                        }
                        else //使用列式存储，准备构建存储信息
                        {
                            msDictionary[ts.rt] = ReadSpectrum(spectrum);
                        }
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
                try
                {
                    compressedColumns = CompressAsColumnMatrixV1(converter, msDictionary, columnIndex);
                }
                catch (Exception e)
                {
                    compressedColumns = CompressAsColumnMatrixV1(converter, msDictionary, columnIndex);
                }

                converter.WriteColumnData(compressedColumns, columnIndex);
            }
        }

        public override void CompressMS2(PwizConverter converter, List<MsIndex> ms2List, BlockIndex index)
        {
            //仅当面向Search的Aird模式下有效
            ConcurrentDictionary<double, TempSpectrum> msDictionary = new ConcurrentDictionary<double, TempSpectrum>();

            Hashtable table = Hashtable.Synchronized(new Hashtable());
            //使用多线程处理数据提取与压缩
            Parallel.For(0, ms2List.Count, (i, ParallelLoopState) =>
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

                    if (converter.JobInfo.ionMobility)
                    {
                        CompressMobility(spectrum, ts);
                    }
                    else
                    {
                        //在面向搜索引擎的场景时，仅DIA模式的二级谱图具备时间上的逻辑相关性
                        if (converter.JobInfo.config.IsSearch() &&
                            converter.JobInfo.type.Equals(AcquisitionMethod.DIA))
                        {
                            msDictionary[ts.rt] = ReadSpectrum(spectrum);
                        }
                        else
                        {
                            Compress(spectrum, ts);
                        }
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
                try
                {
                    compressedColumns = CompressAsColumnMatrixV1(converter, msDictionary, columnIndex);
                }
                catch (Exception e)
                {
                    compressedColumns = CompressAsColumnMatrixV1(converter, msDictionary, columnIndex);
                }

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
                return new TempSpectrum(Array.Empty<int>(), Array.Empty<float>());
            }

            int[] mzArray = new int[size];
            float[] intensityArray = new float[size];
            int j = 0;
            for (int t = 0; t < size; t++)
            {
                if (IgnoreZero && intData[t] == 0) continue;
                mzArray[j] = DataUtil.FetchMz(mzData[t], MzPrecision);
                intensityArray[j] = (float)intData[t];
                j++;
            }

            int[] mzSubArray = new int[j];
            Array.Copy(mzArray, mzSubArray, j);
            float[] intensitySubArray = new float[j];
            Array.Copy(intensityArray, intensitySubArray, j);
            return IsCentroid
                ? CentroidUtil.Centroid(mzSubArray, intensitySubArray, 0d)
                : new TempSpectrum(mzSubArray, intensitySubArray);
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
        public ConcurrentDictionary<int, ByteColumn> CompressAsColumnMatrixV1(PwizConverter converter,
            ConcurrentDictionary<double, TempSpectrum> rowTable, ColumnIndex columnIndex)
        {
            converter.JobInfo.log(null, "Column Compressing");
            //矩阵横坐标
            List<double> rts = rowTable.Keys.ToList();
            List<int> rtsInt = [];
            for (var i = 0; i < rts.Count; i++)
            {
                rtsInt.Add((int)Math.Round(rts[i] * 1000));
            }

            List<TempSpectrum> spectra = rowTable.Values.ToList();
            int totalPoints = 0;
            spectra.ForEach(spectrum => totalPoints += spectrum.mzs.Length);
            HashSet<int> mzsSet = new HashSet<int>(totalPoints);
            foreach (TempSpectrum spectrum in spectra)
            {
                mzsSet.UnionWith(spectrum.mzs);
            }

            int[] totalMzs = mzsSet.ToArray();

            converter.JobInfo.log("合计光谱图" + rowTable.Count + "张,不同质荷比共：" + totalMzs.Length + "个");
            converter.JobInfo.log("质荷比范围:" + totalMzs[0] + "-" + totalMzs[totalMzs.Length - 1]);
            Dictionary<double, int> ptrDict = new Dictionary<double, int>();
            foreach (double rt in rts)
            {
                ptrDict[rt] = 0;
            }

            converter.JobInfo.log("总计包含有效点数:" + totalPoints);
            long totalSize = 0;
            int step = 1;
            long totalPoint = 0;
            ConcurrentDictionary<int, ByteColumn> treeColumn = new ConcurrentDictionary<int, ByteColumn>();

            // 原有的循环处理逻辑
            // foreach (int mz in totalMzs)
            // {
            //     List<int> indexIdList = new List<int>();
            //     List<int> intensityList = new List<int>();
            //     step++;
            //     if (step % 100000 == 0)
            //     {
            //         converter.jobInfo.log(null, Tag.progress(Tag.Column, step, totalMzs.Length));
            //     }
            //
            //     for (var index = 0; index < rts.Count; index++)
            //     {
            //         double rt = rts[index];
            //         TempSpectrum spectrum = rowTable[rt];
            //         int[] currentMzs = spectrum.mzs;
            //         float[] currentInts = spectrum.intensities;
            //         int iter = ptrDict[rt];
            //         bool effect = false;
            //         double intensity = 0;
            //         while (iter < currentMzs.Length && currentMzs[iter] == mz)
            //         {
            //             effect = true;
            //             intensity += currentInts[iter];
            //             iter++;
            //         }
            //
            //         if (effect)
            //         {
            //             indexIdList.Add(index);
            //             intensityList.Add(DataUtil.fetchIntensity(intensity, converter.compressor.intensityPrecision));
            //             ptrDict[rt] = iter;
            //         }
            //     }
            //
            //     totalPoint += intensityList.Count;
            //     byte[] compressedIndexIds = new ZstdWrapper().encode(
            //         ByteTrans.intToByte(
            //             new IntegratedVarByteWrapper().encode(
            //                 ArrayUtil.toIntArray(indexIdList))));
            //     byte[] compressedInts = new ZstdWrapper().encode(
            //         ByteTrans.intToByte(
            //             new VarByteWrapper().encode(
            //                 ArrayUtil.toIntArray(intensityList))));
            //     treeColumn[mz] = new ByteColumn(compressedIndexIds, compressedInts);
            //     totalSize += (compressedIndexIds.Length + compressedInts.Length);
            // }
            //
            bool fastMode = converter.JobInfo.config.fastMode;
            // 并行化处理质荷比
            Parallel.ForEach(totalMzs, mz =>
            {
                List<int> indexIdList = new List<int>();
                List<int> intensityList = new List<int>();
                int currentStep = Interlocked.Increment(ref step);

                if (currentStep % 100000 == 0)
                {
                    converter.JobInfo.log(null, Tag.progress(Tag.Column, currentStep, totalMzs.Length));
                }

                //从每一张光谱图中搜索和当前mz相同的点，如果存在相同mz的值，则直接累加
                for (int index = 0; index < rts.Count; index++)
                {
                    double rt = rts[index];
                    TempSpectrum spectrum = rowTable[rt];
                    int[] currentMzs = spectrum.mzs;
                    float[] currentInts = spectrum.intensities;
                    // int iter = ptrDict[rt];
                    // int iter = 0;
                    // bool effect = false;
                    // double intensity = 0;

                    float sum = AirdProUtil.SumValuesAtIndices(currentMzs, currentInts, mz);

                    // while (iter < currentMzs.Length && currentMzs[iter] == mz)
                    // {
                    //     effect = true;
                    //     intensity += currentInts[iter];
                    //     iter++;
                    // }

                    // if (effect)
                    // {
                    //     indexIdList.Add(index);
                    //     intensityList.Add(DataUtil.fetchIntensity(intensity, converter.compressor.intensityPrecision));
                    //     ptrDict[rt] = iter;
                    // }
                    if (sum > 0)
                    {
                        indexIdList.Add(index);
                        intensityList.Add(DataUtil.FetchIntensity(sum, converter.Compressor.IntensityPrecision));
                        // ptrDict[rt] = iter;
                    }
                }

                Interlocked.Add(ref totalPoint, intensityList.Count);

                int length = indexIdList.Count;
                byte[] compressedIndexIds = null;
                byte[] compressedInts = null;
                if (length > 4)
                {
                    if (fastMode)
                    {
                        compressedIndexIds =
                            AirdProUtil.IntToByte(
                                new IntegratedVarByteWrapper().encode(ArrayUtil.toIntArray(indexIdList)));
                        compressedInts =
                            AirdProUtil.IntToByte(new VarByteWrapper().encode(ArrayUtil.toIntArray(intensityList)));
                    }
                    else
                    {
                        compressedIndexIds =
                            new ZstdWrapper().encode(
                                AirdProUtil.IntToByte(
                                    new IntegratedVarByteWrapper().encode(ArrayUtil.toIntArray(indexIdList))));
                        compressedInts = new ZstdWrapper().encode(
                            AirdProUtil.IntToByte(new VarByteWrapper().encode(ArrayUtil.toIntArray(intensityList))));
                    }
                }
                else
                {
                    compressedIndexIds = ByteTrans.intToByte(ArrayUtil.toIntArray(indexIdList));
                    compressedInts = ByteTrans.intToByte(ArrayUtil.toIntArray(intensityList));
                }

                // byte[] compressedIndexIds = new ZstdWrapper().encode(
                //     AirdProUtil.intToByte(
                //         new IntegratedVarByteWrapper().encode(
                //             ArrayUtil.toIntArray(indexIdList))));
                //
                // byte[] compressedInts = new ZstdWrapper().encode(
                //     AirdProUtil.intToByte(
                //         new VarByteWrapper().encode(
                //             ArrayUtil.toIntArray(intensityList))));

                treeColumn[mz] = new ByteColumn(compressedIndexIds, compressedInts);

                Interlocked.Add(ref totalSize, compressedIndexIds.Length + compressedInts.Length);
            });

            converter.JobInfo.log("有效点数:" + totalPoint + "个");
            converter.JobInfo.log("总体积为:" + AirdProFileUtil.GetSizeLabel(totalSize));
            columnIndex.mzs = totalMzs;
            columnIndex.rts = rtsInt.ToArray();
            return treeColumn;
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