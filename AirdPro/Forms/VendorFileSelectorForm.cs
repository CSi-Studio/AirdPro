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
using System.Windows.Forms;
using AirdPro.Constants;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class VendorFileSelectorForm : Form
    {
        private AirdForm airdForm;
        public VendorFileSelectorForm(AirdForm form)
        {
            InitializeComponent();
            this.airdForm = form;
            betterFolderBrowser.Multiselect = true;
        }

        private void CustomPathForm_Load(object sender, EventArgs e)
        {
            tbPaths.Text = string.Empty;
        }

        public void clearInfos()
        {
            this.tbPaths.Text = string.Empty;
            for (int i = 0; i < gBoxMode.Controls.Count; i++)
            {
                var cb = gBoxMode.Controls[i] as RadioButton;
                if (cb.Checked)
                {
                    cb.Checked = false;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string expType = null;
            for (int i = 0; i < gBoxMode.Controls.Count; i++)
            {
                var cb = gBoxMode.Controls[i] as RadioButton;
                if (cb.Checked)
                {
                    expType = cb.Text;
                }
            }

            if (expType == null)
            {
                MessageBox.Show("Choose one acquisition mode first!");
                return;
            }

            var paths = tbPaths.Text;

            if (paths.IsNullOrEmpty())
            {
                MessageBox.Show("Input your own paths first!");
                return;
            }
            var pathList = paths.Split(Const.Change_Line.ToCharArray());

            foreach (var path in pathList)
            {
                airdForm.addFile(path, expType);
            }
            this.Hide();
        }

        private void btnFileSelector_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var filePath in openFileDialog.FileNames)
                {
                    tbPaths.Text = tbPaths.Text + filePath + Const.Change_Line;
                }
            }
        }


        private void btnFolderSelector_Click(object sender, EventArgs e)
        {
            if (betterFolderBrowser.ShowDialog(this) == DialogResult.OK)
            {
                foreach (var filePath in betterFolderBrowser.SelectedPaths)
                {
                    tbPaths.Text = tbPaths.Text + filePath + Const.Change_Line;
                }
                
            }
        }
    }
}
