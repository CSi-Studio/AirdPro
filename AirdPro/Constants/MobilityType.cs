/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Constants
{
    public static class MobilityType
    {
        public const string NONE = "None";
        public const string TIMS = "TIMS"; //"1/k0", "Vs/cm^2", trapped ion mobility spectrometry
        public const string DTIMS = "DTIMS"; //"Drift time", "ms",drift tube
        public const string TWIMS = "TWIMS"; //"Drift time", "ms",traveling wave ion mobility spectrometry
        public const string FAIMS = "FAIMS"; //field asymmetric waveform ion mobility spectrometry
    }
}