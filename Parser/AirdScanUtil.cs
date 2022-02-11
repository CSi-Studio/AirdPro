/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

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
