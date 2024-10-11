using AirdPro.csimzMLParser.mzml;

namespace AirdPro.csimzMLParser.exceptions
{
    public class InvalidCVParamValue : NonFatalParseIssue
    {

        CVParam param;
        string value;

        public InvalidCVParamValue(string message)
        {
            this.issueMessage = message;
        }
    }
}
