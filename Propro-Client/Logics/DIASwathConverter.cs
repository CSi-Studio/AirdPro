using Newtonsoft.Json;
using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using Propro_Client.Domains.Aird;
using Propro_Client.Utils;
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
        private float overlap;//SWATH窗口间的区域重叠值
        private int rangeSize;//总计的WindowRange数目
        private int totalSize;//总计的谱图数目
        private int swathBlocks;//SWATH块数目
        private long startPosition;//块索引的指针
        private long blockStartPosition;//块索引的指针
        private int progress;//进度计数器
        
        public DIASwathConverter(ConvertJobInfo jobInfo) : base(jobInfo) {}

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
                    buildWindowsRanges();  //Getting SWATH Windows
                    computeOverlap(); //Compute Overlap
                    adjustOverlap(); //Adjust Overlap
                    initGlobalVar();//初始化全局变量
                    coreLoop();//核心的循环
                    writeToAirdInfoFile();//将Info数据写入文件
                }
            }
            finish();
        }

        //初始化全局变量
        private void initGlobalVar()
        {
            rangeSize = ranges.Count + 1; //加上MS1的一个Range
            totalSize = msd.run.spectrumList.size(); //原始文件中谱图的总数目(包含MS1和MS2)
            swathBlocks = (totalSize % rangeSize == 0) ? (totalSize / rangeSize) : (totalSize / rangeSize + 1);//如果是整除的,说明每一个block都包含完整的信息,如果不整除,那么最后一个block内的窗口不完整
            startPosition = 0;//文件的存储位置,每一次解析完就会将指针往前挪移
            blockStartPosition = 0;//当一整个range的所有谱图块全部解析完毕以后再讲本字段往前挪移
            progress = 0;//扫描进度计数器
            //PreCheck size数目必须是rangeSize的整倍数,否则数据有误
            if (totalSize % rangeSize != 0) jobInfo.log("Spectrum not perfect,Total Size : " + totalSize + "|Range Size : " + rangeSize);
            jobInfo.log("Swath Windows Size:" + ranges.Count).log("Total Size(Include MS1):" + totalSize).log("Overlap:" + overlap);
        }

        //提取SWATH 窗口信息
        private void buildWindowsRanges()
        {
            jobInfo.log("Start getting windows", "Getting Windows");
            int i = 0;
            Spectrum spectrum = spectrumList.spectrum(0);
            while (!spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals("1"))
            {
                i++;
                spectrum = spectrumList.spectrum(i);
                if (i > spectrumList.size() / 2 || i > 500)
                {
                    //如果找了一半的spectrum或者找了超过500个spectrum仍然没有找到ms1,那么数据格式有问题,返回空;
                    jobInfo.logError("Search for more than 500 spectrums and no ms1 was found. File Format Error");
                    throw new Exception("Search for more than 500 spectrums and no ms1 was found. File Format Error");
                }
            }

            i++;
            spectrum = spectrumList.spectrum(i);
            while (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS2))
            {
                float mz = (float) double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_target_m_z).value.ToString());
                float lowerOffset = (float) double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_lower_offset).value.ToString());
                float upperOffset = (float) double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_upper_offset).value.ToString());

                ranges.Add(new WindowRange(mz - lowerOffset, mz + upperOffset, mz));
                i++;
                spectrum = spectrumList.spectrum(i);
            }
            jobInfo.log("Finished Getting Windows");
        }

        //计算窗口间的重叠区域的大小
        private void computeOverlap()
        {
            WindowRange range1 = ranges[0];
            float range1Right = range1.end;
            WindowRange range2 = ranges[1];
            float range2Left = range2.start;
            overlap = range1Right - range2Left;
        }

        //调整间距
        private void adjustOverlap()
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

        private void coreLoop()
        {
            jobInfo.log(null, progress + "/" + totalSize);
            for (int i = 0; i < rangeSize; i++)
            {
                buildSWATHBlock(i);
            }
        }

        //完整计算一个SWATH块的信息,包含谱图索引创建,数据压缩和SWATH块索引创建
        private void buildSWATHBlock(int rangeBatchIndex)
        {
            List<ScanIndex> blockIndexList = new List<ScanIndex>();
            jobInfo.log("开始解析第" + (rangeBatchIndex + 1) + "批数据,共" + rangeSize + "批");
            //每一次循环代表一个完整的Swath窗口
            for (int j = 0; j < swathBlocks; j++)
            {
                int index = rangeBatchIndex + j * rangeSize;
                if (index > totalSize - 1) continue;
                Spectrum spectrum = spectrumList.spectrum(index, true);
                if (spectrum.scanList.scans.Count != 1) continue;
                ScanIndex scanIndex = buildScanIndex(spectrum, index);
                compressToFile(spectrum, scanIndex);
                blockIndexList.Add(scanIndex);
                if (progress % 10 == 0) jobInfo.log(null, progress + 1 + "/" + totalSize);
                progress++;
            }

            //将谱图块存入总列表中
            scanIndexList.AddRange(blockIndexList);
            //如果是MS2的SWATH块,则需要额外创建一个基于Swath MS2谱图块的索引
            if (rangeBatchIndex != 0)
            {
                buildSWATHIndex(blockIndexList, ranges[rangeBatchIndex - 1]);
            }
            else
            {
                //如果不是MS2的谱图块,那么直接把位置指针往后挪
                foreach (ScanIndex index in blockIndexList)
                {
                    long mzDelta = ((Position)index.pos[PositionType.AIRD_MZ]).d;
                    long intensityDelta = ((Position)index.pos[PositionType.AIRD_INTENSITY]).d;
                    blockStartPosition = blockStartPosition + mzDelta + intensityDelta;
                }
            }

            jobInfo.log("第" + (rangeBatchIndex + 1) + "批数据解析完毕");
        }

        //计算每一张谱图的索引信息
        private ScanIndex buildScanIndex(Spectrum spectrum, int index)
        {
            ScanIndex scanIndex = new ScanIndex();
            scanIndex.num = index;
            Scan scan = spectrum.scanList.scans[0];
            scanIndex.rt = parseRT(scan);

            if (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS1))
            {
                scanIndex.level = 1;
            }
            else //MS2的信息
            {
                scanIndex.level = 2;
                if (spectrum.precursors.Count != 1) jobInfo.log("spectrum precursor count is not 1, scanIndex num:" + index);
                float mz = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_target_m_z).value.ToString());
                float lowerOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_lower_offset).value.ToString());
                float upperOffset = (float)double.Parse(spectrum.precursors[0].isolationWindow
                    .cvParamChild(CVID.MS_isolation_window_upper_offset).value.ToString());

                Hashtable features = new Hashtable();
                features.Add(Features.scan_index_original_width, lowerOffset + upperOffset);
                features.Add(Features.scan_index_original_precursor_mz_start, mz - lowerOffset);
                features.Add(Features.scan_index_original_precursor_mz_end, mz + upperOffset);

                scanIndex.features = FeaturesUtil.toString(features);
                scanIndex.mz = mz;
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

            return scanIndex;
        }

        //将谱图进行压缩并且存储文件流中
        private void compressToFile(Spectrum spectrum, ScanIndex scanIndex)
        {
            try
            {
                byte[] mzArrayBytes, intArrayBytes;
                compress(spectrum, out mzArrayBytes, out intArrayBytes);

                scanIndex.pos = new Hashtable();
                scanIndex.pos.Add(PositionType.AIRD_MZ, new Position(startPosition, mzArrayBytes.Length));
                scanIndex.pos.Add(PositionType.AIRD_INTENSITY, new Position(startPosition + mzArrayBytes.Length, intArrayBytes.Length));
                startPosition = startPosition + mzArrayBytes.Length + intArrayBytes.Length;

                airdStream.Write(mzArrayBytes, 0, mzArrayBytes.Length);
                airdStream.Write(intArrayBytes, 0, intArrayBytes.Length);
            }
            catch (Exception exception)
            {
                jobInfo.log(exception.Message);
                Console.Out.WriteLine(exception.Message);
            }
        }

        //计算每一个SWATH块的索引信息
        private void buildSWATHIndex(List<ScanIndex> blockIndexList, WindowRange range)
        {
            ScanIndex swathIndex = new ScanIndex();
            swathIndex.level = 0;
            swathIndex.pos = new Hashtable();
            swathIndex.pos.Add(PositionType.SWATH, new Position(blockStartPosition, 0));
            swathIndex.mzStart = range.start;
            swathIndex.mzEnd = range.end;
            List<long> blockSizes = new List<long>();
            List<float> rts = new List<float>();
            foreach (ScanIndex index in blockIndexList)
            {
                long mzDelta = ((Position)index.pos[PositionType.AIRD_MZ]).d;
                long intensityDelta = ((Position)index.pos[PositionType.AIRD_INTENSITY]).d;
                blockSizes.Add(mzDelta);
                blockSizes.Add(intensityDelta);
                blockStartPosition = blockStartPosition + mzDelta + intensityDelta;
                rts.Add(index.rt);
            }

            Position position = (Position)swathIndex.pos[PositionType.SWATH];
            position.d = blockStartPosition - position.s;
            swathIndex.rts = rts;
            swathIndex.blocks = blockSizes;
            swathIndexList.Add(swathIndex);
        }

        
    }
}