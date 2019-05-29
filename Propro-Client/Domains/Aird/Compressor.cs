using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propro_Client.Domains.Aird
{
    public class Compressor
    {
        //压缩对象,支持mz和intensity两种
        public string target;

        //压缩方法,使用分号隔开,目前支持PFor和Zlib两种
        public string method;

        //数值精度,具体指代精确到小数点后多少位
        public int precision;

    }
}
