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
    public class Compressor
    {
        public static string TARGET_MZ = "mz";
        public static string TARGET_INTENSITY = "intensity";

        public static string METHOD_ZLIB = "zlib";
        public static string METHOD_PFOR = "pFor";
        public static string METHOD_LOG10 = "log2";

        //压缩对象,支持mz和intensity两种
        public string target;

        //压缩对象使用的压缩方法列表,按照顺序进行压缩
        public List<string> methods;

        //压缩精度, 有1000,10000和100000三种,代表精确到小数点后3,4,5位
        public int precision;

        public void addMethod(string method)
        {
            if (methods == null)
            {
                methods = new List<string>();
            }
            methods.Add(method);
        }
    }
}
