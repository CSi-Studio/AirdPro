namespace AirdPro.csimzMLParser.mzml
{
    public class ReferenceableParamGroupRef(ReferenceableParamGroup refGroup) : MzMLReference<ReferenceableParamGroup>(refGroup)
    {
        public override string GetTagName()
        {
            return "referenceableParamGroupRef";
        }
    }
}
