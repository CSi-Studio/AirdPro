﻿/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.IO;
using Compress;
using Compress.Enums;
using Ionic.Zlib;

namespace AirdPro.Algorithms
{
    public class ZlibWrapper:ByteComp
    {
        //使用zlib将byte数组压缩
        public override string getName()
        {
            return ByteCompType.Zlib.ToString();
        }

        public override byte[] encode(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                Stream compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Default);

                ZlibBaseStream.CompressBuffer(data, compressor);
                return ms.ToArray();
            }
        }

        //使用zlib将byte数组解压缩
        public override byte[] decode(byte[] data)
        {
            return ZlibStream.UncompressBuffer(data);
        }
    }
}