using AirdPro.Algorithms.Compressor;
using AirdPro.Constants;
using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.parser;
using AirdPro.Domains;
using AirdSDK.Beans;
using AirdSDK.Compressor;
using AirdSDK.Utils;
using Google.Protobuf;
using HZH_Controls;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using AirdPro.csimzMLParser.imzml;
using Activator = AirdPro.Constants.Activator;
using AirdPro.Algorithms.Maths;
using Software = AirdSDK.Beans.Software;
using ByteOrder = AirdPro.Constants.ByteOrder;
using static AirdPro.csimzMLParser.mzml.Component;
using AirdPro.Utils;
using CVUtil = AirdPro.Utils.imzml.CVUtil;
using DataUtil = AirdPro.Utils.imzml.DataUtil;
using MsiUtil = AirdPro.Utils.imzml.MsiUtil;
using System.Linq;

namespace AirdPro.Converters
{
    public class ImzMLConverter : Converter
    {
        public ImzML imzML;
        public SpectrumList spectrumList;
        public double minMZ = double.MaxValue;
        public double maxMZ = double.MinValue;

        protected List<BlockIndex> IndexList = []; //用于存储的全局的SWATH List

        public List<MsIndex> Ms1List = []; //用于存放MS1索引及基础信息,泛型为MsIndex
        protected Hashtable FeaturesMap = [];

        //用于离子淌度相关的字段
        public double[] MobiArray;
        public Dictionary<double, int> MobiDict;
        public MobiInfo MobiInfo = new();

        protected int MobiPrecision = 10000000; //mobility默认精确到小数点后7位
        protected int IntensityPrecision = 1; //Intensity默认精确到个位数

        protected int SpectraNumForIntensityPrecisionPredict = 5; //用于ComboComp预测Intensity精度时的采样光谱数
        public ImzMLComp Compressor;

        public bool CopyToLocal = false; //是否拷贝到本地

        public override void Init(JobInfo jobInfo)
        {
            JobInfo = jobInfo;
            Compressor = new ImzMLComp(this);
        }

        public override void InitCompressor()
        {
            //探索模式和非自动决策模式,会在此处初始化指定的压缩内核
            if (JobInfo.ionMobility)
            {
                Compressor.MobiIntComp = IntComp.build(JobInfo.config.mobiIntComp);
                Compressor.MobiByteComp = ByteComp.build(JobInfo.config.mobiByteComp);
            }

            Compressor.MzIntComp = SortedIntComp.build(JobInfo.config.mzIntComp);
            Compressor.MzByteComp = ByteComp.build(JobInfo.config.mzByteComp);

            Compressor.IntIntComp = IntComp.build(JobInfo.config.intIntComp);
            Compressor.IntByteComp = ByteComp.build(JobInfo.config.intByteComp);


            Compressor.RtIntComp4Chroma = SortedIntComp.build(JobInfo.config.rtIntComp);
            Compressor.RtByteComp4Chroma = ByteComp.build(JobInfo.config.rtByteComp);
        }

        public override void DoConvert()
        {
            Start();
            CopyFile(); //如果检测到是网络挂载磁盘,则首先拷贝到本地以后再进行转换,以提升转换速度

            ImzML imzML = ImportImzMLFile();
            try
            {
                if (imzML == null)
                {
                    JobInfo.LogError("imzML is null");
                    return;
                }
                StartPosition = 0;
                InitDirectory(); //创建文件夹,首先创建文件夹的目的在于确保对指定目录拥有写权限,如果无法正常创建,则在本步骤就中断

                ReadMSIData(imzML);                
                using (AirdStream = new FileStream(JobInfo.airdFilePath, FileMode.Create))
                {                    
                    InitCompressor();
                    ConverterWorkFlow.DDA(this);                   
                }
                ClearCache();
            }
            finally
            {
                Finish();
                if (CopyToLocal)
                {
                    AirdProFileUtil.ClearLocalTempFiles();
                }
            }
        }

