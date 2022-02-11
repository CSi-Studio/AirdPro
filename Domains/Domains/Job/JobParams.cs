/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;

namespace AirdPro.Domains.Convert
{
    public class JobParams
    {
        /**
         * Ignore the mz-intensity pairs whose intensity is zero.
         * 忽略intensity为0的数据
         */
        public Boolean ignoreZeroIntensity = true;
        /**
         * the decimal point of the mz. The default value is 0.0001
         * mz精度,默认保留到小数点后第4位
         */
        public Double mzPrecision = 0.0001;
        /**
         * if using the multi thread for acceleration. The default value is true
         * 是否使用CPU多核加速,默认加速
         */
        public Boolean threadAccelerate = true;

        /**
         * The extra suffix for every converted file's name
         * 额外的文件后缀名称
         */
        public String suffix;

        /**
         * The operator's name
         * 操作员姓名
         */
        public String creator;

        /**
         * The core compressor algorithm of Aird
         * 使用的Aird核心压缩算法
         * 1 : ZDPD, the first generation of aird compressor
         * 2 : ZDVB, the second generation of aird compressor
         * 3 : Stack ZDPD, the second generation of aird compressor
         */
        public int airdAlgorithm = 1;

        /**
         * The stack layers's tag
         * 2^digit = layer's count
         * eg. if the layer's size is 256, then the digit is 8
         */
        public int digit = 8;

        /**
         * If Store the PSI CV
         * 是否保存可控词汇表的相关信息
         */
        public Boolean includeCV = false;

        public JobParams()
        {
        }

        public String getAirdAlgorithmStr()
        {
            switch (airdAlgorithm)
            {
                case 1: return "ZDPD";
                case 2: return "ZDVB";
                case 3: return ("Stack-ZDPD:" + Math.Pow(2, digit) + " Layers");
                default: return "ZDPD";
            }
            // return airdAlgorithm == 1 ? "ZDPD" : ("Stack-ZDPD:" + Math.Pow(2, digit).ToString() + " Layers");
        }

        public Boolean useStackZDPD()
        {
            return airdAlgorithm == 2;
        }
    }
}
