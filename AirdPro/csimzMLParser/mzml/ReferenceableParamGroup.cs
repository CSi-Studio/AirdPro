using AirdPro.csimzMLParser.util;
using System;

namespace AirdPro.csimzMLParser.mzml
{
    public class ReferenceableParamGroup : MzMLContentWithParams, IReferenceableTag
    {
        private static readonly long serialVersionUID = 1L;
        private static int idNumber = 0;
        private string id; 

        public ReferenceableParamGroup()
        {
            id = "refParam" + idNumber++;
        }

        public ReferenceableParamGroup(string id)
        {
            if (id == null)
                throw new ArgumentException("ID cannot be null for ReferenceableParamGroup.");
            this.id = id;
        }

        public ReferenceableParamGroup(ReferenceableParamGroup rpg) : base(rpg, null)
        {
            id = rpg.id;
        }

        public string GetID()
        {
            return id;
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        public override string GetXMLAttributeText()
        {
            return "id=\"" + XMLHelper.EnsureSafeXML(this.GetID()) + "\"";
        }

        public override string ToString()
        {
            return "referenceableParamGroup: " + id;
        }

        public override string GetTagName()
        {
            return "referenceableParamGroup";
        }
    }
}
