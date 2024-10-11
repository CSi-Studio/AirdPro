using System;

namespace AirdPro.csimzMLParser.data
{
    public class MSNumpressDataTransform : IDataTransform
    {
        public enum NumpressAlgorithm
        {
            LINEAR,
            PIC,
            SLOF
        }

        private readonly NumpressAlgorithm algorithm;
        private string accession;
        private double mzError = 1e5;

        public MSNumpressDataTransform(NumpressAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case NumpressAlgorithm.LINEAR:
                    accession = MSNumpress.ACC_NUMPRESS_LINEAR;
                    break;
                case NumpressAlgorithm.PIC:
                    accession = MSNumpress.ACC_NUMPRESS_PIC;
                    break;
                case NumpressAlgorithm.SLOF:
                    accession = MSNumpress.ACC_NUMPRESS_SLOF;
                    break;
            }

            this.algorithm = algorithm;
        }
        public byte[] ForwardTransform(byte[] data)
        {
            byte[] encoded = new byte[data.Length];
            double[] dataAsDouble = DataTypeTransform.ConvertDataToDouble(data, DataType.DOUBLE);
            int numBytes = -1;

            switch (algorithm)
            {
                case NumpressAlgorithm.LINEAR:
                    numBytes = MSNumpress.encodeLinear(dataAsDouble, dataAsDouble.Length, encoded, mzError);

                    break;
                case NumpressAlgorithm.PIC:
                    numBytes = MSNumpress.encodePic(dataAsDouble, dataAsDouble.Length, encoded);

                    break;
                case NumpressAlgorithm.SLOF:
                    double fixedPoint = MSNumpress.optimalSlofFixedPoint(dataAsDouble, dataAsDouble.Length);
                    numBytes = MSNumpress.encodeSlof(dataAsDouble, dataAsDouble.Length, encoded, fixedPoint);

                    break;
            }

            if (numBytes >= 0 && numBytes != encoded.Length)
            {
                byte[] truncatedEncoded = new byte[numBytes];
                Array.Copy(encoded, truncatedEncoded, numBytes);
                encoded = truncatedEncoded;
            }

            return encoded;
        }

        public byte[] ReverseTransform(byte[] data) 
        {
            double[] result = MSNumpress.decode(accession, data, data.Length);
            return DataTypeTransform.ConvertDoublesToBytes(result);
        }
    }
}
