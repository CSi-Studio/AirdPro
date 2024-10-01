using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.exceptions
{
    public interface IIssue
    {
        public enum IssueLevel
        {
            SEVERE,
            ERROR,
            WARNING
        }

        string GetIssueTitle();

        string GetIssueMessage();

        IssueLevel GetIssueLevel();
    }
}
