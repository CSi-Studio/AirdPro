using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows.Forms;
using AirdPro.Algorithms.Parser.DownloadXML;
using AirdPro.Constants;
using AirdPro.Utils;
using HZH_Controls;

namespace AirdPro.Forms;

public partial class DownloadLinksForm : Form
{
    private string _web;
    private string _from;
    private string _identifier;

    private WebBrowser _pxdPage;
    private WebBrowser _massIvePage;

    private string GetUniqueTag()
    {
        return _from + ":" + _identifier;
    }

    public DownloadLinksForm(string web, string from, string identifier)
    {
        InitializeComponent();
        _web = web;
        _from = from;
        _identifier = identifier;
        tbFrom.Text = from;
        tbIdentifier.Text = identifier;
        Text = from + ":" + identifier;
    }

    private void DownloadLinksForm_Load(object sender, EventArgs e)
    {
        if (_web.Equals(Froms.WEB_ML))
        {
            tbHome.Text = UrlConst.pxDetailUrl + _identifier;
            tbFTP.Text = UrlConst.mlFtpUrl + _identifier;
        }
        else if (_web.Equals(Froms.WEB_PX))
        {
            tbHome.Text = UrlConst.pxDetailUrl + _identifier;
        }
    }

    //所有的链接均从PXD页面开始路由
    private void ReadPxdPage()
    {
        Text = GetUniqueTag() + " loading";
        _pxdPage.Navigate(UrlConst.pxDetailUrl + _identifier);
    }

    private async void readPXDPage_Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        WebBrowser web = (WebBrowser)sender;
        HtmlElementCollection elements = web.Document.GetElementsByTagName("table");
        if (elements.Count < 8)
        {
            Loading(false);
            return;
        }

        tbFTP.Text = elements[6].Children[0].Children[0].Children[0].Children[0].GetAttribute("href");
        //路由，不同的源需要解码PXD详情页上不同的元素
        switch (_from.ToLower())
        {
            case "pride":
            case "iprox":
                HtmlElement announcementXMLElement =
                    elements[1].Children[0].Children[4].Children[1].Children[0]; //直接定位到<a>标签
                string prideHref = announcementXMLElement.GetAttribute("href");
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(prideHref);
                        response.EnsureSuccessStatusCode();
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            using (var reader = new System.IO.StreamReader(stream))
                            {
                                string responseData = await reader.ReadToEndAsync();
                                List<string> downloadList = PRIDEParser.parse(responseData);
                                RenderList(downloadList);
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"Http Request Error: {ex.Message}");
                    }
                }

                lblTips.Text = "Use 迅雷,IDM,NDM to download the following files";
                break;
            case "jpost":
                HtmlElement jpostElement = elements[6].Children[0].Children[1].Children[0].Children[0]; //直接定位到<a>标签
                string jpostHref = jpostElement.GetAttribute("href");
                string[] array = jpostHref.Split('/');
                TabPage tab = BuildOutput("1-1", jpostHref + array[array.Length - 2] + "_all.zip");
                tabControl.TabPages.Add(tab);
                lblTips.Text = "Use FileZilla or other FTP tools to download the following files";
                break;
            case "massive":
                HtmlElement massIVEElement = elements[6].Children[0].Children[0].Children[0].Children[0]; //直接定位到<a>标签
                string massIVEHref = massIVEElement.GetAttribute("href");
                _massIvePage.Navigate(massIVEHref);
                lblTips.Text = "Use FileZilla or other FTP tools to download the following files";
                break;
        }
    }

    private void readMassIVEPage_Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        WebBrowser web = (WebBrowser)sender;
        HtmlElement inputElement = web.Document.GetElementById("ftpLink");
        string ftpLink = inputElement.GetAttribute("value");
        TabPage tab = BuildOutput(_identifier, ftpLink);
        tabControl.TabPages.Add(tab);
    }

    private void RenderList(List<string> downloadList)
    {
        try
        {
            if (downloadList.Count == 0)
            {
                MessageBox.Show("File list is empty");
                Loading(false);
                return;
            }

            tabControl.TabPages.Clear();
            string downloadListStr = "";
            int count = 0;

            for (var i = 0; i < downloadList.Count; i++)
            {
                count += 1;
                downloadListStr += downloadList[i] + "\r\n";
                if (count % 1000 == 0)
                {
                    TabPage tabPage = BuildOutput((count - 999) + "-" + count, downloadListStr);
                    tabControl.TabPages.Add(tabPage);
                    downloadListStr = "";
                }
            }

            TabPage tabPageLast = BuildOutput((count - count % 1000 + 1) + "~" + count, downloadListStr);
            tabControl.TabPages.Add(tabPageLast);
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }

        Loading(false);
    }

    private TabPage BuildOutput(string name, string tasks)
    {
        TabPage tabPage = new TabPage();
        tabPage.SuspendLayout();
        tabPage.Text = name;

        TextBox tb = new TextBox();
        tb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tb.MaxLength = 1000000;
        tb.Multiline = true;
        tb.Name = "tb" + name;
        tb.ScrollBars = ScrollBars.Both;
        tb.Size = new System.Drawing.Size(922, 413);
        tb.TabIndex = 1;
        tb.Text = tasks;
        tabPage.Controls.Add(tb);
        return tabPage;
    }

    private void Loading(bool load)
    {
        Text = GetUniqueTag() + (load ? " loading" : " loaded");
    }

    private void btnReload_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    private void LoadData()
    {
        Loading(true);
        tabControl.TabPages.Clear();
        if (_web.Equals(Froms.WEB_PX))
        {
            if (_from.ToLower().Equals(Froms.MassIVE))
            {
                _massIvePage = new WebBrowser();
                _massIvePage.ScriptErrorsSuppressed = true;
                _massIvePage.DocumentCompleted += readMassIVEPage_Completed;
            }

            _pxdPage = new WebBrowser();
            _pxdPage.ScriptErrorsSuppressed = true;
            _pxdPage.DocumentCompleted += readPXDPage_Completed;
            ReadPxdPage();
        }
        else if (_web.Equals(Froms.WEB_ML))
        {
            ReadMlFileList();
        }
    }

    private void ReadMlFileList()
    {
        List<string> paths = HttpUtil.FetchFtpFilePaths(UrlConst.mlFtpUrl+_identifier);
        if (paths == null)
        {
            MessageBox.Show("Getting FTP files Error!");
        }
        else
        {
            RenderList(paths);
        }

        Loading(false);
    }

    private void btnListFtpFiles_Click(object sender, EventArgs e)
    {
        if (tbFTP.Text.IsEmpty())
        {
            MessageBox.Show("FTP Link is empty!");
            return;
        }

        List<string> paths = null;
        int count = 3;
        while (paths == null && count > 0)
        {
            count--;
            paths = HttpUtil.FetchFtpFilePaths(tbFTP.Text);
        }
        
        if (paths == null)
        {
            MessageBox.Show("Getting FTP files Error!");
        }
        else
        {
            RenderList(paths);
        }
    }
}