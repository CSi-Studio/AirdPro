using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AirdPro.Constants;
using AirdPro.Converters;
using AirdPro.Domains;
using AirdPro.Utils;
using AirdSDK.Beans;
using AirdSDK.Compressor;

namespace AirdPro.Algorithms.Compressor.Tdms;

public class TdmsComp
{
    public int mzPrecision = 100000;
    public bool ignoreZero = true;
    public bool isCentroid = false;
    public int intensityPrecision = 10;
    
    public SortedIntComp mzIntComp;
    public ByteComp mzByteComp;
    public IntComp intIntComp;
    public ByteComp intByteComp;
    
    public TdmsComp(JobInfo jobInfo)
    {
        this.mzPrecision = jobInfo.config.mzPrecision;
        this.ignoreZero = jobInfo.config.ignoreZeroIntensity;
        this.isCentroid = jobInfo.config.centroid;
        
       mzIntComp = SortedIntComp.build(jobInfo.config.mzIntComp);
       mzByteComp = ByteComp.build(jobInfo.config.mzByteComp);

       intIntComp = IntComp.build(jobInfo.config.intIntComp);
       intByteComp = ByteComp.build(jobInfo.config.intByteComp);
    }

    public void compressMS1(TdmsConverter converter, BlockIndex index)
    {
        Hashtable ms1Table = Hashtable.Synchronized(new Hashtable());
        int process = 0;
        for (var i = 0; i < converter.ms1List.Count; i++)
        {
            converter.jobInfo.log(null, Tag.progress(Tag.MS1, process, converter.ms1List.Count));
            MsIndex ms1Index = converter.ms1List[i];
            TempScan ts = new TempScan(ms1Index);
            TdmsSpectrum spectrum = converter.spectra[i];
            compress(spectrum, ts);
            ms1Table.Add(i, ts);
        }
        converter.writeToFile(ms1Table, index);
    }

    public void compress(TdmsSpectrum spectrum, TempScan ts)
    {
        List<double> mzData = new List<double>(spectrum.mzChannel.GetFirstData<double>());
        List<float> intData = new List<float>(spectrum.intChannel.GetFirstData<float>());
        var size = mzData.Count();
        if (size == 0)
        {
            ts.mzArrayBytes = new byte[0];
            ts.intArrayBytes = new byte[0];
            return;
        }
        
        int[] mzArray = new int[size];
        int[] intensityArray = new int[size];
        
        int j = 0;
        for (int t = 0; t < size; t++)
        {
            if (ignoreZero && intData[t] == 0) continue;
            mzArray[j] = DataUtil.fetchMz(mzData[t], mzPrecision);
            // intensityArray[j] = Convert.ToInt32(Math.Log(intData[t]) / Math.Log(2) * 100);
            intensityArray[j] = DataUtil.fetchIntensity(intData[t], intensityPrecision);
            j++;
        }
        int[] mzSubArray = new int[j];
        Array.Copy(mzArray, mzSubArray, j);
        int[] intensitySubArray = new int[j];
        Array.Copy(intensityArray, intensitySubArray, j);
        byte[] compressedMzArray = null;
        byte[] compressedIntArray = null;
        
        if (mzSubArray.Length == 0)
        {
            compressedMzArray = new byte[0];
        }
        else
        {
            compressedMzArray = ComboComp.encode(mzIntComp, mzByteComp, mzSubArray);
        }

        if (intensitySubArray.Length == 0)
        {
            compressedIntArray = new byte[0];
        }
        else
        {
            compressedIntArray = ComboComp.encode(intIntComp, intByteComp, intensitySubArray);
        }

        ts.tic = (long)intData.Sum();
        int index = DataUtil.findMaxIndex(intData);
        ts.basePeakIntensity = intData[index];
        ts.basePeakMz = mzData[index];
        ts.mzArrayBytes = compressedMzArray;
        ts.intArrayBytes = compressedIntArray;
    }
}