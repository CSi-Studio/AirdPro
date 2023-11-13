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
using System.Diagnostics;
using System.IO;
using System.Text;
using AirdPro.Algorithms;
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
    public class Converter
    {
        protected MSData msd;
        public SpectrumList spectrumList;
        public ChromatogramList chromatogramList;

        public JobInfo jobInfo;
        protected Stopwatch stopwatch = new Stopwatch();
        public FileStream airdStream;
        public FileStream airdJsonStream;
        public FileStream airdColumnJsonStream;
        protected List<WindowRange> ranges = new List<WindowRange>(); //SWATH/DIA Window的窗口
        protected Hashtable rangeTable = new Hashtable(); //用于存放SWATH/DIA窗口的信息,key为mz
        protected List<BlockIndex> indexList = new List<BlockIndex>(); //用于存储的全局的SWATH List
        protected List<ColumnIndex> columnIndexList = new List<ColumnIndex>(); //列存储索引，尽在面向Search的场景下有效

        protected Hashtable
            ms2Table = Hashtable.Synchronized(new Hashtable()); //用于存放MS2的索引信息,DDA采集模式下key为ms1的num, DIA采集模式下key为mz

        public List<MsIndex> ms1List = new List<MsIndex>(); //用于存放MS1索引及基础信息,泛型为MsIndex
        protected Hashtable featuresMap = new Hashtable();
        public ICompressor compressor;

        public double[] mobiArray;
        public Dictionary<double, int> mobiDict;
        public MobiInfo mobiInfo = new MobiInfo();

        protected long fileSize; //厂商文件大小
        public long startPosition = 0; //文件指针
        protected int totalSize = 0; //总计的谱图数目
        protected int totalChroma = 0; //总计的色谱数目

        protected int intensityPrecision = 1; //Intensity默认精确到个位数
        protected int mobiPrecision = 10000000; //mobility默认精确到小数点后7位

        protected int spectraNumForIntensityPrecisionPredict = 5; //用于ComboComp预测Intensity精度时的采样光谱数

        public ChromatogramIndex chromatogramIndex;

        public Converter(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            initCompressor();
        }

        public void doConvert()
        {
            try
            {
                start();
                MSDataList msdList = readVendorFile(); //准备读取Vendor文件
                if (msdList.Count == 0 || msdList == null)
                {
                    return;
                }

                for (int i = 0; i < msdList.Count; i++)
                {
                    startPosition = 0;
                    if (msdList.Count > 1) //如果msdList中包含多个msd，那么每一个msd会被单独导出为一个文件，导出的文件名按照msd的ID进行命名
                    {
                        String id = msdList[i].id;
                        jobInfo.airdFilePath = Path.Combine(jobInfo.outputPath, id+".aird");
                        jobInfo.airdJsonFilePath = Path.Combine(jobInfo.outputPath, id+".json");
                        //     AirdProFileUtil.replaceLast(jobInfo.airdFilePath, jobInfo.airdFileName, id);
                        // jobInfo.airdJsonFilePath =
                        //     AirdProFileUtil.replaceLast(jobInfo.airdJsonFilePath, jobInfo.airdFileName, id);
                        jobInfo.airdFileName = id;
                    }

                    readMsd(msdList[i]);
                    initDirectory(); //创建文件夹
                    using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
                    {
                        using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                        {
                            predictAcquisitionMethod();
                            switch (jobInfo.type)
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
                                    jobInfo.ionMobility = true;
                                    ConverterWorkFlow.DDAPasef(this);
                                    break;
                                case AcquisitionMethod.DIA_PASEF:
                                    jobInfo.ionMobility = true;
                                    ConverterWorkFlow.DIAPasef(this);
                                    break;
                            }
                        }
                    }

                    if (msd != null)
                    {
                        msd.Dispose();
                    }
                    clearCache();
                }
                if (msdList != null)
                {
                    msdList.Dispose();
                }
            }
            finally
            {
                finish();
            }
        }

        public void start()
        {
            stopwatch.Start();
            jobInfo.log(Tag.Ready_To_Start, Status.Starting);
            AppLogs.WriteInfo(Tag.BaseInfo + jobInfo.getJsonInfo(), true);
        }

        public void finish()
        {
            stopwatch.Stop();
            jobInfo.refreshReport = true;
            jobInfo.log(Tag.Total_Time_Cost + stopwatch.Elapsed.TotalSeconds, Status.Finished);
            clearCache();
            jobInfo.setStatus(ProcessingStatus.FINISHED);
            if (msd != null)
            {
                msd.Dispose();
                msd = null;
            }
        }

        public void initCompressor()
        {
            ICompressor comp = jobInfo.config.stack ? new StackComp(this) : new CoreComp(this);
            //探索模式和非自动决策模式,会在此处初始化指定的压缩内核
            if (!jobInfo.config.autoDesicion)
            {
                if (jobInfo.ionMobility)
                {
                    comp.mobiIntComp = IntComp.build(jobInfo.config.mobiIntComp);
                    comp.mobiByteComp = ByteComp.build(jobInfo.config.mobiByteComp);
                }

                comp.mzIntComp = SortedIntComp.build(jobInfo.config.mzIntComp);
                comp.mzByteComp = ByteComp.build(jobInfo.config.mzByteComp);

                comp.intIntComp = IntComp.build(jobInfo.config.intIntComp);
                comp.intByteComp = ByteComp.build(jobInfo.config.intByteComp);
            }

            this.compressor = comp;
        }

        public void initDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdFilePath));
            Directory.CreateDirectory(Path.GetDirectoryName(jobInfo.airdJsonFilePath));
        }

        public void initBrukerMobi()
        {
            jobInfo.log(Tag.Init_Mobility_Array);
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

        /**
         * 用于检测当前文件的采集模式
         * 如果含有Mobility属性,则为PASEF模式
         * 如果前三帧都是MS1,则必然不是DIA
         * 如果MS2的precursor范围大于3,则可能是DIA
         * 如果MS2的precursor范围小于1,则可能是DDA
         */
        public void predictAcquisitionMethod()
        {
            if (!jobInfo.type.Equals(JobInfo.AutoType))
            {
                jobInfo.setType(jobInfo.type);
                return;
            }

            bool mobi = false;
            jobInfo.log(Tag.Predict_Acquisition_Method, Status.Init);

            //如果有光谱图
            if (spectrumList != null && spectrumList.size() > 0)
            {
                Spectrum firstSpec = spectrumList.spectrum(0, true);
                List<Spectrum> predictSpecList = new List<Spectrum>();
                //首先取10个窗口
                for (int i = 0; i < 10; i++)
                {
                    predictSpecList.Add(spectrumList.spectrum(i, true));
                }

                //首先判断是不是带有离子淌度的ion mobility模式
                if (firstSpec.binaryDataArrays.Count == 3)
                {
                    foreach (BinaryDataArray dataArray in firstSpec.binaryDataArrays)
                    {
                        if (dataArray.cvParams[0].cvid.Equals(CVID.MS_mean_inverse_reduced_ion_mobility_array))
                        {
                            jobInfo.ionMobility = true;
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
                        double width = CVUtil.parsePrecursorWidth(spectrum.precursors[0].isolationWindow, jobInfo);
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
                    jobInfo.setType(AcquisitionMethod.DDA_PASEF);
                }

                if (isDDA && !mobi)
                {
                    jobInfo.setType(AcquisitionMethod.DDA);
                }

                if (isDIA && mobi)
                {
                    jobInfo.setType(AcquisitionMethod.DIA_PASEF);
                }

                if (isDIA && !mobi)
                {
                    jobInfo.setType(AcquisitionMethod.DIA);
                }
            }

            try
            {
                //如果有色谱图,且谱图数目大于2(排除TIC和BPC图),则预测为SRM模式
                if (chromatogramList != null && chromatogramList.size() > 10)
                {
                    List<Chromatogram> predictChromatoList = new List<Chromatogram>();
                    // 首先取10个窗口
                    for (int i = 0; i < 10; i++)
                    {
                        using (Chromatogram chroma = chromatogramList.chromatogram(i, false))
                        {
                            predictChromatoList.Add(chroma);
                        }
                    }

                    jobInfo.setType(AcquisitionMethod.MRM);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Hello");
            }
        }

        public void predictForBestCombination()
        {
            if (!jobInfo.config.autoDesicion)
            {
                return;
            }

            jobInfo.log(Tag.Predict_For_Best_Combination + jobInfo.airdFileName, Status.Predicting);
            Combination combination = randomSampling(jobInfo.config.spectraToPredict, jobInfo.ionMobility);
            combination.enable(jobInfo.config, compressor);
            jobInfo.log(jobInfo.getCompressorStr());
            jobInfo.config.autoDesicion = false;
            jobInfo.setCombination(jobInfo.getCompressorStr());
        }

        /**
         * num:采样数目,建议:5
         */
        public void predictForIntensityPrecision()
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

            intensityPrecision = findIt ? 10 : 1;
            if (findIt)
            {
                intensityPrecision = 10;
            }
            else
            {
                intensityPrecision = 1;
            }

            compressor.intensityPrecision = intensityPrecision;
            jobInfo.log(Tag.Intensity_Precision + intensityPrecision);
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
            columnIndex.startMzListPtr = startPosition;
            startPosition += compressedMzs.Length;
            columnIndex.endMzListPtr = startPosition;
            airdStream.Write(compressedMzs, 0, compressedMzs.Length);

            //写入矩阵的纵坐标实际值
            columnIndex.startRtListPtr = startPosition;
            startPosition += compressedRts.Length;
            columnIndex.endRtListPtr = startPosition;
            airdStream.Write(compressedRts, 0, compressedRts.Length);

            columnIndex.spectraIds = new int[columnIndex.mzs.Length];
            columnIndex.intensities = new int[columnIndex.mzs.Length];
            columnIndex.startPtr = startPosition;
            for (var i = 0; i < columnIndex.mzs.Length; i++)
            {
                int mz = columnIndex.mzs[i];
                ByteColumn byteColumn = compressedColumns[mz];
                if (byteColumn.indexIds != null && byteColumn.intensities != null)
                {
                    columnIndex.spectraIds[i] = byteColumn.indexIds.Length;
                    columnIndex.intensities[i] = byteColumn.intensities.Length;
                    startPosition = startPosition + byteColumn.indexIds.Length + byteColumn.intensities.Length;
                    airdStream.Write(byteColumn.indexIds, 0, byteColumn.indexIds.Length);
                    airdStream.Write(byteColumn.intensities, 0, byteColumn.intensities.Length);
                }
                else
                {
                    columnIndex.spectraIds[i] = 0;
                    columnIndex.intensities[i] = 0;
                }
            }

            columnIndex.endPtr = startPosition;

            byte[] compressedSpectraIds =
                new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new VarByteWrapper().encode(columnIndex.spectraIds)));
            byte[] compressedInts =
                new ZstdWrapper().encode(
                    ByteTrans.intToByte(
                        new VarByteWrapper().encode(columnIndex.intensities)));
            //写入矩阵的横坐标实际值
            columnIndex.startSpecrtaIdListPtr = startPosition;
            startPosition += compressedSpectraIds.Length;
            columnIndex.endSpecrtaIdListPtr = startPosition;
            airdStream.Write(compressedSpectraIds, 0, compressedSpectraIds.Length);

            //写入矩阵的横坐标实际值
            columnIndex.startIntensityListPtr = startPosition;
            startPosition += compressedInts.Length;
            columnIndex.endIntensityListPtr = startPosition;
            airdStream.Write(compressedInts, 0, compressedInts.Length);

            columnIndex.mzs = null;
            columnIndex.rts = null;
            columnIndex.spectraIds = null;
            columnIndex.intensities = null;

            columnIndexList.Add(columnIndex);
        }

        //注意:本函数会操作startPosition这个全局变量
        public void addToIndex(BlockIndex index, object tempScan)
        {
            if (jobInfo.config.stack)
            {
                TempScanSZDPD ts = (TempScanSZDPD)tempScan;

                index.nums.AddRange(ts.nums);
                index.rts.AddRange(ts.rts);
                index.tics.AddRange(ts.tics);
                index.basePeakIntensities.AddRange(ts.basePeakIntensities);
                index.basePeakMzs.AddRange(ts.basePeakMzs);
                index.injectionTimes.AddRange(ts.injectionTimes);

                index.polarities.AddRange(ts.polarities);
                index.energies.AddRange(ts.energies);
                index.activators.AddRange(ts.activators);
                index.filterStrings.AddRange(ts.filterStrings);
                index.msTypes.AddRange(ts.msTypes);
                index.cvList.AddRange(ts.cvs);

                if (ts.mzArrayBytes.Length != 0 && ts.intArrayBytes.Length != 0 && ts.tagArrayBytes.Length != 0) 
                {
                    index.mzs.Add(ts.mzArrayBytes.Length);
                    index.ints.Add(ts.intArrayBytes.Length);
                    index.tags.Add(ts.tagArrayBytes.Length);
                    startPosition = startPosition + ts.mzArrayBytes.Length + ts.tagArrayBytes.Length +
                                    ts.intArrayBytes.Length;
                    airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                    airdStream.Write(ts.tagArrayBytes, 0, ts.tagArrayBytes.Length);
                    airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
                }
            }
            else
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
                    startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                    airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                    airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
                }

                if (ts.mobilityArrayBytes != null)
                {
                    index.mobilities.Add(ts.mobilityArrayBytes.Length);
                    startPosition += ts.mobilityArrayBytes.Length;
                    airdStream.Write(ts.mobilityArrayBytes, 0, ts.mobilityArrayBytes.Length);
                }
            }
        }

        protected MSDataList readVendorFile()
        {
            jobInfo.log(Tag.Prepare_To_Parse_Vendor_File, Status.Prepare);
            ReaderList readerList = ReaderList.FullReaderList;
            var readerConfig = new ReaderConfig
            {
                allowMsMsWithoutPrecursor = false,
                combineIonMobilitySpectra = true,
                ignoreZeroIntensityPoints = jobInfo.config.ignoreZeroIntensity
            };

            MSDataList msInfo = new MSDataList();
            readerList.read(jobInfo.inputPath, msInfo, readerConfig);
            if (msInfo.Count == 0)
            {
                jobInfo.logError(ResultCode.Reading_Vendor_File_Error_Run_Is_Null);
                return null;
            }

            jobInfo.log(Tag.Adapting_Vendor_File_API, Status.Adapting);

            switch (jobInfo.format)
            {
                case FileFormat.WIFF:
                case FileFormat.WIFF2:
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
                case FileFormat.mzML:
                    FileInfo mzML = new FileInfo(jobInfo.inputPath);
                    if (mzML.Exists) fileSize += mzML.Length;
                    break;
                case FileFormat.mzXML:
                    FileInfo mzXML = new FileInfo(jobInfo.inputPath);
                    if (mzXML.Exists) fileSize += mzXML.Length;
                    break;
                case FileFormat.D:
                    long totalSize = AirdProFileUtil.getDirectorySize(jobInfo.inputPath);
                    fileSize += totalSize;
                    break;
                default:
                    FileInfo file = new FileInfo(jobInfo.inputPath);
                    if (file.Exists) fileSize += file.Length;
                    break;
            }

            return msInfo;
        }

        public void readMsd(MSData msd)
        {
            this.msd = msd;
            List<string> filter = new List<string>();
            SpectrumListFactory.wrap(msd, filter); //这一步操作可以帮助加快Wiff文件的初始化速度

            spectrumList = msd.run.spectrumList;
            if (spectrumList == null || spectrumList.empty())
            {
                jobInfo.log(ResultCode.No_Spectra_Found);
            }
            else
            {
                totalSize = spectrumList.size();
            }

            chromatogramList = msd.run.chromatogramList;
            if (chromatogramList == null || chromatogramList.empty())
            {
                jobInfo.log(ResultCode.No_Chromatograms_Found);
            }
            else
            {
                totalChroma = chromatogramList.size();
            }

            jobInfo.log(Tag.Adapting_Finished);
            jobInfo.log(Tag.Total_Spectra + totalSize);
            jobInfo.log(Tag.Total_Chromatograms + totalChroma);
        }

        //将最终的数据写入文件中
        public void writeToAirdInfoFile()
        {
            jobInfo.log(Tag.Write_Index_File, Status.Writing_Index_File);
            AirdInfo airdInfo = buildAirdInfo();

            if (jobInfo.config.compressedIndex)
            {
                List<BlockIndex> indexList = airdInfo.indexList;
                string indexListStr = JsonConvert.SerializeObject(indexList,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] indexListByte = new ZstdWrapper().encode(Encoding.Default.GetBytes(indexListStr));
                airdInfo.indexStartPtr = startPosition;
                startPosition += indexListByte.Length;
                airdInfo.indexEndPtr = startPosition;
                airdInfo.indexList = null;
                airdStream.Write(indexListByte, 0, indexListByte.Length);
            }

            string airdInfoStr = JsonConvert.SerializeObject(airdInfo,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            startPosition += airdBytes.Length;
            airdJsonStream.Write(airdBytes, 0, airdBytes.Length);

            if (jobInfo.config.isSearch())
            {
                ColumnInfo columnInfo = buildColumnInfo();
                string columnInfoStr = JsonConvert.SerializeObject(columnInfo,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] columnInfoBytes = Encoding.Default.GetBytes(columnInfoStr);
                using (airdColumnJsonStream = new FileStream(jobInfo.airdColumnJsonFilePath, FileMode.Create))
                {
                    airdColumnJsonStream.Write(columnInfoBytes, 0, columnInfoBytes.Length);
                }
            }
        }

        public void clearCache()
        {
            ranges = new();
            spectrumList = null;

            rangeTable = new();
            indexList = new();
            ms2Table = new();
            ms1List = new();
            featuresMap = new();
            mobiDict = new();
            mobiInfo = new();
            chromatogramIndex = new();
            spectrumList = null;
            chromatogramList = null;
            if (msd != null)
            {
                msd.Dispose();
                msd = null;
            }
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
            if (spectrum.scanList.scans.Count != 1)
            {
                return ms1;
            }

            Scan scan = spectrum.scanList.scans[0];
            ms1.cvs = CVUtil.trans(spectrum.cvParams);
            //将对应scan的cvParams也冗余到ms1上来
            if (scan.cvParams != null)
            {
                ms1.cvs.AddRange(CVUtil.trans(scan.cvParams));
            }

            ms1.filterString = CVUtil.parseFilterString(scan, jobInfo);
            ms1.rt = CVUtil.parseRT(scan, jobInfo);
            ms1.tic = CVUtil.parseTIC(spectrum);
            ms1.basePeakIntensity = CVUtil.parseBasePeakIntensity(spectrum);
            ms1.basePeakMz = CVUtil.parseBasePeakMz(spectrum);
            ms1.injectionTime = CVUtil.parseInjectionTime(scan);
            if (mobiInfo.unit == null || mobiInfo.type == null)
            {
                CVUtil.parseMobility(scan, mobiInfo);
            }

            ms1.msType = CVUtil.parseMsType(spectrum);
            ms1.polarity = CVUtil.parsePolarity(spectrum);
            ms1.activator = Activator.UNKNOWN;
            ms1.energy = -1;
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
                ms2.precursor = CVUtil.parseIsolationWindow(spectrum.precursors[0], jobInfo);
            }
            catch (Exception e)
            {
                jobInfo.log(ResultCode.Error).log(Tag.SpectrumIndex + spectrum.index)
                    .log(Tag.SpectrumId + spectrum.id)
                    .log(Tag.Key_MZ + spectrum.precursors[0].isolationWindow
                        .cvParamChild(CVID.MS_isolation_window_target_m_z).value)
                    .log(Tag.LowerOffset + spectrum.precursors[0].isolationWindow
                        .cvParamChild(CVID.MS_isolation_window_lower_offset).value)
                    .log(Tag.UpperOffset + spectrum.precursors[0].isolationWindow
                        .cvParamChild(CVID.MS_isolation_window_upper_offset).value);
                throw e;
            }

            if (spectrum.scanList.scans.Count != 1) return ms2;


            var result = CVUtil.parseActivator(spectrum.precursors[0].activation);
            ms2.activator = result.activator;
            ms2.energy = result.energy;
            ms2.msType = CVUtil.parseMsType(spectrum);
            ms2.polarity = CVUtil.parsePolarity(spectrum);
            Scan scan = spectrum.scanList.scans[0];
            if (mobiInfo.unit == null || mobiInfo.type == null)
            {
                CVUtil.parseMobility(scan, mobiInfo);
            }

            ms2.filterString = CVUtil.parseFilterString(scan, jobInfo);
            ms2.cvs = CVUtil.trans(spectrum.cvParams);
            if (scan.cvParams != null) ms2.cvs.AddRange(CVUtil.trans(scan.cvParams));
            ms2.rt = CVUtil.parseRT(scan, jobInfo);
            ms2.tic = CVUtil.parseTIC(spectrum);
            ms2.basePeakIntensity = CVUtil.parseBasePeakIntensity(spectrum);
            ms2.basePeakMz = CVUtil.parseBasePeakMz(spectrum);
            ms2.injectionTime = CVUtil.parseInjectionTime(scan);
            return ms2;
        }

        public void compressMS2BlockForPRM()
        {
            jobInfo.log("Start Processing MS2 List");
            int progress = 0;
            foreach (double key in ms2Table.Keys)
            {
                List<MsIndex> ms2List = ms2Table[key] as List<MsIndex>;
                WindowRange range = new WindowRange(ms2List[0].precursor.start, ms2List[0].precursor.end, key);

                BlockIndex index = new BlockIndex(); //为每一个key组创建一个SwathBlock
                index.level = 2;
                index.startPtr = startPosition;
                index.setWindowRange(range); //顺便创建一个WindowRanges,用以让Propro服务端快速获取全局的窗口数目和mz区间
                ranges.Add(range);

                jobInfo.log(null, Tag.progress(Tag.MS2, progress, ms2Table.Keys.Count));
                progress++;
                compressor.compressMS2(this, ms2List, index);
                index.endPtr = startPosition;
                indexList.Add(index);
                jobInfo.log("MS2 Group Finished:" + progress + "/" + ms2Table.Keys.Count);
            }
        }

        public void compressMobiDict()
        {
            int[] mobiIntArray = new int[mobiArray.Length];
            for (var i = 0; i < mobiArray.Length; i++)
            {
                mobiIntArray[i] = (int)Math.Round(mobiArray[i] * mobiPrecision);
            }

            byte[] compressedMobiData =
                new ZstdWrapper().encode(ByteTrans.intToByte(new IntegratedVarByteWrapper().encode(mobiIntArray)));
            mobiInfo.dictStart = startPosition;
            startPosition += compressedMobiData.Length;
            airdStream.Write(compressedMobiData, 0, compressedMobiData.Length);
            mobiInfo.dictEnd = startPosition;
        }

        public void compressMS1Block()
        {
            BlockIndex index = new BlockIndex();
            index.level = 1;
            index.startPtr = startPosition;
            compressor.compressMS1(this, index);
            index.endPtr = startPosition;
            indexList.Add(index);
        }

        public void compressMS2BlockForDIA()
        {
            jobInfo.log(Tag.Start_Processing_MS2_List);
            int progress = 0;
            foreach (double precursorMz in ms2Table.Keys)
            {
                List<MsIndex> ms2List = ms2Table[precursorMz] as List<MsIndex>;
                WindowRange range = rangeTable[precursorMz] as WindowRange;

                BlockIndex index = new BlockIndex(); //为每一个key组创建一个SwathBlock
                index.level = 2;
                index.startPtr = startPosition;
                index.setWindowRange(range);

                jobInfo.log(null, Tag.progress(Tag.MS2, progress, ms2Table.Keys.Count));
                progress++;
                compressor.compressMS2(this, ms2List, index);
                index.endPtr = startPosition;
                indexList.Add(index);
                jobInfo.log(Tag.progress(Tag.MS2_Group_Finished, progress, ms2Table.Keys.Count));
            }
        }

        //处理MS2,由于每一个MS1只跟随少量的MS2光谱图,因此DDA采集模式下MS2的压缩模式仍然使用Aird ZDPD的压缩算法
        public void compressMS2BlockForDDA()
        {
            int progress = 0;
            jobInfo.log(Tag.Start_Processing_MS2_List);
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
                jobInfo.log(null, Tag.progress(Tag.MS2, progress, ms2Table.Keys.Count));
                progress++;

                foreach (MsIndex index in tempIndexList)
                {
                    // WindowRange range = new WindowRange(index.mzStart, index.mzEnd, index.precursorMz);
                    WindowRange range = index.precursor;
                    ms2Ranges.Add(range);
                    TempScan ts = new TempScan(index);
                    if (jobInfo.ionMobility)
                    {
                        compressor.compressMobility(spectrumList.spectrum(index.num, true), ts);
                    }
                    else
                    {
                        compressor.compress(spectrumList.spectrum(index.num, true), ts);
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
                    startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                    airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                    airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
                    if (ts.mobilityArrayBytes != null)
                    {
                        blockIndex.mobilities.Add(ts.mobilityArrayBytes.Length);
                        startPosition += ts.mobilityArrayBytes.Length;
                        airdStream.Write(ts.mobilityArrayBytes, 0, ts.mobilityArrayBytes.Length);
                    }
                }

                blockIndex.rangeList = ms2Ranges;
                blockIndex.endPtr = startPosition;
                indexList.Add(blockIndex);
            }
        }

        public void compressChromatograms()
        {
            if (chromatogramList == null || chromatogramList.size() == 0)
            {
                return;
            }

            chromatogramIndex = new ChromatogramIndex();
            compressor.initForChromatogram();

            int totalSize = chromatogramList.size();
            int progress = 0;
            jobInfo.log(null, Tag.progress(Tag.Chroma, progress, totalSize));
            chromatogramIndex.startPtr = startPosition;
            for (int i = 0; i < chromatogramList.size(); i++)
            {
                Chromatogram chromatogram = chromatogramList.chromatogram(i, true);
                TempScanChroma tempScan = new TempScanChroma();
                chromatogramIndex.nums.Add(i);
                chromatogramIndex.ids.Add(chromatogram.id);
                chromatogramIndex.cvs.Add(CVUtil.trans(chromatogram.cvParams));

                var result = CVUtil.parseActivator(chromatogram.precursor.activation);
                chromatogramIndex.activators.Add(result.activator);
                chromatogramIndex.energies.Add(result.energy);
                chromatogramIndex.polarities.Add(CVUtil.parsePolarity(chromatogram));

                try
                {
                    chromatogramIndex.precursors.Add(CVUtil.parseIsolationWindow(chromatogram.precursor, jobInfo));
                    chromatogramIndex.products.Add(CVUtil.parseIsolationWindow(chromatogram.product.isolationWindow,
                        jobInfo));
                }
                catch (Exception e)
                {
                    jobInfo.log(ResultCode.Error).log(Tag.SpectrumIndex + i)
                        .log(Tag.SpectrumId + chromatogram.id)
                        .log(Tag.Key_MZ + chromatogram.precursor.isolationWindow
                            .cvParamChild(CVID.MS_isolation_window_target_m_z).value)
                        .log(Tag.LowerOffset + chromatogram.precursor.isolationWindow
                            .cvParamChild(CVID.MS_isolation_window_lower_offset).value)
                        .log(Tag.UpperOffset + chromatogram.precursor.isolationWindow
                            .cvParamChild(CVID.MS_isolation_window_upper_offset).value);
                    throw e;
                }

                compressor.compress(chromatogram, tempScan);
                chromatogramIndex.rts.Add(tempScan.rtArrayBytes.Length);
                chromatogramIndex.ints.Add(tempScan.intArrayBytes.Length);
                startPosition = startPosition + tempScan.rtArrayBytes.Length + tempScan.intArrayBytes.Length;
                airdStream.Write(tempScan.rtArrayBytes, 0, tempScan.rtArrayBytes.Length);
                airdStream.Write(tempScan.intArrayBytes, 0, tempScan.intArrayBytes.Length);

                progress++;
                jobInfo.log(null, Tag.progress(Tag.Chroma, progress, totalSize));
            }

            chromatogramIndex.totalCount = chromatogramIndex.ids.Count;
            chromatogramIndex.endPtr = startPosition;
        }

        protected AirdInfo buildAirdInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            List<Software> softwares = new List<Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();

            //Basic Job Info
            airdInfo.scene = jobInfo.config.scene;
            airdInfo.airdPath = jobInfo.airdFilePath;
            airdInfo.fileSize = fileSize;
            airdInfo.createDate = DateTime.Now.ToString();
            airdInfo.type = jobInfo.type;
            airdInfo.totalCount = msd.run.spectrumList.size();
            airdInfo.creator = jobInfo.config.creator;

            HashSet<string> activators = new HashSet<string>();
            HashSet<float> energies = new HashSet<float>();
            HashSet<string> polarities = new HashSet<string>();
            HashSet<string> msTypes = new HashSet<string>();
            HashSet<string> filterStrings = new HashSet<string>();

            for (var i = 0; i < indexList.Count; i++)
            {
                activators.UnionWith(indexList[i].activators);
                energies.UnionWith(indexList[i].energies);
                polarities.UnionWith(indexList[i].polarities);
                msTypes.UnionWith(indexList[i].msTypes);
                filterStrings.UnionWith(indexList[i].filterStrings);
            }

            if (activators.Count == 1)
            {
                airdInfo.activator = indexList[0].activators[0];
                for (var i = 0; i < indexList.Count; i++)
                {
                    indexList[i].activators = null;
                }
            }

            if (energies.Count == 1)
            {
                airdInfo.energy = indexList[0].energies[0];
                for (var i = 0; i < indexList.Count; i++)
                {
                    indexList[i].energies = null;
                }
            }

            if (polarities.Count == 1)
            {
                airdInfo.polarity = indexList[0].polarities[0];
                for (var i = 0; i < indexList.Count; i++)
                {
                    indexList[i].polarities = null;
                }
            }

            if (msTypes.Count == 1)
            {
                airdInfo.msType = indexList[0].msTypes[0];
                for (var i = 0; i < indexList.Count; i++)
                {
                    indexList[i].msTypes = null;
                }
            }

            if (filterStrings.Count == 1)
            {
                airdInfo.filterString = indexList[0].filterStrings[0];
                for (var i = 0; i < indexList.Count; i++)
                {
                    indexList[i].filterStrings = null;
                }
            }

            // airdInfo.rtUnit = rtUnit;
            airdInfo.mobiInfo = mobiInfo;
            //Scan index and window range info
            airdInfo.rangeList = ranges;

            //Block index
            airdInfo.indexList = indexList;

            //ChromatogramIndex
            airdInfo.chromatogramIndex = chromatogramIndex;

            //Instrument Info
            List<Instrument> instruments = new List<Instrument>();
            foreach (InstrumentConfiguration ic in msd.instrumentConfigurationList)
            {
                Instrument instrument = new Instrument();
                //仪器设备信息
                if (jobInfo.format.Equals(FileFormat.WIFF) || jobInfo.format.Equals(FileFormat.WIFF2))
                {
                    instrument.manufacturer = Manufacturer.SCIEX;
                }

                if (jobInfo.format.Equals(FileFormat.RAW))
                {
                    instrument.manufacturer = Manufacturer.Thermo;
                }

                //设备信息在不同的源文件格式中取法不同,有些是在instrumentConfigurationList中获取,有些是在paramGroups获取,因此出现了以下比较丑陋的写法
                if (ic.cvParams.Count != 0)
                {
                    foreach (CVParam cv in ic.cvParams)
                    {
                        if (!featuresMap.ContainsKey(cv.name))
                        {
                            featuresMap.Add(cv.name, cv.value);
                        }
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
            airdInfo.startTimeStamp = msd.run.startTimeStamp;
            //Software Info
            foreach (pwiz.CLI.msdata.Software soft in msd.softwareList)
            {
                Software software = new Software();
                software.name = soft.id;
                software.version = soft.version;
                softwares.Add(software);
            }

            Software airdPro = new Software();
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
                intCompressor.precision = intensityPrecision;

                mobiCompressor.addMethod(jobInfo.config.mobiIntComp.ToString());
                mobiCompressor.addMethod(jobInfo.config.mobiByteComp.ToString());
                mobiCompressor.precision = mobiPrecision;
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
            return airdInfo;
        }

        protected ColumnInfo buildColumnInfo()
        {
            ColumnInfo columnInfo = new ColumnInfo();
            columnInfo.type = jobInfo.type;
            columnInfo.indexList = columnIndexList;
            columnInfo.mzPrecision = jobInfo.config.mzPrecision;
            columnInfo.intPrecision = intensityPrecision;
            columnInfo.airdPath = jobInfo.airdFilePath;
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
                int index = rn.Next(0, totalSize);
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
            Spectrum spectrum = spectrumList.spectrum(index, true);
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
                    dataArray[t] = new TimsData(mobiDict[mobiData[t]], mzData[t], intData[t]);
                }

                Array.Sort(dataArray, (p1, p2) => p1.mz.CompareTo(p2.mz));
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(dataArray[i].mz * jobInfo.config.mzPrecision);
                    intensityArray[i] = DataUtil.fetchIntensity(dataArray[i].intensity, intensityPrecision);
                    mobilityNoArray[i] = dataArray[i].mobilityNo;
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    mzArray[i] = Convert.ToInt32(mzData[i] * jobInfo.config.mzPrecision);
                    intensityArray[i] = DataUtil.fetchIntensity(intData[i], intensityPrecision);
                }
            }

            arrays.Add(mzArray);
            arrays.Add(intensityArray);
            arrays.Add(mobilityNoArray);
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

            double csWeight = jobInfo.config.compressionSizeWeight;
            double ctWeight = jobInfo.config.compressionTimeWeight;
            double dtWeight = jobInfo.config.decompressionTimeWeight;
            int bestIndex4Mz = StatUtil.calcBestIndex(mzStatList, csWeight, ctWeight, dtWeight);
            int bestIndex4Intensity = StatUtil.calcBestIndex(intensityStatList, csWeight, ctWeight, dtWeight);
            Combination bestCombination = null;
            if (ionMobi)
            {
                int bestIndex4Mobi = StatUtil.calcBestIndex(mobiStatList, csWeight, ctWeight, dtWeight);
                jobInfo.log(Tag.Best_Combo_Comp + mzStatList[bestIndex4Mz].key + Const.Left_Slash +
                            intensityStatList[bestIndex4Intensity].key + Const.Left_Slash +
                            mobiStatList[bestIndex4Mobi].key);
                bestCombination = new Combination(mzStatList[bestIndex4Mz].key,
                    intensityStatList[bestIndex4Intensity].key,
                    mobiStatList[bestIndex4Mobi].key);
            }
            else
            {
                jobInfo.log(Tag.Best_Combo_Comp + mzStatList[bestIndex4Mz].key + Const.Left_Slash +
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
            jobInfo.log(Tag.Pretreatment + totalSize, Status.Pretreatment);
            for (var i = 0; i < totalSize; i++)
            {
                Spectrum spectrum = spectrumList.spectrum(i, false);
                string msLevel = CVUtil.parseMsLevel(spectrum);
                jobInfo.setStatus("Pre:" + i + "/" + totalSize);
                //最后一个谱图,单独判断
                if (i == totalSize - 1)
                {
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        ms1List.Add(parseMS1(spectrum, i)); //如果是MS1谱图,加入到MS1List
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
                        ms1List.Add(parseMS1(spectrum, i)); //加入MS1List
                        Spectrum next = spectrumList.spectrum(i + 1);
                        if (CVUtil.parseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                        {
                            parentNum = i;
                        }
                    }

                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                        addToMS2Map(ms2Index.pNum, ms2Index); //如果是MS2谱图,加入到谱图组
                    }
                }
            }

            jobInfo.log(Tag.Effective_MS1_List_Size + ms1List.Count);
            jobInfo.log(Tag.MS2_Group_List_Size + ms2Table.Count);
            jobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentDIA()
        {
            int parentNum = 0;
            jobInfo.log(Tag.Pretreatment + totalSize, Status.Pretreatment);
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < totalSize; i++)
            {
                progress++;
                jobInfo.log(null, Tag.progress(Tag.Pre, progress, totalSize));
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = CVUtil.parseMsLevel(spectrum);
                //如果这个谱图是MS1                          
                if (msLevel.Equals(MsLevel.MS1))
                {
                    parentNum = i;
                    ms1List.Add(parseMS1(spectrum, i));
                }

                //如果这个谱图是MS2
                if (msLevel.Equals(MsLevel.MS2))
                {
                    MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                    //边扫描边建立SWATH WindowRange
                    if (!rangeTable.Contains(ms2Index.precursor.mz))
                    {
                        WindowRange range = ms2Index.precursor;
                        ranges.Add(range);
                        rangeTable.Add(range.mz, range);
                    }

                    //DIA的MS2Map以precursorMz为key
                    addToMS2Map(ms2Index.precursor.mz, ms2Index);
                }
            }

            jobInfo.log(Tag.Total_SWATH_WINDOWS + ranges.Count);
            jobInfo.log(Tag.Effective_MS1_List_Size + ms1List.Count);
            jobInfo.log(Tag.MS2_Group_List_Size + ms2Table.Count);
            jobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentDDAPasef()
        {
            int parentNum = 0;
            jobInfo.log(Tag.Pretreatment + totalSize, Status.Pretreatment);
            for (var i = 0; i < totalSize; i++)
            {
                jobInfo.log(null, Tag.progress(Tag.Pre, i, totalSize));
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = CVUtil.parseMsLevel(spectrum);

                //最后一个谱图,单独判断
                if (i == totalSize - 1)
                {
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        ms1List.Add(parseMS1(spectrum, i)); //如果是MS1谱图,加入到MS1List
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
                        ms1List.Add(parseMS1(spectrum, i)); //加入MS1List
                        Spectrum next = spectrumList.spectrum(i + 1);
                        if (CVUtil.parseMsLevel(next).Equals(MsLevel.MS2)) //如果下一个谱图是MS2, 那么将这个谱图设置为当前的父谱图
                        {
                            parentNum = i;
                        }
                    }

                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                        addToMS2Map(ms2Index.pNum, ms2Index); //如果这个谱图是MS2
                    }
                }
            }

            jobInfo.log(Tag.Effective_MS1_List_Size + ms1List.Count);
            jobInfo.log(Tag.MS2_Group_List_Size + ms2Table.Count);
            jobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentDIAPasef()
        {
            int parentNum = 0;
            jobInfo.log(Tag.Pretreatment + totalSize, Status.Pretreatment);
            int progress = 0;
            // 预处理所有的MS谱图,将MS1与MS2的信息扫描以后放入对应的内存对象中
            for (int i = 0; i < totalSize; i++)
            {
                progress++;
                jobInfo.log(null, Tag.progress(Tag.Pre, progress, totalSize));
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = CVUtil.parseMsLevel(spectrum);
                //如果这个谱图是MS1                          
                if (msLevel.Equals(MsLevel.MS1))
                {
                    parentNum = i;
                    ms1List.Add(parseMS1(spectrum, i));
                }

                //如果这个谱图是MS2
                if (msLevel.Equals(MsLevel.MS2))
                {
                    MsIndex ms2Index = parseMS2(spectrum, i, parentNum);
                    //边扫描边建立SWATH WindowRange
                    if (!rangeTable.Contains(ms2Index.precursor.mz))
                    {
                        WindowRange range = ms2Index.precursor;
                        ranges.Add(range);
                        rangeTable.Add(range.mz, range);
                    }

                    //DIA的MS2Map以precursorMz为key
                    addToMS2Map(ms2Index.precursor.mz, ms2Index);
                }
            }

            jobInfo.log(Tag.Total_SWATH_WINDOWS + ranges.Count);
            jobInfo.log(Tag.Effective_MS1_List_Size + ms1List.Count);
            jobInfo.log(Tag.MS2_Group_List_Size + ms2Table.Count);
            jobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public void pretreatmentPRM()
        {
            int parentNum = 0;
            jobInfo.log(Status.tag_preprocessing + totalSize, Status.Preprocessing);
            for (int i = 0; i < totalSize; i++)
            {
                jobInfo.log(null, Tag.progress(Tag.Empty, (i + 1), totalSize));
                Spectrum spectrum = spectrumList.spectrum(i);
                string msLevel = CVUtil.parseMsLevel(spectrum);
                //如果是最后一个谱图,那么单独判断
                if (i == totalSize - 1)
                {
                    //如果是MS1谱图,那么直接跳过
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        continue;
                    }

                    //如果是MS2谱图,加入到谱图组
                    if (msLevel.Equals(MsLevel.MS2))
                    {
                        MsIndex ms2Index = parseMS2(spectrumList.spectrum(i), i, parentNum);
                        addToMS2Map(ms2Index.precursor.mz, ms2Index);
                        continue;
                    }
                }

                //如果这个谱图是MS1
                if (msLevel.Equals(MsLevel.MS1))
                {
                    Spectrum next = spectrumList.spectrum(i + 1);
                    string msLevelNext = CVUtil.parseMsLevel(next);
                    //如果下一个谱图仍然是MS1, 那么直接忽略这个谱图
                    if (msLevelNext.Equals(MsLevel.MS1))
                    {
                        continue;
                    }

                    if (msLevelNext.Equals(MsLevel.MS2))
                    {
                        parentNum = i;
                        ms1List.Add(parseMS1(spectrumList.spectrum(i), i));
                    }
                }

                if (msLevel.Equals(MsLevel.MS2))
                {
                    MsIndex ms2Index = parseMS2(spectrumList.spectrum(i), i, parentNum);
                    addToMS2Map(ms2Index.precursor.mz, ms2Index); //如果这个谱图是MS2
                }
            }

            jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
            jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
            jobInfo.log("Start Processing MS1 List");
        }
    }
}