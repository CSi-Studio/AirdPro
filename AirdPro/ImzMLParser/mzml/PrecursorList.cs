namespace AirdPro.ImzMLParser.mzml
{
    public class PrecursorList : MzMLContentList<Precursor>
    {
        private static readonly long serialVersionUID = 1L;

        public PrecursorList(int count) : base(count)
        {
            
        }

        public PrecursorList(PrecursorList precursorList, ReferenceableParamGroupList rpgList, SourceFileList sourceFileList) : this(precursorList.Size())
        {
            foreach (Precursor precursor in precursorList)
            {
                this.Add(new Precursor(precursor, rpgList, sourceFileList));
            }
        }

        public void AddPrecursor(Precursor precursor)
        {
            Add(precursor);
        }

        public Precursor GetPrecursor(int index)
        {
            return Get(index);
        }

        public override string GetTagName()
        {
            return "precursorList";
        }
    }
}