        public void Finish()
        {
            Stopwatch.Stop();
            JobInfo.refreshReport = true;
            JobInfo.Log(Tag.Total_Time_Cost + Stopwatch.Elapsed.TotalSeconds, Status.Finished);
            Console.WriteLine(Stopwatch.Elapsed.TotalSeconds);
            JobInfo.SetConversionTime(Stopwatch.Elapsed.TotalMilliseconds);
            ClearCache();
            JobInfo.SetStatus(ProcessingStatus.FINISHED);
            if (imzML != null)
            {
                imzML = null;
            }
        }

        public void InitBrukerMobi()
        {
            JobInfo.Log(Tag.Init_Mobility_Array);
            long handle = TdfUtil.tims_open(JobInfo.inputPath, 1);
            double[] scanNums = new double[2000];
            for (int i = 0; i < scanNums.Length; i++)
            {
                scanNums[i] = i;
            }

            double[] mobility = new double[2000];
            TdfUtil.tims_scannum_to_oneoverk0(handle, 1, scanNums, mobility, scanNums.Length);
            TdfUtil.tims_close(handle);
            MobiDict = [];
            for (var i = 0; i < mobility.Length; i++)
            {
                MobiDict.Add(mobility[i], i);
            }

            MobiArray = mobility;
            Compressor.MobiDict = MobiDict;
        }   

        public void PredictForBestCombination()
        {
            if (!JobInfo.config.autoDecision)
            {
                return;
            }

            Stopwatch sw = new();
            sw.Start();
            JobInfo.Log(Tag.Predict_For_Best_Combination + JobInfo.airdFileName, Status.Predicting);
            Combination combination = RandomSampling(JobInfo.config.spectraToPredict, JobInfo.ionMobility);
            combination.enable(JobInfo.config, Compressor);
            JobInfo.Log(JobInfo.GetCompressorStr());
            JobInfo.config.autoDecision = false;
            sw.Stop();
            JobInfo.predictionTime = sw.Elapsed.TotalMilliseconds;
        }

