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
    static class SoftwareInfo
    {
        public static string VERSION = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string CLIENT_VERSION_DESCRIPTION = "1. Completely solve the problem of memory leaks\r\n" +
                                                          "2. Increase the conversion speed of columns\r\n" +
                                                          "3. Distributed task processing system\r\n" +
                                                          "4. Support for Mixed Polarity Filter\r\n"
                                                          ;

        public static string NAME = "AirdPro";

        public static string GetVersion()
        {
            return "version " + VERSION;
        }

        public static string GetDescription()
        {
            return CLIENT_VERSION_DESCRIPTION;
        }
    }
}