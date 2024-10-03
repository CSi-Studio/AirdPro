using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class InvalidMzMLIssue : FatalParseIssue
    {

    public InvalidMzMLIssue(string message) : base(message)
        {
        
        }

        public InvalidMzMLIssue(string title, string message) : base(title, message)
        { 
        
        }

    }
}
