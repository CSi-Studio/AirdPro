using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Utils;
using AirdSDK.Beans;
using AirdSDK.Compressor;
using AirdSDK.Enums;
using AirdSDK.Utils;
using Google.Protobuf;
using Newtonsoft.Json;
using pwiz.CLI.msdata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using pwiz.CLI.cv;
using pwiz.CLI.data;

namespace AirdPro.Converters
{
    public class MSIConvert: PwizConverter
    {
        public MSIConvert() { }

        public long totalCount = 0;
        public string startTime = string.Empty;
        List<AirdSDK.Beans.Software> softwares = new List<AirdSDK.Beans.Software>();
        List<Instrument> instruments = new List<Instrument>();
        List<ParentFile> parentFiles = new List<ParentFile>();
        int MSIPixelsX = 0;
        int MSIPixelsY = 0;
        int MSIPixelsZ = 0;
        public void test()
        {
            InitCompressor();
        }

        public override void DoConvert()
        {
            Start();
            //CopyFile(); //如果检测到是网络挂载磁盘,则首先拷贝到本地以后再进行转换,以提升转换速度
            InitDirectory();
            try
            {
                using (AirdStream = new FileStream(JobInfo.airdFilePath, FileMode.Create))
                {
                    StartPosition = 0;
                    InitCompressor();
                    foreach (var inputPath in JobInfo.inputPaths.Split('|'))
                    {
                        JobInfo.inputPath = inputPath;
                        /*if (inputPath.ToLower().EndsWith(".imzml"))
                        {
                            
                        }*/
                        using (MSDataList msdList = ReadVendorFile())
                        {
                            if (msdList.Count == 0)
                            {
                                JobInfo.LogError("Msd List is Empty");
                                continue;
                            }
                          
                            foreach (var msd in msdList)
                            {
                                ReadMsd(msd);
                                switch (JobInfo.type)
                                {
                                    case AcquisitionMethod.DDA_MSI:
                                        ConverterWorkFlow.DDAMSI(this);
                                        break;
                                    case AcquisitionMethod.DIA_MSI:
                                        ConverterWorkFlow.DIAMSI(this);
                                        break;
                                } 
                                msd?.Dispose();
                            }
                        }
                        ClearCache();
                    }
                    WriteToAirdInfoFile();
                }
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

        //public void CompressMs1Block()
        //{
        //    if (JobInfo.config.noMS1)
        //    {
        //        JobInfo.Log(Tag.FILTER_NO_MS1);
        //        return;
        //    }
        //    BlockIndex index = new BlockIndex();
        //    index.level = 1;
        //    index.startPtr = StartPosition;
        //    Compressor.CompressMS1(this, index);
        //    index.endPtr = StartPosition;
        //    IndexList.Add(index);
        //}



        public void PretreatmentDda()
        {
            int parentNum = 0;
            int MS1Num = 0;
            if (JobInfo.MSIPixels != null) 
            {

            }
            JobInfo.Log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            for (var i = 0; i < TotalSpectraCount; i++)
            {
                using (Spectrum spectrum = SpectrumList.spectrum(i, false))
                {
                    string msLevel = CVUtil.ParseMsLevel(spectrum);
                    JobInfo.SetStatus("Pre:" + i + "/" + TotalSpectraCount);
                    //最后一个谱图,单独判断
                    if (i == TotalSpectraCount - 1)
                    {
                        if (msLevel.Equals(MsLevel.MS1))
                        {
                            Ms1List.Add(ParseMs1(spectrum, i)); //如果是MS1谱图,加入到MS1List
                            MS1Num++;
                        }

                        if (msLevel.Equals(MsLevel.MS2))
                        {
                            MsIndex ms2Index = ParseMs2(spectrum, i, parentNum);
                            AddToMs2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
                        }
                    }
                    else
                    {
                        //如果这个谱图是MS1
                        if (msLevel.Equals(MsLevel.MS1))
                        {
                            Ms1List.Add(ParseMs1(spectrum, i)); //加入MS1List
                            using (Spectrum next = SpectrumList.spectrum(i + 1))
                            {
                                if (CVUtil.ParseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                                {
                                    parentNum = i;
                                }
                            }
                        }

                        if (msLevel.Equals(MsLevel.MS2))
                        {
                            MsIndex ms2Index = ParseMs2(spectrum, i, parentNum);
                            AddToMs2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
                        }
                    }
                }
            }

            JobInfo.Log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.Log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.Log(Tag.Start_Processing_MS1_List);
        }

        public void PretreatmentDia()
        {
            int parentNum = 0;
            JobInfo.Log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < TotalSpectraCount; i++)
            {
                progress++;
                JobInfo.Log(null, Tag.progress(Tag.Pre, progress, TotalSpectraCount));
                using (Spectrum spectrum = SpectrumList.spectrum(i))
                {
                    string msLevel = CVUtil.ParseMsLevel(spectrum);
                    //如果这个谱图是MS1                          
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        parentNum = i;
                        Ms1List.Add(ParseMs1(spectrum, i));
                    }

                    //如果这个谱图是MS2
                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = ParseMs2(spectrum, i, parentNum);
                        //边扫描边建立SWATH WindowRange
                        if (!RangeTable.Contains(ms2Index.precursor.mz))
                        {
                            WindowRange range = ms2Index.precursor;
                            Ranges.Add(range);
                            RangeTable.Add(range.mz, range);
                        }

                        //DIA的MS2Map以precursorMz为key
                        AddToMs2Map(ms2Index.precursor.mz, ms2Index);
                    }
                }
            }

            JobInfo.Log(Tag.Total_SWATH_WINDOWS + Ranges.Count);
            JobInfo.Log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.Log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.Log(Tag.Start_Processing_MS1_List);
        }

