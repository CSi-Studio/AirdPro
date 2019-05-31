using Propro.Constants;
using Propro.Domains;
using Propro.Structs;
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Propro_Client.Domains.Aird;

namespace Propro.Logics
{
    internal class PRMConverter : IConverter
    {
        private int totalSize;//总计的谱图数目
        private long startPosition;//块索引的指针
        private int progress;//进度计数器
        List<TempIndex> ms1List = new List<TempIndex>();//用于存放MS1索引及基础信息
        Hashtable ms2Table = new Hashtable();//用于存放MS2的索引信息,key为mz

        public PRMConverter(ConvertJobInfo jobInfo) : base(jobInfo) {}

        public override void doConvert()
        {
            start();
            initDirectory();//创建文件夹
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile();//准备读取Vendor文件
                    wrapping();//Data Wrapping
                    initGlobalVar();//初始化全局变量
                    preProgress();//预处理谱图,将MS1和MS2谱图分开存储
                    doWithMS1Block();//处理MS1,并将索引写入文件流中
                    doWithMS2Block();//处理MS2,并将索引写入文件流中
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
        }

        private string getMsLevel(int index)
        {
            return spectrumList.spectrum(index).cvParamChild(CVID.MS_ms_level).value.ToString();
        }

        private TempIndex parseMS1(Spectrum spectrum, int index)
        {
            TempIndex ms1 = new TempIndex();
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

        private TempIndex parseMS2(Spectrum spectrum, int index, int parentIndex)
        {
            TempIndex ms2 = new TempIndex();
            ms2.level = 2;
            ms2.num = parentIndex;

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
            
            if (spectrum.scanList.scans.Count != 1) return ms2;
            Scan scan = spectrum.scanList.scans[0];
            ms2.rt = parseRT(scan);
            
            return ms2;
        }

        private void addToMS2Map(TempIndex ms2Index)
        {
            if (ms2Table.Contains(ms2Index.mz))
            {
                (ms2Table[ms2Index.mz] as List<TempIndex>).Add(ms2Index);
            }
            else
            {
                List<TempIndex> indexList = new List<TempIndex>();
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

        private void doWithMS1Block()
        {
            SwathIndex swathIndex = new SwathIndex();
            swathIndex.level = 1;
            swathIndex.startPtr = startPosition;
            
            for (int i = 0; i < ms1List.Count; i++)
            {
                jobInfo.log(null, "MS1:" + i + "/" + ms1List.Count);
                byte[] mzArrayBytes, intArrayBytes;
                TempIndex scanIndex = ms1List[i];
                compress(spectrumList.spectrum(scanIndex.num, true), out mzArrayBytes, out intArrayBytes);

                swathIndex.rts.Add(scanIndex.rt);
                swathIndex.nums.Add(scanIndex.num);
                swathIndex.mzs.Add(mzArrayBytes.Length);
                swathIndex.ints.Add(intArrayBytes.Length);

                startPosition = startPosition + mzArrayBytes.Length + intArrayBytes.Length;
                
                airdStream.Write(mzArrayBytes, 0, mzArrayBytes.Length);
                airdStream.Write(intArrayBytes, 0, intArrayBytes.Length);
            }

            indexList.Add(swathIndex);
        }

        private void doWithMS2Block()
        {
            jobInfo.log("Start Processing MS2 List");
            foreach (float key in ms2Table.Keys)
            {
                List<TempIndex> tempIndexList = ms2Table[key] as List<TempIndex>;
                //为每一个key组创建一个SwathBlock
                SwathIndex swathIndex = new SwathIndex();
                swathIndex.level = 2;
                swathIndex.startPtr = startPosition;
                
                //顺便创建一个WindowRanges,用以让Propro服务端快速获取全局的窗口数目和mz区间
                WindowRange range = new WindowRange(tempIndexList[0].mzStart, tempIndexList[0].mzEnd, key);
                swathIndex.range = range;
                ranges.Add(range);

                jobInfo.log(null, "MS2:" + progress + "/" + ms2Table.Keys.Count);
                progress++;
                foreach (TempIndex index in tempIndexList)
                {
                    byte[] mzArrayBytes, intArrayBytes;
                    compress(spectrumList.spectrum(index.num, true), out mzArrayBytes, out intArrayBytes);

                    swathIndex.rts.Add(index.rt);
                    swathIndex.mzs.Add(mzArrayBytes.Length);
                    swathIndex.ints.Add(intArrayBytes.Length);

                    startPosition = startPosition + mzArrayBytes.Length + intArrayBytes.Length;
                    
                    airdStream.Write(mzArrayBytes, 0, mzArrayBytes.Length);
                    airdStream.Write(intArrayBytes, 0, intArrayBytes.Length);
                }

                swathIndex.endPtr = startPosition;
                indexList.Add(swathIndex);
                jobInfo.log("MS2 Group Finished:" + progress + "/" + ms2Table.Keys.Count);
            }
        }

    }
}