using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.Converters.HuffmanTree
{
    public class Node
    {
        public HuffmanTree data;
        public Node link;

        public Node(HuffmanTree newData)
        {
            data = newData;
        }
    }
}
