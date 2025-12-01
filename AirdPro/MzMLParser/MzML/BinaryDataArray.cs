using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.obo;
using AirdPro.csimzMLParser.util;
using log4net;
using System;
using static AirdPro.csimzMLParser.data.DataTypeTransform;

namespace AirdPro.csimzMLParser.mzml
{ 
    public class BinaryDataArray : MzMLContentWithParams
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(BinaryDataArray));

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
            return compressionType switch
            {
                CompressionType.NONE => OBO.GetOBO().GetTerm(NO_COMPRESSION_ID),
                CompressionType.ZLIB => OBO.GetOBO().GetTerm(ZLIB_COMPRESSION_ID),
                CompressionType.XZ => OBO.GetOBO().GetTerm(XZ_COMPRESSION_ID),
                CompressionType.LZ4 => OBO.GetOBO().GetTerm(LZ4_COMPRESSION_ID),
                CompressionType.ZSTD => OBO.GetOBO().GetTerm(ZSTD_COMPRESSION_ID),
                CompressionType.MSNUMPRESS_LINEAR => OBO.GetOBO().GetTerm(MSNUMPRESS_LINEAR_ID),
                CompressionType.MSNUMPRESS_POSITIVE => OBO.GetOBO().GetTerm(MSNUMPRESS_POSITIVE_ID),
                CompressionType.MSNUMPRESS_SLOF => OBO.GetOBO().GetTerm(MSNUMPRESS_SLOF_ID),
                CompressionType.MSNUMPRESS_LINEAR_ZLIB => OBO.GetOBO().GetTerm(MSNUMPRESS_LINEAR_ZLIB_ID),
                CompressionType.MSNUMPRESS_POSITIVE_ZLIB => OBO.GetOBO().GetTerm(MSNUMPRESS_POSITIVE_ZLIB_ID),
                CompressionType.MSNUMPRESS_SLOF_ZLIB => OBO.GetOBO().GetTerm(MSNUMPRESS_SLOF_ZLIB_ID),
                CompressionType.MSNUMPRESS_LINEAR_XZ => OBO.GetOBO().GetTerm(MSNUMPRESS_LINEAR_XZ_ID),
                CompressionType.MSNUMPRESS_POSITIVE_XZ => OBO.GetOBO().GetTerm(MSNUMPRESS_POSITIVE_XZ_ID),
                CompressionType.MSNUMPRESS_SLOF_XZ => OBO.GetOBO().GetTerm(MSNUMPRESS_SLOF_XZ_ID),
                CompressionType.MSNUMPRESS_LINEAR_LZ4 => OBO.GetOBO().GetTerm(MSNUMPRESS_LINEAR_LZ4_ID),
                CompressionType.MSNUMPRESS_POSITIVE_LZ4 => OBO.GetOBO().GetTerm(MSNUMPRESS_POSITIVE_LZ4_ID),
                CompressionType.MSNUMPRESS_SLOF_LZ4 => OBO.GetOBO().GetTerm(MSNUMPRESS_SLOF_LZ4_ID),
                CompressionType.MSNUMPRESS_LINEAR_ZSTD => OBO.GetOBO().GetTerm(MSNUMPRESS_LINEAR_ZSTD_ID),
                CompressionType.MSNUMPRESS_POSITIVE_ZSTD => OBO.GetOBO().GetTerm(MSNUMPRESS_POSITIVE_ZSTD_ID),
                CompressionType.MSNUMPRESS_SLOF_ZSTD => OBO.GetOBO().GetTerm(MSNUMPRESS_SLOF_ZSTD_ID),
                _ => null,
            };
        }

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
        public const string MOBILITY_ARRAY_ID = "MS:1003006";
        public const string MOBILITY_ARRAY_UNITS_ID = "MS:1002814";
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

        private int arrayLength = -1;
        private DataProcessing dataProcessingRef;
        private readonly int encodedLength = 0;
        private double[] data;
        private bool isMzArray;
        private bool isIntensityArray;
        private bool isMobilityArray;
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
                    if (bda.dataProcessingRef.GetID().Equals(dp.GetID()))
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
            CVParam dataTypeCVParam = GetCVParam(SINGLE_PRECISION_ID);
            return dataTypeCVParam != null;
        }

        public bool IsSigned8BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(SIGNED_8BIT_INTEGER_ID);
            return dataTypeCVParam != null;
        }

        public bool IsSigned16BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(SIGNED_16BIT_INTEGER_ID);
            return dataTypeCVParam != null;
        }

        public bool IsSigned32BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(SIGNED_32BIT_INTEGER_ID);
            CVParam imsDataType = GetCVParam(IMS_SIGNED_32BIT_INTEGER_ID);
            return !(dataTypeCVParam == null && imsDataType == null);
        }

        public bool IsSigned64BitInteger()
        {
            CVParam dataTypeCVParam = GetCVParam(SIGNED_64BIT_INTEGER_ID);
            CVParam imsDataType = GetCVParam(IMS_SIGNED_64BIT_INTEGER_ID);
            return !(dataTypeCVParam == null && imsDataType == null);
        }

        public bool IsCompressed()
        {
            CVParam compression = GetCVParam(NO_COMPRESSION_ID);
            return compression == null;
        }

        public static int GetDataTypeInBytes(CVParam dataType)
        {
            string dataTypeTermID = dataType.GetTerm().GetID();

            if (dataTypeTermID.Equals(DOUBLE_PRECISION_ID))
            {
                return 8;
            }
            else if (dataTypeTermID.Equals(SINGLE_PRECISION_ID))
            {
                return 4;
            }
            else if (dataTypeTermID.Equals(SIGNED_8BIT_INTEGER_ID))
            {
                return 1;
            }
            else if (dataTypeTermID.Equals(SIGNED_16BIT_INTEGER_ID))
            {
                return 2;
            }
            else if (dataTypeTermID.Equals(SIGNED_32BIT_INTEGER_ID))
            {
                return 4;
            }
            else if (dataTypeTermID.Equals(SIGNED_64BIT_INTEGER_ID))
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
                if (grandParent is MzMLDataContainer container)
                {
                    container.ConvertMzMLDataStorageToBase64();
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
                LOGGER.Error("Data format exception occurred.", ex);
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

            CVParam compressionCVParam = GetCVParamOrChild(COMPRESSION_TYPE_ID);

            if (compressionCVParam != null)
            {
                switch (compressionCVParam.GetTerm().GetID())
                {
                    case ZLIB_COMPRESSION_ID:
                        transformation.AddTransform(new ZlibDataTransform());
                        break;                   
                    case LZ4_COMPRESSION_ID:
                        //transformation.AddTransform(new LZ4DataTransform((int)(ExternalArrayLength * GetDataTypeInBytes(GetCVParamOrChild(Accessions.BINARY_DATA_TYPE_ID)))));
                        break;
                    case ZSTD_COMPRESSION_ID:
                        //transformation.AddTransform(new ZstdDataTransform((int)(ExternalArrayLength * GetDataTypeInBytes(GetCVParamOrChild(Accessions.BINARY_DATA_TYPE_ID)))));
                        break;                               
                }
            }

            return transformation;
        }

        public DataType GetDataType()
        {
            CVParam dataTypeParam = GetCVParamOrChild(BINARY_DATA_TYPE_ID);
            DataType dataType = new();

            if (dataTypeParam != null)
            {
                string term = dataTypeParam.GetTerm().GetID();

                if (term.Equals(DOUBLE_PRECISION_ID))
                {
                    dataType = DataType.DOUBLE;
                }
                else if (term.Equals(SINGLE_PRECISION_ID))
                {
                    dataType = DataType.FLOAT;
                }
                else if (term.Equals(SIGNED_64BIT_INTEGER_ID) || term.Equals(IMS_SIGNED_64BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_64BIT;
                }
                else if (term.Equals(SIGNED_32BIT_INTEGER_ID) || term.Equals(IMS_SIGNED_32BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_32BIT;
                }
                else if (term.Equals(SIGNED_16BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_16BIT;
                }
                else if (term.Equals(SIGNED_8BIT_INTEGER_ID))
                {
                    dataType = DataType.INTEGER_8BIT;
                }
            }
            else
            {
                LOGGER.InfoFormat("BinaryDataArray#getDataType(): {0}", GetCVParamList().ToArray().ToString());
            }
            return dataType;
        }

        public long GetExternalArrayLength()
        {
            CVParam arrayLengthCVParam = GetCVParam(EXTERNAL_ARRAY_LENGTH_ID);
            return arrayLengthCVParam? .GetValueAsLong() ?? -1;
        }

        public long GetExternalEncodedLength()
        {
            CVParam encodedLengthCVParam = GetCVParam(EXTERNAL_ENCODED_LENGTH_ID);
            return encodedLengthCVParam?.GetValueAsLong() ?? -1;
        }

        public long GetExternalOffset()
        {
            CVParam externalOffset = GetCVParam(EXTERNAL_OFFSET_ID);
            return externalOffset?.GetValueAsLong() ?? -1;
        }

        public override void AddCVParam(CVParam cvParam)
        {
            LOGGER.Info($"Adding CVParam to BinaryDataArray {cvParam}");

            if (cvParam.GetTerm().IsChildOf(BINARY_DATA_ARRAY_ID))
            {
                string term = cvParam.GetTerm().GetID();

                if (term.Equals(MZ_ARRAY_ID))
                {
                    LOGGER.Info($"Found m/z array");
                    isMzArray = true;
                }
                else if (term.Equals(INTENSITY_ARRAY_ID))
                {
                    isIntensityArray = true;
                }
                else if (term.Equals(MOBILITY_ARRAY_ID))
                {
                    isMobilityArray = true;
                }
            }
            base.AddCVParam(cvParam);
        }

        public override void AddReferenceableParamGroupRef(ReferenceableParamGroupRef rpgRef)
        {
            CVParam child = rpgRef.GetReference().GetCVParamOrChild(BINARY_DATA_ARRAY_ID);
            if (child != null)
            {
                if (child.GetTerm().GetID().Equals(MZ_ARRAY_ID))
                {
                    isMzArray = true;
                }
                else if (child.GetTerm().GetID().Equals(INTENSITY_ARRAY_ID))
                {
                    isIntensityArray = true;
                }
                else if (child.GetTerm().GetID().Equals(MOBILITY_ARRAY_ID))
                {
                    isMobilityArray = true;
                }
            }
            base.AddReferenceableParamGroupRef(rpgRef);
        }

        public bool IsMzArray()
        {
            return isMzArray;
        }

        public bool IsIntensityArray()
        {
            return isIntensityArray;
        }

        public bool IsMobilityArray() 
        {
            return isMobilityArray;
        }

        public void SetCompression(CompressionType compression)
        {
            RemoveChildrenOfCVParam(COMPRESSION_TYPE_ID, false);
            AddCVParam(new EmptyCVParam(ToOBOTerm(compression)));
        }

        public void SetDataType(DataType dataType)
        {
            string newDataTypeID = dataType switch
            {
                DataType.FLOAT => SINGLE_PRECISION_ID,
                DataType.INTEGER_64BIT => SIGNED_64BIT_INTEGER_ID,
                DataType.INTEGER_32BIT => SIGNED_32BIT_INTEGER_ID,
                DataType.INTEGER_16BIT => SIGNED_16BIT_INTEGER_ID,
                DataType.INTEGER_8BIT => SIGNED_8BIT_INTEGER_ID,
                _ => DOUBLE_PRECISION_ID,
            };
            RemoveChildrenOfCVParam(BINARY_DATA_TYPE_ID, false);
            AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(newDataTypeID)));
        }

        public CVParam GetDataArrayType()
        {
            return GetCVParamOrChild(BINARY_DATA_ARRAY_ID);
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
