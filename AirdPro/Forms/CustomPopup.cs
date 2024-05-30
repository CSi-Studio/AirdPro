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
    public partial class CustomPopup : Form
    {
        public CustomPopup()
        {
            InitializeComponent();
            this.Text = "This is a custom popup window";
            this.Controls.Add(new Label
            {
                Text = "",
                Location = new Point(10, 10)
            });
        }

        private void CustomPopup_Load(object sender, EventArgs e)
        {
        }

        private void content_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
