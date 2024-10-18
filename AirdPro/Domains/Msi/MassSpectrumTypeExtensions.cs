using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Domains.Msi
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
            switch (spectraType)
            {
                case MassSpectrumType.PROFILE:
                    return false;
                case MassSpectrumType.THRESHOLDED:
                case MassSpectrumType.MIXED:
                case MassSpectrumType.CENTROIDED:
                case MassSpectrumType.ANY:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spectraType), spectraType, null);
            }
        }
    }
}
