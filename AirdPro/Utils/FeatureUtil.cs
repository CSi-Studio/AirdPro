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