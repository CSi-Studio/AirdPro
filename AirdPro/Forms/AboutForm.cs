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
using System.Diagnostics;
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Domains;

namespace AirdPro.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void HelpAboutForm_Load(object sender, EventArgs e)
        {
            HardwareInfo hardwareInfo = new HardwareInfo();
            lblSoftwareVersion.Text = SoftwareInfo.getVersion();
            lblCompanyInfo.Text =
                "CopyRight © 2022 CSi (HangZhou) Biotechnology Corporation co.,ltd. All rights reserved.";
            lblCPUInfo.Text = hardwareInfo.cpuInfo;
            lblPhysicMemory.Text = hardwareInfo.physicMemory;
            lblOpVersion.Text = hardwareInfo.opVersion + " " + hardwareInfo.systemType;
            lblDesc.Text = SoftwareInfo.getDescription();
            tbPaperZDPD.Text =
                "1. Lu, M., An, S., Wang, R. et al. Aird: a computation-oriented mass spectrometry data format enables a higher compression ratio and less decoding time. BMC Bioinformatics 23, 35 (2022).\r\n" +
                "2. Wang,J. et al. StackZDPD: a novel encoding scheme for mass spectrometry data optimized for speed and compression ratio. Scientific Reports, 12, 5384.(2022).\r\n" +
                "3. ComboComp: A combinable compressors framework with dynamic-decider for lossy mass spectrometry data compression."
                ;
        }

        //查看许可证状态
        private void LicenseCheck_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llLicense.LinkVisited = true;
            Process.Start("http://license.coscl.org.cn/MulanPSL2");
        }

        //打开AirdPro在Gitee上的页面
        private void AirdProonGitee_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llAirdProGitee.LinkVisited = true;
            Process.Start("https://gitee.com/CSiStudio/AirdPro");
        }

        //打开AirdPro在Github上的页面
        private void CheckAirdProonGithub_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llAirdProonGithub.LinkVisited = true;
            Process.Start("https://github.com/CSi-Studio/AirdPro");
        }

        //打开AirdPro-JDK在Gitee上的页面
        private void AirdProSDKGitee_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llAirdProSdkGitee.LinkVisited = true;
            Process.Start("https://gitee.com/CSiStudio/Aird-SDK-Java");
        }

        //打开AirdPro-JDK在Github上的页面
        private void AirdProSDKGithub_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llAirdProSDKGithub.LinkVisited = true;
            Process.Start("https://github.com/CSi-Studio/Aird-SDK");
        }

        private void tbPaperZDPD_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}