/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections.Generic;
using System;
using AirdSDK.Beans.Common;
using System.IO;
using AirdSDK.Bean.Msi.HeatMap;
using AirdSDK.Beans;

namespace AirdSDK.Parser;

public class MSIParser : DDAParser
{
    private const double TOLERANCE = 0.0015;
    private double mz;
    public List<DDAMs> msList;
    private List<ImageData> imageDataList;
    public double maxIntensity;    

    public MSIParser(string indexFilePath) : base(indexFilePath)
    {
        msList = ReadAllToMemory();
    }    

    /**
     * 返回值是一个map,其中key为rt,value为这个rt对应点原始谱图信息
     * 特别需要注意的是,本函数在使用完raf对象以后并不会直接关闭该对象,需要使用者在使用完DIAParser对象以后手动关闭该对象
     * <p>
     * the result key is rt,value is the spectrum(mz-intensity pairs) In particular, this function
     * will not close the RAF object directly after using it. Users need to close the object manually
     * after using the diaparser object
     *
     * @param start      起始指针位置 start point
     * @param end        结束指针位置 end point
     * @param rtList     rt列表,包含所有的光谱产出时刻 the retention time list
     * @param mzOffsets  mz块的大小列表 the mz block size list
     * @param intOffsets intensity块的大小列表 the intensity block size list
     * @return 每一个时刻对应的光谱信息 the spectrum of the target retention time
     */
    public new Dictionary<double, Spectrum> GetSpectra(long start, long end, List<double> rtList, List<int> mzOffsets,
        List<int> intOffsets)
    {
        Dictionary<double, Spectrum> map = new Dictionary<double, Spectrum>();
        fs.Seek(start, SeekOrigin.Begin);
        long delta = end - start;
        byte[] result = new byte[(int)delta];
        fs.Read(result, 0, result.Length);
        int iter = 0;
        for (int i = 0; i < rtList.Count; i++)
        {
            map.Add(i, GetSpectrum(result, iter, mzOffsets[i], intOffsets[i]));
            iter = iter + mzOffsets[i] + intOffsets[i];
        }

        return map;
    }

    public new List<DDAMs> ReadAllToMemory()
    {
        BlockIndex ms1Index = GetMs1Index(); //所有的ms1谱图都在第一个index中
        for (int i = 0; i < ms1Index.nums.Count; i++)
        {
            ms1Index.rts[i] = i + 1;
        }
        Dictionary<double, Spectrum> ms1Map = GetSpectra(ms1Index);
        List<double> ms1RtList = new List<double>(ms1Map.Keys);
        List<DDAMs> ms1List = BuildDdaMsList(ms1RtList, 0, ms1RtList.Count, ms1Index, ms1Map, false);
        return ms1List;
    }

    public List<ImageData> GetImageDataList(double mz)
    {
        imageDataList = new List<ImageData>();
        maxIntensity = 0;
        this.mz = mz;
        int[] x = airdInfo.msiInfo.spectraPosition.x;
        int[] y = airdInfo.msiInfo.spectraPosition.y;
        
        for (int index = 0; index < msList.Count; index++)
        {
            double[] mzArray = msList[index].spectrum.mzs;
            double[] intArray = msList[index].spectrum.ints;

            double intensity = 0;
            for (int i = 0; i < mzArray.Length; i++)
            {
                if (Math.Abs(mzArray[i] - mz) <= TOLERANCE)
                {
                    intensity += intArray[i];
                }
                if(intensity > maxIntensity)
                {
                    maxIntensity = intensity;
                }
            }
            imageDataList.Add(new ImageData()
            {
                X = x[index],
                Y = y[index],
                Intensity = intensity
            });

        }
        return imageDataList;
    }
}