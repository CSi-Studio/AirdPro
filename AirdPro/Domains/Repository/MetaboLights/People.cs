using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class People
    {
        public List<NameValue> comments { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string affiliation { get; set; }
        public string address { get; set; }
        public string fax { get; set; }
        public string midInitials { get; set; }
        public string phone { get; set; }
        public List<Annotation> roles { get; set; }

    }
}