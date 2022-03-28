﻿using AirdPro.Constants;
using CSharpFastPFOR;

namespace Compress
{
    public class OptPFDS16Wrapper:IntComp
    {
        public override string getName()
        {
            return IntCompType.OptPFD.ToString();
        }

        public override int[] encode(int[] uncompressed)
        {
            int[] compressedInts = new IntCompressor(new OptPFDS16()).compress(uncompressed);
            return compressedInts;
        }

        public override int[] decode(int[] compressed)
        {
            int[] sortedInts = new IntCompressor(new OptPFDS16()).uncompress(compressed);
            return sortedInts;
        }
    }
}