        public void ClearCache()
        {
            Ranges = new();
            RangeTable = new();
            Ms2Table = new();
            Ms1List = new();
            FeaturesMap = new();
            MobiDict = new();
            MobiInfo = new();
            ChromatogramIndex = new();

            //清空所有非托管内存
            if (SpectrumList != null)
            {
                SpectrumList.Dispose();
                SpectrumList = null;
            }

            if (ChromatogramList != null)
            {
                ChromatogramList.Dispose();
                ChromatogramList = null;
            }

            if (Msd != null)
            {
                Msd.Dispose();
                Msd = null;
            }
        }

        public void WriteToAirdInfoFile()
        {
            JobInfo.Log(Tag.Write_Index_File, Status.Writing_Index_File);
            AirdInfo airdInfo = buildAirdInfo();

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

            //列式存储引擎需要额外存储一个cjson的文件
            if (JobInfo.config.ColumnCompression())
            {
                ColumnInfo columnInfo = BuildColumnInfo();

                if (JobInfo.config.indexFormat == 0 || JobInfo.config.indexFormat == 2)
                {
                    string columnInfoStr = JsonConvert.SerializeObject(columnInfo,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                    byte[] columnInfoBytes = Encoding.Default.GetBytes(columnInfoStr);
                    using (AirdColumnJsonStream = new FileStream(JobInfo.airdColumnJsonFilePath, FileMode.Create))
                    {
                        AirdColumnJsonStream.Write(columnInfoBytes, 0, columnInfoBytes.Length);
                        totalSize += columnInfoBytes.Length;
                    }
                }

                if (JobInfo.config.indexFormat == 1 || JobInfo.config.indexFormat == 2)
                {
                    ColumnInfoProto proto = columnInfo.ToProto();
                    byte[] protoBytes = proto.ToByteArray();
                    using (AirdColumnProtoStream = new FileStream(JobInfo.airdColumnProtoFilePath, FileMode.Create))
                    {
                        AirdColumnProtoStream.Write(protoBytes, 0, protoBytes.Length);
                        totalSize += protoBytes.Length;
                    }
                }
            }

            JobInfo.SetAirdFileSize(totalSize);
        }

        public AirdInfo buildAirdInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            
            //Basic Job Info
            airdInfo.engine = JobInfo.config.engine;
            airdInfo.airdPath = JobInfo.airdFilePath;
            airdInfo.fileSize = JobInfo.vendorFileSize;
            airdInfo.createDate = DateTime.Now.ToString();
            airdInfo.type = JobInfo.type;
            airdInfo.totalCount = totalCount;
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
            //Scan index and window range info
            airdInfo.rangeList = Ranges;

            //Block index
            airdInfo.indexList = IndexList;

            //ChromatogramIndex
            airdInfo.chromatogramIndex = ChromatogramIndex;

            airdInfo.instruments = instruments;
            airdInfo.startTimeStamp = startTime;

            AirdSDK.Beans.Software airdPro = new AirdSDK.Beans.Software
            {
                name = SoftwareInfo.NAME,
                version = SoftwareInfo.VERSION,
                type = "DataFormatConversion"
            };
            softwares.Add(airdPro);
            airdInfo.softwares = softwares;
            airdInfo.parentFiles = parentFiles;

            //Compressor Info
            List<Compressor> comps = [];
            Compressor mzCompressor = new Compressor(AirdSDK.Beans.Compressor.TARGET_MZ);
            Compressor intCompressor = new Compressor(AirdSDK.Beans.Compressor.TARGET_INTENSITY);
            Compressor mobiCompressor = new Compressor(AirdSDK.Beans.Compressor.TARGET_MOBILITY);
            Compressor rtCompressor = new Compressor(AirdSDK.Beans.Compressor.TARGET_RT);

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
            //Features Info
            //FeaturesMap.Add(Features.raw_id, Msd.id);
            FeaturesMap.Add(Features.ignore_zero_intensity, JobInfo.config.ignoreZeroIntensity);
            FeaturesMap.Add(Features.source_file_format, JobInfo.format);
            FeaturesMap.Add(Features.byte_order, ByteOrder.LITTLE_ENDIAN);
            FeaturesMap.Add(Features.aird_algorithm, JobInfo.GetCompressorStr());
            airdInfo.features = FeaturesUtil.toString(FeaturesMap);
            airdInfo.version = SoftwareInfo.VERSION;
            return airdInfo;
        }

