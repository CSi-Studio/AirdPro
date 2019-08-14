using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using Propro_Client.Domains.Aird;
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace Propro.Logics
{
    public class ScanningSwathConvert : IConverter
    {
        private int totalSize;//总计的谱图数目
        private int progress;//进度计数器
        Hashtable rangeMap = Hashtable.Synchronized(new Hashtable());//用于存储TargetMZ-SwathIndex
        Hashtable ms2Map = Hashtable.Synchronized(new Hashtable());//用于存储TargetMZ-List<TempScan>

        public ScanningSwathConvert(ConvertJobInfo jobInfo) : base(jobInfo) { }

        public override void doConvert()
        {
            start();
            initDirectory();//创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile();//准备读取Vendor文件
                    initGlobalVar();//初始化全局变量
                    buildWindowsRanges(); //Getting SWATH Windows
                    coreLoop();//核心扫描解析逻辑
                    sortAndWrite();//整理最终的解析数据,并且写入文件
                    writeToAirdInfoFile();//将Info数据写入文件
                    clearCache();
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
        }

        /**
         * 循环搜索,直到出现第一个重复的MS窗口停止
         * 即便这样,如果在第一轮扫描中没有出现的窗口是无法被初始化的,需要在后续不断的作补充
         *
         */
        private void buildWindowsRanges()
        {
            jobInfo.log("Start getting windows", "Getting Windows");
            int i = 0;
            Spectrum spectrum = spectrumList.spectrum(0);
            while (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS2))
            {
                float mz = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_target_m_z);
                float lowerOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_lower_offset);
                float upperOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_upper_offset);

                if (rangeMap.Contains(mz))
                {
                    break;
                }

                WindowRange range = new WindowRange(mz - lowerOffset, mz + upperOffset, mz);
                SwathIndex swathIndex = new SwathIndex();
                swathIndex.range = range;
                rangeMap.Add(mz, swathIndex);
                ms2Map.Add(mz, ArrayList.Synchronized(new ArrayList()));

                i++;
                spectrum = spectrumList.spectrum(i);
            }
            jobInfo.log("Finished Getting Windows");
        }

        private void coreLoop()
        {
            jobInfo.log(null, progress + "/" + totalSize);
            Parallel.For(0, totalSize, (i, ParallelLoopState) =>
            {
                scan(i);
                if (progress % 100 == 0) jobInfo.log(null, progress + 1 + "/" + totalSize);
                progress++;
            });
        }

        private void sortAndWrite()
        {
            jobInfo.log("Writing Aird File", "writing file");
            ArrayList mzKeys = new ArrayList(rangeMap.Keys); //别忘了导入System.Collections
            mzKeys.Sort(); //按mz顺序进行排序
            foreach (float key in mzKeys)
            {
                SwathIndex swathIndex = (SwathIndex)rangeMap[key];
                ArrayList tempScanList = (ArrayList)ms2Map[key];
                swathIndex.level = 2;
                swathIndex.startPtr = startPosition;
                tempScanList.Sort();
                foreach (TempScan tempScan in tempScanList)
                {
                    addToIndex(swathIndex, tempScan);
                }

                swathIndex.endPtr = startPosition;
                indexList.Add(swathIndex);
                ranges.Add(swathIndex.range);
            }
        }

        private void scan(int i)
        {
            Spectrum spectrum = spectrumList.spectrum(i, true);
            //忽略所有MS1的谱图
            if (spectrum.cvParamChild(CVID.MS_ms_level).value.ToString().Equals(MsLevel.MS1))
            {
                return;
            }
            float targetMz = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_target_m_z);
            ArrayList ms2List = null;
            if (rangeMap.Contains(targetMz))
            {
                ms2List = (ArrayList) ms2Map[targetMz];
            }
            else
            {
                float mz = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_target_m_z);
                float lowerOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_lower_offset);
                float upperOffset = getPrecursorIsolationWindowParams(spectrum, CVID.MS_isolation_window_upper_offset);

                WindowRange range = new WindowRange(mz - lowerOffset, mz + upperOffset, mz);
                SwathIndex addIndex = new SwathIndex();
                addIndex.range = range;
                rangeMap.Add(targetMz, addIndex);
                ms2Map.Add(targetMz, ArrayList.Synchronized(new ArrayList()));
                ms2List = (ArrayList)ms2Map[targetMz];
            }

            if (spectrum.scanList.scans.Count != 1)
            {
                return;
            }
            try
            {
                TempScan ts = new TempScan(i, parseRT(spectrum.scanList.scans[0]));
                compress(spectrum, ts);
                ms2List.Add(ts);
            }
            catch (Exception exception)
            {
                jobInfo.log(exception.Message);
                return;
            }
        }

        private void clearCache()
        {
            rangeMap = Hashtable.Synchronized(new Hashtable());
            ms2Map = Hashtable.Synchronized(new Hashtable());

        }
       
    }
}