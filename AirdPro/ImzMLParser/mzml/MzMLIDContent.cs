namespace AirdPro.ImzMLParser.mzml
{
    public abstract class MzMLIDContent : MzMLContent, IReferenceableTag
    {
        public string Id;
        public string GetID()
        {
            return Id;
        }

        public void SetID(string id)
        {
            Id = id;
        }
    }
}
