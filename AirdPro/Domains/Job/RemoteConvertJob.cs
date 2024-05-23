/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdPro.Storage.Config;
using AirdSDK.Enums;

namespace AirdPro.Domains
{
    public class RemoteConvertJob
    {
        public string remoteId;
        public string sourcePath;
        public string targetPath;
        public string? type = JobInfo.AutoType;
        public int? mzPrecision;
        public string creator;
        public string suffix;
        // public string? scene = Scene.Computation;
        public int? engine = (int)AirdEngine.RowCompression;
        public int? indexFormat = 0;
        public bool ignoreZeroIntensity = true;
        public bool? compressedIndex = false;
        public string? mzIntComp;
        public string? mzByteComp;
        public string? intIntComp;
        public string? intByteComp;
        public string? mobiIntComp;
        public string? mobiByteComp;
        public bool? autoDecision;
        public string consumeIP;
        public string consumeTime;

        public RemoteConvertJob()
        {
        }

        public RemoteConvertJob(string inputPath, string outputPath, string airdType, ConversionConfig config)
        {
            sourcePath = inputPath;
            targetPath = outputPath;
            type = airdType;
            mzPrecision = config.mzPrecision;
            creator = config.creator;
            suffix = config.suffix;
            engine = config.engine;
            indexFormat = config.indexFormat;
            ignoreZeroIntensity = config.ignoreZeroIntensity;
            compressedIndex = config.compressedIndex;
            autoDecision = config.autoDecision;
            mzIntComp = config.mzIntComp.ToString();
            mzByteComp = config.mzByteComp.ToString();
            intIntComp = config.intIntComp.ToString();
            intByteComp = config.intByteComp.ToString();
            mobiIntComp = config.mobiIntComp.ToString();
            mobiByteComp = config.mobiByteComp.ToString();
        }
    }
}