namespace AirdPro.DomainsCore.Aird;

public class TimsData
{ 
    public double mobility;
    public double mz;
    public double intensity;

    public TimsData(double mobility, double mz, double intensity)
    {
        this.mobility = mobility;
        this.mz = mz;
        this.intensity = intensity;
    }
}