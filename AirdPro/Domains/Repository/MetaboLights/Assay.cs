using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class Assay
    {
        public List<NameValue> comments { get; set; }
        public Annotation measurementType { get; set; }
        public Annotation technologyType { get; set; }
        public string technologyPlatform { get; set; }
        public string filename { get; set; }

    }
}