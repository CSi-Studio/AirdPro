/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

namespace AirdSDK.Compressor
{
    public class HuffmanTree
    {
        private HuffmanTree lChild;
        private HuffmanTree rChild;
        private int num;
        private int freq;

        public HuffmanTree(int num)
        {
            this.num = num;
            this.freq = 1;
        }

        public HuffmanTree LChild
        {
            get { return lChild; }
            set { lChild = value; }
        }

        public HuffmanTree RChild
        {
            get { return rChild; }
            set { rChild = value; }
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        public int Freq
        {
            get { return freq; }
            set { freq = value; }
        }

        public void IncFreq()
        {
            freq++;
        }

        public byte[] toBytes()
        {
            //TODO 童俊杰
            return null;
        }

        public static HuffmanTree fromBytes(byte[] byteTree)
        {
            //TODO 童俊杰
            return null;
        }
    }
}