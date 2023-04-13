/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Windows;

namespace AirdPro.Constants
{
    class SoftwareInfo
    {
        public static string VERSION = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static string CLIENT_VERSION_DESCRIPTION = "1. New GUI: Aird Files Preview;\r\n" +
                                                          "2. New GUI: XIC, Spectrum Quick look;\r\n" +
                                                          "3. New Func: Remote Repository Async Tool;\r\n" +
                                                          "4. New Func: Support Conversion for mzML and mzXML;\r\n" +
                                                          "5. New Func: Support Acquisition Method for SRM/MRM;\r\n" +
                                                          "6. New Func: Support Scene for Search;\r\n";

        public static string NAME = "AirdPro";

        public static string getVersion()
        {
            return "Version " + VERSION;
        }

        public static string getDescription()
        {
            return CLIENT_VERSION_DESCRIPTION;
        }
    }
}