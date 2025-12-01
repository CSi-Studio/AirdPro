using System;
using static AirdPro.csimzMLParser.exceptions.IIssue;

namespace AirdPro.csimzMLParser.exceptions
{
    [Serializable]
    public class FatalParseIssue : IParseIssue {

        string title;
        string message;

        public FatalParseIssue(string message)
        {
            this.title = message;
            this.message = message;
        }

        public FatalParseIssue(string title, string message)
        {
            this.title = title;
            this.message = message;
        }

        public virtual string GetIssueTitle()
        {
            return title;
        }

        public virtual string GetIssueMessage()
        {
            return message;
        }

        public virtual IssueLevel GetIssueLevel()
        {
            return IssueLevel.SEVERE;
        }


    }
}
