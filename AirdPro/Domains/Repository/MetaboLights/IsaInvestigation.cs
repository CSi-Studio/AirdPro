using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class IsaInvestigation
    {
        public List<NameValue> comments { get; set; }
        public string identifier { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string submissionDate { get; set; }
        public string publicReleaseDate { get; set; }
        public string filename { get; set; }
        public List<Study> studies { get; set; }
    }
}