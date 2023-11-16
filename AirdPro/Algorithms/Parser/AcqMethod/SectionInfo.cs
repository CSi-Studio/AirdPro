using System.Xml.Serialization;

namespace AirdPro.Algorithms.Parser;

[XmlType(TypeName = "SectionInfo")]
public class SectionInfo
{
    [XmlElement("RepeaterId1")]
    public string RepeaterId1 { get; set; }

    [XmlElement("RepeaterId2")]
    public string RepeaterId2 { get; set; }
    
    [XmlElement("ParentName")]
    public string ParentName { get; set; }
    
    [XmlElement("ParentID")]
    public string ParentID { get; set; }
    
    [XmlElement("ParentType")]
    public string ParentType { get; set; }
    
    [XmlElement("SectionType")]
    public string SectionType { get; set; }
    
    [XmlElement("RowIndex")]
    public int RowIndex { get; set; }
    
    [XmlElement("ColumnIndex")]
    public int ColumnIndex { get; set; }
    
    [XmlElement("Name")]
    public string Name { get; set; }
    
    [XmlElement("ID")]
    public string ID { get; set; }
    
    [XmlElement("Value")]
    public string Value { get; set; }
    
    [XmlElement("NameAlignment")]
    public string NameAlignment { get; set; }
    
    [XmlElement("ValueAlignment")]
    public string ValueAlignment { get; set; }
}