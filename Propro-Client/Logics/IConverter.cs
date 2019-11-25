using Newtonsoft.Json;
using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using Propro.Utils;
using Propro_Client.Constants;
using Propro_Client.Domains.Aird;
using Propro_Client.Utils;
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
using MSFileReaderLib;
using Software = pwiz.CLI.msdata.Software;

namespace Propro.Logics
{
    public abstract class IConverter
    {
        private static readonly object calculateSHA1Mutex = new object();
        protected MSData msd;
        protected SpectrumList spectrumList;
        protected ConvertJobInfo jobInfo;
        protected Stopwatch stopwatch = new Stopwatch();
        protected FileStream airdStream;
        protected FileStream airdJsonStream;
        protected List<WindowRange> ranges = new List<WindowRange>();//SWATH Window的窗口
        protected List<SwathIndex> indexList = new List<SwathIndex>();//用于存储的全局的SWATH List
        protected List<BlockIndex> blockIndexList = new List<BlockIndex>();//适用于DDA的块索引
        protected Hashtable featuresMap = new Hashtable();
        protected long fileSize;
        protected long startPosition;//文件指针

        public IConverter(ConvertJobInfo jobInfo)
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
            BinaryData mzData = spectrum.getMZArray().data;
            BinaryData intData = spectrum.getIntensityArray().data;

            List<int> mzList = new List<int>();
            List<float> intensityList = new List<float>();
            for (int t = 0; t < mzData.Count; t++)
            {
                if (jobInfo.ignoreZeroIntensity && intData[t] == 0) continue;
                
                int mz = Convert.ToInt32(mzData[t] * 1000);
                mzList.Add(mz); //精确到小数点后面三位

                if (jobInfo.log10)
                {
                    intensityList.Add(Convert.ToSingle(Math.Round(Math.Log10(intData[t]), 3))); //取log10并且精确到小数点后3位
                }
                else
                {
                    intensityList.Add(Convert.ToSingle(Math.Round(intData[t], 1))); //精确到小数点后一位
                }
            }

            float[] intensityArray = intensityList.ToArray();
            int[] mzArray = CompressUtil.compressWithPFor(mzList.ToArray());
            ts.mzArrayBytes = CompressUtil.compressWithZlib(mzArray);
            ts.intArrayBytes = CompressUtil.compressWithZlib(intensityArray);
        }

        protected void outputWithOrder(Hashtable table, SwathIndex swathIndex)
        {
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                TempScan tempScan = (TempScan)table[key];
                addToIndex(swathIndex, tempScan);
            }
        }

        //注意:本函数会操作startPosition这个全局变量
        protected void addToIndex(SwathIndex index, TempScan ts)
        {
            index.nums.Add(ts.num);
            index.rts.Add(ts.rt);
            index.mzs.Add(ts.mzArrayBytes.Length);
            index.ints.Add(ts.intArrayBytes.Length);
            startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
            airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
            airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
        }

        protected float getPrecursorIsolationWindowParams(Spectrum spectrum, CVID cvid)
        {
            float result = -1;
            int retryTimes = 3;
            while (result == -1 && retryTimes > 0)
            {
                try
                {
                    result = (float) double.Parse(spectrum.precursors[0].isolationWindow.cvParamChild(cvid).value.ToString());
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
            
            spectrumList = msd.run.spectrumList;
            if (spectrumList == null || spectrumList.empty())
            {
                jobInfo.logError("No Spectrums Found");
            }
            else
            {
                jobInfo.log("Adapting Finished");
            }
            //            ReaderList readers = ReaderList.FullReaderList;
            //            MSDataList msdList = new MSDataList();
            //            ReaderConfig rc = new ReaderConfig();
            //            rc.ignoreZeroIntensityPoints = true;
            //            readers.read(jobInfo.inputFilePath, msdList, rc);

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
            indexList = new List<SwathIndex>();
        }

        protected AirdInfo buildBasicInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            List<Domains.Software> softwares = new List<Domains.Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();

            //Basic Job Info
            airdInfo.airdPath = jobInfo.airdFilePath;
            airdInfo.fileSize = fileSize;
            airdInfo.creator = jobInfo.creator;
            airdInfo.createDate = new DateTime();
            airdInfo.type = jobInfo.type;
            //Scan index and window range info
            airdInfo.rangeList = ranges;
            airdInfo.indexList = indexList;
            airdInfo.blockIndexList = blockIndexList;

            //Instrument Info
            InstrumentConfiguration ic = msd.instrumentConfigurationList[0];
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
                            featuresMap.Add(cv.name, cv.value);
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
            
            airdInfo.instrument = instrument;

            //Software Info
            foreach (Software sof in msd.softwareList)
            {
                Domains.Software software = new Domains.Software();
                software.name = sof.id;
                software.version = sof.version;
                softwares.Add(software);
            }
            airdInfo.softwares = softwares;

            //Parent Files Info
            foreach (SourceFile sf in msd.fileDescription.sourceFiles)
            {
                ParentFile file = new ParentFile();
                file.name = sf.name;
                file.location = sf.location;
                file.type = sf.id;
            }
            airdInfo.parentFiles = parentFiles;

            //Compressor Info
            List<Compressor> coms = new List<Compressor>();
            //mz compressor
            Compressor mzCompressor = new Compressor();
            mzCompressor.method = Compressor.METHOD_PFOR + "," + Compressor.METHOD_ZLIB;
            mzCompressor.target = Compressor.TARGET_MZ;
            coms.Add(mzCompressor);
            //intensity compressor
            Compressor intCompressor = new Compressor();
            if (jobInfo.log10)
            {
                intCompressor.method = Compressor.METHOD_LOG10 + "," + Compressor.METHOD_ZLIB;
            }
            else
            {
                intCompressor.method = Compressor.METHOD_ZLIB;
            }
            
            intCompressor.target = Compressor.TARGET_INTENSITY;
            coms.Add(intCompressor);
            airdInfo.compressors = coms;

            //Features Info
            featuresMap.Add(Features.raw_id, msd.id);
            featuresMap.Add(Features.ignore_zero_intensity, jobInfo.ignoreZeroIntensity);
            featuresMap.Add(Features.source_file_format, jobInfo.format);
            featuresMap.Add(Features.aird_version, SoftwareVersion.AIRD_VERSION);
            featuresMap.Add(Features.propro_client_version, SoftwareVersion.CLIENT_VERSION);
            featuresMap.Add(Features.byte_order, ByteOrder.LITTLE_ENDIAN);
            airdInfo.features = FeaturesUtil.toString(featuresMap);

            return airdInfo;
        }

    }
}
