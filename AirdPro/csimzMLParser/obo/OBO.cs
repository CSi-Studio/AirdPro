using HZH_Controls;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AirdPro.csimzMLParser.obo
{
    [Serializable]
    public class OBO
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(OBO));

        private static readonly long serialVersionUID = 1L;

        public static string MS_OBO_URI = "https://raw.githubusercontent.com/HUPO-PSI/psi-ms-CV/master/psi-ms.obo";
        public static string MS_OBO_FULLNAME = "Proteomics Standards Initiative Mass Spectrometry ontology";

        public static string UO_OBO_URI = "http://purl.obolibrary.org/obo/uo.obo";
        public static string UO_OBO_FULLNAME = "Units of Measurement ontology";

        public static string PATO_OBO_FULLNAME = "Phenotype And Trait ontology";
        public static string PATO_OBO_URI = "https://raw.githubusercontent.com/pato-ontology/pato/master/pato.obo";

        public static string IMS_OBO_URI = "https://raw.githubusercontent.com/imzML/imzML/development/imagingMS.obo";
        public static string IMS_OBO_FULLNAME = "Mass Spectrometry Imaging ontology";
        public static string IMS_OBO_ID = "IMS";
        public static string IMS_OBO_VERSION = "???";

        private readonly string path;
        private readonly List<OBO> imports;
        private readonly string ontologyIdentifier;
        private readonly string defaultNamespace;
        private readonly string dataVersion;
        private readonly Dictionary<string, OBOTerm> terms;

        protected static OBO ONTOLOGY;

        private OBO(string location, IOBOLoader loader)
        {
            imports = [];
            terms = [];

            using (StreamReader reader = new(loader.GetInputStream(location)))
            {
                string curLine;
                OBOTerm curTerm = null;
                bool processingTerms = false;

                while ((curLine = reader.ReadLine()) != null)
                {
                    if (curLine.Trim().IsEmpty())
                    {
                        continue;
                    }

                    if (curLine.Trim().Equals("[Term]"))
                    {
                        processingTerms = true;

                        curLine = reader.ReadLine();
                        if (curLine != null)
                        {
                            int indexOfColon = curLine.IndexOf(':');
                            string id = curLine.Substring(indexOfColon + 1).Trim();

                            curTerm = new OBOTerm(this, id);
                            terms.Add(id, curTerm);
                        }
                    }
                    else if (curLine.Trim().Equals("[Typedef]"))
                    {
                        processingTerms = false;
                    }
                    else if (curTerm != null && processingTerms)
                    {
                        curTerm.Parse(curLine);
                    }
                    else
                    {
                        int locationOfColon = curLine.IndexOf(':');
                        string tag = curLine.Substring(0, locationOfColon).Trim();
                        string value = curLine.Substring(locationOfColon + 1).Trim().ToLower();

                        if ("import".Equals(tag))
                        {
                            imports.Add(new OBO(value, loader));
                        }
                        else if ("default-namespace".Equals(tag))
                        {
                            defaultNamespace = value;
                        }
                        else if (tag.Equals("ontology"))
                        {
                            ontologyIdentifier = value;
                        }
                        else if ("data-version".Equals(tag))
                        {
                            dataVersion = value;
                        }
                    }
                }
            }

            // Process relationships
            foreach (OBOTerm term in terms.Values)
            {
                ICollection<string> is_a = term.GetIsA();

                if (is_a != null)
                {
                    foreach (string id in is_a)
                    {
                        OBOTerm parentTerm = GetTerm(id);

                        if (parentTerm == null)
                        {
                            LOGGER.Warn($"Haven't found {id}");
                        }
                        else
                        {
                            parentTerm.AddChild(term);
                            term.AddParent(parentTerm);
                        }
                    }

                    term.ClearIsA();
                }

                // Units
                if (term.unitList != null)
                {
                    foreach (string unitName in term.unitList)
                    {
                        term.AddUnit(GetTerm(unitName));
                    }

                    term.unitList = null;
                }
            }

            return;
        }        

        public static OBO GetOBO()
        {
            if (ONTOLOGY == null)
            {
                try
                {
                    LOGGER.Info("Trying to load obo from files");
                    ONTOLOGY = OBO.LoadOntologyFromFile(IMS_OBO_URI);
                }
                catch (IOException)
                {
                    try
                    {
                        LOGGER.Info("Trying to load obo from URL");
                        ONTOLOGY = OBO.LoadOntologyFromURL(IMS_OBO_URI);
                    }
                    catch (IOException)
                    {
                        LOGGER.Info("Trying to load obo from resource");

                        try
                        {
                            ONTOLOGY = OBO.LoadOntologyFromResource(IMS_OBO_URI);
                        }
                        catch (IOException e)
                        {
                            LOGGER.Error("Failed to load any ontology", e);
                        }
                    }
                }
            }

            return ONTOLOGY;
        }

        public static void DownloadOBO(string oboLocation)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(oboLocation);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                bool redirect = response.StatusCode == HttpStatusCode.MovedPermanently ||
                                response.StatusCode == HttpStatusCode.TemporaryRedirect ||
                                response.StatusCode == HttpStatusCode.SeeOther;

                if (redirect)
                {
                    // Follow the redirect
                    string newUrl = response.Headers["Location"];
                    response.Close();
                    request = (HttpWebRequest)WebRequest.Create(newUrl);
                    response = (HttpWebResponse)request.GetResponse();
                }

                FileStream inStream = (FileStream)response.GetResponseStream();
                string filename = oboLocation.Substring(oboLocation.LastIndexOf('/') + 1);

                InstallOBO(inStream, filename);

                inStream.Close();
                response.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("Error downloading OBO file: " + e.Message);
            }            
        }

        public static string ONTOLOGIES_FOLDER = "Ontologies";

        public static void SetOntologiesFolder(string folder)
        {
            ONTOLOGIES_FOLDER = folder;
        }

        public static void InstallOBO(Stream inStream, string filename)
        {     
            // 确定文件夹路径
            string ontologiesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), ONTOLOGIES_FOLDER);

            // 如果文件夹不存在，则创建
            if (!Directory.Exists(ontologiesFolderPath))
            {
                Directory.CreateDirectory(ontologiesFolderPath);
            }

            // 构建文件路径
            string filePath = Path.Combine(ontologiesFolderPath, filename);

            try
            {
                // 使用using语句确保流正确关闭
                using Stream outStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                byte[] buffer = new byte[1024];
                int bytesRead;

                // 从输入流读取数据并写入文件
                while ((bytesRead = inStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    outStream.Write(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                // 处理可能发生的异常，例如权限问题或磁盘空间不足
                throw new IOException("An error occurred while installing the obo file.", ex);
            }
        }

        public static void SetOBO(OBO obo)
        {
            ONTOLOGY = obo;
        }

        public static OBO LoadOntologyFromURL(string url)
        {
            return new OBO(url, new HTTPOBOLoader());
        }

        public static OBO LoadOntologyFromResource(string resource)
        {
            return new OBO(resource, new ResourceOBOLoader());
        }

        public static OBO LoadOntologyFromFile(string file)
        {
            return new OBO(file, new FileOBOLoader());
        }

        public List<OBO> GetImports()
        {
            return imports;
        }

        public List<OBO> GetFullImportHierarchy()
        {
            List<OBO> fullList = [];

            foreach (OBO importedOBO in this.imports)
            {
                fullList.AddRange(importedOBO.GetFullImportHierarchy());
            }

            fullList.Add(this);

            return fullList;
        }

        public IEnumerable<OBOTerm> GetTerms()
        {
            return terms.Values;
        }

        public OBOTerm GetTerm(string id)
        {
            if (id == null)
            {
                return null;
            }

            //此行报错：KeyNotFoundException: 给定关键字不在字典中
            //OBOTerm term = terms[id];
            OBOTerm term = null;
            if (terms.ContainsKey(id))
            {
                term = terms[id];
                Console.WriteLine("Can not find key " + id + " in terms dictionary!");
            }

            if (terms == null)
            {
                foreach(OBO parent in imports)
                {
                    term = parent.GetTerm(id);
                    if (term != null)
                    {
                        break;
                    }
                }
            }            

            return term;
        }

        public string GetPath() { return path; }    
        
        public string GetDefaultNamespace() { return defaultNamespace; }

        public string GetOntology() { return ontologyIdentifier; }

        public string GetDataVersion() {  return dataVersion; }

        public OBO GetOBOWithID(string id)
        {
            if (ontologyIdentifier.Equals(id, StringComparison.OrdinalIgnoreCase))
                return this;

            OBO foundOBO = null;

            foreach (OBO importedOBO in imports)
            {
                foundOBO = importedOBO.GetOBOWithID(id);

                if (foundOBO != null)
                    break;
            }
            return foundOBO;            
        }

        public override string ToString()
        {
            return path;
        }

        public static string GetNameFromID(string id)
        {
            return id switch
            {
                "IMS" => OBO.IMS_OBO_FULLNAME,
                "MS" => OBO.MS_OBO_FULLNAME,
                "UO" => OBO.UO_OBO_FULLNAME,
                "PATO" => OBO.PATO_OBO_FULLNAME,
                _ => id,
            };
        }
    }

}
