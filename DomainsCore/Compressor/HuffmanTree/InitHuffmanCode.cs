using System;
using System.Collections.Generic;
using System.Text;
using AirdPro.Converters.HuffmanTree;

namespace AirdSDK.Compressor.HuffmanTree
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
                    int[] array = new int[mobiArrays[i].Length];
                    array = mobiArrays[i];
                    mobiList.Add(array[j]);
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
            String[] signTable = treeList.GetSignTable();
            String[] keyTable = treeList.GetKeyTable();
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
