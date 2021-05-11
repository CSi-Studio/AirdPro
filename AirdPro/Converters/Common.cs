/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections;
using System.IO;
using System.Threading.Tasks;
using AirdPro.Constants;
using AirdPro.Domains.Aird;
using AirdPro.Domains.Convert;

namespace AirdPro.Converters
{
    internal class Common : IConverter
    {
        private int progress; //进度计数器

        public Common(JobInfo jobInfo) : base(jobInfo)
        {
        }

        public override void doConvert()
        {
            start();
            initDirectory();
            using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
            {
                using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                {
                    readVendorFile(); //准备读取Vendor文件
                    initGlobalVar();
                    doProgress(); //开始逐帧解析数据
                    writeToAirdInfoFile(); //将Info数据写入文件
                }
            }

            finish();
        }

        private void initGlobalVar()
        {
            totalSize = spectrumList.size();
            progress = 0;
            jobInfo.log("Total Spectrums Size:" + totalSize);
            startPosition = 0;
        }

        //解析MS1和MS2谱图
        protected void doProgress()
        {
            var ms1Index = new BlockIndex(1);
            var ms2Index = new BlockIndex(2);
            jobInfo.log("Preprocessing:" + totalSize, "Preprocessing");
            if (jobInfo.jobParams.threadAccelerate)
            {
                var ms1Table = Hashtable.Synchronized(new Hashtable());
                var ms2Table = Hashtable.Synchronized(new Hashtable());
                //使用多线程处理数据提取与压缩
                Parallel.For(0, totalSize, (i, ParallelLoopState) =>
                {
                    jobInfo.log(null, progress + "/" + totalSize);
                    progress++;
                    var spectrum = spectrumList.spectrum(i, true);
                    if (spectrum.scanList.scans.Count != 1)
                    {
                        ParallelLoopState.Break();
                        return;
                    }
                   
                    var scan = spectrum.scanList.scans[0];
                    var ts = new TempScan(i, parseRT(scan));
                    if (jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = CV.trans(spectrum);
                    }
                    compress(spectrum, ts);
                    var msLevel = getMsLevel(spectrum);
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        ms1Table.Add(i, ts);
                    }else
                    {
                        ms2Table.Add(i, ts);
                    }
                });
              
                outputWithOrder(ms1Table, ms1Index);
                outputWithOrder(ms2Table, ms2Index);
            }
            else
            {
                for (var i = 0; i < totalSize; i++)
                {
                    var spectrum = spectrumList.spectrum(i, true);
                    if (spectrum.scanList.scans.Count != 1)
                    {
                        continue;
                    }
                    var scan = spectrum.scanList.scans[0];
                    var ts = new TempScan(i, parseRT(scan));
                    if (jobInfo.jobParams.includeCV)
                    {
                        ts.cvs = CV.trans(spectrum);
                    }
                    compress(spectrum, ts);
                    var msLevel = getMsLevel(spectrum);
                    if (msLevel.Equals(MsLevel.MS1))
                    {
                        addToIndex(ms1Index, ts);
                    }
                    else
                    {
                        addToIndex(ms2Index, ts);
                    }
                }
            }

            indexList.Add(ms1Index);
            indexList.Add(ms2Index);
        }

        protected new void outputWithOrder(Hashtable table, BlockIndex index)
        {
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                TempScan tempScan = (TempScan)table[key];
                addToIndex(index, tempScan);
            }
        }

        //注意:本函数会操作startPosition这个全局变量
        protected new void addToIndex(BlockIndex index, TempScan ts)
        {
            index.nums.Add(ts.num);
            index.rts.Add(ts.rt);
            index.mzs.Add(startPosition);
            index.ints.Add(startPosition + ts.mzArrayBytes.Length);
            index.cvList.Add(ts.cvs);
            startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
            index.endPtr = startPosition;
            airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
            airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
        }
    }
}