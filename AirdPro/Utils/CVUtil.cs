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
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using Activator = AirdPro.Constants.Activator;
using CV = AirdSDK.Beans.CV;

namespace AirdPro.Utils;

public class CVUtil
{
    //由于直接对这些指定的cv字段进行存储,因此不需要再转存一遍
    private static readonly HashSet<CVID> skipList = new()
    {
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
    };

    public static List<CV> trans(CVParamList paramList)
    {
        if (paramList == null) return null;

        var cvList = new List<CV>();
        foreach (var cvParam in paramList)
        {
            if (skipList.Contains(cvParam.cvid)) continue;

            cvList.Add(build(cvParam));
        }

        return cvList;
    }

    public static CV build(CVParam param)
    {
        var cv = new CV();
        cv.cvid = (int) param.cvid + ":" + param.name;
        cv.value = param.value.ToString();
        var unitsId = (int) param.units;
        if (unitsId != -1) cv.units = (int) param.units + ":" + param.unitsName;

        return cv;
    }

    public static string parseMsLevel(Spectrum spectrum)
    {
        return spectrum.cvParamChild(CVID.MS_ms_level).value.ToString();
    }

    public static double parseRT(Scan scan, JobInfo jobInfo)
    {
        var cv = scan.cvParamChild(CVID.MS_scan_start_time);
        var time = double.Parse(cv.value.ToString());
        if (cv.unitsName.Equals("minute")) time = time * 60;

        time = Math.Round(time * 10000) / 10000;
        return time;
    }

    public static string parseFilterString(Scan scan, JobInfo jobInfo)
    {
        if (scan.hasCVParamChild(CVID.MS_filter_string))
        {
            var cv = scan.cvParamChild(CVID.MS_filter_string);
            return cv.value.ToString();
        }
        else
        {
            return null;
        }
    
    }

    public static void parseMobility(Scan scan, MobiInfo mobiInfo)
    {
        // float mobility = 0f;
        if (scan.hasCVParamChild(CVID.MS_inverse_reduced_ion_mobility))
        {
            var cv = scan.cvParamChild(CVID.MS_inverse_reduced_ion_mobility);
            mobiInfo.unit = cv.unitsName;
            mobiInfo.type = MobilityType.TIMS;
        }
        else if (scan.hasCVParamChild(CVID.MS_ion_mobility_drift_time))
        {
            var cv = scan.cvParamChild(CVID.MS_ion_mobility_drift_time);
            mobiInfo.unit = cv.unitsName;
            mobiInfo.type = MobilityType.DTIMS;
        }
    }