        /**
         * num:采样数目,建议:5
         */
        public void PredictForIntensityPrecision()
        {
            Random rd = new();
            HashSet<int> nums = [];
            for (int i = 0; i < SpectraNumForIntensityPrecisionPredict; i++)
            {
                nums.Add(rd.Next(1, TotalSpectraCount));
            }

            bool findIt = false;
            for (var i = 0; i < nums.Count; i++)
            {
                Spectrum spectrum = spectrumList.GetSpectrum(i);
                foreach (double d in spectrum.GetIntensityArray())
                {
                    if ((d - (int)d) != 0) //如果随机采集到的intensity是精确到小数点后一位的,精度确定为10,即精确到小数点后一位
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

            IntensityPrecision = findIt ? 10 : 1;
            if (findIt)
            {
                IntensityPrecision = 10;
            }
            else
            {
                IntensityPrecision = 1;
            }

            Compressor.IntensityPrecision = IntensityPrecision;
            JobInfo.Log(Tag.Intensity_Precision + IntensityPrecision);
        }

        public void WriteToFile(Hashtable table, BlockIndex index)
        {
            ArrayList keys = new(table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                AddToIndex(index, table[key]);
            }
        }

        //注意:本函数会操作startPosition这个全局变量
        public void AddToIndex(BlockIndex index, object tempScan)
        {
            TempScan ts = (TempScan)tempScan;

            index.nums.Add(ts.num);
            index.rts.Add(ts.rt);
            index.tics.Add(ts.tic);
            index.basePeakIntensities.Add(ts.basePeakIntensity);
            index.injectionTimes.Add(ts.injectionTime);
            index.basePeakMzs.Add(ts.basePeakMz);

            index.polarities.Add(ts.polarity);
            index.energies.Add(ts.energy);
            index.activators.Add(ts.activator);
            index.filterStrings.Add(ts.filterString);
            index.msTypes.Add(ts.msType);

            if (ts.mzArrayBytes != null && ts.intArrayBytes != null)
            {
                index.mzs.Add(ts.mzArrayBytes.Length);
                index.ints.Add(ts.intArrayBytes.Length);
                StartPosition = StartPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                AirdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                AirdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
            }

            if (ts.mobilityArrayBytes != null)
            {
                index.mobilities.Add(ts.mobilityArrayBytes.Length);
                StartPosition += ts.mobilityArrayBytes.Length;
                AirdStream.Write(ts.mobilityArrayBytes, 0, ts.mobilityArrayBytes.Length);
            }
        }

        protected void CopyFile()
        {
            string driveLetter = Path.GetPathRoot(JobInfo.inputPath);
            DriveInfo driveInfo = new(driveLetter);
            if (driveInfo.DriveType != DriveType.Network)
            {
                return;
            }

            JobInfo.refreshReport = true;
            JobInfo.Log(Tag.Copy_File_To_Local, Status.Copying);
            CopyToLocal = true;
            string tempPath = AirdProFileUtil.GetAirdProTempPath();
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            string directoryPath = Path.GetDirectoryName(JobInfo.inputPath);
            string fileName = Path.GetFileNameWithoutExtension(JobInfo.inputPath);
            if (directoryPath == null)
            {
                return;
            }
            FileInfo imzML = new(Path.Combine(directoryPath, fileName + ".imzML"));
            if (imzML.Exists)
            {
                string path = Path.Combine(tempPath, imzML.Name);
                File.Copy(imzML.FullName, path, true);
            }
            FileInfo ibd = new(Path.Combine(directoryPath, fileName + ".ibd"));
            if (ibd.Exists)
            {
                string path = Path.Combine(tempPath, ibd.Name);
                File.Copy(ibd.FullName, path, true);
            }
        }

        public ImzML ImportImzMLFile()
        {
            JobInfo.Log(Tag.Prepare_To_Parse_ImzML_File, Status.Prepare);

            ImzML imzML;
            if (CopyToLocal)
            {
                imzML = ImzMLHandler.ParseimzML(Path.Combine(AirdProFileUtil.GetAirdProTempPath()));
            }
            else
            {
                imzML = ImzMLHandler.ParseimzML(JobInfo.inputPath);
            }

            if (imzML == null)
            {
                JobInfo.LogError(ResultCode.Reading_ImzML_File_Error_Run_Is_Null);
                return null;
            }
            JobInfo.Log(Tag.Adapting_ImzML_File_API, Status.Adapting);
            return imzML;
        }

        public void ReadMSIData(ImzML imzML)
        {
            this.imzML = imzML;
            //spectrumList
            spectrumList = imzML.GetRun().GetSpectrumList();
            if (spectrumList == null || spectrumList.IsEmpty())
            {
                JobInfo.Log(ResultCode.No_Spectra_Found);
            }
            else
            {
                TotalSpectraCount = spectrumList.Size();
            }           

            JobInfo.Log(Tag.Adapting_Finished);
            JobInfo.Log(Tag.Total_Spectra + TotalSpectraCount);
        }

        private bool IsMsSpectrum(Spectrum spectrum)
        {
            // one thats not MS (code for UV?)
            CVParam cvParams = spectrum.GetCVParam("MS:1000804");

            // By default, let's assume unidentified spectrumList are MS spectrumList
            return cvParams == null;
        }

        //将最终的数据写入文件中
        public void WriteToAirdInfoFile()
        {
            JobInfo.Log(Tag.Write_Index_File, Status.Writing_Index_File);
            AirdInfo airdInfo = BuildAirdInfo();

            if (JobInfo.config.compressedIndex)
            {
                List<BlockIndex> indexList = airdInfo.indexList;
                string indexListStr = JsonConvert.SerializeObject(indexList,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] indexListByte = new ZstdWrapper().encode(Encoding.Default.GetBytes(indexListStr));
                airdInfo.indexStartPtr = StartPosition;
                StartPosition += indexListByte.Length;
                airdInfo.indexEndPtr = StartPosition;
                airdInfo.indexList = null;
                AirdStream.Write(indexListByte, 0, indexListByte.Length);
            }

            string briefInfo = JobInfo.airdFileName + "," + airdInfo.type + "," + airdInfo.instruments[0].manufacturer + "," + airdInfo.fileSize + ",";
            Console.WriteLine(briefInfo);
            long totalSize = AirdStream.Length;
            if (JobInfo.config.indexFormat == 0 || JobInfo.config.indexFormat == 2)
            {
                string airdInfoStr = JsonConvert.SerializeObject(airdInfo,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
                using (AirdJsonStream = new FileStream(JobInfo.airdJsonFilePath, FileMode.Create))
                {
                    AirdJsonStream.Write(airdBytes, 0, airdBytes.Length);
                    totalSize += airdBytes.Length;
                }
            }

            if (JobInfo.config.indexFormat == 1 || JobInfo.config.indexFormat == 2)
            {
                AirdInfoProto proto = airdInfo.ToProto();
                byte[] protoBytes = proto.ToByteArray();
                using (AirdProtoStream = new FileStream(JobInfo.airdIndexFilePath, FileMode.Create))
                {
                    AirdProtoStream.Write(protoBytes, 0, protoBytes.Length);
                    totalSize += protoBytes.Length;
                }
            }

            JobInfo.SetAirdFileSize(totalSize);
        }

        public void ClearCache()
        {
            Ms1List = [];
            FeaturesMap = [];
            MobiDict = [];
            MobiInfo = new();
            if (imzML != null)
            {
                imzML = null;
            }
            if (spectrumList != null)
            {
                spectrumList = null;
            }
        }

        protected MsIndex ParseMs1(Spectrum spectrum, int index)
        {
            MsIndex ms1 = new()
            {
                level = 1,
                num = index
            };
            if (spectrum.GetScanList().Size() != 1)
            {
                return ms1;
            }

            Scan scan = spectrum.GetScanList().Get(0);
            ms1.filterString = CVUtil.ParseFilterString(scan, JobInfo);
            // ms1.rt = CVUtil.ParseRt(scan, JobInfo);
            ms1.tic = CVUtil.ParseTic(spectrum);
            ms1.basePeakIntensity = CVUtil.ParseBasePeakIntensity(spectrum);
            ms1.basePeakMz = CVUtil.ParseBasePeakMz(spectrum);
            // ms1.injectionTime = CVUtil.ParseInjectionTime(scan);
            if (MobiInfo.unit == null || MobiInfo.type == null)
            {
                CVUtil.ParseMobility(scan, MobiInfo);
            }
            ms1.msType = CVUtil.ParseMsType(spectrum);
            ms1.polarity = CVUtil.ParsePolarity(spectrum);
            ms1.activator = Activator.UNKNOWN;
            ms1.energy = -1;
            //min max mz
            double[] mzArray = spectrum.GetMzArray();
            if(mzArray !=null && mzArray.Length!=0)
            {
                ms1.minMz = mzArray.Min();
                ms1.maxMz = mzArray.Max();
            }
            return ms1;
        }

        public void CompressMobiDict()
        {
            int[] mobiIntArray = new int[MobiArray.Length];
            for (var i = 0; i < MobiArray.Length; i++)
            {
                mobiIntArray[i] = (int)Math.Round(MobiArray[i] * MobiPrecision);
            }

            byte[] compressedMobiData =
                new ZstdWrapper().encode(ByteTrans.intToByte(new IntegratedVarByteWrapper().encode(mobiIntArray)));
            MobiInfo.dictStart = StartPosition;
            StartPosition += compressedMobiData.Length;
            AirdStream.Write(compressedMobiData, 0, compressedMobiData.Length);
            MobiInfo.dictEnd = StartPosition;
        }

        public void CompressMs1Block()
        {
            if (JobInfo.config.noMS1)
            {
                JobInfo.Log(Tag.FILTER_NO_MS1);
                return;
            }

            BlockIndex blockIndex = new();
            BlockIndex index = blockIndex;
            index.level = 1;
            index.startPtr = StartPosition;
            Compressor.CompressMS1(this, index);
            index.endPtr = StartPosition;
            IndexList.Add(index);
        }

        protected AirdInfo BuildAirdInfo()
        {
            AirdInfo airdInfo = new();
            List<Software> softwares = [];
            List<ParentFile> parentFiles = [];

            //Basic Job Info
            airdInfo.engine = JobInfo.config.engine;
            airdInfo.airdPath = JobInfo.airdFilePath;
            airdInfo.fileSize = JobInfo.vendorFileSize;
            airdInfo.createDate = DateTime.Now.ToString();
            airdInfo.type = JobInfo.type;
            airdInfo.totalCount = imzML.GetRun().GetSpectrumList().Size();
            airdInfo.creator = JobInfo.config.creator;

            HashSet<string> activators = [];
            HashSet<float> energies = [];
            HashSet<string> polarities = [];
            HashSet<string> msTypes = [];
            HashSet<string> filterStrings = [];

            for (var i = 0; i < IndexList.Count; i++)
            {
                activators.UnionWith(IndexList[i].activators);
                energies.UnionWith(IndexList[i].energies);
                polarities.UnionWith(IndexList[i].polarities);
                msTypes.UnionWith(IndexList[i].msTypes);
                filterStrings.UnionWith(IndexList[i].filterStrings);
            }

            if (activators.Count == 1)
            {
                airdInfo.activator = IndexList[0].activators[0];
                foreach (var index in IndexList)
                {
                    index.activators = null;
                }
            }

            if (energies.Count == 1)
            {
                airdInfo.energy = IndexList[0].energies[0];
                foreach (var index in IndexList)
                {
                    index.energies = null;
                }
            }

            if (polarities.Count == 1)
            {
                airdInfo.polarity = IndexList[0].polarities[0];
                foreach (var index in IndexList)
                {
                    index.polarities = null;
                }
            }

            if (msTypes.Count == 1)
            {
                airdInfo.msType = IndexList[0].msTypes[0];
                foreach (var index in IndexList)
                {
                    index.msTypes = null;
                }
            }

            if (filterStrings.Count == 1)
            {
                airdInfo.filterString = IndexList[0].filterStrings[0];
                foreach (var index in IndexList)
                {
                    index.filterStrings = null;
                }
            }

            airdInfo.mobiInfo = MobiInfo;

            //Block index
            airdInfo.indexList = IndexList;

            //Instrument Info
            List<Instrument> instruments = [];
            foreach (InstrumentConfiguration ic in imzML.GetInstrumentConfigurationList())
            {
                Instrument instrument = new();

                if (!ic.GetCVParamOrChild(InstrumentConfiguration.INSTRUMENT_WATERS_INSTRUMENT_MODEL_ID).IsEmpty())
                {
                    instrument.manufacturer = Manufacturer.Waters;
                }

                //设备信息在不同的源文件格式中取法不同,有些是在instrumentConfigurationList中获取,有些是在paramGroups获取,因此出现了以下比较丑陋的写法
                if (ic.GetCVParamCount() != 0)
                {
                    foreach (CVParam cv in ic.GetCVParamList())
                    {
                        if (!FeaturesMap.ContainsKey(cv.ToString()))
                        {
                            FeaturesMap.Add(cv.ToString(), cv.GetValueAsString());
                        }
                    }

                    instrument.model = ic.GetCVParamList()[0].ToString();
                }
                else if (imzML.GetReferenceableParamGroupList().Size() != 0)
                {
                    foreach (ReferenceableParamGroup rpg in imzML.GetReferenceableParamGroupList())
                    {
                        if (!rpg.GetCVParamOrChild(InstrumentConfiguration.INSTRUMENT_AGILENT_INSTRUMENT_MODEL_ID).IsEmpty())
                        {
                            instrument.manufacturer = Manufacturer.Agilent;
                        }

                        if (!rpg.GetCVParamList().IsEmpty())
                        {
                            foreach (CVParam cv in rpg.GetCVParamList())
                            {
                                if (!FeaturesMap.ContainsKey(cv.ToString()))
                                {
                                    FeaturesMap.Add(cv.ToString(), cv.GetValueAsString());
                                }
                            }

                            instrument.model = rpg.GetCVParamList()[0].ToString();
                        }
                    }
                }

                if (!ic.componentList.IsEmpty())
                {
                    foreach (Component component in ic.componentList)
                    {
                        switch (component.Type)
                        {
                            case ComponentType.ComponentType_Analyzer:
                                foreach (CVParam cv in component.GetCVParamList())
                                {
                                    instrument.analyzer.Add(cv.ToString());
                                }

                                break;
                            case ComponentType.ComponentType_Source:
                                foreach (CVParam cv in component.GetCVParamList())
                                {
                                    instrument.source.Add(cv.ToString());
                                }

                                break;
                            case ComponentType.ComponentType_Detector:
                                foreach (CVParam cv in component.GetCVParamList())
                                {
                                    instrument.detector.Add(cv.ToString());
                                }

                                break;
                            case ComponentType.ComponentType_Unknown:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }

                instruments.Add(instrument);
            }

            airdInfo.instruments = instruments;
            airdInfo.startTimeStamp = imzML.GetRun().GetStartTimeStamp();
            //Software Info
            foreach (var soft in imzML.GetSoftwareList())
            {
                Software software = new()
                {
                    name = soft.GetID(),
                    version = soft.GetVersion()
                };
                softwares.Add(software);
            }

            Software airdPro = new()
            {
                name = SoftwareInfo.NAME,
                version = SoftwareInfo.VERSION,
                type = "DataFormatConversion"
            };
            softwares.Add(airdPro);
            airdInfo.softwares = softwares;

            //Parent Files Info
            if (imzML.GetFileDescription().GetSourceFileList() != null)
            {
                foreach (var sf in imzML.GetFileDescription().GetSourceFileList())
                {
                    ParentFile file = new()
                    {
                        name = sf.name,
                        location = sf.location,
                        formatType = sf.id
                    };
                    parentFiles.Add(file);
                }
                airdInfo.parentFiles = parentFiles;
            }                

            //Compressor Info
            List<Compressor> comps = [];
            Compressor mzCompressor = new(AirdSDK.Beans.Compressor.TARGET_MZ);
            Compressor intCompressor = new(AirdSDK.Beans.Compressor.TARGET_INTENSITY);
            Compressor mobiCompressor = new(AirdSDK.Beans.Compressor.TARGET_MOBILITY);
            Compressor rtCompressor = new(AirdSDK.Beans.Compressor.TARGET_RT);

            mzCompressor.addMethod(JobInfo.config.mzIntComp.ToString());
            mzCompressor.addMethod(JobInfo.config.mzByteComp.ToString());
            mzCompressor.precision = JobInfo.config.mzPrecision;

            intCompressor.addMethod(JobInfo.config.intIntComp.ToString());
            intCompressor.addMethod(JobInfo.config.intByteComp.ToString());
            intCompressor.precision = IntensityPrecision;

            mobiCompressor.addMethod(JobInfo.config.mobiIntComp.ToString());
            mobiCompressor.addMethod(JobInfo.config.mobiByteComp.ToString());
            mobiCompressor.precision = MobiPrecision;

            rtCompressor.addMethod(JobInfo.config.rtIntComp.ToString());
            rtCompressor.addMethod(JobInfo.config.rtByteComp.ToString());
            rtCompressor.precision = 100000;

            comps.Add(mzCompressor);
            comps.Add(intCompressor);
            comps.Add(mobiCompressor);
            comps.Add(rtCompressor);
            airdInfo.compressors = comps;

            airdInfo.ignoreZeroIntensityPoint = JobInfo.config.ignoreZeroIntensity;

            //Msi Info
            airdInfo.msiInfo = MsiUtil.GetMsiInfo(imzML, minMZ, maxMZ);     
                      
            //write position info to SpectraPositionFile
            /*string positionFile = Path.ChangeExtension(JobInfo.airdFilePath, "txt");
            int[] x = airdInfo.msiInfo.spectraPosition.x;
            int[] y = airdInfo.msiInfo.spectraPosition.y;
            int[] z = airdInfo.msiInfo.spectraPosition.z;
            using (StreamWriter writer = new StreamWriter(positionFile))
            {
                for (int i = 0; i < x.Length; i++)
                {
                    // 将x和y坐标写入文件，格式为"x,y"
                    writer.WriteLine($"{x[i]},{y[i]},{z[i]}");
                }
            }*/

            //Features Info
            FeaturesMap.Add(Features.raw_id, imzML);
            FeaturesMap.Add(Features.ignore_zero_intensity, JobInfo.config.ignoreZeroIntensity);
            FeaturesMap.Add(Features.source_file_format, JobInfo.format);
            FeaturesMap.Add(Features.byte_order, ByteOrder.LITTLE_ENDIAN);
            FeaturesMap.Add(Features.aird_algorithm, JobInfo.GetCompressorStr());
            airdInfo.features = FeaturesUtil.toString(FeaturesMap);
            airdInfo.version = SoftwareInfo.VERSION;
            return airdInfo;
        }

        readonly List<ByteComp> byteCompList = [new BrotliWrapper(), new SnappyWrapper(), new ZstdWrapper(), new ZlibWrapper()];

        readonly List<SortedIntComp> integratedIntCompList =
        [
            new DeltaWrapper(), new IntegratedBinPackingWrapper()
        ];

        readonly List<IntComp> intCompList = [new VarByteWrapper(), new BinPackingWrapper(), new Empty()];

        readonly List<IntComp> intIonCompList = [new VarByteWrapper(), new BinPackingWrapper(), new DeltaZigzagVBWrapper(), new Empty()];

        public Combination RandomSampling(int randomNum, bool ionMobi)
        {
            List<int[]> mzArrays = [];
            List<int[]> intensityArrays = [];
            List<int[]> mobiNoArrays = [];
            GaussianRandomGenerator generator = new (TotalSpectraCount / 2, 10000);
            int[] indexes = generator.GenerateRandomNumbers(randomNum, 1, TotalSpectraCount);

            for (var i = 0; i < randomNum; i++)
            {
                List<int[]> dataList = FetchSpectrum(indexes[i], ionMobi);
                if (dataList[0].Length > 0)
                {
                    mzArrays.Add(dataList[0]);
                    intensityArrays.Add(dataList[1]);
                    if (ionMobi)
                    {
                        mobiNoArrays.Add(dataList[2]);
                    }
                }
            }

            return CompressForTargetArrays(mzArrays, intensityArrays, mobiNoArrays, ionMobi);
        }

        public List<int[]> FetchSpectrum(int index, bool mobi)
        {
            List<int[]> arrays = [];
            Spectrum spectrum = spectrumList.GetSpectrum(index);
            double[] mzData = spectrum.GetMzArray();
            double[] intData = spectrum.GetIntensityArray();

            var size = mzData.Length;
            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int[] mobilityNoArray = new int[size];

            if (mobi)
            {
                double[] mobiData = DataUtil.GetMobilityData(spectrum);
                TimsData[] dataArray = new TimsData[size];
                for (int t = 0; t < size; t++)
                {
                    dataArray[t] = new TimsData(MobiDict[mobiData[t]], mzData[t], intData[t]);
                }

                Array.Sort(dataArray, (p1, p2) => p1.mz.CompareTo(p2.mz));
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(dataArray[i].mz * JobInfo.config.mzPrecision);
                    intensityArray[i] = DataUtil.FetchIntensity(dataArray[i].intensity, IntensityPrecision);
                    mobilityNoArray[i] = dataArray[i].mobilityNo;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(mzData[i] * JobInfo.config.mzPrecision);
                    intensityArray[i] = DataUtil.FetchIntensity(intData[i], IntensityPrecision);
                }
            }

            arrays.Add(mzArray);
            arrays.Add(intensityArray);
            arrays.Add(mobilityNoArray);
            spectrum = null;
            return arrays;
        }

        public Combination CompressForTargetArrays(List<int[]> mzArrays, List<int[]> intensityArrays,
            List<int[]> mobiNoArrays, bool ionMobi)
        {
            Dictionary<string, long> ctMap = [];
            Dictionary<string, long> dtMap = [];
            Dictionary<string, long> sizeMap = [];

            foreach (SortedIntComp intComp in integratedIntCompList)
            {
                foreach (ByteComp byteComp in byteCompList)
                {
                    string key = BuildComboKey(Tag.Key_MZ, intComp.getName(), byteComp.getName());
                    ctMap.Add(key, 0);
                    dtMap.Add(key, 0);
                    sizeMap.Add(key, 0);
                    StatUtil.Stat4ComboComp(intComp, byteComp, mzArrays, key, sizeMap, ctMap, dtMap);
                }
            }

            foreach (IntComp intComp in intCompList)
            {
                foreach (ByteComp byteComp in byteCompList)
                {
                    string key = BuildComboKey(Tag.Key_Intensity, intComp.getName(), byteComp.getName());
                    ctMap.Add(key, 0);
                    dtMap.Add(key, 0);
                    sizeMap.Add(key, 0);
                    StatUtil.Stat4ComboComp(intComp, byteComp, intensityArrays, key, sizeMap, ctMap, dtMap);
                }
            }

            if (ionMobi)
            {
                foreach (IntComp intComp in intIonCompList)
                {
                    foreach (ByteComp byteComp in byteCompList)
                    {
                        string key = BuildComboKey(Tag.Key_Mobi, intComp.getName(), byteComp.getName());
                        ctMap.Add(key, 0);
                        dtMap.Add(key, 0);
                        sizeMap.Add(key, 0);
                        StatUtil.Stat4ComboComp(intComp, byteComp, mobiNoArrays, key, sizeMap, ctMap, dtMap);
                    }
                }
            }

            List<CompressStat> mzStatList = [];
            List<CompressStat> intensityStatList = [];
            List<CompressStat> mobiStatList = [];
            foreach (KeyValuePair<string, long> pair in sizeMap)
            {
                string key = pair.Key;
                CompressStat stat = new(key, sizeMap[key], ctMap[key], dtMap[key]);
                if (key.StartsWith(Tag.Key_MZ))
                {
                    stat.key = stat.key.Replace(Tag.Key_MZ_Dash, Tag.Empty);
                    mzStatList.Add(stat);
                }

                if (pair.Key.StartsWith(Tag.Key_Intensity))
                {
                    stat.key = stat.key.Replace(Tag.Key_Intensity_Dash, Tag.Empty);
                    intensityStatList.Add(stat);
                }

                if (pair.Key.StartsWith(Tag.Key_Mobi))
                {
                    stat.key = stat.key.Replace(Tag.Key_Mobi_Dash, Tag.Empty);
                    mobiStatList.Add(stat);
                }
            }

            double csWeight = JobInfo.config.compressionSizeWeight;
            double ctWeight = JobInfo.config.compressionTimeWeight;
            double dtWeight = JobInfo.config.decompressionTimeWeight;
            int bestIndex4Mz = StatUtil.CalcBestIndex(mzStatList, csWeight, ctWeight, dtWeight);
            int bestIndex4Intensity = StatUtil.CalcBestIndex(intensityStatList, csWeight, ctWeight, dtWeight);
            Combination bestCombination = null;
            if (ionMobi)
            {
                int bestIndex4Mobi = StatUtil.CalcBestIndex(mobiStatList, csWeight, ctWeight, dtWeight);
                JobInfo.Log(Tag.Best_Combo_Comp + mzStatList[bestIndex4Mz].key + Const.Left_Slash +
                            intensityStatList[bestIndex4Intensity].key + Const.Left_Slash +
                            mobiStatList[bestIndex4Mobi].key);
                bestCombination = new Combination(mzStatList[bestIndex4Mz].key,
                    intensityStatList[bestIndex4Intensity].key,
                    mobiStatList[bestIndex4Mobi].key);
            }
            else
            {
                JobInfo.Log(Tag.Best_Combo_Comp + mzStatList[bestIndex4Mz].key + Const.Left_Slash +
                            intensityStatList[bestIndex4Intensity].key);
                bestCombination = new Combination(mzStatList[bestIndex4Mz].key,
                    intensityStatList[bestIndex4Intensity].key);
            }

            return bestCombination;
        }

        public string BuildComboKey(string key, string intCompName, string byteCompName)
        {
            return key + Const.Dash + intCompName + Const.Dash + byteCompName;
        }

        public void PretreatmentDda()
        {
            JobInfo.Log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            for (var i = 0; i < TotalSpectraCount; i++)
            {
                JobInfo.SetStatus("Pre:" + i + "/" + TotalSpectraCount);
                MsIndex msIndex = ParseMs1(spectrumList.Get(i), i);
                Ms1List.Add(msIndex); //加入MS1List
                //minMZ, maxMZ
                if (minMZ > msIndex.minMz)
                {
                    minMZ = msIndex.minMz;
                }
                if(maxMZ < msIndex.maxMz)
                {
                    maxMZ = msIndex.maxMz;
                }
            }            
            
            JobInfo.Log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.Log(Tag.Start_Processing_MS1_List);
        }

    }    
}