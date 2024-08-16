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
using CSharpFastPFOR.Port;
using System.Drawing;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf
{
    public class TDFImportTask
    {
        private string fileName;
        private FileInfo tdfFile;
        private FileInfo tdfBinFile;        
        private TDFMetaDataTable metaDataTable;
        private TDFFrameTable frameTable;
        private TDFPrecursorTable precursorTable;
        private TDFPasefFrameMsMsInfoTable pasefFrameMsMsInfoTable;
        private TDFFrameMsMsInfoTable frameMsMsInfoTable;
        private FramePrecursorTable framePrecursorTable;
        private PrmFrameTargetTable prmFrameTargetTable;
        private TDFMaldiFrameInfoTable maldiFrameInfoTable;
        private TDFMaldiFrameLaserInfoTable maldiFrameLaserInfoTable;
        private IIMSRawData iMSRawDataFile;
        private bool isMaldi;
        private readonly int loadedFrames;
        private static readonly object LockObject = new();
        private readonly List<Frame> frameList = new();
        long error;
        string message;
       
        private readonly List<string> messages = new();        
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

        public List<Frame> GetFrameList()
        {
            return frameList;
        }

        public void Run(string fileName)
        {
            this.fileName = fileName;
            messages.Clear();
            //String message = "";
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
            tdfFile = Directory.EnumerateFiles(fileName, "*.tdf").Select(f => new FileInfo(f)).FirstOrDefault();
            tdfBinFile = Directory.EnumerateFiles(fileName, "*.tdf_bin").Select(f => new FileInfo(f)).FirstOrDefault();

            // 检查是否找到了文件
            if (tdfFile == null || tdfBinFile == null)
            {
                message = "Could not find both .tdf and .tdf_bin files in the specified directory.";
                messages.Add(message);
                throw new FileNotFoundException(message);
            }            

            message = "Selected tdf file: " + tdfFile.FullName;
            messages.Add(message);
            message = "Selected tdf_bin file: " + tdfBinFile.FullName;
            messages.Add(message);

            //init all tables
            metaDataTable = new TDFMetaDataTable();
            frameTable = new TDFFrameTable();
            precursorTable = new TDFPrecursorTable();
            pasefFrameMsMsInfoTable = new TDFPasefFrameMsMsInfoTable();
            prmFrameTargetTable = new PrmFrameTargetTable();
            frameMsMsInfoTable = new TDFFrameMsMsInfoTable();
            framePrecursorTable = new FramePrecursorTable();

            maldiFrameInfoTable = new TDFMaldiFrameInfoTable();
            maldiFrameLaserInfoTable = new TDFMaldiFrameLaserInfoTable();
            isMaldi = false;

            //import data from db(tdf) file
            ReadMetadata();

            //iMSRawDataFile.setStartTimeStamp(metaDataTable.getAcquisitionDateTime());

            //import data from tdf_bin file
            ReadBinData();
        }

        private void ReadMetadata()
        {
            String message = "Initializing SQL ...";
            messages.Add(message);
            try
            {
                using (var connection = new SQLiteConnection($"Data Source={tdfFile.FullName};Version=3;"))
                {
                    message = $"Establishing SQL connection to {tdfFile.Name}";
                    messages.Add(message);

                    lock (typeof(SQLiteConnection))
                    {
                        try
                        {
                            connection.Open();

                            //read metaDataTable
                            message = $"Reading metadata from " + metaDataTable.GetTableName() + " ...";
                            messages.Add(message);
                            metaDataTable.ExecuteQuery(connection);
                            message = "fetched records: " + (metaDataTable.GetKeyColumn().Values.Count());
                            messages.Add(message);
                            //ShowTableData(metaDataTable);

                            //read frameTable
                            message = $"Reading metadata from " + frameTable.GetTableName() + " ...";
                            messages.Add(message);
                            frameTable.ExecuteQuery(connection);
                            message = "fetched records: " + (frameTable.GetKeyColumn().Values.Count());
                            messages.Add(message);
                            //ShowTableData(frameTable);
                        }
                        catch (Exception ex)
                        {
                            messages.Add(ex.ToString());
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                messages.Add(e.ToString());
            }
        }

        //用于测试或调试
        private void ShowTableData(TDFDataTable dataTable, int countLimit = 30)
        {
            if (showDetail)
            {
                int colCount = dataTable.GetColumns().Count;
                int rowCount = dataTable.GetKeyColumn().Values.Count();
                if (rowCount > countLimit) //避免显示太多数据
                { 
                    rowCount = countLimit;
                    messages.Add("Show first " + rowCount + " records:");
                }  
                else
                {
                    messages.Add("Show all " + rowCount + " records:");
                }
                String colNames = "rownum";
                for (int j = 0; j < colCount; j++)
                {
                    colNames += "\t" + dataTable.GetColumns()[j].ColumnName;
                }
                messages.Add(colNames);
                String oneRow = "";
                for (int i = 0; i < rowCount; i++)
                {
                    oneRow = (i + 1) + "";
                    for (int j = 0; j < colCount; j++)
                    {
                        oneRow += "\t" + (dataTable.GetColumns()[j]).Values.ElementAt(i).ToString();
                    }
                    messages.Add(oneRow);
                }
            }
        }

        private void ReadBinData()
        {
            TDFUtils tdfUtils = new();
            long handle = tdfUtils.openFile(fileName);
            //long handle = TDFLibrary.tims_open_v2(fileName, 1, 0);
            if (handle == 0)
            {
                message = $"open the file {fileName} failed!";
                messages.Add(message);
                throw new Exception(message);
            }
            else
            {
                messages.Add($"open the file {fileName} successfully!");
            }

            try
            {
                frameList.Clear();
                int numFrames = frameTable.GetFrameIdColumn().GetValueList().Count;
                messages.Add($"Total {numFrames} frames in the file");
                //for test: 读frameId为2,18,19的数据
                //int readFrameCount = numFrames > 100 ? 100 : numFrames;
                long[] testFrameIds = [2,18,19];
                for (int i = 0; i < testFrameIds.Length; i++)
                {
                    List<long> frameIdList = frameTable.GetFrameIdColumn().GetValueList();
                    int index = frameIdList.IndexOf(testFrameIds[i]);
                    long frameId = frameTable.GetFrameIdColumn().GetValueList()[index];
                    double rt = frameTable.GetTimeColumn().GetValueList()[index] / 60; // rt，单位：分
                    long numScans = frameTable.GetNumScansColumn().GetValueList()[index]; //910，固定值
                    long numPeaks = frameTable.GetNumPeaksColumn().GetValueList()[index];  //mz、intensity、mobility数组大小
                    double maxIntensity = frameTable.GetMaxIntensityColumn().GetValueList()[index];
                    double summedIntensity = frameTable.GetSummedIntensityColumn().GetValueList()[index];

                    //Range<double> mzRange = metaDataTable.GetMzRange();

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
                        long[] scanNums = createPopulatedArrayFrom1(numScans);
                        messages.Add($"scanNums size{scanNums.Length}");
                        mobilities = tdfUtils.convertScanNumsToMobilities(handle, frameId, scanNums);
                        /*error = TDFLibrary.tims_scannum_to_oneoverk0(handle, frameId, scanNums.Select(x => (double)x).ToArray(), mobilities, scanNums.Length);
                        if (error == 0)
                        {
                            messages.Add($"Could not convert scan nums to 1/K0 for frame {frameId}");
                        }*/
                    }

                    Frame frame = new Frame(frameId);
                    frame.centroidData = centroidData;
                    frame.mzArray = centroidData.Mzs;
                    frame.intensityArray = centroidData.Intensities;
                    frame.mobilityArray = mobilities;
                    frameList.Add(frame);
                    messages.Add($"frameId: {frameId}, rt: {rt}, numPeaks: {numPeaks}");



                    /* if (i > 0 && i % 1000 == 0)
                     {
                         messages.Add($"{DateTime.Now:yyyyMMdd HH:mm:ss}: loaded {i} frames, percentage {((float)i / (float)numFrames):P2}");
                     }*/
                }

                //messages.Add($"{DateTime.Now:yyyyMMdd HH:mm:ss}: successfully loaded all {frameList.Count} frames!");
            }
            catch (Exception e)
            {
                messages.Add(e.Message);
                throw e;
            }
            finally
            {
                tdfUtils.close();
                //TDFLibrary.tims_close(handle);
            }

            //show first 5 frame
            //ShowSomeFrames();
            ShowTestFrames();
        }

        private void ShowTestFrames()
        {
            for (int i = 0; i < frameList.Count; i++)
            {                
                messages.Add($"test frame count: {frameList.Count}");
                messages.Add($"frameId: {frameList[i].FrameId}" + "--------------");
                message = string.Join(", ", frameList[i].mzArray);
                messages.Add($"mz array: {message}");
                message = string.Join(", ", frameList[i].intensityArray);
                messages.Add($"intensity array: {message}");
                message = string.Join(", ", frameList[i].mobilityArray);
                messages.Add($"moblility array: {message}");
                messages.Add("-------------------------------------------------");
            }
        }

        private long[] createPopulatedArrayFrom1(long numScans)
        {
            long[] scanNums = new long[numScans];
            for (int i = 0; i < numScans; i++)
            {
                scanNums[i] = i + 1;
            }
            return scanNums;
        }

        void ShowSomeFrames(int showNum = 5)
        {
            int showCount = frameList.Count > showNum ? showNum : frameList.Count;
            if (showCount > 0)
            {
                Messages.Add($"Show first {showCount} frames' detail information below:");
                Messages.Add("-------------------------------------------------");
            }

            for (int i = 0; i < showCount; i++)
            {
                Messages.Add($"frameId: {frameList[i].FrameId}");                
                string strMzs = string.Join(", ", frameList[i].mzArray);
                Messages.Add($"mz array: {strMzs}");
                string strIntensities = string.Join(", ", frameList[i].intensityArray);
                Messages.Add($"intensity array: {strIntensities}");
                string strMobilities = string.Join(", ", frameList[i].mobilityArray);
                Messages.Add($"moblility array: {strMobilities}");
                Messages.Add("-------------------------------------------------");

            }
        }
    }
}
