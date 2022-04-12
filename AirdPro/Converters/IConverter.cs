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
using AirdPro.Utils;
using Newtonsoft.Json;
using pwiz.CLI.analysis;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AirdPro.Algorithms;
using AirdPro.Domains;
using AirdSDK.Compressor;
using AirdSDK.Domains;
using pwiz.CLI.cv;
using ByteOrder = AirdPro.Constants.ByteOrder;
using CV = AirdSDK.Domains.CV;
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

        protected Hashtable
            ms2Table = Hashtable.Synchronized(new Hashtable()); //用于存放MS2的索引信息,DDA采集模式下key为ms1的num, DIA采集模式下key为mz

        public List<MsIndex> ms1List = new List<MsIndex>(); //用于存放MS1索引及基础信息,泛型为MsIndex
        protected Hashtable featuresMap = new Hashtable();
        public ICompressor compressor;

        public double[] mobiArray;
        public Dictionary<double, int> mobiDict;
        public MobiInfo mobiInfo = new MobiInfo();

        protected long fileSize; //厂商文件大小
        protected long startPosition = 0; //文件指针
        protected int totalSize; //总计的谱图数目

        protected string activator; //HCD,CID....
        protected float energy; //轰击能
        protected string msType; //Profile, Centroided
        protected string polarity; //Negative, Positive
        protected string rtUnit; //Minute, Second
        protected int intensityPrecision = 1; //Intensity默认精确到个位数

        protected int spectraNumForIntensityPrecisionPredict = 5;
        protected int spectraNumForCombinableCompressorsPredict = 100;

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
            jobInfo.log("Finished! Total Cost: " + stopwatch.Elapsed.TotalSeconds + " seconds", "Finished");
        }

        protected void initDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdFilePath));
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdJsonFilePath));
        }

        public void initBrukerMobi()
        {
            jobInfo.log("Init Mobility Array");
            long handle = TdfUtil.tims_open(jobInfo.inputPath, 1);
            double[] scanNums = new double[2000];
            for (int i = 0; i < scanNums.Length; i++)
            {
                scanNums[i] = i;
            }

            double[] mobility = new double[2000];
            TdfUtil.tims_scannum_to_oneoverk0(handle, 1, scanNums, mobility, scanNums.Length);
            TdfUtil.tims_close(handle);
            mobiDict = new Dictionary<double, int>();
            for (short i = 0; i < mobility.Length; i++)
            {
                mobiDict.Add(mobility[i], i);
            }

            mobiArray = mobility;
            compressor.mobiDict = mobiDict;
        }

        protected void predictForCombinableComps()
        {
            jobInfo.log("predict for combinable compressors", "predicting");
            randomSampling(spectraNumForCombinableCompressorsPredict, jobInfo.ionMobility);
        }

        /**
         * num:采样数目,建议:5
         */
        protected void predictForIntensityPrecision()
        {
            Random rd = new Random();
            HashSet<int> nums = new HashSet<int>();
            for (int i = 0; i < spectraNumForIntensityPrecisionPredict; i++)
            {
                nums.Add(rd.Next(1, totalSize));
            }

            bool findIt = false;
            for (var i = 0; i < nums.Count; i++)
            {
                Spectrum spectrum = spectrumList.spectrum(i, true);
                foreach (double d in spectrum.getIntensityArray().data.Storage())
                {
                    if ((d - (int) d) != 0) //如果随机采集到的intensity是精确到小数点后一位的,精确确定为10,即精确到小数点后一位
                    {
                        findIt = true;
                        break;
                    }
                }

                if (findIt)
                {
                    break;
                }
            }

            if (findIt)
            {
                intensityPrecision = 10;
            }
            else
            {
                intensityPrecision = 1;
            }

            compressor.intensityPrecision = intensityPrecision;
            jobInfo.log("Intensity Precision:" + intensityPrecision);
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

        protected void parseMobility(Scan scan)
        {
            if (mobiInfo.unit != null && mobiInfo.type != null)
            {
                return;
            }

            // float mobility = 0f;
            if (scan.hasCVParamChild(CVID.MS_inverse_reduced_ion_mobility))
            {
                CVParam cv = scan.cvParamChild(CVID.MS_inverse_reduced_ion_mobility);
                // mobility = float.Parse(cv.value.ToString());
                mobiInfo.unit = cv.unitsName;
                mobiInfo.type = MobilityType.TIMS;
            }
            else if (scan.hasCVParamChild(CVID.MS_ion_mobility_drift_time))
            {
                CVParam cv = scan.cvParamChild(CVID.MS_ion_mobility_drift_time);
                // mobility = float.Parse(cv.value.ToString());
                mobiInfo.unit = cv.unitsName;
                mobiInfo.type = MobilityType.DTIMS;
            }
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
            if (polarity != null)
            {
                return;
            }

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
            if (msType != null)
            {
                return;
            }

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

            if (activator != null)
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
            if (jobInfo.config.stack)
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
            }
        }

        protected void readVendorFile()
        {
            jobInfo.log("Prepare to Parse Vendor File", "Prepare");
            ReaderList readerList = ReaderList.FullReaderList;
            var readerConfig = new ReaderConfig
            {
                combineIonMobilitySpectra = true,
                ignoreZeroIntensityPoints = jobInfo.config.ignoreZeroIntensity
            };

            MSDataList msInfo = new MSDataList();
            readerList.read(jobInfo.inputPath, msInfo, readerConfig);
            if (msInfo == null || msInfo.Count == 0)
            {
                jobInfo.logError("Reading Vendor File Error, Run is Null");
                return;
            }

            msd = msInfo[0];
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
                    FileInfo wiff = new FileInfo(jobInfo.inputPath);
                    if (wiff.Exists) fileSize += wiff.Length;
                    FileInfo mtd = new FileInfo(jobInfo.inputPath + ".mtd");
                    if (mtd.Exists) fileSize += mtd.Length;
                    FileInfo scan = new FileInfo(jobInfo.inputPath + ".scan");
                    if (scan.Exists) fileSize += scan.Length;
                    break;
                case FileFormat.RAW:
                    FileInfo raw = new FileInfo(jobInfo.inputPath);
                    if (raw.Exists) fileSize += raw.Length;
                    break;
            }
        }

        //将最终的数据写入文件中
        public void writeToAirdInfoFile()
        {
            jobInfo.log("Writing Index File", "Writing Index File");
            AirdInfo airdInfo = buildBasicInfo();
            string airdInfoStr = JsonConvert.SerializeObject(airdInfo, new JsonSerializerSettings
                {NullValueHandling = NullValueHandling.Ignore});
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
            parseMobility(scan);
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

            parseActivator(spectrum.precursors[0].activation);
            Scan scan = spectrum.scanList.scans[0];

            ms2.cvList = CV.trans(spectrum.cvParams);
            if (scan.cvParams != null) ms2.cvList.AddRange(CV.trans(scan.cvParams));
            ms2.rt = parseRT(scan);
            ms2.tic = parseTIC(spectrum);
            return ms2;
        }

        protected void compressMobiDict()
        {
            int[] mobiIntArray = new int[mobiArray.Length];
            for (var i = 0; i < mobiArray.Length; i++)
            {
                mobiIntArray[i] = (int) Math.Round(mobiArray[i] * AirdInfo.PRECISION_MOBI);
            }

            byte[] compressedMobiData =
                new ZstdWrapper().encode(ByteTrans.intToByte(new IntegratedVarByteWrapper().encode(mobiIntArray)));
            mobiInfo.dictStart = startPosition;
            startPosition += compressedMobiData.Length;
            airdStream.Write(compressedMobiData, 0, compressedMobiData.Length);
            mobiInfo.dictEnd = startPosition;
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
                    if (precursor.isolationWindow.hasCVParamChild(cvid))
                    {
                        result = Double.Parse(precursor.isolationWindow.cvParamChild(cvid).value.ToString());
                    }
                    else
                    {
                        result = 0;
                    }
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
            List<AirdSDK.Domains.Software> softwares = new List<AirdSDK.Domains.Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();

            //Basic Job Info
            airdInfo.airdPath = jobInfo.airdFilePath;
            airdInfo.fileSize = fileSize;
            airdInfo.createDate = new DateTime();
            airdInfo.type = jobInfo.type;
            airdInfo.totalScanCount = msd.run.spectrumList.size();
            airdInfo.creator = jobInfo.config.creator;
            airdInfo.activator = activator;
            airdInfo.energy = energy;
            airdInfo.rtUnit = rtUnit;

            airdInfo.mobiInfo = mobiInfo;
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
                AirdSDK.Domains.Software software = new AirdSDK.Domains.Software();
                software.name = soft.id;
                software.version = soft.version;
                softwares.Add(software);
            }

            AirdSDK.Domains.Software airdPro = new AirdSDK.Domains.Software();
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
            Compressor mobiCompressor = new Compressor(Compressor.TARGET_MOBILITY);
            if (jobInfo.config.stack)
            {
                mzCompressor.addMethod(jobInfo.config.mzIntComp.ToString());
                mzCompressor.addMethod(jobInfo.config.mzByteComp.ToString());
                mzCompressor.precision = jobInfo.config.mzPrecision;
                mzCompressor.digit = jobInfo.config.digit;
                intCompressor.addMethod(jobInfo.config.intByteComp.ToString());
                intCompressor.precision = 10;
            }
            else
            {
                mzCompressor.addMethod(jobInfo.config.mzIntComp.ToString());
                mzCompressor.addMethod(jobInfo.config.mzByteComp.ToString());
                mzCompressor.precision = jobInfo.config.mzPrecision;

                intCompressor.addMethod(jobInfo.config.intIntComp.ToString());
                intCompressor.addMethod(jobInfo.config.intByteComp.ToString());
                mobiCompressor.precision = intensityPrecision;

                mobiCompressor.addMethod(jobInfo.config.mobiIntComp.ToString());
                mobiCompressor.addMethod(jobInfo.config.mobiByteComp.ToString());
            }

            coms.Add(mzCompressor);
            coms.Add(intCompressor);
            coms.Add(mobiCompressor);
            airdInfo.compressors = coms;

            airdInfo.ignoreZeroIntensityPoint = jobInfo.config.ignoreZeroIntensity;
            //Features Info
            featuresMap.Add(Features.raw_id, msd.id);
            featuresMap.Add(Features.ignore_zero_intensity, jobInfo.config.ignoreZeroIntensity);
            featuresMap.Add(Features.source_file_format, jobInfo.format);
            featuresMap.Add(Features.byte_order, ByteOrder.LITTLE_ENDIAN);
            featuresMap.Add(Features.aird_algorithm, jobInfo.getCompressorStr());
            airdInfo.features = FeaturesUtil.toString(featuresMap);
            airdInfo.version = SoftwareInfo.VERSION;
            airdInfo.versionCode = SoftwareInfo.VERSION_CODE;
            return airdInfo;
        }

        List<ByteComp> byteCompList = new List<ByteComp>()
        {
            new BrotliWrapper(), new SnappyWrapper(), new ZstdWrapper(), new ZlibWrapper()
        };

        List<SortedIntComp> integratedIntCompList = new List<SortedIntComp>()
        {
            new IntegratedVarByteWrapper(), new IntegratedBinPackingWrapper(),
            new IntegratedNewPFDWrapper(), new IntegratedOptPFDWrapper()
        };

        List<IntComp> intCompList = new List<IntComp>()
        {
            new VarByteWrapper(), new BinPackingWrapper(), new NewPFDWrapper(), new OptPFDWrapper(), new Empty()
        };

        public void randomSampling(int randomNum, bool ionMobi)
        {
            List<int[]> mzArrays = new List<int[]>();
            List<int[]> intensityArrays = new List<int[]>();
            List<int[]> mobiNoArrays = new List<int[]>();
            Random rn = new Random();
            List<int> logIndexes = new List<int>();
            for (var i = 0; i < randomNum; i++)
            {
                int index = rn.Next(0, totalSize);
                logIndexes.Add(index);
                List<int[]> dataList = fetchSpectrum(index, ionMobi);
                mzArrays.Add(dataList[0]);
                intensityArrays.Add(dataList[1]);
                if (ionMobi)
                {
                    mobiNoArrays.Add(dataList[2]);
                }

                Console.Write(index + "-");
            }

            compressForTargetArrays(mzArrays, intensityArrays, mobiNoArrays, ionMobi);
        }

        public List<int[]> fetchSpectrum(int index, bool mobi)
        {
            List<int[]> arrays = new List<int[]>();
            Spectrum spectrum = spectrumList.spectrum(index, true);
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();

            var size = mzData.Length;
            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int[] mobilityNoArray = new int[size];

            if (mobi)
            {
                double[] mobiData = SpectrumUtil.getMobilityData(spectrum);
                TimsData[] dataArray = new TimsData[size];
                for (int t = 0; t < size; t++)
                {
                    dataArray[t] = new TimsData(mobiDict[mobiData[t]], mzData[t], intData[t]);
                }

                Array.Sort(dataArray, (p1, p2) => p1.mz.CompareTo(p2.mz));
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(dataArray[i].mz * jobInfo.config.mzPrecision);
                    intensityArray[i] = Convert.ToInt32(Math.Round(dataArray[i].intensity * intensityPrecision));
                    mobilityNoArray[i] = dataArray[i].mobilityNo;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(mzData[i] * jobInfo.config.mzPrecision);
                    intensityArray[i] = SpectrumUtil.fetchIntensity(intData[i], intensityPrecision);
                }
            }

            arrays.Add(mzArray);
            arrays.Add(intensityArray);
            arrays.Add(mobilityNoArray);
            return arrays;
        }

        public void compressForTargetArrays(List<int[]> mzArrays, List<int[]> intensityArrays,
            List<int[]> mobilityNoArrays, bool ionMobi)
        {
            Dictionary<string, long> compressTimeMap = new Dictionary<string, long>();
            Dictionary<string, long> decompressTimeMap = new Dictionary<string, long>();
            Dictionary<string, long> sizeMap = new Dictionary<string, long>();

            foreach (SortedIntComp intComp4Mz in integratedIntCompList)
            {
                foreach (ByteComp byteComp4Mz in byteCompList)
                {
                    compressTimeMap.Add(buildComboKey("mz", intComp4Mz.getName(), byteComp4Mz.getName()), 0);
                    decompressTimeMap.Add(buildComboKey("mz", intComp4Mz.getName(), byteComp4Mz.getName()), 0);
                    sizeMap.Add(buildComboKey("mz", intComp4Mz.getName(), byteComp4Mz.getName()), 0);
                }
            }

            foreach (IntComp intComp4Intensity in intCompList)
            {
                foreach (ByteComp byteComp4Intensity in byteCompList)
                {
                    compressTimeMap.Add(
                        buildComboKey("intensity", intComp4Intensity.getName(), byteComp4Intensity.getName()), 0);
                    decompressTimeMap.Add(
                        buildComboKey("intensity", intComp4Intensity.getName(), byteComp4Intensity.getName()), 0);
                    sizeMap.Add(buildComboKey("intensity", intComp4Intensity.getName(), byteComp4Intensity.getName()),
                        0);
                }
            }

            if (ionMobi)
            {
                foreach (IntComp intComp4Mobi in intCompList)
                {
                    foreach (ByteComp byteComp4Mobi in byteCompList)
                    {
                        compressTimeMap.Add(buildComboKey("mobi", intComp4Mobi.getName(), byteComp4Mobi.getName()), 0);
                        decompressTimeMap.Add(buildComboKey("mobi", intComp4Mobi.getName(), byteComp4Mobi.getName()),
                            0);
                        sizeMap.Add(buildComboKey("mobi", intComp4Mobi.getName(), byteComp4Mobi.getName()), 0);
                    }
                }
            }

            long originSizeMz = mzArrays.Count * 4;
            long originSizeIntensity = intensityArrays.Count * 4;
            long originSizeMobi = mobilityNoArrays.Count * 4;

            foreach (SortedIntComp intComp4Mz in integratedIntCompList)
            {
                foreach (ByteComp byteComp4Mz in byteCompList)
                {
                    StatUtil.stat4OneComboComp(intComp4Mz, byteComp4Mz, mzArrays, "mz", sizeMap, compressTimeMap,
                        decompressTimeMap);
                }
            }

            foreach (IntComp intComp4Intensity in intCompList)
            {
                foreach (ByteComp byteComp4Intensity in byteCompList)
                {
                    StatUtil.stat4OneComboComp(intComp4Intensity, byteComp4Intensity, intensityArrays, "intensity",
                        sizeMap,
                        compressTimeMap,
                        decompressTimeMap);
                }
            }

            if (ionMobi)
            {
                foreach (IntComp intComp4Mobi in intCompList)
                {
                    foreach (ByteComp byteComp4Mobi in byteCompList)
                    {
                        StatUtil.stat4OneComboComp(intComp4Mobi, byteComp4Mobi, mobilityNoArrays, "mobi", sizeMap,
                            compressTimeMap,
                            decompressTimeMap);
                    }
                }
            }

            List<CompressStat> mzStatList = new List<CompressStat>();
            List<CompressStat> intensityStatList = new List<CompressStat>();
            List<CompressStat> mobiStatList = new List<CompressStat>();
            foreach (KeyValuePair<string, long> pair in sizeMap)
            {
                string key = pair.Key;
                CompressStat stat = new CompressStat(key, sizeMap[key], compressTimeMap[key], decompressTimeMap[key]);
                if (key.StartsWith("mz"))
                {
                    stat.key.Replace("mz-", "");
                    mzStatList.Add(stat);
                }

                if (pair.Key.StartsWith("intensity"))
                {
                    stat.key = stat.key.Replace("intensity-", "");
                    intensityStatList.Add(stat);
                }

                if (pair.Key.StartsWith("mobi"))
                {
                    stat.key = stat.key.Replace("mobi-", "");
                    mobiStatList.Add(stat);
                }
            }

            int bestIndex4Mz = StatUtil.calcBestIndex(mzStatList);
            int bestIndex4Intensity = StatUtil.calcBestIndex(intensityStatList);
            if (ionMobi)
            {
                int bestIndex4Mobi = StatUtil.calcBestIndex(mobiStatList);
                jobInfo.log($@"Origin Size:{originSizeMz}-{originSizeIntensity}-{originSizeMobi}");
                jobInfo.log(@"------------------------");
                jobInfo.log(
                    $@"Best Combo Comp:{mzStatList[bestIndex4Mz].key}-{intensityStatList[bestIndex4Intensity].key}-{mobiStatList[bestIndex4Mobi].key}");
            }


            jobInfo.log($@"Origin Size:{originSizeMz}-{originSizeIntensity}");
            jobInfo.log(@"------------------------");
            jobInfo.log(
                $@"Best Combo Comp:{mzStatList[bestIndex4Mz].key}-{intensityStatList[bestIndex4Intensity].key}");
            Console.WriteLine(JsonConvert.SerializeObject(mzStatList, new JsonSerializerSettings
                {NullValueHandling = NullValueHandling.Ignore}));
            Console.WriteLine(JsonConvert.SerializeObject(intensityStatList, new JsonSerializerSettings
                {NullValueHandling = NullValueHandling.Ignore}));
            Console.WriteLine(JsonConvert.SerializeObject(mobiStatList, new JsonSerializerSettings
                {NullValueHandling = NullValueHandling.Ignore}));
            jobInfo.log(@"------------------------");
        }

        public string buildComboKey(string key, string intCompName, string byteCompName)
        {
            return key + "-" + intCompName + "-" + byteCompName;
        }
    }
}