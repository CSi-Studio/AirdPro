using AirdPro.ImzMLParser.mzml;

namespace AirdPro.ImzMLParser.affair
{
    public class ValueCVParamChangeAffair<T> : CVParamChangeAffair
    {
        private readonly T oldValue;
        private readonly T newValue;

        public ValueCVParamChangeAffair(CVParam source, T oldValue, T newValue) : base(source)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public T OldValue
        {
            get { return oldValue; }
        }

        public T NewValue
        {
            get { return newValue; }
        }
    }
}
