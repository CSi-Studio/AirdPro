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
using pwiz.CLI.cv;
using pwiz.CLI.msdata;
using SharpCompress;

namespace AirdPro.Utils
{
    public class DataUtil
    {
        public static int FetchIntensity(double target, int intensityPrecision)
        {
            int result = 0;
            double ori = target * intensityPrecision;
            if (ori <= int.MaxValue)
            {
                result = Convert.ToInt32(Math.Round(ori)); //整数后第一位
            }
            else
            {
                result = -Convert.ToInt32(Math.Log(ori) / Math.Log(2) * 100000);
            }

            return result;
        }

        public static int FetchMz(double target, int mzPrecision)
        {
            int result = -1;
            try
            {
                result = Convert.ToInt32(target * mzPrecision);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return result;
        }

        //yongy
        public static int FetchRt(double target, int rtPrecision)
        {
            int result = -1;
            try
            {
                result = Convert.ToInt32(target * rtPrecision);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }

            return result;
        }

        public static double[] GetMobilityData(Spectrum spectrum)
        {
            return spectrum.getArrayByCVID(CVID.MS_mean_ion_mobility_drift_time_array)?.data
                       .Storage() ??
                   spectrum.getArrayByCVID(CVID.MS_mean_inverse_reduced_ion_mobility_array)
                       ?.data.Storage() ??
                   spectrum.getArrayByCVID(CVID.MS_raw_ion_mobility_array)?.data.Storage() ??
                   spectrum.getArrayByCVID(CVID.MS_raw_inverse_reduced_ion_mobility_array)
                       ?.data.Storage();
        }

        public static int FindMaxIndex(List<float> targets)
        {
            int maxIndex = 0;
            float currentMax = float.MinValue;
            for (var i = 0; i < targets.Count; i++)
            {
                if (targets[i] > currentMax)
                {
                    maxIndex = i;
                    currentMax = targets[i];
                }
            }

            return maxIndex;
        }

        public static double GetMinMZ(SpectrumList spectrumList)
        {
            double minMZ = double.MaxValue;
            for (int i = 0; i < spectrumList.size(); i++)
            {
                spectrumList.spectrum(i).getMZArray().data.ForEach(mz =>
                {
                    if (mz < minMZ)
                    {
                        minMZ = mz;
                    }
                });
            }
            return minMZ;
        }

        public static double GetMaxMZ(SpectrumList spectrumList)
        {
            double maxMZ = double.MinValue;
            for (int i = 0; i < spectrumList.size(); i++)
            {
                spectrumList.spectrum(i).getMZArray().data.ForEach(mz =>
                {
                    if (mz > maxMZ)
                    {
                        maxMZ = mz;
                    }
                });
            }
            return maxMZ;
        }
    }
}