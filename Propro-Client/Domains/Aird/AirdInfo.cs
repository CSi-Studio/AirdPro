﻿using Propro.Structs;
using Propro_Client.Domains.Aird;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Propro.Domains
{
    public class AirdInfo
    {
        /**
         * 仪器设备信息
         */
        public List<Instrument> instruments;
    
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
         * The whole new scan index for new format file
         * 为MS1和MS2的scan作的索引列表
         */
        public List<ScanIndex> scanIndexList;

        /**
         * [核心字段]
         * The swath window location(start and and) for new format file
         * 专门用于存储SWATH块的索引列表
         */
        public List<ScanIndex> swathIndexList;

        /**
         * [核心字段]
         * 实验类型,目前支持DIA_SWATH和PRM两种
         */
        public string type;

        /**
         * LITTLE_ENDIAN和BIG_ENDIAN两种
         */
        public string byteOrder = "LITTLE_ENDIAN";

        /**
         * 转换压缩后的aird二进制文件路径,默认读取同目录下的同名文件,如果不存在才去去读本字段对应的路径
         */
        public string airdPath;

        /**
         * 实验的描述
         */
        public string description;

        /**
         * 实验的创建者,本字段在被导入Propro Server时会被操作人覆盖
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
       
    }
}
