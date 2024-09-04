using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.IMSRawDataCompress.preference
{
    public class NumberFormatParameter
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsRequired { get; private set; }
        public IFormatProvider FormatProvider { get; private set; }

        public NumberFormatParameter(string name, string description, bool isRequired, IFormatProvider formatProvider)
        {
            Name = name;
            Description = description;
            IsRequired = isRequired;
            FormatProvider = formatProvider;
        }
    }
}
