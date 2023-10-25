/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdSDK.Enums;

namespace AirdPro.Domains
{
    public class RemoteConvertJob
    {
        public string sourcePath;
        public string targetPath;
        public string? type = JobInfo.AutoType;
        public int? mzPrecision;
        public string creator;
        public string suffix;
        public string? scene = Scene.Computation;
        public bool? centroid = false;
        public bool ignoreZeroIntensity = true;
        public bool? compressedIndex = false;
    }
}