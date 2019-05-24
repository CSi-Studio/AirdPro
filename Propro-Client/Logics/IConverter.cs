using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pwiz.CLI.cv;
using pwiz.CLI.data;
using pwiz.CLI.msdata;
using pwiz.CLI.util;
using Propro.Domains;
using Propro.Utils;

namespace Propro.Logics
{
    public abstract class IConverter
    {
        public abstract void doConvert(ConvertJobInfo jonInfo);

        public float parseRT(Scan scan)
        {
            CVParam cv = scan.cvParamChild(CVID.MS_scan_start_time);
            float time = float.Parse(cv.value.ToString());
            if (cv.unitsName.Equals("minute"))
            {
                return time * 60;
            }
            else
            {
                return time;
            }
        }

        public void compress(Spectrum spectrum, ConvertJobInfo jobInfo, out byte[] mzBytes, out byte[] intensityBytes)
        {
            BinaryData mzData = spectrum.getMZArray().data;
            BinaryData intData = spectrum.getIntensityArray().data;

            List<int> mzList = new List<int>();
            List<float> intensityList = new List<float>();
            for (int t = 0; t < mzData.Count; t++)
            {
                
                if (jobInfo.ignoreZeroIntensity && intData[t] == 0)
                {
                    continue;
                }
                mzList.Add((int)(mzData[t] * jobInfo.mzPrecision)); //精确到小数点后面三位
                intensityList.Add((float)(Math.Round(intData[t] * jobInfo.intensityPrecision) / jobInfo.intensityPrecision));
            }

            float[] intensityArray = intensityList.ToArray();
            int[] mzArray = CompressUtil.compressWithPFor(mzList.ToArray());
            mzBytes = CompressUtil.compressWithZlib(mzArray);
            intensityBytes = CompressUtil.compressWithZlib(intensityArray);
        }
    }
}
