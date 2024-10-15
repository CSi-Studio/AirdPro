using AirdPro.Constants;
using AirdPro.csimzMLParser.mzml;
using AirdPro.Domains;
using AirdSDK.Beans;
using HZH_Controls;
using System;
using Activator = AirdPro.Constants.Activator;

namespace AirdPro.csimzMLParser.util
{
    public class CVUtil
    {    
        public static string ParseMsLevel(Spectrum spectrum)
        {
            CVParam cv = spectrum.GetCVParamOrChild(Spectrum.MS_lEVEL_ID);
            if (cv == null)
            {
                return default;
            }
            string msLevel = cv.GetValueAsDouble().ToString();
            return msLevel;
        }

        public static double ParseRt(Scan scan, JobInfo jobInfo)
        {
            CVParam cv = scan.GetCVParamOrChild(Scan.SCAN_START_TIME_ID);
            double time = cv.GetValueAsDouble();
            if (cv.GetUnits().Equals("minute")) time = time * 60;
            time = Math.Round(time * 10000) / 10000;
            return time;
        }

        public static string ParseFilterString(Scan scan, JobInfo jobInfo)
        {
            if (!scan.GetCVParamOrChild(Scan.SCAN_FILTER_STRING_ID).IsEmpty())
            {
                CVParam cv = scan.GetCVParamOrChild(Scan.SCAN_FILTER_STRING_ID);
                string filterString = cv.GetValueAsString();
                return filterString;
            }
            else
            {
                return null;
            }
        }

        public static void ParseMobility(Scan scan, MobiInfo mobiInfo)
        {
            if (!scan.GetCVParamOrChild(Scan.SCAN_INVERSE_REDUCED_ION_MOBILITY_ID).IsEmpty())
            {
                CVParam cv = scan.GetCVParamOrChild(Scan.SCAN_INVERSE_REDUCED_ION_MOBILITY_ID);
                mobiInfo.unit = cv.units.GetName();
                mobiInfo.type = MobilityType.TIMS;
            }
            else if (!scan.GetCVParamOrChild(Scan.SCAN_ION_MOBILITY_DRIFT_TIME_ID).IsEmpty())
            {
                CVParam cv = scan.GetCVParamOrChild(Scan.SCAN_ION_MOBILITY_DRIFT_TIME_ID);
                mobiInfo.unit = cv.units.GetName();
                mobiInfo.type = MobilityType.DTIMS;
            }
        }

