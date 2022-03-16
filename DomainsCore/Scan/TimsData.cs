namespace AirdPro.DomainsCore.Aird;

public class TimsData
{ 
    public int mobilityNo;
    public double mz;
    public double intensity;

    public TimsData(int mobilityNo, double mz, double intensity)
    {
        this.mobilityNo = mobilityNo;
        this.mz = mz;
        this.intensity = intensity;
    }
}