﻿/*
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
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AirdPro.Algorithms;
using AirdPro.Domains.Convert;
using ThermoFisher.CommonCore.Data;
using CV = AirdPro.DomainsCore.Aird.CV;

namespace AirdPro.Converters
{
    internal class DDA : IConverter
    {
        private int progress;

        public DDA(JobInfo jobInfo) : base(jobInfo) {}

        public override void doConvert()
        {
            start();
            initDirectory();
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile(); //准备读取Vendor文件
                    initGlobalVar(); //初始化全局变量
                    preProcess(); //MS1和MS2分开建立索引
                    doWithMS1Block(); //处理MS1,并将索引写入文件流中
                    doWithMS2Block(); //处理MS2,并将索引写入文件流中
                    writeToAirdInfoFile(); //将Info数据写入文件
                }
            }

            finish();
        }

        private void initGlobalVar()
        {
            totalSize = spectrumList.size();
            progress = 0;
            jobInfo.log("Total Spectra:" + totalSize);
            startPosition = 0;
        }

        private void preProcess()
        {
            int parentNum = 0;
            jobInfo.log("Preprocessing:" + totalSize, "Preprocessing");
            for (var i = 0; i < totalSize; i++)
            {
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
                        addToMS2Map(parseMS2(spectrum, i, parentNum)); //如果是MS2谱图,加入到谱图组
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
                        addToMS2Map(parseMS2(spectrum, i, parentNum)); //如果这个谱图是MS2
                    }
                        
                }
            }

            jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
            jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
            jobInfo.log("Start Processing MS1 List");
        }

        //建立Ms1Scan的索引
        private MsIndex parseMS1(Spectrum spectrum, int index)
        {
            MsIndex ms1 = new MsIndex();
            ms1.level = 1;
            ms1.num = index;
            if (spectrum.scanList.scans.Count != 1)
            {
                return ms1;
            }
           
            Scan scan = spectrum.scanList.scans[0];
            if (jobInfo.jobParams.includeCV)
            {
                ms1.cvList = CV.trans(spectrum);
                if (scan.cvParams != null)
                {
                    ms1.cvList.AddRange(CV.trans(scan.cvParams));
                }
            }
            ms1.rt = parseRT(scan);
            ms1.tic = parseTIC(spectrum);
            if (msType == null)
            {
                parseMsType(spectrum);
            }

            if (polarity == null)
            {
                parsePolarity(spectrum);
            }
            return ms1;
        }

        //建立Ms2Scan的索引
        private MsIndex parseMS2(Spectrum spectrum, int index, int parentIndex)
        {
            MsIndex ms2 = new MsIndex();
            ms2.level = 2;
            ms2.pNum = parentIndex;
            ms2.num = index;
          
            try
            {
                float mz = (float) double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_target_m_z).value.ToString());

                //兼容Agilent的DDA数据格式中可能出现的lower offset和upper offset为空的情况
                string lowerOffsetStr =spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_lower_offset).value.ToString();
                string upperOffsetStr = spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_upper_offset).value.ToString();

                float lowerOffset = 0f;
                if (!lowerOffsetStr.IsNullOrEmpty())
                {
                    lowerOffset = (float) double.Parse(lowerOffsetStr);
                }

                float upperOffset = 0f;
                if (!upperOffsetStr.IsNullOrEmpty())
                {
                    upperOffset = (float) double.Parse(upperOffsetStr);
                }

                ms2.charge = getPrecursorCharge(spectrum);
                ms2.mz = mz;
                ms2.mzStart = mz - lowerOffset;
                ms2.mzEnd = mz + upperOffset;
                ms2.wid = lowerOffset + upperOffset;
            }
            catch (Exception e)
            {
                jobInfo.log("ERROR:SpectrumIndex:" + spectrum.index)
                    .log("ERROR:SpectrumId:" + spectrum.id)
                    .log("ERROR: mz:" + spectrum.precursors[0].isolationWindow
                             .cvParamChild(CVID.MS_isolation_window_target_m_z).value)
                    .log("ERROR: lowerOffset:" + spectrum.precursors[0].isolationWindow
                             .cvParamChild(CVID.MS_isolation_window_lower_offset).value)
                    .log("ERROR: upperOffset:" + spectrum.precursors[0].isolationWindow
                             .cvParamChild(CVID.MS_isolation_window_upper_offset).value);
                throw e;
            }

            if (spectrum.scanList.scans.Count != 1) return ms2;
            Scan scan = spectrum.scanList.scans[0];
            if (jobInfo.jobParams.includeCV)
            {
                ms2.cvList = CV.trans(spectrum);
                if (scan.cvParams != null)
                {
                    ms2.cvList.AddRange(CV.trans(scan.cvParams));
                }
            }
            ms2.rt = parseRT(scan);
            ms2.tic = parseTIC(spectrum);
            if (activator == null)
            {
                parseActivator(spectrum.precursors[0].activation);
            }
            return ms2;
        }

        new private void addToMS2Map(MsIndex ms2Index)
        {
            if (ms2Table.Contains(ms2Index.pNum))
            {
                (ms2Table[ms2Index.pNum] as List<MsIndex>).Add(ms2Index);
            }
            else
            {
                List<MsIndex> indexList = new List<MsIndex>();
                indexList.Add(ms2Index);
                ms2Table.Add(ms2Index.pNum, indexList);
            }
        }

        //处理MS1
        private void doWithMS1Block()
        {
            BlockIndex index = new BlockIndex();
            index.level = 1;
            index.startPtr = 0;

            switch (jobInfo.jobParams.airdAlgorithm)
            {
                case 1:
                    new ZDPD(this).compressMS1(index);
                    break;
                case 2:
                    new ZDVB(this).compressMS1(index);
                    break;
                case 3:
                    new StackZDPD(this).compressMS1(index);
                    break;
            }

            index.endPtr = startPosition;
            indexList.Add(index);
        }

        //处理MS2,由于每一个MS1只跟随少量的MS2光谱图,因此DDA采集模式下MS2的压缩模式仍然使用Aird ZDPD的压缩算法
        private void doWithMS2Block()
        {
            jobInfo.log("Start Processing MS2 List");
            ArrayList keys = new ArrayList(ms2Table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                List<MsIndex> tempIndexList = ms2Table[key] as List<MsIndex>;
                //为每一组key创建一个Block
                BlockIndex blockIndex = new BlockIndex();
                blockIndex.level = 2;
                blockIndex.startPtr = startPosition;
                blockIndex.num = key;
                //创建这一个block中每一个ms2的窗口序列
                List<WindowRange> ms2Ranges = new List<WindowRange>();
                jobInfo.log(null, "MS2:" + progress + "/" + ms2Table.Keys.Count);
                progress++;

                foreach (MsIndex index in tempIndexList)
                {
                    WindowRange range = new WindowRange(index.mzStart, index.mzEnd, index.mz);
                    if (index.charge != 0)
                    {
                        range.charge = index.charge;
                    }
                    ms2Ranges.Add(range);
                    TempScan ts = new TempScan(index.num, index.rt, index.tic);
                    if (jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = index.cvList;
                    }
                    compress(spectrumList.spectrum(index.num, true), ts);
                    blockIndex.nums.Add(ts.num);
                    blockIndex.rts.Add(ts.rt);
                    blockIndex.tics.Add(ts.tic);
                    if (jobInfo.jobParams.includeCV)
                    {
                        blockIndex.cvList.Add(ts.cvs);
                    }
                    blockIndex.mzs.Add(ts.mzArrayBytes.Length);
                    blockIndex.ints.Add(ts.intArrayBytes.Length);
                    startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                    airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                    airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
                }

                blockIndex.rangeList = ms2Ranges;
                blockIndex.endPtr = startPosition;
                indexList.Add(blockIndex);
            }
        }
    }
}