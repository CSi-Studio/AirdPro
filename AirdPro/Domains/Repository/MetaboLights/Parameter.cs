using System.Collections.Generic;

namespace AirdPro.Repository.MetaboLights
{
    public class Parameter
    {
        public List<NameValue> comments { get; set; }
        public Annotation parameterName { get; set; }
    }
}