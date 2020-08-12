using System;

namespace AirdPro.Domains.Convert
{
    public class JobParams
    {
        //忽略intensity为0的数据
        public Boolean ignoreZeroIntensity = true;
        //对intensity是否求log10以降低精度
        public Boolean log2 = true;
        //mz精度,默认保留到小数点后第4位
        public Double mzPrecision = 0.0001;
        // 是否使用CPU多核加速,默认加速
        public Boolean threadAccelerate = true;
        //额外的文件后缀名称
        public String suffix;
        //操作员姓名
        public String creator;

        public JobParams()
        {
        }
    }
}
