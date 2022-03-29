﻿/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using AirdPro.Algorithms;
using Compress.Enums;

namespace Compress;

public abstract class ByteComp
{
    public abstract string getName();
    public abstract byte[] encode(byte[] uncompressed);
    public abstract byte[] decode(byte[] compressed);

    public static ByteComp build(ByteCompType type)
    {
        switch (type)
        {
            case ByteCompType.Brotli:
                return new BrotliWrapper();
            case ByteCompType.Snappy:
                return new SnappyWrapper();
            case ByteCompType.Zstd:
                return new ZstdWrapper();
            case ByteCompType.Zlib:
                return new ZlibWrapper();
            default: throw new Exception("No Implementation for " + type);
        }
    }
}