        public static long ParseTic(Spectrum spectrum)
        {
            try
            {
                CVParam cv = spectrum.GetCVParamOrChild(Spectrum.TOTAL_ION_CURRENT_ID);
                return cv.GetValueAsLong();
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
                CVParam cv = spectrum.GetCVParamOrChild(Spectrum.BASE_PEAK_INTENSITY_ID);
                return cv.GetValueAsDouble();
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
                CVParam cv = spectrum.GetCVParamOrChild(Spectrum.BASE_PEAK_MZ_ID);
                return cv.GetValueAsDouble();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string ParsePolarity(Spectrum spectrum)
        {
            CVParam cvNeg = spectrum.GetCVParamOrChild(Spectrum.NEGATIVE_SCAN_ID);
            if (!cvNeg.IsEmpty())
                return Polarity.NEGATIVE;

            CVParam cvPos = spectrum.GetCVParamOrChild(Spectrum.POSITIVE_SCAN_ID);
            if (!cvPos.IsEmpty())
                return Polarity.POSITIVE;
            return "Unknown";
        }

        public static string ParsePolarity(Chromatogram chromatogram)
        {
            CVParam cvNeg = chromatogram.GetCVParamOrChild(Chromatogram.NEGATIVE_SCAN_ID);
            if (!cvNeg.IsEmpty())
                return Polarity.NEGATIVE;

            CVParam cvPos = chromatogram.GetCVParamOrChild(Chromatogram.POSITIVE_SCAN_ID);
            if (!cvPos.IsEmpty())
                return Polarity.POSITIVE;
            return "Unknown";
        }

        public static string ParseMsType(Spectrum spectrum)
        {
            CVParam cvProfile = spectrum.GetCVParamOrChild(Spectrum.PROFILE_SPECTRUM_ID);
            if (!cvProfile.IsEmpty())
                return MSType.PROFILE;

            CVParam cvCentroid = spectrum.GetCVParamOrChild(Spectrum.CENTROID_SPECTRUM_ID);
            if (!cvCentroid.IsEmpty())
                return MSType.CENTROIDED;

            return MSType.UNKNOWN;
        }

        public static string ParseMsType(Chromatogram chromatogram)
        {
            CVParam cvProfile = chromatogram.GetCVParamOrChild(Chromatogram.PROFILE_SPECTRUM_ID);
            if (!cvProfile.IsEmpty())
                return MSType.PROFILE;

            CVParam cvCentroid = chromatogram.GetCVParamOrChild(Chromatogram.CENTROID_SPECTRUM_ID);
            if (!cvProfile.IsEmpty())
                return MSType.CENTROIDED;            

            return MSType.UNKNOWN;
        }

        /**
             * 解析activation以及对应的energy
             * 需要从ms2的谱图上获取
             */
        public static (string activator, float energy) ParseActivator(Precursor precursor)
        {
            Activation activation = precursor.Activation;
            if (activation == null) return (Activator.UNKNOWN, -1);

            var act = "";
            float ene = -1;

            if (!activation.GetCVParamOrChild(Activator.HCD).Equals(Activator.UNKNOWN))
                act = Activator.HCD;
            else if (!activation.GetCVParamOrChild(Activator.CID).Equals(Activator.UNKNOWN))
                act = Activator.CID;
            else if (!activation.GetCVParamOrChild(Activator.ECD).Equals(Activator.UNKNOWN))
                act = Activator.ECD;
            else if (!activation.GetCVParamOrChild(Activator.ETD).Equals(Activator.UNKNOWN))
                act = Activator.ETD;
            else
                act = Activator.UNKNOWN;

            if (!activation.GetCVParamOrChild(Activation.ACTIVATION_COLLISION_ENERGY).Equals(Activator.UNKNOWN))
                ene = Convert.ToSingle(activation.GetCVParamOrChild(Activation.ACTIVATION_COLLISION_ENERGY).GetValueAsString());
            else
                ene = -1;
            return (act, ene);
        }

        public static double ParsePrecursorParams(IsolationWindow isolationWindow, string cvid, JobInfo jobInfo)
        {
            double? result = null;
            var retryTimes = 3;
            while (result == null && retryTimes > 0)
            {
                try
                {
                    CVParam cv = isolationWindow.GetCVParamOrChild(cvid);
                    if (cv != null)
                    {
                        result = cv.GetValueAsDouble();
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
                CVParam cv = isolationWindow.GetCVParamOrChild(cvid);
                throw new Exception(ResultCode.Parse_Double_Error + ":" + cv.GetValueAsDouble());
            }
            return result.Value;
        }

        public static int? ParsePrecursorCharge(Precursor precursor, JobInfo jobInfo)
        {
            var result = 0;
            var retryTimes = 3;
            while (result < 0 && retryTimes > 0)
            {
                try
                {
                    if (precursor.SelectedIonList == null || precursor.SelectedIonList.Get(0).GetCVParamOrChild(Precursor.PRECURSOR_CHARGE_STATE_ID).IsEmpty())
                    {
                        return null;
                    }
                    result = precursor.SelectedIonList.Get(0).GetCVParamOrChild(Precursor.PRECURSOR_CHARGE_STATE_ID).GetValueAsInteger();
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

        public static double ParsePrecursorWidth(IsolationWindow isolationWindow, JobInfo jobInfo)
        {
            var retryTimes = 3;
            double lower = -1;
            double upper = -1;
            while (upper + lower < 0 && retryTimes > 0)
            {
                try
                {
                    CVParam cv = isolationWindow.GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_LOWER_OFFSET_ID);
                    if (cv != null)
                    {                        
                        lower = cv.GetValueAsDouble();
                    }
                    else
                    {
                        lower = 0;
                    }

                    cv = isolationWindow.GetCVParamOrChild(IsolationWindow.ISOLATION_WINDOW_UPPER_OFFSET_ID);
                    if (cv != null)
                    {                        
                        upper = cv.GetValueAsDouble();
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

        public static WindowRange ParseIsolationWindow(Precursor precursor, JobInfo jobInfo)
        {
            var windowRange = new WindowRange();
            var precursorMz = ParsePrecursorParams(precursor.IsolationWindow, IsolationWindow.ISOLATION_WINDOW_TARGET_MZ, jobInfo);
            var lowerOffset =
                ParsePrecursorParams(precursor.IsolationWindow, IsolationWindow.ISOLATION_WINDOW_LOWER_OFFSET_ID, jobInfo);
            var upperOffset =
                ParsePrecursorParams(precursor.IsolationWindow, IsolationWindow.ISOLATION_WINDOW_UPPER_OFFSET_ID, jobInfo);
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
            var precursorMz = ParsePrecursorParams(isolationWindow, IsolationWindow.ISOLATION_WINDOW_TARGET_MZ, jobInfo);
            var lowerOffset = ParsePrecursorParams(isolationWindow, IsolationWindow.ISOLATION_WINDOW_LOWER_OFFSET_ID, jobInfo);
            var upperOffset = ParsePrecursorParams(isolationWindow, IsolationWindow.ISOLATION_WINDOW_UPPER_OFFSET_ID, jobInfo);

            windowRange.mz = precursorMz;
            windowRange.start = precursorMz - lowerOffset;
            windowRange.end = precursorMz + upperOffset;
            return windowRange;
        }

        public static float ParseInjectionTime(Scan scan)
        {
            CVParam cv = scan.GetCVParamOrChild(Scan.SCAN_ION_INJECTION_TIME_ID);
            if (cv != null && !cv.GetValueAsDouble().IsEmpty())
            {
                return (float)Math.Round(cv.GetValueAsDouble() * 10000) / 10000;
            }

            return -1;
        }

    }
}
