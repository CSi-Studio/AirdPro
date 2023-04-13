/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdPro.Domains.View
{
    public class SpectrumRow
    {
        public int Scan { get; set; }
        public int? ParentScan { get; set; }
        public string Polarity { get; set; }
        public float? Energy { get; set; }
        public string Activator { get; set; }
        public string ScanType { get; set; }
        public int MSn { get; set; }
        public double RT { get; set; }
        public string Precursor { get; set; }
        public long TotalIons { get; set; }
        public double BasePeakMz { get; set; }
        public double BasePeakIntensity { get; set; }
        public string FilterString { get; set; }
    }
}