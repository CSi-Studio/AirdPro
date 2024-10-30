namespace AirdPro.csimzMLParser.mzml
{
    public class ScanWindow : MzMLContentWithParams
    {
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
