using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class ImzMLWriteException : Exception
    {
        private static readonly long serialVersionUID = 1L;

        public ImzMLWriteException(String message) : base(message)
        {
            
        }

        public ImzMLWriteException(String message, Exception exception) : base(message, exception)
        {
            
        }
    }
}
