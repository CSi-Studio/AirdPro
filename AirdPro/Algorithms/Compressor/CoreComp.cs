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
using AirdSDK.Beans.Common;
using AirdSDK.Compressor;
using pwiz.CLI.msdata;
using Spectrum = pwiz.CLI.msdata.Spectrum;
using System.Linq;
using System.Collections.Concurrent;
using System.Numerics;
using AirdPro.Domains.Common;
using AirdSDK.Enums;
using AirdSDK.Utils;
using MathNet.Numerics.LinearAlgebra.Complex;
using MathNet.Numerics.LinearAlgebra.Storage;

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
            //仅当面向Search的Aird模式下有效
            ConcurrentDictionary<double, TempSpectrum> msDictionary = new ConcurrentDictionary<double, TempSpectrum>();
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
                        if (converter.jobInfo.config.isComputation())
                        {
                            compress(spectrum, ts);
                        }
                        else
                        {
                            msDictionary[ts.rt] = readSpectrum(spectrum);
                        }
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
                            if (converter.jobInfo.config.isComputation())
                            {
                                compress(spectrum, ts);
                            }
                            else
                            {
                                msDictionary[ts.rt] = readSpectrum(spectrum);
                            }
                        }
                    }

                    converter.addToIndex(index, ts);
                }
            }

            //如果是面向搜索引擎的格式转换，则msRowTable不为空，准备启动行矩阵向列矩阵转换的过程
            if (converter.jobInfo.config.isSearch())
            {
                ColumnIndex columnIndex = new ColumnIndex();
                columnIndex.level = 1;
                ConcurrentDictionary<int, ByteColumn> compressedColumns = null;
                try
                {
                    compressedColumns = compressAsColumnMatrix(converter, msDictionary, columnIndex);
                }
                catch (Exception e)
                {
                    compressedColumns = compressAsColumnMatrixV1(converter, msDictionary, columnIndex);
                }

                converter.writeColumnData(compressedColumns, columnIndex);
            }
        }

        public override void compressMS2(Converter converter, List<MsIndex> ms2List, BlockIndex index)
        {
            //仅当面向Search的Aird模式下有效
            ConcurrentDictionary<double, TempSpectrum> msDictionary = new ConcurrentDictionary<double, TempSpectrum>();
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
                        //在面向搜索引擎的场景时，仅DIA模式的二级谱图具备时间上的逻辑相关性
                        if (converter.jobInfo.config.isSearch() &&
                            converter.jobInfo.type.Equals(AcquisitionMethod.DIA))
                        {
                            msDictionary[ts.rt] = readSpectrum(spectrum);
                        }
                        else
                        {
                            compress(spectrum, ts);
                        }
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
                            //在面向搜索引擎的场景时，仅DIA模式的二级谱图具备时间上的逻辑相关性
                            if (converter.jobInfo.config.isSearch() &&
                                converter.jobInfo.type.Equals(AcquisitionMethod.DIA))
                            {
                                msDictionary[ts.rt] = readSpectrum(spectrum);
                            }
                            else
                            {
                                compress(spectrum, ts);
                            }
                        }
                    }

                    converter.addToIndex(index, ts);
                }
            }

            //如果是面向搜索引擎的格式转换，则msRowTable不为空，准备启动行矩阵向列矩阵转换的过程
            if (converter.jobInfo.config.isSearch() &&
                converter.jobInfo.type.Equals(AcquisitionMethod.DIA))
            {
                ColumnIndex columnIndex = new ColumnIndex();
                columnIndex.level = 2;
                columnIndex.range = index.getWindowRange();
                ConcurrentDictionary<int, ByteColumn> compressedColumns = null;
                try
                {
                    compressedColumns = compressAsColumnMatrix(converter, msDictionary, columnIndex);
                }
                catch (Exception e)
                {
                    compressedColumns = compressAsColumnMatrixV1(converter, msDictionary, columnIndex);
                }

                converter.writeColumnData(compressedColumns, columnIndex);
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
                rtArray[t] = DataUtil.fetchRt(rtData[t]);
                intensityArray[t] = DataUtil.fetchIntensity(intData[t], 1);
            }

            byte[] compressedRtArray = rtByteComp4Chroma.encode(ByteTrans.intToByte(rtIntComp4Chroma.encode(rtArray)));
            byte[] compressedIntArray =
                intByteComp4Chroma.encode(ByteTrans.intToByte(intIntComp4Chroma.encode(intensityArray)));

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

        public TempSpectrum readSpectrum(Spectrum spectrum)
        {
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();
            var size = mzData.Length;
            if (size == 0)
            {
                return new TempSpectrum(new int[0], new float[0]);
            }

            int[] mzArray = new int[size];
            float[] intensityArray = new float[size];
            int j = 0;
            for (int t = 0; t < size; t++)
            {
                if (ignoreZero && intData[t] == 0) continue;
                mzArray[j] = DataUtil.fetchMz(mzData[t], mzPrecision);
                intensityArray[j] = (float)intData[t];
                j++;
            }

            //TODO 王金银 在这里做Centroid和降噪
            int[] mzSubArray = new int[j];
            Array.Copy(mzArray, mzSubArray, j);
            float[] intensitySubArray = new float[j];
            Array.Copy(intensityArray, intensitySubArray, j);
            return isCentroid
                ? CentroidUtil.centroid(mzSubArray, intensitySubArray, 0d)
                : new TempSpectrum(mzSubArray, intensitySubArray);
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

        /**
         * 将按光谱(即按行)存储的模式改为按列存储
         * 第一代野鸡算法，转换速度慢
         */
        public ConcurrentDictionary<int, ByteColumn> compressAsColumnMatrixV1(Converter converter,
            ConcurrentDictionary<double, TempSpectrum> rowTable, ColumnIndex columnIndex)
        {
            converter.jobInfo.log(null, "Column Compressing");
            //矩阵横坐标
            HashSet<int> mzsSet = new HashSet<int>();
            List<double> rts = rowTable.Keys.ToList();
            List<int> rtsInt = new List<int>();
            for (var i = 0; i < rts.Count; i++)
            {
                rtsInt.Add((int)Math.Round(rts[i] * 1000));
            }

            List<TempSpectrum> spectra = rowTable.Values.ToList();
            int totalIntensityNum = 0;
            foreach (TempSpectrum spectrum in spectra)
            {
                for (var i = 0; i < spectrum.mzs.Length; i++)
                {
                    mzsSet.Add(spectrum.mzs[i]);
                }

                totalIntensityNum += spectrum.intensities.Length;
            }

            List<int> sortedMzs = new List<int>(mzsSet);
            sortedMzs.Sort();
            converter.jobInfo.log("合计光谱图" + rowTable.Count + "张,不同质荷比共：" + sortedMzs.Count + "个");
            converter.jobInfo.log("质荷比范围:" + sortedMzs[0] + "-" + sortedMzs[sortedMzs.Count - 1]);
            Dictionary<double, int> ptrDict = new Dictionary<double, int>();
            foreach (double rt in rts)
            {
                ptrDict[rt] = 0;
            }

            converter.jobInfo.log("总计包含有效点数:" + totalIntensityNum);
            long totalSize = 0;
            int step = 1;
            long totalPoint = 0;
            ConcurrentDictionary<int, ByteColumn> treeColumn = new ConcurrentDictionary<int, ByteColumn>();
            foreach (int mz in sortedMzs)
            {
                List<int> indexIdList = new List<int>();
                List<int> intensityList = new List<int>();
                step++;
                if (step % 100000 == 0)
                {
                    converter.jobInfo.log(null, Tag.progress(Tag.Column, step, sortedMzs.Count));
                }

                for (var index = 0; index < rts.Count; index++)
                {
                    double rt = rts[index];
                    TempSpectrum spectrum = rowTable[rt];
                    int[] currentMzs = spectrum.mzs;
                    float[] currentInts = spectrum.intensities;
                    int iter = ptrDict[rt];
                    bool effect = false;
                    double intensity = 0;
                    while (iter < currentMzs.Length && currentMzs[iter] == mz)
                    {
                        effect = true;
                        intensity += currentInts[iter];
                        iter++;
                    }

                    if (effect)
                    {
                        indexIdList.Add(index);
                        intensityList.Add(DataUtil.fetchIntensity(intensity, converter.compressor.intensityPrecision));
                        ptrDict[rt] = iter;
                    }
                }

                totalPoint += intensityList.Count;
                byte[] compressedIndexIds = new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new IntegratedVarByteWrapper().encode(
                            ArrayUtil.toIntArray(indexIdList))));
                byte[] compressedInts = new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new VarByteWrapper().encode(
                            ArrayUtil.toIntArray(intensityList))));
                treeColumn[mz] = new ByteColumn(compressedIndexIds, compressedInts);
                totalSize += (compressedIndexIds.Length + compressedInts.Length);
            }

            converter.jobInfo.log("有效点数:" + totalPoint + "个");
            converter.jobInfo.log("总体积为:" + totalSize / 1024 / 1024 + "MB");
            columnIndex.mzs = sortedMzs.ToArray();
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
        public ConcurrentDictionary<int, ByteColumn> compressAsColumnMatrix(Converter converter,
            ConcurrentDictionary<double, TempSpectrum> rowTable, ColumnIndex columnIndex)
        {
            var dict = rowTable.OrderBy(x => x.Key).ToDictionary(k => k.Key, v => v.Value);
            converter.jobInfo.log(null, columnIndex.toString() + "Compressing");
            //矩阵横坐标
            HashSet<int> mzsSet = new HashSet<int>();
            List<double> rts = dict.Keys.ToList();
            List<int> rtsInt = new List<int>();
            for (var i = 0; i < rts.Count; i++)
            {
                rtsInt.Add((int)Math.Round(rts[i] * 1000));
            }

            List<TempSpectrum> spectra = dict.Values.ToList();
            int totalIntensityNum = 0;
            foreach (TempSpectrum spectrum in spectra)
            {
                for (var i = 0; i < spectrum.mzs.Length; i++)
                {
                    mzsSet.Add(spectrum.mzs[i]);
                }

                totalIntensityNum += spectrum.intensities.Length;
            }

            List<int> sortedMzs = new List<int>(mzsSet);
            sortedMzs.Sort();

            Dictionary<int, int> mzIndexDict = new Dictionary<int, int>();
            for (var i = 0; i < sortedMzs.Count; i++)
            {
                mzIndexDict[sortedMzs[i]] = i;
            }

            converter.jobInfo.log("Total Spectra:" + dict.Count);
            converter.jobInfo.log("Total Diff m/z：" + sortedMzs.Count);
            converter.jobInfo.log("mz range:" + sortedMzs[0] * 1.0 / converter.compressor.mzPrecision + "-" +
                                  sortedMzs[sortedMzs.Count - 1] * 1.0 / converter.compressor.mzPrecision);
            SparseMatrix matrix = new SparseMatrix(rts.Count, mzsSet.Count);
            
            int iter = 0;
            long totalPoint = 0;
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
                        
                        matrix[iter, mzIndexDict[spectrum.mzs[i]]] = intensitySum;
                        i = j;
                    }

                    iter++;
                    converter.jobInfo.log(null, Tag.progress(columnIndex.toString(), iter, spectra.Count));
                }
            }
            catch (Exception e)
            {
                converter.jobInfo.log(e.Message);
                throw e;
            }

            converter.jobInfo.log("Column Index Finished", "Column Index Finished");
            ConcurrentDictionary<int, ByteColumn> treeColumn = new ConcurrentDictionary<int, ByteColumn>();

            int progress = 0;
            Parallel.For(0, matrix.ColumnCount, (i, ParallelLoopState) =>
            {
                Interlocked.Increment(ref progress);
                if (progress % 100000 == 0)
                {
                    converter.jobInfo.setStatus(columnIndex.toString() + (progress * 100.0 / matrix.ColumnCount).ToString("F1") + "%");
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
                        DataUtil.fetchIntensity(valueTuple.Item2.Real, converter.compressor.intensityPrecision);
                    loop++;
                }
                //如果使用组合压缩，最小的压缩byte也需要17个byte,当列中的点小于4个时默认不压缩
                int length = spectraIds.Length;
                byte[] compressedIndexIds = length > 4
                    ? new ZstdWrapper().encode(
                        ByteTrans.intToByte(new IntegratedVarByteWrapper().encode(spectraIds)))
                    : ByteTrans.intToByte(spectraIds);
                
                byte[] compressedInts = length > 4 ? new ZstdWrapper().encode(
                    ByteTrans.intToByte(new VarByteWrapper().encode(ints)))
                        : ByteTrans.intToByte(ints);

                treeColumn[sortedMzs[i]] = new ByteColumn(compressedIndexIds, compressedInts);
            });

            converter.jobInfo.log("NonZero Points:" + matrix.NonZerosCount);
            converter.jobInfo.log("Column Count:" + matrix.ColumnCount + ";Row Count:" + matrix.RowCount);

            columnIndex.mzs = sortedMzs.ToArray();
            columnIndex.rts = rtsInt.ToArray();

            return treeColumn;
        }
    }
}