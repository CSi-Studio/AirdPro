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
using AirdPro.Constants;

namespace AirdPro.Domains.Convert
{
    public class ConversionConfig
    {
        /**
         * Ignore the mz-intensity pairs whose intensity is zero.
         * 忽略intensity为0的数据
         */
        public bool ignoreZeroIntensity = true;

        /**
         * the decimal point of the mz. The default value is 0.00001
         * mz精度,默认保留到小数点后第5位
         */
        public int mzPrecision = 100000;

        /**
         * if using the multi thread for acceleration. The default value is true
         * 是否使用CPU多核加速,默认加速
         */
        public bool threadAccelerate = true;

        /**
         * The extra suffix for every converted file's name
         * 额外的文件后缀名称
         */
        public string suffix;

        /**
         * The operator's name
         * 操作员姓名
         */
        public string creator = "Admin";

        /**
         * 文件的输出路径
         */
        public string outputPath;

        /**
         * 用于mz压缩的int数组压缩方法
         */
        public IntCompType mzIntComp = IntCompType.IBP;

        /**
         * 用于mz压缩的byte数组压缩方法
         */
        public ByteCompType mzByteComp = ByteCompType.Zlib;

        /**
         * 用于intensity压缩的byte数组压缩方法
         */
        public ByteCompType intByteComp = ByteCompType.Zlib;

        /**
         * 是否使用stack layer压缩
         */
        public bool stack = false;

        /**
         * The stack layers's tag
         * 2^digit = layer's count
         * eg. if the layer's size is 256, then the digit is 8
         */
        public int digit = 8;

        public ConversionConfig()
        {
        }

        public String getCompressorStr()
        {
            return mzIntComp + "|" + mzByteComp + "|" + intByteComp;
        }
    }
}