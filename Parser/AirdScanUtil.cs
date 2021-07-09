using AirdPro.DomainsCore.Aird;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using AirdPro.Constants;

namespace AirdPro.Parser
{
    class AirdScanUtil
    {
        /**
        * load Aird Index Infomation from index file(JSON Format),and parse into AirdInfo
        * @param indexFile 索引文件
        * @return 该索引文件内的JSON信息,即AirdInfo信息
        */
        public static AirdInfo loadAirdInfo(string indexFile)
        {
            string content = File.ReadAllText(indexFile);
            AirdInfo airdInfo;
            try
            {
                airdInfo = JsonConvert.DeserializeObject<AirdInfo>(content);
            }
            catch (Exception e)
            {
                Console.WriteLine(indexFile);
                Console.WriteLine(ResultCodeEnum.NOT_AIRD_INDEX_FILE.Message);
                Console.WriteLine(e.Message);
                return null;
            }
            return airdInfo;
        }

    }
}
