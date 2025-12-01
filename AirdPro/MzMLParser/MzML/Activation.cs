using System;
using System.Collections.Generic;
using System.Linq;
namespace AirdPro.csimzMLParser.mzml
{
    public class Activation :  MzMLContentWithParams
    {
        public static readonly string PRECURSOR_ACTIVATION_ATTRIBUTE_ID = "MS:1000510"; 

        public static readonly string DISSOCIATION_METHOD_ID = "MS:1000044";
        public static readonly string ACTIVATION_COLLISION_ENERGY = "MS:1002680";


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
