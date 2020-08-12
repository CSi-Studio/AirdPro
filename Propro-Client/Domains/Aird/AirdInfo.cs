/*
 * Copyright (c) 2020 Propro Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;

namespace AirdPro.Domains.Aird
{
    public class AirdInfo
    {
        /**
         * Instrument information list
         */
        public List<Instrument> instruments;

        /**
         * dataProcessing information list
         */
        public List<DataProcessing> dataProcessings;

        /**
         * 处理的软件信息
         */
        public List<Software> softwares;

        /**
         * 处理前的文件信息
         */
        public List<ParentFile> parentFiles;

        /**
         * [核心字段]
         * 数组压缩策略
         */
        public List<Compressor> compressors;

        /**
        * [核心字段]
        * Store the window rangs which have been adjusted with experiment overlap
        * 存储SWATH窗口信息,窗口已经根据overlap进行过调整
        */
        public List<WindowRange> rangeList;

        /**
         * [核心字段]
         * 用于存储Block的索引（适用于PRM/DIA/ScanningSwath/DDA）
         * 当存储SWATH窗口信息,窗口已经根据overlap进行过调整,并且由于同一个SWATH窗口中所有的MS2的precursor都相同,因此本数组的长度恒为1
         */
        public List<BlockIndex> indexList;

        /**
         * [核心字段]
         * 实验类型,目前支持DIA_SWATH,PRM,DDA,SCANNING_SWATH和COMMON 5种
         */
        public string type;

        /**
         * 转换压缩后的aird二进制文件路径,默认读取同目录下的同名文件,如果不存在才去去读本字段对应的路径
         */
        public string airdPath;

        /**
         * 原始文件的文件大小,单位byte
         */
        public long fileSize;

        /**
         * 总计拥有的光谱数
         */
        public long totalScanCount;
        /**
         * 实验的创建者
         */
        public string creator;

        /**
         * 实验的创建日期
         */
        public DateTime createDate;

        /**
         * 特征键值对,详情见Features.cs
         */
        public string features;

        /**
         * 是否忽略intensity为0的点
         */
        public bool ignoreZeroIntensityPoint = true;

        public string version;

        public int versionCode;
    }
}
