﻿/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using Compress.Enums;
using ZstdNet;

namespace Compress
{
    public class ZstdWrapper:ByteComp
    {

        //使用zstd将byte数组压缩
        public override string getName()
        {
            return ByteCompType.Zstd.ToString();
        }

        public override byte[] encode(byte[] data)
        {
            var compressor = new Compressor();
            var compressedData = compressor.Wrap(data);
            return compressedData;
        }

        public override byte[] decode(byte[] data)
        {
            var decompressor = new Decompressor();
            var decompressedData = decompressor.Unwrap(data);
            return decompressedData;
        }

    }
}