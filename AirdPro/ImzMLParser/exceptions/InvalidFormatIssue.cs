using AirdPro.ImzMLParser.mzml;
using AirdPro.ImzMLParser.obo;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AirdPro.ImzMLParser.mzml.CVParam;
using static AirdPro.ImzMLParser.obo.OBOTerm;

namespace AirdPro.ImzMLParser.exceptions
{
    public class InvalidFormatIssue : NonFatalParseIssue
    {
        protected string value;
        protected string attribute;
        protected string expectedFormat;
        protected OBOTerm term;
        protected CVParamType expectedParamType;
        protected XMLType xmlType;
        protected StringCVParam newParam;

        public InvalidFormatIssue(OBOTerm term, string value)
        {
            this.term = term;
            this.value = value;
        }

        public InvalidFormatIssue(OBOTerm term, XMLType xmlType)
        {
            this.term = term;
            this.xmlType = xmlType;
        }

        public InvalidFormatIssue(OBOTerm term, CVParam.CVParamType paramType)
        {
            this.term = term;
            this.expectedParamType = paramType;
        }

        public InvalidFormatIssue(string attribute, string expectedFormat, string value)
        {
            this.attribute = attribute;
            this.expectedFormat = expectedFormat;
            this.value = value;
        }

        public void FixAttemptedByChangingType(StringCVParam param)
        {
            newParam = param;
            attemptedFix = true;
        }

        public override string GetIssueTitle()
        {
            return "Invalid value format";
        }

        public override string GetIssueMessage()
        {
            StringBuilder message = new();

            if (term != null && xmlType == null && expectedParamType == null)
            {
                message.Append("Expected");

                if (term.GetValueType() == null)
                    message.Append(" no value");
                else
                    message.Append(" a value of type ").Append(term.GetValueType());

                message.Append(" for CVParam ").Append(term.id);

                if (term.name != null)
                    message.Append(" (").Append(term.name).Append(")");

                if (value == null)
                    message.Append(" but the value attribute was omitted");
                else
                    message.Append(" but got value \"").Append(value).Append("\"");
            }

            if (term != null && xmlType != null)
            {
                message.Append("Unimplemented or unexpected XMLType ");
                message.Append(term.GetValueType().ToString());
                message.Append(" (assigned to term ");
                message.Append(term.id);
                message.Append(")");
            }

            if (term == null)
            {
                message.Append("Expected format ").Append(expectedFormat).Append(" but got ").Append(value);
            }

            if (attemptedFix)
            {
                message.Append("\nAttempted to fix by changing CVParam value type to String.");
            }
            return message.ToString(); 
        }

    }
}
