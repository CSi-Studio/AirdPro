using AirdPro.csimzMLParser.mzml;
using AirdPro.Domains.Msi;
using CSharpFastPFOR.Port;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AirdPro.csimzMLParser.util
{
    public class DataUtil
    {
        private static Dictionary<string, int> scanIdTable = [];
        private static int lastScanNumber = 0;

        public static int ConvertScanIdToScanNumber(string scanId)
        {
            if (scanIdTable.ContainsKey(scanId))
            {
                return scanIdTable[scanId];
            }

            Regex pattern = new Regex(@"scan=([0-9]+)");
            Match matcher = pattern.Match(scanId);
            bool scanNumberFound = matcher.Success;

            int scanNumber;
            if (scanNumberFound)
            {
                scanNumber = int.Parse(matcher.Groups[1].Value);
                scanIdTable.Add(scanId, scanNumber);
                return scanNumber;
            }

            scanNumber = lastScanNumber + 1;
            lastScanNumber++;
            scanIdTable.Add(scanId, scanNumber);
            return scanNumber;
        }

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
            BinaryDataArrayList bdas = spectrum.GetBinaryDataArrayList();
            foreach (BinaryDataArray bda in bdas)
            {
                if (bda.IsMobilityArray())
                {
                    return bda.GetDataAsDouble();
                }
            }
            return null;
        }

        public static double[] ExtractIntensityValues(Spectrum spectrum)
        {
            try
            {
                BinaryDataArrayList dataList = spectrum.GetBinaryDataArrayList();
                BinaryDataArray intensityArray = dataList.GetIntensityArray();
                double[] intensityValues = intensityArray.GetDataAsDouble();
                return intensityValues;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return [];
            }
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

        public static double[] ExtractMzValues(Spectrum spectrum)
        {
            try
            {
                BinaryDataArrayList dataList = spectrum.GetBinaryDataArrayList();
                BinaryDataArray mzArray = dataList.GetMzArray();
                double[] mzValues = mzArray.GetDataAsDouble();
                return mzValues;

            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return [];
            }
        }

        public static string ExtractScanDefinition(Spectrum spectrum)
        {
            CVParam cvParams = spectrum.GetCVParam("MS:1000512");
            if (cvParams != null)
            {
                return cvParams.GetValueAsString();
            }

            ScanList scanListElement = spectrum.GetScanList();
            if (scanListElement != null)
            {
                for (int i = 0; i < scanListElement.Size(); i++)
                {
                    Scan scan = scanListElement.Get(i);

                    cvParams = scan.GetCVParam("MS:1000512");
                    if (cvParams != null)
                    {
                        return cvParams.GetValueAsString();
                    }
                }
            }
            return spectrum.GetID();
        }

        public static int ExtractPrecursorCharge(Spectrum spectrum)
        {
            PrecursorList precursorList = spectrum.GetPrecursorList();
            if ((precursorList == null) || (precursorList.Size() == 0))
            {
                return 0;
            }

            foreach (Precursor parent in precursorList)
            {
                SelectedIonList selectedIonListElement = parent.GetSelectedIonList();
                if ((selectedIonListElement == null) || (selectedIonListElement.Size() == 0))
                {
                    return 0;
                }

                foreach (SelectedIon sion in selectedIonListElement)
                {

                    // precursor charge
                    CVParam param = sion.GetCVParam("MS:1000041");
                    if (param != null)
                    {
                        return param.GetValueAsInteger();
                    }
                }
            }
            return 0;
        }

        public static double ExtractPrecursorMz(Spectrum spectrum)
        {
            PrecursorList precursorListElement = spectrum.GetPrecursorList();
            if ((precursorListElement == null) || (precursorListElement.Size() == 0))
            {
                return 0;
            }

            foreach (Precursor parent in precursorListElement)
            {

                SelectedIonList selectedIonListElement = parent.GetSelectedIonList();
                if ((selectedIonListElement == null) || (selectedIonListElement.Size() == 0))
                {
                    return 0;
                }

                // MS:1000040 is used in mzML 1.0,
                // MS:1000744 is used in mzML 1.1.0
                foreach (SelectedIon sion in selectedIonListElement)
                {
                    CVParam param = sion.GetCVParam("MS:1000040");
                    if (param != null)
                    {
                        return param.GetValueAsDouble();
                    }

                    param = sion.GetCVParam("MS:1000744");
                    if (param != null)
                    {
                        return param.GetValueAsDouble();
                    }
                }
            }
            return 0;
        }

        public static PolarityType ExtractPolarity(Spectrum spectrum)
        {
            CVParam cv = spectrum.GetCVParam(Spectrum.SCAN_POLARITY_ID);
            if (spectrum.GetCVParam("MS:1000130") != null)
            {
                return PolarityType.POSITIVE;
            }
            else if (spectrum.GetCVParam("MS:1000129") != null)
            {
                return PolarityType.NEGATIVE;
            }

            ScanList scanListElement = spectrum.GetScanList();
            if (scanListElement != null)
            {
                for (int i = 0; i < scanListElement.Size(); i++)
                {
                    Scan scan = scanListElement.Get(i);

                    if (scan.GetCVParam("MS:1000130") != null)
                    {
                        return PolarityType.POSITIVE;
                    }
                    else if (scan.GetCVParam("MS:1000129") != null)
                    {
                        return PolarityType.NEGATIVE;
                    }
                }
            }
            return PolarityType.ANY;
        }

        public static float ExtractRetentionTime(Spectrum spectrum)
        {
            ScanList scanListElement = spectrum.GetScanList();
            if (scanListElement == null)
            {
                return 0;
            }

            foreach (Scan scan in scanListElement)
            {
                try
                {
                    // scan start time correct?
                    CVParam param = scan.GetCVParam(Scan.SCAN_START_TIME_ID);
                    if (param != null)
                    {
                        return (float)param.GetValueAsDouble();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return 0;
        }

        public static Coordinates ExtractCoordinates(Spectrum spectrum)
        {
            ScanList list = spectrum.GetScanList();
            if (list != null)
            {
                foreach (Scan scan in spectrum.GetScanList())
                {
                    CVParam xValue = scan.GetCVParam(Scan.POSITION_X_ID);
                    CVParam yValue = scan.GetCVParam(Scan.POSITION_Y_ID);
                    CVParam zValue = scan.GetCVParam(Scan.POSITION_Z_ID);

                    if (xValue != null && yValue != null)
                    {
                        int x = xValue.GetValueAsInteger() - 1;
                        int y = yValue.GetValueAsInteger() - 1;

                        if (zValue != null)
                        {
                            return new Coordinates(x, y, zValue.GetValueAsInteger() - 1);
                        }
                        else
                        {
                            return new Coordinates(x, y, 0);
                        }
                    }
                }
            }
            return null;
        }

        public static int ExtractMSLevel(Spectrum spectrum)
        {
            CVParam cv = spectrum.GetCVParam(Spectrum.MS_LEVEL_ID);
            if (cv == null)
            {
                return 1;
            }
            if (cv.GetValueAsInteger() == 2)
            {
                return 2;
            }

            return 1;
        }

        public static int ExtractParentScanNumber(Spectrum spectrum)
        {
            PrecursorList precursorListElement = spectrum.GetPrecursorList();
            if ((precursorListElement == null) || (precursorListElement.Size() == 0))
            {
                return -1;
            }

            foreach (Precursor parent in precursorListElement)
            {
                // Get the precursor scan number
                string precursorScanId = parent.GetXMLAttributeText();
                if (precursorScanId == null)
                {
                    return -1;
                }
                int parentScan = ConvertScanIdToScanNumber(precursorScanId);
                return parentScan;
            }
            return -1;
        }

    }
}
