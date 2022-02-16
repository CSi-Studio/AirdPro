/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AirdPro.Converters;
using AirdPro.DomainsCore.Aird;

namespace AirdPro.Algorithms
{
    public class ZDVB :ICompressor
    {
        public ZDVB(IConverter converter) : base(converter) {}

        override
       public void compressMS1(BlockIndex index)
        {
            if (converter.jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());

                //使用多线程处理数据提取与压缩
                Parallel.For(0, converter.ms1List.Count, (i, ParallelLoopState) =>
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + converter.ms1List.Count);
                    TempIndex scanIndex = converter.ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt, scanIndex.tic);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = scanIndex.cvList;
                    }
                    converter.compress(converter.spectrumList.spectrum(scanIndex.num, true), ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                for (int i = 0; i < converter.ms1List.Count; i++)
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + converter.ms1List.Count);
                    TempIndex scanIndex = converter.ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt, scanIndex.tic);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = scanIndex.cvList;
                    }
                    converter.compress(converter.spectrumList.spectrum(scanIndex.num, true), ts);
                    converter.addToIndex(index, ts);
                }
            }
        }

        override
        public void compressMS2(List<TempIndex> tempIndexList, BlockIndex index)
        {
            if (converter.jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, tempIndexList.Count, (i, ParallelLoopState) =>
                {
                    TempIndex tempIndex = tempIndexList[i];
                    TempScan ts = new TempScan(tempIndex.num, tempIndex.rt, tempIndex.tic);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = tempIndex.cvList;
                    }
                    converter.compress(converter.spectrumList.spectrum(tempIndex.num, true), ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                foreach (TempIndex tempIndex in tempIndexList)
                {
                    TempScan ts = new TempScan(tempIndex.num, tempIndex.rt, tempIndex.tic);
                    if (converter.jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = tempIndex.cvList;
                    }
                    converter.compress(converter.spectrumList.spectrum(tempIndex.num, true), ts);
                    converter.addToIndex(index, ts);
                }
            }
        }
    }
}