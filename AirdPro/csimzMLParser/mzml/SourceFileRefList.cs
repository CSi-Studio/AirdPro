namespace AirdPro.csimzMLParser.mzml
{
    public class SourceFileRefList(int count) : MzMLContentList<SourceFileRef>(count)
    {
        public SourceFileRefList(SourceFileRefList sourceFileRefList, SourceFileList sourceFileList) : this(sourceFileRefList.Size())
        {            
            foreach (SourceFileRef sourceFileRef in sourceFileRefList)
            {
                foreach (SourceFile sourceFile in sourceFileList)
                {
                    if (sourceFileRef.GetReference().GetID().Equals(sourceFile.GetID()))
                    {
                        Add(new SourceFileRef(sourceFile));
                    }
                }
            }
        }

        public void AddSourceFileRef(SourceFileRef sourceFileRef)
        {
            Add(sourceFileRef);
        }

        public override string GetTagName()
        {
            return "sourceFileRefList";
        }
    }
}
