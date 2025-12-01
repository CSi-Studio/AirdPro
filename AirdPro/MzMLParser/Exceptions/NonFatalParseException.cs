using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class NonFatalParseException : Exception
    {
        private readonly NonFatalParseIssue issue;

        public NonFatalParseException(NonFatalParseIssue issue) : base(issue.GetIssueTitle())
        {
            this.issue = issue;
        }

        public NonFatalParseIssue Issue
        {
            get { return issue; }
        }
    }
}
