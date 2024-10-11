namespace AirdPro.csimzMLParser.mzml
{
    public class SourceFileRef : MzMLReference<SourceFile>
    {
        private static readonly long serialVersionUID = 1L;

        public SourceFileRef(SourceFile sourceFileRef) : base(sourceFileRef)
        { 
        
        }

        public override string GetTagName()
        {
            return "sourceFileRef";
        }
    }
}
