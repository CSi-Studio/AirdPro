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
    class SoftwareInfo
    {
        public static string VERSION = "4.0.1";
        public static int VERSION_CODE = 402;

        public static string CLIENT_VERSION_DESCRIPTION = "1. New GUI: Aird Files Preview;\r\n" +
                                                          "2. New GUI: XIC, Spectrum Quick look;"+
                                                          "3. New Func: Remote Repository Async Tool;"
                                                          
                                                          ;

        public static string NAME = "AirdPro";

        public static string getVersion()
        {
            return "Version " + SoftwareInfo.VERSION;
        }

        public static string getDescription()
        {
            return CLIENT_VERSION_DESCRIPTION;
        }
    }
}