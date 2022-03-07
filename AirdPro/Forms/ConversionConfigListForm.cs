using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using AirdPro.Storage;


namespace AirdPro.Forms
{
    public partial class ConversionConfigListForm : Form, Observer
    {
        public ConversionConfigListForm()
        {
            InitializeComponent();
        }
        private void ConversionConfigListForm_Load(object sender, EventArgs e)
        {

        }
        public void update(Dictionary<string, ConversionConfig> configMap)
        {
            lvConfigList.Items.Clear();
            foreach (var configEntry in configMap)
            {
                ListViewItem item = new ListViewItem(new string[]
                {
                    configEntry.Key,
                    configEntry.Value.creator,
                    configEntry.Value.mzPrecision.ToString(),
                    configEntry.Value.getCompressorStr(),
                    configEntry.Value.outputPath
                });
                lvConfigList.Items.Add(item);
            }
        }

    }
}
