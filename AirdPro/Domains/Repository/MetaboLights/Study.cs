using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class Study
    {
        public List<NameValue> comments { get; set; }
        public string identifier { get; set; }
        public string filename { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string submissionDate { get; set; }
        public string publicReleaseDate { get; set; }
        public List<People> people { get; set; }

        public List<Annotation> studyDesignDescriptors { get; set; }
        public List<Publication> publications { get; set; }

        public List<Factor> factors { get; set; }
        public List<Protocol> protocols { get; set; }
        public List<Assay> assays { get; set; }
    }
}