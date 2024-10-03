using AirdPro.csimzMLParser.obo;
using log4net;
using System;
using System.IO;
using System.Text;
using static AirdPro.csimzMLParser.mzml.BinaryDataArray;

namespace AirdPro.csimzMLParser.data
{    
    public enum DataType
    {
        DOUBLE,
        FLOAT,
        INTEGER_8BIT,
        INTEGER_16BIT,
        INTEGER_32BIT,
        INTEGER_64BIT
    }
    public static class DataTypeExtensions
    {        
        public static OBOTerm ToOBOTerm(this DataType dataType)
        {
            switch (dataType)
            {
                case DataType.DOUBLE:
                    return OBO.GetOBO().GetTerm(Accessions.DOUBLE_PRECISION_ID);
                case DataType.FLOAT:
                    return OBO.GetOBO().GetTerm(Accessions.SINGLE_PRECISION_ID);
                case DataType.INTEGER_64BIT:
                    return OBO.GetOBO().GetTerm(Accessions.SIGNED_64BIT_INTEGER_ID);
                case DataType.INTEGER_32BIT:
                    return OBO.GetOBO().GetTerm(Accessions.SIGNED_32BIT_INTEGER_ID);
                case DataType.INTEGER_16BIT:
                    return OBO.GetOBO().GetTerm(Accessions.SIGNED_16BIT_INTEGER_ID);
                case DataType.INTEGER_8BIT:
                    return OBO.GetOBO().GetTerm(Accessions.SIGNED_8BIT_INTEGER_ID);
                default:
                    return null;
            }
        }

        public static DataType FromOBOTerm(OBOTerm term)
        {
            string accession = term.id;

            if (accession == Accessions.DOUBLE_PRECISION_ID)
            {
                return DataType.DOUBLE;
            }
            else if (accession == Accessions.SINGLE_PRECISION_ID)
            {
                return DataType.FLOAT;
            }
            else if (accession == Accessions.SIGNED_64BIT_INTEGER_ID || accession == Accessions.IMS_SIGNED_64BIT_INTEGER_ID)
            {
                return DataType.INTEGER_64BIT;
            }
            else if (accession == Accessions.SIGNED_32BIT_INTEGER_ID || accession == Accessions.IMS_SIGNED_32BIT_INTEGER_ID)
            {
                return DataType.INTEGER_32BIT;
            }
            else if (accession == Accessions.SIGNED_16BIT_INTEGER_ID)
            {
                return DataType.INTEGER_16BIT;
            }
            else if (accession == Accessions.SIGNED_8BIT_INTEGER_ID)
            {
                return DataType.INTEGER_8BIT;
            }

            return default(DataType);
        }
    }

    [Serializable]
    public class DataTypeTransform : IDataTransform
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DataTypeTransform));

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
                for (int i = 0; i < 8; i++)
                {
                    convertedData[j++] = (byte)(doubleVal >> (8 * i));
                }
            }
            return convertedData;
        }

        public static double[] ConvertDataToDouble(byte[] data, DataType dataType)
        {
            if (data == null || data.Length == 0)
            {
                return Array.Empty<double>();
            }

            using (MemoryStream memoryStream = new(data))
            using (BinaryReader reader = new(memoryStream))
            {
                double[] convertedData;

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
                        throw new NotSupportedException("Data type not supported: " + dataType);
                }

                return convertedData;
            }
        }

        public static byte[] ConvertData(byte[] data, DataType from, DataType to)
        {
            if (from == to)
            {
                return data;
            }

            double[] doubleData = ConvertDataToDouble(data, from);

            MemoryStream memoryStream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(memoryStream, Encoding.Unicode, true))
            {
                try
                {
                    switch (to)
                    {
                        case DataType.DOUBLE:
                            foreach (double dataPoint in doubleData)
                            {
                                byte[] bytes = BitConverter.GetBytes(dataPoint);
                                if (!BitConverter.IsLittleEndian)
                                {
                                    Array.Reverse(bytes);
                                }
                                writer.Write(bytes);
                            }
                            break;

                        case DataType.FLOAT:
                            foreach (double dataPoint in doubleData)
                            {
                                byte[] bytes = BitConverter.GetBytes((float)dataPoint);
                                if (!BitConverter.IsLittleEndian)
                                {
                                    Array.Reverse(bytes);
                                }
                                writer.Write(bytes);
                            }
                            break;

                        case DataType.INTEGER_64BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                long value = (long)dataPoint;
                                byte[] bytes = BitConverter.GetBytes(value);
                                if (!BitConverter.IsLittleEndian)
                                {
                                    Array.Reverse(bytes);
                                }
                                writer.Write(bytes);
                            }
                            break;

                        case DataType.INTEGER_32BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                int value = (int)dataPoint;
                                byte[] bytes = BitConverter.GetBytes(value);
                                if (!BitConverter.IsLittleEndian)
                                {
                                    Array.Reverse(bytes);
                                }
                                writer.Write(bytes);
                            }
                            break;

                        case DataType.INTEGER_16BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                short value = (short)dataPoint;
                                byte[] bytes = BitConverter.GetBytes(value);
                                if (!BitConverter.IsLittleEndian)
                                {
                                    Array.Reverse(bytes);
                                }
                                writer.Write(bytes);
                            }
                            break;

                        case DataType.INTEGER_8BIT:
                            foreach (double dataPoint in doubleData)
                            {
                                byte value = (byte)dataPoint;
                                writer.Write(value);
                            }
                            break;

                        default:
                            throw new NotSupportedException("Data type not supported: " + to.ToString());
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(null, ex);
                }
            }

            // Return the byte array from the MemoryStream
            return memoryStream.ToArray();
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
