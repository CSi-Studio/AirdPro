using System.Collections.Generic;
using System.Xml.Serialization;

namespace AirdPro.Algorithms.Parser.DownloadXML;

[XmlRoot("ProteomeXchangeDataset")]
public class ProteomeXchangeDataset
{
    [XmlArray("DatasetFileList")]
    public List<DatasetFile> datasetFileList;
    
    
}