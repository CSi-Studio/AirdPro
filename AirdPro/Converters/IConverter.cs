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
using AirdPro.Domains.Convert;
using AirdPro.Utils;
using Newtonsoft.Json;
using pwiz.CLI.analysis;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AirdPro.Algorithms;
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
        
        protected List<BlockIndex> indexList = new List<BlockIndex>(); //用于存储的全局的SWATH List
        protected Hashtable ms2Table = Hashtable.Synchronized(new Hashtable()); //用于存放MS2的索引信息,key为mz
        public List<MsIndex> ms1List = new List<MsIndex>(); //用于存放MS1索引及基础信息
        protected Hashtable featuresMap = new Hashtable();
        public ICompressor compressor;
        protected int ms1Size = 0;
        protected long fileSize; //厂商文件大小
        protected long startPosition; //文件指针
        protected int totalSize; //总计的谱图数目

        protected string activator; //HCD,CID....
        protected float energy; //轰击能
        protected string msType; //Profile, Centroided
        protected string polarity; //Negative, Positive
        protected string rtUnit; //Minute, Second


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
            jobInfo.log("Finished! Total Cost: " + stopwatch.Elapsed.TotalSeconds+" seconds", "Finished");
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
            if (!spectrum.cvParamChild(CVID.MS_profile_spectrum).cvid.Equals(CVID.CVID_Unknown)){
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



        public void outputWithOrder(Hashtable table, BlockIndex index)
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
                TempScanSZDPD ts = (TempScanSZDPD)tempScan;

                index.nums.AddRange(ts.nums);
                index.rts.AddRange(ts.rts);
                index.tics.AddRange(ts.tics);
                index.cvList.AddRange(ts.cvs);
                index.mzs.Add(ts.mzArrayBytes.Length);
                index.ints.Add(ts.intArrayBytes.Length);
                index.tags.Add(ts.tagArrayBytes.Length);
                startPosition = startPosition + ts.mzArrayBytes.Length + ts.tagArrayBytes.Length + ts.intArrayBytes.Length;
                airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                airdStream.Write(ts.tagArrayBytes, 0, ts.tagArrayBytes.Length);
                airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
            }
            else
            {
                TempScan ts = (TempScan)tempScan;

                index.nums.Add(ts.num);
                index.rts.Add(ts.rt);
                index.tics.Add(ts.tic);
                index.cvList.Add(ts.cvs);
                index.mzs.Add(ts.mzArrayBytes.Length);
                index.ints.Add(ts.intArrayBytes.Length);
                startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
            }
        }

        protected double getPrecursorIsolationWindowParams(Spectrum spectrum, CVID cvid)
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
                    jobInfo.log(cvid + "-重试次数-" + retryTimes+"-Result:"+result);
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

        protected int getPrecursorCharge(Spectrum spectrum)
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
                jobInfo.log("Adapting Finished");
            }

            if (jobInfo.format.Equals(FileFormat.WIFF))
            {
                FileInfo file1 = new FileInfo(jobInfo.inputFilePath);
                if (file1.Exists)
                {
                    fileSize += file1.Length;
                }
             
                FileInfo file2 = new FileInfo(jobInfo.inputFilePath+".mtd");
                if (file2.Exists)
                {
                    fileSize += file2.Length;
                }

                FileInfo file3 = new FileInfo(jobInfo.inputFilePath + ".scan");
                if (file3.Exists)
                {
                    fileSize += file3.Length;
                }
            }

            if (jobInfo.format.Equals(FileFormat.RAW))
            {
                FileInfo file1 = new FileInfo(jobInfo.inputFilePath);
                if (file1.Exists)
                {
                    fileSize += file1.Length;
                }
            }
        }

        //将最终的数据写入文件中
        public void writeToAirdInfoFile()
        {
            jobInfo.log("Writing Index File", "Writing Index File");
            AirdInfo airdInfo = buildBasicInfo();
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string airdInfoStr = JsonConvert.SerializeObject(airdInfo, jsonSetting);
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            airdJsonStream.Write(airdBytes, 0, airdBytes.Length);
        }

        public void clearCache()
        {
            ranges = new List<WindowRange>();
            indexList = new List<BlockIndex>();
        }

        protected void addToMS2Map(MsIndex ms2Index)
        {
            if (ms2Table.Contains(ms2Index.mz))
            {
                (ms2Table[ms2Index.mz] as List<MsIndex>).Add(ms2Index);
            }
            else
            {
                List<MsIndex> indexList = new List<MsIndex>();
                indexList.Add(ms2Index);
                ms2Table.Add(ms2Index.mz, indexList);
            }
        }

        protected MsIndex parseMS1(Spectrum spectrum, int index)
        {
            MsIndex ms1 = new MsIndex();
            ms1.level = 1;
            ms1.num = index;
           
            if (spectrum.scanList.scans.Count != 1)
            {
                return ms1;
            }

            Scan scan = spectrum.scanList.scans[0];
            ms1.rt = parseRT(scan);
            ms1.tic = parseTIC(spectrum);
            ms1.cvList = CV.trans(spectrum);
            if (scan.cvParams != null)
            {
                ms1.cvList.AddRange(CV.trans(scan.cvParams));
            }
            
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

        protected MsIndex parseMS2(Spectrum spectrum, int index, int parentIndex)
        {
            MsIndex ms2 = new MsIndex();
            ms2.level = 2;
            ms2.pNum = parentIndex;
            ms2.num = index;
           
            try
            {
                double mz = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_target_m_z);
                double lowerOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_lower_offset);
                double upperOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_upper_offset);
                int charge = getPrecursorCharge(spectrum);
                ms2.charge = charge;
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
            if (activator == null)
            {
                parseActivator(spectrum.precursors[0].activation);
            }

            if (spectrum.scanList.scans.Count != 1) return ms2;
            Scan scan = spectrum.scanList.scans[0];
           
            ms2.cvList = CV.trans(spectrum);
            if (scan.cvParams != null)
            {
                ms2.cvList.AddRange(CV.trans(scan.cvParams));
            }
            
            ms2.rt = parseRT(scan);
            ms2.tic = parseTIC(spectrum);
            return ms2;
        }

        protected void parseAndStoreMS1Block()
        {
            BlockIndex index = new BlockIndex();
            index.level = 1;
            index.startPtr = startPosition;
            compressor.compressMS1(this,index);
            index.endPtr = startPosition;
            indexList.Add(index);
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
                case 1: //ZDPD
                    mzCompressor.addMethod(Compressor.METHOD_ZDPD);
                    mzCompressor.precision = jobInfo.jobParams.mzPrecision;
                    intCompressor.addMethod(Compressor.METHOD_ZLIB);
                    intCompressor.precision = 10;
                    break;
                case 2: //ZDVB
                    mzCompressor.addMethod(Compressor.METHOD_ZDVB);
                    mzCompressor.precision = jobInfo.jobParams.mzPrecision;
                    intCompressor.addMethod(Compressor.METHOD_ZVB);
                    intCompressor.precision = 1;
                    break;
                case 3: //Stack-ZDPD
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
