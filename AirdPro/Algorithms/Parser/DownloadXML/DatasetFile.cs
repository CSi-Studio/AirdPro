using System.Collections.Generic;
using System.Xml.Serialization;

namespace AirdPro.Algorithms.Parser.DownloadXML;

public class DatasetFile
{
    [XmlElement("cvParam")]
    public List<CvParam> cvParams;
    
    [XmlAttribute("name")]
    public string name;
    
    [XmlAttribute("id")]
    public string id;
}