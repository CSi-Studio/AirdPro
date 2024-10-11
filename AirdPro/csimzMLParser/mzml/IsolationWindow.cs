namespace AirdPro.csimzMLParser.mzml
{
    public class IsolationWindow : MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string ISOLATION_WINDOW_ATTRIBUTE_ID = "MS:1000792"; 

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
