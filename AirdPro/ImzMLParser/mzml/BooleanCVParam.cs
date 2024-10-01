using AirdPro.ImzMLParser.affair;
using AirdPro.ImzMLParser.obo;
using System;

namespace AirdPro.ImzMLParser.mzml
{
    public class BooleanCVParam : CVParam
    {
        protected bool value;

        public BooleanCVParam(OBOTerm term, bool value, OBOTerm units) : this(term, value)
        {
            this.units = units;
        }

        public BooleanCVParam(OBOTerm term, bool value) 
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term), "OBOTerm cannot be null for BooleanCVParam");
            }

            this.term = term;
            this.value = value;
        }

        public BooleanCVParam(BooleanCVParam cvParam)
        {
           this.term = cvParam.term;
            this.value = cvParam.value;
            this.units = cvParam.units;
        }

        public override string GetValueAsString()
        {
            return "" + value;
        }

        public override double GetValueAsDouble()
        {
            return value ? 1.0 : 0.0;
        }

        public override int GetValueAsInteger()
        {
            return value ? 1 : 0;
        }

        public override long GetValueAsLong()
        {
            return value ? 1L : 0L;
        }

        public override void SetValueAsString(string newValue)
        {
            bool oldValue = this.value;
            value = bool.Parse(newValue);

            if (HasListeners())
            {
                NotifyListeners(new ValueCVParamChangeAffair<bool>(this, oldValue, value));
            }
        }

        public override void ResetValue()
        {
            value = false;
        }
    }
}
