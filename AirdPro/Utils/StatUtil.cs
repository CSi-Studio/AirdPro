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
using System.Diagnostics;
using AirdPro.Domains;
using AirdSDK.Compressor;

namespace AirdPro.Utils;

public class StatUtil
{
    List<double> minMaxNorm(List<double> numbers)
    {
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
        List<double> normList = new List<double>();
        for (var i = 0; i < numbers.Count; i++)
        {
            normList.Add((numbers[i] - min) / delta);
        }

        return normList;
    }

    public static void stat4OneComboComp(BaseComp<int> intComp, ByteComp byteComp, List<int[]> arrays, string dim,
        Dictionary<string, long> sizeMap, Dictionary<string, long> compressTimeMap,
        Dictionary<string, long> decompressTimeMap)
    {
        string key = buildComboKey(dim, intComp.getName(), byteComp.getName());
        Stopwatch watchMz = new Stopwatch();
        int tempMzSize = 0;
        watchMz.Start();
        List<byte[]> encodeList = new List<byte[]>();
        for (int i = 0; i < arrays.Count; i++)
        {
            byte[] compMz = ComboComp.encode(intComp, byteComp, arrays[i]);
            tempMzSize += compMz.Length;
            encodeList.Add(compMz);
        }

        sizeMap[key] = tempMzSize;
        compressTimeMap[key] = watchMz.Elapsed.Ticks;
        watchMz.Restart();
        for (int i = 0; i < encodeList.Count; i++)
        {
            int[] mz = ComboComp.decode(intComp, byteComp, encodeList[i]);
            if (mz.Length != arrays[i].Length)
            {
                Console.WriteLine("Encoding Error");
            }
        }

        decompressTimeMap[key] = watchMz.Elapsed.Ticks;
        watchMz.Stop();
    }

    public static void calcBest(List<CompressStat> statList)
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
        Stat mzStat = new Stat(sizeList);
        Stat ctStat = new Stat(ctList);
        Stat dtStat = new Stat(dtList);
    }

    public static string buildComboKey(string key, string intCompName, string byteCompName)
    {
        return key + "-" + intCompName + "-" + byteCompName;
    }
}