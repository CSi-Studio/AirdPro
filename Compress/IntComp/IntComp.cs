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
using Compress.Enums;

namespace Compress
{
    public abstract class IntComp
    {
        public abstract string getName();
        public abstract int[] encode(int[] uncompressed);
        public abstract int[] decode(int[] compressed);

        public static IntComp build(IntCompType type)
        {
            switch (type)
            {
                case IntCompType.VB:
                    return new VarByteWrapper();
                case IntCompType.BP:
                    return new BinPackingWrapper();
                case IntCompType.Simple:
                    return new Simple16Wrapper();
                case IntCompType.NewPFD:
                    return new NewPFDS16Wrapper();
                case IntCompType.OptPFD:
                    return new OptPFDS16Wrapper();
                case IntCompType.Empty:
                    return new Empty();
                default: throw new Exception("No Implementation for " + type);
            }
        }
    }
}

