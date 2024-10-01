using AirdPro.ImzMLParser.obo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.obo
{
    public class ResourceOBOLoader : IOBOLoader
    {
        private static readonly TraceSource LOGGER = new TraceSource("ResourceOBOLoader");

        public Stream GetInputStream(string location)
        {
            string filename = location.Substring(location.LastIndexOf('/') + 1);

            try
            {
                Stream inStream = GetType().Assembly.GetManifestResourceStream("AirdPro.ImzMLParser.obo." + filename);

                if (inStream != null)
                {
                    // 假设OBO.installOBO是一个静态方法，你需要在这里调用它
                    // OBO.InstallOBO(inStream, filename);

                    // 注意：不要关闭流，因为它将被返回给调用者使用
                }
                else
                {
                    throw new FileNotFoundException("Resource not found", filename);
                }
            }
            catch (Exception ex)
            {
                LOGGER.TraceEvent(TraceEventType.Error, 0, "Failed to extract obo for use later", ex);
                throw; // 重新抛出异常，让调用者处理
            }

            // 这里需要重新打开资源流，因为上面的流已经被关闭了
            return GetType().Assembly.GetManifestResourceStream("AirdPro.ImzMLParser.obo." + filename);
        }
    }
}
