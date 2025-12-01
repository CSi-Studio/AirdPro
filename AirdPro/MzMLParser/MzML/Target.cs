namespace AirdPro.csimzMLParser.mzml
{
    public class Target : MzMLContentWithParams
    {
        public Target() : base() { }

        public Target(Target target, ReferenceableParamGroupList rpgList) : base(target, rpgList) { }

        public override string GetTagName() 
        {
            return "target";
        }
    }
}
