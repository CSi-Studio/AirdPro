using AirdPro.ImzMLParser.obo;
using AirdPro.ImzMLParser.util;

namespace AirdPro.ImzMLParser.mzml
{
    public class UserParam : MzMLContent
    {
        private const long serialVersionUID = 1L;
        private string name;
        private string type;
        private OBOTerm units;
        private string value;

        public UserParam(string name)
        {
            this.name = name;
        }

        public UserParam(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public UserParam(string name, string value, OBOTerm units)
        {
            this.name = name;
            this.value = value;
            this.units = units;
        }

        public UserParam(UserParam userParam)
        {
            this.name = userParam.name;
            this.type = userParam.type;
            this.units = userParam.units;
            this.value = userParam.value;
        }

        public string Name
        {
            get { return name; }
        }

        public void SetType(string type)
        {
            this.type = type;
        }

        public string Type
        {
            get { return type; }
        }

        public void SetUnits(OBOTerm units)
        {
            this.units = units;
        }

        public OBOTerm Units
        {
            get { return units; }
        }

        public void SetValue(string value)
        {
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            return $"userParam: {name} - {(value != null ? value : "")}";
        }

        public override string GetTagName()
        {
            return "userParam";
        }

        public string ToXMLAttributeText()
        {
            string attributes = $" name=\"{XMLHelper.EnsureSafeXML(Name)}\"";

            if (!string.IsNullOrEmpty(Type))
                attributes += $" type=\"{XMLHelper.EnsureSafeXML(Type)}\"";

            if (!string.IsNullOrEmpty(Value))
                attributes += $" value=\"{XMLHelper.EnsureSafeXML(Value)}\"";

            if (Units != null)
            {
                attributes += $" unitCvRef=\"{XMLHelper.EnsureSafeXML(Units.nameSpace)}\"";
                attributes += $" unitAccession=\"{XMLHelper.EnsureSafeXML(Units.id)}\"";
                attributes += $" unitName=\"{XMLHelper.EnsureSafeXML(Units.name)}\"";
            }

            return attributes;
        }
    }
}
