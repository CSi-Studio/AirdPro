using AirdPro.csimzMLParser.data;
using AirdPro.csimzMLParser.util;
using System.Text;
using static AirdPro.csimzMLParser.data.DataTypeTransform;
using static AirdPro.csimzMLParser.mzml.BinaryDataArray;

namespace AirdPro.csimzMLParser.mzml
{
    public abstract class MzMLDataContainer : MzMLIndexedContentWithParams
    {
        protected DataLocation dataLocation;
        protected BinaryDataArrayList binaryDataArrayList;
        protected int defaultArrayLength;
        protected DataProcessing dataProcessingRef;
        private IReferenceList<DataProcessing> dataProcessingList;

        public MzMLDataContainer(MzMLDataContainer mzMLContent, ReferenceableParamGroupList rpgList, DataProcessingList dpList)
            : base(mzMLContent, rpgList)
        {
            dataLocation = mzMLContent.dataLocation;
            defaultArrayLength = mzMLContent.defaultArrayLength;

            if (mzMLContent.dataProcessingRef != null && dpList != null)
            {
                foreach (DataProcessing dp in dpList)
                {
                    if (mzMLContent.dataProcessingRef.GetID() == dp.GetID())
                    {
                        dataProcessingRef = dp;
                        break;
                    }
                }
            }

            if (mzMLContent.binaryDataArrayList != null)
            {
                binaryDataArrayList = new BinaryDataArrayList(mzMLContent.binaryDataArrayList, rpgList, dpList);
            }
        }

        public MzMLDataContainer(string id, int defaultArrayLength)
        {
            this.id = id;
            this.defaultArrayLength = defaultArrayLength;
        }

        public void SetBinaryDataArrayList(BinaryDataArrayList binaryDataArrayList)
        {
            binaryDataArrayList.SetParent(this);
            this.binaryDataArrayList = binaryDataArrayList;
        }

        public BinaryDataArrayList GetBinaryDataArrayList()
        {
            if (binaryDataArrayList == null)
            {
                binaryDataArrayList = new BinaryDataArrayList(0);
            }
            return binaryDataArrayList;
        }

        public void SetDataProcessingRef(DataProcessing dataProcessingRef)
        {
            this.dataProcessingRef = dataProcessingRef;
            EnsureValidReferences();
        }

        public DataProcessing GetDataProcessingRef()
        {
            return dataProcessingRef;
        }

        public void SetDataProcessingList(IReferenceList<DataProcessing> dataProcessingList)
        {
            this.dataProcessingList = dataProcessingList;
            EnsureValidReferences();
        }

        public DataLocation GetDataLocation()
        {
            return dataLocation;
        }

        public void SetDataLocation(DataLocation dataLocation)
        {
            this.dataLocation = dataLocation;
        }

        public void EnsureLoadableData()
        {
            if (dataLocation != null && dataLocation.GetDataStorage() is MzMLSpectrumDataStorage)
            {
                ConvertMzMLDataStorageToBase64();
            }            
        }

        public void ConvertMzMLDataStorageToBase64()
        {
            if (dataLocation != null && dataLocation.GetDataStorage() is MzMLSpectrumDataStorage)
            {
                // Load in the data from the data storage
                byte[] data = dataLocation.GetBytes();
                string spectrumData = Encoding.UTF8.GetString(data);

                MzMLSpectrumDataStorage mzMLDataStorage = (MzMLSpectrumDataStorage)dataLocation.GetDataStorage();

                // Identify where each binary data array is within the spectrum mzML
                if (binaryDataArrayList != null)
                {
                    foreach (BinaryDataArray bda in binaryDataArrayList)
                    {
                        CVParam cvParam = bda.GetCVParamOrChild(BinaryDataArray.BINARY_DATA_ARRAY_ID);
                        string cvParamID = cvParam.GetTerm().GetID();

                        int cvParamLocation = spectrumData.IndexOf(cvParamID);

                        if (cvParamLocation != -1)
                        {
                            string subSpectrumData = spectrumData.Substring(cvParamLocation);

                            int binaryStart = subSpectrumData.IndexOf("<binary>") + cvParamLocation + "<binary>".Length;
                            int binaryEnd = subSpectrumData.IndexOf("</binary>") + cvParamLocation;

                            DataLocation location = new DataLocation(mzMLDataStorage.GetBase64DataStorage(), binaryStart + dataLocation.GetOffset(), binaryEnd - binaryStart);
                            bda.SetDataLocation(location);
                            location.SetDataTransformation(bda.GenerateDataTransformation());
                        }
                    }
                }

                dataLocation = null;
            }
        }

        public double[] GetIntensityArray()
        {
            return GetIntensityArray(false);
        }

        public double[] GetIntensityArray(bool keepInMemory)
        {
            if (binaryDataArrayList == null)
            {
                return [];
            }

            EnsureLoadableData();

            return binaryDataArrayList.GetIntensityArray().GetDataAsDouble(keepInMemory);
        }

        public void SetCompression(CompressionType compression)
        {
            binaryDataArrayList.SetCompression(compression);
        }

        public void SetDataType(DataType dataType)
        {
            binaryDataArrayList.SetDataType(dataType);
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = base.GetXMLAttributeText();

            if (!string.IsNullOrEmpty(attributeText))
            {
                attributeText += " ";
            }

            attributeText += $"defaultArrayLength=\"{defaultArrayLength}\"";

            if (dataProcessingRef != null)
            {
                attributeText += $" dataProcessingRef=\"{XMLHelper.EnsureSafeXML(dataProcessingRef.GetID())}\"";
            }

            return attributeText;
        }

        protected new void EnsureValidReferences()
        {
            if (dataProcessingList != null && dataProcessingRef != null)
            {
                dataProcessingRef = dataProcessingList.GetValidReference(dataProcessingRef);
            }
        }
    }
}
