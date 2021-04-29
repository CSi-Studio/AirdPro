using System;
using System.Windows.Forms;
using ThermoFisher.CommonCore.Data;

namespace AirdPro.Forms
{
    public partial class CustomPathForm : Form
    {
        private AirdForm airdForm;
        public CustomPathForm(AirdForm form)
        {
            InitializeComponent();
            this.airdForm = form;
        }

        private void CustomPathForm_Load(object sender, EventArgs e)
        {
            tbPaths.Text = "";
        }

        public void clearInfos()
        {
            this.tbPaths.Text = "";
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
            String expType = null;
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
            var pathList = paths.Split("\r\n".ToCharArray());

            foreach (var path in pathList)
            {
                airdForm.addFile(path, expType);
            }
            this.Hide();
        }
    }
}
