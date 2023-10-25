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
    public class ConversionConfigHandler : Subject<Dictionary<string, ConversionConfig>>
    {
        private static string DEFAULT = "Default";
        private static string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory, "ConversionConfig.json");
        public Dictionary<string, ConversionConfig> configMap = new Dictionary<string, ConversionConfig>();

        private HashSet<Observer<Dictionary<string, ConversionConfig>>> observers =
            new HashSet<Observer<Dictionary<string, ConversionConfig>>>();

        public ConversionConfigHandler()
        {
            read();
        }

        public void saveConfig(string name, ConversionConfig config)
        {
            if (this.configMap.ContainsKey(name))
            {
                this.configMap[name] = config;
            }
            else
            {
                this.configMap.Add(name, config);
            }

            save();
            notify();
        }

        public void removeConfig(string name)
        {
            this.configMap.Remove(name);
            save();
            notify();
        }

        public void removeConfig(List<string> nameList)
        {
            foreach (string name in nameList)
            {
                this.configMap.Remove(name);
            }

            save();
            notify();
        }

        public void save()
        {
            if (!File.Exists(CONFIG_PATH))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_PATH));
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                    { NullValueHandling = NullValueHandling.Ignore };
                string defaultConfigStr = JsonConvert.SerializeObject(configMap, jsonSetting);
                byte[] defaultConfigBytes = Encoding.UTF8.GetBytes(defaultConfigStr);
                using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.Create))
                {
                    defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
                }
            }
            else
            {
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                    { NullValueHandling = NullValueHandling.Ignore };
                string defaultConfigStr = JsonConvert.SerializeObject(configMap, jsonSetting);
                byte[] defaultConfigBytes = Encoding.UTF8.GetBytes(defaultConfigStr);
                using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.Truncate))
                {
                    defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
                }
            }
        }

        public Dictionary<string, ConversionConfig> read()
        {
            initConfig();
            this.configMap =
                JsonConvert.DeserializeObject<Dictionary<string, ConversionConfig>>(File.ReadAllText(CONFIG_PATH));
            notify();
            return configMap;
        }

        public void initConfig()
        {
            if (!File.Exists(CONFIG_PATH))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_PATH));
                configMap.Add(DEFAULT, new ConversionConfig());
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                    { NullValueHandling = NullValueHandling.Ignore };
                string defaultConfigStr = JsonConvert.SerializeObject(configMap, jsonSetting);
                byte[] defaultConfigBytes = Encoding.UTF8.GetBytes(defaultConfigStr);
                using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.OpenOrCreate))
                {
                    defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
                }
            }
        }

        public bool contains(Observer<Dictionary<string, ConversionConfig>> observer)
        {
            return observers.Contains(observer);
        }

        public void attach(Observer<Dictionary<string, ConversionConfig>> observer)
        {
            observers.Add(observer);
            notify();
        }

        public void detach(Observer<Dictionary<string, ConversionConfig>> observer)
        {
            observers.Remove(observer);
            notify();
        }

        public void notify()
        {
            foreach (Observer<Dictionary<string, ConversionConfig>> observer in observers)
            {
                observer.update(configMap);
            }
        }
    }
}