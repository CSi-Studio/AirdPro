namespace AirdSDK.Domains.common;

public class Spectrum
{
    public double[] mzs;
    public double[] ints;
    public double[] mobilities;

    public Spectrum(double[] mzs, double[] ints)
    {
        this.mzs = mzs;
        this.ints = ints;
    }

    public Spectrum(double[] mzs, double[] ints, double[] mobilities)
    {
        this.mzs = mzs;
        this.ints = ints;
        this.mobilities = mobilities;
    }
}