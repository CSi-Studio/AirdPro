using AirdPro.csimzMLParser.util;

namespace AirdPro.csimzMLParser.mzml
{
    public class ProcessingMethod : MzMLOrderedContentWithParams
    {
        private const long serialVersionUID = 1L;

        public static readonly string DATA_TRANSFORMATION_ID = "MS:1000452"; // Required child (1+)
        public static readonly string DATA_PROCESSING_PARAMETER_ID = "MS:1000630"; // Optional child (1+)
        public static readonly string FILE_FORMAT_CONVERSION_ID = "MS:1000530";
        public static readonly string CONVERSION_TO_MZML_ID = "MS:1000544";

        private Software softwareRef;

        public ProcessingMethod(Software softwareRef)
        {
            this.softwareRef = softwareRef;
        }

        public ProcessingMethod(ProcessingMethod pm, ReferenceableParamGroupList rpgList, SoftwareList softwareList)
            : base(pm, rpgList)
        {
            if (pm.softwareRef != null && softwareList != null)
            {
                foreach (Software software in softwareList)
                {
                    if (pm.softwareRef.GetID().Equals(software.GetID()))
                    {
                        softwareRef = software;
                        break;
                    }
                }
            }
        }

        public void SetSoftwareRef(Software softwareRef)
        {
            this.softwareRef = softwareRef;
        }

        public Software GetSoftwareRef()
        {
            return softwareRef;
        }

        public virtual string ToXMLAttributeText()
        {
            return $"softwareRef=\"{XMLHelper.EnsureSafeXML(softwareRef.GetID())}\"";
        }

        public override string ToString()
        {
            return $"processingMethod: softwareRef=\"{softwareRef.GetID()}\"";
        }

        public override string GetTagName()
        {
            return "processingMethod";
        }
    }
}
