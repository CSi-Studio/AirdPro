using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AirdPro.ImzMLParser.obo
{
    [Serializable]
    public class OBO
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(OBO));

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

        private string path;
        private List<OBO> imports;
        private string defaultNamespace;
        private string ontologyIdentifier;
        private string dataVersion;
        private Dictionary<string, OBOTerm> terms;

        private static OBO ONTOLOGY;

        protected OBO(string location, IOBOLoader loader)
        {
            imports = [];
            terms = [];

            using (StreamReader reader = new StreamReader(loader.GetInputStream(location)))
            {
                string line;
                OBOTerm currentTerm = null;
                bool processingTerms = false;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    if (line.Equals("[Term]", StringComparison.OrdinalIgnoreCase))
                    {
                        processingTerms = true;

                        line = reader.ReadLine();
                        if (line != null)
                        {
                            int indexOfColon = line.IndexOf(':');
                            string id = line.Substring(indexOfColon + 1).Trim();

                            currentTerm = new OBOTerm(this, id);
                            terms.Add(id, currentTerm);
                        }
                    }
                    else if (line.Equals("[Typedef]", StringComparison.OrdinalIgnoreCase))
                    {
                        processingTerms = false;
                    }
                    else if (currentTerm != null && processingTerms)
                    {
                        currentTerm.Parse(line);
                    }
                    else
                    {
                        int indexOfColon = line.IndexOf(':');
                        string tag = line.Substring(0, indexOfColon).Trim();
                        string value = line.Substring(indexOfColon + 1).Trim().ToLower();

                        if (tag.Equals("import", StringComparison.OrdinalIgnoreCase))
                        {
                            imports.Add(new OBO(value, loader));
                        }
                        else if (tag.Equals("default-namespace", StringComparison.OrdinalIgnoreCase))
                        {
                            defaultNamespace = value;
                        }
                        else if (tag.Equals("ontology", StringComparison.OrdinalIgnoreCase))
                        {
                            ontologyIdentifier = value;
                        }
                        else if (tag.Equals("data-version", StringComparison.OrdinalIgnoreCase))
                        {
                            dataVersion = value;
                        }
                    }
                }
            }

            // Process relationships
            foreach (var term in terms.Values)
            {
                var is_a = term.GetIsA();

                if (is_a != null)
                {
                    foreach (var id in is_a)
                    {
                        var parentTerm = GetTerm(id);

                        if (parentTerm == null)
                        {
                            logger.Warn($"Haven't found {id}");
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
                    foreach (var unitName in term.unitList)
                    {
                        term.AddUnit(GetTerm(unitName));
                    }

                    term.unitList = null;
                }
            }
        }

        public OBO(string url, HTTPOBOLoader hTTPOBOLoader)
        {
            this.url = url;
            this.hTTPOBOLoader = hTTPOBOLoader;
        }

        public OBO(string resource, ResourceOBOLoader resourceOBOLoader)
        {
            this.resource = resource;
            this.resourceOBOLoader = resourceOBOLoader;
        }

        public OBO(string file, FileOBOLoader fileOBOLoader)
        {
            this.file = file;
            this.fileOBOLoader = fileOBOLoader;
        }

        public static OBO GetOBO()
        {
            if (ONTOLOGY == null)
            {
                try
                {
                    logger.Info("Trying to load obo from files");
                    ONTOLOGY = LoadOntologyFromFile(IMS_OBO_URI);
                }
                catch (IOException ex)
                {
                    try
                    {
                        logger.Info("Trying to load obo from URL");
                        ONTOLOGY = LoadOntologyFromURL(IMS_OBO_URI);
                    }
                    catch (IOException ex1)
                    {
                        logger.Info("Trying to load obo from resource");

                        try
                        {
                            ONTOLOGY = LoadOntologyFromResource(IMS_OBO_URI);
                        }
                        catch (IOException e)
                        {
                            logger.Error("Failed to load any ontology", e);
                        }
                    }
                }
            }

            return ONTOLOGY;
        }

        public static void DownloadOBO(string oboLocation)
        {
            // 创建一个WebClient实例用于下载文件
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    // 检查响应头来处理重定向
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(oboLocation);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    // 如果状态码表示重定向，则获取新的URL
                    if (response.StatusCode == HttpStatusCode.MovedPermanently ||
                        response.StatusCode == HttpStatusCode.Found ||
                        response.StatusCode == HttpStatusCode.SeeOther ||
                        response.StatusCode == HttpStatusCode.TemporaryRedirect)
                    {
                        oboLocation = response.Headers["location"];
                        response.Close();
                        request = (HttpWebRequest)WebRequest.Create(oboLocation);
                        response = (HttpWebResponse)request.GetResponse();
                    }

                    // 从响应流中读取数据
                    using (Stream inStream = response.GetResponseStream())
                    {
                        // 从URL路径中提取文件名
                        string filename = oboLocation.Substring(oboLocation.LastIndexOf('/') + 1);

                        // 调用InstallOBO方法来处理输入流和文件名
                        InstallOBO(inStream, filename);
                    }
                }
                catch (WebException ex)
                {
                    // 捕获并处理可能的网络异常
                    throw new IOException("Error downloading obo file", ex);
                }
            }
        }

        public static string ONTOLOGIES_FOLDER = "Ontologies";
        private string url;
        private HTTPOBOLoader hTTPOBOLoader;
        private string resource;
        private ResourceOBOLoader resourceOBOLoader;
        private string file;
        private FileOBOLoader fileOBOLoader;

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
                using (FileStream outStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    // 从输入流读取数据并写入文件
                    while ((bytesRead = inStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        outStream.Write(buffer, 0, bytesRead);
                    }
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
            List<OBO> fullList = new List<OBO>();

            foreach (var importedOBO in imports)
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
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            OBOTerm term = terms.ContainsKey(id) ? terms[id] : null;

            if (term == null)
            {
                foreach (var parent in imports)
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

        public string GetPath()
        {
            return path;
        }

        public string GetDefaultNamespace()
        {
            return defaultNamespace;
        }

        public string GetOntology()
        {
            return ontologyIdentifier;
        }

        public string GetDataVersion()
        {
            return dataVersion;
        }

        public OBO GetOBOWithID(string id)
        {
            if (ontologyIdentifier.Equals(id, StringComparison.OrdinalIgnoreCase))
                return this;

            foreach (var importedOBO in imports)
            {
                var foundOBO = importedOBO.GetOBOWithID(id);

                if (foundOBO != null)
                    return foundOBO;
            }

            return null;
        }

        public override string ToString()
        {
            return path;
        }

        public static string GetNameFromID(string id)
        {
            switch (id.ToUpper())
            {
                case "IMS":
                    return IMS_OBO_FULLNAME;
                case "MS":
                    return MS_OBO_FULLNAME;
                case "UO":
                    return UO_OBO_FULLNAME;
                case "PATO":
                    return PATO_OBO_FULLNAME;
                default:
                    return id;
            }
        }
    }

}
