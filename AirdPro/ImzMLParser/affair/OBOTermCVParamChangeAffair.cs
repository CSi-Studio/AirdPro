using AirdPro.ImzMLParser.mzml;
using AirdPro.ImzMLParser.obo;

namespace AirdPro.ImzMLParser.affair
{
    public class OBOTermCVParamChangeAffair : CVParamChangeAffair
    {
        private readonly OBOTerm oldValue;
        private readonly OBOTerm newValue;

        public OBOTermCVParamChangeAffair(CVParam source, OBOTerm oldValue, OBOTerm newValue) : base(source)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public OBOTerm OldValue
        {
            get { return oldValue; }
        }

        public OBOTerm NewValue
        {
            get { return newValue; }
        }
    }
}
