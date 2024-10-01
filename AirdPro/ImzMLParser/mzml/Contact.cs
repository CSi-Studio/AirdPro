using AirdPro.ImzMLParser.obo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AirdPro.ImzMLParser.mzml
{
    public class Contact : MzMLContentWithParams
    {
        private const long serialVersionUID = 1L;

        /**
         * Accession: Contact organisation (MS:1000590) [Required].
         */
        public static readonly string CONTACT_ORGANISATION_ID = "MS:1000590"; // Required (1)

        /**
         * Accession: Contact name (MS:1000586) [Required].
         */
        public static readonly string CONTACT_NAME_ID = "MS:1000586"; // Required (1)

        /**
         * Accession: Contact person attribute (MS:1000585) [Optional].
         */
        public static readonly string CONTACT_PERSON_ATTRIBUTE_ID = "MS:1000585"; // Optional child (1+)

        /**
         * Default constructor.
         */
        public Contact() : base()
        {
            
        }

        /**
         * Copy constructor.
         * 
         * @param contact Contact to copy
         * @param rpgList New ReferenceableParamGroupList to match references to
         */
        public Contact(Contact contact, ReferenceableParamGroupList rpgList) : base(contact, rpgList)
        {

        }

        /**
         * Returns the name of the contact.
         *
         * @return Contact's name.
         */
        public String GetName()
        {
            return GetCVParam(CONTACT_NAME_ID).GetValueAsString();
        }

        /**
         * Returns the name of the organisation that the contact belongs to.
         *
         * @return Contact's organisation.
         */
        public String GetOrganisation()
        {
            return GetCVParam(CONTACT_ORGANISATION_ID).GetValueAsString();
        }

        /**
         * Set the name of the contact. A new CV param will be added if the contact name is not already present, or will be
         * updated if so.
         *
         * @param name Name of the contact.
         */
        public void SetName(String name)
        {
            CVParam param = GetCVParam(CONTACT_NAME_ID);

            if (param != null)
                param.SetValueAsString(name);
            else
                AddCVParam(new StringCVParam(OBO.GetOBO().GetTerm(CONTACT_NAME_ID), name));
        }

        /**
         * Set the organisation of the contact. A new CV param will be added if the contact organisation is not already
         * present, or will be updated if so.
         *
         * @param organisation Organisation of the contact.
         */
        public void SetOrganisation(String organisation)
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
