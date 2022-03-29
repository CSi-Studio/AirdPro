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

namespace AirdSDK.Compressor
{
    public abstract class SortedIntComp
    {
        public abstract string getName();
        public abstract int[] encode(int[] uncompressed);
        public abstract int[] decode(int[] compressed);

        public static SortedIntComp build(SortedIntCompType type)
        {
            switch (type)
            {
                case SortedIntCompType.IBP:
                    return new IntegratedBinPackingWrapper();
                case SortedIntCompType.IVB:
                    return new IntegratedVarByteWrapper();
                default: throw new Exception("No Implementation for " + type);
            }
        }
    }
}

