using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class Activation :  MzMLContentWithParams
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string PRECURSOR_ACTIVATION_ATTRIBUTE_ID = "MS:1000510"; 

        public static readonly string DISSOCIATION_METHOD_ID = "MS:1000044";  

        public Activation() : base()
        {
            
        }

        public Activation(Activation activation, ReferenceableParamGroupList rpgList) : base(activation, rpgList)
        {

        }

        public override string GetTagName()
        {
            return "activation";
        }
    }
}
