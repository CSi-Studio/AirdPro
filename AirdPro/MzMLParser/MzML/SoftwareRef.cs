namespace AirdPro.csimzMLParser.mzml
{    
    public class SoftwareRef : MzMLReference<Software>
    {

        private const long serialVersionUID = 1L;

        
        public SoftwareRef(Software reference) : base(reference)
        {
        }

        public override string GetTagName()
        {
            return "softwareRef";
        }
    }
}
