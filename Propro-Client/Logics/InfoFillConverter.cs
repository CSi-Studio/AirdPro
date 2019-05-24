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
    internal class FillInfoConverter : IConverter
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
          
            foreach (MSData msd in msdList)
            {
                jobInfo.progress.Report("Wrapping");
                jobInfo.addLog("Start Filling Information");
                
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

                if (File.Exists(jobInfo.airdJsonFilePath))
                {
                   
                    string json = File.ReadAllText(jobInfo.airdJsonFilePath);
                    AirdInfo info = JsonConvert.DeserializeObject<AirdInfo>(json);
                    info.rawId = msd.id;
                    info.rangeList = ranges;
                    JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                    string airdInfoStr = JsonConvert.SerializeObject(info, jsonSetting);
                    byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
                    using (FileStream airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                    {
                        airdJsonStream.Write(airdBytes, 0, airdBytes.Length);
                    }
                }
            }

            stopwatch.Stop();
            jobInfo.progress.Report("Finished");
            jobInfo.addLog("总耗时:" + stopwatch.Elapsed.TotalSeconds);

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