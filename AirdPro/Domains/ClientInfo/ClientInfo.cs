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
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using AirdPro.Constants;
using AirdPro.Utils;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;

namespace AirdPro.Domains
{
    public class ClientInfo
    {
        public static string SystemType;
        public static string CpuInfo;
        public static string PhysicMemory;
        public static string OpVersion;
        public static string AirdProVersion;

        static ClientInfo()
        {
            SystemType = GetSystemType();
            CpuInfo = GetCpuInfo();
            PhysicMemory = GetPhysicMemory();
            OpVersion = GetOpVersion();
            AirdProVersion = SoftwareInfo.GetVersion();
        }

        public static string toJSON()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("SystemType", SystemType);
            dict.Add("CpuInfo", CpuInfo);
            dict.Add("PhysicMemory",PhysicMemory);
            dict.Add("OpVersion",OpVersion);
            dict.Add("AirdProVersion",AirdProVersion);
            string json = JsonConvert.SerializeObject(dict);
            return json;
        }
        //获取系统类型
        public static string GetSystemType()
        {
            try
            {
                string st = "";
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    st = mo["SystemType"].ToString();
                }

                moc.Dispose();
                mc.Dispose();
                return st;
            }
            catch
            {
                return "Unknown";
            }
        }

        //获取操作系统型号
        public static string GetOpVersion()
        {
            return new ComputerInfo().OSFullName;
        }

        //获取CPU信息
        public static string GetCpuInfo()
        {
            string cpuName = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                cpuName = mo["Name"].ToString();
            }

            mos.Dispose();
            return cpuName;
        }

        //获取物理内存数目和大小
        public static string GetPhysicMemory()
        {
            // 创建ManagementClass对象并设置查询条件
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            long totalMemory = 0L;
            foreach (var mo in moc)
            {
                ulong oneMem = (ulong)mo["TotalPhysicalMemory"];
                totalMemory += (long)oneMem;
            }
            // 将字节数转换为更友好的格式
            return AirdProFileUtil.GetSizeLabel(totalMemory);
        }
    }
}