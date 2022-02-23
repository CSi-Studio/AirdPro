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
using AirdPro.Utils;
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AirdPro.Domains.Convert;

namespace AirdPro.Converters
{
    internal class DIAPasef : IConverter
    {
        private double overlap;//SWATH窗口间的区域重叠值
        private Hashtable rangeTable = new Hashtable(); //用于存放SWATH窗口的信息,key为mz
        public DIAPasef(JobInfo jobInfo) : base(jobInfo) {}

        public override void doConvert()
        {
            start();
            initDirectory();//创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile();//准备读取Vendor文件
                    // buildWindowsRanges();  //Getting SWATH Windows
                    pretreatment();//预处理谱图,将MS1和MS2谱图分开存储
                    parseAndStoreMS1Block();
                    parseAndStoreMS2Block();
                    writeToAirdInfoFile();//将Info数据写入文件
                }
            }
            finish();
        }

        //解析MS1和MS2谱图
        protected void pretreatment()
        {
            int parentNum = 0;
            jobInfo.log("Preprocessing:" + totalSize, "Preprocessing");
            int progress = 0;
            
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < totalSize; i++)
            {
                progress++;
                jobInfo.log(null, "Pre:" + progress + "/" + totalSize);
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = parseMsLevel(spectrum);
                if (msLevel.Equals(MsLevel.MS1))
                {
                    parentNum = i;
                    ms1List.Add(parseMS1(spectrum, i));
                }
                if (msLevel.Equals(MsLevel.MS2))
                {
                    MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                    addToMS2Map(ms2Index);
                }
            }
            jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
            jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
            jobInfo.log("Start Processing MS1 List");
        }

        //提取SWATH 窗口信息
        private void buildWindowsRanges()
        {
            jobInfo.log("Start getting windows", "Getting Windows");
            int i = 0;
            List<Double> mzList = new List<Double>();
            Spectrum spectrum = spectrumList.spectrum(0);
            while (true)
            { 
                if (parseMsLevel(spectrum).Equals(MsLevel.MS1))
                {
                    i++;
                    spectrum = spectrumList.spectrum(i);
                    continue;
                }

                double mz, lowerOffset, upperOffset;
                mz = parsePrecursorParams(spectrum, CVID.MS_isolation_window_target_m_z);
                if (!mzList.Contains(mz))
                {
                    mzList.Add(mz);
                    lowerOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_lower_offset);
                    upperOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_upper_offset);
                    WindowRange range = new WindowRange(mz - lowerOffset, mz + upperOffset, mz);
                    Hashtable features = new Hashtable();
                    features.Add(Features.original_width, lowerOffset + upperOffset);
                    features.Add(Features.original_precursor_mz_start, mz - lowerOffset);
                    features.Add(Features.original_precursor_mz_end, mz + upperOffset);
                    range.features = FeaturesUtil.toString(features);
                    ranges.Add(range);
                }
                
                i++;
                if (i>=spectrumList.size())
                {
                    break;
                }
                spectrum = spectrumList.spectrum(i);
            }
            
            computeOverlap();
            adjustOverlap();
            jobInfo.log("Finished Getting Windows, Total SWATH Windows:" +ranges.Count);
        }

        //计算窗口间的重叠区域的大小
        private void computeOverlap()
        {
            WindowRange range1 = ranges[0];
            double range1Right = range1.end;
            WindowRange range2 = ranges[1];
            double range2Left = range2.start;
            overlap = range1Right - range2Left;
            featuresMap.Add(Features.overlap, overlap);
        }

        //调整间距,第一个窗口的上区间不做调整,最后一个窗口的下区间不做调整
        private void adjustOverlap()
        {
            int size = ranges.Count;
            if (size <= 2)
            {
                jobInfo.log("Windows Size Exception: Only " + size + " Windows");
                throw new Exception("Windows Size Exception: Only " + size + " Windows");
            }
            ranges[0].end = ranges[0].end - (overlap / 2);  //第一个窗口的上区间保持不变,下区间做调整
            ranges[size - 1].start = ranges[size - 1].start + (overlap / 2); //最后一个窗口的下区间不变,上区间做调整

            for (int i=1;i < size - 1;i++)
            {
                ranges[i].start = ranges[i].start + (overlap / 2);
                ranges[i].end = ranges[i].end - (overlap / 2);
            }

            foreach (WindowRange range in ranges)
            {
                rangeTable.Add(range.mz, range);
            }
        }

        private void parseAndStoreMS2Block()
        {
            jobInfo.log("Start Processing MS2 List");
            int progress = 0;
            foreach (double key in ms2Table.Keys)
            {
                List<MsIndex> ms2List = ms2Table[key] as List<MsIndex>;
                WindowRange range = rangeTable[key] as WindowRange;
                
                BlockIndex index = new BlockIndex(); //为每一个key组创建一个SwathBlock
                index.level = 2;
                index.startPtr = startPosition;
                index.setWindowRange(range);

                jobInfo.log(null, "MS2:" + progress + "/" + ms2Table.Keys.Count);
                progress++;
                compressor.compressMS2(this, ms2List, index);
                index.endPtr = startPosition;
                indexList.Add(index);
                jobInfo.log("MS2 Group Finished:" + progress + "/" + ms2Table.Keys.Count);
            }
        }
    }
}