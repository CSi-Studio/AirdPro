using AirdPro.csimzMLParser.obo;
using System;

namespace AirdPro.csimzMLParser.mzml
{
    public class EmptyCVParam : CVParam
    {
        private static readonly string NoValueError = "No value to get in EmptyCVParam";

        public EmptyCVParam(OBOTerm term)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term), "OBOTerm cannot be null for EmptyCVParam");
            }

            this.term = term;
        }

        public EmptyCVParam(OBOTerm term, OBOTerm units)
        {
            if (term == null)
            {
                throw new ArgumentNullException(nameof(term), "OBOTerm term cannot be null for EmptyCVParam");
            }

            this.term = term;
            this.units = units;
        }

        public EmptyCVParam(EmptyCVParam cvParam)
        {
            this.term = cvParam.term;
            this.units = cvParam.units;
        }

        public override string GetValueAsString()
        {
            return null;
        }

        public override double GetValueAsDouble()
        {
            throw new NotSupportedException(NoValueError);
        }

        public override int GetValueAsInteger()
        {
            throw new NotSupportedException(NoValueError);
        }

        public override long GetValueAsLong()
        {
            throw new NotSupportedException(NoValueError);
        }

        public override void SetValueAsString(string newValue)
        {
            throw new NotSupportedException("Cannot change the value of an EmptyCVParam");
        }

        public override void ResetValue()
        {
            
        }
        
    }
}
