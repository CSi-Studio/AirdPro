using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class Publication
    {
        public List<NameValue> comments { get; set; }
        public string title { get; set; }
        public string authorList { get; set; }
        public string pubMedID { get; set; }
        public string doi { get; set; }
        public Annotation status { get; set; }

    }
}