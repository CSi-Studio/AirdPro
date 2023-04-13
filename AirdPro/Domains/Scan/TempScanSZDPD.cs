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
using AirdSDK.Beans;

namespace AirdPro.Domains
{
    public class TempScanSZDPD : IComparable
    {
        public List<int> nums;
        public List<double> rts;
        public List<long> tics;
        public List<float> injectionTimes;
        public List<double> basePeakIntensities;
        public List<double> basePeakMzs;
        public List<string> activators;
        public List<float> energies;
        public List<string> polarities;
        public List<string> msTypes;
        public List<string> filterStrings;
        public List<List<CV>> cvs;
        public byte[] mzArrayBytes;
        public byte[] intArrayBytes;
        public byte[] tagArrayBytes;

        public TempScanSZDPD(List<int> nums, List<double> rts, List<long> tics, List<double> basePeakIntensities,
            List<double> basePeakMzs, List<float> injectionTimes,
            List<string> filterStrings, List<string> polarities, List<float> energies, List<string> activators,
            List<string> msTypes,
            List<List<CV>> cvs)
        {
            this.nums = nums;
            this.rts = rts;
            this.tics = tics;
            this.basePeakIntensities = basePeakIntensities;
            this.basePeakMzs = basePeakMzs;
            this.injectionTimes = injectionTimes;
            this.filterStrings = filterStrings;
            this.polarities = polarities;
            this.activators = activators;
            this.energies = energies;
            this.msTypes = msTypes;
            this.cvs = cvs;
        }

        public int CompareTo(object obj)
        {
            TempScanSZDPD ts = (TempScanSZDPD)obj;
            return ts.rts[0].CompareTo(this.rts[0]);
        }
    }
}