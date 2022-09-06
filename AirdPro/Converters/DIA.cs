/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using pwiz.CLI.msdata;
using System.IO;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdSDK.Beans;
using AirdSDK.Enums;

namespace AirdPro.Converters
{
    internal class DIA : IConverter
    {
        public DIA(JobInfo jobInfo) : base(jobInfo)
        {
        }

        public override void doConvert()
        {
            start();
            initDirectory(); //创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile(); //准备读取Vendor文件
                    predictForIntensityPrecision(); //预测intensity需要保留的精度
                    predictForBestCombination(); //预测最佳压缩组合
                    pretreatment(); //预处理谱图,将MS1和MS2谱图分开存储
                    compressMS1Block();
                    compressMS2BlockForDIA();
                    writeToAirdInfoFile(); //将Info数据写入文件
                }
            }

            finish();
        }

        //预处理MS1与MS2谱图
        protected void pretreatment()
        {
            int parentNum = 0;
            jobInfo.log(Tag.Pretreatment + totalSize, Status.Pretreatment);
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < totalSize; i++)
            {
                progress++;
                jobInfo.log(null, Tag.progress(Tag.Pre, progress, totalSize));
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

            jobInfo.log(Tag.Total_SWATH_WINDOWS + ranges.Count);
            jobInfo.log(Tag.Effective_MS1_List_Size + ms1List.Count);
            jobInfo.log(Tag.MS2_Group_List_Size + ms2Table.Count);
            jobInfo.log(Tag.Start_Processing_MS1_List);
        }
    }
}