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
using System.IO;
using System.Threading.Tasks;
using AirdPro.Constants;
using AirdPro.DomainsCore.Aird;
using AirdPro.Domains.Convert;
using pwiz.CLI.msdata;

namespace AirdPro.Converters
{
    internal class PRM : IConverter
    {
        private int progress;//进度计数器

        public PRM(JobInfo jobInfo) : base(jobInfo) {}

        public override void doConvert()
        {
            start();
            initDirectory();//创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile();//准备读取Vendor文件
                    initGlobalVar();//初始化全局变量
                    pretreatment();//预处理谱图,将MS1和MS2谱图分开存储
                    compressMS1Block();//处理MS1,并将索引写入文件流中
                    parseAndStoreMS2Block();//处理MS2,并将索引写入文件流中
                    writeToAirdInfoFile();//将Info数据写入文件
                }
            }
            finish();
        }

        private void initGlobalVar()

        {
            totalSize = spectrumList.size();
            progress = 0;
            jobInfo.log("Total Spectra:" + totalSize);
            startPosition = 0;//文件的存储位置,每一次解析完就会将指针往前挪移
        }

        //Step1. 解析MS1和MS2谱图
        protected void pretreatment()
        {
            int parentNum = 0;
            jobInfo.log("Preprocessing:" + totalSize, "Preprocessing");
            Parallel.For(0, totalSize, (i, ParallelLoopState) =>
            {
                jobInfo.log(null, (i + 1) + "/" + totalSize);
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = parseMsLevel(spectrum);
                //如果是最后一个谱图,那么单独判断
                if (i == totalSize - 1)
                {
                    //如果是MS1谱图,那么直接跳过
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        ParallelLoopState.Break();
                        return;
                    }
                    //如果是MS2谱图,加入到谱图组
                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrumList.spectrum(i), i, parentNum);
                        addToMS2Map(ms2Index.precursorMz, ms2Index);
                        ParallelLoopState.Break();
                        return;
                    }
                }

                //如果这个谱图是MS1
                if (msLevel.Equals(MsLevel.MS1))
                {
                    Spectrum next = spectrumList.spectrum(i + 1);
                    string msLevelNext = parseMsLevel(next);
                    //如果下一个谱图仍然是MS1, 那么直接忽略这个谱图
                    if (msLevelNext.Equals(MsLevel.MS1))
                    {
                        ParallelLoopState.Break();
                        return;
                    }
                    if (msLevelNext.Equals(MsLevel.MS2))
                    {
                        parentNum = i;
                        ms1List.Add(parseMS1(spectrumList.spectrum(i), i));
                    }
                }

                if (msLevel.Equals(MsLevel.MS2))
                {
                    MsIndex ms2Index = parseMS2(spectrumList.spectrum(i), i, parentNum);
                    addToMS2Map(ms2Index.precursorMz, ms2Index); //如果这个谱图是MS2
                }
            });

            jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
            jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
            jobInfo.log("Start Processing MS1 List");
        }

        private void parseAndStoreMS2Block()
        {
            jobInfo.log("Start Processing MS2 List");
            foreach (double key in ms2Table.Keys)
            {
                List<MsIndex> ms2List = ms2Table[key] as List<MsIndex>;
                WindowRange range = new WindowRange(ms2List[0].mzStart, ms2List[0].mzEnd, key);

                BlockIndex index = new BlockIndex(); //为每一个key组创建一个SwathBlock
                index.level = 2;
                index.startPtr = startPosition;
                index.setWindowRange(range); //顺便创建一个WindowRanges,用以让Propro服务端快速获取全局的窗口数目和mz区间
                ranges.Add(range);

                jobInfo.log(null, "MS2:" + progress + "/" + ms2Table.Keys.Count);
                progress++;
                compressor.compressMS2(this,ms2List, index);
                index.endPtr = startPosition;
                indexList.Add(index);
                jobInfo.log("MS2 Group Finished:" + progress + "/" + ms2Table.Keys.Count);
            }
        }

    }
}