        private void AddInstrumentInfo()
        {
            //Instrument Info
            foreach (InstrumentConfiguration ic in Msd.instrumentConfigurationList)
            {
                Instrument instrument = new Instrument();

                switch (JobInfo.format)
                {
                    //仪器设备信息
                    case FileFormat.WIFF:
                    case FileFormat.WIFF2:
                        instrument.manufacturer = Manufacturer.SCIEX;
                        break;
                    case FileFormat.RAW:
                        if (!JobInfo.isDir)
                        {
                            instrument.manufacturer = Manufacturer.Thermo;
                        }
                        break;
                    case FileFormat.D:
                        instrument.manufacturer = Manufacturer.Bruker;
                        break;
                }

                if (!ic.cvParamChild(CVID.MS_Waters_instrument_model).cvid.Equals(CVID.CVID_Unknown))
                {
                    instrument.manufacturer = Manufacturer.Waters;
                }

                //设备信息在不同的源文件格式中取法不同,有些是在instrumentConfigurationList中获取,有些是在paramGroups获取,因此出现了以下比较丑陋的写法
                if (ic.cvParams.Count != 0)
                {
                    foreach (CVParam cv in ic.cvParams)
                    {
                        if (!FeaturesMap.ContainsKey(cv.name))
                        {
                            FeaturesMap.Add(cv.name, cv.value);
                        }
                    }

                    instrument.model = ic.cvParams[0].name;
                }
                else if (Msd.paramGroups.Count != 0)
                {
                    foreach (ParamGroup pg in Msd.paramGroups)
                    {
                        if (!pg.cvParamChild(CVID.MS_Agilent_instrument_model).cvid.Equals(CVID.CVID_Unknown))
                        {
                            instrument.manufacturer = Manufacturer.Agilent;
                        }

                        if (pg.cvParams.Count != 0)
                        {
                            foreach (CVParam cv in pg.cvParams)
                            {
                                if (!FeaturesMap.ContainsKey(cv.name))
                                {
                                    FeaturesMap.Add(cv.name, cv.value.ToString());
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
                        case ComponentType.ComponentType_Unknown:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                instruments.Add(instrument);
                ic.Dispose();
            }
        }

        public void StoreAirdInfo() 
        {
            if (string.IsNullOrEmpty(startTime))
            {
                startTime = Msd.run.startTimeStamp;
            }
            totalCount += Msd.run.spectrumList.size();
            AddInstrumentInfo();
            //Software Info
            foreach (var soft in Msd.softwareList)
            {
                AirdSDK.Beans.Software software = new AirdSDK.Beans.Software();
                software.name = soft.id;
                software.version = soft.version;
                softwares.Add(software);
                soft.Dispose();
            }
            //Parent Files Info
            foreach (var sf in Msd.fileDescription.sourceFiles)
            {
                ParentFile file = new ParentFile
                {
                    name = sf.name,
                    location = sf.location,
                    formatType = sf.id
                };
                parentFiles.Add(file);
            }
        }
    }
}
