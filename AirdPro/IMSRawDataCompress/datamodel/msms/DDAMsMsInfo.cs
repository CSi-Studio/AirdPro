using System;


namespace AirdPro.IMSRawDataCompress.datamodel.msms
{
    public class DDAMsMsInfo : IDDAMsMsInfo
    {
        public static String XML_TYPE_NAME = "ddamsmsinfo";

        private readonly double isolationMz;
        private readonly long? charge;
        private readonly double? activationEnergy;
        private readonly int msLevel;
        private readonly ActivationMethod method;
        private readonly Range<double> isolationWindow;
        private readonly Scan parentScan;    
        private readonly Scan msMsScan;

        public DDAMsMsInfo(double isolationMz, long? charge,
            double? activationEnergy, Scan msMsScan, Scan parentScan,
            int msLevel, ActivationMethod method, Range<double> isolationWindow)
        {
            this.isolationMz = isolationMz;
            this.charge = charge;
            this.activationEnergy = activationEnergy;
            this.msMsScan = msMsScan;
            this.parentScan = parentScan;
            this.msLevel = msLevel;
            this.method = method;
            this.isolationWindow = isolationWindow;
        }

        public string XML_PRECURSOR_MZ_ATTR => throw new NotImplementedException();

        public string XML_PRECURSOR_CHARGE_ATTR => throw new NotImplementedException();

        public string XML_FRAGMENT_SCAN_ATTR => throw new NotImplementedException();

        public string XML_PARENT_SCAN_ATTR => throw new NotImplementedException();

        public string XML_ACTIVATION_ENERGY_ATTR => throw new NotImplementedException();

        public string XML_ACTIVATION_TYPE_ATTR => throw new NotImplementedException();

        public string XML_MSLEVEL_ATTR => throw new NotImplementedException();

        public string XML_ISOLATION_WINDOW_ATTR => throw new NotImplementedException();

        public string XML_ELEMENT => throw new NotImplementedException();

        public string XML_TYPE_ATTRIBUTE => throw new NotImplementedException();

        public IMsMsInfo CreateCopy()
        {
            throw new NotImplementedException();
        }

        public float GetActivationEnergy()
        {
            throw new NotImplementedException();
        }

        public ActivationMethod GetActivationMethod()
        {
            throw new NotImplementedException();
        }

        public double GetIsolationMz()
        {
            throw new NotImplementedException();
        }

        public Range<double> GetIsolationWindow()
        {
            throw new NotImplementedException();
        }

        public int GetMsLevel()
        {
            throw new NotImplementedException();
        }

        public Scan GetMsMsScan()
        {
            throw new NotImplementedException();
        }

        public Scan GetParentScan()
        {
            throw new NotImplementedException();
        }

        public int GetPrecursorCharge()
        {
            throw new NotImplementedException();
        }

        public bool SetMsMsScan(Scan scan)
        {
            throw new NotImplementedException();
        }
    }
}
