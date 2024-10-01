using AirdPro.ImzMLParser.affair;
using AirdPro.ImzMLParser.obo;
using System;

namespace AirdPro.ImzMLParser.mzml
{
    public class IntegerCVParam : CVParam
    {
        protected int value;

        public IntegerCVParam(OBOTerm term, int value, OBOTerm units) : this(term, value)
        {
            this.units = units;
        }

        public IntegerCVParam(OBOTerm term, int value)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term), "OBOTerm cannot be null for IntegerCVParam");
            }
            this.term = term;
            this.value = value;
        }

        public IntegerCVParam(IntegerCVParam cvParam)
        {
            this.term = cvParam.term;
            this.value = cvParam.value;
            this.units = cvParam.units;
        }

        public int GetValue()
        {
            return value;
        }

        public void SetValue(int value)
        {
            int oldValue = this.value;
            this.value = value;

            if (HasListeners())
                NotifyListeners(new ValueCVParamChangeAffair<int>(this, oldValue, value));
        }

        public override string GetValueAsString()
        {
            return "" + GetValue();
        }

        public override double GetValueAsDouble()
        {
            return value;
        }

        public override int GetValueAsInteger()
        {
            return value;
        }

        public override long GetValueAsLong()
        {
            return value;
        }

        public override void SetValueAsString(string newValue)
        {
            int parsedValue = int.Parse(newValue);
            SetValue(parsedValue);
        }

        public override void ResetValue()
        {
            value = 0;
        }  

    }
}
