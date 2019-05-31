using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Propro.Constants;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using pwiz.CLI.util;
using Propro.Domains;
using Propro.Structs;
using Propro.Utils;
using Propro_Client.Domains.Aird;
using Propro_Client.Utils;
using pwiz.CLI.analysis;
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

        //Wrapping数据
        protected void wrapping()
        {
            //Wrapping 
            jobInfo.log("Start Wrapping", "Wrapping");
            SpectrumListFactory.wrap(msd, new List<string>());
            if (msd.run.spectrumList == null || msd.run.spectrumList.empty()) jobInfo.logError("No Spectrums Found");
            spectrumList = msd.run.spectrumList;
        }

        protected float parseRT(Scan scan)
        {
            CVParam cv = scan.cvParamChild(CVID.MS_scan_start_time);
            float time = float.Parse(cv.value.ToString());
            if (cv.unitsName.Equals("minute"))
            {
                return time * 60;
            }
            else
            {
                return time;
            }
        }

        protected void compress(Spectrum spectrum, out byte[] mzBytes, out byte[] intensityBytes)
        {
            BinaryData mzData = spectrum.getMZArray().data;
            BinaryData intData = spectrum.getIntensityArray().data;

            List<int> mzList = new List<int>();
            List<float> intensityList = new List<float>();
            
            for (int t = 0; t < mzData.Count; t++)
            {
                if (jobInfo.ignoreZeroIntensity && intData[t] == 0) continue;
                mzList.Add(Convert.ToInt32(mzData[t] * 1000)); //精确到小数点后面三位
                if (jobInfo.log10)
                {
                    intensityList.Add((float)Math.Round(Math.Log10(intData[t]), 3)); //取log10并且精确到小数点后3位
                }
                else
                {
                    intensityList.Add((float)Math.Round(intData[t], 1)); //精确到小数点后一位
                }
            }

            float[] intensityArray = intensityList.ToArray();
            int[] mzArray = CompressUtil.compressWithPFor(mzList.ToArray());
            mzBytes = CompressUtil.compressWithZlib(mzArray);
            intensityBytes = CompressUtil.compressWithZlib(intensityArray);
        }

        protected void readVendorFile()
        {
            ReaderList readers = ReaderList.FullReaderList;
            MSDataList msdList = new MSDataList();
            ReaderConfig rc = new ReaderConfig();
            rc.ignoreZeroIntensityPoints = true;
            readers.read(jobInfo.inputFilePath, msdList, rc);
            //用于Swath Aird平台的实验源文件中,msdList.Count必然为1,否则实验文件格式不符合要求
            if (msdList.Count != 1) jobInfo.logError("File Format Error.MSDataList size must be 1");

            msd = msdList[0];
        }

        //将最终的数据写入文件中
        public void writeToAirdInfoFile()
        {
            jobInfo.log("Write-in", "Write-in");
            AirdInfo airdInfo = buildBasicInfo();

            JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string airdInfoStr = JsonConvert.SerializeObject(airdInfo, jsonSetting);
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            airdJsonStream.Write(airdBytes, 0, airdBytes.Length);

        }

        protected AirdInfo buildBasicInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            Hashtable features = new Hashtable();
            List<Domains.Software> softwares = new List<Domains.Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();

            //Basic Job Info
            airdInfo.creator = jobInfo.creator;
            airdInfo.createDate = new DateTime();
            switch (jobInfo.type)
            {
                case "0":
                    airdInfo.type = ExperimentType.DIA_SWATH;
                    break;
                case "1":
                    airdInfo.type = ExperimentType.PRM;
                    break;
                default:
                    airdInfo.type = ExperimentType.DIA_SWATH;
                    break;
            }

            //Scan index and window range info
            airdInfo.rangeList = ranges;
            airdInfo.indexList = indexList;

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
                    features.Add(cv.name, cv.value);
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
                            features.Add(cv.name, cv.value);
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
            mzCompressor.method = "pFor,zlib";
            mzCompressor.target = "mz";
            coms.Add(mzCompressor);
            //intensity compressor
            Compressor intCompressor = new Compressor();
            if (jobInfo.log10)
            {
                intCompressor.method = "log10,zlib";
            }
            else
            {
                intCompressor.method = "zlib";
            }
            
            intCompressor.target = "intensity";
            coms.Add(intCompressor);
            airdInfo.compressors = coms;

            //Features Info
            
            features.Add(Features.aird_info_raw_id, msd.id);
            features.Add(Features.aird_info_ignore_zero_intensity, jobInfo.ignoreZeroIntensity);
            airdInfo.features = FeaturesUtil.toString(features);

            return airdInfo;
        }

    }
}
