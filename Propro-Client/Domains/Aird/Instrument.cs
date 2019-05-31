using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro.Domains
{
    //仪器设备与信号信息
    public class Instrument
    {
        //设备仪器厂商
        public string manufacturer;
        //设备类型
        public string model;
        
        public List<string> source = new List<string>();
        //分析方式
        public List<string> analyzer = new List<string>();
        //探测器
        public List<string> detector = new List<string>();

        //其他特征,使用K:V;K:V;K:V;类似的格式进行存储
        public string features;
    }
}
