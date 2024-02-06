namespace AirdPro.Domains.Common;

public class TempSpectrum
{
    public int indexId;
    public double rt;
    public int[] mzs;
    public int[] intensities;

    public TempSpectrum(int[] mzs, int[] intensities)
    {
        this.mzs = mzs;
        this.intensities = intensities;
    }
}