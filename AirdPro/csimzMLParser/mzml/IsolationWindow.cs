namespace AirdPro.csimzMLParser.mzml
{
    public class IsolationWindow : MzMLContentWithParams
    {
        public static readonly string ISOLATION_WINDOW_ATTRIBUTE_ID = "MS:1000792";
        public static readonly string ISOLATION_WINDOW_TARGET_MZ = "MS:1000827";
        public static readonly string ISOLATION_WINDOW_LOWER_OFFSET_ID = "MS:1000828";
        public static readonly string ISOLATION_WINDOW_UPPER_OFFSET_ID = "MS:1000829";       

        public IsolationWindow() : base()
        {
            
        }

        public IsolationWindow(IsolationWindow isolationWindow, ReferenceableParamGroupList rpgList) : base(isolationWindow, rpgList)
        {
           
        }

        public override string GetTagName()
        {
            return "isolationWindow";
        }
    }
}
