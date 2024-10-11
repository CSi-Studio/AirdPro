using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class ImzMLParseException : MzMLParseException
    {

        public ImzMLParseException(FatalParseIssue issue) : base(issue)
        {
            
        }

        public ImzMLParseException(FatalParseIssue issue, Exception exception) : base(issue, exception)
        {
            
        }

    }
}
