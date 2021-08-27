/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Collections.Generic;

namespace AirdPro.DomainsCore.Aird
{
    public class TempIndex
    {
        public int level;

        //retention time
        public float rt;

        //tic数值
        public long tic;

        //对应的ms1的序号
        public int pNum;

        //序号
        public int num;

        //前体的荷质比,precursor mz
        public double mz;

        //前体的荷质比窗口开始位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public double mzStart;

        //前体的荷质比窗口结束位置,已经经过ExperimentDO.overlap参数调整,precursor mz
        public double mzEnd;

        //前体的荷质比窗口
        public double wid;

        //PSI CV PSI可控词汇表
        public List<CV> cvList;

        //用于存储KV键值对
        public string features;
    }
}
