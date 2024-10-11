using AirdPro.csimzMLParser.exceptions;

namespace AirdPro.csimzMLParser.parser
{
    public interface IParserListener
    {
        void IssueFound(IIssue exception);
    }
}
