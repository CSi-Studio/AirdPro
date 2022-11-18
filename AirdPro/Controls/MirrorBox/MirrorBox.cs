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
