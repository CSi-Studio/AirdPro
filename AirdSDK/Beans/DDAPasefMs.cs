using CSharpFastPFOR.Port;
using System.Collections.Generic;
using System;
using AirdSDK.Domains.common;
using System.Linq;

namespace AirdSDK.Domains;

public class DDAPasefMs
{
    /**
    * num for current scan
    */
    public int num;

    /**
     * Retention Time, unit: reference from raw file, default is second required
     */
    public double rt;

    /**
     * the tic data for current scan
     */
    public long tic;

    /**
     * cvList for current scan
     */
    public List<CV> cvList;

    /**
     * the window range for current scan
     */
    public WindowRange range;

    /**
     * the ms1 spectrum data pairs required
     */
    public Spectrum spectrum;

    /**
     * related ms2 list
     */
    public List<DDAPasefMs> ms2List;

    public DDAPasefMs(double rt, Spectrum spectrum)
    {
        this.rt = rt;
        this.spectrum = spectrum;
    }
}