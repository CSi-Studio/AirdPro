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
    internal class TestLinearRead : IConverter
    {
        public override void doConvert(ConvertJobInfo jobInfo)
        {
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

                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        for (int i = 0; i < rangeSize; i++)
                        {
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
                                BinaryData mzData = spectrum.getMZArray().data;
                                BinaryData intData = spectrum.getIntensityArray().data;

                                List<double> mzList = new List<double>();
                                List<double> intensityList = new List<double>();
                                for (int t = 0; t < mzData.Count; t++)
                                {
                                    mzList.Add(mzData[t] );
                                    intensityList.Add(intData[t]);
                                }
                            }
                            stopwatch.Stop();
                            jobInfo.addLog("第" + (i + 1) + "批数据解析完毕,耗时:"+stopwatch.Elapsed.TotalSeconds);
                            stopwatch.Restart();
                        }
                        stopwatch.Stop();
                        jobInfo.progress.Report("Finished");
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