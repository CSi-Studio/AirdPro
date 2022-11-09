/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Constants
{
    public class UrlConst
    {
        public static string pxListUrl = "http://proteomecentral.proteomexchange.org/cgi/GetDataset?action=search&filterstr=";

        public static string pxDetailUrl = "http://proteomecentral.proteomexchange.org/cgi/GetDataset?ID=";

        public static string pxDetailJsonUrl = "http://proteomecentral.proteomexchange.org/cgi/GetDataset?outputMode=JSON&test=no&ID=";

        public static string mlListUrl = "https://www.ebi.ac.uk/metabolights/ws/studies";

        public static string mlDetailUrl = "https://www.ebi.ac.uk/metabolights/";

        public static string mlFtpUrl = "ftp://ftp.ebi.ac.uk/pub/databases/metabolights/studies/public/";
    }
}