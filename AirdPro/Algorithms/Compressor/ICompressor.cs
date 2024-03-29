﻿/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections.Generic;
using AirdPro.Converters;
using AirdPro.Domains;
using AirdSDK.Beans;
using AirdSDK.Compressor;
using pwiz.CLI.msdata;

namespace AirdPro.Algorithms
{
    public abstract class ICompressor
    {
        public bool multiThread = true;
        public int mzPrecision = 100000;
        public bool ignoreZero = true;
        public bool isCentroid = false;
        public bool includeCV = true;
        public int digit = 8;

        public SortedIntComp mzIntComp;
        public ByteComp mzByteComp;
        public IntComp intIntComp;
        public ByteComp intByteComp;
        public IntComp mobiIntComp;
        public ByteComp mobiByteComp;

        public SortedIntComp rtIntComp4Chroma;
        public ByteComp rtByteComp4Chroma;
        public IntComp intIntComp4Chroma;
        public ByteComp intByteComp4Chroma;

        public Dictionary<double, int> mobiDict;
        public int intensityPrecision;

        public ICompressor(Converter converter)
        {
            this.multiThread = converter.jobInfo.config.threadAccelerate;
            this.mzPrecision = converter.jobInfo.config.mzPrecision;
            this.ignoreZero = converter.jobInfo.config.ignoreZeroIntensity;
            this.isCentroid = converter.jobInfo.config.centroid;
            this.includeCV = true;
            this.digit = converter.jobInfo.config.digit;
        }

        /**
         * 由于色谱图数据量小, 使用固定IBP+Zstd的固定组合压缩器进行压缩
         */
        public void initForChromatogram()
        {
            rtIntComp4Chroma = new IntegratedVarByteWrapper();
            rtByteComp4Chroma = new ZstdWrapper();
            intIntComp4Chroma = new VarByteWrapper();
            intByteComp4Chroma = new ZstdWrapper();
            // rtIntComp4Chroma = new SortIntEmpty();
            // rtByteComp4Chroma = new ZlibWrapper();
            // intIntComp4Chroma = new Empty();
            // intByteComp4Chroma = new ZlibWrapper();
        }

        public abstract void compressMS1(Converter converter, BlockIndex index);

        public abstract void compressMS2(Converter converter, List<MsIndex> ms2List, BlockIndex index);

        public abstract void compress(Spectrum spectrum, TempScan ts);

        public abstract void compressMobility(Spectrum spectrum, TempScan ts);

        public abstract void compress(Chromatogram chromatogram, TempScanChroma ts);
    }
}