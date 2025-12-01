namespace AirdPro.csimzMLParser.mzml
{
    public class Component : MzMLOrderedContentWithParams
    {
        public ComponentType Type { get; set; }
        public Component()
        {

        }
        public Component(Component component, ReferenceableParamGroupList rpgList) : base(component, rpgList)
        {
            
        }

        public enum ComponentType
        {
            ComponentType_Analyzer,
            ComponentType_Source,
            ComponentType_Detector,
            ComponentType_Unknown
        }
    }


}
