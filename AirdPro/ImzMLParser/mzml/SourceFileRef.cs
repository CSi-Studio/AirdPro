using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class SourceFileRef : MzMLReference<SourceFile>
    {
        private static readonly long serialVersionUID = 1L;

        public SourceFileRef(SourceFile sourceFileRef) : base(sourceFileRef)
        { 
        
        }

        public override string GetTagName()
        {
            return "sourceFileRef";
        }
    }
}
