/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;

namespace AirdSDK.Beans
{
    public class MobiInfo
    {
        public long dictStart; // start position in the aird for mobi array
        public long dictEnd; //end position in the aird for mobi array
        public string unit; //ion mobility unit
        public double value; //ion mobility value
        public string type; //ion mobility type, see MobilityType
        
        public MobiInfoProto ToProto()
        {
            MobiInfoProto proto = new MobiInfoProto()
            {
                DictStart = this.dictStart,
                DictEnd = this.dictEnd,
                Value = this.value,
            };

            if (unit != null)
            {
                proto.Unit = unit;
            }
            if (type != null)
            {
                proto.Type = type;
            }
            
            
            return proto;
        }
        
        public static MobiInfo FromProto(MobiInfoProto proto)
        {
            if (proto == null)
                throw new ArgumentNullException(nameof(proto));

            return new MobiInfo
            {
                dictStart = proto.DictStart,
                dictEnd = proto.DictEnd,
                unit = proto.Unit,
                value = proto.Value,
                type = proto.Type
            };
        }
    }
}