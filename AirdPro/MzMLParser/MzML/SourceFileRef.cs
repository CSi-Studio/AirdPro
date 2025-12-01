namespace AirdPro.csimzMLParser.mzml
{
    public class SourceFileRef(SourceFile sourceFileRef) : MzMLReference<SourceFile>(sourceFileRef)
    {
        public override string GetTagName()
        {
            return "sourceFileRef";
        }
    }
}
