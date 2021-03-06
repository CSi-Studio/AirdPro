﻿/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Domains.Aird
{
    /**
     * Path to all the ancestor files (up to the native acquisition file) used to generate the current Aird instance document.
     */
    public class ParentFile
    {
        /**
         * Name of the parent file
         */
        public string name;

        /**
         * Location of the parent file
         */
        public string location;

        /**
         * Was the parent file a native acquisition file? Or was it processed data?
         * enumeration: RAWData, processedData
         */
        public string fileType;

        /**
         * sha-1 sum of the parent file, the length must less than 40
         */
        public string fileSha1;

        /**
         * 文件格式
         */
        public string formatType;

    }
}
