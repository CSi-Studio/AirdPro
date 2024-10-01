using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.exceptions
{
    public class InvalidXPathException : Exception
    {
        protected readonly string xPath;

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
