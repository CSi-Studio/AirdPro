using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using pwiz.CLI.analysis;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using pwiz.CLI.util;
using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using Propro.Utils;

namespace Propro.Logics
{
    internal class PRMConverter : IConverter
    {
//        private static readonly object calculateSHA1Mutex = new object();

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
            //用于PRM Aird平台的实验源文件中,msdList.Count必然为1,否则实验文件格式不符合要求
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

                SpectrumList spectrumList = msd.run.spectrumList;
                int size = spectrumList.size();
                jobInfo.addLog("Total Size(Include useless MS1):" + size);

                //文件的存储位置,每一次解析完就会将指针往前挪移
                long startPosition = 0;
                //当一整个range的所有谱图块全部解析完毕以后再讲本字段往前挪移
                long blockStartPosition = 0;

                using (var airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
                {
                    using (var airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
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
                        List<ScanIndex> ms1List = new List<ScanIndex>();
                        List<ScanIndex> swathIndexList = new List<ScanIndex>();
                        List<ScanIndex> scanIndexList = new List<ScanIndex>();
                        Hashtable ms2Table = new Hashtable();
                        int parentNum = 0;
                        //Step1. 预处理
                        jobInfo.progress.Report("Preprocessing");
                        jobInfo.addLog("Preprocessing:"+size);
                        for (var i = 0; i < size; i++)
                        {
                            //如果是最后一个谱图,那么单独判断
                            if (i == size - 1)
                            {
                                //如果是MS1谱图,那么直接跳过
                                if (getMsLevel(spectrumList, i).Equals(MsLevel.MS1))
                                {
                                    continue;
                                }

                                //如果是MS2谱图,加入到谱图组
                                if (getMsLevel(spectrumList, i).Equals(MsLevel.MS2))
                                {
                                    ScanIndex index = parseMS2(spectrumList.spectrum(i), jobInfo, i, parentNum);
                                    addToMS2Map(ms2Table, index);
                                }
                            }           
                            //如果这个谱图是MS1                          
                            if (getMsLevel(spectrumList, i).Equals(MsLevel.MS1))
                            {
                                //如果下一个谱图仍然是MS1,那么直接忽略这个谱图  
                                if (getMsLevel(spectrumList, i + 1).Equals(MsLevel.MS1))
                                {
                                    continue;
                                }
                                //如果下一个谱图是MS2,那么将这个谱图设置为当前的父谱图
                                else if (getMsLevel(spectrumList, i + 1).Equals(MsLevel.MS2))
                                {
                                    parentNum = i;
                                    ScanIndex ms1Index = parseMS1(spectrumList.spectrum(i), i);
                                    ms1List.Add(ms1Index);
                                }
                            }
                            //如果这个谱图是MS2
                            else if (getMsLevel(spectrumList, i).Equals(MsLevel.MS2))
                            {
                                ScanIndex ms2Index = parseMS2(spectrumList.spectrum(i), jobInfo, i, parentNum);
                                addToMS2Map(ms2Table, ms2Index);
                            }
                        }
                        jobInfo.addLog("Effective MS1 List Size:" + ms1List.Count);
                        jobInfo.addLog("MS2 Group List Size:" + ms2Table.Count);
                        jobInfo.addLog("Start Processing MS1 List");
                        //Step1. 正式处理MS1
                        for (int i = 0; i < ms1List.Count; i++)
                        {
                            jobInfo.progress.Report("MS1:"+i+"/"+ms1List.Count);
                            byte[] mzArrayBytes, intArrayBytes;
                            ScanIndex scanIndex = ms1List[i];
                            compress(spectrumList.spectrum(scanIndex.num, true), jobInfo, out mzArrayBytes, out intArrayBytes);

                            scanIndex.pos = new Hashtable();
                            scanIndex.pos.Add(PositionType.AIRD_MZ, new Position(startPosition, mzArrayBytes.Length));
                            scanIndex.pos.Add(PositionType.AIRD_INTENSITY, new Position(startPosition + mzArrayBytes.Length, intArrayBytes.Length));
                            scanIndexList.Add(scanIndex);
                            startPosition = startPosition + mzArrayBytes.Length + intArrayBytes.Length;
                            blockStartPosition = startPosition;
                            airdStream.Write(mzArrayBytes, 0, mzArrayBytes.Length);
                            airdStream.Write(intArrayBytes, 0, intArrayBytes.Length);
                        }
                        List<WindowRange> ranges = new List<WindowRange>();

                        //Step1. 正式处理MS2
                        jobInfo.addLog("Start Processing MS2 List");
                        int count = 0;
                        foreach (float key in ms2Table.Keys)
                        {
                            List<ScanIndex> indexList = ms2Table[key] as List<ScanIndex>;
                            //为每一个key组创建一个SwathBlock
                            ScanIndex swathIndex = new ScanIndex();
                            swathIndex.level = 0;
                            swathIndex.pos = new Hashtable();
                            swathIndex.pos.Add(PositionType.SWATH, new Position(blockStartPosition, 0));
                            swathIndex.mz = key;
                            swathIndex.mzStart = indexList[0].mzStart; //因为每一个ScanIndex的Wid都是相同的,这里直接取第一个
                            swathIndex.mzEnd = indexList[0].mzEnd;
                            //顺便创建一个WindowRanges,用以让Propro服务端快速获取全局的窗口数目和mz区间
                            ranges.Add(new WindowRange(swathIndex.mzStart, swathIndex.mzEnd, swathIndex.mz));

                            List<long> blocks = new List<long>();
                            List<float> rts = new List<float>();
                            jobInfo.progress.Report("MS2:" + count + "/" + ms2Table.Keys.Count);
                            count++;
                            foreach (ScanIndex index in indexList)
                            {
                                byte[] mzArrayBytes, intArrayBytes;
                             
                                compress(spectrumList.spectrum(index.num, true), jobInfo, out mzArrayBytes, out intArrayBytes);

                                index.pos = new Hashtable();
                                index.pos.Add(PositionType.AIRD_MZ, new Position(startPosition, mzArrayBytes.Length));
                                index.pos.Add(PositionType.AIRD_INTENSITY, new Position(startPosition + mzArrayBytes.Length, intArrayBytes.Length));
                                scanIndexList.Add(index);//加入到最终需要序列化的列表中
                                startPosition = startPosition + mzArrayBytes.Length + intArrayBytes.Length;
                                blockStartPosition = startPosition;
                                //预设SwathIndex的信息
                                blocks.Add(mzArrayBytes.Length);
                                blocks.Add(intArrayBytes.Length);
                                rts.Add(index.rt);

                                airdStream.Write(mzArrayBytes, 0, mzArrayBytes.Length);
                                airdStream.Write(intArrayBytes, 0, intArrayBytes.Length);
                            }

                            swathIndex.blocks = blocks;
                            swathIndex.rts = rts;
                            Position position = (Position)swathIndex.pos[PositionType.SWATH];
                            position.d = blockStartPosition - position.s;
                            swathIndexList.Add(swathIndex);
                            jobInfo.addLog("MS2 Group Finished:" + count + "/" + ms2Table.Keys.Count);
                        }

                        //Step4. 写入AirdInfo JSON文件中
                        jobInfo.progress.Report("Writing into File");
                        airdInfo.scanIndexList = scanIndexList;
                        airdInfo.swathIndexList = swathIndexList;
                        airdInfo.rangeList = ranges;
                        airdInfo.creator = "Admin";
                        airdInfo.type = ExperimentType.PRM;
                        Strategy mzStrategy = new Strategy("pFor,zlib", jobInfo.mzPrecision);
                        Strategy intensityStrategy = new Strategy("zlib", jobInfo.intensityPrecision);
                        airdInfo.strategies.Add("mz", mzStrategy);
                        airdInfo.strategies.Add("intensity", intensityStrategy);
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

        public string getMsLevel(SpectrumList spectrumList,int index)
        {
            return spectrumList.spectrum(index).cvParamChild(CVID.MS_ms_level).value.ToString();
        }

        public ScanIndex parseMS1(Spectrum spectrum, int index)
        {
            ScanIndex ms1 = new ScanIndex();
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

        public ScanIndex parseMS2(Spectrum spectrum,ConvertJobInfo jobInfo, int index, int parentIndex)
        {
            ScanIndex ms2 = new ScanIndex();
            ms2.level = 2;
            ms2.num = index;
            ms2.pNum = parentIndex;

            try
            {
                float mz = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_target_m_z).value.ToString());
                float lowerOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_lower_offset).value.ToString());
                float upperOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_upper_offset).value.ToString());

