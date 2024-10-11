using AirdPro.csimzMLParser.mzml;

namespace AirdPro.csimzMLParser.affair
{
    public class CVParamAddRemoveAffair : ChildAddRemoveAffair
    {
        private readonly CVParam param;
    
    
        public CVParamAddRemoveAffair(MzMLContentWithParams source, CVParam param) : base(source)
        {
            this.param = param;
        }

        public CVParam GetCVParam()
        {
            return param;
        }
    }
}
