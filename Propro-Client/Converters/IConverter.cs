using AirdPro.Constants;
using AirdPro.Domains.Aird;
using AirdPro.Domains.Convert;
using AirdPro.Utils;
using Newtonsoft.Json;
using pwiz.CLI.analysis;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using pwiz.CLI.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ByteOrder = AirdPro.Constants.ByteOrder;
using DataProcessing = pwiz.CLI.msdata.DataProcessing;
using Software = pwiz.CLI.msdata.Software;

namespace AirdPro.Converters
{
    public abstract class IConverter
    {
        private static readonly object calculateSHA1Mutex = new object();
        protected MSData msd;
        protected SpectrumList spectrumList;
        protected JobInfo jobInfo;
        protected Stopwatch stopwatch = new Stopwatch();
        protected FileStream airdStream;
        protected FileStream airdJsonStream;
        protected List<WindowRange> ranges = new List<WindowRange>();//SWATH Window的窗口
        protected List<BlockIndex> indexList = new List<BlockIndex>();//用于存储的全局的SWATH List
        protected Hashtable ms2Table = Hashtable.Synchronized(new Hashtable());//用于存放MS2的索引信息,key为mz

        protected List<TempIndex> ms1List = new List<TempIndex>(); //用于存放MS1索引及基础信息
        protected Hashtable featuresMap = new Hashtable();
        protected long fileSize;
        protected long startPosition;//文件指针
        protected int totalSize;//总计的谱图数目

        protected int mzPrecision;
        // protected Hashtable ms2IntTable = Hashtable.Synchronized(new Hashtable());//用于存放MS2的intensity编码表,key为intensity,value为自增键
        // protected SortedSet<float> ms2IntensitySet = new SortedSet<float>();//用于存放MS2的mz编码表
        //
        // protected Hashtable ms2mzTable = Hashtable.Synchronized(new Hashtable());//用于存放MS2的mz编码表,key为mz,value为自增键
        // protected SortedSet<int> ms2mzSet = new SortedSet<int>();//用于存放MS2的mz编码表

        protected static double log2 = Math.Log(2);
        public IConverter(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            mzPrecision = (int)Math.Ceiling(1 / jobInfo.jobParams.mzPrecision);
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
            jobInfo.log("Finished! Total Cost: " + stopwatch.Elapsed.TotalSeconds+" seconds", "Finished");
        }

        protected void calculateSHA1()
        {
            lock (calculateSHA1Mutex)
            {
                jobInfo.log("SHA1 Checking", "SHA1 Checking");
                MSDataFile.calculateSHA1Checksums(msd);
            }
        }

        protected string getMsLevel(int index)
        {
            return spectrumList.spectrum(index).cvParamChild(CVID.MS_ms_level).value.ToString();
        }

        protected string getMsLevel(Spectrum spectrum)
        {
            return spectrum.cvParamChild(CVID.MS_ms_level).value.ToString();
        }

