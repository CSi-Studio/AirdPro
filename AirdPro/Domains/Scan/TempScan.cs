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
using AirdSDK.Beans;

namespace AirdPro.Domains
{
    //包含mz,intensity数组
    public class TempScan : IComparable
    {
        public int num;
        public double rt;
        public long tic;
        public float injectionTime;
        public double basePeakIntensity;
        public double basePeakMz;
        public List<CV> cvs;
        public string activator;
        public float energy;
        public string polarity;
        public string msType;
        public string filterString;

        public byte[] mzArrayBytes;
        public byte[] intArrayBytes;
        public byte[] mobilityArrayBytes;

        public TempScan(int num, double rt, long tic, double basePeakIntensity, double basePeakMz, float injectionTime,
            List<CV> cvs)
        {
            this.num = num;
            this.rt = rt;
            this.tic = tic;
            this.injectionTime = injectionTime;
            this.basePeakIntensity = basePeakIntensity;
            this.basePeakMz = basePeakMz;
            this.cvs = cvs;
        }

        public TempScan(MsIndex msIndex)
        {
            this.num = msIndex.num;
            this.rt = msIndex.rt;
            this.tic = msIndex.tic;
            this.injectionTime = msIndex.injectionTime;
            this.basePeakIntensity = msIndex.basePeakIntensity;
            this.basePeakMz = msIndex.basePeakMz;
            this.activator = msIndex.activator;
            this.energy = msIndex.energy;
            this.polarity = msIndex.polarity;
            this.msType = msIndex.msType;
            this.filterString = msIndex.filterString;
            this.cvs = msIndex.cvs;
        }

        public int CompareTo(object obj)
        {
            TempScan ts = (TempScan)obj;
            return ts.rt.CompareTo(this.rt);
        }
    }
}