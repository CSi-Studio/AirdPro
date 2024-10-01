using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class SelectedIon : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string ION_SELECTION_ATTRIBUTE_ID = "MS:1000455"; 

        public SelectedIon() : base()
        {
            
        }

        public SelectedIon(SelectedIon selectedIon, ReferenceableParamGroupList rpgList) : base(selectedIon, rpgList)
        {
            
        }

        public override string GetTagName()
        {
            return "selectedIon";
        }
    }
}
