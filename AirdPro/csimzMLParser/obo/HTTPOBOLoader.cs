using log4net;
using System;
using System.IO;
using System.Net.Http;

namespace AirdPro.csimzMLParser.obo
{
    public class HTTPOBOLoader : IOBOLoader
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(HTTPOBOLoader));

        private readonly HttpClient httpClient;

        public HTTPOBOLoader()
        {
            httpClient = new HttpClient();
        }

        public Stream GetInputStream(string location)
        {
            try
            {
                OBO.DownloadOBO(location);
            }
            catch (Exception ex)
            {
                LOGGER.Error("Failed to download obo for use later", ex);
            }

            try
            {
                // 使用HttpClient获取输入流
                var response = httpClient.GetAsync(location).Result; 
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStreamAsync().Result;
            }
            catch (Exception ex)
            {
                LOGGER.Error("Failed to open the obo file stream", ex);
                throw;
            }
        }
    }

}
