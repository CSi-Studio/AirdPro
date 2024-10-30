namespace AirdPro.csimzMLParser.mzml
{
    public class SourceFileList(int count) : MzMLIDContentList<SourceFile>(count)
    {
        public SourceFileList(SourceFileList sourceFileList, ReferenceableParamGroupList rpgList) : this(sourceFileList.Size())
        { 
            foreach (SourceFile sourceFile in sourceFileList)
            {
                Add(new SourceFile(sourceFile, rpgList));
            }
        }

        public void AddSourceFile(SourceFile sourceFile)
        {
            Add(sourceFile);
        }

        public SourceFile GetSourceFile(string id)
        {
            return Get(id);
        }

        public SourceFile GetSourceFile(int index)
        {            
            return Get(index);
        }

        public override string GetTagName()
        {
            return "ourceFileList";
        }
    }
}
