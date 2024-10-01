using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class Target : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public Target() : base() { }

        public Target(Target target, ReferenceableParamGroupList rpgList) : base(target, rpgList) { }

        public override string GetTagName() 
        {
            return "target";
        }
    }
}
