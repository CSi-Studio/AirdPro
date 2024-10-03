using AirdPro.csimzMLParser.affair;
using AirdPro.csimzMLParser.obo;
using System;

namespace AirdPro.csimzMLParser.mzml
{
    public class DoubleCVParam : CVParam
    {
        protected double value;

        public DoubleCVParam(OBOTerm term, double value, OBOTerm units) : this(term, value)
        {
            this.units = units;
        }

        public DoubleCVParam(OBOTerm term, double value)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term), "OBOTerm cannot be null for DoubleCVParam");
            }
            this.term = term;
            this.value = value;
        }

        public DoubleCVParam(DoubleCVParam cvParam)
        {
            this.term = cvParam.term;
            this.value = cvParam.value;
            this.units = cvParam.units;
        }

        public double GetValue()
        {
            return value;
        }

        public void SetValue(double value)
        {
            double oldValue = this.value;
            this.value = value;
            if (HasListeners())
            {
                NotifyListeners(new ValueCVParamChangeAffair<double>(this, oldValue, value));
            }            
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
            return (int)Math.Round(value);
        }

        public override long GetValueAsLong()
        {
            return (long)Math.Round(value);
        }

        public override void SetValueAsString(string newValue)
        {
            double parsedValue = Double.Parse(newValue);
            SetValue(parsedValue);
        }

        public override void ResetValue()
        {
            value = 0;
        }
        
    }

}
