using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.preference
{
    public class TimsPreferences
    {
        public static readonly NumberFormatParameter MzFormat = new NumberFormatParameter(
            "m/z value format",
            "Format of m/z values",
            false,
            new DecimalFormat("0.0000")
        );

        public static readonly NumberFormatParameter rtFormat = new NumberFormatParameter(
            "Retention time value format",
            "Format of retention time values",
            false,
            new DecimalFormat("0.00")
        );

        public static readonly NumberFormatParameter mobilityFormat = new NumberFormatParameter(
            "Mobility value format",
            "Format of mobility values",
            false,
            new DecimalFormat("0.000")
        );

        public static readonly NumberFormatParameter ccsFormat = new NumberFormatParameter(
            "CCS value format",
            "Format of CCS values",
            false,
            new DecimalFormat("0.0")
        );

        public static readonly NumberFormatParameter intensityFormat = new NumberFormatParameter(
            "Intensity value format",
            "Format of intensity values",
            false,
            new DecimalFormat("0.0E0")
        );
    }
}
