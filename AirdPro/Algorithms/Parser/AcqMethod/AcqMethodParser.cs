using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AirdPro.Constants;

namespace AirdPro.Algorithms.Parser;

public class AcqMethodParser(string rawFilePath)
{
    public string rawFilePath = rawFilePath;
    public string acqMethodPath;

    public Dictionary<string, AcqCompound> Parse()
    {
        Dictionary<string, AcqCompound> compoundDict = [];
        if (!rawFilePath.ToUpper().EndsWith(FileFormat.DotD))
        {
            return compoundDict;
        }

        DirectoryInfo dir = new(rawFilePath);
        if (!dir.Exists)
        {
            return compoundDict;
        }
        
        acqMethodPath = Path.Combine(Path.Combine(rawFilePath, "AcqData"), "AcqMethod.xml");
        FileInfo file = new(acqMethodPath);
        if (file.Exists)
        {
            XmlDocument doc = new();
            doc.Load(acqMethodPath);  
            XmlNode rootNode = doc.DocumentElement;
            //XmlNode rcDevicesXmlNode = null;
            XmlNode scicDevicesXmlNode = null;
            foreach (XmlNode node in rootNode.ChildNodes)  
            {
                if (node.Name.Equals("MethodReport"))
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {                        
                        switch (childNode.Name)
                        {
                            case "SCICDevicesXml":
                                scicDevicesXmlNode = childNode;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

           
           
            if (scicDevicesXmlNode != null)
            {
                string innerXml = scicDevicesXmlNode.InnerXml;
                innerXml = innerXml.Replace("&lt;", "<").Replace("&gt;", ">");
                XmlSerializer serializer = new(typeof(NewDataSet));
                StringReader reader = new(innerXml);
                NewDataSet newDataSet = (NewDataSet)serializer.Deserialize(reader);
                reader.Close();
                Dictionary<int, AcqCompound> rowCompDict = [];
                int rowIndex;
                AcqCompound currentCompound;
                foreach (SectionInfo sectionInfo in newDataSet.SectionInfos)
                {
                    if (sectionInfo.ParentName != null && sectionInfo.ParentName.Equals("Scan Segments"))
                    {
                        rowIndex = sectionInfo.RowIndex;
                        if (!rowCompDict.ContainsKey(rowIndex))
                        {
                            AcqCompound compound = new();
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