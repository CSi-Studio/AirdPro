using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class Detector : Component
    {
        private static readonly long serialVersionUID = 1L;
        public const string DETECTOR_TYPE_ID = "MS:1000026";
        public const string DETECTOR_ATTRIBUTE_ID = "MS:1000481";
        public const string DETECTOR_ACQUISITION_MODE_ID = "MS:1000027";

        public Detector()
        {
        }

        public Detector(Detector detector, ReferenceableParamGroupList rpgList)
            : base(detector, rpgList) // Assuming the base class Component has a parameterized constructor that takes another Component and a ReferenceableParamGroupList
        {
        }

        public override string GetTagName()
        {
            return "detector";
        }
    }
}