                ms2.mz = mz;
                ms2.mzStart = mz - lowerOffset;
                ms2.mzEnd = mz + upperOffset;
                ms2.wid = lowerOffset + upperOffset;
            }
            catch (Exception e)
            {
                jobInfo.addLog("ERROR:SpectrumIndex:"+spectrum.index);
                jobInfo.addLog("ERROR:SpectrumId:"+spectrum.id);
                jobInfo.addLog("ERROR: mz:"+ spectrum.precursors[0].isolationWindow.cvParamChild(CVID.MS_isolation_window_target_m_z).value);
                jobInfo.addLog("ERROR: lowerOffset:"+ spectrum.precursors[0].isolationWindow.cvParamChild(CVID.MS_isolation_window_lower_offset).value);
                jobInfo.addLog("ERROR: upperOffset:" + spectrum.precursors[0].isolationWindow.cvParamChild(CVID.MS_isolation_window_upper_offset).value);
                throw e;
            }
            
            if (spectrum.scanList.scans.Count != 1)
            {
                return ms2;
            }

            Scan scan = spectrum.scanList.scans[0];
            ms2.rt = parseRT(scan);
            
            return ms2;
        }

        public void addToMS2Map(Hashtable table, ScanIndex ms2Index)
        {
            if (table.Contains(ms2Index.mz))
            {
                (table[ms2Index.mz] as List<ScanIndex>).Add(ms2Index);
            }
            else
            {
                List<ScanIndex> indexList = new List<ScanIndex>();
                indexList.Add(ms2Index);
                table.Add(ms2Index.mz, indexList);
            }
        }
    }
}