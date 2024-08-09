using System;
using System.Collections.Generic;

namespace AirdPro.IMSRawDataCompress.datamodel.sql
{
    public class TDFMetaDataTable : TDFDataTable<string>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string METADATA_TABLE = "GlobalMetadata";
        public const string VALUE_COLUMN = "Value";
        public const string KEY_COLUMN = "Key";

        private static readonly List<string> AllowedFileVersions = new List<string> { "3.1", "3.2" };

        private readonly TDFDataColumn<string> valueCol;
        private readonly TDFDataColumn<string> keyCol;

        public TDFMetaDataTable(): base(METADATA_TABLE, KEY_COLUMN)
        {
            keyCol = (TDFDataColumn<string>)base.Columns[0];
            valueCol = new TDFDataColumn<string>(VALUE_COLUMN);
            base.Columns.Add(valueCol);
        }

        private Range<double> mzRange;
        private string instrumentType;

        public bool HasLineSpectra()
        {
            int index = keyCol.IndexOf(Keys.HasLineSpectra.ToString());
            return index != -1 && int.Parse(valueCol[index]) == 1;
        }

        public bool IsFileVersionValid()
        {
            if (valueCol == null)
            {
                return false;
            }
            string version = valueCol[keyCol.IndexOf(Keys.SchemaVersionMajor.ToString())]
                                 + "." + valueCol[keyCol.IndexOf(Keys.SchemaVersionMinor.ToString())];

            if (!AllowedFileVersions.Contains(version))
            {
                //MZmineCore.Desktop.DisplayMessage($"TDF version {version} is not supported. This might lead to unexpected results.");
                return false;
            }

            return true;
        }

        public override bool ExecuteQuery(System.Data.IDbConnection connection)
        {
            bool b = base.ExecuteQuery(connection);
            if (!b)
            {
                return false;
            }

            /*
            //验证的必要性不大，需要的话可以放开，执行的结构：就只保留给定的这些key，其它的会被删除掉
            for (int i = 0; i < keyCol.Count; i++)
            {
                try
                {
                    Enum.Parse(typeof(Keys), keyCol[i]);
                }
                catch (ArgumentException)
                {
                    foreach (var col in base.Columns)
                    {
                        col.RemoveAt(i);
                    }
                    i--;
                }
            }*/

            return true;
        }

        public Range<double> GetMzRange()
        {
            if (mzRange == null)
            {
                if (keyCol.Count == 0)
                {
                    logger.Info("Cannot determine mz range. Metadata not loaded yet.");
                    return Range<double>.Closed(0d, 0d);
                }
                int lowerIndex = keyCol.IndexOf(Keys.MzAcqRangeLower.ToString());
                int upperIndex = keyCol.IndexOf(Keys.MzAcqRangeUpper.ToString());
                if (lowerIndex == -1 || upperIndex == -1)
                {
                    logger.Info("Cannot determine mz range. Metadata did not contain required information.");
                    return Range<double>.Closed(0d, 0d);
                }
                mzRange = Range<double>.Closed(double.Parse(valueCol[lowerIndex]), double.Parse(valueCol[upperIndex]));
            }
            return mzRange;
        }

        public string GetInstrumentType()
        {
            if (instrumentType == null)
            {
                int row = keyCol.IndexOf(Keys.InstrumentName.ToString());
                instrumentType = valueCol[row];
            }
            return instrumentType;
        }

        public bool HasProfileSpectra()
        {
            int index = keyCol.IndexOf(Keys.HasProfileSpectra.ToString());
            return index != -1 && int.Parse(valueCol[index]) == 1;
        }

        public Nullable<DateTime> GetAcquisitionDateTime()
        {
            int index = keyCol.IndexOf(Keys.AcquisitionDateTime.ToString());
            string date = index != -1 ? valueCol[index]: null;

            if (date == null)
            {
                return null;
            }
            try
            {
                return DateTime.Parse(date);
            }
            catch (FormatException)
            {
                var sampleName = valueCol[keyCol.IndexOf(Keys.SampleName.ToString())];
                //logger.LogWarning($"Cannot parse acquisition date of sample {sampleName}");
                return null;
            }
        }

        public enum Keys
        {
            SchemaType, SchemaVersionMajor, SchemaVersionMinor, MzAcqRangeLower, MzAcqRangeUpper,
            OneOverK0AcqRangeLower, OneOverK0AcqRangeUpper, AcquisitionSoftwareVersion, InstrumentName,
            Description, SampleName, MethodName, HasProfileSpectra, HasLineSpectra, ImagingAreaMinXIndexPos,
            Geometry, ImagingAreaMaxXIndexPos, ImagingAreaMinYIndexPos, ImagingAreaMaxYIndexPos,
            AcquisitionDateTime
        }

        public string GetValueForKey(Keys key)
        {
            int index = keyCol.IndexOf(key.ToString());
            return index != -1 ? valueCol[index] : "";
        }
    }
}
