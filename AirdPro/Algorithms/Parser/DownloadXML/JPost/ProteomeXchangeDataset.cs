using System.Collections.Generic;
using System.Xml.Serialization;

namespace AirdPro.Algorithms.Parser.DownloadXML;

[XmlRoot("jPostDataset")]
public class JPostDataset
{
    [XmlArray("DatasetFileList")]
    public List<DatasetFile> datasetFileList;
    
    
}