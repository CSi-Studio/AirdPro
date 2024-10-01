using AirdPro.ImzMLParser.obo;
using log4net;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.data
{
    [Serializable]
    public class DataLocation
    {
        private static readonly long serialVersionUID = 1L;
        private static readonly ILog logger = LogManager.GetLogger(typeof(DataLocation));

        public static readonly long EXTENDED_OFFSET = 4294967296L; // 2^32
        protected DataStorage dataStorage;
        protected long offset;
        protected int length;
        protected DataTransformation dataTransformation;

        public DataLocation(DataStorage dataStorage, long offset, int length)
        {
            this.dataStorage = dataStorage;
            this.offset = offset;
            this.length = length;
        }

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
                logger.InfoFormat("Data is of size {0} for {1}", [length, dataStorage]);
                return [];
            }
            if (offset < 0)
            {
                logger.InfoFormat("Offset is {0} for {1}. Attempting to fix integer overflow.", [offset, dataStorage]);
                offset += EXTENDED_OFFSET; // By default is 2^32
            }

            return dataStorage.GetData(offset, length);
        }

        public double[] GetData()
        {
            byte[] data = GetBytes();

            if (dataTransformation == null)
                return DataTypeTransform.ConvertDataToDouble(data, DataType.DOUBLE);

            return dataTransformation.PerformReverseTransform(data);
        }

        public void SetDataTransformation(DataTransformation transformation)
        {
            this.dataTransformation = transformation;
        }

        public override string ToString()
        {
            return $"[{offset} ( {length} )] {dataStorage}";
        }
    }
}
