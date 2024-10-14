using System;
using System.IO;

namespace AirdPro.csimzMLParser.obo
{
    public class FileOBOLoader : IOBOLoader
    {
        public FileStream GetFileStream(string location)
        {
            location = location.Substring(location.LastIndexOf('/') + 1);
            string resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\obo\");
            location = Path.Combine(resourcePath, location);
            FileInfo file = new(location);

            if (!file.Exists)
            {               
                file = new FileInfo(Path.Combine("Ontologies", location));                
            }

            FileStream fileStream = file.OpenRead();
            return fileStream;
        }
    }
}
