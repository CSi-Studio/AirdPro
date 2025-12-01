using AirdPro.csimzMLParser.mzml;
using System;

namespace AirdPro.csimzMLParser.exceptions
{
    public class CVParamAccessionNotFoundIssue : NonFatalParseIssue
    {
        private readonly string accession;
        private readonly UserParam userParam;

        public CVParamAccessionNotFoundIssue(string accession)
        {
            this.accession = accession;
            this.userParam = null;
        }

        public CVParamAccessionNotFoundIssue(String accession, UserParam fixAttempted)
        {
            this.accession = accession;
            this.userParam = fixAttempted;

        }

        public override string GetIssueMessage()
        {
            return "Couldn't find " + accession + " in any OBO.";
        }

        public override string GetFixMessage()
        {
            if (userParam == null)
                return fixMessage;
            else
                return "Changed CVParam to UserParam: " + userParam;
        }
    }
}
