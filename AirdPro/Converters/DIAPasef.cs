/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.Constants;
using AirdPro.DomainsCore.Aird;
using pwiz.CLI.msdata;
using System.IO;
using AirdPro.Domains.Convert;

namespace AirdPro.Converters
{
    internal class DIAPasef : IConverter
    {
        public DIAPasef(JobInfo jobInfo) : base(jobInfo) {}

        public override void doConvert()
        {
            start();
            initDirectory(); //创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile(); //准备读取Vendor文件
                    initMobi();
                    predictForIntensityPrecision(); //预测intensity需要保留的精度
                    // testCode();
                    // return;
                    pretreatment(); //预处理谱图,将MS1和MS2谱图分开存储
                    compressMobiDict();
                    compressMS1Block();
                    compressMS2BlockForDIA();
                    writeToAirdInfoFile(); //将Info数据写入文件
                }
            }

            finish();
        }

        protected void pretreatment()
        {
            int parentNum = 0;
            jobInfo.log("Pretreatment:" + totalSize, "Pretreatment");
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < totalSize; i++)
            {
                progress++;
                jobInfo.log(null, "Pre:" + progress + "/" + totalSize);
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = parseMsLevel(spectrum);
                //如果这个谱图是MS1                          
                if (msLevel.Equals(MsLevel.MS1))
                {
                    parentNum = i;
                    ms1List.Add(parseMS1(spectrum, i));
                }
                //如果这个谱图是MS2
                if (msLevel.Equals(MsLevel.MS2))
                {
                    MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                    //边扫描边建立SWATH WindowRange
                    if (!rangeTable.Contains(ms2Index.precursorMz))
                    {
                        WindowRange range = new WindowRange(ms2Index.mzStart, ms2Index.mzEnd, ms2Index.precursorMz);
                        ranges.Add(range);
                        rangeTable.Add(ms2Index.precursorMz, range);
                    }
                    //DIA的MS2Map以precursorMz为key
                    addToMS2Map(ms2Index.precursorMz, ms2Index);
                }
            }

            jobInfo.log("Total SWATH Windows:" + ranges.Count);
            jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
            jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
            jobInfo.log("Start Processing MS1 List");
        }
        //预处理MS1和MS2谱图
        // protected void pretreatment()
        // {
        //     int parentNum = 0;
        //     jobInfo.log("Pretreatment:" + totalSize, "Pretreatment");
        //     double lastPrecursorMz = 0;
        //     MsIndex ms2Index = null;
        //     HashSet<float> totalMobilitySet = new HashSet<float>();
        //     // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
        //     for (int i = 0; i < totalSize; i++)
        //     {
        //         MsIndex ms1Index = null; //清空ms1Index
        //         jobInfo.log(null, "Pre:" + i + "/" + totalSize);
        //         Spectrum spectrum = spectrumList.spectrum(i);
        //         string msLevel = parseMsLevel(spectrum);
        //         //如果是MS1,开始聚合ms1的光谱
        //         while (msLevel.Equals(MsLevel.MS1))
        //         {
        //             if (ms1Index == null)
        //             {
        //                 parentNum = i;
        //                 ms1Index = parseMS1(spectrum, i);
        //                 ms1Index.mobilities = new List<float>();
        //                 ms1Index.scanNums = new List<int>();
        //             }
        //            
        //             ms1Index.tic += parseTIC(spectrum);
        //             float mobility = parseMobility(spectrum.scanList.scans[0]);
        //             ms1Index.scanNums.Add(i);
        //             ms1Index.mobilities.Add(mobility);
        //             i++;
        //             if (i < totalSize)
        //             {
        //                 jobInfo.log(null, "Pre:" + i + "/" + totalSize);
        //                 spectrum = spectrumList.spectrum(i);
        //                 msLevel = parseMsLevel(spectrum);
        //             }
        //             else
        //             {
        //                 break;
        //             }
        //         }
        //         
        //         if (ms1Index != null)
        //         {
        //             ms1List.Add(ms1Index);
        //         }
        //         //回滚一位
        //         i--;
        //         
        //         while (msLevel.Equals(MsLevel.MS2))
        //         {
        //             double precursorMz = parsePrecursorParams(spectrum, CVID.MS_isolation_window_target_m_z);
        //             if (lastPrecursorMz != precursorMz) //如果上一个precursorMz和当前的不一样,说明来自不同的frame
        //             {
        //                 if (ms2Index != null)
        //                 {
        //                     addToMS2Map(ms2Index.precursorMz, ms2Index);
        //                 }
        //                 //初始化新的ms2Index
        //                 ms2Index = parseMS2(spectrum, i, parentNum);
        //                 ms2Index.mobilities = new List<float>();
        //                 ms2Index.scanNums = new List<int>();
        //                 lastPrecursorMz = precursorMz;
        //                 if (!rangeTable.Contains(ms2Index.precursorMz))
        //                 {
        //                     WindowRange range = new WindowRange(ms2Index.mzStart, ms2Index.mzEnd, ms2Index.precursorMz);
        //                     ranges.Add(range);
        //                     rangeTable.Add(ms2Index.precursorMz, range);
        //                 }
        //             }
        //             
        //             ms2Index.tic += parseTIC(spectrum);
        //             float mobility = parseMobility(spectrum.scanList.scans[0]);
        //             ms2Index.scanNums.Add(i);
        //             ms2Index.mobilities.Add(mobility);
        //             i++;
        //             if (i < totalSize)
        //             {
        //                 jobInfo.log(null, "Pre:" + i + "/" + totalSize);
        //                 spectrum = spectrumList.spectrum(i);
        //                 msLevel = parseMsLevel(spectrum);
        //             }
        //             else
        //             {
        //                 break;
        //             }
        //         }
        //
        //         //如果检测到不是MS2,则回滚一帧
        //         i--;
        //     }
        //     jobInfo.log("Total SWATH Windows:" + ranges.Count);
        //     jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
        //     jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
        //     jobInfo.log("Start Processing MS1 List");
        // }
    }
}