using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AirdPro.Domains.Convert;
using System.Collections;
using AirdPro.Forms.Config;


namespace AirdPro.Forms
{
    //ConfigSubject的订阅者（观察者）
    public partial class ConfigListView : Form
    {     
        ArrayList currentFiles = new ArrayList();
        public ConfigListView()
        {
            InitializeComponent();
        }
        public void Receive (object obj)
        {
            ConfigSubject configSubject = obj as ConfigSubject;
            if(configSubject != null)
            {
                if (configSubject.ConfigName != "" && !currentFiles.Contains(configSubject.ConfigName))
                {
                    ListViewItem item = new ListViewItem(new string[] { configSubject.ConfigName,
                                                                        configSubject.Creator,
                                                                        configSubject.ConfigMzPrecision,
                                                                        configSubject.ConfigCompressor,
                                                                        configSubject.ConfigOutputPath});
                    lvConfigList.Items.Add(item);
                    currentFiles.Add(configSubject.ConfigName);
                }
            }
        }
    }
}
