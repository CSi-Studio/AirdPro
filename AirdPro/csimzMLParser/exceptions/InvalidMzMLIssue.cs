using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class InvalidMzMLIssue : FatalParseIssue
    {

    public InvalidMzMLIssue(string message) : base(message)
        {
        
        }

        public InvalidMzMLIssue(String title, String message) : base(title, message)
        { 
        
        }

    }
}
