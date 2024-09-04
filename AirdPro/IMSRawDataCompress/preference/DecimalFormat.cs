using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.preference
{
    public class DecimalFormat : IFormatProvider, ICustomFormatter
    {
        private readonly string _format;

        public DecimalFormat(string format)
        {
            _format = format;
        }

        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is IFormattable formattable)
            {
                return formattable.ToString(_format, formatProvider);
            }
            return arg.ToString();
        }
    }
}
