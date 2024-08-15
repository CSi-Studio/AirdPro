using CSharpFastPFOR.Port;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.datamodel.msms
{
    public interface IDDAMsMsInfo : IMsMsInfo
    {
        String XML_PRECURSOR_MZ_ATTR { get; }
        String XML_PRECURSOR_CHARGE_ATTR { get; }
        String XML_FRAGMENT_SCAN_ATTR { get; }
        String XML_PARENT_SCAN_ATTR { get; }
        String XML_ACTIVATION_ENERGY_ATTR { get; }
        String XML_ACTIVATION_TYPE_ATTR { get; }
        String XML_MSLEVEL_ATTR { get; }
        String XML_ISOLATION_WINDOW_ATTR { get; }
       
        double GetIsolationMz();
        int GetPrecursorCharge();
        Scan GetParentScan();
        Scan GetMsMsScan();



    }
}
