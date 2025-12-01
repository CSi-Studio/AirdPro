namespace AirdPro.csimzMLParser.mzml
{
    public abstract class MzMLIDContent : MzMLContent, IReferenceableTag
    {
        public string id;
        public string GetID()
        {
            return id;
        }

        public void SetID(string id)
        {
            this.id = id;
        }
    }
}
