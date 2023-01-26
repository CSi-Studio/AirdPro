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
        public bool multiThread = true;
        public int mzPrecision = 100000;
        public bool ignoreZero = true;
        public bool includeCV = true;
        public int digit = 8;

        public SortedIntComp mzIntComp;
        public ByteComp mzByteComp;
        public IntComp intIntComp;
        public ByteComp intByteComp;
        public IntComp mobiIntComp;
        public ByteComp mobiByteComp;
        public Dictionary<double, int> mobiDict;
        public int intensityPrecision;

        public ICompressor(Converter converter)
        {
            this.multiThread = converter.jobInfo.config.threadAccelerate;
            this.mzPrecision = converter.jobInfo.config.mzPrecision;
            this.ignoreZero = converter.jobInfo.config.ignoreZeroIntensity;
            this.includeCV = true;
            this.digit = converter.jobInfo.config.digit;
        }

        public abstract void compressMS1(Converter converter, BlockIndex index);

        public abstract void compressMS2(Converter converter, List<MsIndex> ms2List, BlockIndex index);

        public abstract void compress(Chromatogram chromatogram, TempScanChroma ts);

        public abstract void compress(Spectrum spectrum, TempScan ts);

        public abstract void compressMobility(Spectrum spectrum, TempScan ts);
    }
}