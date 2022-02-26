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
using AirdPro.Domains.Convert;
using AirdPro.Utils;
using Newtonsoft.Json;
using pwiz.CLI.analysis;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AirdPro.Algorithms;
using Compress;
using pwiz.CLI.util;
using ByteOrder = AirdPro.Constants.ByteOrder;
using CV = AirdPro.DomainsCore.Aird.CV;
using Software = pwiz.CLI.msdata.Software;

namespace AirdPro.Converters
{
    public abstract class IConverter
    {
        protected MSData msd;
        public SpectrumList spectrumList;
        public JobInfo jobInfo;
        protected Stopwatch stopwatch = new Stopwatch();
        protected FileStream airdStream;
        protected FileStream airdJsonStream;
        protected List<WindowRange> ranges = new List<WindowRange>(); //SWATH Window的窗口
        protected Hashtable rangeTable = new Hashtable(); //用于存放SWATH窗口的信息,key为mz
        protected List<BlockIndex> indexList = new List<BlockIndex>(); //用于存储的全局的SWATH List
        protected Hashtable ms2Table = Hashtable.Synchronized(new Hashtable()); //用于存放MS2的索引信息,DDA采集模式下key为ms1的num, DIA采集模式下key为mz
        public ConcurrentBag<MsIndex> ms1List = new ConcurrentBag<MsIndex>(); //用于存放MS1索引及基础信息,泛型为MsIndex
        protected Hashtable featuresMap = new Hashtable();
        public ICompressor compressor;

        protected long fileSize; //厂商文件大小
        protected long startPosition = 0; //文件指针
        protected int totalSize; //总计的谱图数目

        protected string activator; //HCD,CID....
        protected float energy; //轰击能
        protected string msType; //Profile, Centroided
        protected string polarity; //Negative, Positive
        protected string rtUnit; //Minute, Second
        protected string ccsUnit;
        protected string mobilityType;

        public IConverter(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
        }

        public abstract void doConvert();

        public void start()
        {
            stopwatch.Start();
            jobInfo.log("Ready To Start", "Starting");
        }

        public void finish()
        {
            stopwatch.Stop();
            jobInfo.refreshReport = true;
            jobInfo.retryTimes = 0;
            jobInfo.log("Finished! Total Cost: " + stopwatch.Elapsed.TotalSeconds + " seconds", "Finished");
        }

