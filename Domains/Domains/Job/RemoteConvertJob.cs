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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Domains.Job
{
    public class RemoteConvertJob
    {
        public string sourcePath;
        public string targetPath;
        public string type;
        public double mzPrecision;
        //目前远程任务默认为第一代压缩算法
        public int airdAlgorithm = 1;
        public int digit = 8;

        public String getAirdAlgorithmStr()
        {
            return airdAlgorithm == 1 ? "ZDPD" : ("Stack-ZDPD:" + Math.Pow(2,digit).ToString() + " Layers");
        }
    }
}
