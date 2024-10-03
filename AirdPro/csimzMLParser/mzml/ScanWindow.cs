namespace AirdPro.csimzMLParser.mzml
{
    public class ScanWindow : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public ScanWindow() : base()
        {
            
        }

        public ScanWindow(ScanWindow scanWindow, ReferenceableParamGroupList rpgList) : base(scanWindow, rpgList)
        {
            
        }

        public override string GetTagName()
        {
            return "scanWindow";
        }


    }
}
