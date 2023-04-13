/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Domains
{
    public class CompressStat
    {
        public string key;
        public long size;
        public long compressTime;
        public long decompressTime;
    
        public CompressStat(string key, long size, long compressTime, long decompressTime)
        {
            this.key = key;
            this.size = size;
            this.compressTime = compressTime;
            this.decompressTime = decompressTime;
        }
    }
}

