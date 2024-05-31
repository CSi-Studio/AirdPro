/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Runtime.InteropServices;

namespace AirdSDK.Compressor
{
    public class ByteTrans
    {
        //将short数组转化为byte数组
        public static byte[] shortToByte(short[] src)
        {
            var bytes = new byte[src.Length * 2];

            for (var i = 0; i < src.Length; i++)
            {
                var value = src[i];
                bytes[i * 2] = (byte)value;
                bytes[(i * 2) + 1] = (byte)(value >> 8);
            }

            return bytes;
        }

        //将int数组转化为byte数组
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
        
        //将long数组转化为byte数组
        public static byte[] longToByte(long[] src)
        {
            int byteLength = src.Length * sizeof(long);
            byte[] byteArray = new byte[byteLength];
            Buffer.BlockCopy(src, 0, byteArray, 0, byteLength);
            return byteArray;
        }

        public static long[] byteToLong(byte[] byteArray)
        {
            int longCount = byteArray.Length / sizeof(long);
            long[] longArray = new long[longCount];

            Buffer.BlockCopy(byteArray, 0, longArray, 0, byteArray.Length);

            for (int i = 0; i < longCount; i++)
            {
                longArray[i] = BitConverter.ToInt64(byteArray, i * sizeof(long));
            }

            return longArray;
        }
        
        //将float数组转化为byte数组
        public static byte[] floatToByte(float[] src)
        {
            var bytes = new byte[src.Length * 4];
            for (var i = 0; i < src.Length; i++)
            {
                BitConverter.GetBytes(src[i]).CopyTo(bytes, i * 4);
            }

            return bytes;
        }

        //将byte数组转化为float数组,使用BigEndian进行转换
        public static float[] byteToFloat(byte[] src)
        {
            float[] fArray = new float[src.Length / 4];
            for (int i = 0; i < fArray.Length; i++)
            {
                fArray[i] = BitConverter.ToSingle(src, i * 4);
            }

            return fArray;
        }

        //将byte数组转化为float数组,使用BigEndian进行转换
        public static int[] byteToInt(byte[] src)
        {
            var intArray = MemoryMarshal.Cast<byte, int>(src);
            return intArray.ToArray();
        }
    }
}