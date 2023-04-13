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
using System.Windows.Forms;
using AirdPro.Constants;

namespace AirdPro.Controls.MirrorBox
{
    public partial class MirrorBox : UserControl
    {
        public string vendorFolder;
        public string airdFolder;

        public MirrorBox()
        {
            InitializeComponent();
        }

        private void btnVendorFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select a vendor files folder to monitor";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                tbVendor.Text = dialog.SelectedPath;
                vendorFolder = dialog.SelectedPath;
            }
        }

        private void btnAirdFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select an aird files folder as output folder";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                tbAird.Text = dialog.SelectedPath;
                airdFolder = dialog.SelectedPath;
            }
        }

        public string getPair()
        {
            return vendorFolder + Const.COLON + airdFolder;
        }
    }
}