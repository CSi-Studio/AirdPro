using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirdPro.Forms
{
    public partial class MainForm : Form
    {
        private AboutForm aboutForm = new AboutForm();
        private GlobalSettingForm globalSettingForm;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (aboutForm == null || aboutForm.IsDisposed)
            {
                aboutForm = new AboutForm();
            }

            aboutForm.Show();
        }

        private void globalSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (globalSettingForm == null || globalSettingForm.IsDisposed)
            {
                globalSettingForm = new GlobalSettingForm();
            }

            globalSettingForm.Show();
        }

        private void startConversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.conversionForm.Visible = true;
        }
    }
}
