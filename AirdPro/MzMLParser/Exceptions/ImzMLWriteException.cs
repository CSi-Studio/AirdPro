using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class ImzMLWriteException : Exception
    {
        public ImzMLWriteException(String message) : base(message)
        {
            
        }

        public ImzMLWriteException(String message, Exception exception) : base(message, exception)
        {
            
        }
    }
}
