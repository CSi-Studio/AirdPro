/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2.
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirdPro.Algorithms;
using AirdPro.Algorithms.Parser;
using AirdPro.Constants;
using AirdPro.Domains;
using AirdPro.Utils;
using AirdSDK.Beans;
using AirdSDK.Beans.Common;
using AirdSDK.Compressor;
using AirdSDK.Enums;
using AirdSDK.Utils;
using Newtonsoft.Json;
using pwiz.CLI.analysis;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using Activator = AirdPro.Constants.Activator;
using Software = AirdSDK.Beans.Software;
using Spectrum = pwiz.CLI.msdata.Spectrum;

namespace AirdPro.Converters
{
    public class PwizConverter : Converter
    {
        protected MSData Msd; //非托管内存，需要手动回收

        public SpectrumList SpectrumList;
        protected ChromatogramList ChromatogramList;
        protected List<WindowRange> Ranges = new(); //SWATH/DIA Window的窗口
        protected Hashtable RangeTable = new(); //用于存放SWATH/DIA窗口的信息,key为mz
        protected List<BlockIndex> IndexList = new(); //用于存储的全局的SWATH List
        protected List<ColumnIndex> ColumnIndexList = new(); //列存储索引，尽在面向Search的场景下有效

        protected Hashtable
            Ms2Table = Hashtable.Synchronized(new Hashtable()); //用于存放MS2的索引信息,DDA采集模式下key为ms1的num, DIA采集模式下key为mz

        public List<MsIndex> Ms1List = new(); //用于存放MS1索引及基础信息,泛型为MsIndex
        protected Hashtable FeaturesMap = new();

        //用于离子淌度相关的字段
        public double[] MobiArray;
        public Dictionary<double, int> MobiDict;
        public MobiInfo MobiInfo = new();
        protected int MobiPrecision = 10000000; //mobility默认精确到小数点后7位

        protected int IntensityPrecision = 1; //Intensity默认精确到个位数

        protected int SpectraNumForIntensityPrecisionPredict = 5; //用于ComboComp预测Intensity精度时的采样光谱数
        public ICompressor Compressor;
        public ChromatogramIndex ChromatogramIndex;

        public Dictionary<string, AcqCompound>
            MrmCompoundDict = new(); //用于MRM采集模式下,预存储化合物名称与离子对的词典,当前仅适用于Agilent的.d文件夹类型的质谱文件

        public override void Init(JobInfo jobInfo)
        {
            JobInfo = jobInfo;
            InitCompressor();
        }

        private void InitCompressor()
        {
            ICompressor comp = new CoreComp(this);
            //探索模式和非自动决策模式,会在此处初始化指定的压缩内核
            if (!JobInfo.config.autoDesicion)
            {
                if (JobInfo.ionMobility)
                {
                    comp.MobiIntComp = IntComp.build(JobInfo.config.mobiIntComp);
                    comp.MobiByteComp = ByteComp.build(JobInfo.config.mobiByteComp);
                }

                comp.MzIntComp = SortedIntComp.build(JobInfo.config.mzIntComp);
                comp.MzByteComp = ByteComp.build(JobInfo.config.mzByteComp);

                comp.IntIntComp = IntComp.build(JobInfo.config.intIntComp);
                comp.IntByteComp = ByteComp.build(JobInfo.config.intByteComp);
            }

            this.Compressor = comp;
        }

