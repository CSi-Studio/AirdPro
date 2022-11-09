using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirdPro.Repository
{
    public partial class RepositoryGuiderForm : Form
    {
        public RepositoryGuiderForm()
        {
            InitializeComponent();
        }

        private void btnPX_Click(object sender, EventArgs e)
        {
            new PXForm().Show();
        }

        private void btnML_Click(object sender, EventArgs e)
        {
            new MLForm().Show();
        }
    }
}
