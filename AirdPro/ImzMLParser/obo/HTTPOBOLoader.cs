using log4net;
using System;
using System.IO;
using System.Net.Http;

namespace AirdPro.ImzMLParser.obo
{
    public class HTTPOBOLoader : IOBOLoader
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(HTTPOBOLoader));

        private readonly HttpClient _httpClient;

        public HTTPOBOLoader()
        {
            _httpClient = new HttpClient();
        }

        public Stream GetInputStream(string location)
        {
            try
            {
                OBO.DownloadOBO(location);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to download obo for use later", ex);
            }

            try
            {
                // 使用HttpClient获取输入流
                var response = _httpClient.GetAsync(location).Result; // .Result 会阻塞直到获取结果，也可以使用异步方式
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStreamAsync().Result;
            }
            catch (Exception ex)
            {
                logger.Error("Failed to open the obo file stream", ex);
                throw;
            }
        }
    }

}
