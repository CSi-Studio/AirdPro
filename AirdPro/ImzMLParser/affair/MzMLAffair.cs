using AirdPro.ImzMLParser.mzml;

namespace AirdPro.ImzMLParser.affair
{
    public abstract class MzMLAffair
    {
        protected readonly MzMLContent source;
        private bool notifyParents = true;

        protected MzMLAffair(MzMLContent source)
        {
            this.source = source;
        }

        public void NotifyParents(bool notifyParents)
        {
            this.notifyParents = notifyParents;
        }

        public bool NotifyParents()
        {
            return notifyParents;
        }

        public virtual MzMLContent GetSource()
        {
            return source;
        }
    }
}
