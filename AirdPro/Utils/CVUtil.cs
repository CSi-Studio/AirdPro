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
using AirdPro.Constants;
using AirdPro.Domains;
using AirdSDK.Beans;
using CSharpFastPFOR;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using Activator = AirdPro.Constants.Activator;
using CV = AirdSDK.Beans.CV;

namespace AirdPro.Utils;

public class CVUtil
{
    //由于直接对这些指定的cv字段进行存储,因此不需要再转存一遍
    private static readonly HashSet<CVID> SkipList =
    [
        CVID.MS_scan_start_time,
        CVID.MS_ms_level,
        CVID.MS_MSn_spectrum,
        CVID.MS_MS1_spectrum,
        CVID.MS_inverse_reduced_ion_mobility,
        CVID.MS_TIC,
        CVID.MS_negative_scan,
        CVID.MS_positive_scan,
        CVID.MS_profile_spectrum,
        CVID.MS_centroid_spectrum,
        CVID.MS_HCD,
        CVID.MS_CID,
        CVID.MS_ECD,
        CVID.MS_ETD,
        CVID.MS_collision_energy,
        CVID.MS_base_peak_intensity,
        CVID.MS_base_peak_m_z,
        CVID.MS_ion_injection_time,
        CVID.MS_highest_observed_m_z,
        CVID.MS_lowest_observed_m_z,
        CVID.MS_filter_string,
        CVID.MS_preset_scan_configuration,
        CVID.MS_SRM_chromatogram
    ];

    /**
     * 本函数会直接导致内存溢出
     */
    public static List<CV> Trans(CVParamList paramList)
    {
        if (paramList == null)
        {
            return null;
        }

        var cvList = new List<CV>();
        for (var i = 0; i < paramList.Count; i++)
        {
            CVParam cv = paramList[i];
            CVID id = cv.cvid;
            if (SkipList.Contains(id)) continue;
            cvList.Add(Build(cv));
            cv.Dispose();
        }
        
        paramList.Dispose();
        return cvList;
    }

    public static CV Build(CVParam param)
    {
        var cv = new CV();
        cv.cvid = (int)param.cvid + ":" + param.name;
        using (var value = param.value)
        {
            cv.value = (String)value;
        }
        
        var unitsId = (int)param.units;
        if (unitsId != -1) cv.units = (int)param.units + ":" + param.unitsName;
        return cv;
    }

    public static string ParseMsLevel(Spectrum spectrum)
    {
        using (CVParam cv = spectrum.cvParamChild(CVID.MS_ms_level))
        {
            string msLevel = cv.value.ToString();
            return msLevel;
        }
    }

    public static double ParseRt(Scan scan, JobInfo jobInfo)
    {
        using (var cv = scan.cvParamChild(CVID.MS_scan_start_time))
        {
            var time = double.Parse(cv.value.ToString());
            if (cv.unitsName.Equals("minute")) time = time * 60;

            time = Math.Round(time * 10000) / 10000;
            return time;
        }
    }

    public static string ParseFilterString(Scan scan, JobInfo jobInfo)
    {
        if (scan.hasCVParamChild(CVID.MS_filter_string))
        {
            var cv = scan.cvParamChild(CVID.MS_filter_string);
            string filterString = cv.value.ToString();
            cv.Dispose();
            return filterString;
        }
        else
        {
            return null;
        }
    }

    public static void ParseMobility(Scan scan, MobiInfo mobiInfo)
    {
        if (scan.hasCVParamChild(CVID.MS_inverse_reduced_ion_mobility))
        {
            using var cv = scan.cvParamChild(CVID.MS_inverse_reduced_ion_mobility);
            mobiInfo.unit = cv.unitsName;
            mobiInfo.type = MobilityType.TIMS;
        }
        else if (scan.hasCVParamChild(CVID.MS_ion_mobility_drift_time))
        {
            using var cv = scan.cvParamChild(CVID.MS_ion_mobility_drift_time);
            mobiInfo.unit = cv.unitsName;
            mobiInfo.type = MobilityType.DTIMS;
        }
    }

