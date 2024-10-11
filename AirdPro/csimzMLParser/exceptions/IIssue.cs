namespace AirdPro.csimzMLParser.exceptions
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
