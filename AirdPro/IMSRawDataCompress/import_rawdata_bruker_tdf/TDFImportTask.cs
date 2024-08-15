using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql;
using AirdPro.IMSRawDataCompress.datamodel;
using AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel;
using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using static AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.TDFLibrary;

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
        public List<Frame> GetFrameList() { return frameList; }

        long error;
        string message;


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

            //
            //iMSRawDataFile.setStartTimeStamp(metaDataTable.getAcquisitionDateTime());

            //import data from tdb_bin file
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

                            //import metadata
                            message = $"Reading metadata from " + metaDataTable.GetTableName() + " ...";
                            messages.Add(message);
                            metaDataTable.ExecuteQuery(connection);
                            message = "fetched records: " + (metaDataTable.GetKeyColumn().Values.Count());
                            messages.Add(message);
                            ShowTableData(metaDataTable);

                            //import frames
                            message = $"Reading metadata from " + frameTable.GetTableName() + " ...";
                            messages.Add(message);
                            frameTable.ExecuteQuery(connection);
                            message = "fetched records: " + (frameTable.GetKeyColumn().Values.Count());
                            messages.Add(message);
                            ShowTableData(frameTable);

                            connection.Close();
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
            TDFUtils tdfUtils = new TDFUtils();
            //long handle = tdfUtils.openFile(fileName);
            long handle = TDFLibrary.tims_open_v2(this.fileName, 1, 0);
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
                //for test: 最多取100条
                numFrames = numFrames > 100 ? 100 : numFrames;
                for (int i = 0; i < numFrames; i++)
                {
                    int frameId = (int)frameTable.GetFrameIdColumn().GetValueList()[i];
                    int numScans = (int)frameTable.GetNumScansColumn().GetValueList()[i];
                    float rt = (float)(frameTable.GetTimeColumn().GetValueList()[i] / 60); // to minutes
                    //PolarityType polarity = 
                    //int msLevel = 
                    //String scanDefinition =
                    //float accumulationTime = 
                    Range<Double> mzRange = metaDataTable.GetMzRange();

                    CentroidData centroidData = null;
                    //Give a callback impl, which will be called by DLL function and DLL function will pass values to the first 4 parameters
                    CentroidCallback centroidCallbackImpl = new CentroidCallback((precursorId, numPeaks, pMz, pIntensities, userData) =>
                    {
                        //方案1：可以仅仅写下面一行代码来实现，将具体逻辑放到CentroidData的构造方法中，但可读性差一些
                        //centroidData = new CentroidData(precursorId, numPeaks, pMz, pIntensities);

                        //方案2：将具体实现放在这里，可读性强一些，更容易理解
                        if (numPeaks != 0)
                        {
                            if (pMz == IntPtr.Zero || pIntensities == IntPtr.Zero)
                            {
                                throw new InvalidOperationException("Construct CentroidData failed for Pointer (pMz or pIntensities) is invalid.");
                            }

                            // 为mz数组分配内存
                            double[] Mzs = new double[numPeaks];
                            // 从非托管内存复制数据到托管数组 (double和float不同，不能使用相同的代码）
                            for (int i = 0; i < numPeaks; i++)
                            {
                                // 计算每个元素的起始地址
                                IntPtr elementPtr = new IntPtr(pMz.ToInt64() + i * sizeof(double));
                                // 将指针指向的数据结构化为double类型并存储到数组中
                                Mzs[i] = (double)Marshal.PtrToStructure(elementPtr, typeof(double));
                            }

                            // 为强度数组分配内存
                            float[] Intensities = new float[numPeaks];
                            // 从非托管内存复制数据到托管数组（float数组的操作比较简单一些）
                            Marshal.Copy(pIntensities, Intensities, 0, numPeaks);

                            // 创建 CentroidData 对象并填充数据
                            centroidData = new CentroidData(precursorId, numPeaks, Mzs, Intensities);
                        }
                        else
                        {
                            // 如果没有峰值，则使用空数组
                            centroidData = new CentroidData(precursorId, numPeaks, Array.Empty<double>(), Array.Empty<float>());
                        }
                    });
                    lock (LockObject)
                    {
                        long error = TDFLibrary.tims_extract_centroided_spectrum_for_frame_v2(handle, frameId, 0, numScans, centroidCallbackImpl, IntPtr.Zero);
                        if (error == 0)
                        {
                            message = $"Could not extract centroid scan from frame {frameId} for scans 0 to {numScans}";
                            messages.Add(message);
                            throw new Exception(message);
                        }
                    }

                    Frame frame = new Frame(frameId, centroidData);
                    frameList.Add(frame);

                    if (i > 0 && i % 1000 == 0)
                    {
                        messages.Add($"{DateTime.Now:yyyyMMdd HH:mm:ss}: loaded {i} frames, percentage {((float)i / (float)numFrames):P2}");
                    }
                }

                messages.Add($"{DateTime.Now:yyyyMMdd HH:mm:ss}: successfully loaded all {frameList.Count} frames!");
            }
            catch (Exception e)
            {
                messages.Add(e.Message);
                throw e;
            }
            finally
            {
                TDFLibrary.tims_close(handle);
            }

            //show first 5 frame
            ShowSomeFrames();
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
                Messages.Add($"frameid: {frameList[i].FrameId}");
                Messages.Add($"CentroidData.PrecursorId: {frameList[i].CentroidData.PrecursorId}");
                Messages.Add($"CentroidData.NumPeaks: {frameList[i].CentroidData.NumPeaks}");
                string strMzs = string.Join(", ", frameList[i].CentroidData.Mzs);
                Messages.Add($"CentroidData.Mzs: {strMzs}");
                string strIntensities = string.Join(", ", frameList[i].CentroidData.Intensities);
                Messages.Add($"CentroidData.Intensities: {strIntensities}");
                Messages.Add("-------------------------------------------------");

            }
        }
    }
}
