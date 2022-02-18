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
    public class ZDPD :ICompressor
    {
        
        public ZDPD(IConverter converter) : base(converter) {}

      
         
        public override void compressMS1(IConverter converter, BlockIndex index)
        {
            MsIndex[] ms1List = converter.ms1List.ToArray();
            if (multiThread)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                int process = 0;
                //使用多线程处理数据提取与压缩
                Parallel.For(0, converter.ms1List.Count, (i, ParallelLoopState) =>
                {
                    Interlocked.Increment(ref process);
                    converter.jobInfo.log(null, "MS1:" + process + "/" + converter.ms1List.Count);
                    MsIndex scanIndex = ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt, scanIndex.tic, scanIndex.cvList);
                    compress(converter.spectrumList.spectrum(scanIndex.num, true), ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                for (int i = 0; i < converter.ms1List.Count; i++)
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + converter.ms1List.Count);
                    MsIndex scanIndex = ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt, scanIndex.tic, scanIndex.cvList);
                    compress(converter.spectrumList.spectrum(scanIndex.num, true), ts);
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
                    MsIndex msIndex = ms2List[i];
                    TempScan ts = new TempScan(msIndex.num, msIndex.rt, msIndex.tic, msIndex.cvList);
                    compress(converter.spectrumList.spectrum(msIndex.num, true), ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                foreach (MsIndex tempIndex in ms2List)
                {
                    TempScan ts = new TempScan(tempIndex.num, tempIndex.rt, tempIndex.tic, tempIndex.cvList);
                    compress(converter.spectrumList.spectrum(tempIndex.num, true), ts);
                    converter.addToIndex(index, ts);
                }
            }
        }

        public override void compress(Spectrum spectrum, TempScan ts)
        {
            BinaryDataDouble mzData = spectrum.getMZArray().data;
            BinaryDataDouble intData = spectrum.getIntensityArray().data;
            var size = mzData.Count;
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
    }

    
}