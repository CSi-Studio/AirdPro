using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace AirdPro.Forms.Config
{
    //声明参数（观察者模式）的委托
    public delegate void ConfigNotifyEventHandler(object sender);
    public class ConfigSubject
    {
      
        //创建并发布事件ConfigNotifyEvent
        public event ConfigNotifyEventHandler ConfigNotifyEvent;
        //保存Json文件流
        public FileStream customConfigStream;

        public ConfigCustomization.ConfigInfo configInfo;
        //实例化一个ConfigInfo类型的List用来存储从外部来的ConfigInfo数据
        List<ConfigCustomization.ConfigInfo> configLists = new List<ConfigCustomization.ConfigInfo>();
        public ConfigSubject(ConfigCustomization.ConfigInfo configInfo)
        {
            this.configInfo = configInfo;
            this.configLists.Add(configInfo);
            this.createAJson();
            publishConfigInfo();
        }
        public string ConfigName { get; set; }
        public string Creator { get; set; }
        public string ConfigMzPrecision { get; set; }
        public string ConfigCompressor { get; set; }
        public string ConfigOutputPath { get; set; }
        public ConfigSubject(string configName,string creator,string configMzPrecision,string configCompressor,string configOutputPath)
        {
            this.ConfigName = configName;
            this.Creator = creator;
            this.ConfigMzPrecision = configMzPrecision;
            this.ConfigCompressor = configCompressor;
            this.ConfigOutputPath = configOutputPath;
            publishConfigInfo();
        }

        public ConfigSubject()
        {

        }
        //遍历Lists中所有的ConfigInfo数据，并且以Json格式存储到本地
        public void createAJson()
        {
            foreach (ConfigCustomization.ConfigInfo configList in configLists)
            {
                string str = Environment.CurrentDirectory;
                string customConfigPath = Path.Combine(str, configInfo.configName + ".json");
                Directory.CreateDirectory(Path.GetDirectoryName(customConfigPath));
                JsonSerializerSettings jsonSetting = new JsonSerializerSettings
                { NullValueHandling = NullValueHandling.Ignore };
                String customConfigStr = JsonConvert.SerializeObject(configInfo, jsonSetting);
                byte[] customConfigBytes = Encoding.Default.GetBytes(customConfigStr);
                using (customConfigStream = new FileStream(customConfigPath, FileMode.Create))
                {
                    customConfigStream.Write(customConfigBytes, 0, customConfigBytes.Length);
                }
            }
        }
        //增加观察者
        public void addObserver(ConfigNotifyEventHandler observer)
        {
            ConfigNotifyEvent += observer;
        }
        //移除观察者的方法
        public void removeObserver(ConfigNotifyEventHandler observer)
        {
            ConfigNotifyEvent -= observer;
        }
        //向观察者发布更新configInfo的通知
        public void publishConfigInfo()
        {
            if (ConfigNotifyEvent != null)
            {
                ConfigNotifyEvent(this);
            }
        }
    }
}
