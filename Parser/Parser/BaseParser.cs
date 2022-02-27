/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.DomainsCore.Aird;
using AirdPro.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using AirdPro.Constants;
using AirdPro.DomainsCore.Parser;
using System.Linq;
using AirdPro.Algorithms;
using Compress;

namespace AirdPro.Parser
{
    public class BaseParser
    {
        public FileStream airdFile;

        public AirdInfo airdInfo;

        public Compressor mzCompressor;

        public Compressor intCompressor;

        public int mzPrecision;

        public string type;

        public BaseParser()
        {

        }

        public BaseParser(string indexPath)
        {
            airdInfo = AirdScanUtil.loadAirdInfo(indexPath);
            if (airdInfo == null)
            {
                throw new Exception(ResultCodeEnum.AIRD_INDEX_FILE_PARSE_ERROR.Message);
            }
            try
            {
                airdFile = File.OpenRead(FileNameUtil.getAirdPathByIndexPath(indexPath));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception(ResultCodeEnum.AIRD_FILE_PARSE_ERROR.Message);
            }

            mzCompressor = getMzCompressor(airdInfo.compressors);
            intCompressor = getIntCompressor(airdInfo.compressors);
            mzPrecision = mzCompressor.precision;
            type = airdInfo.type;
        }

        /**
        * 使用直接的关键信息进行初始化
        *
        * @param airdPath      Aird文件路径
        * @param mzCompressor  mz压缩策略
        * @param intCompressor intensity压缩策略
        * @param mzPrecision   mz数字精度
        * @param airdType      aird类型
        * @throws ScanException 扫描异常
        */
        public BaseParser(String airdPath, Compressor mzCompressor, Compressor intCompressor, int mzPrecision, String airdType)
        {
            var indexFile = FileNameUtil.getIndexPathByAirdPath(airdPath);
            airdInfo = AirdScanUtil.loadAirdInfo(indexFile);
            try
            {
                airdFile = File.OpenRead(airdPath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
                throw new Exception(ResultCodeEnum.AIRD_FILE_PARSE_ERROR.Message);
            }
            this.mzCompressor = mzCompressor;
            this.intCompressor = intCompressor;
            this.mzPrecision = mzPrecision;
            this.type = airdType;
        }

        public AirdInfo getAirdInfo()
        {
            return airdInfo;
        }

        /// <summary>
        /// 根据特定BlockIndex取出对应SortedDictionary
        /// </summary>
        /// <param name="file">filereader</param>
        /// <param name="blockIndex"> the block index read from the index file </param>
        /// <returns> 解码内容, key为rt, value为光谱中的键值对 </returns>
        /// <exception cref="Exception"> 读取文件异常 </exception>
        public virtual SortedDictionary<double, MzIntensityPairs> parseBlockValue(FileStream file, BlockIndex blockIndex)
        {
            SortedDictionary<double, MzIntensityPairs> map = new SortedDictionary<double, MzIntensityPairs>();
            IList<float> rts = blockIndex.rts;

            file.Seek(blockIndex.startPtr, SeekOrigin.Begin);
            long delta = blockIndex.endPtr - blockIndex.startPtr;
            byte[] result = new byte[(int)delta];
            file.Read(result, 0, (int)delta);
            IList<long> mzSizes = blockIndex.mzs;
            IList<long> intensitySizes = blockIndex.ints;

            int start = 0;
            for (int i = 0; i < mzSizes.Count; i++)
            {
                var mz = new byte[(int)mzSizes[i]];
                Buffer.BlockCopy(result, start, mz, 0, ((int)mzSizes[i]));
                start = start + ((int)mzSizes[i]);

                var intensity = new byte[(int)intensitySizes[i]];
                Buffer.BlockCopy(result, start, intensity, 0, ((int)intensitySizes[i]));
                start = start + ((int)intensitySizes[i]);
                try
                {
                    MzIntensityPairs pairs = new MzIntensityPairs(getMzValues(mz), getIntValues(intensity));
                    map[rts[i] / 60d] = pairs;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return map;
        }

        /// <summary>
        /// get mz values only for aird file
        /// 默认从Aird文件中读取,编码Order为LITTLE_ENDIAN,精度为小数点后三位
        /// </summary>
        /// <param name="value"> 压缩后的数组 </param>
        /// <returns> 解压缩后的数组 </returns>
        public float[] getMzValues(byte[] value)
        {
            var values = mzInt.decodeToInt(value);
            var intValues = IntegratedBinPacking.decode(values);
            float[] floatValues = new float[intValues.Length];
            for (int index = 0; index < intValues.Length; index++)
            {
                floatValues[index] = (float)intValues[index] / mzPrecision;
            }
            return floatValues;
        }


        /// <summary>
        /// get mz values only for aird file
        /// 默认从Aird文件中读取,编码Order为LITTLE_ENDIAN,精度为小数点后三位
        /// </summary>
        /// <param name="value">  压缩后的数组 </param>
        /// <param name="start">  起始位置 </param>
        /// <param name="length"> 读取长度 </param>
        /// <returns> 解压缩后的数组 </returns>
        public float[] getMzValues(byte[] value, int start, int length)
        {
            var values = Zlib.decodeToInt(value.Skip(start).Take(length).ToArray());
            var intValues = IntegratedBinPacking.decode(values);
            float[] floatValues = new float[intValues.Length];
            for (int index = 0; index < intValues.Length; index++)
            {
                floatValues[index] = (float)intValues[index] / mzPrecision;
            }
            return floatValues;
        }


        /// <summary>
        /// get mz values only for aird file
        /// 默认从Aird文件中读取,编码Order为LITTLE_ENDIAN
        /// </summary>
        /// <param name="value"> 加密的数组 </param>
        /// <returns> 解压缩后的数组 </returns>
        public int[] getMzValuesAsInteger(byte[] value)
        {
            var intValues = Zlib.decodeToInt(value);
            intValues = IntegratedBinPacking.decode(intValues);
            return intValues;
        }


        public int[] getTags(byte[] value)
        {
            var byteBuffer = Zlib.decode(value);
            byte[] byteValue = new byte[byteBuffer.Length * 8];
            for (int i = 0; i < byteBuffer.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    byteValue[8 * i + j] = (byte)(((byteBuffer[i] & 0xff) >> j) & 1);
                }
            }
            int digit = mzCompressor.digit;
            int[] tags = new int[byteValue.Length / digit];
            for (int i = 0; i < tags.Length; i++)
            {
                for (int j = 0; j < digit; j++)
                {
                    tags[i] += byteValue[digit * i + j] << j;
                }
            }
            return tags;
        }

        public int[] getTags(byte[] value, int start, int length)
        {
            var tagShift = Zlib.decode(value.Skip(start).Take(length).ToArray());
            byte[] byteValue = new byte[tagShift.Length * 8];
            for (int i = 0; i < tagShift.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    byteValue[8 * i + j] = (byte)(((tagShift[i] & 0xff) >> j) & 1);
                }
            }
            int digit = mzCompressor.digit;
            int[] tags = new int[byteValue.Length / digit];
            for (int i = 0; i < tags.Length; i++)
            {
                for (int j = 0; j < digit; j++)
                {
                    tags[i] += byteValue[digit * i + j] << j;
                }
            }
            return tags;
        }

        /// <summary>
        /// get intensity values only for aird file
        /// </summary>
        /// <param name="value"> 压缩的数组 </param>
        /// <returns> 解压缩后的数组 </returns>
        public float[] getIntValues(byte[] value)
        {
            return Zlib.decodeToFloat(value);
        }

        /// <summary>
        /// get intensity values only for aird file
        /// </summary>
        /// <param name="value"> 压缩的数组 </param>
        /// <param name="start">  起始位置 </param>
        /// <param name="length"> 读取长度 </param>
        /// <returns> 解压缩后的数组 </returns>
        public float[] getIntValues(byte[] value, int start, int length)
        {
            return Zlib.decodeToFloat(value.Skip(start).Take(length).ToArray());
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

        public void close()
        {
            airdFile.Close();
        }
    }
}