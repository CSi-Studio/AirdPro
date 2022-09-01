using System.Collections.Generic;
using System;

namespace AirdPro.Utils;

public class ArrayUtil
{
    public static double[] toPrimitive(List<double> list)
    {
        double[] array = new double[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            array[i] = list[i];
        }

        return array;
    }
}