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
using AirdPro.Constants;
using AirdSDK.Enums;

namespace AirdPro.Storage.Config
{
    public class ConversionConfig : ICloneable
    {
        /**
         * 配置名称
         */
        public string configName;

        /**
         * Ignore the MZ-intensity pairs whose intensity is zero.
         * 忽略intensity为0的数据
         */
        public bool ignoreZeroIntensity = true;

        /**
         * the decimal point of the MZ. The default value is 5dp
         * mz精度,默认保留到小数点后第5位
         */
        public int mzPrecision = 100000;

        /**
         * The extra suffix for every converted file's name
         * 额外的文件后缀名称
         */
        public string suffix;

        /**
         * 是否压缩索引，是则会将IndexList的信息转化为JSON格式后使用Zstd进行压缩并存储与Aird文件中
         */
        public bool compressedIndex = false;

        /**
         * The operator's name
         * 操作员姓名
         */
        public string creator = Environment.UserName;

        /**
         * 是否使用动态参数决策
         */
        public bool autoDesicion = false;

        /**
         * 转换后的索引格式，支持输出JSON，Protobuf或者两者均输出的形式
         * 0. JSON
         * 1. Protobuf
         * 2. Both
         */
        public int indexFormat = 0;

        /**
         * 面向的场景,默认为Aird-ComboComp面向计算的场景，主要使用行存储的方式进行排列与压缩
         * Aird-Search为面向搜索的场景，主要使用列存储的方式进行存储与压缩
         */
        public int engine = (int)AirdEngine.RowCompression;
        
        /**
         * 用于mz压缩的int数组压缩方法
         */
        public SortedIntCompType mzIntComp = SortedIntCompType.IVB;

        /**
         * 用于mz压缩的byte数组压缩方法
         */
        public ByteCompType mzByteComp = ByteCompType.Zstd;

        /**
         * 用于intensity压缩的int数组压缩方法
         */
        public IntCompType intIntComp = IntCompType.VB;

        /**
         * 用于intensity压缩的byte数组压缩方法
         */
        public ByteCompType intByteComp = ByteCompType.Zstd;

        /**
         * 用于mobility压缩的int数组压缩方法
         */
        public IntCompType mobiIntComp = IntCompType.DZVB;

        /**
         * 用于mobility压缩的byte数组压缩方法
         */
        public ByteCompType mobiByteComp = ByteCompType.Zstd;

        /**
         * 用于rt压缩的int数组压缩方法
         */
        public SortedIntCompType rtIntComp = SortedIntCompType.IVB;

        /**
         * 用于rt压缩的byte数组压缩方法
         */
        public ByteCompType rtByteComp = ByteCompType.Zstd;

        /**
         * 决策器的权重，默认为1:1:1
         */
        public double compressionSizeWeight = 1;

        public double compressionTimeWeight = 1;
        public double decompressionTimeWeight = 1;
        public int spectraToPredict = 50;

        /**
         * Filter 字段
         */
        public bool noMS1 = false;
        public bool noMS2 = false;
        public double rtStart = 0;
        public double rtEnd = Double.MaxValue;

        public string GetMzPrecisionStr()
        {
            return ((int)Math.Log10(mzPrecision)) + "dp";
        }

   
        public bool ColumnCompression()
        {
            return engine == (int)AirdEngine.ColumnCompression;
        }

        public string EngineName()
        {
            switch (engine)
            {
                case (int)AirdEngine.RowCompression:
                    return "Row Compression";
                case (int)AirdEngine.ColumnCompression:
                    return "Column Compression";
                default:
                    return "Unknown";
            }
        }

        /**
         * 自由探索模式下自动组装所有压缩组合，面向计算场景下的函数
         */
        public List<ConversionConfig> BuildExplorerConfigs(bool mobility)
        {
            List<ConversionConfig> configList = new List<ConversionConfig>();

            Array sortIntCompTypes = Enum.GetValues(typeof(SortedIntCompType));
            Array intCompTypes = Enum.GetValues(typeof(IntCompType));
            Array byteCompTypes = Enum.GetValues(typeof(ByteCompType));

            SortedIntCompType mzIntComp;
            ByteCompType mzByteComp;
            IntCompType intIntComp;
            ByteCompType intByteComp;
            IntCompType mobiIntComp;
            ByteCompType mobiByteComp;

            foreach (SortedIntCompType mzIntCompType in sortIntCompTypes)
            {
                mzIntComp = mzIntCompType;
                foreach (ByteCompType mzByteCompType in byteCompTypes)
                {
                    mzByteComp = mzByteCompType;
                    foreach (IntCompType intIntCompType in intCompTypes)
                    {
                        intIntComp = intIntCompType;
                        foreach (ByteCompType intByteCompType in byteCompTypes)
                        {
                            intByteComp = intByteCompType;
                            if (mobility)
                            {
                                foreach (IntCompType mobiIntCompType in intCompTypes)
                                {
                                    mobiIntComp = mobiIntCompType;
                                    foreach (ByteCompType mobiByteCompType in byteCompTypes)
                                    {
                                        mobiByteComp = mobiByteCompType;
                                        ConversionConfig config = (ConversionConfig)Clone();
                                        config.mzIntComp = mzIntComp;
                                        config.mzByteComp = mzByteComp;
                                        config.intIntComp = intIntComp;
                                        config.intByteComp = intByteComp;
                                        config.mobiIntComp = mobiIntComp;
                                        config.mobiByteComp = mobiByteComp;
                                        config.suffix =
                                            Const.Dash + mzIntComp + Const.Dash + mzByteComp + Const.Dash + intIntComp +
                                            Const.Dash + intByteComp +
                                            Const.Dash + mobiIntComp + Const.Dash +
                                            mobiByteComp;
                                        configList.Add(config);
                                    }
                                }
                            }
                            else
                            {
                                ConversionConfig config = (ConversionConfig)Clone();
                                config.mzIntComp = mzIntComp;
                                config.mzByteComp = mzByteComp;
                                config.intIntComp = intIntComp;
                                config.intByteComp = intByteComp;
                                config.suffix = Const.Dash + mzIntComp + Const.Dash + mzByteComp + Const.Dash +
                                                intIntComp + Const.Dash +
                                                intByteComp;
                                configList.Add(config);
                            }
                        }
                    }
                }
            }

            return configList;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
       
    }
}