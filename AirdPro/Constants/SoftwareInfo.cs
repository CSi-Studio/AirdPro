/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
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
        public static string VERSION = "3.0.0";
        public static int VERSION_CODE = 301;
        public static string CLIENT_VERSION_DESCRIPTION = "1. PASEF Mode Support;\r\n" +
                                                          "2. Intensity Precision Prediction;\r\n" +
                                                          "3. Mapping For Mobility Compression:\r\n" +
                                                          "4. Combinable compressor for Byte and Integer Array:\r\n" +
                                                          "  For Byte: Brotli, Snappy, Zstd, Zlib;\r\n" +
                                                          "  For Integer:(Integrated) Variable Byte, (Integrated) Binary Packing\r\n";
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
