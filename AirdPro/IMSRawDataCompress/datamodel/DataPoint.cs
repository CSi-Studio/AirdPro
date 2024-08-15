namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class DataPoint
    {
        public double Mz { get; }
        public double Intensity { get; }

        public DataPoint()
        {
            
        }

        public DataPoint(double mz, double intensity)
        {
            Mz = mz;
            Intensity = intensity;
        }
    }
}
