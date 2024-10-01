using AirdPro.ImzMLParser.mzml;

namespace AirdPro.ImzMLParser.affair
{
    public abstract class ChildAddRemoveAffair : MzMLAffair
    {
        private readonly MzMLContentWithParams specificSource;

        public ChildAddRemoveAffair(MzMLContentWithParams source) : base(source)
        {
            specificSource = source;
        }

        public new MzMLContentWithParams GetSource()
        {
            return (MzMLContentWithParams)source;
        }
        
    }
}
