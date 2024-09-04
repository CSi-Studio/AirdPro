using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql;
using AirdPro.IMSRawDataCompress.datamodel;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel;
using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using static AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.TDFLibrary;
using AirdPro.IMSRawDataCompress.datamodel.enums;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf
{   
    public class TDFImportTask
    {
        long error;
        string message;

        public static int BUFFER_SIZE = 300000; // start with 300 kb of buffer size
        public static int BUFFER_SIZE_INCREMENT = 100_000; // 100 kb increase each time we fail
        public static int SCAN_PACKAGE_SIZE = 5000;
        public static int TRY_TIMES = 5;

        private string fileName; //bruker fileName, 是一个文件夹
        private FileInfo tdf, tdfBin;
        private TDFMetaDataTable metaDataTable;
        private TDFFrameTable frameTable;
        private TDFPrecursorTable precursorTable;
        private TDFPasefFrameMsMsInfoTable pasefFrameMsMsInfoTable;
        private TDFFrameMsMsInfoTable frameMsMsInfoTable;
        private FramePrecursorTable framePrecursorTable;
        //private PrmFrameTargetTable prmFrameTargetTable;
        //private TDFMaldiFrameInfoTable maldiFrameInfoTable;
        //private TDFMaldiFrameLaserInfoTable maldiFrameLaserInfoTable;
        //private TimsData timsData;
        private bool isMaldi;
        private int loadedFrames;

        private static readonly object LockObject = new();
        
        private List<string> messages = new();
        public List<string> Messages
        {
            get { return messages; }
        }

        private bool showDetail = true;
        public bool ShowDetail
        {
            get { return showDetail; }
            set { showDetail = value; }
        }

        public void Run(string fileName)
        {
            this.fileName = fileName;
            messages.Clear();
            //获取tdf和tdfBin
            // 检查文件夹是否存在
            if (!Directory.Exists(fileName)) 
            {
                message = $"The directory '{fileName}' does not exist.";
                messages.Add(message);
                throw new DirectoryNotFoundException(message);
            }
            // 定义文件过滤器
            string tdfFilter = Path.Combine(fileName, "*.tdf");
            string tdfBinFilter = Path.Combine(fileName, "*.tdf_bin");
            // 获取匹配的文件
            tdf = Directory.EnumerateFiles(fileName, "*.tdf").Select(f => new FileInfo(f)).FirstOrDefault();
            tdfBin = Directory.EnumerateFiles(fileName, "*.tdf_bin").Select(f => new FileInfo(f)).FirstOrDefault();
            // 检查是否找到了文件
            if (tdf == null || tdfBin == null)
            {
                message = "Could not find both .tdf and .tdf_bin files in the specified directory.";
                messages.Add(message);
                throw new FileNotFoundException(message);
            } 
            message = "Selected tdf file: " + tdf.FullName;
            messages.Add(message);
            message = "Selected tdf_bin file: " + tdfBin.FullName;
            messages.Add(message);

            //创建表对象
            //元数据
            metaDataTable = new TDFMetaDataTable();
            //Frame
            frameTable = new TDFFrameTable();
            //PASEF
            precursorTable = new TDFPrecursorTable();
            pasefFrameMsMsInfoTable = new TDFPasefFrameMsMsInfoTable();
            //DIA
            framePrecursorTable = new FramePrecursorTable();
            frameMsMsInfoTable = new TDFFrameMsMsInfoTable();            
            //PRM
            //prmFrameTargetTable = new PrmFrameTargetTable();
            //MALDI
            /*maldiFrameInfoTable = new TDFMaldiFrameInfoTable();
            maldiFrameLaserInfoTable = new TDFMaldiFrameLaserInfoTable();*/
            isMaldi = false;

            //import data from tdf
            ReadMetadata();
            if (isMaldi)
            {
                // to do...
            }

            //timsData = new TimsData();
            //timsData.AcquisitionDateTime = (DateTime)metaDataTable.GetAcquisitionDateTime();

            //import data from tdf_bin
            ReadBinData();
        }

        private void ReadMetadata()
        {
            messages.Add("Initializing SQL ...");
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={tdf.FullName};Version=3;"))
                {
                    messages.Add($"Establishing SQL connection to {tdf.Name}");
                    lock (typeof(SQLiteConnection))
                    {
                        try
                        {
                            connection.Open();

                            //reading metadata
                            messages.Add($"Reading metadata from {metaDataTable.GetTableName()} ...");
                            metaDataTable.ExecuteQuery(connection);
                            messages.Add("fetched records: " + metaDataTable.GetKeyColumn().Values.Count());
                            //ShowTableData(metaDataTable);

                            //reading simpleFrame data
                            messages.Add($"Reading metadata from {frameTable.GetTableName()} ...");
                            frameTable.ExecuteQuery(connection);
                            messages.Add($"fetched records: {frameTable.GetKeyColumn().Values.Count()}");
                            isMaldi = frameTable.GetScanModeColumn().Values.Contains(BrukerScanMode.MALDI.Num);

                            //reading precursor info
                            messages.Add($"Reading precursor info from {precursorTable.GetTableName()} ...");
                            precursorTable.ExecuteQuery(connection);
                            messages.Add($"fetched records: {precursorTable.GetKeyColumn().Values.Count()}");

                            //reading MS/MS-Precursor info
                            messages.Add($"Reading precursor info from {framePrecursorTable.GetTableName()} ...");
                            framePrecursorTable.ExecuteQuery(connection);
                            messages.Add($"fetched records: {framePrecursorTable.GetKeyColumn().Values.Count()}");

                            //reading PRM Target info

                            if (isMaldi)
                            {
                                // to do。。。
                            }
                        }
                        catch (Exception ex)
                        {
                            messages.Add(ex.ToString());
                        }
                        finally
                        {
                            connection.Close();                            
                        }
                        messages.Add ("Metadata read successfully!");
                    }
                }
            }
            catch (Exception e)
            {
                messages.Add(e.ToString());
            }
        }

        private void ReadBinData()
        {
            TDFUtils tdfUtils = new();
            messages.Add($"open tdf_bin file {fileName}");
            long handle = tdfUtils.OpenFile(fileName);
            if (handle == 0)
            {
                messages.Add($"open tdf_bin file {fileName} failed!");
                return;
            }
                        
            loadedFrames = 0;
            // collect average spectra for each frame#######
            List<Frame> frameList = new();
            bool importProfile = false;
            try
            {
                List<long> frameIdList = frameTable.GetFrameIdColumn().GetValueList();
                int numFrames = frameIdList.Count;
                messages.Add($"Total {numFrames} frames, starting frame import.");

                //for test: 读frameId为19的数据
                //int readFrameCount = numFrames > 100 ? 100 : numFrames;               
                //long[] testFrameIds = [2,18,19];
                long[] testFrameIds = [19];
                for (int i = 0; i < testFrameIds.Length; i++)
                {                    
                    int index = frameIdList.IndexOf(testFrameIds[i]);
                    long frameId = frameTable.GetFrameIdColumn().GetValueList()[index];
                    double rt = frameTable.GetTimeColumn().GetValueList()[index]; // rt，单位：秒
                    long numScans = frameTable.GetNumScansColumn().GetValueList()[index]; //910，固定值
                    long numPeaks = frameTable.GetNumPeaksColumn().GetValueList()[index];  //mz、intensity、mobility数组大小
                    double maxIntensity = frameTable.GetMaxIntensityColumn().GetValueList()[index];
                    double summedIntensity = frameTable.GetSummedIntensityColumn().GetValueList()[index];

                    //Range<double> mzRange = metaDataTable.GetMzRange();

                    //frame，frameId = 19
                    CentroidData centroidData = null;
                    double[] mobilities = null;
                    //Give a callback impl, which will be called by DLL function and DLL function will pass values to the first 4 parameters
                    CentroidCallback callback = new CentroidCallback((precursorId, numPeaks, pMz, pIntensities, userData) =>
                    {
                        centroidData = new CentroidData(precursorId, numPeaks, pMz, pIntensities);
                    });
                    lock (LockObject)
                    {
                        //mz 和intensity
                        long error = TDFLibrary.tims_extract_centroided_spectrum_for_frame_v2(handle, frameId, 0, numScans, callback, IntPtr.Zero);
                        if (error == 0)
                        {
                            messages.Add($"Could not extract centroid scan from frame {frameId} for scans 0 to {numScans}");
                            throw new Exception(message);
                        }
                        //mobility
                        double[] scanNum = CreatePopulatedArrayFrom1(numScans);
                        messages.Add($"scanNums size：{scanNum.Length}");
                        mobilities = tdfUtils.ConvertScanNumsToMobilities(handle, frameId, scanNum);
                    }

                    Frame frame = new Frame(frameId);
                    frame.centroidData = centroidData;
                    frame.mzArray = centroidData.Mzs;
                    frame.intensityArray = centroidData.Intensities;
                    frame.mobilityArray = mobilities;
                    frameList.Add(frame);
                    messages.Add($"frameId: {frameId}, rt: {rt}, numPeaks: {numPeaks}");

                    //spectrum1
                    CentroidData centroidData1 = null;
                    double[] mobilities1 = null;
                    long scanNumBegin = 0;
                    long scanNumEnd = 667;
                    long numPeaks1 = 40;
                    //Give a callback impl, which will be called by DLL function and DLL function will pass values to the first 4 parameters
                    CentroidCallback callback1 = new CentroidCallback((precursorId, numPeaks1, pMz, pIntensities, userData) =>
                    {
                        centroidData1 = new CentroidData(precursorId, numPeaks1, pMz, pIntensities);
                    });
                    lock (LockObject)
                    {
                        //mz 和intensity
                        long error1 = TDFLibrary.tims_extract_centroided_spectrum_for_frame_v2(handle, frameId, scanNumBegin, scanNumEnd, callback1, IntPtr.Zero);
                        if (error1 == 0)
                        {
                            messages.Add($"Could not extract centroid scan from frame {frameId} for scans {scanNumBegin} to {scanNumEnd}");
                            throw new Exception(message);
                        }
                        //mobility
                        double[] scanNum = CreatePopulatedArrayFrom1(scanNumEnd);
                        messages.Add($"scanNums size：{scanNum.Length}");
                        mobilities1 = tdfUtils.ConvertScanNumsToMobilities(handle, frameId, scanNum);
                    }

                    Frame spectrum1 = new Frame(frameId);
                    spectrum1.centroidData = centroidData1;
                    spectrum1.mzArray = centroidData1.Mzs;
                    spectrum1.intensityArray = centroidData1.Intensities;
                    spectrum1.mobilityArray = mobilities1;
                    frameList.Add(spectrum1);
                    messages.Add($"spectrum1, frameId: {frameId}, rt: {rt}, scanNumBegin: {scanNumBegin}, scanNumEnd: {scanNumEnd}, numPeaks: {numPeaks1}");

                    //spectrum2
                    CentroidData centroidData2 = null;
                    double[] mobilities2 = null;
                    scanNumBegin = 668;
                    scanNumEnd = 910;
                    long numPeaks2 = 19;
                    //Give a callback impl, which will be called by DLL function and DLL function will pass values to the first 4 parameters
                    CentroidCallback callback2 = new CentroidCallback((precursorId, numPeaks2, pMz, pIntensities, userData) =>
                    {
                        centroidData2 = new CentroidData(precursorId, numPeaks2, pMz, pIntensities);
                    });
                    lock (LockObject)
                    {
                        //mz 和intensity
                        long error2 = TDFLibrary.tims_extract_centroided_spectrum_for_frame_v2(handle, frameId, scanNumBegin, scanNumEnd, callback2, IntPtr.Zero);
                        if (error2 == 0)
                        {
                            messages.Add($"Could not extract centroid scan from frame {frameId} for scans {scanNumBegin} to {scanNumEnd}");
                            throw new Exception(message);
                        }
                        //mobility
                        double[] scanNum = CreatePopulatedArrayFromScanRange(scanNumBegin, scanNumEnd);
                        messages.Add($"scanNums size：{scanNum.Length}");
                        mobilities2 = tdfUtils.ConvertScanNumsToMobilities(handle, frameId, scanNum);
                    }

                    Frame spectrum2 = new Frame(frameId);
                    spectrum2.centroidData = centroidData2;
                    spectrum2.mzArray = centroidData2.Mzs;
                    spectrum2.intensityArray = centroidData2.Intensities;
                    spectrum2.mobilityArray = mobilities2;
                    frameList.Add(spectrum2);
                    messages.Add($"spectrum2, frameId: {frameId}, rt: {rt}, scanNumBegin: {scanNumBegin}, scanNumEnd: {scanNumEnd}, numPeaks: {numPeaks2}");

                    //获取每个scan对应的 mz[] & intensity[]
                    List<BuildingMobilityScan> spectra = LoadSpectraForFrame(handle, frameId, numScans);
                    frame.mobilityScans = spectra;

                    //利用spectra手动获取所有不同的 mz/intensity/mobility 组合，see if 组合数量=num of peaks ?
                    var distinctMonoSpectrumList = new List<SpectrumData>(); //仅考虑mz
                    var distinctBiSpectrumList = new List<SpectrumData>(); //考虑mz + intensity
                    var distinctTriSpectrumList = new List<SpectrumData>(); //考虑mz + intensity + mobility

                    var distinctSpectrumList0_667 = new List<SpectrumData>();
                    var distinctSpectrumList668_910 = new List<SpectrumData>();
                    for (int jj = 0; jj < spectra.Count; jj++)  
                    {
                        BuildingMobilityScan spec = spectra[jj];
                        for (int k = 0; k < spec.MzValues.Length; k++) 
                        {
                            if (!distinctMonoSpectrumList.Any(spectrum =>
                            Math.Abs(spectrum.Mz - spec.MzValues[k]) < 0.015))
                            {
                                distinctMonoSpectrumList.Add(new SpectrumData(spec.MzValues[k], spec.IntensityValues[k], mobilities[spec.ScanNumber]));
                            }

                            if (!distinctBiSpectrumList.Any(spectrum =>
                            Math.Abs(spectrum.Mz - spec.MzValues[k]) < 0.015 &&
                            Math.Abs(spectrum.Intensity - spec.IntensityValues[k]) < 0.0001))
                            {
                                distinctBiSpectrumList.Add(new SpectrumData(spec.MzValues[k], spec.IntensityValues[k], mobilities[spec.ScanNumber]));
                            }

                            if (!distinctTriSpectrumList.Any(spectrum =>
                            Math.Abs(spectrum.Mz - spec.MzValues[k]) < 0.015 &&
                            Math.Abs(spectrum.Intensity - spec.IntensityValues[k]) < 0.0001 &&
                            Math.Abs(spectrum.Mobility - mobilities[spec.ScanNumber]) < 0.0001))
                            {
                                distinctTriSpectrumList.Add(new SpectrumData(spec.MzValues[k], spec.IntensityValues[k], mobilities[spec.ScanNumber]));
                            }

                            if (jj < 667)
                            {
                                if (!distinctSpectrumList0_667.Any(spectrum =>
                                    Math.Abs(spectrum.Mz - spec.MzValues[k]) < 0.015 &&
                                    Math.Abs(spectrum.Intensity - spec.IntensityValues[k]) < 0.0001 &&
                                    Math.Abs(spectrum.Mobility - mobilities[spec.ScanNumber]) < 0.0001))
                                {
                                    distinctSpectrumList0_667.Add(new SpectrumData(spec.MzValues[k], spec.IntensityValues[k], mobilities[spec.ScanNumber]));
                                }
                            }
                            else
                            {
                                if (!distinctSpectrumList668_910.Any(spectrum =>
                                    Math.Abs(spectrum.Mz - spec.MzValues[k]) < 0.015 &&
                                    Math.Abs(spectrum.Intensity - spec.IntensityValues[k]) < 0.0001 &&
                                    Math.Abs(spectrum.Mobility - mobilities[spec.ScanNumber]) < 0.0001))
                                {
                                    distinctSpectrumList668_910.Add(new SpectrumData(spec.MzValues[k], spec.IntensityValues[k], mobilities[spec.ScanNumber]));
                                }
                            }
                        }
                    }

                    message = $"MobilityScan list size: {spectra.Count}, distinctBiSpectrumList size: {distinctBiSpectrumList.Count}, distinctTriSpectrumList size: {distinctTriSpectrumList.Count}, distinctSpectrumList0_667 size: {distinctSpectrumList0_667.Count}, distinctSpectrumList668_910 size: {distinctSpectrumList668_910.Count}";
                    messages.Add(message);
                    message = "distinctBiSpectrumList(Mz, Intenstiry, Mobility): " + string.Join(", ", distinctBiSpectrumList.Select(sd => $"({sd.Mz}, {sd.Intensity}, {sd.Mobility})"));
                    messages.Add(message);
                    message = "distinctTriSpectrumList(Mz, Intenstiry, Mobility): " + string.Join(", ", distinctTriSpectrumList.Select(sd => $"({sd.Mz}, {sd.Intensity}, {sd.Mobility})"));
                    messages.Add(message);
                    message = "distinctSpectrumList0_667(Mz, Intenstiry, Mobility): " + string.Join(", ", distinctSpectrumList0_667.Select(sd => $"({sd.Mz}, {sd.Intensity}, {sd.Mobility})"));
                    messages.Add(message);
                    message = "distinctSpectrumList668_910(Mz, Intenstiry, Mobility): " + string.Join(", ", distinctSpectrumList668_910.Select(sd => $"({sd.Mz}, {sd.Intensity}, {sd.Mobility})"));
                    messages.Add(message);
                }                
            }
            catch (Exception e)
            {
                messages.Add(e.Message);
                throw e;
            }
            finally
            {
                tdfUtils.Close();
            }

            //show first 5 frame
            //ShowSomeFrames();
            ShowTestFrames(frameList);
        }

        private List<BuildingMobilityScan> LoadSpectraForFrame(long handle, long frameId, long totalNumScans)
        {
            List<BuildingMobilityScan> spectra = new List<BuildingMobilityScan>((int)totalNumScans);
            Dictionary<int, double> indexToMzBuffer = new Dictionary<int, double>();

            byte[] buffer = new byte[BUFFER_SIZE];
            long start = 0;
            int tryTimes = 0;
            int scanIndex = 0;
            while (start < totalNumScans)
            {
                long end = Math.Min((start + SCAN_PACKAGE_SIZE), totalNumScans);
                int numScans = (int)(end - start);

                lock (LockObject)
                {
                    long error = TDFLibrary.tims_read_scans_v2(handle, frameId, start, end, buffer, buffer.Length);
                    // check if the buffer size was enough
                    if (error == 0)
                    {
                        tryTimes++;
                        BUFFER_SIZE += BUFFER_SIZE_INCREMENT;
                        message = $"Could not read scans {start}-{end} for frame {frameId}. Increasing buffer size to {BUFFER_SIZE} and reloading.";
                        messages.Add(message);
                        if (tryTimes >= TRY_TIMES)
                        {
                            throw new Exception(message);
                        }
                        else 
                        {
                            buffer = new byte[BUFFER_SIZE];
                            continue; // try again
                        }
                    }
                }

                start = start + SCAN_PACKAGE_SIZE;

                //将 byte[] 转换为 int[]
                if (buffer.Length % 4 != 0)
                {
                    throw new ArgumentException("Byte buffer length must be a multiple of 4.", nameof(buffer));
                }
                int[] scanBuffer = new int[buffer.Length / 4];
                for (int i = 0; i < scanBuffer.Length; i++)
                {
                    scanBuffer[i] = BitConverter.ToInt32(buffer, i * 4);
                }

                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(scanBuffer);  // 如果不是小端，需要反转数组中的每个整数
                }

                Array.Clear(buffer, 0, buffer.Length);

                int d = numScans;
                for (int i = 0; i < numScans; i++)
                {
                    int numPeaks = scanBuffer[i];
                    int[] indices = new int[numPeaks];
                    Array.Copy(scanBuffer, d, indices, 0, numPeaks);
                    d += numPeaks;
                    double[] intensities =new double[numPeaks];
                    Array.Copy(scanBuffer, d, intensities, 0, numPeaks);
                    d += numPeaks;

                    Dictionary<int, int> indicesToIndexMap = new Dictionary<int, int>();
                    List<double> unknownIndices = null;
                    double[] mzs = new double[indices.Length];
                    lock (LockObject)
                    {
                        //indicesToIndexMap.Clear();
                        for (int j = 0; j < indices.Length; j++)
                        {
                            double mz;
                            if (indexToMzBuffer.TryGetValue(indices[j], out mz))
                            {
                                mzs[j] = mz;
                            }
                            else
                            {
                                if (unknownIndices == null)
                                {
                                    unknownIndices = new List<double>(indices.Length / 2);
                                }
                                indicesToIndexMap[indices[j]] = j;
                                unknownIndices.Add(indices[j]);
                            }
                        }

                        if (unknownIndices != null)
                        {
                            double[] doubleBuffer = new double[unknownIndices.Count];
                            long error = TDFLibrary.tims_index_to_mz(handle, frameId, unknownIndices.ToArray(), doubleBuffer, unknownIndices.Count);

                            if (error == 0)
                            {
                                message = $"Could not convert indices to mzs for frameid {frameId} scan_index {i}";
                                messages.Add(message);
                            }

                            for (int k = 0; k < unknownIndices.Count; k++)
                            {
                                int peakIndex = (int)unknownIndices[k];
                                indexToMzBuffer[peakIndex] = doubleBuffer[k]; //方便后续直接取，能取到则不用调用库接口方法取
                                mzs[indicesToIndexMap[peakIndex]] = doubleBuffer[k];
                            }
                        }
                    }

                    //
                    spectra.Add(new BuildingMobilityScan(scanIndex, mzs, intensities));
                    scanIndex++;
                }
            }

            return spectra;
        }

        private double[] CreatePopulatedArrayFromScanRange(long scanNumBegin, long scanNumEnd)
        {
            long numScans = scanNumEnd - scanNumBegin + 1;
            double[] scanNum = new double[numScans];
            for (long i = 0; i < numScans; i++)
            {
                scanNum[i] = i + scanNumBegin;
            }
            return scanNum;
        }

        private void ShowTestFrames(List<Frame> frameList)
        {
            messages.Add($"test frame count: {frameList.Count}");
            for (int i = 0; i < frameList.Count; i++)
            {                                
                messages.Add($"frameId: {frameList[i].FrameId}--------------");
                messages.Add($"mzArray size: {frameList[i].mzArray.Length}");
                message = string.Join(", ", frameList[i].mzArray);
                messages.Add($"mz array: {message}");
                messages.Add($"intensityArray size: {frameList[i].intensityArray.Length}");
                message = string.Join(", ", frameList[i].intensityArray);
                messages.Add($"intensity array: {message}");
                messages.Add($"mobilityArray size: {frameList[i].mobilityArray.Length}");
                message = string.Join(", ", frameList[i].mobilityArray);
                messages.Add($"moblility array: {message}");
                messages.Add("-------------------------------------------------");
            }
        }

        private double[] CreatePopulatedArrayFrom1(long numScans)
        {
            double[] scanNum = new double[numScans];
            for (long i = 0; i < numScans; i++)
            {
                scanNum[i] = i + 1;
            }
            return scanNum;
        }

        void ShowSomeFrames(List<Frame> frameList, int showNum = 5)
        {
            int showCount = frameList.Count > showNum ? showNum : frameList.Count;
            if (showCount > 0)
            {
                messages.Add($"Show first {showCount} frames' detail information below:");
                messages.Add("-------------------------------------------------");
            }

            for (int i = 0; i < showCount; i++)
            {
                messages.Add($"frameId: {frameList[i].FrameId}");                
                string strMzs = string.Join(", ", frameList[i].mzArray);
                messages.Add($"mz array: {strMzs}");
                string strIntensities = string.Join(", ", frameList[i].intensityArray);
                messages.Add($"intensity array: {strIntensities}");
                string strMobilities = string.Join(", ", frameList[i].mobilityArray);
                messages.Add($"moblility array: {strMobilities}");
                messages.Add("-------------------------------------------------");

            }
        }
    }
}
