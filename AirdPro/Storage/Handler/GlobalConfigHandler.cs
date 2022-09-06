/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirdPro.Storage.Config;
using Newtonsoft.Json;

namespace AirdPro.Storage.Handler
{
    public class GlobalConfigHandler : Subject<GlobalConfig>
    {
        private static string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory, "GlobalConfig.json");
        public GlobalConfig config = new();

        public HashSet<Observer<GlobalConfig>> observers = new HashSet<Observer<GlobalConfig>>();

        public GlobalConfigHandler()
        {
            read();
        }

        public string getRedisConnectStr()
        {
            return config.redisHost + ":" + config.redisPort;
        }

        public GlobalConfig read()
        {
            initConfig();
            this.config = JsonConvert.DeserializeObject<GlobalConfig>(File.ReadAllText(CONFIG_PATH));
            notify();
            return config;
        }

        public void saveConfig(GlobalConfig config)
        {
            this.config = config;
            save();
            notify();
        }

        public void initConfig()
        {
            if (!File.Exists(CONFIG_PATH))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_PATH));
                config = new GlobalConfig();
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                    {NullValueHandling = NullValueHandling.Ignore};
                string defaultConfigStr = JsonConvert.SerializeObject(config, jsonSetting);
                byte[] defaultConfigBytes = Encoding.UTF8.GetBytes(defaultConfigStr);
                using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.OpenOrCreate))
                {
                    defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
                }
            }
        }

        public void save()
        {
            if (!File.Exists(CONFIG_PATH))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_PATH));
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                    {NullValueHandling = NullValueHandling.Ignore};
                string defaultConfigStr = JsonConvert.SerializeObject(config, jsonSetting);
                byte[] defaultConfigBytes = Encoding.UTF8.GetBytes(defaultConfigStr);
                using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.Create))
                {
                    defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
                }
            }
            else
            {
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                    {NullValueHandling = NullValueHandling.Ignore};
                string defaultConfigStr = JsonConvert.SerializeObject(config, jsonSetting);
                byte[] defaultConfigBytes = Encoding.UTF8.GetBytes(defaultConfigStr);
                using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.Truncate))
                {
                    defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
                }
            }
        }

        public void attach(Observer<GlobalConfig> observer)
        {
            observers.Add(observer);
            notify();
        }

        public void detach(Observer<GlobalConfig> observer)
        {
            observers.Remove(observer);
            notify();
        }

        public void notify()
        {
            foreach (Observer<GlobalConfig> observer in observers)
            {
                observer.update(config);
            }
        }
    }
}