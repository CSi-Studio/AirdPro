using System;
using System.Text;

namespace AirdPro.csimzMLParser.util
{
    public static class HexHelper
    {
        public static byte[] HexStringToByteArray(string s)
        {
            int len = s.Length;
            byte[] data = new byte[len / 2];

            for (int i = 0; i < len; i += 2)
            {
                data[i / 2] = (byte)((Convert.ToInt32(s[i].ToString(), 16) << 4)
                                     + Convert.ToInt32(s[i + 1].ToString(), 16));
            }

            return data;
        }

        public static string ByteArrayToHexString(byte[] byteArray)
        {
            StringBuilder sb = new StringBuilder(byteArray.Length * 2);

            for (int i = 0; i < byteArray.Length; i++)
            {
                int val = byteArray[i] & 0xff;
                sb.Append(Convert.ToString(val, 16).PadLeft(2, '0'));
            }

            return sb.ToString();
        }
    }
}
