using AirdPro.Algorithms.Compressor;
using AirdPro.Algorithms.Parser;
using AirdPro.Constants;
using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.parser;
using AirdPro.Domains;
using AirdSDK.Beans;
using AirdSDK.Beans.Common;
using AirdSDK.Compressor;
using AirdSDK.Enums;
using AirdSDK.Utils;
using Google.Protobuf;
using HZH_Controls;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Spectrum = AirdPro.csimzMLParser.mzml.Spectrum;
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

namespace AirdPro.Converters
{
    public class ImzMLConverter : Converter
    {
        public ImzML imzML;
        public SpectrumList spectrumList;
        protected ChromatogramList chromatogramList;

        protected List<WindowRange> Ranges = []; //SWATH/DIA Window的窗口
        protected Hashtable RangeTable = []; //用于存放SWATH/DIA窗口的信息,key为mz
        protected List<BlockIndex> IndexList = []; //用于存储的全局的SWATH List
        protected List<ColumnIndex> ColumnIndexList = []; //列存储索引，仅在面向Search的场景下有效

        protected Hashtable
            Ms2Table = Hashtable.Synchronized([]); //用于存放MS2的索引信息,DDA采集模式下key为ms1的num, DIA采集模式下key为mz

        public List<MsIndex> Ms1List = []; //用于存放MS1索引及基础信息,泛型为MsIndex
        protected Hashtable FeaturesMap = [];

        //用于离子淌度相关的字段
        public double[] MobiArray;
        public Dictionary<double, int> MobiDict;
        public MobiInfo MobiInfo = new();

        // protected int MzPrecision
        protected int MobiPrecision = 10000000; //mobility默认精确到小数点后7位
        protected int IntensityPrecision = 1; //Intensity默认精确到个位数

        protected int SpectraNumForIntensityPrecisionPredict = 5; //用于ComboComp预测Intensity精度时的采样光谱数
        public ImzMLComp Compressor;
        public ChromatogramIndex ChromatogramIndex;

        public Dictionary<string, AcqCompound>
            MrmCompoundDict = []; //用于MRM采集模式下,预存储化合物名称与离子对的词典,当前仅适用于Agilent的.d文件夹类型的质谱文件

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
                    PredictAcquisitionMethod();
                    InitCompressor();
                    switch (JobInfo.type)
                    {
                        case AcquisitionMethod.DIA:
                            ConverterWorkFlow.DIA(this);
                            break;
                        case AcquisitionMethod.DDA:
                            ConverterWorkFlow.DDA(this);
                            break;
                        case AcquisitionMethod.PRM:
                            ConverterWorkFlow.PRM(this);
                            break;
                        case AcquisitionMethod.MRM:
                            ConverterWorkFlow.MRM(this);
                            break;
                        case AcquisitionMethod.DDA_PASEF:
                            JobInfo.ionMobility = true;
                            ConverterWorkFlow.DDAPasef(this);
                            break;
                        case AcquisitionMethod.DIA_PASEF:
                            JobInfo.ionMobility = true;
                            ConverterWorkFlow.DIAPasef(this);
                            break;
                    }
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

