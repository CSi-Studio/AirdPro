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
using AirdPro.Redis;
using AirdPro.Utils;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;

namespace AirdPro.Domains
{
    public class ClientInfo
    {
        public string ClientID { get; set; }
        public string CpuInfo { get; set; }
        public string PhysicMemory { get; set; }
        public string OpVersion { get; set; }
        public string AirdProVersion { get; set; }
        public double LastUpdateTime { get; set; }
        public string ServerName { get; set; }
        public List<string> IPList { get; set; }
        public bool ConsumingJob { get; set; }

        public void init()
        {
            CpuInfo = BuildCpuInfo();
            ServerName = Environment.MachineName;
            PhysicMemory = BuildPhysicMemory();
            OpVersion = BuildOpVersion();
            AirdProVersion = SoftwareInfo.VERSION;
            ClientID = BuildUniqueID();
        }

        public string ToJson()
        {
            LastUpdateTime = DateTime.Now.ToOADate();
            IPList = HttpUtil.GetIPV4List();
            ConsumingJob = RedisManager.GlobalConsumeJobSwitch;
            return JsonConvert.SerializeObject(this);
        }

        //获取操作系统型号
        public static string BuildOpVersion()
        {
            return new ComputerInfo().OSFullName;
        }

        //获取CPU信息
        public static string BuildCpuInfo()
        {
            string cpuName = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (var mo in mos.Get())
            {
                cpuName = mo["name"].ToString();
            }

            mos.Dispose();
            return cpuName;
        }

        //获取物理内存数目和大小
        public static string BuildPhysicMemory()
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

        public static string BuildUniqueID()
        {
            // 创建 ManagementClass 对象
            ManagementClass mc = new ManagementClass("Win32_ComputerSystemProduct");

            // 获取计算机硬件信息
            ManagementObjectCollection moc = mc.GetInstances();

            // 遍历计算机硬件信息并生成唯一标识符
            foreach (ManagementObject mo in moc)
            {
                string identifier = mo.Properties["UUID"].Value.ToString();
                return identifier;
            }

            return Environment.MachineName;
        }
    }
}