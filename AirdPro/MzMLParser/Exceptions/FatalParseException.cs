using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class FatalParseException : Exception
    {
        public readonly FatalParseIssue issue;

        public FatalParseException(FatalParseIssue issue) : base(issue.GetIssueMessage())
        {
            this.issue = issue;
        }

        public FatalParseException(FatalParseIssue issue, Exception exception) : base(issue.GetIssueMessage(), exception)
        {
            this.issue = issue;
        }

        public FatalParseIssue GetIssue()
        {
            return issue;
        }
    }
}
