using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Forms.Config
{
    //具体的被观察者类
    public class ConfigParamsSubject : ConfigSubject
    {
        public ConfigParamsSubject(ConfigCustomization.ConfigInfo configInfo)
            : base(configInfo)
        {

        }
        public ConfigParamsSubject(string configName, string creator, string configMzPrecision, string configCompressor, string configOutputPath)
            : base (configName, creator, configMzPrecision, configCompressor, configOutputPath)
        {

        }
        public ConfigParamsSubject()
            :base()
        {

        }
    }
}
