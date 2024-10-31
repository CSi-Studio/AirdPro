using AirdPro.csimzMLParser.mzml;
using System;
using System.Diagnostics;
using System.Linq;
using Spectrum = AirdPro.csimzMLParser.mzml.Spectrum;

namespace AirdPro.Utils.imzml
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

        public static double GetMinMZ(SpectrumList spectrumList)
        {
            double minMZ = double.MaxValue;
            foreach (Spectrum spectrum in spectrumList)
            {
                double[] mzArray = spectrum.GetMzArray();
                if (mzArray == null || mzArray.Count() == 0) 
                {
                    continue;
                }
                double mz = mzArray.Min();
                if (minMZ > mz)
                {
                    minMZ = mz;
                }
            }
            return minMZ;
        }

        public static double GetMaxMZ(SpectrumList spectrumList)
        {
            double maxMZ = double.MinValue;
            foreach (Spectrum spectrum in spectrumList)
            {
                double[] mzArray = spectrum.GetMzArray();
                if (mzArray == null || mzArray.Count() == 0)
                {
                    continue;
                }
                double mz = mzArray.Max();
                if (maxMZ < mz)
                {
                    maxMZ = mz;
                }
            }
            return maxMZ;
        }

    }
}
