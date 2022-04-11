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

namespace AirdSDK.Compressor
{
    public class InitHuffmanCode
    {
        public InitHuffmanCode(List<int[]> MobiArrays)
        {
            //预处理传入数据，转换成整数数组
            List<int[]> mobiArrays = new List<int[]>(MobiArrays);
            List<int> mobiList = new List<int>();
            for (int i = 0; i < mobiArrays.Count; i++)
            {
                for (int j = 0; j < mobiArrays[i].Length; j++)
                {
                    mobiList.Add(mobiArrays[i][j]);
                }
            }

            //霍夫曼编码
            TreeList treeList = new TreeList(mobiList);
            for (int j = 0; j < mobiList.Count; j++)
            {
                treeList.AddNum(mobiList[j]);
            }

            treeList.SortTree();
            while (treeList.Length() > 1)
            {
                treeList.MergeTree();
            }

            TreeList.MakeKey(treeList.RemoveTree(), "");
            List<byte> resultByte = TreeList.Translate(mobiList);
            string[] signTable = treeList.GetSignTable();
            string[] keyTable = treeList.GetKeyTable();
            for (int i = 0; i < signTable.Length; i++)
            {
                Console.WriteLine(signTable[i] + ":" + keyTable[i]);
            }

            Console.WriteLine("the mobi converted by HuffmanCoding is " + resultByte.Count * 8 + " bits long. ");
            //霍夫曼解码
            List<int> decodedList = new List<int>();
            for (int k = 0; k < mobiList.Count; k++)
            {
                treeList.AddNum(mobiList[k]);
            }

            treeList.SortTree();
            while (treeList.Length() > 1)
            {
                treeList.MergeTree();
            }

            decodedList = treeList.readHuffmanCode(treeList.RemoveTree(), resultByte);
            for (int m = 0; m < decodedList.Count; m++)
            {
                Console.WriteLine("the decoded numbers are " + decodedList[m]);
            }
        }
    }
}