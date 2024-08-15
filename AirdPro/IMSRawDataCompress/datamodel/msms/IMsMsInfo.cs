using System;

namespace AirdPro.IMSRawDataCompress.datamodel.msms
{
    public interface IMsMsInfo
    {
        string XML_ELEMENT { get; }
        string XML_TYPE_ATTRIBUTE { get; }

        float GetActivationEnergy();
        Scan GetMsMsScan();
        bool SetMsMsScan(Scan scan);
        int GetMsLevel();
        ActivationMethod GetActivationMethod();
        Range<Double> GetIsolationWindow();

        IMsMsInfo CreateCopy();


    }
}
