/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
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
            lblCompanyInfo.Text = "CopyRight © 2022 CSi (HangZhou) Biotechnology Corporation co.,ltd. All rights reserved.";
            lblCPUInfo.Text =  hardwareInfo.cpuInfo;
            lblPhysicMemory.Text = hardwareInfo.physicMemory;
            lblOpVersion.Text = hardwareInfo.opVersion + " " + hardwareInfo.systemType;
            lblDesc.Text = SoftwareInfo.getDescription();
            tbPaperZDPD.Text = "https://doi.org/10.1186/s12859-021-04490-0 \r\n" +
                "Lu, M., An, S., Wang, R. et al. Aird: a computation-oriented mass spectrometry data format enables a higher compression ratio and less decoding time. BMC Bioinformatics 23, 35 (2022).";
        }

        //查看许可证状态
        private void LicenseCheck_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llLicense.LinkVisited = true;
            Process.Start( "http://license.coscl.org.cn/MulanPSL2");
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
        //查看AirdPro论文引用
        private void AirdProPaperCitation_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            llAirdProPaper.LinkVisited = true;
            Process.Start("https://bmcbioinformatics.biomedcentral.com/articles/10.1186/s12859-021-04490-0");
        }
    }
}
