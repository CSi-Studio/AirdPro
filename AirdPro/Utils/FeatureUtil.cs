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
using AirdPro.Constants;
using AirdPro.Domains.Common;

namespace AirdPro.Utils;

public class FeatureUtil
{
    public static List<StrKV> parse(string pairs)
    {
        if (pairs == null || pairs.Equals(String.Empty))
        {
            return null;
        }

        List<StrKV> kvList = new List<StrKV>();
        string[] kvArrays = pairs.Split(Const.DIV.ToCharArray());
        for (var i = 0; i < kvArrays.Length; i++)
        {
            string kvStr = kvArrays[i];
            string[] kvArray = kvStr.Split(Const.COLON.ToCharArray());
            if (kvArray.Length == 2)
            {
                kvList.Add(new StrKV(kvArray[0], kvArray[1]));
            }
        }

        return kvList;
    }
}