using log4net;
using System;
using static AirdPro.csimzMLParser.data.DataTypeTransform;

namespace AirdPro.csimzMLParser.data
{
    [Serializable]
    public class DataLocation(DataStorage dataStorage, long offset, int length)
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(DataLocation));

        public static readonly long EXTENDED_OFFSET = 4294967296L; // 2^32
        protected DataStorage dataStorage = dataStorage;
        protected long offset = offset;
        protected int length = length;
        protected DataTransformation dataTransformation = null;

        public DataStorage GetDataStorage()
        {
            return dataStorage;
        }

        public long GetOffset()
        {
            return offset;
        }

        public int GetLength()
        {
            return length;
        }

        public byte[] GetBytes()
        {
            if (length <= 0)
            {
                LOGGER.InfoFormat("Data is of size {0} for {1}", [length, dataStorage]);
                return [];
            }
            if (offset < 0)
            {
                LOGGER.InfoFormat("Offset is {0} for {1}. Attempting to fix integer overflow.", [offset, dataStorage]);
                offset += EXTENDED_OFFSET; // By default is 2^32
            }

            return dataStorage.GetData(offset, length);
        }

        public double[] GetData()
        {
            byte[] data = GetBytes();

            if (dataTransformation == null)
                return ConvertDataToDouble(data, DataType.DOUBLE);

            return dataTransformation.PerformReverseTransform(data);
        }

        public void SetDataTransformation(DataTransformation transformation)
        {
            dataTransformation = transformation;
        }

        public override string ToString()
        {
            return $"[{offset} ( {length} )] {dataStorage}";
        }
    }
}
