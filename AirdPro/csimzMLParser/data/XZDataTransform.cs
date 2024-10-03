using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using XZ.NET;

namespace AirdPro.csimzMLParser.data
{
    public class XZDataTransform : IDataTransform
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(XZDataTransform));

        public byte[] ForwardTransform(byte[] data)
        {
            MemoryStream outputStream = new MemoryStream();
            using (var xzStream = new XZOutputStream(outputStream))
            {
                try
                {
                    xzStream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    logger.Error("Error during XZ compression", ex);
                }
            }

            outputStream.Position = 0;
            return outputStream.ToArray();
        }

        public byte[] ReverseTransform(byte[] data)
        {
            List<byte> uncompressedData = new List<byte>(data.Length);
            try
            {
                using (var xzInputStream = new XZInputStream(new MemoryStream(data)))
                {
                    int firstByte;
                    while ((firstByte = xzInputStream.ReadByte()) != -1)
                    {
                        byte[] temp = new byte[xzInputStream.Length];
                        temp[0] = (byte)firstByte;
                        xzInputStream.Read(temp, 1, temp.Length - 1);

                        uncompressedData.AddRange(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error during XZ decompression", ex);
            }

            return uncompressedData.ToArray();
        }
    }
}
