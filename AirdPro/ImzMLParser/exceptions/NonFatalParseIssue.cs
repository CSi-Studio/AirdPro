using AirdPro.ImzMLParser.mzml;
using System;
using System.Text;
using static AirdPro.ImzMLParser.exceptions.IIssue;

namespace AirdPro.ImzMLParser.exceptions
{
    [Serializable]
    public abstract class NonFatalParseIssue : IParseIssue
    {
        protected MzMLContent location;

        protected bool attemptedFix = false;

        protected string fixMessage;

        protected string issueTitle;

        protected string issueMessage;

        public void SetIssueLocation(MzMLContent location)
        {
            this.location = location;
        }

        public MzMLContent GetIssueLocation()
        {
            return location;
        }

        public bool HasFixBeenAttempted()
        {
            return attemptedFix;
        }

        public void FixAttempted(string fixMessage)
        {
            this.fixMessage = fixMessage;
            attemptedFix = true;
        }

        protected string GetFixMessage()
        {
            return fixMessage;
        }

        public virtual string GetIssueTitle()
        {
            return issueTitle;
        }

        public virtual string GetIssueMessage()
        {
            string message = issueMessage;

            if (attemptedFix)
                message += "\n" + fixMessage;

            return message;
        }

        public IssueLevel GetIssueLevel()
        {
            return IssueLevel.SEVERE;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append(GetIssueTitle());
            stringBuilder.Append(": ");

            string[] splitString = GetIssueMessage().Split(new[] { "\n" }, StringSplitOptions.None);

            if (splitString.Length > 0)
            {
                stringBuilder.Append(splitString[0]);
            }
            else
            {
                stringBuilder.Append(GetIssueMessage());
            }

            stringBuilder.Append(" at ");
            stringBuilder.Append(location);

            return stringBuilder.ToString();
        }
    }
}
