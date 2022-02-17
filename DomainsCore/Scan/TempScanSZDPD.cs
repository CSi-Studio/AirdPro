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

namespace AirdPro.DomainsCore.Aird
{
    public class TempScanSZDPD:IComparable
    {
        public List<int> nums;
        public List<float> rts;
        public List<long> tics;
        public List<List<CV>> cvs;
        public byte[] mzArrayBytes;
        public byte[] intArrayBytes;
        public byte[] tagArrayBytes;

        public TempScanSZDPD(List<int> nums, List<float> rts, List<long> tics, List<List<CV>> cvs)
        {
            this.nums = nums;
            this.rts = rts;
            this.tics = tics;
            this.cvs = cvs;
        }

        public TempScanSZDPD(List<int> nums, List<float> rts, byte[] mzArrayBytes, byte[] intArrayBytes, byte[] tagArrayBytes)
        {
            this.nums = nums;
            this.rts = rts;
            this.mzArrayBytes = mzArrayBytes;
            this.intArrayBytes = intArrayBytes;
            this.tagArrayBytes = tagArrayBytes;
        }

        public int CompareTo(object obj)
        {
            TempScanSZDPD ts = (TempScanSZDPD) obj;
            return ts.rts[0].CompareTo(this.rts[0]);
        }
    }
}
