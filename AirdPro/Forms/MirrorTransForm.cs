using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirdPro.Properties;

namespace AirdPro.Forms
{
    public partial class MirrorTransForm : Form
    {
        public MirrorTransForm()
        {
            InitializeComponent();
        }

        private void MirrorTransForm_Load(object sender, EventArgs e)
        {
            string mirrorPairsString = Settings.Default.MirrorPairs;

        }

        
    }
}
