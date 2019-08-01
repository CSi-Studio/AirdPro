using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using Propro_Client.Domains.Aird;
using Propro_Client.Utils;
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Propro.Logics
{
    internal class DIASwathConverter : IConverter
    {
        private float overlap;//SWATH窗口间的区域重叠值
        private int rangeSize;//总计的WindowRange数目
        private int totalSize;//总计的谱图数目
        private int swathChilds;//每一个SWATH块中Scan的数量
        private int progress;//进度计数器
        
        public DIASwathConverter(ConvertJobInfo jobInfo) : base(jobInfo) {}

        public override void doConvert()
        {
            start();
            initDirectory();//创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile();//准备读取Vendor文件
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
            swathChilds = (totalSize % rangeSize == 0) ? (totalSize / rangeSize) : (totalSize / rangeSize + 1);//如果是整除的,说明每一个block都包含完整的信息,如果不整除,那么最后一个block内的窗口不完整
            startPosition = 0;//文件的存储位置,每一次解析完就会将指针往前挪移
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
            while (!spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS1))
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
                float mz, lowerOffset, upperOffset;
                mz = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_target_m_z);
                lowerOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_lower_offset);
                upperOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_upper_offset);
                
                WindowRange range = new WindowRange(mz - lowerOffset, mz + upperOffset, mz);
                Hashtable features = new Hashtable();
                features.Add(Features.original_width, lowerOffset + upperOffset);
                features.Add(Features.original_precursor_mz_start, mz - lowerOffset);
                features.Add(Features.original_precursor_mz_end, mz + upperOffset);
                range.features = FeaturesUtil.toString(features);
                ranges.Add(range);

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
            featuresMap.Add(Features.overlap, overlap);
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
                jobInfo.log("开始解析第" + (i + 1) + "批数据,共" + rangeSize + "批");
                buildSwathBlock(i, jobInfo.threadAccelerate);
            }
        }

        /**
         * 完整计算一个SWATH块的信息,包含谱图索引创建,数据压缩和SWATH块索引创建
         * threadAccelerate 是否进行多线程加速
         */
        private void buildSwathBlock(int rangeIndex, Boolean threadAccelerate)
        {

            SwathIndex swathIndex = new SwathIndex();
            swathIndex.startPtr = startPosition;

            //处理MS1 SWATH块
            if (rangeIndex == 0)
            {
                swathIndex.level = 1;
            }
            else
            {
                swathIndex.level = 2;
                swathIndex.range = ranges[rangeIndex - 1];
            }

            long countForRead = 0;
            long countForCompress = 0;

            if (threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, swathChilds, (j, ParallelLoopState) =>
                {
                    int index = rangeIndex + j * rangeSize;
                    if (index > totalSize - 1)
                    {
                        ParallelLoopState.Break();
                        return;
                    }
                    long start = Environment.TickCount;
                    Spectrum spectrum = spectrumList.spectrum(index, true);
                    countForRead += (Environment.TickCount - start);
                    if (spectrum.scanList.scans.Count != 1)
                    {
                        ParallelLoopState.Break();
                        return;
                    }

                    start = Environment.TickCount;
                    try
                    {
                        TempScan ts = new TempScan(j, parseRT(spectrum.scanList.scans[0]));
                        compress(spectrum, ts);
                        table.Add(j, ts);
                    }
                    catch (Exception exception)
                    {
                        jobInfo.log(exception.Message);
                        ParallelLoopState.Break();
                        return;
                    }
                    
                    countForCompress += (Environment.TickCount - start);
                    if (progress % 100 == 0) jobInfo.log(null, progress + 1 + "/" + totalSize);
                    progress++;
                });
                //并行处理完数据以后按次序存入Aird文件中
                outputWithOrder(table, swathIndex);
            }
            else
            {
                for (int j = 0; j < swathChilds; j++)
                {
                    int index = rangeIndex + j * rangeSize;
                    if (index > totalSize - 1) continue;
                    long start = Environment.TickCount;
                    Spectrum spectrum = spectrumList.spectrum(index, true);
                    countForRead += (Environment.TickCount - start);
                    if (spectrum.scanList.scans.Count != 1) continue;

                    start = Environment.TickCount;
                    compressToFile(j, spectrum, swathIndex);
                    countForCompress += (Environment.TickCount - start);
                    if (progress % 10 == 0) jobInfo.log(null, progress + 1 + "/" + totalSize);
                    progress++;
                }
            }
            
            jobInfo.log("Read Time(ms):" + countForRead + ": Compress Time(ms):" + countForCompress);
            //Swath块结束的位置
            swathIndex.endPtr = startPosition;
            indexList.Add(swathIndex);
            jobInfo.log("第" + (rangeIndex + 1) + "批数据解析完毕");
        }

        //将谱图进行压缩并且存储文件流中
        private void compressToFile(int num, Spectrum spectrum, SwathIndex swathIndex)
        {
            try
            {
                TempScan ts = new TempScan(num, parseRT(spectrum.scanList.scans[0]));
                compress(spectrum, ts);
                addToIndex(swathIndex, ts);
            }
            catch (Exception exception)
            {
                jobInfo.log(exception.Message);
            }
        }
    }
}