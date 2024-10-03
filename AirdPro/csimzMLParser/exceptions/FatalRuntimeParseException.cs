using System;

namespace AirdPro.csimzMLParser.exceptions
{    
    public class FatalRuntimeParseException : Exception
    {
        public readonly FatalParseIssue issue;

        public FatalRuntimeParseException(FatalParseIssue issue) : base(issue.GetIssueMessage())
        {
            this.issue = issue;
        }

        public FatalRuntimeParseException(FatalParseIssue issue, Exception exception) : base(issue.GetIssueMessage(), exception)
        {
            this.issue = issue;
        }

        public FatalParseIssue GetIssue()
        {
            return issue;
        }
    }
}
