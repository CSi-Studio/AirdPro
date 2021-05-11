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
using System.Collections.Generic;
using pwiz.CLI.data;

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
         * The processed software information
         * 处理的软件信息
         */
        public List<Software> softwares;

        /**
         * The file information before converting
         * 处理前的文件信息
         */
        public List<ParentFile> parentFiles;

        /**
         * [Core Field]
         * The array compressor strategy
         * [核心字段]
         * 数组压缩策略
         */
        public List<Compressor> compressors;

        /**
         * [Core Field]
         * Store the window rangs which have been adjusted with experiment overlap
         * [核心字段]
         * 存储SWATH窗口信息,窗口已经根据overlap进行过调整
         */
        public List<WindowRange> rangeList;

        /**
         * [Core Field]
         * Used for block index(PRM/DIA/ScanningSWATH/DDA)
         * When using for storing SWATH window range information, the swath windows have already been adjusted by overlap between windows.
         * [核心字段]
         * 用于存储Block的索引（适用于PRM/DIA/ScanningSwath/DDA）
         * 当存储SWATH窗口信息,窗口已经根据overlap进行过调整
         */
        public List<BlockIndex> indexList;

        /**
         * [Core Field]
         * Experiment Type, Support for DIA/SWATH, PRM, DDA, SCANNING_SWATH, COMMON
         * [核心字段]
         * 实验类型,目前支持DIA_SWATH,PRM,DDA,SCANNING_SWATH和COMMON 5种
         */
        public string type;

        /**
         * the aird file path.
         * 转换压缩后的aird二进制文件路径,默认读取同目录下的同名文件,如果不存在才去去读本字段对应的路径
         */
        public string airdPath;

        /**
         * the vendor file size
         * 原始文件的文件大小,单位byte
         */
        public long fileSize;

        /**
         * the total spectrums count
         * 总计拥有的光谱数
         */
        public long totalScanCount;

        /**
         * the aird file creator
         * 实验的创建者
         */
        public string creator;

        /**
         * the create data
         * 实验的创建日期
         */
        public DateTime createDate;

        /**
         * the features. See Features.cs
         * 特征键值对,详情见Features.cs
         */
        public string features;

        /**
         * If ignore the point which intensity = 0
         * 是否忽略intensity为0的点
         */
        public bool ignoreZeroIntensityPoint = true;

        /**
         * Aird version
         * Aird的版本号
         */
        public string version;

        /**
         * Aird Code
         * Aird的版本编码
         */
        public int versionCode;

        /**
         * PSI CV
         */
        public List<CVParam> cvParams;

    }
}
