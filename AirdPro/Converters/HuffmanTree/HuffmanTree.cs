using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Converters.HuffmanTree
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
    }
}
