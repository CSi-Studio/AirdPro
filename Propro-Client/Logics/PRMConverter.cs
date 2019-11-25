using Propro.Domains;
using Propro.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Propro.Constants;
using Propro_Client.Domains.Aird;

namespace Propro.Logics
{
    internal class PRMConverter : IConverter
    {
        private int progress;//进度计数器

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
                    initGlobalVar();//初始化全局变量
                    preProgress();//预处理谱图,将MS1和MS2谱图分开存储
                    parseAndStoreMS1Block();//处理MS1,并将索引写入文件流中
                    parseAndStoreMS2Block();//处理MS2,并将索引写入文件流中
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

        //Step1. 解析MS1和MS2谱图
        protected void preProgress()
        {
            int parentNum = 0;
            jobInfo.log("Preprocessing:" + totalSize, "Preprocessing");
            Parallel.For(0, totalSize, (i, ParallelLoopState) =>
            {
                jobInfo.log(null, (i + 1) + "/" + totalSize);
                //如果是最后一个谱图,那么单独判断
                if (i == totalSize - 1)
                {
                    //如果是MS1谱图,那么直接跳过
                    if (getMsLevel(i).Equals(MsLevel.MS1))
                    {
                        ParallelLoopState.Break();
                        return;
                    }
                    //如果是MS2谱图,加入到谱图组
                    if (getMsLevel(i).Equals(MsLevel.MS2))
                    {
                        addToMS2Map(parseMS2(spectrumList.spectrum(i), i, parentNum));
                        ParallelLoopState.Break();
                        return;
                    }
                }

                //如果这个谱图是MS1
                if (getMsLevel(i).Equals(MsLevel.MS1))
                {
                    //如果下一个谱图仍然是MS1, 那么直接忽略这个谱图
                    if (getMsLevel(i + 1).Equals(MsLevel.MS1))
                    {
                        ParallelLoopState.Break();
                        return;
                    }
                    if (getMsLevel(i + 1).Equals(MsLevel.MS2))
                    {
                        parentNum = i;
                        ms1List.Add(parseMS1(spectrumList.spectrum(i), i));
                    }
                }

                if (getMsLevel(i).Equals(MsLevel.MS2))
                {
                    addToMS2Map(parseMS2(spectrumList.spectrum(i), i, parentNum)); //如果这个谱图是MS2
                }
            });

            jobInfo.log("Effective MS1 List Size:" + ms1List.Count);
            jobInfo.log("MS2 Group List Size:" + ms2Table.Count);
            jobInfo.log("Start Processing MS1 List");
        }

        private void parseAndStoreMS2Block()
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

                if (jobInfo.threadAccelerate)
                {
                    Hashtable table = Hashtable.Synchronized(new Hashtable());
                    //使用多线程处理数据提取与压缩
                    Parallel.For(0, tempIndexList.Count, (i, ParallelLoopState) =>
                    {
                        TempIndex index = tempIndexList[i];
                        TempScan ts = new TempScan(index.pNum, index.rt);
                        compress(spectrumList.spectrum(index.num, true), ts);
                        //SwathIndex中只存储MS2谱图对应的MS1谱图的序号,其本身的序号已经没用了,不做存储,所以只存储了pNum
                        table.Add(i, ts);
                    });

                    outputWithOrder(table, swathIndex);
                }
                else
                {
                    foreach (TempIndex index in tempIndexList)
                    {
                        TempScan ts = new TempScan(index.pNum, index.rt);
                        compress(spectrumList.spectrum(index.num, true), ts);
                        //SwathIndex中只存储MS2谱图对应的MS1谱图的序号,其本身的序号已经没用了,不做存储
                        addToIndex(swathIndex, ts);
                    }
                }

                swathIndex.endPtr = startPosition;
                indexList.Add(swathIndex);
                jobInfo.log("MS2 Group Finished:" + progress + "/" + ms2Table.Keys.Count);
            }
        }

    }
}