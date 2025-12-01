using AirdPro.csimzMLParser.mzml;
using System;
using System.Text;
using static AirdPro.csimzMLParser.exceptions.IIssue;

namespace AirdPro.csimzMLParser.exceptions
{
    public class MissingReferenceIssue : NonFatalParseIssue
    {
        public readonly string reference;
        private readonly string tagName;
        private readonly string attributeName;
        private MzMLContent newReference;

        public MissingReferenceIssue(string reference, string tagName, string attributeName)
        {
            this.reference = reference;
            this.tagName = tagName;
            this.attributeName = attributeName;
        }

        public void FixAttemptedByRemovingReference()
        {
            this.attemptedFix = true;
        }

        public void FixAttemptedByChangingReference(MzMLContent newReference)
        {
            this.attemptedFix = true;
            this.newReference = newReference;
        }

        public override string GetIssueTitle()
        {
            return "Missing reference " + reference + " at <" + tagName + ">";

        }

        public override string GetIssueMessage()
        {
            StringBuilder message = new ("Expected the attribute ");

            message.Append(attributeName);
            message.Append(" to have a valid reference (");
            message.Append(reference);
            message.Append(") at <");
            message.Append(tagName);
            message.Append(">\n");

            if (attemptedFix)
            {
                if (newReference == null)
                {
                    message.Append("Attempted to fix by removing the reference");
                }
                else
                {
                    message.Append("Attempted to fix by changing the reference to ");
                    message.Append(newReference);
                }
            }

            return message.ToString();
        }

        public override IssueLevel GetIssueLevel()
        {
            return IssueLevel.WARNING;
        }
    }
}
