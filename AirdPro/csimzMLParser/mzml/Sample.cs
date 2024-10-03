using AirdPro.csimzMLParser.util;
using System;

namespace AirdPro.csimzMLParser.mzml
{
    public class Sample : MzMLContentWithParams, IReferenceableTag
    {
        private const long serialVersionUID = 1L;

        public static int idNumber = 0;
        public string id;		// Required
        public string name;	// Optional

        public Sample() : this("sample" + idNumber++)
        {
            
        }

        public Sample(string id) : this(id, null)
        {
            
        }

        public Sample(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Sample(Sample sample, ReferenceableParamGroupList rpgList) : base(sample, rpgList)
        {
            this.id = sample.id;
            this.name = sample.name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetID()
        {
            return id;
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = "id=\"" + XMLHelper.EnsureSafeXML(id) + "\"";

            if (name != null)
            {
                attributeText += " name=\"" + XMLHelper.EnsureSafeXML(name) + "\"";
            }

            return attributeText;
        }

        public override string ToString()
        {
            return $"sample: id=\"{id}\"{(name != null && !string.IsNullOrEmpty(name) ? $" name=\"{name}\"" : "")}";
        }

        public override string GetTagName()
        {
            return "sample";
        }

        public virtual void SetID(String id)
        {
            this.id = id;
        }
    }
}
