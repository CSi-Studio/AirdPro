using log4net;
using System;
using System.Diagnostics;
using System.IO;

namespace AirdPro.csimzMLParser.obo
{
    public class ResourceOBOLoader : IOBOLoader
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(ResourceOBOLoader));

        public FileStream GetInputStream(string location)
        {            
            location = location.Substring(location.LastIndexOf('/') + 1);
            string resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\obo\");
            location = Path.Combine(resourcePath, location);

            try
            {                
                FileStream inStream = (FileStream)GetType().Assembly.GetManifestResourceStream(location);
                OBO.InstallOBO(inStream, location);
                inStream.Close();                
            }
            catch (IOException ex)
            {
                LOGGER.Error("Failed to extract obo for use later", ex);
            }

            return (FileStream)GetType().Assembly.GetManifestResourceStream(location);
        }
    }
}
