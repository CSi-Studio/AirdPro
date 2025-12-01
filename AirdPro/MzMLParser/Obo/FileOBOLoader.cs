using System;
using System.IO;

namespace AirdPro.csimzMLParser.obo
{
    public class FileOBOLoader : IOBOLoader
    {
        public FileStream GetInputStream(string location)
        {
            FileStream inputStream = null;

            location = location.Substring(location.LastIndexOf('/') + 1);
            string resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\obo\");
            location = Path.Combine(resourcePath, location);

            FileInfo fileInfo = new(location);            
            if (!fileInfo.Exists)
            {
                fileInfo = new FileInfo(Path.Combine("Ontologies", location));
            }      
            inputStream = fileInfo.OpenRead();

            return inputStream;
        }
    }
}
