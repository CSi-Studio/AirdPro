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
    public class TreeList
    {
        private int count = 0;
        private Node first = null;
        private static string[] signTable = null;
        private static string[] keyTable = null;

        public TreeList(List<int> list)
        {
            List<int> tmpList = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (!tmpList.Contains(list[i]))
                {
                    tmpList.Add(list[i]);
                }
            }

            signTable = new string[tmpList.Count];
            keyTable = new string[tmpList.Count];
        }

        public void AddHuffNum(int num)
        {
            HuffmanTree hTemp = new HuffmanTree(num);
            Node eTemp = new Node(hTemp);
            if (first == null)
            {
                first = eTemp;
            }
            else
            {
                eTemp.link = first;
                first = eTemp;
            }

            count++;
        }

        //增加数字，排出所有节点
        public void AddNum(int num)
        {
            if (first == null)
            {
                AddHuffNum(num);
                return;
            }

            Node tmp = first;
            while (tmp != null)
            {
                if (tmp.data.Num == num)
                {
                    tmp.data.IncFreq();
                    return;
                }

                tmp = tmp.link;
            }

            AddHuffNum(num);
        }

        //整理节点，按权重freq的升序排列
        public void SortTree()
        {
            if (first != null && first.link != null)
            {
                Node tmp1;
                Node tmp2;
                for (tmp1 = first; tmp1 != null; tmp1 = tmp1.link)
                {
                    for (tmp2 = tmp1.link; tmp2 != null; tmp2 = tmp2.link)
                    {
                        if (tmp1.data.Freq > tmp2.data.Freq)
                        {
                            HuffmanTree tmpHT = tmp1.data;
                            tmp1.data = tmp2.data;
                            tmp2.data = tmpHT;
                        }
                    }
                }
            }
        }

        //计数加入的数的个数
        public int Length()
        {
            return count;
        }

        //合并树节点及其权重freq，生成霍夫曼树
        public void MergeTree()
        {
            if (first != null)
            {
                if (first.link != null)
                {
                    HuffmanTree aTmp = RemoveTree();
                    HuffmanTree bTmp = RemoveTree(); //移除下一个
                    HuffmanTree sumTmp = new HuffmanTree(0);
                    sumTmp.LChild = aTmp;
                    sumTmp.RChild = bTmp;
                    sumTmp.Freq = aTmp.Freq + bTmp.Freq;
                    InsertTree(sumTmp);
                }
            }
        }

        //将第一个树节点返回，并且将后一个向前移动
        public HuffmanTree RemoveTree()
        {
            if (first != null)
            {
                HuffmanTree hTmp;
                hTmp = first.data;
                first = first.link;
                count--;
                return hTmp;
            }

            return null;
        }

        //将合并后的树节点重新插入霍夫曼树中
        public void InsertTree(HuffmanTree hTmp)
        {
            Node eTmp = new Node(hTmp);
            if (first == null)
            {
                first = eTmp;
            }
            else
            {
                Node node = first;
                while (node.link != null)
                {
                    if (node.data.Freq <= hTmp.Freq && node.link.data.Freq >= hTmp.Freq)
                    {
                        break;
                    }

                    node = node.link;
                }

                eTmp.link = node.link;
                node.link = eTmp;
            }

            count++;
        }
        //生成霍夫曼编码表

        static int pos = 0;

        public static void MakeKey(HuffmanTree tree, String code)
        {
            if (tree.LChild == null)
            {
                signTable[pos] = Convert.ToString(tree.Num);
                keyTable[pos] = code;
                pos++;
            }
            else
            {
                MakeKey(tree.LChild, code + "0");
                MakeKey(tree.RChild, code + "1");
            }
        }

        static String appendStr = "";

        //将输入的数组转换成新的编码字符
        public static List<byte> Translate(List<int> old)
        {
            List<byte> newData = new List<byte>();
            for (int i = 0; i < old.Count; i++)
            {
                for (int j = 0; j < signTable.Length; j++)
                {
                    if (signTable[j] == old[i].ToString())
                    {
                        if (appendStr.Length > 8)
                        {
                            String appendSubStr = appendStr.Substring(0, 8);
                            byte append = Convert.ToByte(appendSubStr, 2);
                            newData.Add(append);
                            appendStr = appendStr.Remove(0, 8);
                        }

                        String str = appendStr + keyTable[j];
                        if (str.Length >= 8)
                        {
                            String subStr = str.Substring(0, 8);
                            byte b = Convert.ToByte(subStr, 2);
                            appendStr = str.Remove(0, 8);
                            newData.Add(b);
                            break;
                        }

                        appendStr = str;
                    }
                }
            }

            return newData;
        }

        static String keyStr = "";

        static List<int> decodeSigns = new List<int>();

        //霍夫曼编码解码
        public List<int> readHuffmanCode(HuffmanTree htree, List<byte> result)
        {
            List<byte> decodeByte = new List<byte>(result);
            String decodeStr = byteToStr(decodeByte[0]);
            int i = 0;
            int countNum = 1;
            while (i < decodeByte.Count)
            {
                HuffmanTree tree = htree;
                while (tree.LChild != null)
                {
                    if (decodeStr.Equals(""))
                    {
                        i++;
                        if (i == decodeByte.Count)
                        {
                            keyStr = keyStr + appendStr;
                            break;
                        }

                        decodeStr = byteToStr(decodeByte[i]);
                    }

                    if (decodeStr.Substring(0, 1).Equals("0"))
                    {
                        tree = tree.LChild;
                        keyStr += "0";
                        decodeStr = decodeStr.Substring(1);
                        countNum++;
                    }
                    else
                    {
                        tree = tree.RChild;
                        keyStr += "1";
                        decodeStr = decodeStr.Substring(1);
                        countNum++;
                    }
                }

                for (int j = 0; j < keyTable.Length; j++)
                {
                    if (keyStr.Equals(keyTable[j]))
                    {
                        decodeSigns.Add(Convert.ToInt32(signTable[j]));
                        keyStr = "";
                        break;
                    }
                }
            }

            return decodeSigns;
        }

        public String byteToStr(Byte testByte)
        {
            Byte bt = testByte;
            String temStr = Convert.ToString(bt, 2);
            if (temStr.Length != 8)
            {
                int i = 8 - temStr.Length;
                int j = 0;
                while (j < i)
                {
                    temStr = "0" + temStr;
                    j++;
                }
            }

            return temStr;
        }

        public String[] GetSignTable()
        {
            return signTable;
        }

        public String[] GetKeyTable()
        {
            return keyTable;
        }
    }
}