using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace AirdPro.IMSRawDataCompress.datamodel.enums
{
    public enum MassSpectrumType
    {
        PROFILE,
        THRESHOLDED,
        CENTROIDED,
        MIXED,
        ANY
    }

    public static class MassSpectrumTypeExtensions
    {
        public static bool IsCentroided(this MassSpectrumType spectraType)
        {
            return spectraType switch
            {
                MassSpectrumType.PROFILE => false,
                MassSpectrumType.THRESHOLDED or MassSpectrumType.MIXED or MassSpectrumType.CENTROIDED or MassSpectrumType.ANY => true,
                _ => throw new ArgumentOutOfRangeException(nameof(spectraType), "Invalid MassSpectrumType value")
            };
        }
    }
}
