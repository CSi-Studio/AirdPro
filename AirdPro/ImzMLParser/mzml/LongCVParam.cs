using AirdPro.ImzMLParser.affair;
using AirdPro.ImzMLParser.obo;
using System;

namespace AirdPro.ImzMLParser.mzml
{
    public class LongCVParam : CVParam
    {
        protected long value;

        public LongCVParam(OBOTerm term, long value, OBOTerm units) : this(term, value)
        {
            this.units = units;
        }

        public LongCVParam(OBOTerm term, long value) 
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term), "OBOTerm cannot be null for LongCVParam");
            }
            this.term = term;
            this.value = value;
        }

        public LongCVParam(LongCVParam cvParam)
        {
            this.term = cvParam.term;
            this.value = cvParam.value;
            this.units = cvParam.units;
        }

        public long GetValue()
        {
            return value;
        }

        public void SetValue(long value)
        {
            long oldValue = this.value;
            this.value = value;

            if (HasListeners())
            {
                NotifyListeners(new ValueCVParamChangeAffair<long>(this, oldValue, this.value));
            }           
        }

        public override string GetValueAsString()
        {
            return "" + GetValueAsLong();
        }

        public override double GetValueAsDouble()
        {
            return value;
        }

        public override int GetValueAsInteger()
        {
            return (int)value;
        }

        public override long GetValueAsLong()
        {
            return value;
        }

        public override void SetValueAsString(string newValue)
        {
            long parsedValue = long.Parse(newValue);
            SetValue(parsedValue);
        }

        public override void ResetValue()
        {
            value = 0;
        }
    }
}
