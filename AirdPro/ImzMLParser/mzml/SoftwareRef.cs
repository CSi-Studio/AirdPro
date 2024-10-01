using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{    
    public class SoftwareRef : MzMLReference<Software>
    {

        private const long serialVersionUID = 1L;

        
        public SoftwareRef(Software reference) : base(reference)
        {
        }

        public override string GetTagName()
        {
            return "softwareRef";
        }
    }
}
