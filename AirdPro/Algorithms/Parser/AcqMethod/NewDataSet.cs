using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AirdPro.Algorithms.Parser;

[XmlRoot("NewDataSet")]
public class NewDataSet
{
    [XmlElement("SectionInfo")]
    public List<SectionInfo> SectionInfos { get; set; }
}