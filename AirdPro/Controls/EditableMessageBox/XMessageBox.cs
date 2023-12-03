using System;
using System.Windows.Forms;

namespace AirdPro.Controls.EditableMessageBox;

public partial class XMessageBox : Form
{
    public XMessageBox(string title, string content)
    {
        InitializeComponent();
        Text = title;
        tbContent.Text = content;
    }
    
    public XMessageBox(string content)
    {
        InitializeComponent();
        tbContent.Text = content;
    }
}