        /**
         * 用于检测当前文件的采集模式
         * 如果含有Mobility属性,则为PASEF模式
         * 如果前三帧都是MS1,则必然不是DIA
         * 如果MS2的precursor范围大于3,则可能是DIA
         * 如果MS2的precursor范围小于1,则可能是DDA
         */
        public void PredictAcquisitionMethod()
        {
            if (!JobInfo.type.Equals(JobInfo.AutoType))
            {
                JobInfo.SetType(JobInfo.type);
                if (JobInfo.type.Equals(AcquisitionMethod.DIA_PASEF) || JobInfo.type.Equals(AcquisitionMethod.DDA_PASEF))
                {
                    JobInfo.ionMobility = true;
                }
                return;
            }

            bool mobi = false;
            JobInfo.Log(Tag.Predict_Acquisition_Method, Status.Init);

            //如果有光谱图
            if (spectrumList != null && spectrumList.Size() > 0)
            {
                Spectrum firstSpec = spectrumList.GetSpectrum(0);
                List<Spectrum> predictSpecList = [];
                //首先取10个窗口，当不足10个时，取spectra.Size()
                int fetchCount = 10;
                if (spectrumList.Size() < 10)
                {
                    fetchCount = spectrumList.Size();
                }
                for (int i = 0; i < fetchCount; i++)
                {
                    predictSpecList.Add(spectrumList.GetSpectrum(i));
                }

                //首先判断是不是带有离子淌度的ion mobility模式
                if (firstSpec.GetBinaryDataArrayList().Size() == 3)
                {
                    foreach (BinaryDataArray dataArray in firstSpec.GetBinaryDataArrayList())
                    {
                        if (dataArray.IsMobilityArray())
                        {
                            JobInfo.ionMobility = true;
                            mobi = true;
                            break;
                        }
                    }
                }

                bool isDDA = true;
                bool isDIA = false;

                foreach (Spectrum spectrum in predictSpecList)
                {
                    //如果全部扫描下来都没有MS2, 说明是Full Scan扫描模式,设置为DDA                    
                    if (CVUtil.ParseMsLevel(spectrum).Equals(MsLevel.MS2))
                    {
                        double width = CVUtil.ParsePrecursorWidth(spectrum.GetPrecursorList().Get(0).IsolationWindow, JobInfo);
                        //然后判断前体的宽度范围,如果范围小于4,则被预测为DDA模式,否则会被认定为DIA模式
                        if (width < 4)
                        {
                            isDDA = true;
                            isDIA = false;
                            //自动模式会将DDA模式与PRM模式混合
                            break;
                        }
                        else
                        {
                            isDDA = false;
                            isDIA = true;
                            break;
                        }
                    }
                }

                if (isDDA && mobi)
                {
                    JobInfo.SetType(AcquisitionMethod.DDA_PASEF);
                }

                if (isDDA && !mobi)
                {
                    JobInfo.SetType(AcquisitionMethod.DDA);
                }

                if (isDIA && mobi)
                {
                    JobInfo.SetType(AcquisitionMethod.DIA_PASEF);
                }

                if (isDIA && !mobi)
                {
                    JobInfo.SetType(AcquisitionMethod.DIA);
                }
            }

            try
            {
                //如果有色谱图,且谱图数目大于2(排除TIC和BPC图),则预测为SRM模式
                if (chromatogramList != null && chromatogramList.Size() > 10)
                {
                    List<Chromatogram> predictChromatoList = [];
                    // 首先取10个窗口
                    for (int i = 0; i < 10; i++)
                    {
                        Chromatogram chroma = chromatogramList.GetChromatogram(i);
                        predictChromatoList.Add(chroma);
                    }

                    JobInfo.SetType(AcquisitionMethod.MRM);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Chromatogram Reading Failed:" + e.Message);
            }
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

        /**
         * 存储列存储数据
         * 注意，本函数会操作startPosition这个全局变量
         */
        public void WriteColumnData(ConcurrentDictionary<int, ByteColumn> compressedColumns, ColumnIndex columnIndex)
        {
            byte[] compressedMzs = ByteTrans.intToByte(
                        new IntegratedVarByteWrapper().encode(columnIndex.mzs));
            byte[] compressedRts = ByteTrans.intToByte(
                        new IntegratedVarByteWrapper().encode(columnIndex.rts));
            //写入矩阵的横坐标实际值
            columnIndex.startMzListPtr = StartPosition;
            StartPosition += compressedMzs.Length;
            columnIndex.endMzListPtr = StartPosition;
            AirdStream.Write(compressedMzs, 0, compressedMzs.Length);

            //写入矩阵的纵坐标实际值
            columnIndex.startRtListPtr = StartPosition;
            StartPosition += compressedRts.Length;
            columnIndex.endRtListPtr = StartPosition;
            AirdStream.Write(compressedRts, 0, compressedRts.Length);

            columnIndex.spectraIds = new int[columnIndex.mzs.Length];
            columnIndex.intensities = new int[columnIndex.mzs.Length];
            columnIndex.startPtr = StartPosition;

            int step = 100000;
            long[] anchors = new long[columnIndex.mzs.Length / step + 1];

            for (var i = 0; i < columnIndex.mzs.Length; i++)
            {
                //每隔10W个数插入一帧
                if (i % step == 0)
                {
                    anchors[i / step] = StartPosition;
                }

                int mz = columnIndex.mzs[i];
                ByteColumn byteColumn = compressedColumns[mz];
                if (byteColumn.spectraIds != null && byteColumn.intensities != null)
                {
                    columnIndex.spectraIds[i] = byteColumn.spectraIds.Length;
                    columnIndex.intensities[i] = byteColumn.intensities.Length;
                    StartPosition = StartPosition + byteColumn.spectraIds.Length + byteColumn.intensities.Length;

                    AirdStream.Write(byteColumn.spectraIds, 0, byteColumn.spectraIds.Length);
                    AirdStream.Write(byteColumn.intensities, 0, byteColumn.intensities.Length);
                }
                else
                {
                    columnIndex.spectraIds[i] = 0;
                    columnIndex.intensities[i] = 0;
                }
            }

            columnIndex.endPtr = StartPosition;
            columnIndex.anchors = anchors;
            byte[] compressedSpectraIds = ByteTrans.intToByte(new VarByteWrapper().encode(columnIndex.spectraIds));
            byte[] compressedInts = ByteTrans.intToByte(new VarByteWrapper().encode(columnIndex.intensities));

            //写入矩阵的横坐标实际值
            columnIndex.startSpectraIdListPtr = StartPosition;
            StartPosition += compressedSpectraIds.Length;
            columnIndex.endSpectraIdListPtr = StartPosition;
            AirdStream.Write(compressedSpectraIds, 0, compressedSpectraIds.Length);

            //写入矩阵的横坐标实际值
            columnIndex.startIntensityListPtr = StartPosition;
            StartPosition += compressedInts.Length;
            columnIndex.endIntensityListPtr = StartPosition;
            AirdStream.Write(compressedInts, 0, compressedInts.Length);

            columnIndex.mzs = null;
            columnIndex.rts = null;
            columnIndex.spectraIds = null;
            columnIndex.intensities = null;

            ColumnIndexList.Add(columnIndex);
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

            switch (JobInfo.format)
            {
                case FileFormat.imzML:
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
                    break;
                case FileFormat.mzML:
                    FileInfo mzML = new(JobInfo.inputPath);
                    if (mzML.Exists)
                    {
                        string path = Path.Combine(tempPath, mzML.Name);
                        File.Copy(mzML.FullName, path, true);
                    }
                    break;
                case FileFormat.mzXML:
                    FileInfo mzXML = new(JobInfo.inputPath);
                    if (mzXML.Exists)
                    {
                        string path = Path.Combine(tempPath, mzXML.Name);
                        File.Copy(mzXML.FullName, path, true);
                    }
                    break;
                case FileFormat.D:
                    DirectoryInfo directory = new(JobInfo.inputPath);
                    if (directory.Exists)
                    {
                        string path = Path.Combine(tempPath, directory.Name);
                        AirdProFileUtil.CopyFolder(directory.FullName, path);
                    }
                    break;
                default:
                    FileInfo file = new(JobInfo.inputPath);
                    if (file.Exists)
                    {
                        string path = Path.Combine(tempPath, file.Name);
                        File.Copy(file.FullName, path, true);
                    }
                    break;
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

            //chromatogramList
            chromatogramList = imzML.GetRun().GetChromatogramList();
            if (chromatogramList == null || chromatogramList.IsEmpty())
            {
                JobInfo.Log(ResultCode.No_Chromatograms_Found);
            }
            else
            {
                TotalChromaCount = chromatogramList.Size();
            }

            JobInfo.Log(Tag.Adapting_Finished);
            JobInfo.Log(Tag.Total_Spectra + TotalSpectraCount);
            JobInfo.Log(Tag.Total_Chromatograms + TotalChromaCount);
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

        public void ClearCache()
        {
            Ranges = [];
            RangeTable = [];
            IndexList = [];
            Ms2Table = [];
            Ms1List = [];
            FeaturesMap = [];
            MobiDict = [];
            MobiInfo = new();
            ChromatogramIndex = new();
            if (imzML != null)
            {
                imzML = null;
            }
            if (spectrumList != null)
            {
                spectrumList = null;
            }
            if (chromatogramList != null)
            {
                chromatogramList = null;
            }
        }

        //DDA模式下,key为ms2Index.pNum, DIA模式下,key为ms2Index.precursorMz
        protected void AddToMs2Map(Object key, MsIndex ms2Index)
        {
            if (Ms2Table.Contains(key))
            {
                ((List<MsIndex>)Ms2Table[key]).Add(ms2Index);
            }
            else
            {
                List<MsIndex> indexList = [];
                indexList.Add(ms2Index);
                Ms2Table.Add(key, indexList);
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
            ms1.rt = CVUtil.ParseRt(scan, JobInfo);
            ms1.tic = CVUtil.ParseTic(spectrum);
            ms1.basePeakIntensity = CVUtil.ParseBasePeakIntensity(spectrum);
            ms1.basePeakMz = CVUtil.ParseBasePeakMz(spectrum);
            ms1.injectionTime = CVUtil.ParseInjectionTime(scan);
            if (MobiInfo.unit == null || MobiInfo.type == null)
            {
                CVUtil.ParseMobility(scan, MobiInfo);
            }
            ms1.msType = CVUtil.ParseMsType(spectrum);
            ms1.polarity = CVUtil.ParsePolarity(spectrum);
            ms1.activator = Activator.UNKNOWN;
            ms1.energy = -1;

            return ms1;
        }

        protected MsIndex ParseMs2(Spectrum spectrum, int num, int pNum)
        {
            MsIndex ms2 = new()
            {
                level = 2,
                pNum = pNum,
                num = num
            };

            Precursor precursor = spectrum.GetPrecursorList().Get(0);
            try
            {
                ms2.precursor = CVUtil.ParseIsolationWindow(precursor, JobInfo);
            }
            catch (Exception e)
            {
                JobInfo.Log(ResultCode.Error).Log(Tag.SpectrumIndex + spectrum.GetID())
                    .Log(Tag.SpectrumId + spectrum.GetID());
                IsolationWindow isolationWindow = precursor.IsolationWindow;
                CVParam cv = isolationWindow.GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_TARGET_MZ);
                JobInfo.Log(Tag.Key_MZ + cv.GetValueAsDouble());
                cv = isolationWindow.GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_LOWER_OFFSET_ID);
                JobInfo.Log(Tag.LowerOffset + cv.GetValueAsDouble());
                cv = isolationWindow.GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_UPPER_OFFSET_ID);
                JobInfo.Log(Tag.UpperOffset + cv.GetValueAsDouble());
                throw e;
            }

            if (spectrum.GetScanList().Size() < 1) return ms2;

            var (activator, energy) = CVUtil.ParseActivator(spectrum.GetPrecursorList().Get(0));
            ms2.activator = activator;
            ms2.energy = energy;
            ms2.msType = CVUtil.ParseMsType(spectrum);
            ms2.polarity = CVUtil.ParsePolarity(spectrum);
            ms2.tic = CVUtil.ParseTic(spectrum);
            ms2.basePeakIntensity = CVUtil.ParseBasePeakIntensity(spectrum);
            ms2.basePeakMz = CVUtil.ParseBasePeakMz(spectrum);

            Scan scan = spectrum.GetScanList().Get(0);
            ms2.rt = CVUtil.ParseRt(scan, JobInfo);
            ms2.injectionTime = CVUtil.ParseInjectionTime(scan);
            if (MobiInfo.unit == null || MobiInfo.type == null)
            {
                CVUtil.ParseMobility(scan, MobiInfo);
            }

            ms2.filterString = CVUtil.ParseFilterString(scan, JobInfo);

            return ms2;
        }

        public void CompressMs2BlockForPrm()
        {
            JobInfo.Log("Start Processing MS2 List");
            int progress = 0;
            foreach (double key in Ms2Table.Keys)
            {
                List<MsIndex> ms2List = Ms2Table[key] as List<MsIndex>;
                WindowRange range = new(ms2List[0].precursor.start, ms2List[0].precursor.end, key);

                BlockIndex index = new()
                {
                    level = 2,
                    startPtr = StartPosition
                }; //为每一个key组创建一个SwathBlock
                index.setWindowRange(range); //顺便创建一个WindowRanges,用以让Propro服务端快速获取全局的窗口数目和mz区间
                Ranges.Add(range);

                JobInfo.Log(null, Tag.progress(Tag.MS2, progress, Ms2Table.Keys.Count));
                progress++;
                Compressor.CompressMS2(this, ms2List, index);
                index.endPtr = StartPosition;
                IndexList.Add(index);
                JobInfo.Log("MS2 Group Finished:" + progress + "/" + Ms2Table.Keys.Count);
            }
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

        public void CompressMs2BlockForDia()
        {
            if (JobInfo.config.noMS2)
            {
                JobInfo.Log(Tag.FILTER_NO_MS2);
                return;
            }
            JobInfo.Log(Tag.Start_Processing_MS2_List);
            int progress = 0;
            foreach (double precursorMz in Ms2Table.Keys)
            {
                List<MsIndex> ms2List = Ms2Table[precursorMz] as List<MsIndex>;
                WindowRange range = RangeTable[precursorMz] as WindowRange;

                BlockIndex index = new()
                {
                    level = 2,
                    startPtr = StartPosition
                }; //为每一个key组创建一个SwathBlock
                index.setWindowRange(range);

                JobInfo.Log(null, Tag.progress(Tag.MS2, progress, Ms2Table.Keys.Count));
                progress++;
                Compressor.CompressMS2(this, ms2List, index);
                index.endPtr = StartPosition;
                IndexList.Add(index);
                JobInfo.Log(Tag.progress(Tag.MS2_Group_Finished, progress, Ms2Table.Keys.Count));
            }
        }

        //处理MS2,由于每一个MS1只跟随少量的MS2光谱图,因此DDA采集模式下MS2的压缩模式仍然使用Aird ZDPD的压缩算法
        public void compressMS2BlockForDDA()
        {
            int progress = 0;
            JobInfo.Log(Tag.Start_Processing_MS2_List);
            ArrayList keys = new(Ms2Table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                List<MsIndex> tempIndexList = Ms2Table[key] as List<MsIndex>;
                //为每一组key创建一个Block
                BlockIndex blockIndex = new()
                {
                    level = 2,
                    startPtr = StartPosition,
                    num = key
                };
                //创建这一个block中每一个ms2的窗口序列
                List<WindowRange> ms2Ranges = [];
                JobInfo.Log(null, Tag.progress(Tag.MS2, progress, Ms2Table.Keys.Count));
                progress++;

                foreach (MsIndex index in tempIndexList)
                {
                    // WindowRange range = new WindowRange(index.mzStart, index.mzEnd, index.precursorMz);
                    WindowRange range = index.precursor;
                    ms2Ranges.Add(range);
                    TempScan ts = new(index);
                    if (JobInfo.ionMobility)
                    {
                        Compressor.CompressMobility(spectrumList.GetSpectrum(index.num), ts);
                    }
                    else
                    {
                        Compressor.Compress(spectrumList.GetSpectrum(index.num), ts);
                    }

                    blockIndex.nums.Add(ts.num);
                    blockIndex.rts.Add(ts.rt);
                    blockIndex.tics.Add(ts.tic);

                    blockIndex.polarities.Add(ts.polarity);
                    blockIndex.energies.Add(ts.energy);
                    blockIndex.activators.Add(ts.activator);
                    blockIndex.tics.Add(ts.tic);

                    blockIndex.basePeakIntensities.Add(ts.basePeakIntensity);
                    blockIndex.basePeakMzs.Add(ts.basePeakMz);
                    blockIndex.mzs.Add(ts.mzArrayBytes.Length);
                    blockIndex.ints.Add(ts.intArrayBytes.Length);
                    StartPosition = StartPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                    AirdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                    AirdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
                    if (ts.mobilityArrayBytes != null)
                    {
                        blockIndex.mobilities.Add(ts.mobilityArrayBytes.Length);
                        StartPosition += ts.mobilityArrayBytes.Length;
                        AirdStream.Write(ts.mobilityArrayBytes, 0, ts.mobilityArrayBytes.Length);
                    }
                }

                blockIndex.rangeList = ms2Ranges;
                blockIndex.endPtr = StartPosition;
                IndexList.Add(blockIndex);
            }
        }

        public void CompressChromatograms()
        {
            if (chromatogramList == null || chromatogramList.Size() == 0)
            {
                return;
            }

            ChromatogramIndex = new ChromatogramIndex();
            //如果是.d的文件夹类型的质谱文件,可以直接解析AcqMethod.xml文件,用于读取设定的化合物名称
            ReadMRMCompounds();

            int totalSize = chromatogramList.Size();
            int progress = 0;
            JobInfo.Log(null, Tag.progress(Tag.Chroma, progress, totalSize));
            ChromatogramIndex.startPtr = StartPosition;
            for (int i = 0; i < chromatogramList.Size(); i++)
            {
                Chromatogram chromatogram = chromatogramList.Get(i);
                TempScanChroma tempScan = new();
                ChromatogramIndex.nums.Add(i);
                ChromatogramIndex.ids.Add(chromatogram.GetID());
                var (activator, energy) = CVUtil.ParseActivator(chromatogram.precursor);
                ChromatogramIndex.activators.Add(activator);
                ChromatogramIndex.energies.Add(energy);
                ChromatogramIndex.polarities.Add(CVUtil.ParsePolarity(chromatogram));

                try
                {
                    Precursor precursor = chromatogram.precursor;
                    WindowRange precursorMz = CVUtil.ParseIsolationWindow(precursor, JobInfo);
                    IsolationWindow isolationWindow = chromatogram.product.getIsolationWindow();
                    WindowRange productMz = CVUtil.ParseIsolationWindow(isolationWindow, JobInfo);
                    string ionPair = Math.Round(precursorMz.mz, 1) + "-" + Math.Round(productMz.mz, 1);
                    if (MrmCompoundDict.ContainsKey(ionPair))
                    {
                        string compoundName = MrmCompoundDict[ionPair].name;
                        ChromatogramIndex.compounds.Add(compoundName);
                    }
                    else
                    {
                        ChromatogramIndex.compounds.Add("");
                    }
                    ChromatogramIndex.products.Add(productMz);
                    ChromatogramIndex.precursors.Add(precursorMz);
                }
                catch (Exception e)
                {
                    JobInfo.Log(ResultCode.Error).Log(Tag.SpectrumIndex + i)
                        .Log(Tag.SpectrumId + chromatogram.GetID())
                        .Log(Tag.Key_MZ + chromatogram.precursor.GetIsolationWindow()
                            .GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_TARGET_MZ).GetValueAsDouble())
                        .Log(Tag.LowerOffset + chromatogram.precursor.GetIsolationWindow()
                            .GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_LOWER_OFFSET_ID).GetValueAsDouble())
                        .Log(Tag.UpperOffset + chromatogram.precursor.GetIsolationWindow()
                            .GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_UPPER_OFFSET_ID).GetValueAsDouble());
                    throw e;
                }

                Compressor.Compress(chromatogram, tempScan);
                ChromatogramIndex.rts.Add(tempScan.rtArrayBytes.Length);
                ChromatogramIndex.ints.Add(tempScan.intArrayBytes.Length);
                StartPosition = StartPosition + tempScan.rtArrayBytes.Length + tempScan.intArrayBytes.Length;
                AirdStream.Write(tempScan.rtArrayBytes, 0, tempScan.rtArrayBytes.Length);
                AirdStream.Write(tempScan.intArrayBytes, 0, tempScan.intArrayBytes.Length);

                progress++;
                JobInfo.Log(null, Tag.progress(Tag.Chroma, progress, totalSize));
            }

            ChromatogramIndex.totalCount = ChromatogramIndex.ids.Count;
            ChromatogramIndex.endPtr = StartPosition;
        }

