using AirdPro.Forms;
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
        private String tdfDir;
        private FileInfo tdfFile;
        private FileInfo tdfBinFile;
        private TDFMetaDataTable metaDataTable;
        private TDFFrameTable tdfFrameTable;
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

        /*private readonly MZmineProject _project;
            private readonly ScanImportProcessorConfig _scanProcessorConfig;
            private readonly Type _module;
            private readonly ParameterSet _parameters;
            //private FileInfo? _fileNameToOpen;
            //private FileInfo? _tdf;
            //private FileInfo? _tdfBin;
            private string _rawDataFileName;
            //private TDFMetaDataTable _metaDataTable;
            private TDFFrameTable _frameTable;
            private TDFPrecursorTable _precursorTable;
            private TDFPasefFrameMsMsInfoTable _pasefFrameMsMsInfoTable;
            private TDFFrameMsMsInfoTable _frameMsMsInfoTable;
            private FramePrecursorTable _framePrecursorTable;
            private PrmFrameTargetTable _prmFrameTargetTable;
            private TDFMaldiFrameInfoTable _maldiFrameInfoTable;
            private TDFMaldiFrameLaserInfoTable _maldiFrameLaserInfoTable;
            private IMSRawDataFile _newMZmineFile;
            private bool _isMaldi;
            //private string _description;
            private double _finishedPercentage;
            private double _lastFinishedPercentage;
            private int _loadedFrames;*/

        public void Run(String tdfDir)
        {
            this.messages.Clear();
            String message = "";
            // 检查文件夹是否存在
            if (!Directory.Exists(tdfDir)) 
            {
                message = $"The directory '{tdfDir}' does not exist.";
                this.messages.Add(message);
                throw new DirectoryNotFoundException(message);
            }

            // 定义文件过滤器
            string tdfFilter = Path.Combine(tdfDir, "*.tdf");
            string tdfBinFilter = Path.Combine(tdfDir, "*.tdf_bin");

            // 获取匹配的文件
            FileInfo tdfFile = Directory.EnumerateFiles(tdfDir, "*.tdf").Select(f => new FileInfo(f)).FirstOrDefault();
            FileInfo tdfBinFile = Directory.EnumerateFiles(tdfDir, "*.tdf_bin").Select(f => new FileInfo(f)).FirstOrDefault();

            // 检查是否找到了文件
            if (tdfFile == null || tdfBinFile == null)
            {
                message = "Could not find both .tdf and .tdf_bin files in the specified directory.";
                this.messages.Add(message);
                throw new FileNotFoundException(message);
            }

            this.tdfDir = tdfDir;
            this.tdfFile = tdfFile;
            this.tdfBinFile = tdfBinFile;

            message = "Selected tdf file: " + tdfFile.FullName;
            this.messages.Add(message);
            message = "Selected tdf_bin file: " + tdfBinFile.FullName;
            this.messages.Add(message);

            //init all tables
            metaDataTable = new TDFMetaDataTable();
            tdfFrameTable = new TDFFrameTable();

            //import data from db(tdf) file
            ReadMetadata();
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
    }
}
