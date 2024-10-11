namespace AirdPro.csimzMLParser.mzml
{
    public class SelectedIon : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string ION_SELECTION_ATTRIBUTE_ID = "MS:1000455"; 

        public SelectedIon() : base()
        {
            
        }

        public SelectedIon(SelectedIon selectedIon, ReferenceableParamGroupList rpgList) : base(selectedIon, rpgList)
        {
            
        }

        public override string GetTagName()
        {
            return "selectedIon";
        }
    }
}
