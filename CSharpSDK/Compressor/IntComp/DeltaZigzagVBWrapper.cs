/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using AirdSDK.Enums;
using CSharpFastPFOR;
using CSharpFastPFOR.Port;

namespace AirdSDK.Compressor
{
    public class DeltaZigzagVBWrapper : IntComp
    {
        public override string getName()
        {
            return IntCompType.DZVB.ToString();
        }

        public override int[] encode(int[] uncompressed)
        {
            IntegerCODEC codec = new DeltaZigzagVariableByte();
            int[] compressed = new int[uncompressed.Length + uncompressed.Length / 100 + 1024];// could need more
            IntWrapper outputoffset = new IntWrapper(1);
            codec.compress(uncompressed, new IntWrapper(0), uncompressed.Length, compressed, outputoffset);
            compressed = Arrays.copyOf(compressed, outputoffset.intValue());
            compressed[0] = uncompressed.Length;
            return compressed;
        }

        public override int[] decode(int[] compressed)
        {
            if (compressed.Length == 0) {
                return new int[0];
            }
            IntegerCODEC codec = new DeltaZigzagVariableByte();
            int[] decompressed = new int[compressed[0]];
            codec.uncompress(compressed, new IntWrapper(1), compressed.Length - 1, decompressed, new IntWrapper(0));
            return decompressed;
        }
    }
}