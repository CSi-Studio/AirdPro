using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.obo
{
    public class FileOBOLoader : IOBOLoader
    {
        public Stream GetInputStream(string location)
        {
            // Look in the current folder for the file, if not, try the Ontologies folder
            string filename = location.Substring(location.LastIndexOf('/') + 1);
            Stream inputStream = null;

            FileInfo file = new FileInfo(filename);

            if (file.Exists)
            {
                inputStream = file.OpenRead();
            }
            else
            {
                file = new FileInfo(Path.Combine("Ontologies", location));

                // If this fails, then we want an IOException to be thrown so that
                inputStream = file.OpenRead();
            }

            return inputStream;
        }
    }
}
