using System.Xml.Serialization;

namespace AirdPro.Algorithms.Parser.DownloadXML;

public class CvParam
{
    [XmlAttribute("name")]
    public string name;    
    
    [XmlAttribute("value")]
    public string value;   
    
    [XmlAttribute("accession")]
    public string accession;
    
    [XmlAttribute("cvRef")]
    public string cvRef;
}