        public override void DoConvert()
        {
            Start();
            using (MSDataList msdList = readVendorFile())
            {
                try
                {
                    if (msdList.Count == 0)
                    {
                        return;
                    }

                    foreach (var msd in msdList)
                    {
                        StartPosition = 0;
                        if (msdList.Count > 1) //如果msdList中包含多个msd，那么每一个msd会被单独导出为一个文件，导出的文件名按照msd的ID进行命名
                        {
                            String id = msd.id;
                            JobInfo.airdFilePath = Path.Combine(JobInfo.outputPath, id + ".aird");
                            JobInfo.airdJsonFilePath = Path.Combine(JobInfo.outputPath, id + ".json");
                            JobInfo.airdFileName = id;
                        }

                        readMsd(msd);
                        InitDirectory(); //创建文件夹
                        using (AirdStream = new FileStream(JobInfo.airdFilePath, FileMode.Create))
                        {
                            using (AirdJsonStream = new FileStream(JobInfo.airdJsonFilePath, FileMode.Create))
                            {
                                predictAcquisitionMethod();
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
                        }

                        msd?.Dispose();
                        clearCache();
                    }
                }
                finally
                {
                    finish();
                }
            }
        }

        public void finish()
        {
            Stopwatch.Stop();
            JobInfo.refreshReport = true;
            JobInfo.log(Tag.Total_Time_Cost + Stopwatch.Elapsed.TotalSeconds, Status.Finished);
            clearCache();
            JobInfo.setStatus(ProcessingStatus.FINISHED);
            if (Msd != null)
            {
                Msd.Dispose();
                Msd = null;
            }
        }

        public void initBrukerMobi()
        {
            JobInfo.log(Tag.Init_Mobility_Array);
            long handle = TdfUtil.tims_open(JobInfo.inputPath, 1);
            double[] scanNums = new double[2000];
            for (int i = 0; i < scanNums.Length; i++)
            {
                scanNums[i] = i;
            }

            double[] mobility = new double[2000];
            TdfUtil.tims_scannum_to_oneoverk0(handle, 1, scanNums, mobility, scanNums.Length);
            TdfUtil.tims_close(handle);
            MobiDict = new Dictionary<double, int>();
            for (short i = 0; i < mobility.Length; i++)
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
        public void predictAcquisitionMethod()
        {
            if (!JobInfo.type.Equals(JobInfo.AutoType))
            {
                JobInfo.setType(JobInfo.type);
                return;
            }

            bool mobi = false;
            JobInfo.log(Tag.Predict_Acquisition_Method, Status.Init);

            //如果有光谱图
            if (SpectrumList != null && SpectrumList.size() > 0)
            {
                Spectrum firstSpec = SpectrumList.spectrum(0, true);
                List<Spectrum> predictSpecList = new List<Spectrum>();
                //首先取10个窗口
                for (int i = 0; i < 10; i++)
                {
                    predictSpecList.Add(SpectrumList.spectrum(i, true));
                }

                //首先判断是不是带有离子淌度的ion mobility模式
                if (firstSpec.binaryDataArrays.Count == 3)
                {
                    foreach (BinaryDataArray dataArray in firstSpec.binaryDataArrays)
                    {
                        if (dataArray.cvParams[0].cvid.Equals(CVID.MS_mean_inverse_reduced_ion_mobility_array))
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
                    if (CVUtil.parseMsLevel(spectrum).Equals(MsLevel.MS2))
                    {
                        double width = CVUtil.parsePrecursorWidth(spectrum.precursors[0].isolationWindow, JobInfo);
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
                    JobInfo.setType(AcquisitionMethod.DDA_PASEF);
                }

                if (isDDA && !mobi)
                {
                    JobInfo.setType(AcquisitionMethod.DDA);
                }

                if (isDIA && mobi)
                {
                    JobInfo.setType(AcquisitionMethod.DIA_PASEF);
                }

                if (isDIA && !mobi)
                {
                    JobInfo.setType(AcquisitionMethod.DIA);
                }
            }

            try
            {
                //如果有色谱图,且谱图数目大于2(排除TIC和BPC图),则预测为SRM模式
                if (ChromatogramList != null && ChromatogramList.size() > 10)
                {
                    List<Chromatogram> predictChromatoList = new List<Chromatogram>();
                    // 首先取10个窗口
                    for (int i = 0; i < 10; i++)
                    {
                        using (Chromatogram chroma = ChromatogramList.chromatogram(i, false))
                        {
                            predictChromatoList.Add(chroma);
                        }
                    }

                    JobInfo.setType(AcquisitionMethod.MRM);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Hello");
            }
        }

        public void predictForBestCombination()
        {
            if (!JobInfo.config.autoDesicion)
            {
                return;
            }

            JobInfo.log(Tag.Predict_For_Best_Combination + JobInfo.airdFileName, Status.Predicting);
            Combination combination = randomSampling(JobInfo.config.spectraToPredict, JobInfo.ionMobility);
            combination.enable(JobInfo.config, Compressor);
            JobInfo.log(JobInfo.getCompressorStr());
            JobInfo.config.autoDesicion = false;
            JobInfo.setCombination(JobInfo.getCompressorStr());
        }

        /**
         * num:采样数目,建议:5
         */
        public void predictForIntensityPrecision()
        {
            Random rd = new Random();
            HashSet<int> nums = new HashSet<int>();
            for (int i = 0; i < SpectraNumForIntensityPrecisionPredict; i++)
            {
                nums.Add(rd.Next(1, TotalSpectraCount));
            }

            bool findIt = false;
            for (var i = 0; i < nums.Count; i++)
            {
                using Spectrum spectrum = SpectrumList.spectrum(i, true);
                foreach (double d in spectrum.getIntensityArray().data.Storage())
                {
                    if ((d - (int)d) != 0) //如果随机采集到的intensity是精确到小数点后一位的,精确确定为10,即精确到小数点后一位
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
            JobInfo.log(Tag.Intensity_Precision + IntensityPrecision);
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

        /**
         * 存储列存储数据
         * 注意，本函数会操作startPosition这个全局变量
         */
        public void writeColumnData(ConcurrentDictionary<int, ByteColumn> compressedColumns, ColumnIndex columnIndex)
        {
            byte[] compressedMzs =
                new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new IntegratedVarByteWrapper().encode(columnIndex.mzs)));
            byte[] compressedRts =
                new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new IntegratedVarByteWrapper().encode(columnIndex.rts)));
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
            for (var i = 0; i < columnIndex.mzs.Length; i++)
            {
                int mz = columnIndex.mzs[i];
                ByteColumn byteColumn = compressedColumns[mz];
                if (byteColumn.indexIds != null && byteColumn.intensities != null)
                {
                    columnIndex.spectraIds[i] = byteColumn.indexIds.Length;
                    columnIndex.intensities[i] = byteColumn.intensities.Length;
                    StartPosition = StartPosition + byteColumn.indexIds.Length + byteColumn.intensities.Length;
                    AirdStream.Write(byteColumn.indexIds, 0, byteColumn.indexIds.Length);
                    AirdStream.Write(byteColumn.intensities, 0, byteColumn.intensities.Length);
                }
                else
                {
                    columnIndex.spectraIds[i] = 0;
                    columnIndex.intensities[i] = 0;
                }
            }

            columnIndex.endPtr = StartPosition;

            byte[] compressedSpectraIds =
                new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new VarByteWrapper().encode(columnIndex.spectraIds)));
            byte[] compressedInts =
                new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new VarByteWrapper().encode(columnIndex.intensities)));
            //写入矩阵的横坐标实际值
            columnIndex.startSpecrtaIdListPtr = StartPosition;
            StartPosition += compressedSpectraIds.Length;
            columnIndex.endSpecrtaIdListPtr = StartPosition;
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
        public void addToIndex(BlockIndex index, object tempScan)
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
            index.cvList.Add(ts.cvs);

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

        /**
         * 引用本函数的时候需要注意在使用完MSDataList对象以后需要手动释放
         */
        protected MSDataList readVendorFile()
        {
            JobInfo.log(Tag.Prepare_To_Parse_Vendor_File, Status.Prepare);
            ReaderList readerList = ReaderList.FullReaderList;
            var readerConfig = new ReaderConfig
            {
                allowMsMsWithoutPrecursor = false,
                combineIonMobilitySpectra = true,
                ignoreZeroIntensityPoints = JobInfo.config.ignoreZeroIntensity
            };

            MSDataList msInfo = new MSDataList();
            readerList.read(JobInfo.inputPath, msInfo, readerConfig);

            if (msInfo.Count == 0)
            {
                JobInfo.logError(ResultCode.Reading_Vendor_File_Error_Run_Is_Null);
                msInfo.Dispose();
                readerList.Dispose();
                return null;
            }

            JobInfo.log(Tag.Adapting_Vendor_File_API, Status.Adapting);

            switch (JobInfo.format)
            {
                case FileFormat.WIFF:
                case FileFormat.WIFF2:
                    FileInfo wiff = new FileInfo(JobInfo.inputPath);
                    if (wiff.Exists) FileSize += wiff.Length;
                    if (JobInfo.inputPath.ToLower().EndsWith(".wiff"))
                    {
                        FileInfo wiff2 = new FileInfo(JobInfo.inputPath.Replace("wiff", "wiff2"));
                        if (wiff2.Exists) FileSize += wiff2.Length;
                    }
                    else
                    {
                        FileInfo wiff1 = new FileInfo(JobInfo.inputPath.Replace("wiff2", "wiff"));
                        if (wiff1.Exists) FileSize += wiff1.Length;
                    }

                    FileInfo mtd = new FileInfo(JobInfo.inputPath + ".mtd");
                    if (mtd.Exists) FileSize += mtd.Length;
                    FileInfo scan = new FileInfo(JobInfo.inputPath + ".scan");
                    if (scan.Exists) FileSize += scan.Length;
                    FileInfo timeseries = new FileInfo(JobInfo.inputPath + ".timeseries.data");
                    if (timeseries.Exists) FileSize += timeseries.Length;
                    break;
                case FileFormat.RAW:
                    FileInfo raw = new FileInfo(JobInfo.inputPath);
                    if (raw.Exists) FileSize += raw.Length;
                    break;
                case FileFormat.mzML:
                    FileInfo mzML = new FileInfo(JobInfo.inputPath);
                    if (mzML.Exists) FileSize += mzML.Length;
                    break;
                case FileFormat.mzXML:
                    FileInfo mzXML = new FileInfo(JobInfo.inputPath);
                    if (mzXML.Exists) FileSize += mzXML.Length;
                    break;
                case FileFormat.D:
                    long totalSize = AirdProFileUtil.getDirectorySize(JobInfo.inputPath);
                    FileSize += totalSize;
                    break;
                default:
                    FileInfo file = new FileInfo(JobInfo.inputPath);
                    if (file.Exists) FileSize += file.Length;
                    break;
            }

            readerList.Dispose();
            return msInfo;
        }

        public void readMsd(MSData msd)
        {
            this.Msd = msd;
            List<string> filter = new List<string>();
            SpectrumListFactory.wrap(msd, filter); //这一步操作可以帮助加快Wiff文件的初始化速度

            SpectrumList = msd.run.spectrumList;
            if (SpectrumList == null || SpectrumList.empty())
            {
                JobInfo.log(ResultCode.No_Spectra_Found);
            }
            else
            {
                TotalSpectraCount = SpectrumList.size();
            }

            ChromatogramList = msd.run.chromatogramList;
            if (ChromatogramList == null || ChromatogramList.empty())
            {
                JobInfo.log(ResultCode.No_Chromatograms_Found);
            }
            else
            {
                TotalChromaCount = ChromatogramList.size();
            }

            JobInfo.log(Tag.Adapting_Finished);
            JobInfo.log(Tag.Total_Spectra + TotalSpectraCount);
            JobInfo.log(Tag.Total_Chromatograms + TotalChromaCount);
        }

        //将最终的数据写入文件中
        public void writeToAirdInfoFile()
        {
            JobInfo.log(Tag.Write_Index_File, Status.Writing_Index_File);
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

            string airdInfoStr = JsonConvert.SerializeObject(airdInfo,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            StartPosition += airdBytes.Length;
            AirdJsonStream.Write(airdBytes, 0, airdBytes.Length);

            if (JobInfo.config.isSearch())
            {
                ColumnInfo columnInfo = buildColumnInfo();
                string columnInfoStr = JsonConvert.SerializeObject(columnInfo,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] columnInfoBytes = Encoding.Default.GetBytes(columnInfoStr);
                using (AirdColumnJsonStream = new FileStream(JobInfo.airdColumnJsonFilePath, FileMode.Create))
                {
                    AirdColumnJsonStream.Write(columnInfoBytes, 0, columnInfoBytes.Length);
                }
            }
        }

        public void clearCache()
        {
            Ranges = new();
            RangeTable = new();
            IndexList = new();
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

        //DDA模式下,key为ms2Index.pNum, DIA模式下,key为ms2Index.precursorMz
        protected void addToMS2Map(Object key, MsIndex ms2Index)
        {
            if (Ms2Table.Contains(key))
            {
                (Ms2Table[key] as List<MsIndex>).Add(ms2Index);
            }
            else
            {
                List<MsIndex> indexList = new List<MsIndex>();
                indexList.Add(ms2Index);
                Ms2Table.Add(key, indexList);
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

            using (Scan scan = spectrum.scanList.scans[0])
            {
                // ms1.cvs = CVUtil.trans(spectrum.cvParams);
                //将对应scan的cvParams也冗余到ms1上来
                // if (scan.cvParams != null)
                // {
                //     ms1.cvs.AddRange(CVUtil.trans(scan.cvParams));
                // }

                ms1.filterString = CVUtil.parseFilterString(scan, JobInfo);
                ms1.rt = CVUtil.parseRT(scan, JobInfo);
                ms1.tic = CVUtil.parseTIC(spectrum);
                ms1.basePeakIntensity = CVUtil.parseBasePeakIntensity(spectrum);
                ms1.basePeakMz = CVUtil.parseBasePeakMz(spectrum);
                ms1.injectionTime = CVUtil.parseInjectionTime(scan);
                if (MobiInfo.unit == null || MobiInfo.type == null)
                {
                    CVUtil.parseMobility(scan, MobiInfo);
                }

                ms1.msType = CVUtil.parseMsType(spectrum);
                ms1.polarity = CVUtil.parsePolarity(spectrum);
                ms1.activator = Activator.UNKNOWN;
                ms1.energy = -1;
            }

            return ms1;
        }

        protected MsIndex parseMS2(Spectrum spectrum, int num, int pNum)
        {
            MsIndex ms2 = new MsIndex();
            ms2.level = 2;
            ms2.pNum = pNum;
            ms2.num = num;

            using (Precursor precursor = spectrum.precursors[0])
            {
                try
                {
                    ms2.precursor = CVUtil.parseIsolationWindow(precursor, JobInfo);
                }
                catch (Exception e)
                {
                    JobInfo.log(ResultCode.Error).log(Tag.SpectrumIndex + spectrum.index)
                        .log(Tag.SpectrumId + spectrum.id);
                    using (IsolationWindow isolationWindow = precursor.isolationWindow)
                    {
                        using (var cv = isolationWindow.cvParamChild(CVID.MS_isolation_window_target_m_z))
                        {
                            JobInfo.log(Tag.Key_MZ + cv.value);
                        }

                        using (var cv = isolationWindow.cvParamChild(CVID.MS_isolation_window_lower_offset))
                        {
                            JobInfo.log(Tag.LowerOffset + cv.value);
                        }

                        using (var cv = isolationWindow.cvParamChild(CVID.MS_isolation_window_upper_offset))
                        {
                            JobInfo.log(Tag.UpperOffset + cv.value);
                        }
                    }

                    throw e;
                }
            }

            if (spectrum.scanList.scans.Count != 1) return ms2;

            var result = CVUtil.parseActivator(spectrum.precursors[0]);
            ms2.activator = result.activator;
            ms2.energy = result.energy;
            ms2.msType = CVUtil.parseMsType(spectrum);
            ms2.polarity = CVUtil.parsePolarity(spectrum);
            ms2.tic = CVUtil.parseTIC(spectrum);
            ms2.basePeakIntensity = CVUtil.parseBasePeakIntensity(spectrum);
            ms2.basePeakMz = CVUtil.parseBasePeakMz(spectrum);

            // using (CVParamList cvParams = spectrum.cvParams)
            // {
            // ms2.cvs = CVUtil.trans(cvParams);
            // }

            using (Scan scan = spectrum.scanList.scans[0])
            {
                ms2.rt = CVUtil.parseRT(scan, JobInfo);
                ms2.injectionTime = CVUtil.parseInjectionTime(scan);
                if (MobiInfo.unit == null || MobiInfo.type == null)
                {
                    CVUtil.parseMobility(scan, MobiInfo);
                }

                ms2.filterString = CVUtil.parseFilterString(scan, JobInfo);

                // using (CVParamList cvParams = scan.cvParams)
                // {
                //     if (cvParams != null) ms2.cvs.AddRange(CVUtil.trans(cvParams));
                // }
            }

            return ms2;
        }

        public void CompressMs2BlockForPrm()
        {
            JobInfo.log("Start Processing MS2 List");
            int progress = 0;
            foreach (double key in Ms2Table.Keys)
            {
                List<MsIndex> ms2List = Ms2Table[key] as List<MsIndex>;
                WindowRange range = new WindowRange(ms2List[0].precursor.start, ms2List[0].precursor.end, key);

                BlockIndex index = new BlockIndex(); //为每一个key组创建一个SwathBlock
                index.level = 2;
                index.startPtr = StartPosition;
                index.setWindowRange(range); //顺便创建一个WindowRanges,用以让Propro服务端快速获取全局的窗口数目和mz区间
                Ranges.Add(range);

                JobInfo.log(null, Tag.progress(Tag.MS2, progress, Ms2Table.Keys.Count));
                progress++;
                Compressor.CompressMs2(this, ms2List, index);
                index.endPtr = StartPosition;
                IndexList.Add(index);
                JobInfo.log("MS2 Group Finished:" + progress + "/" + Ms2Table.Keys.Count);
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
            BlockIndex index = new BlockIndex();
            index.level = 1;
            index.startPtr = StartPosition;
            Compressor.CompressMs1(this, index);
            index.endPtr = StartPosition;
            IndexList.Add(index);
        }

        public void CompressMs2BlockForDia()
        {
            JobInfo.log(Tag.Start_Processing_MS2_List);
            int progress = 0;
            foreach (double precursorMz in Ms2Table.Keys)
            {
                List<MsIndex> ms2List = Ms2Table[precursorMz] as List<MsIndex>;
                WindowRange range = RangeTable[precursorMz] as WindowRange;

                BlockIndex index = new BlockIndex(); //为每一个key组创建一个SwathBlock
                index.level = 2;
                index.startPtr = StartPosition;
                index.setWindowRange(range);

                JobInfo.log(null, Tag.progress(Tag.MS2, progress, Ms2Table.Keys.Count));
                progress++;
                Compressor.CompressMs2(this, ms2List, index);
                index.endPtr = StartPosition;
                IndexList.Add(index);
                JobInfo.log(Tag.progress(Tag.MS2_Group_Finished, progress, Ms2Table.Keys.Count));
            }
        }

        //处理MS2,由于每一个MS1只跟随少量的MS2光谱图,因此DDA采集模式下MS2的压缩模式仍然使用Aird ZDPD的压缩算法
        public void compressMS2BlockForDDA()
        {
            int progress = 0;
            JobInfo.log(Tag.Start_Processing_MS2_List);
            ArrayList keys = new ArrayList(Ms2Table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                List<MsIndex> tempIndexList = Ms2Table[key] as List<MsIndex>;
                //为每一组key创建一个Block
                BlockIndex blockIndex = new BlockIndex();
                blockIndex.level = 2;
                blockIndex.startPtr = StartPosition;
                blockIndex.num = key;
                //创建这一个block中每一个ms2的窗口序列
                List<WindowRange> ms2Ranges = new List<WindowRange>();
                JobInfo.log(null, Tag.progress(Tag.MS2, progress, Ms2Table.Keys.Count));
                progress++;

                foreach (MsIndex index in tempIndexList)
                {
                    // WindowRange range = new WindowRange(index.mzStart, index.mzEnd, index.precursorMz);
                    WindowRange range = index.precursor;
                    ms2Ranges.Add(range);
                    TempScan ts = new TempScan(index);
                    if (JobInfo.ionMobility)
                    {
                        Compressor.CompressMobility(SpectrumList.spectrum(index.num, true), ts);
                    }
                    else
                    {
                        Compressor.Compress(SpectrumList.spectrum(index.num, true), ts);
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
                    blockIndex.cvList.Add(ts.cvs);
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

        public void compressChromatograms()
        {
            if (ChromatogramList == null || ChromatogramList.size() == 0)
            {
                return;
            }

            ChromatogramIndex = new ChromatogramIndex();
            Compressor.InitForChromatogram();
            //如果是.d的文件夹类型的质谱文件,可以直接解析AcqMethod.xml文件,用于读取设定的化合物名称
            readMRMCompounds();

            int totalSize = ChromatogramList.size();
            int progress = 0;
            JobInfo.log(null, Tag.progress(Tag.Chroma, progress, totalSize));
            ChromatogramIndex.startPtr = StartPosition;
            for (int i = 0; i < ChromatogramList.size(); i++)
            {
                Chromatogram chromatogram = ChromatogramList.chromatogram(i, true);
                TempScanChroma tempScan = new TempScanChroma();
                ChromatogramIndex.nums.Add(i);
                ChromatogramIndex.ids.Add(chromatogram.id);
                ChromatogramIndex.cvs.Add(CVUtil.trans(chromatogram.cvParams));

                var result = CVUtil.parseActivator(chromatogram.precursor);
                ChromatogramIndex.activators.Add(result.activator);
                ChromatogramIndex.energies.Add(result.energy);
                ChromatogramIndex.polarities.Add(CVUtil.parsePolarity(chromatogram));

                try
                {
                    using (var precursor = chromatogram.precursor)
                    {
                        WindowRange precursorMz = CVUtil.parseIsolationWindow(precursor, JobInfo);
                        using (var isolationWindow = chromatogram.product.isolationWindow)
                        {
                            WindowRange productMz = CVUtil.parseIsolationWindow(isolationWindow, JobInfo);
                            string ionPair = Math.Round(precursorMz.mz, 1) + "-" + Math.Round(productMz.mz, 1);
                            if (MrmCompoundDict.ContainsKey(ionPair))
                            {
                                string compoundName = MrmCompoundDict[ionPair].name;
                                ChromatogramIndex.compounds.Add(compoundName);
                            }
                            else
                            {
                                ChromatogramIndex.compounds.Add(null);
                            }

                            ChromatogramIndex.products.Add(productMz);
                        }

                        ChromatogramIndex.precursors.Add(precursorMz);
                    }
                }
                catch (Exception e)
                {
                    JobInfo.log(ResultCode.Error).log(Tag.SpectrumIndex + i)
                        .log(Tag.SpectrumId + chromatogram.id)
                        .log(Tag.Key_MZ + chromatogram.precursor.isolationWindow
                            .cvParamChild(CVID.MS_isolation_window_target_m_z).value)
                        .log(Tag.LowerOffset + chromatogram.precursor.isolationWindow
                            .cvParamChild(CVID.MS_isolation_window_lower_offset).value)
                        .log(Tag.UpperOffset + chromatogram.precursor.isolationWindow
                            .cvParamChild(CVID.MS_isolation_window_upper_offset).value);
                    throw e;
                }

                Compressor.Compress(chromatogram, tempScan);
                ChromatogramIndex.rts.Add(tempScan.rtArrayBytes.Length);
                ChromatogramIndex.ints.Add(tempScan.intArrayBytes.Length);
                StartPosition = StartPosition + tempScan.rtArrayBytes.Length + tempScan.intArrayBytes.Length;
                AirdStream.Write(tempScan.rtArrayBytes, 0, tempScan.rtArrayBytes.Length);
                AirdStream.Write(tempScan.intArrayBytes, 0, tempScan.intArrayBytes.Length);

                progress++;
                JobInfo.log(null, Tag.progress(Tag.Chroma, progress, totalSize));
            }

            ChromatogramIndex.totalCount = ChromatogramIndex.ids.Count;
            ChromatogramIndex.endPtr = StartPosition;
        }

        /**
         * 用于解析.d文件中的AcqMethod.XML文件
         */
        public void readMRMCompounds()
        {
            if (JobInfo.format.Equals(FileFormat.D) && JobInfo.type.Equals(AcquisitionMethod.MRM))
            {
                MrmCompoundDict = new AcqMethodParser(JobInfo.inputPath).parse();
            }
        }

        protected AirdInfo buildAirdInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            List<Software> softwares = new List<Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();

            //Basic Job Info
            airdInfo.scene = JobInfo.config.scene;
            airdInfo.airdPath = JobInfo.airdFilePath;
            airdInfo.fileSize = FileSize;
            airdInfo.createDate = DateTime.Now.ToString();
            airdInfo.type = JobInfo.type;
            airdInfo.totalCount = Msd.run.spectrumList.size();
            airdInfo.creator = JobInfo.config.creator;

            HashSet<string> activators = new HashSet<string>();
            HashSet<float> energies = new HashSet<float>();
            HashSet<string> polarities = new HashSet<string>();
            HashSet<string> msTypes = new HashSet<string>();
            HashSet<string> filterStrings = new HashSet<string>();

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
                for (var i = 0; i < IndexList.Count; i++)
                {
                    IndexList[i].activators = null;
                }
            }

            if (energies.Count == 1)
            {
                airdInfo.energy = IndexList[0].energies[0];
                for (var i = 0; i < IndexList.Count; i++)
                {
                    IndexList[i].energies = null;
                }
            }

            if (polarities.Count == 1)
            {
                airdInfo.polarity = IndexList[0].polarities[0];
                for (var i = 0; i < IndexList.Count; i++)
                {
                    IndexList[i].polarities = null;
                }
            }

            if (msTypes.Count == 1)
            {
                airdInfo.msType = IndexList[0].msTypes[0];
                for (var i = 0; i < IndexList.Count; i++)
                {
                    IndexList[i].msTypes = null;
                }
            }

            if (filterStrings.Count == 1)
            {
                airdInfo.filterString = IndexList[0].filterStrings[0];
                for (var i = 0; i < IndexList.Count; i++)
                {
                    IndexList[i].filterStrings = null;
                }
            }

            // airdInfo.rtUnit = rtUnit;
            airdInfo.mobiInfo = MobiInfo;
            //Scan index and window range info
            airdInfo.rangeList = Ranges;

            //Block index
            airdInfo.indexList = IndexList;

            //ChromatogramIndex
            airdInfo.chromatogramIndex = ChromatogramIndex;

            //Instrument Info
            List<Instrument> instruments = new List<Instrument>();
            foreach (InstrumentConfiguration ic in Msd.instrumentConfigurationList)
            {
                Instrument instrument = new Instrument();
                //仪器设备信息
                if (JobInfo.format.Equals(FileFormat.WIFF) || JobInfo.format.Equals(FileFormat.WIFF2))
                {
                    instrument.manufacturer = Manufacturer.SCIEX;
                }

                if (JobInfo.format.Equals(FileFormat.RAW))
                {
                    instrument.manufacturer = Manufacturer.Thermo;
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
                    }
                }

                instruments.Add(instrument);
                ic.Dispose();
            }

            airdInfo.instruments = instruments;
            airdInfo.startTimeStamp = Msd.run.startTimeStamp;
            //Software Info
            foreach (pwiz.CLI.msdata.Software soft in Msd.softwareList)
            {
                Software software = new Software();
                software.name = soft.id;
                software.version = soft.version;
                softwares.Add(software);
                soft.Dispose();
            }

            Software airdPro = new Software();
            airdPro.name = SoftwareInfo.NAME;
            airdPro.version = SoftwareInfo.VERSION;
            airdPro.type = "DataFormatConversion";
            softwares.Add(airdPro);
            airdInfo.softwares = softwares;

            //Parent Files Info
            foreach (SourceFile sf in Msd.fileDescription.sourceFiles)
            {
                ParentFile file = new ParentFile();
                file.name = sf.name;
                file.location = sf.location;
                file.formatType = sf.id;
                parentFiles.Add(file);
            }

            airdInfo.parentFiles = parentFiles;

            //Compressor Info
            List<Compressor> comps = new List<Compressor>();
            Compressor mzCompressor = new Compressor(AirdSDK.Beans.Compressor.TARGET_MZ);
            Compressor intCompressor = new Compressor(AirdSDK.Beans.Compressor.TARGET_INTENSITY);
            Compressor mobiCompressor = new Compressor(AirdSDK.Beans.Compressor.TARGET_MOBILITY);

            mzCompressor.addMethod(JobInfo.config.mzIntComp.ToString());
            mzCompressor.addMethod(JobInfo.config.mzByteComp.ToString());
            mzCompressor.precision = JobInfo.config.mzPrecision;

            intCompressor.addMethod(JobInfo.config.intIntComp.ToString());
            intCompressor.addMethod(JobInfo.config.intByteComp.ToString());
            intCompressor.precision = IntensityPrecision;

            mobiCompressor.addMethod(JobInfo.config.mobiIntComp.ToString());
            mobiCompressor.addMethod(JobInfo.config.mobiByteComp.ToString());
            mobiCompressor.precision = MobiPrecision;

            comps.Add(mzCompressor);
            comps.Add(intCompressor);
            comps.Add(mobiCompressor);
            airdInfo.compressors = comps;

            airdInfo.ignoreZeroIntensityPoint = JobInfo.config.ignoreZeroIntensity;
            //Features Info
            FeaturesMap.Add(Features.raw_id, Msd.id);
            FeaturesMap.Add(Features.ignore_zero_intensity, JobInfo.config.ignoreZeroIntensity);
            FeaturesMap.Add(Features.source_file_format, JobInfo.format);
            FeaturesMap.Add(Features.byte_order, ByteOrder.LITTLE_ENDIAN);
            FeaturesMap.Add(Features.aird_algorithm, JobInfo.getCompressorStr());
            airdInfo.features = FeaturesUtil.toString(FeaturesMap);
            airdInfo.version = SoftwareInfo.VERSION;
            return airdInfo;
        }

        protected ColumnInfo buildColumnInfo()
        {
            ColumnInfo columnInfo = new ColumnInfo();
            columnInfo.type = JobInfo.type;
            columnInfo.indexList = ColumnIndexList;
            columnInfo.mzPrecision = JobInfo.config.mzPrecision;
            columnInfo.intPrecision = IntensityPrecision;
            columnInfo.airdPath = JobInfo.airdFilePath;
            return columnInfo;
        }

        List<ByteComp> byteCompList = new()
        {
            new BrotliWrapper(), new SnappyWrapper(), new ZstdWrapper(), new ZlibWrapper()
        };

        List<SortedIntComp> integratedIntCompList = new()
        {
            new DeltaWrapper(), new IntegratedBinPackingWrapper(),
        };

        List<IntComp> intCompList = new()
        {
            new VarByteWrapper(), new BinPackingWrapper(), new Empty()
        };

        public Combination randomSampling(int randomNum, bool ionMobi)
        {
            List<int[]> mzArrays = new List<int[]>();
            List<int[]> intensityArrays = new List<int[]>();
            List<int[]> mobiNoArrays = new List<int[]>();
            Random rn = new Random();
            List<int> logIndexes = new List<int>();
            for (var i = 0; i < randomNum; i++)
            {
                int index = rn.Next(0, TotalSpectraCount);
                logIndexes.Add(index);
                List<int[]> dataList = fetchSpectrum(index, ionMobi);
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

            return compressForTargetArrays(mzArrays, intensityArrays, mobiNoArrays, ionMobi);
        }

        public List<int[]> fetchSpectrum(int index, bool mobi)
        {
            List<int[]> arrays = new List<int[]>();
            Spectrum spectrum = SpectrumList.spectrum(index, true);
            double[] mzData = spectrum.getMZArray().data.Storage();
            double[] intData = spectrum.getIntensityArray().data.Storage();

            var size = mzData.Length;
            int[] mzArray = new int[size];
            int[] intensityArray = new int[size];
            int[] mobilityNoArray = new int[size];

            if (mobi)
            {
                double[] mobiData = DataUtil.getMobilityData(spectrum);
                TimsData[] dataArray = new TimsData[size];
                for (int t = 0; t < size; t++)
                {
                    dataArray[t] = new TimsData(MobiDict[mobiData[t]], mzData[t], intData[t]);
                }

                Array.Sort(dataArray, (p1, p2) => p1.mz.CompareTo(p2.mz));
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(dataArray[i].mz * JobInfo.config.mzPrecision);
                    intensityArray[i] = DataUtil.fetchIntensity(dataArray[i].intensity, IntensityPrecision);
                    mobilityNoArray[i] = dataArray[i].mobilityNo;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(mzData[i] * JobInfo.config.mzPrecision);
                    intensityArray[i] = DataUtil.fetchIntensity(intData[i], IntensityPrecision);
                }
            }

            arrays.Add(mzArray);
            arrays.Add(intensityArray);
            arrays.Add(mobilityNoArray);

            spectrum.Dispose();
            return arrays;
        }

        public Combination compressForTargetArrays(List<int[]> mzArrays, List<int[]> intensityArrays,
            List<int[]> mobiNoArrays, bool ionMobi)
        {
            Dictionary<string, long> ctMap = new Dictionary<string, long>();
            Dictionary<string, long> dtMap = new Dictionary<string, long>();
            Dictionary<string, long> sizeMap = new Dictionary<string, long>();

            foreach (SortedIntComp intComp in integratedIntCompList)
            {
                foreach (ByteComp byteComp in byteCompList)
                {
                    string key = buildComboKey(Tag.Key_MZ, intComp.getName(), byteComp.getName());
                    ctMap.Add(key, 0);
                    dtMap.Add(key, 0);
                    sizeMap.Add(key, 0);
                    StatUtil.stat4ComboComp(intComp, byteComp, mzArrays, key, sizeMap, ctMap, dtMap);
                }
            }

            foreach (IntComp intComp in intCompList)
            {
                foreach (ByteComp byteComp in byteCompList)
                {
                    string key = buildComboKey(Tag.Key_Intensity, intComp.getName(), byteComp.getName());
                    ctMap.Add(key, 0);
                    dtMap.Add(key, 0);
                    sizeMap.Add(key, 0);
                    StatUtil.stat4ComboComp(intComp, byteComp, intensityArrays, key, sizeMap, ctMap, dtMap);
                }
            }

            if (ionMobi)
            {
                foreach (IntComp intComp in intCompList)
                {
                    foreach (ByteComp byteComp in byteCompList)
                    {
                        string key = buildComboKey(Tag.Key_Mobi, intComp.getName(), byteComp.getName());
                        ctMap.Add(key, 0);
                        dtMap.Add(key, 0);
                        sizeMap.Add(key, 0);
                        StatUtil.stat4ComboComp(intComp, byteComp, mobiNoArrays, key, sizeMap, ctMap, dtMap);
                    }
                }
            }

            List<CompressStat> mzStatList = new List<CompressStat>();
            List<CompressStat> intensityStatList = new List<CompressStat>();
            List<CompressStat> mobiStatList = new List<CompressStat>();
            foreach (KeyValuePair<string, long> pair in sizeMap)
            {
                string key = pair.Key;
                CompressStat stat = new CompressStat(key, sizeMap[key], ctMap[key], dtMap[key]);
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
            int bestIndex4Mz = StatUtil.calcBestIndex(mzStatList, csWeight, ctWeight, dtWeight);
            int bestIndex4Intensity = StatUtil.calcBestIndex(intensityStatList, csWeight, ctWeight, dtWeight);
            Combination bestCombination = null;
            if (ionMobi)
            {
                int bestIndex4Mobi = StatUtil.calcBestIndex(mobiStatList, csWeight, ctWeight, dtWeight);
                JobInfo.log(Tag.Best_Combo_Comp + mzStatList[bestIndex4Mz].key + Const.Left_Slash +
                            intensityStatList[bestIndex4Intensity].key + Const.Left_Slash +
                            mobiStatList[bestIndex4Mobi].key);
                bestCombination = new Combination(mzStatList[bestIndex4Mz].key,
                    intensityStatList[bestIndex4Intensity].key,
                    mobiStatList[bestIndex4Mobi].key);
            }
            else
            {
                JobInfo.log(Tag.Best_Combo_Comp + mzStatList[bestIndex4Mz].key + Const.Left_Slash +
                            intensityStatList[bestIndex4Intensity].key);
                bestCombination = new Combination(mzStatList[bestIndex4Mz].key,
                    intensityStatList[bestIndex4Intensity].key);
            }

            return bestCombination;
        }

        public string buildComboKey(string key, string intCompName, string byteCompName)
        {
            return key + Const.Dash + intCompName + Const.Dash + byteCompName;
        }

        public void pretreatmentDDA()
        {
            int parentNum = 0;
            JobInfo.log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            for (var i = 0; i < TotalSpectraCount; i++)
            {
                using (Spectrum spectrum = SpectrumList.spectrum(i, false))
                {
                    string msLevel = CVUtil.parseMsLevel(spectrum);
                    JobInfo.setStatus("Pre:" + i + "/" + TotalSpectraCount);
                    //最后一个谱图,单独判断
                    if (i == TotalSpectraCount - 1)
                    {
                        if (msLevel.Equals(MsLevel.MS1))
                        {
                            Ms1List.Add(parseMS1(spectrum, i)); //如果是MS1谱图,加入到MS1List
                        }

                        if (msLevel.Equals(MsLevel.MS2))
                        {
                            MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                            addToMS2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
                        }
                    }
                    else
                    {
                        //如果这个谱图是MS1
                        if (msLevel.Equals(MsLevel.MS1))
                        {
                            Ms1List.Add(parseMS1(spectrum, i)); //加入MS1List
                            using (Spectrum next = SpectrumList.spectrum(i + 1))
                            {
                                if (CVUtil.parseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                                {
                                    parentNum = i;
                                }
                            }
                        }

                        if (msLevel.Equals(MsLevel.MS2))
                        {
                            MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                            addToMS2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
                        }
                    }
                }
            }

            JobInfo.log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentDIA()
        {
            int parentNum = 0;
            JobInfo.log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < TotalSpectraCount; i++)
            {
                progress++;
                JobInfo.log(null, Tag.progress(Tag.Pre, progress, TotalSpectraCount));
                using (Spectrum spectrum = SpectrumList.spectrum(i))
                {
                    string msLevel = CVUtil.parseMsLevel(spectrum);
                    //如果这个谱图是MS1                          
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        parentNum = i;
                        Ms1List.Add(parseMS1(spectrum, i));
                    }

                    //如果这个谱图是MS2
                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                        //边扫描边建立SWATH WindowRange
                        if (!RangeTable.Contains(ms2Index.precursor.mz))
                        {
                            WindowRange range = ms2Index.precursor;
                            Ranges.Add(range);
                            RangeTable.Add(range.mz, range);
                        }

                        //DIA的MS2Map以precursorMz为key
                        addToMS2Map(ms2Index.precursor.mz, ms2Index);
                    }
                }
            }

            JobInfo.log(Tag.Total_SWATH_WINDOWS + Ranges.Count);
            JobInfo.log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentDDAPasef()
        {
            int parentNum = 0;
            JobInfo.log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            for (var i = 0; i < TotalSpectraCount; i++)
            {
                JobInfo.log(null, Tag.progress(Tag.Pre, i, TotalSpectraCount));
                using (Spectrum spectrum = SpectrumList.spectrum(i))
                {
                    string msLevel = CVUtil.parseMsLevel(spectrum);
                    //最后一个谱图,单独判断
                    if (i == TotalSpectraCount - 1)
                    {
                        if (msLevel.Equals(MsLevel.MS1))
                        {
                            Ms1List.Add(parseMS1(spectrum, i)); //如果是MS1谱图,加入到MS1List
                        }

                        if (msLevel.Equals(MsLevel.MS2))
                        {
                            MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                            addToMS2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
                        }
                    }
                    else
                    {
                        //如果这个谱图是MS1
                        if (msLevel.Equals(MsLevel.MS1))
                        {
                            Ms1List.Add(parseMS1(spectrum, i)); //加入MS1List
                            using (Spectrum next = SpectrumList.spectrum(i + 1))
                            {
                                if (CVUtil.parseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                                {
                                    parentNum = i;
                                }
                            }
                        }

                        if (msLevel.Equals(MsLevel.MS2))
                        {
                            MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                            addToMS2Map(ms2Index.pNum, ms2Index); //如果这个谱图是MS2
                        }
                    }
                }
            }

            JobInfo.log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentDIAPasef()
        {
            int parentNum = 0;
            JobInfo.log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < TotalSpectraCount; i++)
            {
                progress++;
                JobInfo.log(null, Tag.progress(Tag.Pre, progress, TotalSpectraCount));
                using (Spectrum spectrum = SpectrumList.spectrum(i))
                {
                    string msLevel = CVUtil.parseMsLevel(spectrum);
                    //如果这个谱图是MS1                          
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        parentNum = i;
                        Ms1List.Add(parseMS1(spectrum, i));
                    }

                    //如果这个谱图是MS2
                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                        //边扫描边建立SWATH WindowRange
                        if (!RangeTable.Contains(ms2Index.precursor.mz))
                        {
                            WindowRange range = ms2Index.precursor;
                            Ranges.Add(range);
                            RangeTable.Add(range.mz, range);
                        }

                        //DIA的MS2Map以precursorMz为key
                        addToMS2Map(ms2Index.precursor.mz, ms2Index);
                    }
                }
            }

            JobInfo.log(Tag.Total_SWATH_WINDOWS + Ranges.Count);
            JobInfo.log(Tag.Effective_MS1_List_Size + Ms1List.Count);
            JobInfo.log(Tag.MS2_Group_List_Size + Ms2Table.Count);
            JobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentPRM()
        {
            int parentNum = 0;
            JobInfo.log(Status.tag_preprocessing + TotalSpectraCount, Status.Preprocessing);
            for (int i = 0; i < TotalSpectraCount; i++)
            {
                JobInfo.log(null, Tag.progress(Tag.Empty, (i + 1), TotalSpectraCount));
                using (Spectrum spectrum = SpectrumList.spectrum(i))
                {
                    string msLevel = CVUtil.parseMsLevel(spectrum);
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
                            MsIndex ms2Index = parseMS2(SpectrumList.spectrum(i), i, parentNum);
                            addToMS2Map(ms2Index.precursor.mz, ms2Index);
                            continue;
                        }
                    }

                    //如果这个谱图是MS1
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        using (Spectrum next = SpectrumList.spectrum(i + 1))
                        {
                            string msLevelNext = CVUtil.parseMsLevel(next);
                            //如果下一个谱图仍然是MS1, 那么直接忽略这个谱图
                            if (msLevelNext.Equals(MsLevel.MS1))
                            {
                                continue;
                            }

                            if (msLevelNext.Equals(MsLevel.MS2))
                            {
                                parentNum = i;
                                Ms1List.Add(parseMS1(SpectrumList.spectrum(i), i));
                            }
                        }
                    }

                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        using (var current = SpectrumList.spectrum(i))
                        {
                            MsIndex ms2Index = parseMS2(current, i, parentNum);
                            addToMS2Map(ms2Index.precursor.mz, ms2Index); //如果这个谱图是MS2
                        }
                    }
                }
            }

            JobInfo.log("Effective MS1 List Size:" + Ms1List.Count);
            JobInfo.log("MS2 Group List Size:" + Ms2Table.Count);
            JobInfo.log("Start Processing MS1 List");
        }
    }
}