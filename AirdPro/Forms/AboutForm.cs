using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Domains.HardwareInfo;
using AirdPro.DomainsCore.Aird;

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
            lblCompanyInfo.Text = "CopyRight © 2022"+ "\r\n"+ "CSi (HangZhou) Biotechnology Corporation co.,ltd." + "\r\n" + "All rights reserved.";
            lblCPUInfo.Text =  hardwareInfo.cpuInfo;
            lblPhysicMemory.Text = hardwareInfo.physicMemory;
            lblOpVersion.Text = hardwareInfo.opVersion + " " + hardwareInfo.systemType;
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
