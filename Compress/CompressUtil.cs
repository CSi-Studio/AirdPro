/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;
using System.IO;
using AirdPro.DomainsCore.Aird;
using CSharpFastPFOR;
using CSharpFastPFOR.Differential;
using CSharpFastPFOR.Port;
using Ionic.Zlib;

namespace AirdPro.Utils
{
    public class CompressUtil
    {
        //使用zlib将byte数组压缩
        public static byte[] zlibEncoder(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                Stream compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Default);

                ZlibBaseStream.CompressBuffer(data, compressor);
                return ms.ToArray();
            }
        }

        //使用zlib将int数组压缩为byte数组
        public static byte[] zlibEncoder(int[] target)
        {
            var targetArray = intToByte(target);
            var compressedArray = zlibEncoder(targetArray);
            return compressedArray;
        }

        //使用zlib将float数组压缩为byte数组
        public static byte[] zlibEncoder(float[] target)
        {
            var targetArray = floatToByte(target);
            var compressedArray = zlibEncoder(targetArray);
            return compressedArray;
        }

        //使用zlib将byte数组解压缩
        public static byte[] zlibDecoder(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }

        //使用zlib将byte数组解压缩为float数组
        public static float[] zlibDecoderToFloat(byte[] compressed)
        {
            byte[] array = ZlibStream.UncompressBuffer(compressed);
            return byteToFloat(array);
        }

        //使用zlib将byte数组解压缩为int数组
        public static int[] zlibDecoderToInt(byte[] compressed)
        {
            byte[] array = ZlibStream.UncompressBuffer(compressed);
            return byteToInt(array);
        }

        //使用FastPfor算法将排序了的int数组进行压缩,注意:target数组必须是排序后的数组
        public static int[] fastPforEncoder(int[] uncompressed)
        {
            int[] compressedInts = new IntegratedIntCompressor().compress(uncompressed);
            return compressedInts;
            // var codec = new SkippableIntegratedComposition(new IntegratedBinaryPacking(), new IntegratedVariableByte());
            // var compressed = new int[uncompressed.Length + 1024];
            // var inputoffset = new IntWrapper(0);
            // var outputoffset = new IntWrapper(1);
            // codec.headlessCompress(uncompressed, inputoffset, uncompressed.Length, compressed, outputoffset, new IntWrapper(0));
            // compressed[0] = uncompressed.Length;
            // compressed = Arrays.copyOf(compressed, outputoffset.intValue());
            //
            // return compressed;
        }

        //使用VariableByte算法将排序了的int数组进行压缩
        public static int[] varbyteEncoder(int[] uncompressed)
        {
            int[] compressedInts = new IntegratedIntCompressor(new IntegratedVariableByte()).compress(uncompressed);
            return compressedInts;
        }

        //使用PFor算法对已经压缩的int数组进行解压缩
        public static int[] fastPforDecoder(int[] compressed)
        {
            // var codec = new SkippableIntegratedComposition(new IntegratedBinaryPacking(), new IntegratedVariableByte());
            // var size = compressed[0];
            // // output vector should be large enough...
            // var recovered = new int[size];
            // var inPoso = new IntWrapper(1);
            // var outPoso = new IntWrapper(0);
            // var recoffset = new IntWrapper(0);
            // codec.headlessUncompress(compressed, inPoso, compressed.Length, recovered, recoffset, size, outPoso);
            // return recovered;
            int[] sortedInts = new IntegratedIntCompressor().uncompress(compressed);
            return sortedInts;
        }

        //使用VariableByte算法对已经压缩的int数组进行解压缩
        public static int[] varbyteDecoder(int[] compressed)
        {
            int[] sortedInts = new IntegratedIntCompressor(new IntegratedVariableByte()).uncompress(compressed);
            return sortedInts;
        }

        //将int数组转化为byte数组
        public static byte[] intToByte(int[] src)
        {
            var bytes = new byte[src.Length * 4];
            for (var i = 0; i < src.Length; i++)
            {
                BitConverter.GetBytes(src[i]).CopyTo(bytes, i * 4);
            }
            return bytes;
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
            int[] fArray = new int[src.Length / 4];
            for (int i = 0; i < fArray.Length; i++)
            {
                fArray[i] = BitConverter.ToInt32(src, i * 4);
            }

            return fArray;
        }

        /// <summary>
        /// get the compressor for m/z
        /// </summary>
        /// <param name="compressors"> 压缩策略 </param>
        /// <returns> the m/z compressor </returns>
        public static Compressor getMzCompressor(IList<Compressor> compressors)
        {
            if (compressors == null)
            {
                return null;
            }
            foreach (Compressor compressor in compressors)
            {
                if (compressor.target.Equals(Compressor.TARGET_MZ))
                {
                    return compressor;
                }
            }
            return null;
        }

        /// <summary>
        /// get the intensity compressor for intensity
        /// </summary>
        /// <param name="compressors"> 压缩策略 </param>
        /// <returns> the intensity compressor </returns>
        public static Compressor getIntCompressor(IList<Compressor> compressors)
        {
            if (compressors == null)
            {
                return null;
            }
            foreach (Compressor compressor in compressors)
            {
                if (compressor.target.Equals(Compressor.TARGET_INTENSITY))
                {
                    return compressor;
                }
            }
            return null;
        }

    }
}