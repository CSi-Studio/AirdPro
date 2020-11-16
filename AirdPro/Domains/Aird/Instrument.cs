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

namespace AirdPro.Domains.Aird
{
    /**
     * General information about the MS instrument.
     */
    public class Instrument
    {
        //设备仪器厂商
        public string manufacturer;

        //设备类型
        public string model;

        public string ionisation;

        public string resolution;
        
        public List<string> source = new List<string>();
        //分析方式
        public List<string> analyzer = new List<string>();
        //探测器
        public List<string> detector = new List<string>();

        //其他特征,使用K:V;K:V;K:V;类似的格式进行存储
        public string features;
    }
}
