/*
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
        public bool MultiThread = true;
        public int MzPrecision = 100000;
        public bool IgnoreZero = true;
        public bool IsCentroid = false;
        public int Digit = 8;

        public SortedIntComp MzIntComp;
        public ByteComp MzByteComp;
        public IntComp IntIntComp;
        public ByteComp IntByteComp;
        public IntComp MobiIntComp;
        public ByteComp MobiByteComp;

        public SortedIntComp RtIntComp4Chroma;
        public ByteComp RtByteComp4Chroma;
        public IntComp IntIntComp4Chroma;
        public ByteComp IntByteComp4Chroma;

        public Dictionary<double, int> MobiDict;
        public int IntensityPrecision;

        public ICompressor(Converter converter)
        {
            MultiThread = converter.JobInfo.config.threadAccelerate;
            MzPrecision = converter.JobInfo.config.mzPrecision;
            IgnoreZero = converter.JobInfo.config.ignoreZeroIntensity;
            IsCentroid = converter.JobInfo.config.centroid;
            Digit = converter.JobInfo.config.digit;
        }

        /**
         * 由于色谱图数据量小, 使用固定IBP+Zstd的固定组合压缩器进行压缩
         */
        public void InitForChromatogram()
        {
            RtIntComp4Chroma = new IntegratedVarByteWrapper();
            RtByteComp4Chroma = new ZstdWrapper();
            IntIntComp4Chroma = new VarByteWrapper();
            IntByteComp4Chroma = new ZstdWrapper();
        }

        public abstract void CompressMs1(PwizConverter converter, BlockIndex index);

        public abstract void CompressMs2(PwizConverter converter, List<MsIndex> ms2List, BlockIndex index);

        public abstract void Compress(Spectrum spectrum, TempScan ts);

        public abstract void CompressMobility(Spectrum spectrum, TempScan ts);

        public abstract void Compress(Chromatogram chromatogram, TempScanChroma ts);
    }
}