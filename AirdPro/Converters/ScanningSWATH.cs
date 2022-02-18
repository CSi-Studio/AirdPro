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
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using AirdPro.Domains.Convert;
using CV = AirdPro.DomainsCore.Aird.CV;

namespace AirdPro.Converters
{
    public class ScanningSWATH : IConverter
    {
        private int progress;//进度计数器
        Hashtable rangeMap = Hashtable.Synchronized(new Hashtable());//用于存储TargetMZ-SwathIndex
        Hashtable ms2Map = Hashtable.Synchronized(new Hashtable());//用于存储TargetMZ-List<TempScan>

        public ScanningSWATH(JobInfo jobInfo) : base(jobInfo) { }

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
                    buildWindowsRanges(); //Getting SWATH Windows
                    coreLoop();//核心扫描解析逻辑
                    sortAndWrite();//整理最终的解析数据,并且写入文件
                    writeToAirdInfoFile();//将Info数据写入文件
                    clearCache();
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

        /**
         * 循环搜索,直到出现第一个重复的MS窗口停止
         * 即便这样,如果在第一轮扫描中没有出现的窗口是无法被初始化的,需要在后续不断的作补充
         *
         */
        private void buildWindowsRanges()
        {
            jobInfo.log("Start getting windows", "Getting Windows");
            int i = 0;
            Spectrum spectrum = spectrumList.spectrum(0);
            while (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS2))
            {
                double mz = parsePrecursorParams(spectrum, CVID.MS_isolation_window_target_m_z);
                double lowerOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_lower_offset);
                double upperOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_upper_offset);

                if (rangeMap.Contains(mz))
                {
                    break;
                }

                WindowRange range = new WindowRange(mz - lowerOffset, mz + upperOffset, mz);
                BlockIndex swathIndex = new BlockIndex();
                swathIndex.setWindowRange(range);
                rangeMap.Add(mz, swathIndex);
                ms2Map.Add(mz, ArrayList.Synchronized(new ArrayList()));

                i++;
                spectrum = spectrumList.spectrum(i);
            }
            jobInfo.log("Finished Getting Windows");
        }

        private void coreLoop()
        {
            jobInfo.log(null, progress + "/" + totalSize);
            Parallel.For(0, totalSize, (i, ParallelLoopState) =>
            {
                scan(i);
                if (progress % 100 == 0) jobInfo.log(null, progress + 1 + "/" + totalSize);
                progress++;
            });
        }

        private void sortAndWrite()
        {
            jobInfo.log("Writing Aird File", "writing file");
            ArrayList mzKeys = new ArrayList(rangeMap.Keys); //别忘了导入System.Collections
            mzKeys.Sort(); //按mz顺序进行排序
            foreach (float key in mzKeys)
            {
                BlockIndex swathIndex = (BlockIndex)rangeMap[key];
                ArrayList tempScanList = (ArrayList)ms2Map[key];
                swathIndex.level = 2;
                swathIndex.startPtr = startPosition;
                tempScanList.Sort();
                foreach (TempScan tempScan in tempScanList)
                {
                    addToIndex(swathIndex, tempScan);
                }

                swathIndex.endPtr = startPosition;
                indexList.Add(swathIndex);
                ranges.Add(swathIndex.getWindowRange());
            }
        }

        private void scan(int i)
        {
            Spectrum spectrum = spectrumList.spectrum(i, true);
            //忽略所有MS1的谱图
            if (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS1))
            {
                return;
            }
            double targetMz = parsePrecursorParams(spectrum, CVID.MS_isolation_window_target_m_z);
            ArrayList ms2List = null;
            if (rangeMap.Contains(targetMz))
            {
                ms2List = (ArrayList) ms2Map[targetMz];
            }
            else
            {
                double mz = parsePrecursorParams(spectrum, CVID.MS_isolation_window_target_m_z);
                double lowerOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_lower_offset);
                double upperOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_upper_offset);

                WindowRange range = new WindowRange(mz - lowerOffset, mz + upperOffset, mz);
                BlockIndex addIndex = new BlockIndex();
                addIndex.setWindowRange(range);
                rangeMap.Add(targetMz, addIndex);
                ms2Map.Add(targetMz, ArrayList.Synchronized(new ArrayList()));
                ms2List = (ArrayList)ms2Map[targetMz];
            }

            if (spectrum.scanList.scans.Count != 1)
            {
                return;
            }
            try
            {
                TempScan ts = new TempScan(i, parseRT(spectrum.scanList.scans[0]), parseTIC(spectrum), CV.trans(spectrum));
                compressor.compress(spectrum, ts);
                ms2List.Add(ts);
            }
            catch (Exception exception)
            {
                jobInfo.log(exception.Message);
                return;
            }
        }

        new private void clearCache()
        {
            rangeMap = Hashtable.Synchronized(new Hashtable());
            ms2Map = Hashtable.Synchronized(new Hashtable());
        }
    }
}