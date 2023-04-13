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
using AirdSDK.Beans;

namespace AirdPro.Domains
{
    //包含mz,intensity数组
    public class TempScanChroma
    {
        public int num;
        public string id;
        public string activator;
        public float energy;
        public string polarity;
        public string msType;
        public WindowRange precursor;
        public WindowRange product;
        public List<CV> cvs;

        public byte[] rtArrayBytes;
        public byte[] intArrayBytes;

        public TempScanChroma()
        {
        }

        public TempScanChroma(int num, WindowRange precursor, WindowRange product, string activator, float energy,
            string polarity, string msType, List<CV> cvs)
        {
            this.num = num;
            this.precursor = precursor;
            this.product = product;
            this.activator = activator;
            this.energy = energy;
            this.polarity = polarity;
            this.msType = msType;
            this.cvs = cvs;
        }
    }
}