    public static long ParseTic(Spectrum spectrum)
    {
        try
        {
            using var cv = spectrum.cvParamChild(CVID.MS_TIC);
            return Convert.ToInt64(Convert.ToDouble(cv.value.ToString()));
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static double ParseBasePeakIntensity(Spectrum spectrum)
    {
        try
        {
            using var cv = spectrum.cvParamChild(CVID.MS_base_peak_intensity);
            return double.Parse(cv.value.ToString());
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static double ParseBasePeakMz(Spectrum spectrum)
    {
        try
        {
            using var cv = spectrum.cvParamChild(CVID.MS_base_peak_m_z);
            if (cv.cvid.Equals(CVID.CVID_Unknown))
            {
                return 0;
            }
            return double.Parse(cv.value.ToString());
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /**
         * 从任意spectrum上获取
         */
    public static string ParsePolarity(Spectrum spectrum)
    {
        using var cvNeg = spectrum.cvParamChild(CVID.MS_negative_scan);
        if (!cvNeg.cvid.Equals(CVID.CVID_Unknown))
            return Polarity.NEGATIVE;

        using var cvPos = spectrum.cvParamChild(CVID.MS_positive_scan);
        if (!cvPos.cvid.Equals(CVID.CVID_Unknown))
            return Polarity.POSITIVE;
        return "Unknown";
    }

    /**
       * 从任意chromatogram上获取
       */
    public static string ParsePolarity(Chromatogram chromatogram)
    {
        using var cvNeg = chromatogram.cvParamChild(CVID.MS_negative_scan);
        if (!cvNeg.cvid.Equals(CVID.CVID_Unknown))
            return Polarity.NEGATIVE;

        using var cvPos = chromatogram.cvParamChild(CVID.MS_positive_scan);
        if (!cvPos.cvid.Equals(CVID.CVID_Unknown))
            return Polarity.POSITIVE;
        return "Unknown";
    }

    /**
         * 从任意spectrum上获取
         */
    public static string ParseMsType(Spectrum spectrum)
    {
        using (var cvProfile = spectrum.cvParamChild(CVID.MS_profile_spectrum))
        {
            if (!cvProfile.cvid.Equals(CVID.CVID_Unknown))
                return MSType.PROFILE;
        }


        using (var cvCentroid = spectrum.cvParamChild(CVID.MS_centroid_spectrum))
        {
            if (!cvCentroid.cvid.Equals(CVID.CVID_Unknown))
                return MSType.CENTROIDED;
        }

        return MSType.UNKNOWN;
    }

    public static string ParseMsType(Chromatogram chromatogram)
    {
        using (var cvProfile = chromatogram.cvParamChild(CVID.MS_profile_spectrum))
        {
            if (!cvProfile.cvid.Equals(CVID.CVID_Unknown))
                return MSType.PROFILE;
        }


        using (var cvCentroid = chromatogram.cvParamChild(CVID.MS_centroid_spectrum))
        {
            if (!cvCentroid.cvid.Equals(CVID.CVID_Unknown))
                return MSType.CENTROIDED;
        }

        return MSType.UNKNOWN;
    }

    /**
         * 解析activation以及对应的energy
         * 需要从ms2的谱图上获取
         */
    public static (string activator, float energy) ParseActivator(Precursor precursor)
    {
        using (Activation activation = precursor.activation)
        {
            if (activation == null) return (Activator.UNKNOWN, -1);

            var act = "";
            float ene = -1;

            if (!activation.cvParamChild(CVID.MS_HCD).cvid.Equals(CVID.CVID_Unknown))
                act = Activator.HCD;
            else if (!activation.cvParamChild(CVID.MS_CID).cvid.Equals(CVID.CVID_Unknown))
                act = Activator.CID;
            else if (!activation.cvParamChild(CVID.MS_ECD).cvid.Equals(CVID.CVID_Unknown))
                act = Activator.ECD;
            else if (!activation.cvParamChild(CVID.MS_ETD).cvid.Equals(CVID.CVID_Unknown))
                act = Activator.ETD;
            else
                act = Activator.UNKNOWN;

            if (!activation.cvParamChild(CVID.MS_collision_energy).cvid.Equals(CVID.CVID_Unknown))
                ene = Convert.ToSingle(activation.cvParamChild(CVID.MS_collision_energy).value.ToString());
            else
                ene = -1;
            return (act, ene);
        }
    }

    public static double ParsePrecursorParams(IsolationWindow isolationWindow, CVID cvid, JobInfo jobInfo)
    {
        double? result = null;
        var retryTimes = 3;
        while (result == null && retryTimes > 0)
        {
            try
            {
                if (isolationWindow.hasCVParamChild(cvid))
                {
                    using var cv = isolationWindow.cvParamChild(cvid);
                    result = double.Parse(cv.value.ToString());
                }
                else
                {
                    result = 0;
                }
            }
            catch (Exception e)
            {
                jobInfo.Log(cvid + "-Retry Times-" + retryTimes + "-Result:" + result);
                jobInfo.Log(e.StackTrace);
            }

            retryTimes--;
        }

        if (result == null)
        {
            using var cv = isolationWindow.cvParamChild(cvid);
            throw new Exception(ResultCode.Parse_Double_Error + ":" + cv.value);
        }

        return result.Value;
    }

    public static double ParsePrecursorWidth(IsolationWindow isolationWindow, JobInfo jobInfo)
    {
        var retryTimes = 3;
        double lower = -1;
        double upper = -1;
        while (upper + lower < 0 && retryTimes > 0)
        {
            try
            {
                if (isolationWindow.hasCVParamChild(CVID.MS_isolation_window_lower_offset))
                {
                    using var cv = isolationWindow.cvParamChild(CVID.MS_isolation_window_lower_offset);
                    lower = double.Parse(cv.value.ToString());
                }
                else
                {
                    lower = 0;
                }


                if (isolationWindow.hasCVParamChild(CVID.MS_isolation_window_upper_offset))
                {
                    using var cv = isolationWindow.cvParamChild(CVID.MS_isolation_window_upper_offset);
                    upper = double.Parse(cv.value.ToString());
                }
                else
                {
                    upper = 0;
                }
            }
            catch (FormatException e)
            {
                jobInfo.Log("Get Precursor Width-Retry Times-" + retryTimes + "-Result:" + upper + "-" + lower + "=" +
                            (upper + lower));
                jobInfo.Log(e.StackTrace);
            }

            retryTimes--;
        }

        if (upper + lower < 0) throw new Exception(ResultCode.Parse_Double_Error + (upper + lower));

        return upper + lower;
    }

    public static int? ParsePrecursorCharge(Precursor precursor, JobInfo jobInfo)
    {
        var result = 0;
        var retryTimes = 3;
        while (result < 0 && retryTimes > 0)
        {
            try
            {
                if (precursor.selectedIons == null || precursor.selectedIons[0].cvParamChild(CVID.MS_charge_state)
                        .cvid.Equals(CVID.CVID_Unknown))
                    return null;

                result = int.Parse(precursor.selectedIons[0].cvParamChild(CVID.MS_charge_state).value.ToString());
            }
            catch (FormatException e)
            {
                jobInfo.Log("Charge-Retry Times-" + retryTimes + "-Result:" + result);
                jobInfo.Log(e.StackTrace);
            }

            retryTimes--;
        }

        if (result < 0) throw new Exception(ResultCode.Parse_Integer_Error + result);

        return result;
    }

    public static WindowRange ParseIsolationWindow(Precursor precursor, JobInfo jobInfo)
    {
        var windowRange = new WindowRange();
        var precursorMz = ParsePrecursorParams(precursor.isolationWindow, CVID.MS_isolation_window_target_m_z, jobInfo);
        var lowerOffset =
            ParsePrecursorParams(precursor.isolationWindow, CVID.MS_isolation_window_lower_offset, jobInfo);
        var upperOffset =
            ParsePrecursorParams(precursor.isolationWindow, CVID.MS_isolation_window_upper_offset, jobInfo);
        var charge = ParsePrecursorCharge(precursor, jobInfo);
        windowRange.charge = charge;
        windowRange.mz = precursorMz;

        windowRange.start = precursorMz - lowerOffset;
        windowRange.end = precursorMz + upperOffset;
        return windowRange;
    }

    public static WindowRange ParseIsolationWindow(IsolationWindow isolationWindow, JobInfo jobInfo)
    {
        var windowRange = new WindowRange();
        var precursorMz = ParsePrecursorParams(isolationWindow, CVID.MS_isolation_window_target_m_z, jobInfo);
        var lowerOffset = ParsePrecursorParams(isolationWindow, CVID.MS_isolation_window_lower_offset, jobInfo);
        var upperOffset = ParsePrecursorParams(isolationWindow, CVID.MS_isolation_window_upper_offset, jobInfo);

        windowRange.mz = precursorMz;
        windowRange.start = precursorMz - lowerOffset;
        windowRange.end = precursorMz + upperOffset;
        return windowRange;
    }

    public static float ParseInjectionTime(Scan scan)
    {
        using (var cv = scan.cvParamChild(CVID.MS_ion_injection_time))
        {
            if (cv != null && cv.value != null)
            {
                return (float)Math.Round(cv.value * 10000) / 10000;
            }

            return -1;
        }
    }
}