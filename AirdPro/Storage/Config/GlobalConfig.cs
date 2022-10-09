/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections;

namespace AirdPro.Storage.Config
{
    public class GlobalConfig
    {
        public string defaultOpenPath;
        public string redisHost;
        public string redisPort;
        public int maxTasks;

        public GlobalConfig()
        {
            this.redisHost = "127.0.0.1";
            this.redisPort = "6379";
            this.maxTasks = 1;
        }

        public GlobalConfig(string defaultOpenPath, string redisHost, string redisPort, int maxTasks)
        {
            this.defaultOpenPath = defaultOpenPath;
            this.redisHost = redisHost;
            this.redisPort = redisPort;
            this.maxTasks = maxTasks;
        }
    }
}