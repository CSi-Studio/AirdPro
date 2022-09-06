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
using AirdSDK.Enums;

namespace AirdPro.Converters
{
    internal class DDAPasef : IConverter
    {
        public DDAPasef(JobInfo jobInfo) : base(jobInfo)
        {
        }

        public override void doConvert()
        {
            start();
            initDirectory();
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile(); //准备读取Vendor文件
                    initBrukerMobi();
                    predictForIntensityPrecision(); //预测intensity需要保留的精度
                    predictForBestCombination(); //预测最佳压缩组合
                    pretreatment(); //MS1和MS2分开建立索引
                    compressMobiDict();
                    compressMS1Block(); //处理MS1,并将索引写入文件流中
                    compressMS2BlockForDDA(); //处理MS2,并将索引写入文件流中
                    writeToAirdInfoFile(); //将Info数据写入文件
                }
            }

            finish();
        }

        private void pretreatment()
        {
            int parentNum = 0;
            jobInfo.log(Tag.Pretreatment + totalSize, Status.Pretreatment);
            for (var i = 0; i < totalSize; i++)
            {
                jobInfo.log(null, Tag.progress(Tag.Pre, i, totalSize));
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = parseMsLevel(spectrum);

                //最后一个谱图,单独判断
                if (i == totalSize - 1)
                {
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        ms1List.Add(parseMS1(spectrum, i)); //如果是MS1谱图,加入到MS1List
                    }

                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                        addToMS2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
                    }
                }
                else
                {
                    //如果这个谱图是MS1
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        ms1List.Add(parseMS1(spectrum, i)); //加入MS1List
                        Spectrum next = spectrumList.spectrum(i + 1);
                        if (parseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                        {
                            parentNum = i;
                        }
                    }

                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                        addToMS2Map(ms2Index.pNum, ms2Index); //如果这个谱图是MS2
                    }
                }
            }

            jobInfo.log(Tag.Effective_MS1_List_Size + ms1List.Count);
            jobInfo.log(Tag.MS2_Group_List_Size + ms2Table.Count);
            jobInfo.log(Tag.Start_Processing_MS1_List);
        }
    }
}