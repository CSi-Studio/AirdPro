using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using AirdPro.Converters;
using AirdPro.Domains.Aird;
using AirdPro.Domains.Convert;
using pwiz.CLI.msdata;

namespace AirdPro.Algorithms
{
    public class ZDPD :IZDPD
    {
        public ZDPD(IConverter converter) : base(converter) {}

        override 
        public void compressMS1(BlockIndex index)
        {
            if (converter.jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                
                //使用多线程处理数据提取与压缩
                Parallel.For(0, converter.ms1List.Count, (i, ParallelLoopState) =>
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + converter.ms1List.Count);

                    TempIndex scanIndex = converter.ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt);
                    converter.compress(converter.spectrumList.spectrum(scanIndex.num, true), ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                for (int i = 0; i < converter.ms1List.Count; i++)
                {
                    converter.jobInfo.log(null, "MS1:" + i + "/" + converter.ms1List.Count);
                    TempIndex scanIndex = converter.ms1List[i];
                    TempScan ts = new TempScan(scanIndex.num, scanIndex.rt);
                    converter.compress(converter.spectrumList.spectrum(scanIndex.num, true), ts);
                    converter.addToIndex(index, ts);
                }
            }
           
        }

        override
        public void compressMS2(List<TempIndex> tempIndexList, BlockIndex index)
        {
            if (converter.jobInfo.jobParams.threadAccelerate)
            {
                Hashtable table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, tempIndexList.Count, (i, ParallelLoopState) =>
                {
                    TempIndex tempIndex = tempIndexList[i];
                    TempScan ts = new TempScan(tempIndex.num, tempIndex.rt);
                    converter.compress(converter.spectrumList.spectrum(tempIndex.num, true), ts);
                    table.Add(i, ts);
                });
                converter.outputWithOrder(table, index);
            }
            else
            {
                foreach (TempIndex tempIndex in tempIndexList)
                {
                    TempScan ts = new TempScan(tempIndex.num, tempIndex.rt);
                    converter.compress(converter.spectrumList.spectrum(tempIndex.num, true), ts);
                    converter.addToIndex(index, ts);
                }
            }
        }
    }
}