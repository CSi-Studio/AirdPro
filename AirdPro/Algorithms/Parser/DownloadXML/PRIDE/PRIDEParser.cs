using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AirdPro.Algorithms.Parser.DownloadXML;

public class PRIDEParser
{
    public static List<string> parse(string xml)
    {
        List<string> downloadList = new List<string>();
        xml = xml.Replace("\r\n-", "\r\n");
        xml = xml.Trim();
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProteomeXchangeDataset));
            StringReader reader = new StringReader(xml);
            ProteomeXchangeDataset project = (ProteomeXchangeDataset)serializer.Deserialize(reader);
            if (project.datasetFileList.Count == 0)
            {
                return downloadList;
            }

            for (var i = 0; i < project.datasetFileList.Count; i++)
            {
                DatasetFile file = project.datasetFileList[i];
                if (file.cvParams.Count == 0)
                {
                    continue;
                }

                if (file.cvParams.Count == 1)
                {
                    downloadList.Add(file.cvParams[0].value);
                }
                else
                {
                    for (int j = 0; j < file.cvParams.Count; j++)
                    {
                        if (file.cvParams[j].accession.Equals("MS:1002846") 
                            || file.cvParams[j].accession.Equals("PRIDE:0000403")
                            || file.cvParams[j].accession.Equals("PRIDE:0000404"))
                        {
                            downloadList.Add(file.cvParams[j].value);
                        }
                    }
                }
               
            }
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }

        return downloadList;
    }
}