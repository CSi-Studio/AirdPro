using AirdPro.IMSRawDataCompress.datamodel;
using AirdPro.IMSRawDataCompress.datamodel.callbacks;
using AirdPro.IMSRawDataCompress.datamodel.sql;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf
{
    public class TDFImportTask
    {
        private String fileName;
        private FileInfo tdfFile;
        private FileInfo tdfBinFile;
        long error;
        string message;
        private TDFMetaDataTable metaDataTable;
        private TDFFrameTable tdfFrameTable;
        private List<Frame> frameList = new List<Frame>();
        private List<String> messages = new List<String>();
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
        

        public void Run(String fileName)
        {
            this.fileName = fileName;
            messages.Clear();
            String message = "";
            // 检查文件夹是否存在
            if (!Directory.Exists(fileName)) 
            {
                message = $"The directory '{fileName}' does not exist.";
                this.messages.Add(message);
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
            tdfFrameTable = new TDFFrameTable();

            //import data from db(tdf) file
            ReadMetadata();

            //import data from tdb_bin file
           // readBinData();
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
                            showTableData(metaDataTable);

                            //import frames
                            message = $"Reading metadata from " + tdfFrameTable.GetTableName() + " ...";
                            messages.Add(message);
                            tdfFrameTable.ExecuteQuery(connection);
                            message = "fetched records: " + (tdfFrameTable.GetKeyColumn().Values.Count());
                            messages.Add(message);
                            showTableData(tdfFrameTable);

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
        private void showTableData(TDFDataTable dataTable, int countLimit = 30)
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

        private void readBinData()
        {
            TDFUtils tdfUtils = new TDFUtils();
            long handle = tdfUtils.openFile(fileName);
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
                int numFrames = tdfFrameTable.GetFrameIdColumn().GetValueList().Count;
                for (int i = 0; i < numFrames; i++)
                {
                    int frameId = (int)tdfFrameTable.GetFrameIdColumn().GetValueList()[i];
                    int numScans = (int)tdfFrameTable.GetNumScansColumn().GetValueList()[i];
                    float rt = (float)(tdfFrameTable.GetTimeColumn().GetValueList()[i] / 60); // to minutes
                    //PolarityType polarity = 
                    //int msLevel = 
                    //String scanDefinition =
                    //float accumulationTime = 
                    Range<Double> mzRange = metaDataTable.GetMzRange();

                    CentroidData data = new CentroidData();
                    int startScanNum = 0;
                    int endScanNum = numScans;
                    data = tdfUtils.extractCentroidsForFrame(handle, 2, startScanNum, endScanNum);
                    if (data == null)
                    {
                        message = $"Could not extract centroid scan for frame {frameId} for scans 0 to {numScans}";
                        messages.Add(message);
                        throw new Exception(message);
                    }
                    double[] mzArray = data.Mzs;
                    float[] intensityArray = data.Intensities;
                    //double[] mobilities = tdfUtils.convertScanNumsToMobilities(handle, frameId, scanNums);

                   /* //
                    double[] mobilities = new double[numScans.Length];
                    double[] scanNum;
                    fixed (double* pBuffer = mobilities)
                    {
                        long error = TDFLibrary.tims_scannum_to_oneoverk0(handle, frameId, scanNum, mobilities, scanNum.Length);
                        if (error == 0)
                        {
                            message = $"Could not convert scan nums to 1/K0 for frame {frameId}";
                            messages.Add(message);
                            throw new Exception(message);
                        }
                        else
                        {
                            messages.Add($"Successfully extract mobilities for frame {frameId} for scans 0 to {numScans}");
                        }
                    }               */

                    Frame frame = new Frame(frameId, data);
                    frameList.Add(frame);
                }
            }
            catch (Exception e)
            {
                messages.Add(e.Message);
                throw e;
            }
            finally
            {
                tdfUtils.close();
            }
        }
    }
}
