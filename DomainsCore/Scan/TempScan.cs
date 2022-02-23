﻿/*
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
    //包含mz,intensity数组
    public class TempScan:IComparable
    {
        public int num;
        public float rt;
        public float ccs;
        public long tic;
        public List<CV> cvs;

        public byte[] mzArrayBytes;
        public byte[] intArrayBytes;

        public TempScan(int num, float rt, long tic, List<CV> cvs)
        {
            this.num = num;
            this.rt = rt;
            this.tic = tic;
            this.cvs = cvs;
        }

        public TempScan(int num, float rt, byte[] mzArrayBytes, byte[] intArrayBytes)
        {
            this.num = num;
            this.rt = rt;
            this.mzArrayBytes = mzArrayBytes;
            this.intArrayBytes = intArrayBytes;
        }

        public int CompareTo(object obj)
        {
            TempScan ts = (TempScan) obj;
            return ts.rt.CompareTo(this.rt);
        }
    }
}
