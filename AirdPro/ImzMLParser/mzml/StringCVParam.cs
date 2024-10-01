using AirdPro.ImzMLParser.affair;
using AirdPro.ImzMLParser.obo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class StringCVParam : CVParam
    {
        protected string value;

        public StringCVParam(OBOTerm term, string value, OBOTerm units) : this(term, value)
        {           
            this.units = units;
        }

        public StringCVParam(OBOTerm term, string value)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term), "OBOTerm cannot be null for StringCVParam");
            }

            this.term = term;
            this.value = value;
        }

        public StringCVParam(StringCVParam cvParam)
        {
            this.term = cvParam.term;
            this.value = cvParam.value;
            this.units = cvParam.units;
        }

        public string GetValue()
        {
            return value; 
        }
            
        public void SetValue(string value) 
        {
            string oldValue = this.value;
            this.value = value;
            if (HasListeners())
            {
                NotifyListeners(new ValueCVParamChangeAffair<string>(this, oldValue, value));
            }
        }

        public override string GetValueAsString()
        {
            return "" + GetValue();
        }

        public override double GetValueAsDouble()
        {
            double convertedValue = double.NaN;

            try
            {
                convertedValue = double.Parse(value);
            }
            catch (FormatException) { }
            catch (ArgumentNullException) { }

            return convertedValue;
        }

        public override int GetValueAsInteger()
        {
            int convertedValue = int.MinValue;

            try
            {
                convertedValue = int.Parse(value);
            }
            catch (FormatException) { }
            catch (ArgumentNullException) { }

            return convertedValue;
        }

        public override long GetValueAsLong()
        {
            long convertedValue = long.MinValue;

            try
            {
                convertedValue = long.Parse(value);
            }
            catch (FormatException) { }
            catch (ArgumentNullException) { }

            return convertedValue;
        }

        public override void SetValueAsString(string newValue)
        {
            SetValue(newValue);
        }

        public override void ResetValue()
        {
            value = "";
        }

    }
}
