using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.exceptions
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
