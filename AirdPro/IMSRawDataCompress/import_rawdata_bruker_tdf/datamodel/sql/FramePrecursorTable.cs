using AirdPro.IMSRawDataCompress.datamodel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AirdPro.IMSRawDataCompress.import_rawdata_bruker_tdf.datamodel.sql
{
    public class FramePrecursorTable : TDFDataTable
    {
        public const String FRAME_PRECURSOR_TABLE = "FramePrecursorTable";

        private readonly TDFDataColumn<long> frameIdColumn;
        private readonly TDFDataColumn<long> precursorIdColumn;
        private readonly TDFDataColumn<long> scanNumBeginColumn;
        private readonly TDFDataColumn<long> scanNumEndColumn;
        private readonly TDFDataColumn<double> collisionEnergyColumn;
        private readonly TDFDataColumn<double> isolationWidthColumn;
        private readonly TDFDataColumn<double> isolationMzColumn;
        private readonly TDFDataColumn<double> largestPeakMzColumn;
        private readonly TDFDataColumn<long> chargeColumn;
        private readonly TDFDataColumn<long> parentIdColumn;

        /**
        * Key = FrameId of the MS2 Frame
        * <p></p>
        * Value = Collection of PasefMsMsInfo on all precursors in the frame.
        */
        private readonly Dictionary<long, HashSet<BuildingPASEFMsMsInfo>> info;

        public FramePrecursorTable() : base(FRAME_PRECURSOR_TABLE)
        {
            frameIdColumn = new TDFDataColumn<long>(TDFPasefFrameMsMsInfoTable.FRAME_ID);
            base.AddKeyColumn(frameIdColumn);
            precursorIdColumn = new TDFDataColumn<long>(TDFPasefFrameMsMsInfoTable.PRECURSOR_ID);
            base.AddColumn(precursorIdColumn);
            scanNumBeginColumn = new TDFDataColumn<long>(TDFPasefFrameMsMsInfoTable.SCAN_NUM_BEGIN);
            base.AddColumn(scanNumBeginColumn);
            scanNumEndColumn = new TDFDataColumn<long>(TDFPasefFrameMsMsInfoTable.SCAN_NUM_END);
            base.AddColumn(scanNumEndColumn);
            collisionEnergyColumn = new TDFDataColumn<double>(TDFPasefFrameMsMsInfoTable.COLLISION_ENERGY);
            base.AddColumn(collisionEnergyColumn);
            isolationWidthColumn = new TDFDataColumn<double>(TDFPasefFrameMsMsInfoTable.ISOLATION_WIDTH);
            base.AddColumn(isolationWidthColumn);
            isolationMzColumn = new TDFDataColumn<double>(TDFPasefFrameMsMsInfoTable.ISOLATION_MZ);
            base.AddColumn(isolationMzColumn);
            largestPeakMzColumn = new TDFDataColumn<double>(TDFPrecursorTable.LARGEST_PEAK_MZ);
            base.AddColumn(largestPeakMzColumn);
            chargeColumn = new TDFDataColumn<long>(TDFPrecursorTable.CHARGE);
            base.AddColumn(chargeColumn);
            parentIdColumn = new TDFDataColumn<long>(TDFPrecursorTable.PARENT_ID);
            base.AddColumn(parentIdColumn);

            info = new();
        }

        public override bool ExecuteQuery(IDbConnection connection)
        {
            bool query = base.ExecuteQuery(connection);
            if (query)
            {
                collapseInfo();
            }
            return query;
        }

        private void collapseInfo()
        {
            List<long> frameIds = frameIdColumn.GetValueList();
            for (int i = 0; i < frameIds.Count; i++)
            {
                long frameId = frameIds[i];

                // 检查是否需要添加新的HashSet到字典中
                if (!info.TryGetValue(frameId, out HashSet<BuildingPASEFMsMsInfo> entry))
                {
                    info.Add(frameId, new HashSet<BuildingPASEFMsMsInfo>());
                    entry = info[frameId];
                }

                // 选择最大的峰m/z或隔离m/z
                double precursorMz = (largestPeakMzColumn.GetValueList()[i] != 0d) ? largestPeakMzColumn.GetValueList()[i] : isolationMzColumn.GetValueList()[i];

                Range<long> range = new(
                    scanNumBeginColumn.GetValueList()[i],
                    scanNumEndColumn.GetValueList()[i] + 1
                );

                // 创建BuildingPASEFMsMsInfo实例并添加到entry中
                entry.Add(new BuildingPASEFMsMsInfo(
                    precursorMz,
                    range,
                    collisionEnergyColumn.GetValueList()[i],
                    chargeColumn.GetValueList()[i],
                    parentIdColumn.GetValueList()[i],
                    frameId,
                    isolationWidthColumn.GetValueList()[i]
                ));
            }
        }

        public HashSet<BuildingPASEFMsMsInfo> getMsMsInfoForFrame(long frameId)
        {
            return info[frameId];
        }

        public override string GetColumnNamesForQuery()
        {
            string msmstable = TDFPasefFrameMsMsInfoTable.PASEF_FRAME_MSMS_TABLE_NAME;
            string precursorstable = TDFPrecursorTable.PRECURSOR_TABLE_NAME;

            return msmstable + "." + TDFPasefFrameMsMsInfoTable.FRAME_ID + ", " + msmstable + "."
                + TDFPasefFrameMsMsInfoTable.PRECURSOR_ID + ", " + msmstable + "."
                + TDFPasefFrameMsMsInfoTable.SCAN_NUM_BEGIN + ", " + msmstable + "."
                + TDFPasefFrameMsMsInfoTable.SCAN_NUM_END + ", " + msmstable + "."
                + TDFPasefFrameMsMsInfoTable.COLLISION_ENERGY + ", " + msmstable + "."
                + TDFPasefFrameMsMsInfoTable.ISOLATION_WIDTH + ", " + msmstable + "."
                + TDFPasefFrameMsMsInfoTable.ISOLATION_MZ + ", " + precursorstable + "."
                + TDFPrecursorTable.LARGEST_PEAK_MZ + ", " + precursorstable + "."
                + TDFPrecursorTable.CHARGE + ", " + precursorstable + "." + TDFPrecursorTable.PARENT_ID;
        }

        public override string GetQueryText(String columnHeadersForQuery)
        {
            String msmstable = TDFPasefFrameMsMsInfoTable.PASEF_FRAME_MSMS_TABLE_NAME;
            String precursorstable = TDFPrecursorTable.PRECURSOR_TABLE_NAME;

            return "SELECT " + columnHeadersForQuery + " " + "FROM " + msmstable + " " + "LEFT JOIN "
                + precursorstable + " ON " + msmstable + "." + TDFPasefFrameMsMsInfoTable.PRECURSOR_ID + "="
                + precursorstable + "." + TDFPrecursorTable.PRECURSOR_ID + " "
                //
                + "ORDER BY " + msmstable + "." + TDFPasefFrameMsMsInfoTable.FRAME_ID;
        }

    }
}
