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
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using AirdPro.Domains;
using AirdSDK.Compressor;

namespace AirdPro.Utils
{
    public class StatUtil
    {
        public static List<double> minMaxNorm(List<double> numbers)
        {
            if (numbers.Count == 0)
            {
                return new List<double>();
            }

            double min = numbers[0];
            double max = numbers[0];
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] > max)
                {
                    max = numbers[i];
                }

                if (numbers[i] < min)
                {
                    min = numbers[i];
                }
            }

            double delta = max - min;
            if (delta == 0)
            {
                throw new Exception("max - min = 0!");
            }

            List<double> normList = new List<double>();
            for (var i = 0; i < numbers.Count; i++)
            {
                normList.Add((numbers[i] - min) / delta);
            }

            return normList;
        }

        public static void stat4ComboComp(BaseComp<int> intComp, ByteComp byteComp, List<int[]> arrays, string key,
            Dictionary<string, long> sizeMap, Dictionary<string, long> compressTimeMap,
            Dictionary<string, long> decompressTimeMap)
        {
            // string key = buildComboKey(dim, intComp.getName(), byteComp.getName());
            Stopwatch watch = new Stopwatch();
            int tempSize = 0;
            watch.Start();
            List<byte[]> encodeList = new List<byte[]>();
            for (int i = 0; i < arrays.Count; i++)
            {
                byte[] comp = ComboComp.encode(intComp, byteComp, arrays[i]);
                tempSize += comp.Length;
                encodeList.Add(comp);
            }

            sizeMap[key] = tempSize;
            compressTimeMap[key] = watch.Elapsed.Ticks;
            watch.Restart();
            for (int i = 0; i < encodeList.Count; i++)
            {
                int[] decodeList = ComboComp.decode(intComp, byteComp, encodeList[i]);

                //测试总长度和首位数据是否一致
                if (decodeList.Length != arrays[i].Length || decodeList[0] != arrays[i][0] || decodeList[decodeList.Length-1] != arrays[i][decodeList.Length - 1])
                {
                    MessageBox.Show("数据不一致: Encoding Error");
                }
            }

            decompressTimeMap[key] = watch.Elapsed.Ticks;
            watch.Stop();
        }

        public static void stat4HuffmanCode(BaseComp<int> intComp, ByteComp byteComp, List<int[]> arrays, string dim,
            Dictionary<string, long> sizeMap, Dictionary<string, long> compressTimeMap,
            Dictionary<string, long> decompressTimeMap)
        {
            string key = buildComboKey(dim, intComp.getName(), byteComp.getName());
            Stopwatch watchMz = new Stopwatch();
            watchMz.Start();
            int[] tempMobiHuffArray = HuffmanCoder.toIntArray(arrays);
            HuffmanTree huffmanTree = HuffmanCoder.buildTree(tempMobiHuffArray);
            byte[] tempMobiHuffByte = HuffmanCoder.encode(tempMobiHuffArray, huffmanTree);
            byte[] compressed = new ZstdWrapper().encode(tempMobiHuffByte);
            sizeMap[key] = compressed.Length;
            compressTimeMap[key] = watchMz.Elapsed.Ticks;
            watchMz.Restart();
            int[] decodedMobiArray = HuffmanCoder.decode(tempMobiHuffByte, huffmanTree);
            if (decodedMobiArray.Length != tempMobiHuffArray.Length)
            {
                Console.WriteLine("Encoding Error");
            }

            decompressTimeMap[key] = watchMz.Elapsed.Ticks;
            watchMz.Stop();
        }

        public static int calcBestIndex(List<CompressStat> statList)
        {
            statList.Sort((a, b) => a.size.CompareTo(b.size));
            List<double> sizeList = new List<double>();
            List<double> ctList = new List<double>();
            List<double> dtList = new List<double>();
            statList.ForEach((stat) =>
            {
                sizeList.Add(stat.size);
                ctList.Add(stat.compressTime);
                dtList.Add(stat.decompressTime);
            });

            List<double> normSize = minMaxNorm(sizeList);
            List<double> normCt = minMaxNorm(ctList);
            List<double> normDt = minMaxNorm(dtList);
            Stat sizeStat = new Stat(normSize);
            // Stat ctStat = new Stat(normCt);
            // Stat dtStat = new Stat(normDt);
            int endIndex = -1;
            for (int i = 0; i < sizeStat.size; i++)
            {
                if (sizeStat.dataList[i] < sizeStat.mean)
                {
                    endIndex = i;
                }
            }

            double bestValue = 3;
            int bestIndex = -1;
            List<double> totalList = new List<double>();
            for (int i = 0; i <= endIndex; i++)
            {
                double total = normSize[i] + normCt[i] + normDt[i];
                totalList.Add(total);
                if (total < bestValue)
                {
                    bestIndex = i;
                    bestValue = total;
                }
            }

            return bestIndex;
        }

        public static string buildComboKey(string key, string intCompName, string byteCompName)
        {
            return key + "-" + intCompName + "-" + byteCompName;
        }
    }
}