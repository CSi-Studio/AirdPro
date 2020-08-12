﻿/*
 * Copyright (c) 2020 Propro Studio
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
        public static string VERSION = "1.0.0";
        public static int VERSION_CODE = 1;
        public static string CLIENT_VERSION_DESCRIPTION = "正式命名为AirdPro,合并SWATHIndex与BlockIndex为BlockIndex";
        public static string NAME = "AirdPro";

        public static string getVersion()
        {
            return "AirdPro V" + SoftwareInfo.VERSION + " (Aird Version Code:" + SoftwareInfo.VERSION_CODE + ")";
        }

        public static string getDescription()
        {
            return CLIENT_VERSION_DESCRIPTION;
        }
    }
}
