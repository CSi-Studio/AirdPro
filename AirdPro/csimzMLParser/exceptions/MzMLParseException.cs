using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class MzMLParseException : FatalParseException
    {

    public MzMLParseException(FatalParseIssue issue) : base(issue)
    {
        
    }

    public MzMLParseException(FatalParseIssue issue, Exception exception) : base(issue, exception)
        {
        
    }

}
}
