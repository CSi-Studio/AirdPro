using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class ScanWindow : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public ScanWindow() : base()
        {
            
        }

        public ScanWindow(ScanWindow scanWindow, ReferenceableParamGroupList rpgList) : base(scanWindow, rpgList)
        {
            
        }

        public override string GetTagName()
        {
            return "scanWindow";
        }


    }
}
