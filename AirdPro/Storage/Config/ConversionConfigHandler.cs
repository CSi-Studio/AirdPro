using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirdPro.Domains.Convert;
using AirdPro.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AirdPro.Domains.Job;

public class ConversionConfigHandler:Subject
{
    private static string DEFAULT = "Default";
    private static string CONFIG_PATH = Path.Combine(Environment.CurrentDirectory, "ConversionConfig.json");
    public Dictionary<string, ConversionConfig> configMap = new Dictionary<string, ConversionConfig>();

    private HashSet<Observer<Dictionary<string, ConversionConfig>>> observers = new HashSet<Observer<Dictionary<string, ConversionConfig>>>();

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

    public void save()
    {
        if (!File.Exists(CONFIG_PATH))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CONFIG_PATH));
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string defaultConfigStr = JsonConvert.SerializeObject(configMap, jsonSetting);
            byte[] defaultConfigBytes = Encoding.Default.GetBytes(defaultConfigStr);
            using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.Create))
            {
                defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
            }
        }
        else
        {
            JsonSerializerSettings jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            string defaultConfigStr = JsonConvert.SerializeObject(configMap, jsonSetting);
            byte[] defaultConfigBytes = Encoding.Default.GetBytes(defaultConfigStr);
            using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.Truncate))
            {
                defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
            }
        }
    }

    public void read()
    {
        initConfig();
        this.configMap = JsonConvert.DeserializeObject<Dictionary<string, ConversionConfig>>(File.ReadAllText(CONFIG_PATH));
        notify();
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
            byte[] defaultConfigBytes = Encoding.Default.GetBytes(defaultConfigStr);
            using (FileStream defaultConfigStream = new FileStream(CONFIG_PATH, FileMode.OpenOrCreate))
            {
                defaultConfigStream.Write(defaultConfigBytes, 0, defaultConfigBytes.Length);
            }
        }
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