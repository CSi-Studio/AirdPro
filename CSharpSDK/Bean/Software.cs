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
    public class Software
    {
        //软件名称
        public string name;


        public string type = "acquisition";

        //软件版本号
        public string version;

        public SoftwareProto ToProto()
        {
            SoftwareProto proto = new SoftwareProto();
            
            if (name != null)
            {
                proto.Name = name;
            } 
            if (type != null)
            {
                proto.Type = type;
            } 
            if (version != null)
            {
                proto.Version = version;
            }
            return proto;
        }
        
        // 静态方法，用于从protobuf对象转换
        public static Software FromProto(SoftwareProto proto)
        {
            if (proto == null)
                throw new ArgumentNullException(nameof(proto));

            return new Software
            {
                name = proto.Name,
                type = proto.Type,
                version = proto.Version
            };
        }
    }
}