using AirdPro.csimzMLParser.obo;
using static AirdPro.csimzMLParser.exceptions.IIssue;

namespace AirdPro.csimzMLParser.exceptions
{
    public class ObsoleteTermUsed : NonFatalParseIssue
    {
        private readonly OBOTerm term;
        public ObsoleteTermUsed(OBOTerm term)
        {
            this.term = term;
        }
        
        public override string GetIssueTitle()
        {
            return "Obsolete term used in cvParam";
        }

        public override string GetIssueMessage()
        {
            return term.id + " (" + term.name + ") used while it is marked OBSOLETE";
        }

        public override IssueLevel GetIssueLevel()
        {
            return IssueLevel.WARNING;
        }
    }
}
