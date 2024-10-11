namespace AirdPro.csimzMLParser.mzml
{
    public abstract class MzMLIndexedContentWithParams : MzMLOrderedContentWithParams, IReferenceableTag
    {
        public string id;
    
        public MzMLIndexedContentWithParams()
        {

        }

        public MzMLIndexedContentWithParams(MzMLIndexedContentWithParams mzMLContent, ReferenceableParamGroupList rpgList) : base(mzMLContent, rpgList)
        {
            
        }

        public virtual string GetID()
        {
            return id;
        }
       
        public virtual void SetID(string id)
        {
            this.id = id;
        }
    }
}