        protected void initDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdFilePath));
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdJsonFilePath));
        }

        protected string parseMsLevel(Spectrum spectrum)
        {
            return spectrum.cvParamChild(CVID.MS_ms_level).value.ToString();
        }

        protected float parseRT(Scan scan)
        {
            CVParam cv = scan.cvParamChild(CVID.MS_scan_start_time);
            float time = float.Parse(cv.value.ToString());
            rtUnit = cv.unitsName;
            return time;
        }

        protected float parseMobility(Scan scan)
        {
            float ccs = 0f;
            if (scan.hasCVParamChild(CVID.MS_inverse_reduced_ion_mobility))
            {
                CVParam cv = scan.cvParamChild(CVID.MS_inverse_reduced_ion_mobility);
                ccs = float.Parse(cv.value.ToString());
                ccsUnit = cv.unitsName;
                mobilityType = MobilityType.TIMS;
            }

            if (scan.hasCVParamChild(CVID.MS_ion_mobility_drift_time))
            {
                CVParam cv = scan.cvParamChild(CVID.MS_ion_mobility_drift_time);
                ccs = float.Parse(cv.value.ToString());
                ccsUnit = cv.unitsName;
                mobilityType = MobilityType.DTIMS;
            }

            return ccs;
        }

        protected long parseTIC(Spectrum spectrum)
        {
            try
            {
                return Convert.ToInt64(Convert.ToDouble(spectrum.cvParamChild(CVID.MS_TIC).value.ToString()));
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /**
         * 从任意spectrum上获取
         */
        protected void parsePolarity(Spectrum spectrum)
        {
            if (!spectrum.cvParamChild(CVID.MS_negative_scan).cvid.Equals(CVID.CVID_Unknown))
            {
                polarity = Polarity.NEGATIVE;
            }
            else if (!spectrum.cvParamChild(CVID.MS_positive_scan).cvid.Equals(CVID.CVID_Unknown))
            {
                polarity = Polarity.POSITIVE;
            }
        }

        /**
         * 从任意spectrum上获取
         */
        protected void parseMsType(Spectrum spectrum)
        {
            if (!spectrum.cvParamChild(CVID.MS_profile_spectrum).cvid.Equals(CVID.CVID_Unknown))
            {
                msType = MSType.PROFILE;
            }
            else if (!spectrum.cvParamChild(CVID.MS_centroid_spectrum).cvid.Equals(CVID.CVID_Unknown))
            {
                msType = MSType.CENTROIDED;
            }
            else
            {
                msType = MSType.UNKNOWN;
            }
        }

        /**
         * 解析activation以及对应的energy
         * 需要从ms2的谱图上获取
         */
        protected void parseActivator(Activation activation)
        {
            //这个解析仅做一次,也就是仅分析第一个scan的activation
            if (activation == null)
            {
                return;
            }

            if (!activation.cvParamChild(CVID.MS_HCD).cvid.Equals(CVID.CVID_Unknown))
            {
                activator = Constants.Activator.HCD;
            }
            else if (!activation.cvParamChild(CVID.MS_CID).cvid.Equals(CVID.CVID_Unknown))
            {
                activator = Constants.Activator.CID;
            }
            else if (!activation.cvParamChild(CVID.MS_ECD).cvid.Equals(CVID.CVID_Unknown))
            {
                activator = Constants.Activator.ECD;
            }
            else if (!activation.cvParamChild(CVID.MS_ETD).cvid.Equals(CVID.CVID_Unknown))
            {
                activator = Constants.Activator.ETD;
            }
            else
            {
                activator = Constants.Activator.UNKNOWN;
            }

            if (!activation.cvParamChild(CVID.MS_collision_energy).cvid.Equals(CVID.CVID_Unknown))
            {
                energy = Convert.ToSingle(activation.cvParamChild(CVID.MS_collision_energy).value.ToString());
            }
            else
            {
                energy = -1;
            }
        }


        public void writeToFile(Hashtable table, BlockIndex index)
        {
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                addToIndex(index, table[key]);
            }
        }

        //注意:本函数会操作startPosition这个全局变量
        public void addToIndex(BlockIndex index, object tempScan)
        {
            if (jobInfo.jobParams.useStackZDPD())
            {
                TempScanSZDPD ts = (TempScanSZDPD) tempScan;

                index.nums.AddRange(ts.nums);
                index.rts.AddRange(ts.rts);
                index.tics.AddRange(ts.tics);
                index.cvList.AddRange(ts.cvs);
                index.mzs.Add(ts.mzArrayBytes.Length);
                index.ints.Add(ts.intArrayBytes.Length);
                index.tags.Add(ts.tagArrayBytes.Length);
                startPosition = startPosition + ts.mzArrayBytes.Length + ts.tagArrayBytes.Length +
                                ts.intArrayBytes.Length;
                airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                airdStream.Write(ts.tagArrayBytes, 0, ts.tagArrayBytes.Length);
                airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
            }
            else
            {
                TempScan ts = (TempScan) tempScan;

                index.nums.Add(ts.num);
                index.rts.Add(ts.rt);
                index.tics.Add(ts.tic);
                index.cvList.Add(ts.cvs);
                index.mzs.Add(ts.mzArrayBytes.Length);
                index.ints.Add(ts.intArrayBytes.Length);
                startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
                if (ts.mobilityArrayBytes != null)
                {
                    startPosition += ts.mobilityArrayBytes.Length;
                    index.mobilities.Add(ts.mobilityArrayBytes.Length);
                    airdStream.Write(ts.mobilityArrayBytes, 0, ts.mobilityArrayBytes.Length);
                }
                
                // startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length
            }
        }

        protected void readVendorFile()
        {
            jobInfo.log("Prepare to Parse Vendor File", "Prepare");
            msd = new MSDataFile(jobInfo.inputFilePath);
            jobInfo.log("Adapting Vendor File API", "Adapting");

            List<string> filter = new List<string>();
            SpectrumListFactory.wrap(msd, filter); //这一步操作可以帮助加快Wiff文件的初始化速度

            spectrumList = msd.run.spectrumList;
            if (spectrumList == null || spectrumList.empty())
            {
                jobInfo.logError("No Spectra Found");
            }
            else
            {
                totalSize = spectrumList.size();
                jobInfo.log("Adapting Finished, Total Spectra:" + totalSize);
            }

            switch (jobInfo.format)
            {
                case FileFormat.WIFF:
                    FileInfo wiff = new FileInfo(jobInfo.inputFilePath);
                    if (wiff.Exists) fileSize += wiff.Length;
                    FileInfo mtd = new FileInfo(jobInfo.inputFilePath + ".mtd");
                    if (mtd.Exists) fileSize += mtd.Length;
                    FileInfo scan = new FileInfo(jobInfo.inputFilePath + ".scan");
                    if (scan.Exists) fileSize += scan.Length;
                    break;
                case FileFormat.RAW:
                    FileInfo raw = new FileInfo(jobInfo.inputFilePath);
                    if (raw.Exists) fileSize += raw.Length;
                    break;
            }
        }

        //将最终的数据写入文件中
        public void writeToAirdInfoFile()
        {
            jobInfo.log("Writing Index File", "Writing Index File");
            AirdInfo airdInfo = buildBasicInfo();
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                {NullValueHandling = NullValueHandling.Ignore};
            string airdInfoStr = JsonConvert.SerializeObject(airdInfo, jsonSetting);
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            airdJsonStream.Write(airdBytes, 0, airdBytes.Length);
        }

        public void clearCache()
        {
            ranges = new List<WindowRange>();
            indexList = new List<BlockIndex>();
        }
        
        //DDA模式下,key为ms2Index.pNum, DIA模式下,key为ms2Index.precursorMz
        protected void addToMS2Map(Object key, MsIndex ms2Index)
        {
            if (ms2Table.Contains(key))
            {
                (ms2Table[key] as List<MsIndex>).Add(ms2Index);
            }
            else
            {
                List<MsIndex> indexList = new List<MsIndex>();
                indexList.Add(ms2Index);
                ms2Table.Add(key, indexList);
            }
        }

        protected MsIndex parseMS1(Spectrum spectrum, int index)
        {
            MsIndex ms1 = new MsIndex();
            ms1.level = 1;
            ms1.num = index;
            if (spectrum.scanList.scans.Count != 1) return ms1;
            Scan scan = spectrum.scanList.scans[0];
            ms1.cvList = CV.trans(spectrum.cvParams);
            if (scan.cvParams != null) ms1.cvList.AddRange(CV.trans(scan.cvParams));

            ms1.rt = parseRT(scan);
            ms1.tic = parseTIC(spectrum);
            if (msType == null) parseMsType(spectrum);
            if (polarity == null) parsePolarity(spectrum);

            return ms1;
        }

        protected MsIndex parseMS2(Spectrum spectrum, int num, int pNum)
        {
            MsIndex ms2 = new MsIndex();
            ms2.level = 2;
            ms2.pNum = pNum;
            ms2.num = num;

            try
            {
                double precursorMz = parsePrecursorParams(spectrum, CVID.MS_isolation_window_target_m_z);
                double lowerOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_lower_offset);
                double upperOffset = parsePrecursorParams(spectrum, CVID.MS_isolation_window_upper_offset);
                int charge = parsePrecursorCharge(spectrum);
                ms2.precursorCharge = charge;
                ms2.precursorMz = precursorMz;
                ms2.mzStart = precursorMz - lowerOffset;
                ms2.mzEnd = precursorMz + upperOffset;
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

            if (activator == null) parseActivator(spectrum.precursors[0].activation);
            Scan scan = spectrum.scanList.scans[0];

            ms2.cvList = CV.trans(spectrum.cvParams);
            if (scan.cvParams != null) ms2.cvList.AddRange(CV.trans(scan.cvParams));
            ms2.rt = parseRT(scan);
            ms2.tic = parseTIC(spectrum);
            return ms2;
        }

        protected void compressMS1Block()
        {
            BlockIndex index = new BlockIndex();
            index.level = 1;
            index.startPtr = startPosition;
            compressor.compressMS1(this, index);
            index.endPtr = startPosition;
            indexList.Add(index);
        }

        protected void compressMS2BlockForDIA()
        {
            jobInfo.log("Start Processing MS2 List");
            int progress = 0;
            foreach (double precursorMz in ms2Table.Keys)
            {
                List<MsIndex> ms2List = ms2Table[precursorMz] as List<MsIndex>;
                WindowRange range = rangeTable[precursorMz] as WindowRange;

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

        //处理MS2,由于每一个MS1只跟随少量的MS2光谱图,因此DDA采集模式下MS2的压缩模式仍然使用Aird ZDPD的压缩算法
        protected void compressMS2BlockForDDA()
        {
            int progress = 0;
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
                    WindowRange range = new WindowRange(index.mzStart, index.mzEnd, index.precursorMz);
                    if (index.precursorCharge != 0)
                    {
                        range.charge = index.precursorCharge;
                    }
                    ms2Ranges.Add(range);
                    TempScan ts = new TempScan(index.num, index.rt, index.tic, index.cvList);
                    compressor.compress(spectrumList.spectrum(index.num, true), ts);
                    blockIndex.nums.Add(ts.num);
                    blockIndex.rts.Add(ts.rt);
                    blockIndex.tics.Add(ts.tic);
                    blockIndex.cvList.Add(ts.cvs);
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

        protected double parsePrecursorParams(Spectrum spectrum, CVID cvid)
        {
            double result = -1;
            var retryTimes = 3;
            while (result < 0 && retryTimes > 0)
            {
                try
                {
                    Precursor precursor = spectrum.precursors[0];
                    result = double.Parse(precursor.isolationWindow.cvParamChild(cvid).value.ToString());
                }
                catch (FormatException e)
                {
                    jobInfo.log(cvid + "-重试次数-" + retryTimes + "-Result:" + result);
                    jobInfo.log(e.StackTrace);
                }
                retryTimes--;
            }

            if (result < 0)
            {
                throw new Exception("Parse Double Error:" + result);
            }

            return result;
        }

        protected int parsePrecursorCharge(Spectrum spectrum)
        {
            int result = 0;
            var retryTimes = 3;
            while (result < 0 && retryTimes > 0)
            {
                try
                {
                    Precursor precursor = spectrum.precursors[0];
                    if (precursor.selectedIons == null || precursor.selectedIons[0].cvParamChild(CVID.MS_charge_state)
                            .cvid.Equals(CVID.CVID_Unknown))
                    {
                        return 0;
                    }

                    result = int.Parse(precursor.selectedIons[0].cvParamChild(CVID.MS_charge_state).value.ToString());
                }
                catch (FormatException e)
                {
                    jobInfo.log("Charge-重试次数-" + retryTimes + "-Result:" + result);
                    jobInfo.log(e.StackTrace);
                }

                retryTimes--;
            }

            if (result < 0)
            {
                throw new Exception("Parse Integer Error:" + result);
            }

            return result;
        }

        protected AirdInfo buildBasicInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            List<DomainsCore.Aird.Software> softwares = new List<DomainsCore.Aird.Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();

            //Basic Job Info
            airdInfo.airdPath = jobInfo.airdFilePath;
            airdInfo.fileSize = fileSize;
            airdInfo.createDate = new DateTime();
            airdInfo.type = jobInfo.type;
            airdInfo.totalScanCount = msd.run.spectrumList.size();
            airdInfo.creator = jobInfo.jobParams.creator;
            airdInfo.activator = activator;
            airdInfo.energy = energy;
            airdInfo.rtUnit = rtUnit;
            airdInfo.ccsUnit = ccsUnit;
            airdInfo.mobilityType = mobilityType;
            airdInfo.msType = msType;
            airdInfo.polarity = polarity;
            //Scan index and window range info
            airdInfo.rangeList = ranges;

            //Block index
            airdInfo.indexList = indexList;

            //Instrument Info
            List<Instrument> instruments = new List<Instrument>();
            foreach (InstrumentConfiguration ic in msd.instrumentConfigurationList)
            {
                Instrument instrument = new Instrument();
                //仪器设备信息
                if (jobInfo.format.Equals(FileFormat.WIFF))
                {
                    instrument.manufacturer = "SCIEX";
                }

                if (jobInfo.format.Equals(FileFormat.RAW))
                {
                    instrument.manufacturer = "THERMO";
                }

                //设备信息在不同的源文件格式中取法不同,有些是在instrumentConfigurationList中获取,有些是在paramGroups获取,因此出现了以下比较丑陋的写法
                if (ic.cvParams.Count != 0)
                {
                    foreach (CVParam cv in ic.cvParams)
                    {
                        featuresMap.Add(cv.name, cv.value);
                    }

                    instrument.model = ic.cvParams[0].name;
                }
                else if (msd.paramGroups.Count != 0)
                {
                    foreach (ParamGroup pg in msd.paramGroups)
                    {
                        if (pg.cvParams.Count != 0)
                        {
                            foreach (CVParam cv in pg.cvParams)
                            {
                                if (!featuresMap.ContainsKey(cv.name))
                                {
                                    featuresMap.Add(cv.name, cv.value.ToString());
                                }
                            }

                            instrument.model = pg.cvParams[0].name;
                        }
                    }
                }

                foreach (Component component in ic.componentList)
                {
                    switch (component.type)
                    {
                        case ComponentType.ComponentType_Analyzer:
                            foreach (CVParam cv in component.cvParams)
                            {
                                instrument.analyzer.Add(cv.name);
                            }

                            break;
                        case ComponentType.ComponentType_Source:
                            foreach (CVParam cv in component.cvParams)
                            {
                                instrument.source.Add(cv.name);
                            }

                            break;
                        case ComponentType.ComponentType_Detector:
                            foreach (CVParam cv in component.cvParams)
                            {
                                instrument.detector.Add(cv.name);
                            }

                            break;
                    }
                }

                instruments.Add(instrument);
            }

            airdInfo.instruments = instruments;

            //Software Info
            foreach (Software soft in msd.softwareList)
            {
                DomainsCore.Aird.Software software = new DomainsCore.Aird.Software();
                software.name = soft.id;
                software.version = soft.version;
                softwares.Add(software);
            }

            DomainsCore.Aird.Software airdPro = new DomainsCore.Aird.Software();
            airdPro.name = SoftwareInfo.NAME;
            airdPro.version = SoftwareInfo.VERSION;
            airdPro.type = "DataFormatConversion";
            softwares.Add(airdPro);
            airdInfo.softwares = softwares;

            //Parent Files Info
            foreach (SourceFile sf in msd.fileDescription.sourceFiles)
            {
                ParentFile file = new ParentFile();
                file.name = sf.name;
                file.location = sf.location;
                file.formatType = sf.id;
                parentFiles.Add(file);
            }

            airdInfo.parentFiles = parentFiles;

            //Compressor Info
            List<Compressor> coms = new List<Compressor>();
            Compressor mzCompressor = new Compressor(Compressor.TARGET_MZ);
            Compressor intCompressor = new Compressor(Compressor.TARGET_INTENSITY);
            switch (jobInfo.jobParams.airdAlgorithm)
            {
                case CompressorType.ZDPD:
                    mzCompressor.addMethod(Compressor.METHOD_ZDPD);
                    mzCompressor.precision = jobInfo.jobParams.mzPrecision;
                    intCompressor.addMethod(Compressor.METHOD_ZLIB);
                    intCompressor.precision = 10;
                    break;
                case CompressorType.ZDVB:
                    mzCompressor.addMethod(Compressor.METHOD_ZDVB);
                    mzCompressor.precision = jobInfo.jobParams.mzPrecision;
                    intCompressor.addMethod(Compressor.METHOD_ZVB);
                    intCompressor.precision = 1;
                    break;
                case CompressorType.StackZDPD:
                    mzCompressor.addMethod(Compressor.METHOD_STACK_ZDPD);
                    mzCompressor.precision = jobInfo.jobParams.mzPrecision;
                    mzCompressor.digit = jobInfo.jobParams.digit;
                    intCompressor.addMethod(Compressor.METHOD_ZLIB);
                    intCompressor.precision = 10;
                    break;
            }

            coms.Add(mzCompressor);
            coms.Add(intCompressor);

            airdInfo.compressors = coms;

            airdInfo.ignoreZeroIntensityPoint = jobInfo.jobParams.ignoreZeroIntensity;
            //Features Info
            featuresMap.Add(Features.raw_id, msd.id);
            featuresMap.Add(Features.ignore_zero_intensity, jobInfo.jobParams.ignoreZeroIntensity);
            featuresMap.Add(Features.source_file_format, jobInfo.format);
            featuresMap.Add(Features.byte_order, ByteOrder.LITTLE_ENDIAN);
            featuresMap.Add(Features.aird_algorithm, jobInfo.jobParams.getAirdAlgorithmStr());
            airdInfo.features = FeaturesUtil.toString(featuresMap);
            airdInfo.version = SoftwareInfo.VERSION;
            airdInfo.versionCode = SoftwareInfo.VERSION_CODE;
            return airdInfo;
        }
    }
}