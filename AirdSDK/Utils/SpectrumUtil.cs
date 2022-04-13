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
using System.Diagnostics;
using pwiz.CLI.cv;
using pwiz.CLI.msdata;

namespace AirdPro.Utils;

public class SpectrumUtil
{
    public static int fetchIntensity(double target, int intensityPrecision)
    {
        int result = 0;
        double ori = target * intensityPrecision;
        if (ori <= int.MaxValue)
        {
            result = Convert.ToInt32(Math.Round(ori)); //精确到小数点后一位
        }
        else
        {
            result = -Convert.ToInt32(Math.Log(ori) / Math.Log(2) * 100000);
        }

        return result;
    }

    public static int fetchMz(double target, int mzPrecision)
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

    public static double[] getMobilityData(Spectrum spectrum)
    {
        return spectrum.getArrayByCVID(CVID.MS_mean_ion_mobility_drift_time_array)?.data
                   .Storage() ??
               spectrum.getArrayByCVID(CVID.MS_mean_inverse_reduced_ion_mobility_array)
                   ?.data.Storage() ??
               spectrum.getArrayByCVID(CVID.MS_raw_ion_mobility_array)?.data.Storage() ??
               spectrum.getArrayByCVID(CVID.MS_raw_inverse_reduced_ion_mobility_array)
                   ?.data.Storage();
    }
}