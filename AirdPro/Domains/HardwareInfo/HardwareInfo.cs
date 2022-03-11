﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.InteropServices;
using System.Net;
using Microsoft.VisualBasic.Devices;

namespace AirdPro.Domains.HardwareInfo
{
    public class HardwareInfo
    {
        public string systemType;
        public string cpuInfo;
        public string physicMemory;
        public string opVersion;
        private static HardwareInfo instance;
        public static HardwareInfo Instance()
        {
            if (instance == null)
                instance = new HardwareInfo();
            return instance;
        }

        public HardwareInfo()
        {
            systemType = getSystemType();
            cpuInfo = getCPUInfo();
            physicMemory = getPhysicMemory();
            opVersion = getOPVersion();
        }
        //获取系统类型
        public string getSystemType()
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

                moc = null;
                mc = null;
                return st;
            }
            catch
            {
                return "Unknown";
            }
            finally
            {
            }
        }
        //获取操作系统型号
        public string getOPVersion()
        {
            string opVersion = "";
            opVersion = new ComputerInfo().OSFullName;
            return opVersion;
        }
        //获取CPU信息
        public string getCPUInfo()
        {
            string CPUName = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                CPUName = mo["Name"].ToString();
            }
            mos.Dispose();
            return CPUName;
        }
        //获取物理内存数目和大小
        public string getPhysicMemory()
        {
            string physicMemoryInfo = "";
            ManagementClass mc = new ManagementClass("Win32_PhysicalMemory");
            ManagementObjectCollection moc = mc.GetInstances();
            physicMemoryInfo = "Physical Memory Numbers : " + moc.Count.ToString() + "\r\n";
            double capacity = 0.0;
            int count = 0;
            foreach (ManagementObject mo in moc)
            {
                count++;
                capacity = ((Math.Round(Int64.Parse(mo.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));
                physicMemoryInfo += "The Size of No." + count.ToString() + " Physical Memory is " + capacity.ToString() + " G " + "\r\n";
            }
            moc.Dispose();
            mc.Dispose();
            return physicMemoryInfo;
        }


    }
}