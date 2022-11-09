using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class Source
    {
        public List<NameValue> comments { get; set; }
        public string name { get; set; }
        public string file { get; set; }
        public string version { get; set; }
        public string description { get; set; }

    }
}