        protected void initDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdFilePath));
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdJsonFilePath));
        }

        protected float parseRT(Scan scan)
        {
            CVParam cv = scan.cvParamChild(CVID.MS_scan_start_time);
            float time = float.Parse(cv.value.ToString());
            if (cv.unitsName.Equals("minute"))
            {
                time = time * 60;
            }

            return Convert.ToSingle(Math.Round(time, 3));
        }

        protected void compress(Spectrum spectrum, TempScan ts)
        {
            BinaryDataDouble mzData = spectrum.getMZArray().data;
            BinaryDataDouble intData = spectrum.getIntensityArray().data;
            
            List<int> mzList = new List<int>();
            List<float> intensityList = new List<float>();
            for (int t = 0; t < mzData.Count; t++)
            {
                if (jobInfo.jobParams.ignoreZeroIntensity && intData[t] == 0) continue;
            
                int mz = Convert.ToInt32(mzData[t] * mzPrecision);
                mzList.Add(mz);
            
                if (jobInfo.jobParams.log2)
                {
                    intensityList.Add(Convert.ToSingle(Math.Round(Math.Log(intData[t])/log2, 3))); //取log10并且精确到小数点后3位
                }
                else
                {
                    float intensity = Convert.ToSingle(Math.Round(intData[t], 1));
                    intensityList.Add(intensity);//精确到小数点后一位
                }
            }

            // float[] intensityArray = intensityList.ToArray();
            // int[] mzArray = CompressUtil.fastPForEncoder(mzList.ToArray());
            int[] compressedMzArray = CompressUtil.fastPForEncoder(mzList.ToArray());
            ts.mzArrayBytes = CompressUtil.zlibEncoder(compressedMzArray);
            ts.intArrayBytes = CompressUtil.zlibEncoder(intensityList.ToArray());
        }

        protected void outputWithOrder(Hashtable table, BlockIndex index)
        {
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                TempScan tempScan = (TempScan)table[key];
                addToIndex(index, tempScan);
            }
        }

        //注意:本函数会操作startPosition这个全局变量
        protected void addToIndex(BlockIndex index, TempScan ts)
        {
            index.nums.Add(ts.num);
            index.rts.Add(ts.rt);
            index.mzs.Add(ts.mzArrayBytes.Length);
            index.ints.Add(ts.intArrayBytes.Length);
            startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
            airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
            airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
        }

        protected double getPrecursorIsolationWindowParams(Spectrum spectrum, CVID cvid)
        {
            double result = -1;
            int retryTimes = 3;
            while (result == -1 && retryTimes > 0)
            {
                try
                {
                    result = double.Parse(spectrum.precursors[0].isolationWindow.cvParamChild(cvid).value.ToString());
                }
                catch (Exception e)
                {
                    jobInfo.logError(e.StackTrace);
                    jobInfo.logError(cvid.GetTypeCode()+"-重试次数-" +retryTimes);
                }
                retryTimes--;
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
                jobInfo.logError("No Spectrums Found");
            }
            else
            {
                jobInfo.log("Adapting Finished");
            }

            if (jobInfo.format.Equals("WIFF"))
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

            if (jobInfo.format.Equals("RAW"))
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

        protected void addToMS2Map(TempIndex ms2Index)
        {
            if (ms2Table.Contains(ms2Index.mz))
            {
                (ms2Table[ms2Index.mz] as List<TempIndex>).Add(ms2Index);
            }
            else
            {
                List<TempIndex> indexList = new List<TempIndex>();
                indexList.Add(ms2Index);
                ms2Table.Add(ms2Index.mz, indexList);
            }
        }

        protected TempIndex parseMS1(Spectrum spectrum, int index)
        {
            TempIndex ms1 = new TempIndex();
            ms1.level = 1;
            ms1.num = index;
            if (spectrum.scanList.scans.Count != 1)
            {
                return ms1;
            }

            Scan scan = spectrum.scanList.scans[0];
            ms1.rt = parseRT(scan);

            return ms1;
        }

        protected TempIndex parseMS2(Spectrum spectrum, int index, int parentIndex)
        {
            TempIndex ms2 = new TempIndex();
            ms2.level = 2;
            ms2.pNum = parentIndex;
            ms2.num = index;
            try
            {
                double mz = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_target_m_z);
                double lowerOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_lower_offset);
                double upperOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_upper_offset);

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
            ms2.rt = parseRT(scan);

            return ms2;
        }

        protected void parseAndStoreMS1Block()
        {
            BlockIndex index = new BlockIndex();
            index.level = 1;
            index.startPtr = startPosition;

            if (jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, ms1List.Count, (i, ParallelLoopState) =>
                {
                    jobInfo.log(null, "MS1:" + i + "/" + ms1List.Count);

                    TempIndex scanIndex = ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt);
                    compress(spectrumList.spectrum(scanIndex.num, true), ts);
                    table.Add(i, ts);
                });
                outputWithOrder(table, index);
            }
            else
            {
                for (int i = 0; i < ms1List.Count; i++)
                {
                    jobInfo.log(null, "MS1:" + i + "/" + ms1List.Count);
                    TempIndex scanIndex = ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt);
                    compress(spectrumList.spectrum(scanIndex.num, true), ts);
                    addToIndex(index, ts);
                }
            }

            index.endPtr = startPosition;
            indexList.Add(index);
        }

        protected AirdInfo buildBasicInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            List<Domains.Aird.Software> softwares = new List<Domains.Aird.Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();

            //Basic Job Info
            airdInfo.airdPath = jobInfo.airdFilePath;
            airdInfo.fileSize = fileSize;
            airdInfo.createDate = new DateTime();
            airdInfo.type = jobInfo.type;
            airdInfo.totalScanCount = msd.run.spectrumList.size();
            airdInfo.creator = jobInfo.jobParams.creator;

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
                if (jobInfo.format.Equals("WIFF"))
                {
                    instrument.manufacturer = "SCIEX";
                }
                if (jobInfo.format.Equals("RAW"))
                {
                    instrument.manufacturer = "THERMO FISHER";
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
            foreach (Software sof in msd.softwareList)
            {
                Domains.Aird.Software software = new Domains.Aird.Software();
                software.name = sof.id;
                software.version = sof.version;
                softwares.Add(software);
            }
            Domains.Aird.Software airdPro = new Domains.Aird.Software();
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
            //mz compressor
            Compressor mzCompressor = new Compressor();
            mzCompressor.addMethod(Compressor.METHOD_PFOR);
            mzCompressor.addMethod(Compressor.METHOD_ZLIB);
            mzCompressor.target = Compressor.TARGET_MZ;
            mzCompressor.precision = (int) (Math.Ceiling(1 / jobInfo.jobParams.mzPrecision));
            coms.Add(mzCompressor);
            //intensity compressor
            Compressor intCompressor = new Compressor();
            if (jobInfo.jobParams.log2)
            {
                intCompressor.addMethod(Compressor.METHOD_LOG10);
                intCompressor.addMethod(Compressor.METHOD_ZLIB);
            }
            else
            {
                intCompressor.addMethod(Compressor.METHOD_ZLIB);
            }
            
            intCompressor.target = Compressor.TARGET_INTENSITY;
            coms.Add(intCompressor);
            airdInfo.compressors = coms;

            airdInfo.ignoreZeroIntensityPoint = jobInfo.jobParams.ignoreZeroIntensity;
            //Features Info
            featuresMap.Add(Features.raw_id, msd.id);
            featuresMap.Add(Features.ignore_zero_intensity, jobInfo.jobParams.ignoreZeroIntensity);
            featuresMap.Add(Features.source_file_format, jobInfo.format);
            featuresMap.Add(Features.byte_order, ByteOrder.LITTLE_ENDIAN);
            airdInfo.features = FeaturesUtil.toString(featuresMap);
            airdInfo.version = SoftwareInfo.VERSION;
            airdInfo.versionCode = SoftwareInfo.VERSION_CODE;
            return airdInfo;
        }

    }
}
