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

namespace AirdPro.Domains.Convert
{
    public class JobParams
    {
        //忽略intensity为0的数据
        public Boolean ignoreZeroIntensity = true;
        //对intensity是否求log10以降低精度
        public Boolean log2 = true;
        //mz精度,默认保留到小数点后第4位
        public Double mzPrecision = 0.0001;
        // 是否使用CPU多核加速,默认加速
        public Boolean threadAccelerate = true;
        //额外的文件后缀名称
        public String suffix;
        //操作员姓名
        public String creator;

        public JobParams()
        {
        }
    }
}
