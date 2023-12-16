using System;
using System.Collections;
using System.Collections.Generic;
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
    public class TdmsConverter : IConverter
    {
        protected List<WindowRange> ranges = new List<WindowRange>(); //SWATH/DIA Window的窗口
        protected List<BlockIndex> indexList = new List<BlockIndex>(); //用于存储的全局的SWATH List
        public List<MsIndex> ms1List = new List<MsIndex>(); //用于存放MS1索引及基础信息,泛型为MsIndex
        protected Hashtable featuresMap = new Hashtable();

        public List<TdmsSpectrum> spectra = new List<TdmsSpectrum>();

        // public 
        public TdmsComp tdmsComp;
        public long firstRT;

        public TdmsConverter()
        {
        }

        public override void init(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            tdmsComp = new TdmsComp(jobInfo);
        }

        public override void doConvert()
        {
            try
            {
                start();
                initDirectory();
                long size = 0;

                using (airdStream = new FileStream(jobInfo.airdFilePath, FileMode.Create))
                {
                    using (airdJsonStream = new FileStream(jobInfo.airdJsonFilePath, FileMode.Create))
                    {
                        using (var tdms = new NationalInstruments.Tdms.File(jobInfo.inputPath))
                        {
                            tdms.Open();
                            parseFirstRT(tdms);
                            pretreatment(tdms);
                            compressMS1Block();
                            writeToAirdInfoFile();
                        }
                    }
                }


            }
            catch (Exception ee)
            {
                jobInfo.log(ee.Message);
            }
            finally
            {
                finish();
            }
        }

        //获取第一个RT时间的时间戳
        public void parseFirstRT(NationalInstruments.Tdms.File tdms)
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
                            firstRT = ((DateTime)kv.Value).Ticks;
                            return;
                        }
                    }
                }
            }
        }

        public void pretreatment(NationalInstruments.Tdms.File tdms)
        {
            FileInfo info = new FileInfo(jobInfo.inputPath);
            int spectraCount = 0;
            jobInfo.log(Tag.Pretreatment + totalSize, Status.Pretreatment);

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
                        MsIndex ms1 = parseMS1(channel, totalCount);
                        ms1List.Add(ms1);
                        spectrum.intChannel = channel;
                        spectra.Add(spectrum);
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

            fileSize = info.Length;
            totalSize = spectraCount;

            jobInfo.log(Tag.Effective_MS1_List_Size + ms1List.Count);
            jobInfo.log(Tag.Start_Processing_MS1_List);
        }

        public double parseRT(Channel channel)
        {
            foreach (var kv in channel.Properties)
            {
                if (kv.Key.Equals("TIMESTAMP"))
                {
                    return (((DateTime)kv.Value).Ticks - firstRT) / TimeSpan.TicksPerMillisecond / 1000d;
                }
            }

            return -1;
        }

        public void compressMS1Block()
        {
            BlockIndex index = new BlockIndex();
            index.level = 1;
            index.startPtr = startPosition;
            tdmsComp.compressMS1(this, index);
            index.endPtr = startPosition;
            indexList.Add(index);
        }

        public MsIndex parseMS1(Channel intChannel, int index)
        {
            MsIndex ms1 = new MsIndex();
            ms1.level = 1;
            ms1.num = index;
            ms1.rt = parseRT(intChannel);

            ms1.msType = MSType.PROFILE;
            ms1.activator = Activator.UNKNOWN;
            ms1.energy = -1;
            return ms1;
        }

        public void writeToFile(Hashtable table, BlockIndex index)
        {
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();
            foreach (int key in keys)
            {
                addToIndex(index, table[key]);
            }
        }

        public void addToIndex(BlockIndex index, object tempScan)
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
                startPosition = startPosition + ts.mzArrayBytes.Length + ts.intArrayBytes.Length;
                airdStream.Write(ts.mzArrayBytes, 0, ts.mzArrayBytes.Length);
                airdStream.Write(ts.intArrayBytes, 0, ts.intArrayBytes.Length);
            }

            if (ts.mobilityArrayBytes != null)
            {
                index.mobilities.Add(ts.mobilityArrayBytes.Length);
                startPosition += ts.mobilityArrayBytes.Length;
                airdStream.Write(ts.mobilityArrayBytes, 0, ts.mobilityArrayBytes.Length);
            }
        }

        public void writeToAirdInfoFile()
        {
            jobInfo.log(Tag.Write_Index_File, Status.Writing_Index_File);
            AirdInfo airdInfo = buildAirdInfo();

            if (jobInfo.config.compressedIndex)
            {
                List<BlockIndex> indexList = airdInfo.indexList;
                string indexListStr = JsonConvert.SerializeObject(indexList,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] indexListByte = new ZstdWrapper().encode(Encoding.Default.GetBytes(indexListStr));
                airdInfo.indexStartPtr = startPosition;
                startPosition += indexListByte.Length;
                airdInfo.indexEndPtr = startPosition;
                airdInfo.indexList = null;
                airdStream.Write(indexListByte, 0, indexListByte.Length);
            }

            string airdInfoStr = JsonConvert.SerializeObject(airdInfo,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            byte[] airdBytes = Encoding.Default.GetBytes(airdInfoStr);
            startPosition += airdBytes.Length;
            airdJsonStream.Write(airdBytes, 0, airdBytes.Length);
        }

        public AirdInfo buildAirdInfo()
        {
            AirdInfo airdInfo = new AirdInfo();
            List<Software> softwares = new List<Software>();
            List<ParentFile> parentFiles = new List<ParentFile>();
            
            //Basic Job Info
            airdInfo.scene = jobInfo.config.scene;
            airdInfo.airdPath = jobInfo.airdFilePath;
            airdInfo.fileSize = fileSize;
            airdInfo.createDate = DateTime.Now.ToString();
            airdInfo.type = jobInfo.type;
            // airdInfo.totalCount = msd.run.spectrumList.size();
            airdInfo.creator = jobInfo.config.creator;

            //Scan index and window range info
            airdInfo.rangeList = ranges;
            //Block index
            airdInfo.indexList = indexList;

            airdInfo.startTimeStamp = new DateTime(firstRT).ToString();
            
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
            
            mzCompressor.addMethod(jobInfo.config.mzIntComp.ToString());
            mzCompressor.addMethod(jobInfo.config.mzByteComp.ToString());
            mzCompressor.precision = jobInfo.config.mzPrecision;

            intCompressor.addMethod(jobInfo.config.intIntComp.ToString());
            intCompressor.addMethod(jobInfo.config.intByteComp.ToString());
            intCompressor.precision = tdmsComp.intensityPrecision;

            mobiCompressor.addMethod(jobInfo.config.mobiIntComp.ToString());
            mobiCompressor.addMethod(jobInfo.config.mobiByteComp.ToString());
            mobiCompressor.precision = 10000000;
            
            comps.Add(mzCompressor);
            comps.Add(intCompressor);
            comps.Add(mobiCompressor);
            airdInfo.compressors = comps;
            
            airdInfo.ignoreZeroIntensityPoint = jobInfo.config.ignoreZeroIntensity;
            airdInfo.version = SoftwareInfo.VERSION;
            return airdInfo;
        }

        public void finish()
        {
            stopwatch.Stop();
            jobInfo.refreshReport = true;
            jobInfo.log(Tag.Total_Time_Cost + stopwatch.Elapsed.TotalSeconds, Status.Finished);
            jobInfo.setStatus(ProcessingStatus.FINISHED);
        }
    }
}