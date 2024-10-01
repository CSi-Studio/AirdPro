using AirdPro.ImzMLParser.obo;
using AirdPro.ImzMLParser.util;
using System;

namespace AirdPro.ImzMLParser.mzml
{
    [Serializable]
    public class CV : MzMLIDContent
    {
        /**
    * 序列化版本ID。
    */
        private const long serialVersionUID = 1L;

        private OBO ontology;

        /**
         * 根据URI、全名和ID创建一个CV标签。
         * 
         * @param ontology
         */
        public CV(OBO ontology)
        {
            this.ontology = ontology;

            Id = ontology.GetOntology().ToUpper();
        }

        /**
         * 获取本体的URI。
         * 
         * @return 本体的URI
         */
        public string GetURI()
        {
            return ontology.GetPath();
        }

        /**
         * 获取本体的名称。
         * 
         * @return 本体名称
         */
        public string GetFullName()
        {
            return OBO.GetNameFromID(Id);
        }

        /**
         * 获取使用的本体版本。
         * 
         * @return 使用的版本
         */
        public string GetVersion()
        {
            return ontology.GetDataVersion();
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = $"URI=\"{XMLHelper.EnsureSafeXML(GetURI())}\"";
            attributeText += $" fullName=\"{XMLHelper.EnsureSafeXML(GetFullName())}\"";
            attributeText += $" id=\"{XMLHelper.EnsureSafeXML(Id)}\"";

            if (GetVersion() != null)
            {
                attributeText += $" version=\"{XMLHelper.EnsureSafeXML(GetVersion())}\"";
            }

            return attributeText;
        }

        public override string ToString()
        {
            return $"cv : URI=\"{GetURI()}\" fullName=\"{GetFullName()}\" id=\"{Id}\"{((GetVersion() != null) ? $" version=\"{GetVersion()}\"" : "")}";
        }

        public override string GetTagName()
        {
            return "cv";
        }

    }
}