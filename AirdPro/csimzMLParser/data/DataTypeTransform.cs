using AirdPro.csimzMLParser.mzml;
using AirdPro.csimzMLParser.obo;
using log4net;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AirdPro.csimzMLParser.data
{  
    [Serializable]
    public class DataTypeTransform : IDataTransform
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(DataTypeTransform));

        public enum DataType
        {
            DOUBLE,
            FLOAT,
            INTEGER_8BIT,
            INTEGER_16BIT,
            INTEGER_32BIT,
            INTEGER_64BIT
        }

        public static OBOTerm ToOBOTerm(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.DOUBLE:
                    return OBO.GetOBO().GetTerm(BinaryDataArray.DOUBLE_PRECISION_ID);
                case DataType.FLOAT:
                    return OBO.GetOBO().GetTerm(BinaryDataArray.SINGLE_PRECISION_ID);
                case DataType.INTEGER_64BIT:
                    return OBO.GetOBO().GetTerm(BinaryDataArray.SIGNED_64BIT_INTEGER_ID);
                case DataType.INTEGER_32BIT:
                    return OBO.GetOBO().GetTerm(BinaryDataArray.SIGNED_32BIT_INTEGER_ID);
                case DataType.INTEGER_16BIT:
                    return OBO.GetOBO().GetTerm(BinaryDataArray.SIGNED_16BIT_INTEGER_ID);
                case DataType.INTEGER_8BIT:
                    return OBO.GetOBO().GetTerm(BinaryDataArray.SIGNED_8BIT_INTEGER_ID);
                default:
                    return null;
            }
        }

        public static DataType FromOBOTerm(OBOTerm term)
        {
            string accession = term.GetID();

            if (accession.Equals(BinaryDataArray.DOUBLE_PRECISION_ID))
            {
                return DataType.DOUBLE;
            }
            else if (accession.Equals(BinaryDataArray.SINGLE_PRECISION_ID))
            {
                return DataType.FLOAT;
            }
            else if (accession.Equals(BinaryDataArray.SIGNED_64BIT_INTEGER_ID) || accession.Equals(BinaryDataArray.IMS_SIGNED_64BIT_INTEGER_ID))
            {
                return DataType.INTEGER_64BIT;
            }
            else if (accession.Equals(BinaryDataArray.SIGNED_32BIT_INTEGER_ID) || accession.Equals(BinaryDataArray.IMS_SIGNED_32BIT_INTEGER_ID))
            {
                return DataType.INTEGER_32BIT;
            }
            else if (accession.Equals(BinaryDataArray.SIGNED_16BIT_INTEGER_ID))
            {
                return DataType.INTEGER_16BIT;
            }
            else if (accession.Equals(BinaryDataArray.SIGNED_8BIT_INTEGER_ID))
            {
                return DataType.INTEGER_8BIT;
            }

            return default;
        }        

        public DataType from;
        public DataType to;

        public DataTypeTransform(DataType from, DataType to)
        {
            this.from = from;
            this.to = to;
        }

        public static byte[] ConvertDoublesToBytes(double[] data)
        {
            byte[] convertedData = new byte[data.Length * 8];
            int j = 0;
            foreach (double aData in data)
            {
                long doubleVal = BitConverter.DoubleToInt64Bits(aData);
                convertedData[j++] = (byte)(doubleVal);
                convertedData[j++] = (byte)(doubleVal >>> 8);
                convertedData[j++] = (byte)(doubleVal >>> 16);
                convertedData[j++] = (byte)(doubleVal >>> 24);
                convertedData[j++] = (byte)(doubleVal >>> 32);
                convertedData[j++] = (byte)(doubleVal >>> 40);
                convertedData[j++] = (byte)(doubleVal >>> 48);
                convertedData[j++] = (byte)(doubleVal >>> 56);
            }
            return convertedData;
        }

        public static double[] ConvertDataToDouble(byte[] data, DataType dataType)
        {
            double[] convertedData = [];
            if (data == null)
            {
                return convertedData;
            }

            using (MemoryStream ms = new(data))
            using (BinaryReader reader = new(ms))
            {
                switch (dataType)
                {
                    case DataType.DOUBLE:
                        convertedData = new double[data.Length / 8];
                        for (int j = 0; j < convertedData.Length; j++)
                        {
                            convertedData[j] = reader.ReadDouble();
                        }
                        break;
                    case DataType.FLOAT:
                        convertedData = new double[data.Length / 4];
                        for (int j = 0; j < convertedData.Length; j++)
                        {
                            convertedData[j] = reader.ReadSingle();
                        }
                        break;
                    case DataType.INTEGER_64BIT:
                        convertedData = new double[data.Length / 8];
                        for (int j = 0; j < convertedData.Length; j++)
                        {
                            convertedData[j] = reader.ReadInt64();
                        }
                        break;
                    case DataType.INTEGER_32BIT:
                        convertedData = new double[data.Length / 4];
                        for (int j = 0; j < convertedData.Length; j++)
                        {
                            convertedData[j] = reader.ReadInt32();
                        }
                        break;
                    case DataType.INTEGER_16BIT:
                        convertedData = new double[data.Length / 2];
                        for (int j = 0; j < convertedData.Length; j++)
                        {
                            convertedData[j] = reader.ReadInt16();
                        }
                        break;
                    case DataType.INTEGER_8BIT:
                        convertedData = new double[data.Length];
                        for (int j = 0; j < convertedData.Length; j++)
                        {
                            convertedData[j] = reader.ReadSByte();
                        }
                        break;
                    default:
                        throw new InvalidOperationException("Data type not supported: " + dataType);
                }
            }
            return convertedData;
        }

        public static byte[] ConvertData(byte[] data, DataType from, DataType to)
        {
            if (from.Equals(to))
            {
                return data;
            }

            double[] doubleData = ConvertDataToDouble(data, from);            

            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms, Encoding.Default, true)) 
                {
                    switch (to)
                    {
                        case DataType.DOUBLE:
                            foreach (double dataPoint in doubleData)
                            {
                                writer.Write(dataPoint);
                            }
                            break;
                        case DataType.FLOAT:
                            foreach (double dataPoint in doubleData)
                            {
                                writer.Write((float)dataPoint);
                            }
                            break;
                        case DataType.INTEGER_64BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                writer.Write((long)dataPoint);
                            }
                            break;
                        case DataType.INTEGER_32BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                writer.Write((int)dataPoint);
                            }
                            break;
                        case DataType.INTEGER_16BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                writer.Write((short)dataPoint);
                            }
                            break;
                        case DataType.INTEGER_8BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                writer.Write((sbyte)dataPoint);
                            }
                            break;
                        default:
                            throw new InvalidOperationException("Data type not supported: " + to);
                    }
                }
                return ms.ToArray();
            }
        }

        public byte[] ForwardTransform(byte[] data)
        {
            return ConvertData(data, from, to);
        }

        public byte[] ReverseTransform(byte[] data)
        {
            return ConvertData(data, to, from);
        }

        public override string ToString()
        {
            return $"DataTypeTransform from {from} to {to}";
        }

    }    
}
