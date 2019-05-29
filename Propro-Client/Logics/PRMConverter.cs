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
        private int totalSize;//总计的谱图数目
        private long startPosition;//块索引的指针
        private long blockStartPosition;//块索引的指针
        private int progress;//进度计数器
        List<ScanIndex> ms1List = new List<ScanIndex>();//用于存放MS1索引及基础信息
        Hashtable ms2Table = new Hashtable();//用于存放MS2的索引信息,key为mz

        public PRMConverter(ConvertJobInfo jobInfo) : base(jobInfo) {}

        public override void doConvert()
        {
            start();
            initDirectory();//创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readyForReadVendorFile();//准备读取Vendor文件
                    wrapping();//Data Wrapping
                    initGlobalVar();//初始化全局变量
                    preProgress();//预处理谱图,将MS1和MS2谱图分开存储
                    doWithMS1();//正式处理MS1,并将索引写入文件流中
                    doWithMS2AndSWATHBlockIndex();//处理MS2,同时生成SWATH Block索引信息
                    writeToAirdInfoFile();//将Info数据写入文件
                }
            }
            finish();
        }

        private void initGlobalVar()
        {
            totalSize = spectrumList.size();
            progress = 0;
            jobInfo.log("Total Size(Include useless MS1):" + totalSize);
            startPosition = 0;//文件的存储位置,每一次解析完就会将指针往前挪移
            blockStartPosition = 0;//当一整个range的所有谱图块全部解析完毕以后再将本字段往前挪移
        }

        private string getMsLevel(int index)
        {
            return spectrumList.spectrum(index).cvParamChild(CVID.MS_ms_level).value.ToString();
        }

        private ScanIndex parseMS1(Spectrum spectrum, int index)
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

        private ScanIndex parseMS2(Spectrum spectrum, int index, int parentIndex)
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
                jobInfo.log("ERROR:SpectrumIndex:" + spectrum.index)
                    .log("ERROR:SpectrumId:" + spectrum.id)
                    .log("ERROR: mz:" + spectrum.precursors[0].isolationWindow
                             .cvParamChild(CVID.MS_isolation_window_target_m_z).value)
                    .log("ERROR: lowerOffset:" + spectrum.precursors[0].isolationWindow
                             .cvParamChild(CVID.MS_isolation_window_lower_offset).value)
                    .log("ERROR: upperOffset:" + spectrum.precursors[0].isolationWindow
                             .cvParamChild(CVID.MS_isolation_window_upper_offset).value);
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

        private void addToMS2Map(ScanIndex ms2Index)
        {
            if (ms2Table.Contains(ms2Index.mz))
            {
                (ms2Table[ms2Index.mz] as List<ScanIndex>).Add(ms2Index);
            }
            else
            {
                List<ScanIndex> indexList = new List<ScanIndex>();
                indexList.Add(ms2Index);
                ms2Table.Add(ms2Index.mz, indexList);
            }
        }

        //Step1. 解析MS1和MS2谱图
        private void preProgress()
        {
            int parentNum = 0;
            jobInfo.log("Preprocessing:" + totalSize, "Preprocessing");
            for (var i = 0; i < totalSize; i++)
            {
                //如果是最后一个谱图,那么单独判断
                if (i == totalSize - 1)
                {
                    if (getMsLevel(i).Equals(MsLevel.MS1)) continue; //如果是MS1谱图,那么直接跳过
                    if (getMsLevel(i).Equals(MsLevel.MS2)) addToMS2Map(parseMS2(spectrumList.spectrum(i), i, parentNum)); //如果是MS2谱图,加入到谱图组

                }
                //如果这个谱图是MS1                          
                if (getMsLevel(i).Equals(MsLevel.MS1))
                {
                    if (getMsLevel(i + 1).Equals(MsLevel.MS1)) continue; //如果下一个谱图仍然是MS1,那么直接忽略这个谱图
                    if (getMsLevel(i + 1).Equals(MsLevel.MS2)) //如果下一个谱图是MS2,那么将这个谱图设置为当前的父谱图
                    {
                        parentNum = i;
                        ms1List.Add(parseMS1(spectrumList.spectrum(i), i));
                    }
                }
                if (getMsLevel(i).Equals(MsLevel.MS2)) addToMS2Map(parseMS2(spectrumList.spectrum(i), i, parentNum)); //如果这个谱图是MS2
            }

            jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
            jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
            jobInfo.log("Start Processing MS1 List");
        }

        private void doWithMS1()
        {
            for (int i = 0; i < ms1List.Count; i++)
            {
                jobInfo.progress.Report("MS1:" + i + "/" + ms1List.Count);
                byte[] mzArrayBytes, intArrayBytes;
                ScanIndex scanIndex = ms1List[i];
                compress(spectrumList.spectrum(scanIndex.num, true), out mzArrayBytes, out intArrayBytes);

                scanIndex.pos = new Hashtable();
                scanIndex.pos.Add(PositionType.AIRD_MZ, new Position(startPosition, mzArrayBytes.Length));
                scanIndex.pos.Add(PositionType.AIRD_INTENSITY, new Position(startPosition + mzArrayBytes.Length, intArrayBytes.Length));
                scanIndexList.Add(scanIndex);
                startPosition = startPosition + mzArrayBytes.Length + intArrayBytes.Length;
                blockStartPosition = startPosition;
                airdStream.Write(mzArrayBytes, 0, mzArrayBytes.Length);
                airdStream.Write(intArrayBytes, 0, intArrayBytes.Length);
            }
            ranges = new List<WindowRange>();
        }

        private void doWithMS2AndSWATHBlockIndex()
        {
            jobInfo.log("Start Processing MS2 List");
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
                jobInfo.log(null, "MS2:" + progress + "/" + ms2Table.Keys.Count);
                progress++;
                foreach (ScanIndex index in indexList)
                {
                    byte[] mzArrayBytes, intArrayBytes;
                    compress(spectrumList.spectrum(index.num, true), out mzArrayBytes, out intArrayBytes);
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
                jobInfo.log("MS2 Group Finished:" + progress + "/" + ms2Table.Keys.Count);
            }
        }

    }
}