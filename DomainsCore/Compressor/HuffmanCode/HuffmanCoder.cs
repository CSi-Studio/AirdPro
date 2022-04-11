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

namespace AirdSDK.Compressor;

public class HuffmanCoder
{
    static int arrayNumCount;
    public static int[] toIntArray(List<int[]> mobiList)
    {
        for (int i = 0; i < mobiList.Count; i++)
        {
            arrayNumCount += mobiList[i].Length;
        }
        List<int> tmpList = new List<int>();
        for (int j = 0; j < mobiList.Count; j++)
        {
            for (int k = 0; k < mobiList[j].Length; k++)
            {
                tmpList.Add(mobiList[j][k]);
            }
        }
        int[] mobiArray = new int[arrayNumCount];
        for (int l = 0; l < arrayNumCount; l++)
        {
            mobiArray[l] = tmpList[l];
        }
        return mobiArray;
    }

    static HuffmanTree codeTree = null;
    public static HuffmanTree buildTree(int[] mobiList)
    {
        TreeList treeList = new TreeList(mobiList);
        for (int k = 0; k < mobiList.Length; k++)
        {
            treeList.addNum(mobiList[k]);
        }
        treeList.sortTree();
        while (treeList.length() > 1)
        {
            codeTree = treeList.mergeTree();
        }
        return codeTree;
    }

    public static byte[] encode(int[] target, HuffmanTree tree)
    {
        TreeList.makeKey(tree, "");
        List<byte> resultByte = TreeList.translate(target);
        byte[] tmpByte = new byte[resultByte.Count];
        for (int i = 0; i < resultByte.Count; i++)
        {
            tmpByte[i] = resultByte[i];
        }
        return tmpByte;
    }

    public static int[] decode(byte[] target, HuffmanTree tree)
    {
        int[] decodeInt = new int[arrayNumCount];
        decodeInt = TreeList.readHuffmanCode(arrayNumCount, target, tree);
        return decodeInt;
    }
}