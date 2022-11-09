using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class Protocol
    {
        public List<NameValue> comments { get; set; }
        public string name { get; set; }
        public Annotation protocolType { get; set; }
        public string description { get; set; }
        public string uri { get; set; }
        public string version { get; set; }
        public List<Parameter> parameters { get; set; }

    }
}