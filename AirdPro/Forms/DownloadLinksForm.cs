using System;
using System.IO;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Xml.Serialization;
using AirdPro.Algorithms.Parser;
using AirdPro.Algorithms.Parser.DownloadXML;

namespace AirdPro.Forms;

public partial class DownloadLinksForm : Form
{
    public string from;
    public string identifier;
    public string PXDUrl = "https://proteomecentral.proteomexchange.org/cgi/GetDataset?ID=";

    public string getUniqueTag()
    {
        return from + ":" + identifier;
    }

    public DownloadLinksForm(string from, string identifier)
    {
        InitializeComponent();
        this.from = from;
        this.identifier = identifier;
        readPXDPage();
    }

    public void init()
    {
        switch (from)
        {
            case "PRIDE":
                readPXDPage();
                break;
        }
    }

    public void readPXDPage()
    {
        Text = getUniqueTag() + " loading";
        WebBrowser web = new WebBrowser();
        web.Navigate(PXDUrl + identifier);
        web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(readPXDPage_Completed);
    }

    public void readPXDPage_Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        WebBrowser web = (WebBrowser)sender;
        HtmlElementCollection elements = web.Document.GetElementsByTagName("table");
        HtmlElement mainTable = null;
        if (elements.Count > 2)
        {
            mainTable = elements[1];
        }
        else
        {
            return;
        }

        HtmlElement element = mainTable.Children[0].Children[4].Children[1].Children[0]; //直接定位到<a>标签
        string href = element.GetAttribute("href");
      
        WebBrowser detail = new WebBrowser();
        detail.Navigate(href);
        detail.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(readXmlPage_Completed);
    }

    public void readXmlPage_Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        WebBrowser web = (WebBrowser)sender;
        string xml = web.Document.Body.InnerText;
        xml = xml.Replace("\r\n-", "");
        xml = xml.Trim();
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProteomeXchangeDataset));
            StringReader reader = new StringReader(xml);
            ProteomeXchangeDataset project = (ProteomeXchangeDataset)serializer.Deserialize(reader);
            if (project.datasetFileList.Count == 0)
            {
                MessageBox.Show("Content is Empty");
                return;
            }

            tabControl.TabPages.Clear();
            string downloadList = "";
            int count = 0;

            for (var i = 0; i < project.datasetFileList.Count; i++)
            {
                count += 1;
                DatasetFile file = project.datasetFileList[i];
                if (file.cvParams.Count == 0)
                {
                    continue;
                }
                
                for (int j = 0; j < file.cvParams.Count; j++)
                {
                    if (file.cvParams[j].accession.Equals("MS:1002846"))
                    {
                        downloadList += file.cvParams[j].value + "\r\n";
                    }
                }
               
                if (count % 1000 == 0)
                {
                    TabPage tabPage = new TabPage();
                    tabPage.SuspendLayout();
                    tabPage.Text = (count - 999) + "~" + count;
                    buildOutput(tabPage, tabPage.Text, downloadList);
                    tabControl.TabPages.Add(tabPage);
                    downloadList = "";
                }
            }

            TabPage tabPageLast = new TabPage();
            tabPageLast.SuspendLayout();
            tabPageLast.Text = (count - count % 1000 + 1) + "~" + count;
            buildOutput(tabPageLast, tabPageLast.Text, downloadList);
            tabControl.TabPages.Add(tabPageLast);
        }
        catch (Exception ee)
        {
            
        }
    }
    
    public void buildOutput(TabPage tabPage, string name, string tasks)
    {
        TextBox tb = new TextBox();
        tb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tb.MaxLength = 1000000;
        tb.Multiline = true;
        tb.Name = "tb"+name;
        tb.ScrollBars = ScrollBars.Both;
        tb.Size = new System.Drawing.Size(922, 413);
        tb.TabIndex = 1;
        tb.Text = tasks;
        tabPage.Controls.Add(tb);
    }

    private void DownloadLinksForm_Load(object sender, EventArgs e)
    {
    }
}