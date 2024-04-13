using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AirdPro.Domains;
using System.IO;
using System.Text;
using AirdPro.Algorithms.Compressor.Tdms;
using AirdPro.Constants;
using AirdSDK.Beans;
using AirdSDK.Compressor;
using NationalInstruments.Tdms;
using Newtonsoft.Json;
using Activator = AirdPro.Constants.Activator;

namespace AirdPro.Converters
{
    public class TdmsConverter : Converter
    {
        protected List<WindowRange> ranges = []; //SWATH/DIA Window的窗口
        protected List<BlockIndex> indexList = []; //用于存储的全局的SWATH List
        public List<MsIndex> ms1List = []; //用于存放MS1索引及基础信息,泛型为MsIndex

        public List<TdmsSpectrum> Spectra = [];

        // public 
        public TdmsComp TdmsComp;
        public long FirstRt;

        public override void Init(JobInfo jobInfo)
        {
            JobInfo = jobInfo;
            TdmsComp = new TdmsComp(jobInfo);
        }

        public override void InitCompressor()
        {
            throw new NotImplementedException();
        }

        public override void DoConvert()
        {
            try
            {
                Start();
                InitDirectory();

                using (AirdStream = new FileStream(JobInfo.airdFilePath, FileMode.Create))
                {
                    using (AirdJsonStream = new FileStream(JobInfo.airdJsonFilePath, FileMode.Create))
                    {
                        using (var tdms = new NationalInstruments.Tdms.File(JobInfo.inputPath))
                        {
                            tdms.Open();
                            ParseFirstRt(tdms);
                            Pretreatment(tdms);
                            CompressMs1Block();
                            WriteToAirdInfoFile();
                        }
                    }
                }


            }
            catch (Exception ee)
            {
                JobInfo.Log(ee.Message);
            }
            finally
            {
                Finish();
            }
        }

        //获取第一个RT时间的时间戳
        public void ParseFirstRt(NationalInstruments.Tdms.File tdms)
        {
            //获取第一帧的RT时间
            foreach (Group group in tdms)
            {
                foreach (Channel channel in group)
                {
                    foreach (var kv in channel.Properties)
                    {
                        if (kv.Key.Equals("TIMESTAMP"))
                        {
                            FirstRt = ((DateTime)kv.Value).Ticks;
                            return;
                        }
                    }
                }
            }
        }

        public void Pretreatment(NationalInstruments.Tdms.File tdms)
        {
            FileInfo info = new FileInfo(JobInfo.inputPath);
            int spectraCount = 0;
            JobInfo.Log(Tag.Pretreatment + TotalSpectraCount, Status.Pretreatment);

            int totalCount = 1;
            foreach (Group group in tdms)
            {
                spectraCount += group.Channels.Count / 2;
                int iter = 0;
                TdmsSpectrum spectrum = null;
                foreach (Channel channel in group)
                {
                    if (iter % 2 == 1)
                    {
                        MsIndex ms1 = ParseMs1(channel, totalCount);
                        ms1List.Add(ms1);
                        if (spectrum != null)
                        {
                            spectrum.intChannel = channel;
                            Spectra.Add(spectrum);
                        }

                        totalCount++;
                    }
                    else
                    {
                        spectrum = new TdmsSpectrum();
                        spectrum.mzChannel = channel;
                    }

                    iter++;
                    
                }
            }

            JobInfo.vendorFileSize = info.Length;
            TotalSpectraCount = spectraCount;

            JobInfo.Log(Tag.Effective_MS1_List_Size + ms1List.Count);
            JobInfo.Log(Tag.Start_Processing_MS1_List);
        }

        public double ParseRt(Channel channel)
        {
            foreach (var kv in channel.Properties)
            {
                if (kv.Key.Equals("TIMESTAMP"))
                {
                    return (((DateTime)kv.Value).Ticks - FirstRt) / TimeSpan.TicksPerMillisecond / 1000d;
                }
            }

            return -1;
        }

        public void CompressMs1Block()
        {
            BlockIndex index = new BlockIndex
            {
                level = 1,
                startPtr = StartPosition
            };
            TdmsComp.CompressMs1(this, index);
            index.endPtr = StartPosition;
            indexList.Add(index);
        }

        public MsIndex ParseMs1(Channel intChannel, int index)
        {
            MsIndex ms1 = new MsIndex
            {
                level = 1,
                num = index,
                rt = ParseRt(intChannel),
                msType = MSType.PROFILE,
                activator = Activator.UNKNOWN,
                energy = -1
            };

            return ms1;
        }

        public void WriteToFile(Hashtable table, BlockIndex index)
        {
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                AddToIndex(index, table[key]);
            }
        }

