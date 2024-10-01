using AirdPro.ImzMLParser.data;
using AirdPro.ImzMLParser.obo;
using AirdPro.ImzMLParser.util;
using log4net;
using System;
using DataType = AirdPro.ImzMLParser.data.DataType;

namespace AirdPro.ImzMLParser.mzml
{ 
    public class BinaryDataArray : MzMLContentWithParams
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(BinaryDataArray));

        private const long serialVersionUID = 1L;

        public enum CompressionType
        {
            NONE,
            ZLIB,
            XZ,
            LZ4,
            ZSTD,
            MSNUMPRESS_LINEAR,
            MSNUMPRESS_POSITIVE,
            MSNUMPRESS_SLOF,
            MSNUMPRESS_LINEAR_ZLIB,
            MSNUMPRESS_POSITIVE_ZLIB,
            MSNUMPRESS_SLOF_ZLIB,
            MSNUMPRESS_LINEAR_XZ,
            MSNUMPRESS_POSITIVE_XZ,
            MSNUMPRESS_SLOF_XZ,
            MSNUMPRESS_LINEAR_LZ4,
            MSNUMPRESS_POSITIVE_LZ4,
            MSNUMPRESS_SLOF_LZ4,
            MSNUMPRESS_LINEAR_ZSTD,
            MSNUMPRESS_POSITIVE_ZSTD,
            MSNUMPRESS_SLOF_ZSTD
        }

        public static OBOTerm ToOBOTerm(CompressionType compressionType)
        {
            switch (compressionType)
            {
                case CompressionType.NONE:
                    return OBO.GetOBO().GetTerm(Accessions.NO_COMPRESSION_ID);
                case CompressionType.ZLIB:
                    return OBO.GetOBO().GetTerm(Accessions.ZLIB_COMPRESSION_ID);
                case CompressionType.XZ:
                    return OBO.GetOBO().GetTerm(Accessions.XZ_COMPRESSION_ID);
                case CompressionType.LZ4:
                    return OBO.GetOBO().GetTerm(Accessions.LZ4_COMPRESSION_ID);
                case CompressionType.ZSTD:
                    return OBO.GetOBO().GetTerm(Accessions.ZSTD_COMPRESSION_ID);
                case CompressionType.MSNUMPRESS_LINEAR:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_ID);
                case CompressionType.MSNUMPRESS_POSITIVE:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_ID);
                case CompressionType.MSNUMPRESS_SLOF:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_ID);
                case CompressionType.MSNUMPRESS_LINEAR_ZLIB:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_ZLIB_ID);
                case CompressionType.MSNUMPRESS_POSITIVE_ZLIB:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_ZLIB_ID);
                case CompressionType.MSNUMPRESS_SLOF_ZLIB:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_ZLIB_ID);
                case CompressionType.MSNUMPRESS_LINEAR_XZ:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_XZ_ID);
                case CompressionType.MSNUMPRESS_POSITIVE_XZ:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_XZ_ID);
                case CompressionType.MSNUMPRESS_SLOF_XZ:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_XZ_ID);
                case CompressionType.MSNUMPRESS_LINEAR_LZ4:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_LZ4_ID);
                case CompressionType.MSNUMPRESS_POSITIVE_LZ4:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_LZ4_ID);
                case CompressionType.MSNUMPRESS_SLOF_LZ4:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_LZ4_ID);
                case CompressionType.MSNUMPRESS_LINEAR_ZSTD:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_ZSTD_ID);
                case CompressionType.MSNUMPRESS_POSITIVE_ZSTD:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_ZSTD_ID);
                case CompressionType.MSNUMPRESS_SLOF_ZSTD:
                    return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_ZSTD_ID);
                default:
                    return null;
            }
        }

        public static class CompressionTypeExtensions
        {
            public static OBOTerm toOBOTerm(CompressionType compressionType)
            {
                switch (compressionType)
                {
                    case CompressionType.NONE:
                        return OBO.GetOBO().GetTerm(Accessions.NO_COMPRESSION_ID);
                    case CompressionType.ZLIB:
                        return OBO.GetOBO().GetTerm(Accessions.ZLIB_COMPRESSION_ID);
                    case CompressionType.XZ:
                        return OBO.GetOBO().GetTerm(Accessions.XZ_COMPRESSION_ID);
                    case CompressionType.LZ4:
                        return OBO.GetOBO().GetTerm(Accessions.LZ4_COMPRESSION_ID);
                    case CompressionType.ZSTD:
                        return OBO.GetOBO().GetTerm(Accessions.ZSTD_COMPRESSION_ID);
                    case CompressionType.MSNUMPRESS_LINEAR:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_ID);
                    case CompressionType.MSNUMPRESS_POSITIVE:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_ID);
                    case CompressionType.MSNUMPRESS_SLOF:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_ID);
                    case CompressionType.MSNUMPRESS_LINEAR_ZLIB:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_ZLIB_ID);
                    case CompressionType.MSNUMPRESS_POSITIVE_ZLIB:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_ZLIB_ID);
                    case CompressionType.MSNUMPRESS_SLOF_ZLIB:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_ZLIB_ID);
                    case CompressionType.MSNUMPRESS_LINEAR_XZ:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_XZ_ID);
                    case CompressionType.MSNUMPRESS_POSITIVE_XZ:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_XZ_ID);
                    case CompressionType.MSNUMPRESS_SLOF_XZ:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_XZ_ID);
                    case CompressionType.MSNUMPRESS_LINEAR_LZ4:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_LZ4_ID);
                    case CompressionType.MSNUMPRESS_POSITIVE_LZ4:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_LZ4_ID);
                    case CompressionType.MSNUMPRESS_SLOF_LZ4:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_LZ4_ID);
                    case CompressionType.MSNUMPRESS_LINEAR_ZSTD:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_LINEAR_ZSTD_ID);
                    case CompressionType.MSNUMPRESS_POSITIVE_ZSTD:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_POSITIVE_ZSTD_ID);
                    case CompressionType.MSNUMPRESS_SLOF_ZSTD:
                        return OBO.GetOBO().GetTerm(Accessions.MSNUMPRESS_SLOF_ZSTD_ID);
                    default:
                        return null;
                }
            }
        }

        public static class Accessions
        {
            public const string COMPRESSION_TYPE_ID = "MS:1000572";
            public const string BINARY_DATA_ARRAY_ID = "MS:1000513";
            public const string BINARY_DATA_TYPE_ID = "MS:1000518";
            public const string IBD_BINARY_DATA_TYPE_ID = "IMS:1000014";
            public const string MZ_ARRAY_ID = "MS:1000514";
            public const string MZ_ARRAY_UNITS_ID = "MS:1000040";
            public const string INTENSITY_ARRAY_ID = "MS:1000515";
            public const string INTENSITY_ARRAY_UNITS_NUMBER_OF_COUNTS_ID = "MS:1000131";
            public const string INTENSITY_ARRAY_UNITS_PERCENTAGE_OF_BASEPEAK_ID = "MS:1000132";
            public const string INTENSITY_ARRAY_UNITS_COUNTS_PER_SECOND_ID = "MS:1000814";
            public const string INTENSITY_ARRAY_UNITS_PERCENTAGE_OF_BASEPEAK_TIMES_100_ID = "MS:1000905";
            public const string DOUBLE_PRECISION_ID = "MS:1000523";
            public const string SINGLE_PRECISION_ID = "MS:1000521";
            public const string SIGNED_32BIT_INTEGER_ID = "MS:1000519";
            public const string IMS_SIGNED_32BIT_INTEGER_ID = "IMS:1000141";
            public const string SIGNED_64BIT_INTEGER_ID = "MS:1000522";
            public const string IMS_SIGNED_64BIT_INTEGER_ID = "IMS:1000142";
            public const string SIGNED_8BIT_INTEGER_ID = "IMS:1100000";
            public const string SIGNED_16BIT_INTEGER_ID = "IMS:1100001";
            public const string NO_COMPRESSION_ID = "MS:1000576";
            public const string ZLIB_COMPRESSION_ID = "MS:1000574";
            public const string XZ_COMPRESSION_ID = "IMS:1005001";
            public const string LZ4_COMPRESSION_ID = "IMS:1005002";
            public const string ZSTD_COMPRESSION_ID = "IMS:1005003";
            public const string MSNUMPRESS_POSITIVE_ID = "MS:1002313";
            public const string MSNUMPRESS_LINEAR_ID = "MS:1002312";
            public const string MSNUMPRESS_SLOF_ID = "MS:1002314";
            public const string MSNUMPRESS_LINEAR_ZLIB_ID = "IMS:1005013";
            public const string MSNUMPRESS_POSITIVE_ZLIB_ID = "IMS:1005014";
            public const string MSNUMPRESS_SLOF_ZLIB_ID = "IMS:1005015";
            public const string MSNUMPRESS_LINEAR_XZ_ID = "IMS:1005004";
            public const string MSNUMPRESS_POSITIVE_XZ_ID = "IMS:1005005";
            public const string MSNUMPRESS_SLOF_XZ_ID = "IMS:1005006";
            public const string MSNUMPRESS_LINEAR_LZ4_ID = "IMS:1005007";
            public const string MSNUMPRESS_POSITIVE_LZ4_ID = "IMS:1005008";
            public const string MSNUMPRESS_SLOF_LZ4_ID = "IMS:1005009";
            public const string MSNUMPRESS_LINEAR_ZSTD_ID = "IMS:1005010";
            public const string MSNUMPRESS_POSITIVE_ZSTD_ID = "IMS:1005011";
            public const string MSNUMPRESS_SLOF_ZSTD_ID = "IMS:1005012";
            public const string EXTERNAL_ARRAY_LENGTH_ID = "IMS:1000103";
            public const string EXTERNAL_DATA_ID = "IMS:1000101";
            public const string EXTERNAL_ENCODED_LENGTH_ID = "IMS:1000104";
            public const string EXTERNAL_OFFSET_ID = "IMS:1000102";
        }

        private int arrayLength = -1;
        private DataProcessing dataProcessingRef;
        private int encodedLength = 0;
        private double[] data;
        private bool isMzArray;
        private bool isIntensityArray;
        protected DataLocation dataLocation;

        public BinaryDataArray(int encodedLength)
        {
            this.encodedLength = encodedLength;
        }

        public BinaryDataArray(BinaryDataArray bda, ReferenceableParamGroupList rpgList, DataProcessingList dpList)
            : base(bda, rpgList)
        {
            this.arrayLength = bda.arrayLength;
            this.encodedLength = bda.encodedLength;
            this.dataLocation = bda.dataLocation;

            this.isMzArray = bda.isMzArray;
            this.isIntensityArray = bda.isIntensityArray;
            this.data = bda.data;

            if (bda.dataProcessingRef != null && dpList != null)
            {
                foreach (DataProcessing dp in dpList)
                {
                    if (bda.dataProcessingRef.id.Equals(dp.id))
                    {
                        this.dataProcessingRef = dp;
                        break;
                    }
                }
            }
        }

        public int GetEncodedLength()
        {
            return encodedLength;
        }

        public void SetArrayLength(int arrayLength)
        {
            this.arrayLength = arrayLength;
        }

        public void SetDataProcessingRef(DataProcessing dataProcessingRef)
        {
            this.dataProcessingRef = dataProcessingRef;
        }

        public bool IsDoublePrecision()
        {
            DataType dataType = GetDataType();

            return dataType == DataType.DOUBLE;
        }

        public bool IsSinglePrecision()
        {
            CVParam dataTypeCVParam = GetCVParam(Accessions.SINGLE_PRECISION_ID);
            return dataTypeCVParam != null;
        }

        public bool IsSigned8BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(Accessions.SIGNED_8BIT_INTEGER_ID);
            return dataTypeCVParam != null;
        }

        public bool IsSigned16BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(Accessions.SIGNED_16BIT_INTEGER_ID);
            return dataTypeCVParam != null;
        }

        public bool IsSigned32BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(Accessions.SIGNED_32BIT_INTEGER_ID);
            CVParam imsDataType = GetCVParam(Accessions.IMS_SIGNED_32BIT_INTEGER_ID);
            return !(dataTypeCVParam == null && imsDataType == null);
        }

        public bool IsSigned64BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(Accessions.SIGNED_64BIT_INTEGER_ID);
            CVParam imsDataType = GetCVParam(Accessions.IMS_SIGNED_64BIT_INTEGER_ID);
            return !(dataTypeCVParam == null && imsDataType == null);
        }

        public bool IsCompressed()
        {
            CVParam compression = GetCVParam(Accessions.NO_COMPRESSION_ID);
            return compression == null;
        }

        public static int GetDataTypeInBytes(CVParam dataType)
        {
            string dataTypeTermID = dataType.GetTerm().id;

            if (dataTypeTermID.Equals(Accessions.DOUBLE_PRECISION_ID))
            {
                return 8;
            }
            else if (dataTypeTermID.Equals(Accessions.SINGLE_PRECISION_ID))
            {
                return 4;
            }
            else if (dataTypeTermID.Equals(Accessions.SIGNED_8BIT_INTEGER_ID))
            {
                return 1;
            }
            else if (dataTypeTermID.Equals(Accessions.SIGNED_16BIT_INTEGER_ID))
            {
                return 2;
            }
            else if (dataTypeTermID.Equals(Accessions.SIGNED_32BIT_INTEGER_ID))
            {
                return 4;
            }
            else if (dataTypeTermID.Equals(Accessions.SIGNED_64BIT_INTEGER_ID))
            {
                return 8;
            }
            return 1;
        }

        public void SetDataLocation(DataLocation dataLocation)
        {
            this.dataLocation = dataLocation;
        }

        public DataLocation GetDataLocation()
        {
            return dataLocation;
        }

        public double[] GetDataAsDouble()
        {
            return GetDataAsDouble(false);
        }

        public double[] GetDataAsDouble(bool keepInMemory)
        {
            if (data != null)
                return data;

            if (dataLocation == null && parent != null)
            {
                IMzMLTag grandParent = parent.GetParent();
                if (grandParent is MzMLDataContainer)
                {
                    ((MzMLDataContainer)grandParent).ConvertMzMLDataStorageToBase64();
                }
            }

            if (dataLocation == null)
            {
                return [];
            }

            double[] loadedData = [];
            try
            {
                loadedData = dataLocation.GetData();
            }
            catch (Exception ex) 
            {
                logger.Error("Data format exception occurred.", ex);
            }

            if (keepInMemory)
            {
                data = loadedData;
            }

            return loadedData;
        }

        public void SetData(double[] data)
        {
            this.data = data;
        }

        public byte[] GetDataAsByte()
        {
            if (dataLocation == null)
            {
                return [];
            }

            return dataLocation.GetBytes();
        }

        public DataTransformation GenerateDataTransformation()
        {
            DataTransformation transformation = new();

            if (!DataType.DOUBLE.Equals(GetDataType()))
            {
                transformation.AddTransform(new DataTypeTransform(DataType.DOUBLE, GetDataType()));
            }

            CVParam compressionCVParam = GetCVParamOrChild(Accessions.COMPRESSION_TYPE_ID);

            if (compressionCVParam != null)
            {
                switch (compressionCVParam.GetTerm().id)
                {
                    case Accessions.ZLIB_COMPRESSION_ID:
                        transformation.AddTransform(new ZlibDataTransform());
                        break;
                    case Accessions.XZ_COMPRESSION_ID:
                        //transformation.AddTransform(new XZDataTransform());
                        break;
                    case Accessions.LZ4_COMPRESSION_ID:
                        //transformation.AddTransform(new LZ4DataTransform((int)(ExternalArrayLength * GetDataTypeInBytes(GetCVParamOrChild(Accessions.BINARY_DATA_TYPE_ID)))));
                        break;
                    case Accessions.ZSTD_COMPRESSION_ID:
                        //transformation.AddTransform(new ZstdDataTransform((int)(ExternalArrayLength * GetDataTypeInBytes(GetCVParamOrChild(Accessions.BINARY_DATA_TYPE_ID)))));
                        break;
                    case Accessions.MSNUMPRESS_LINEAR_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_POSITIVE_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_SLOF_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_LINEAR_LZ4_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_LINEAR_XZ_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_LINEAR_ZLIB_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_LINEAR_ZSTD_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_POSITIVE_LZ4_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_POSITIVE_XZ_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_POSITIVE_ZLIB_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_POSITIVE_ZSTD_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_SLOF_LZ4_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_SLOF_XZ_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_SLOF_ZLIB_ID:
                        //
                        break;
                    case Accessions.MSNUMPRESS_SLOF_ZSTD_ID:
                        //
                        break;                    
                }
            }

            return transformation;
        }

        public DataType GetDataType()
        {
            CVParam dataTypeParam = GetCVParamOrChild(Accessions.BINARY_DATA_TYPE_ID);
            DataType dataType = new();

            if (dataTypeParam != null)
            {
                string term = dataTypeParam.GetTerm().id;

                if (term.Equals(Accessions.DOUBLE_PRECISION_ID))
                {
                    dataType = DataType.DOUBLE;
                }
                else if (term.Equals(Accessions.SINGLE_PRECISION_ID))
                {
                    dataType = DataType.FLOAT;
                }
                else if (term.Equals(Accessions.SIGNED_64BIT_INTEGER_ID) || term.Equals(Accessions.IMS_SIGNED_64BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_64BIT;
                }
                else if (term.Equals(Accessions.SIGNED_32BIT_INTEGER_ID) || term.Equals(Accessions.IMS_SIGNED_32BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_32BIT;
                }
                else if (term.Equals(Accessions.SIGNED_16BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_16BIT;
                }
                else if (term.Equals(Accessions.SIGNED_8BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_8BIT;
                }
            }
            else
            {
                logger.InfoFormat("BinaryDataArray#getDataType(): {0}", GetCVParamList().ToArray().ToString());
            }
            return dataType;
        }

        public long GetExternalArrayLength()
        {
            CVParam arrayLengthCVParam = GetCVParam(Accessions.EXTERNAL_ARRAY_LENGTH_ID);
            return arrayLengthCVParam? .GetValueAsLong() ?? -1;
        }

        public long GetExternalEncodedLength()
        {
            CVParam encodedLengthCVParam = GetCVParam(Accessions.EXTERNAL_ENCODED_LENGTH_ID);
            return encodedLengthCVParam?.GetValueAsLong() ?? -1;
        }

        public long GetExternalOffset()
        {
            CVParam externalOffset = GetCVParam(Accessions.EXTERNAL_OFFSET_ID);
            return externalOffset?.GetValueAsLong() ?? -1;
        }

        public override void AddCVParam(CVParam cvParam)
        {
            logger.Info($"Adding CVParam to BinaryDataArray {cvParam}");

            if (cvParam.GetTerm().IsChildOf(Accessions.BINARY_DATA_ARRAY_ID))
            {
                string term = cvParam.GetTerm().id;

                if (term.Equals(Accessions.MZ_ARRAY_ID))
                {
                    logger.Info($"Found m/z array");
                    isMzArray = true;
                }
                else if (term.Equals(Accessions.INTENSITY_ARRAY_ID))
                {
                    isIntensityArray = true;
                }
            }
            base.AddCVParam(cvParam);
        }

        public override void AddReferenceableParamGroupRef(ReferenceableParamGroupRef rpgr)
        {
            CVParam child = rpgr.GetReference().GetCVParamOrChild(Accessions.BINARY_DATA_ARRAY_ID);
            if (child != null)
            {
                if (child.GetTerm().id.Equals(Accessions.MZ_ARRAY_ID))
                {
                    isMzArray = true;
                }
                else if (child.GetTerm().id.Equals(Accessions.INTENSITY_ARRAY_ID))
                {
                    isIntensityArray = true;
                }
            }
            base.AddReferenceableParamGroupRef(rpgr);
        }

        public bool IsMzArray()
        {
            return isMzArray;
        }

        public bool IsIntensityArray()
        {
            return isIntensityArray;
        }

        public void SetCompression(CompressionType compression)
        {
            RemoveChildrenOfCVParam(Accessions.COMPRESSION_TYPE_ID, false);
            this.AddCVParam(new EmptyCVParam(ToOBOTerm(compression)));
        }

        public void SetDataType(DataType dataType)
        {
            string newDataTypeID;

            switch (dataType)
            {
                case DataType.FLOAT:
                    newDataTypeID = Accessions.SINGLE_PRECISION_ID;
                    break;
                case DataType.INTEGER_64BIT:
                    newDataTypeID = Accessions.SIGNED_64BIT_INTEGER_ID;
                    break;
                case DataType.INTEGER_32BIT:
                    newDataTypeID = Accessions.SIGNED_32BIT_INTEGER_ID;
                    break;
                case DataType.INTEGER_16BIT:
                    newDataTypeID = Accessions.SIGNED_16BIT_INTEGER_ID;
                    break;
                case DataType.INTEGER_8BIT:
                    newDataTypeID = Accessions.SIGNED_8BIT_INTEGER_ID;
                    break;
                case DataType.DOUBLE:
                default:
                    newDataTypeID = Accessions.DOUBLE_PRECISION_ID;
                    break;
            }

            this.RemoveChildrenOfCVParam(Accessions.BINARY_DATA_TYPE_ID, false);
            this.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(newDataTypeID)));
        }

        public CVParam GetDataArrayType()
        {
            return GetCVParamOrChild(Accessions.BINARY_DATA_ARRAY_ID);
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = $"encodedLength=\"{encodedLength}\"";

            if (arrayLength != -1)
            {
                attributeText += $" arrayLength=\"{arrayLength}\"";
            }
            if (dataProcessingRef != null)
            {
                attributeText += $" dataProcessingRef=\"{XMLHelper.EnsureSafeXML(dataProcessingRef.GetID())}\"";
            }
            return attributeText;
        }

        public override string ToString()
        {
            return $"binaryDataArray: {dataLocation}";
        }

        public override string GetTagName()
        {
            return "binaryDataArray";
        }
        
    }

}
