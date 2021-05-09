using System;
using System.Collections;
using System.Collections.Generic;
using AirdPro.Domains.Aird;
using AirdPro.Utils;
using CSharpFastPFOR.Port;
using Xunit;

namespace AirdPro.Test
{
    public class StackZDPDTest
    {
        public static void stackZDPD_Test1()
        {
            List<int[]> arrGroup = new List<int[]>();
            for (int j = 0; j < 256; j++)
            {
                int[] mz = new int[1000 + (int) (new Random().NextDouble() * 100)];
                mz[0] = 10000;
                for (int i = 1; i < mz.Length; i++)
                {
                    mz[i] = mz[i - 1] + (int)(new Random().NextDouble() * 1000);
                }
                arrGroup.Add(mz);
            }
            Layers layers = StackCompressUtil.stackEncode(arrGroup, false);
            List<int[]> decompressArrGroup = StackCompressUtil.stackDecode(layers);
            Boolean pass = true;
            for (int i = 0; i < arrGroup.Count; i++)
            {
                pass = pass && Arrays.equals(arrGroup[i], decompressArrGroup[i]);
                if (!pass)
                {
                    break;
                }
            }
            Console.WriteLine(pass);
        }
    }
}