        public void AddToIndex(BlockIndex index, object tempScan)
        {
            TempScan ts = (TempScan)tempScan;

            index.nums.Add(ts.num);
            index.rts.Add(ts.rt);
            index.tics.Add(ts.tic);
            index.basePeakIntensities.Add(ts.basePeakIntensity);
            index.injectionTimes.Add(ts.injectionTime);
            index.basePeakMzs.Add(ts.basePeakMz);

            index.polarities.Add(ts.polarity);
            index.energies.Add(ts.energy);
            index.activators.Add(ts.activator);
            index.filterStrings.Add(ts.filterString);
            index.msTypes.Add(ts.msType);
            index.cvList.Add(ts.cvs);

            if (ts.mzArrayBytes != null && ts.intArrayBytes != null)
            {
                index.mzs.Add(ts.mzArrayBytes.Length);
                index.ints.Add(ts.intArrayBytes.Length);
                StartPosition = StartPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                AirdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                AirdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
            }

            if (ts.mobilityArrayBytes != null)
            {
                index.mobilities.Add(ts.mobilityArrayBytes.Length);
                StartPosition += ts.mobilityArrayBytes.Length;
                AirdStream.Write(ts.mobilityArrayBytes, 0, ts.mobilityArrayBytes.Length);
            }
        }

        public void WriteToAirdInfoFile()
        {
            JobInfo.Log(Tag.Write_Index_File, Status.Writing_Index_File);
            AirdInfo airdInfo = buildAirdInfo();

            if (JobInfo.config.compressedIndex)
            {
                List<BlockIndex> indexList = airdInfo.indexList;
                string indexListStr = JsonConvert.SerializeObject(indexList,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] indexListByte = new ZstdWrapper().encode(Encoding.Default.GetBytes(indexListStr));
                airdInfo.indexStartPtr = StartPosition;
                StartPosition += indexListByte.Length;
                airdInfo.indexEndPtr = StartPosition;
                airdInfo.indexList = null;
                AirdStream.Write(indexListByte, 0, indexListByte.Length);
            }

            string airdInfoStr = JsonConvert.SerializeObject(airdInfo,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            StartPosition += airdBytes.Length;
            AirdJsonStream.Write(airdBytes, 0, airdBytes.Length);
        }

        public AirdInfo buildAirdInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            List<Software> softwares = new List<Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();
            
            //Basic Job Info
            airdInfo.engine = JobInfo.config.engine;
            airdInfo.airdPath = JobInfo.airdFilePath;
            airdInfo.fileSize = JobInfo.vendorFileSize;
            airdInfo.createDate = DateTime.Now.ToString();
            airdInfo.type = JobInfo.type;
            // airdInfo.totalCount = msd.run.spectrumList.size();
            airdInfo.creator = JobInfo.config.creator;

            //Scan index and window range info
            airdInfo.rangeList = ranges;
            //Block index
            airdInfo.indexList = indexList;

            airdInfo.startTimeStamp = new DateTime(FirstRt).ToString();
            
            Software airdPro = new Software();
            airdPro.name = SoftwareInfo.NAME;
            airdPro.version = SoftwareInfo.VERSION;
            airdPro.type = "DataFormatConversion";
            softwares.Add(airdPro);
            airdInfo.softwares = softwares;
            
            List<Compressor> comps = new List<Compressor>();
            Compressor mzCompressor = new Compressor(Compressor.TARGET_MZ);
            Compressor intCompressor = new Compressor(Compressor.TARGET_INTENSITY);
            Compressor mobiCompressor = new Compressor(Compressor.TARGET_MOBILITY);
            
            mzCompressor.addMethod(JobInfo.config.mzIntComp.ToString());
            mzCompressor.addMethod(JobInfo.config.mzByteComp.ToString());
            mzCompressor.precision = JobInfo.config.mzPrecision;

            intCompressor.addMethod(JobInfo.config.intIntComp.ToString());
            intCompressor.addMethod(JobInfo.config.intByteComp.ToString());
            intCompressor.precision = TdmsComp.IntensityPrecision;

            mobiCompressor.addMethod(JobInfo.config.mobiIntComp.ToString());
            mobiCompressor.addMethod(JobInfo.config.mobiByteComp.ToString());
            mobiCompressor.precision = 10000000;
            
            comps.Add(mzCompressor);
            comps.Add(intCompressor);
            comps.Add(mobiCompressor);
            airdInfo.compressors = comps;
            
            airdInfo.ignoreZeroIntensityPoint = JobInfo.config.ignoreZeroIntensity;
            airdInfo.version = SoftwareInfo.VERSION;
            return airdInfo;
        }

        public void Finish()
        {
            Stopwatch.Stop();
            JobInfo.refreshReport = true;
            JobInfo.Log(Tag.Total_Time_Cost + Stopwatch.Elapsed.TotalSeconds, Status.Finished);
            JobInfo.SetStatus(ProcessingStatus.FINISHED);
        }
    }
}