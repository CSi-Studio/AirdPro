namespace AirdPro.Domains.Common;

public class TempSpectrum
{
    public int[] mzs;
    public float[] intensities;

    public TempSpectrum(int[] mzs, float[] intensities)
    {
        this.mzs = mzs;
        this.intensities = intensities;
    }
}