using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class InvalidXPathException : Exception
    {
        public readonly string xPath;

        public InvalidXPathException(string message, string xPath) : base(message)
        {
            this.xPath = xPath;
        }

        public string GetXPath()
        {
            return xPath;
        }
    }
}
