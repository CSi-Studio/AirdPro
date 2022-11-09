using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class Annotation
    {
        public List<NameValue> comments { get; set; }
        public string annotationValue { get; set; }
        public Source termSource { get; set; }
        public string termAccession { get; set; }
    }
}