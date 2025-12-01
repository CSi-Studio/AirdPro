using AirdPro.csimzMLParser.obo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AirdPro.csimzMLParser.mzml
{
    public class Contact : MzMLContentWithParams
    {
        private const long serialVersionUID = 1L;

        public static readonly string CONTACT_ORGANISATION_ID = "MS:1000590"; 
        public static readonly string CONTACT_NAME_ID = "MS:1000586"; 
        public static readonly string CONTACT_PERSON_ATTRIBUTE_ID = "MS:1000585"; 

        public Contact() : base()
        {
            
        }

        public Contact(Contact contact, ReferenceableParamGroupList rpgList) : base(contact, rpgList)
        {

        }

        public string GetName()
        {
            CVParam cvParam = GetCVParam(CONTACT_NAME_ID);
            if (cvParam == null)
                return null;
            return GetCVParam(CONTACT_NAME_ID).GetValueAsString();
        }

        public string GetOrganisation()
        {
            CVParam cvParam = GetCVParam(CONTACT_ORGANISATION_ID);
            if (cvParam == null)
                return null;
            return GetCVParam(CONTACT_ORGANISATION_ID).GetValueAsString();
        }

        public void SetName(string name)
        {
            CVParam param = GetCVParam(CONTACT_NAME_ID);

            if (param != null)
                param.SetValueAsString(name);
            else
                AddCVParam(new StringCVParam(OBO.GetOBO().GetTerm(CONTACT_NAME_ID), name));
        }

        public void SetOrganisation(string organisation)
        {
            CVParam param = GetCVParam(CONTACT_ORGANISATION_ID);

            if (param != null)
                param.SetValueAsString(organisation);
            else
                AddCVParam(new StringCVParam(OBO.GetOBO().GetTerm(CONTACT_ORGANISATION_ID), organisation));
        }
        
        public override string ToString()
        {
            return "contact: " + GetName() + " (" + GetOrganisation() + ")";
        }
       
        public override string GetTagName()
        {
            return "contact";
        }
    }
}
