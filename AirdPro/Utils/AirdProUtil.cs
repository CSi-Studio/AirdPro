using System;
using System.Runtime.InteropServices;

namespace AirdPro.Utils;

public class AirdProUtil
{
    public static byte[] IntToByte(int[] src)
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
    
    public static int[] ByteToInt(byte[] src)
    {
        var intArray = MemoryMarshal.Cast<byte, int>(src);
        return intArray.ToArray();
    }
    
    public static float SumValuesAtIndices(int[] arrA, float[] arrB, int target)
    {
        float sum = 0;

        int left = 0;
        int right = arrA.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;

            if (arrA[mid] == target)
            {
                // 找到目标值后，向左和向右搜索重复的目标值
                int i = mid - 1;

                while (i >= 0 && arrA[i] == target)
                {
                    sum += arrB[i];
                    i--;
                }

                sum += arrB[mid];

                i = mid + 1;

                while (i < arrA.Length && arrA[i] == target)
                {
                    sum += arrB[i];
                    i++;
                }

                break;
            }
            else if (arrA[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return sum;
    }
}