namespace AirdPro.DomainsCore.Aird;

public class TimsData
{ 
    public float mobility;
    public double mz;
    public double intensity;

    public TimsData(float mobility, double mz, double intensity)
    {
        this.mobility = mobility;
        this.mz = mz;
        this.intensity = intensity;
    }
}