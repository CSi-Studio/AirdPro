using AirdPro.csimzMLParser.mzml;
using System.Text;

namespace AirdPro.csimzMLParser.exceptions
{
    public class InvalidExternalOffset : NonFatalParseIssue
    {
        public readonly MzMLDataContainer container;
        public readonly long offset;

        public InvalidExternalOffset(MzMLDataContainer container, long offset)
        {
            this.container = container;
            this.offset = offset;
            this.attemptedFix = true;
        }

        public override string GetIssueTitle()
        {
            return "Invalid offset " + offset + " at " + container + "";

        }

        public override string GetIssueMessage()
        {
            StringBuilder message = new("Invalid offset supplied (");
            message.Append(offset);
            message.Append("). Offset must be a non-negative integer.\n");

            if (attemptedFix)
            {
                message.Append("Attempted to fix by adding 2^32 to offset.");
            }

            return message.ToString();
        }
    }
}
