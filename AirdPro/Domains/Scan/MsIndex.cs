﻿/*
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
    public class MsIndex
    {
        //ms level
        public int level;

        //filter string
        public string filterString;

        //retention time
        public double rt;

        //tic数值
        public long tic = 0;

        //base peak intensity
        public double basePeakIntensity;

        //base peak mz
        public double basePeakMz;

        //injection time
        public float injectionTime;

        public string activator;

        public float energy;

        public string polarity;

        public string msType;

        //对应的ms1的序号
        public int pNum;

        //序号
        public int num;

        public WindowRange precursor;

        //PSI CV PSI可控词汇表
        public List<CV> cvs;
    }
}