    public static long parseTIC(Spectrum spectrum)
    {
        try
        {
            return Convert.ToInt64(Convert.ToDouble(spectrum.cvParamChild(CVID.MS_TIC).value.ToString()));
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static double parseBasePeakIntensity(Spectrum spectrum)
    {
        try
        {
            return double.Parse(spectrum.cvParamChild(CVID.MS_base_peak_intensity).value.ToString());
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static double parseBasePeakMz(Spectrum spectrum)
    {
        try
        {
            return double.Parse(spectrum.cvParamChild(CVID.MS_base_peak_m_z).value.ToString());
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /**
         * 从任意spectrum上获取
         */
    public static string parsePolarity(Spectrum spectrum)
    {
        if (!spectrum.cvParamChild(CVID.MS_negative_scan).cvid.Equals(CVID.CVID_Unknown))
            return Polarity.NEGATIVE;
        if (!spectrum.cvParamChild(CVID.MS_positive_scan).cvid.Equals(CVID.CVID_Unknown))
            return Polarity.POSITIVE;
        return "Unknown";
    }

    /**
       * 从任意chromatogram上获取
       */
    public static string parsePolarity(Chromatogram chromatogram)
    {
        if (!chromatogram.cvParamChild(CVID.MS_negative_scan).cvid.Equals(CVID.CVID_Unknown))
            return Polarity.NEGATIVE;
        if (!chromatogram.cvParamChild(CVID.MS_positive_scan).cvid.Equals(CVID.CVID_Unknown))
            return Polarity.POSITIVE;
        return "Unknown";
    }

    /**
         * 从任意spectrum上获取
         */
    public static string parseMsType(Spectrum spectrum)
    {
        if (!spectrum.cvParamChild(CVID.MS_profile_spectrum).cvid.Equals(CVID.CVID_Unknown))
            return MSType.PROFILE;
        if (!spectrum.cvParamChild(CVID.MS_centroid_spectrum).cvid.Equals(CVID.CVID_Unknown))
            return MSType.CENTROIDED;
        return MSType.UNKNOWN;
    }

    public static string parseMsType(Chromatogram chromatogram)
    {
        if (!chromatogram.cvParamChild(CVID.MS_profile_spectrum).cvid.Equals(CVID.CVID_Unknown))
            return MSType.PROFILE;
        if (!chromatogram.cvParamChild(CVID.MS_centroid_spectrum).cvid.Equals(CVID.CVID_Unknown))
            return MSType.CENTROIDED;
        return MSType.UNKNOWN;
    }

    /**
         * 解析activation以及对应的energy
         * 需要从ms2的谱图上获取
         */
    public static (string activator, float energy) parseActivator(Activation activation)
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

    public static double parsePrecursorParams(Precursor precursor, CVID cvid, JobInfo jobInfo)
    {
        double result = -1;
        var retryTimes = 3;
        while (result < 0 && retryTimes > 0)
        {
            try
            {
                if (precursor.isolationWindow.hasCVParamChild(cvid))
                    result = double.Parse(precursor.isolationWindow.cvParamChild(cvid).value.ToString());
                else
                    result = 0;
            }
            catch (FormatException e)
            {
                jobInfo.log(cvid + "-Retry Times-" + retryTimes + "-Result:" + result);
                jobInfo.log(e.StackTrace);
            }

            retryTimes--;
        }

        if (result < 0) throw new Exception(ResultCode.Parse_Double_Error + result);

        return result;
    }

    public static double parsePrecursorWidth(Precursor precursor, JobInfo jobInfo)
    {
        var retryTimes = 3;
        double lower = -1;
        double upper = -1;
        while (upper + lower < 0 && retryTimes > 0)
        {
            try
            {
                if (precursor.isolationWindow.hasCVParamChild(CVID.MS_isolation_window_lower_offset))
                    lower = double.Parse(precursor.isolationWindow.cvParamChild(CVID.MS_isolation_window_lower_offset)
                        .value.ToString());
                else
                    lower = 0;

                if (precursor.isolationWindow.hasCVParamChild(CVID.MS_isolation_window_upper_offset))
                    upper = double.Parse(precursor.isolationWindow.cvParamChild(CVID.MS_isolation_window_upper_offset)
                        .value.ToString());
                else
                    upper = 0;
            }
            catch (FormatException e)
            {
                jobInfo.log("Get Precursor Width-Retry Times-" + retryTimes + "-Result:" + upper + "-" + lower + "=" +
                            (upper + lower));
                jobInfo.log(e.StackTrace);
            }

            retryTimes--;
        }

        if (upper + lower < 0) throw new Exception(ResultCode.Parse_Double_Error + (upper + lower));

        return upper + lower;
    }

    public static int parsePrecursorCharge(Precursor precursor, JobInfo jobInfo)
    {
        var result = 0;
        var retryTimes = 3;
        while (result < 0 && retryTimes > 0)
        {
            try
            {
                if (precursor.selectedIons == null || precursor.selectedIons[0].cvParamChild(CVID.MS_charge_state)
                        .cvid.Equals(CVID.CVID_Unknown))
                    return 0;

                result = int.Parse(precursor.selectedIons[0].cvParamChild(CVID.MS_charge_state).value.ToString());
            }
            catch (FormatException e)
            {
                jobInfo.log("Charge-Retry Times-" + retryTimes + "-Result:" + result);
                jobInfo.log(e.StackTrace);
            }

            retryTimes--;
        }

        if (result < 0) throw new Exception(ResultCode.Parse_Integer_Error + result);

        return result;
    }

    public static WindowRange parseIsolationWindow(Precursor precursor, JobInfo jobInfo)
    {
        var windowRange = new WindowRange();
        var precursorMz = parsePrecursorParams(precursor, CVID.MS_isolation_window_target_m_z, jobInfo);
        var lowerOffset = parsePrecursorParams(precursor, CVID.MS_isolation_window_lower_offset, jobInfo);
        var upperOffset = parsePrecursorParams(precursor, CVID.MS_isolation_window_upper_offset, jobInfo);
        var charge = parsePrecursorCharge(precursor, jobInfo);
        windowRange.charge = charge;
        windowRange.mz = precursorMz;
        windowRange.start = precursorMz - lowerOffset;
        windowRange.end = precursorMz + upperOffset;
        return windowRange;
    }

    public static float parseInjectionTime(Scan scan)
    {
        var cv = scan.cvParamChild(CVID.MS_ion_injection_time);
        if (cv != null && cv.value != null)
        {
            return (float)Math.Round(cv.value * 10000) / 10000;
        }

        return -1;
    }
}