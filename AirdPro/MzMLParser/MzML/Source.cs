namespace AirdPro.csimzMLParser.mzml
{
    public class Source : Component
    {
        public static readonly string IONISATION_TYPE_ID = "MS:1000008";
        public static readonly string SOURCE_ATTRIBUTE_ID = "MS:1000482";
        public static readonly string INLET_TYPE_ID = "MS:1000007";
        public static readonly string SAMPLE_STAGE_ID = "IMS:1000002";

        public Source()
        {
        }

        public Source(Source source, ReferenceableParamGroupList rpgList) : base(source, rpgList)
        {
        }

        public override string GetTagName()
        {
            return "source";
        }
    }
}
