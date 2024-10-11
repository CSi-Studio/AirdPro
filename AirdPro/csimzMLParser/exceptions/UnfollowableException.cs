using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class UnfollowableXPathException : InvalidXPathException
    {
        private readonly string subXPath;   

        protected UnfollowableXPathException(string message, string xPath) : base(message, xPath)
        {
            this.subXPath = "";
        }

        public UnfollowableXPathException(String message, String xPath, String subXPath) : base(message, xPath)
        {
            this.subXPath = subXPath;
        }

        public String GetSubXPath()
        {
            return subXPath;
        } 
    }
}
