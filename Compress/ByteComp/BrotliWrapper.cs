﻿/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using BrotliSharpLib;
using Compress.Enums;

namespace Compress
{
    public class BrotliWrapper:ByteComp
    {
        public override string getName()
        {
            return ByteCompType.Brotli.ToString();
        }

        public override byte[] encode(byte[] data)
        {
            byte[] compressed = Brotli.CompressBuffer(data, 0, data.Length);
            return compressed;
        }

        public override byte[] decode(byte[] data)
        {
            byte[] uncompressed = Brotli.DecompressBuffer(data, 0, data.Length);
            return uncompressed;
        }

    }
}