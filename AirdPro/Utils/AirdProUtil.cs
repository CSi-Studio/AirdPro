using System;
using System.Collections.Generic;

namespace AirdPro.Utils;

public class AirdProUtil
{
    public static byte[] intToByte(int[] src)
    {
        byte[] byteArray = new byte[src.Length * 4];
        Span<byte> byteSpan = byteArray.AsSpan();

        for (int index = 0; index < src.Length; ++index)
        {
            int value = src[index];
            byteSpan[index * 4] = (byte)value;
            byteSpan[(index * 4) + 1] = (byte)(value >> 8);
            byteSpan[(index * 4) + 2] = (byte)(value >> 16);
            byteSpan[(index * 4) + 3] = (byte)(value >> 24);
        }

        return byteArray;
    }
}