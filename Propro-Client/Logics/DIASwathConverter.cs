using Newtonsoft.Json;
using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using pwiz.CLI.analysis;
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Propro.Logics
{
    internal class DIASwathConverter : IConverter
    {
        public override void doConvert(ConvertJobInfo jobInfo)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            jobInfo.progress.Report("Starting");
            ReaderList readers = ReaderList.FullReaderList;
            MSDataList msdList = new MSDataList();
            ReaderConfig rc = new ReaderConfig();
            rc.ignoreZeroIntensityPoints = true;
            readers.read(jobInfo.inputFilePath, msdList, rc);
            //用于Swath Aird平台的实验源文件中,msdList.Count必然为1,否则实验文件格式不符合要求
            if (msdList.Count != 1)
            {
                jobInfo.addLog("File Format Error.MSDataList size must be 1");
                jobInfo.progress.Report("Error");
                return;
            }

            foreach (MSData msd in msdList)
            {
                jobInfo.progress.Report("Wrapping");
                jobInfo.addLog("Start Converting");

                //only one thread 
//                lock (calculateSHA1Mutex)
//                {
//                    jobInfo.progress.Report("SHA1 Checking");
//                    MSDataFile.calculateSHA1Checksums(msd);
//                }

                SpectrumListFactory.wrap(msd, new List<string>());

                if (msd.run.spectrumList == null || msd.run.spectrumList.empty())
                {
                    jobInfo.progress.Report("Error");
                    jobInfo.addLog("No Spectrums Found");
                    return;
                }
                
                jobInfo.progress.Report("Getting Windows");
                jobInfo.addLog("Start getting windows");
                List<WindowRange> ranges = getWindows(msd);
                jobInfo.addLog("Finished Getting Windows");
                float overlap = computeOverlap(ranges);
                adjustOverlap(ranges, overlap);
                int rangeSize = ranges.Count + 1; //加上MS1的一个Range
                int size = msd.run.spectrumList.size();
                //size数目必须是rangeSize的整倍数,否则数据有误
                if (size % rangeSize != 0)
                {
                    jobInfo.addLog("Spectrum not perfect,Total Size : " + size + "|Range Size : " + rangeSize);
                }
                
                jobInfo.addLog("Swath Windows Size:"+ranges.Count);
                jobInfo.addLog("Total Size(Include MS1):"+ size);
                jobInfo.addLog("Overlap:"+ overlap);
                
                using (FileStream airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
                {
                    using (FileStream airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                    {
                        AirdInfo airdInfo = new AirdInfo();
                        InstrumentInfo instrumentInfo = new InstrumentInfo();
                        ParentFileInfo parentFileInfo = new ParentFileInfo();

                        //仪器设备信息
                        instrumentInfo.msManufacturer = "SCIEX";
                        instrumentInfo.msModel = msd.run.defaultInstrumentConfiguration.cvParams[0].name;
                        instrumentInfo.msIonisation = "electrospray ionization";
                        instrumentInfo.msMassAnalyer = "quadrupole";
                        instrumentInfo.msDetector = "electron multiplier";
                        instrumentInfo.software_type = "aquisition";
                        instrumentInfo.software_name = msd.softwareList[0].id;
                        instrumentInfo.software_version = msd.softwareList[0].version;
                        airdInfo.instrumentInfo = instrumentInfo;

                        //源文件位置
                        parentFileInfo.fileName = msd.fileDescription.sourceFiles[0].location;
                        parentFileInfo.fileType = "RAWData";
                        airdInfo.parentFileInfo = parentFileInfo;
                        
                        airdInfo.rawId = msd.id;
                        airdInfo.rangeList = ranges;
                        airdInfo.overlap = overlap;
                        Strategy mzStrategy = new Strategy("pFor,zlib",jobInfo.mzPrecision);
                        Strategy intensityStrategy = new Strategy("zlib",jobInfo.intensityPrecision);
                        airdInfo.strategies.Add("mz",mzStrategy);
                        airdInfo.strategies.Add("intensity",intensityStrategy);

                        airdInfo.ignoreZeroIntensity = jobInfo.ignoreZeroIntensity;

                        int swathBlocks = 0;
                        //如果是整除的,说明每一个block都包含完整的信息
                        if (size % rangeSize == 0)
                        {
                            swathBlocks = size / rangeSize; //包含的Swath窗口(含MS1)
                        }
                        else
                        {
                            swathBlocks = size / rangeSize + 1; //如果不整除,那么最后一个block内的窗口不完整
                        }

                        List<ScanIndex> scanIndexList = new List<ScanIndex>();
                        List<ScanIndex> swathIndexList = new List<ScanIndex>();

                        //文件的存储位置,每一次解析完就会将指针往前挪移
                        long startPosition = 0;
                        //当一整个range的所有谱图块全部解析完毕以后再讲本字段往前挪移
                        long blockStartPosition = 0;
                        
                        int count = 0;
                        jobInfo.progress.Report(count + "/" + size);
                        for (int i = 0; i < rangeSize; i++)
                        {
                            List<ScanIndex> blockIndexList = new List<ScanIndex>();
                            jobInfo.addLog("开始解析第" + (i + 1) + "批数据,共" + rangeSize + "批");
                            //每一次循环代表一个完整的Swath窗口
                            for (int j = 0; j < swathBlocks; j++)
                            {
                                int index = i + j * rangeSize;
                                if (index > size - 1)
                                {
                                    continue;
                                }
                                Spectrum spectrum = msd.run.spectrumList.spectrum(index, true);
                                ScanIndex scanIndex = new ScanIndex();
                                scanIndex.num = index;
                                if (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS1))
                                {
                                    scanIndex.level = 1;
                                }
                                else //MS2的信息
                                {
                                    scanIndex.level = 2;
                                    if (spectrum.precursors.Count != 1) Console.Out.WriteLine("Error");
                                    float mz = (float)double.Parse(spectrum.precursors[0].isolationWindow
                                        .cvParamChild(CVID.MS_isolation_window_target_m_z).value.ToString());
                                    float lowerOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                                        .cvParamChild(CVID.MS_isolation_window_lower_offset).value.ToString());
                                    float upperOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                                        .cvParamChild(CVID.MS_isolation_window_upper_offset).value.ToString());
                                    scanIndex.mz = mz;
                                    scanIndex.oWid = lowerOffset + upperOffset;
                                    scanIndex.oMzStart = mz - lowerOffset;
                                    scanIndex.oMzEnd = mz + upperOffset;
                                    scanIndex.wid = lowerOffset + upperOffset - overlap;
                                    //如果左边界在399-401之间的话,统一调整为400
                                    if (mz - (lowerOffset - overlap / 2) - 400 <= 1)
                                    {
                                        scanIndex.mzStart = 400;
                                    }
                                    else
                                    {
                                        scanIndex.mzStart = mz - (lowerOffset - overlap / 2);
                                    }
                                        
                                    scanIndex.mzEnd = mz + (upperOffset - overlap / 2);
                                    scanIndex.pNum = index / rangeSize * rangeSize; //取整数
                                }

                                if (spectrum.scanList.scans.Count != 1)
                                {
                                    continue;
                                }

                                Scan scan = spectrum.scanList.scans[0];
                                scanIndex.rt = parseRT(scan);
                                 
                                try
                                {
                                    byte[] mzArrayBytes, intArrayBytes;
                                    compress(spectrum, jobInfo, out mzArrayBytes,out intArrayBytes);
                                    
                                    scanIndex.pos = new Hashtable();
                                    scanIndex.pos.Add(PositionType.AIRD_MZ, new Position(startPosition, mzArrayBytes.Length));
                                    scanIndex.pos.Add(PositionType.AIRD_INTENSITY, new Position(startPosition + mzArrayBytes.Length, intArrayBytes.Length));
                                    startPosition = startPosition + mzArrayBytes.Length + intArrayBytes.Length;
                                    
                                    airdStream.Write(mzArrayBytes, 0, mzArrayBytes.Length);
                                    airdStream.Write(intArrayBytes, 0, intArrayBytes.Length);
                                    
                                    blockIndexList.Add(scanIndex);
                                    if (count % 10 == 0)
                                    {
                                        jobInfo.progress.Report(count + 1 + "/" + size);
                                    }
                                    
                                    count++;
                                }
                                catch (Exception exception)
                                {
                                    jobInfo.addLog(exception.Message);
                                    Console.Out.WriteLine(exception.Message);
                                }
                            }

                            //将谱图块存入总列表中
                            scanIndexList.AddRange(blockIndexList);
                            //如果是MS2的Swath块,则需要额外创建一个基于Swath MS2谱图块的索引
                            if (i != 0)
                            {
                                ScanIndex swathIndex = new ScanIndex();
                                swathIndex.level = 0;
                                swathIndex.pos = new Hashtable();
                                swathIndex.pos.Add(PositionType.SWATH, new Position(blockStartPosition, 0));
                                swathIndex.mzStart = ranges[i - 1].start;
                                swathIndex.mzEnd = ranges[i - 1].end;
                                List<long> blockSizes = new List<long>();
                                List<float> rts = new List<float>();
                                foreach (ScanIndex index in blockIndexList)
                                {
                                    long mzDelta = ((Position) index.pos[PositionType.AIRD_MZ]).d;
                                    long intensityDelta = ((Position) index.pos[PositionType.AIRD_INTENSITY]).d;
                                    blockSizes.Add(mzDelta);
                                    blockSizes.Add(intensityDelta);
                                    blockStartPosition = blockStartPosition + mzDelta + intensityDelta;
                                    rts.Add(index.rt);
                                }

                                Position position = (Position) swathIndex.pos[PositionType.SWATH];
                                position.d = blockStartPosition - position.s;
                                swathIndex.rts = rts;
                                swathIndex.blocks = blockSizes;
                                swathIndexList.Add(swathIndex);
                            }
                            else
                            {
                                //如果不是MS2的谱图块,那么直接把位置指针往后挪
                                foreach (ScanIndex index in blockIndexList)
                                {
                                    long mzDelta = ((Position) index.pos[PositionType.AIRD_MZ]).d;
                                    long intensityDelta = ((Position) index.pos[PositionType.AIRD_INTENSITY]).d;
                                    blockStartPosition = blockStartPosition + mzDelta + intensityDelta;
                                }
                            }
                            jobInfo.addLog("第" + (i + 1) + "批数据解析完毕");
                        }

                        jobInfo.progress.Report("Write-in");
                        airdInfo.scanIndexList = scanIndexList;
                        airdInfo.swathIndexList = swathIndexList;
                        airdInfo.creator = "Admin";
                        airdInfo.type = ExperimentType.DIA_SWATH;
                        airdInfo.createDate = DateTime.Now;
                        JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                        string airdInfoStr = JsonConvert.SerializeObject(airdInfo, jsonSetting);
                        byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
                        airdJsonStream.Write(airdBytes, 0, airdBytes.Length);
                        stopwatch.Stop();
                        jobInfo.progress.Report("Finished");
                        jobInfo.addLog("总耗时:" + stopwatch.Elapsed.TotalSeconds);
                    }
                }
            }
           
        }

        public List<WindowRange> getWindows(MSData msData)
        {
            List<WindowRange> ranges = new List<WindowRange>();
            int i = 0;
            Spectrum spectrum = msData.run.spectrumList.spectrum(0);
            while (!spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals("1"))
            {
                i++;
                spectrum = msData.run.spectrumList.spectrum(i);
                if (i > msData.run.spectrumList.size()/2 || i > 500)
                {
                    //如果找了一半的spectrum或者找了超过500个spectrum仍然没有找到ms1,那么数据格式有问题,返回空;
                    return null;
                }
            }

            i++;
            spectrum = msData.run.spectrumList.spectrum(i);
            while (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS2))
            {
                float mz = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_target_m_z).value.ToString());
                float lowerOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_lower_offset).value.ToString());
                float upperOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_upper_offset).value.ToString());
              
                ranges.Add(new WindowRange(mz - lowerOffset, mz + upperOffset, mz));
                i++;
                spectrum = msData.run.spectrumList.spectrum(i);
            }

            return ranges;
        }

        //计算窗口间的重叠区域
        public float computeOverlap(List<WindowRange> ranges)
        {
            WindowRange range1 = ranges[0];
            float range1Right = range1.end;
            WindowRange range2 = ranges[1];
            float range2Left = range2.start;
            return range1Right - range2Left;
        }

        public void adjustOverlap(List<WindowRange> ranges, float overlap)
        {
            foreach (WindowRange range in ranges)
            {
                if (range.start + (overlap / 2) - 400 <= 1)
                {
                    range.start = 400;
                }
                else
                {
                    range.start = range.start + (overlap / 2);
                }

                range.end = range.end - (overlap / 2);
            }
        }
    }
}