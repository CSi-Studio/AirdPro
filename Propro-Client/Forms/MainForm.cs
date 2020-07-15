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
        public MainForm()
        {
            InitializeComponent();
        }

        private void convertToAirdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AirdForm form = new AirdForm();
            form.Show();
        }

        private void helpToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }
    }
}