        /**
         * 用于解析.d文件中的AcqMethod.XML文件
         */
        public void ReadMRMCompounds()
        {
            if (JobInfo.format.Equals(FileFormat.D) && JobInfo.type.Equals(AcquisitionMethod.MRM))
            {
                MrmCompoundDict = new AcqMethodParser(JobInfo.inputPath).parse();
            }
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
            //Scan index and window range info
            airdInfo.rangeList = Ranges;

            //Block index
            airdInfo.indexList = IndexList;

            //ChromatogramIndex
            airdInfo.chromatogramIndex = ChromatogramIndex;

            //Instrument Info
            List<Instrument> instruments = [];
            foreach (InstrumentConfiguration ic in imzML.GetInstrumentConfigurationList())
            {
                Instrument instrument = new();

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
            airdInfo.msiInfo = MsiUtil.GetMsiInfo(imzML);

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

        protected ColumnInfo BuildColumnInfo()
        {
            ColumnInfo columnInfo = new()
            {
                type = JobInfo.type,
                indexList = ColumnIndexList,
                mzPrecision = JobInfo.config.mzPrecision,
                intPrecision = IntensityPrecision,
                airdPath = JobInfo.airdFilePath
            };
            return columnInfo;
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
            int parentNum = 0;
            JobInfo.Log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            for (var i = 0; i < TotalSpectraCount; i++)
            {
                Spectrum spectrum = spectrumList.Get(i);
                string msLevel = CVUtil.ParseMsLevel(spectrum);
                JobInfo.SetStatus("Pre:" + i + "/" + TotalSpectraCount);
                //最后一个谱图,单独判断
                if (i == TotalSpectraCount - 1)
                {
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        Ms1List.Add(ParseMs1(spectrum, i)); //如果是MS1谱图,加入到MS1List
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
                        Spectrum next = spectrumList.Get(i + 1);
                        if (CVUtil.ParseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                        {
                            parentNum = i;
                        }
                    }

                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = ParseMs2(spectrum, i, parentNum);
                        AddToMs2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
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
                Spectrum spectrum = spectrumList.Get(i);
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

            JobInfo.Log(Tag.Total_SWATH_WINDOWS + Ranges.Count);
            JobInfo.Log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.Log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.Log(Tag.Start_Processing_MS1_List);
        }

        public void PretreatmentDdaPasef()
        {
            int parentNum = 0;
            JobInfo.Log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            for (var i = 0; i < TotalSpectraCount; i++)
            {
                JobInfo.Log(null, Tag.progress(Tag.Pre, i, TotalSpectraCount));
                Spectrum spectrum = spectrumList.Get(i);
                string msLevel = CVUtil.ParseMsLevel(spectrum);
                //最后一个谱图,单独判断
                if (i == TotalSpectraCount - 1)
                {
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        Ms1List.Add(ParseMs1(spectrum, i)); //如果是MS1谱图,加入到MS1List
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
                        Spectrum next = spectrumList.Get(i + 1);
                        if (CVUtil.ParseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                        {
                            parentNum = i;
                        }
                    }

                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = ParseMs2(spectrum, i, parentNum);
                        AddToMs2Map(ms2Index.pNum, ms2Index); //如果这个谱图是MS2
                    }
                }
            }

            JobInfo.Log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.Log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.Log(Tag.Start_Processing_MS1_List);
        }

        public void PretreatmentDiaPasef()
        {
            int parentNum = 0;
            JobInfo.Log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < TotalSpectraCount; i++)
            {
                progress++;
                JobInfo.Log(null, Tag.progress(Tag.Pre, progress, TotalSpectraCount));
                Spectrum spectrum = spectrumList.Get(i);
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

            JobInfo.Log(Tag.Total_SWATH_WINDOWS + Ranges.Count);
            JobInfo.Log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.Log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.Log(Tag.Start_Processing_MS1_List);
        }

        public void PretreatmentPrm()
        {
            int parentNum = 0;
            JobInfo.Log(Status.tag_preprocessing + TotalSpectraCount, Status.Preprocessing);
            for (int i = 0; i < TotalSpectraCount; i++)
            {
                JobInfo.Log(null, Tag.progress(Tag.Empty, (i + 1), TotalSpectraCount));
                Spectrum spectrum = spectrumList.Get(i);
                string msLevel = CVUtil.ParseMsLevel(spectrum);
                //如果是最后一个谱图,那么单独判断
                if (i == TotalSpectraCount - 1)
                {
                    //如果是MS1谱图,那么直接跳过
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        continue;
                    }

                    //如果是MS2谱图,加入到谱图组
                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = ParseMs2(spectrumList.Get(i), i, parentNum);
                        AddToMs2Map(ms2Index.precursor.mz, ms2Index);
                        continue;
                    }
                }

                //如果这个谱图是MS1
                if (msLevel.Equals(MsLevel.MS1))
                {
                    Spectrum next = spectrumList.GetSpectrum(i + 1);
                    string msLevelNext = CVUtil.ParseMsLevel(next);
                    //如果下一个谱图仍然是MS1, 那么直接忽略这个谱图
                    if (msLevelNext.Equals(MsLevel.MS1))
                    {
                        continue;
                    }

                    if (msLevelNext.Equals(MsLevel.MS2))
                    {
                        parentNum = i;
                        Ms1List.Add(ParseMs1(spectrumList.GetSpectrum(i), i));
                    }
                }

                if (msLevel.Equals(MsLevel.MS2))
                {
                    Spectrum current = spectrumList.GetSpectrum(i);
                    MsIndex ms2Index = ParseMs2(current, i, parentNum);
                    AddToMs2Map(ms2Index.precursor.mz, ms2Index); //如果这个谱图是MS2
                }
            }

            JobInfo.Log("Effective MS1 List Size:" + Ms1List.Count);
            JobInfo.Log("MS2 Group List Size:" + Ms2Table.Count);
            JobInfo.Log("Start Processing MS1 List");
        }

    }    
}
