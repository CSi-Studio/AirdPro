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
        protected List<ScanIndex> scanIndexList = new List<ScanIndex>();//存储每一张谱图的索引及基础信息
        protected List<ScanIndex> swathIndexList = new List<ScanIndex>();//存储每一个SWATH块的索引及基础信息

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
            jobInfo.log("Finished! Total Cost: " + stopwatch.Elapsed.TotalSeconds+" seconds", "Finished");
            stopwatch.Stop();
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
                
                if (jobInfo.ignoreZeroIntensity && intData[t] == 0)
                {
                    continue;
                }
                mzList.Add((int)(mzData[t] * jobInfo.mzPrecision)); //精确到小数点后面三位
                intensityList.Add((float)(Math.Round(intData[t] * jobInfo.intensityPrecision) / jobInfo.intensityPrecision));
            }

            float[] intensityArray = intensityList.ToArray();
            int[] mzArray = CompressUtil.compressWithPFor(mzList.ToArray());
            mzBytes = CompressUtil.compressWithZlib(mzArray);
            intensityBytes = CompressUtil.compressWithZlib(intensityArray);
        }

        protected void readyForReadVendorFile()
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
        protected AirdInfo buildBasicInfo()
        {
            AirdInfo airdInfo = new AirdInfo();

            List<Instrument> instruments = new List<Instrument>();
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
            airdInfo.scanIndexList = scanIndexList;
            airdInfo.swathIndexList = swathIndexList;
            airdInfo.rangeList = ranges;

            //Instrument Info
            foreach (InstrumentConfiguration ic in msd.instrumentConfigurationList)
            {
                Instrument instrument = new Instrument();
                //仪器设备信息
                instrument.manufacturer = "SCIEX";
                instrument.model = msd.run.defaultInstrumentConfiguration.cvParams[0].name;
                instrument.ionisation = "electrospray ionization";
                instrument.massAnalyer = "quadrupole";
                instrument.detector = "electron multiplier";
                instruments.Add(instrument);
            }
            airdInfo.instruments = instruments;

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
//                file.type = sf
            }
            airdInfo.parentFiles = parentFiles;

            //Compressor Info
            List<Compressor> coms = new List<Compressor>();
            //mz compressor
            Compressor mzCompressor = new Compressor();
            mzCompressor.method = "pFor,zlib";
            mzCompressor.precision = jobInfo.mzPrecision;
            mzCompressor.target = "mz";
            coms.Add(mzCompressor);
            //intensity compressor
            Compressor intCompressor = new Compressor();
            intCompressor.method = "zlib";
            intCompressor.precision = jobInfo.intensityPrecision;
            intCompressor.target = "intensity";
            coms.Add(intCompressor);
            airdInfo.compressors = coms;

            //Features Info
            Hashtable features = new Hashtable();
            features.Add(Features.aird_info_raw_id, msd.id);
            features.Add(Features.aird_info_ignore_zero_intensity, jobInfo.ignoreZeroIntensity);

            return airdInfo;
        }

        //将最终的数据写入文件中
        public void writeToAirdInfoFile()
        {
            jobInfo.progress.Report("Write-in");
            AirdInfo airdInfo = buildBasicInfo();

            JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string airdInfoStr = JsonConvert.SerializeObject(airdInfo, jsonSetting);
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            airdJsonStream.Write(airdBytes, 0, airdBytes.Length);

        }
    }
}
