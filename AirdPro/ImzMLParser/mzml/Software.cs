using AirdPro.ImzMLParser.obo;
using AirdPro.ImzMLParser.util;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace AirdPro.ImzMLParser.mzml
{
    [Serializable]
    public class Software : MzMLContentWithParams, IReferenceableTag
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Software));
        private static readonly long serialVersionUID = 1L;
        public const string SOFTWARE_ID = "MS:1000531"; // Required child (1)
        public const string CUSTOM_UNRELEASED_TOOL_ID = "MS:1000799";

        protected string id; // Required
        protected string version; // Required

        public Software(string id, string version)
        {
            this.id = id;
            this.version = version;
        }

        public Software(Software software, ReferenceableParamGroupList rpgList)
            : base(software, rpgList) 
        {
            this.id = software.id;
            this.version = software.version;
        }

        public string GetID()
        {
            return id;
        }

        public string GetVersion()
        {
            return version;
        }

        public void SetVersion(string version)
        {
            this.version = version;
        }

        public override string ToString()
        {
            return $"software: {id} {version}";
        }

        public override string GetXMLAttributeText()
        {
            // Assuming there is an XmlHelper class with a method called EnsureSafeXml
            return $"id=\"{XMLHelper.EnsureSafeXML(id)}\" version=\"{XMLHelper.EnsureSafeXML(version)}\"";
        }

        public override string GetTagName()
        {
            return "software";
        }

        public void SetID(string id)
        {
            this.id = id;
        }

        public static Software Create()
        {
            string version = "Unknown";

            // 获取程序集的 Manifest 中的 jimzMLParser.properties 文件流
            string resourcePath = "jimzMLParser.properties"; 
            try
            {
                using Stream inStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath) ?? throw new FileNotFoundException("Resource not found.", resourcePath);
                var properties = new Dictionary<string, string>();
                using (StreamReader reader = new(inStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.StartsWith("#") && !string.IsNullOrWhiteSpace(line))
                        {
                            string[] keyValue = line.Split(new char[] { '=' }, 2);
                            if (keyValue.Length == 2)
                            {
                                properties[keyValue[0].Trim()] = keyValue[1].Trim();
                            }
                        }
                    }
                }
                version = properties.ContainsKey("version") ? properties["version"] : version;
            }
            catch (Exception ex)
            {
                logger.Error("Error loading software version from properties file.", ex);
            }

            Software software = new Software("jimzMLParser", version);
            software.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(Software.CUSTOM_UNRELEASED_TOOL_ID)));

            return software;
        }

    }
}
