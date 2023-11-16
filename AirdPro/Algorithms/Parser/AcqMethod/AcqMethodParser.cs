using System;
using System.Collections.Generic;
using System.IO;
using System.Xaml;
using System.Xml;
using System.Xml.Serialization;
using AirdPro.Constants;
using ProtoBuf;

namespace AirdPro.Algorithms.Parser;

public class AcqMethodParser
{
    public string rawFilePath;
    public string acqMethodPath;
    public AcqMethodParser(string rawFilePath)
    {
        this.rawFilePath = rawFilePath;
    }

    public Dictionary<string, AcqCompound> parse()
    {
        Dictionary<string, AcqCompound> compoundDict = new Dictionary<string, AcqCompound>();
        if (!rawFilePath.ToUpper().EndsWith(FileFormat.DotD))
        {
            return compoundDict;
        }

        DirectoryInfo dir = new DirectoryInfo(rawFilePath);
        if (!dir.Exists)
        {
            return compoundDict;
        }
        
        acqMethodPath = Path.Combine(Path.Combine(rawFilePath, "AcqData"), "AcqMethod.xml");
        FileInfo file = new FileInfo(acqMethodPath);
        if (file.Exists)
        {
            XmlDocument doc = new XmlDocument();  
            doc.Load(acqMethodPath);  
            XmlNode rootNode = doc.DocumentElement;
            XmlNode rcDevicesXmlNode = null;
            XmlNode scicDevicesXmlNode = null;
            foreach (XmlNode node in rootNode.ChildNodes)  
            {
                if (node.Name.Equals("MethodReport"))
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            // case "RCDevicesXml": 
                            //     rcDevicesXmlNode = childNode;
                            //     break;
                            case "SCICDevicesXml":
                                scicDevicesXmlNode = childNode;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            // if (rcDevicesXmlNode != null)
            // {
            //     string innerXml = rcDevicesXmlNode.InnerXml;
            //     innerXml = innerXml.Replace("&lt;", "<").Replace("&gt;", ">");
            //     XmlDocument rcDevicesXmlDoc = new XmlDocument();
            //     rcDevicesXmlDoc.LoadXml(innerXml);
            //     XmlNode rcRootNode = rcDevicesXmlDoc.DocumentElement;
            // }
           
            if (scicDevicesXmlNode != null)
            {
                string innerXml = scicDevicesXmlNode.InnerXml;
                innerXml = innerXml.Replace("&lt;", "<").Replace("&gt;", ">");
                XmlSerializer serializer = new XmlSerializer(typeof(NewDataSet));
                StringReader reader = new StringReader(innerXml);
                NewDataSet newDataSet = (NewDataSet)serializer.Deserialize(reader);
                reader.Close();
                Dictionary<int, AcqCompound> rowCompDict = new Dictionary<int, AcqCompound>();
                bool start = false;
                bool scan = false;
                int rowIndex;
                AcqCompound currentCompound;
                foreach (SectionInfo sectionInfo in newDataSet.SectionInfos)
                {
                    if (sectionInfo.ParentName != null && sectionInfo.ParentName.Equals("Scan Segments"))
                    {
                        rowIndex = sectionInfo.RowIndex;
                        if (!rowCompDict.ContainsKey(rowIndex))
                        {
                            AcqCompound compound = new AcqCompound();
                            rowCompDict.Add(rowIndex, compound);
                        }
                        
                        currentCompound = rowCompDict[rowIndex];
                        switch (sectionInfo.ID)
                        {
                            case "compoundName":
                                currentCompound.name = sectionInfo.Value;
                                break;
                            case "isISTD":
                                currentCompound.isISTD = sectionInfo.Value;
                                break;
                            case "ms1LowMz":
                                currentCompound.precursorMz = Double.Parse(sectionInfo.Value);
                                break;
                            case "MS1Res":
                                currentCompound.precursorRes = sectionInfo.Value;
                                break;
                            case "ms2LowMz":
                                currentCompound.productMz = Double.Parse(sectionInfo.Value);
                                break;
                            case "MS2Res":
                                currentCompound.productRes = sectionInfo.Value;
                                break;
                            case "fragmentor":
                                currentCompound.fragmentor = sectionInfo.Value;
                                break;
                            case "collisionEnergy":
                                currentCompound.collisionEnergy = sectionInfo.Value;
                                break;
                            case "cellAccVoltage":
                                currentCompound.cellAccVoltage = sectionInfo.Value;
                                break;
                            case "scheduledTime":
                                currentCompound.scheduledTime = sectionInfo.Value;
                                break;
                            case "timeWindow":
                                currentCompound.timeWindow = sectionInfo.Value;
                                break;
                            case "ionPolarity":
                                currentCompound.ionPolarity = sectionInfo.Value;
                                break;
                        }
                    }
                   
                }
                
                foreach (KeyValuePair<int,AcqCompound> keyValuePair in rowCompDict)
                {
                    compoundDict.Add(Math.Round(keyValuePair.Value.precursorMz, 1)+"-"+Math.Round(keyValuePair.Value.productMz, 1), keyValuePair.Value);
                }

                return compoundDict;
            }
        }
        
        return compoundDict;
    }
}