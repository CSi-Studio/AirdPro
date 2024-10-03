using CSharpFastPFOR.Port;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.csimzMLParser.util
{
    public class UUIDHelper
    {

        private UUIDHelper()
        {
            
        }

        public static Guid ByteArrayToUuid(byte[] bytes)
        {
            // Ensure the byte array is exactly 16 bytes long
            if (bytes.Length != 16)
            {
                throw new ArgumentException("Byte array must be exactly 16 bytes in length.", nameof(bytes));
            }

            // Use BitConverter to copy the bytes into the correct position
            byte[] guidBytes = new byte[16];
            Array.Copy(bytes, guidBytes, 16);

            // Reverse the byte array if necessary to match endianness
            Array.Reverse(guidBytes, 0, 4);
            Array.Reverse(guidBytes, 4, 2);
            Array.Reverse(guidBytes, 6, 2);
            Array.Reverse(guidBytes, 8, 2);
            Array.Reverse(guidBytes, 10, 6);

            return new Guid(guidBytes);
        }

        public static byte[] UuidToByteArray(Guid uuid)
        {
            byte[] bytes = uuid.ToByteArray();
            // If necessary, reverse the byte order to match the Java version
            byte[] orderedBytes = new byte[16];
            Array.Copy(bytes, orderedBytes, 16);
            Array.Reverse(orderedBytes, 0, 4);
            Array.Reverse(orderedBytes, 4, 2);
            Array.Reverse(orderedBytes, 6, 2);
            Array.Reverse(orderedBytes, 8, 2);
            Array.Reverse(orderedBytes, 10, 6);
            return orderedBytes;
        }
    }
}
