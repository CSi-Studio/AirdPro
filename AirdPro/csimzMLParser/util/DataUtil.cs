using AirdPro.csimzMLParser.mzml;
using AirdSDK.Bean.Msi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AirdPro.csimzMLParser.util
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

    }
}
