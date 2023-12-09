using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Timers;
using System.Windows.Forms;
using AirdPro.Algorithms.Parser.DownloadXML;
using AirdPro.Constants;
using AirdPro.Utils;
using FluentFTP;
using System.Threading.Tasks;

namespace AirdPro.Forms;

public partial class DownloadLinksForm : Form
{
    public string from;
    public string identifier;
    public string PXDUrl = "https://proteomecentral.proteomexchange.org/cgi/GetDataset?ID=";

    public WebBrowser pxdPage;
    public WebBrowser massIVEPage;

    //px datasets
    public static string MassIVE = "massive";
    public static string JPost = "jpost";
    public static string PRIDE = "pride";
    public static string IProX = "iprox";
    public static string[] pxList = new[] { MassIVE, JPost, PRIDE, IProX };

    //metabolights datasets
    public static string Metabolights = "metabolights";

    public string getUniqueTag()
    {
        return from + ":" + identifier;
    }

    public DownloadLinksForm(string from, string identifier)
    {
        InitializeComponent();
        this.from = from;
        this.identifier = identifier;
    }

    //所有的链接均从PXD页面开始路由
    public void readPXDPage()
    {
        Text = getUniqueTag() + " loading";
        pxdPage.Navigate(PXDUrl + identifier);
    }

    public async void readPXDPage_Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        WebBrowser web = (WebBrowser)sender;
        HtmlElementCollection elements = web.Document.GetElementsByTagName("table");
        if (elements.Count < 8)
        {
            loading(false);
            return;
        }
        //路由，不同的源需要解码PXD详情页上不同的元素
        switch (from.ToLower())
        {
            case "pride":
            case "iprox":
                HtmlElement announcementXMLElement = elements[1].Children[0].Children[4].Children[1].Children[0]; //直接定位到<a>标签
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
                                renderList(downloadList);
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
                TabPage tab = buildOutput("1-1", jpostHref + array[array.Length - 2] + "_all.zip");
                tabControl.TabPages.Add(tab);
                lblTips.Text = "Use FileZilla or other FTP tools to download the following files";
                break;
            case "massive":
                HtmlElement massIVEElement = elements[6].Children[0].Children[0].Children[0].Children[0]; //直接定位到<a>标签
                string massIVEHref = massIVEElement.GetAttribute("href");
                massIVEPage.Navigate(massIVEHref);
                lblTips.Text = "Use FileZilla or other FTP tools to download the following files";
                break;

        }
    }

    public void readMassIVEPage_Completed(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        WebBrowser web = (WebBrowser)sender;
        HtmlElement inputElement = web.Document.GetElementById("ftpLink");
        string ftpLink = inputElement.GetAttribute("value");
        TabPage tab = buildOutput(identifier, ftpLink);
        tabControl.TabPages.Add(tab);
    }

    public void renderList(List<string> downloadList)
    {
        try
        {
            if (downloadList.Count == 0)
            {
                MessageBox.Show("Content is Empty");
                loading(false);
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
                    TabPage tabPage = buildOutput((count - 999) + "-" + count, downloadListStr);
                    tabControl.TabPages.Add(tabPage);
                    downloadListStr = "";
                }
            }

            TabPage tabPageLast = buildOutput((count - count % 1000 + 1) + "~" + count, downloadListStr);
            tabControl.TabPages.Add(tabPageLast);
        }
        catch (Exception ee)
        {
            Console.WriteLine(ee.Message);
        }
        loading(false);
    }

    public TabPage buildOutput(string name, string tasks)
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

    public void loading(bool load)
    {
        Text = getUniqueTag() + (load ? " loading" : " loaded");
    }
    private void btnReload_Click(object sender, EventArgs e)
    {
        loading(true);
        tabControl.TabPages.Clear();
        loadData();
    }

    private void DownloadLinksForm_Load(object sender, EventArgs e)
    {
    }

    public void loadData()
    {
        //忽略界面的脚本操作
        if (from.ToLower().Equals(MassIVE))
        {
            massIVEPage = new WebBrowser();
            massIVEPage.ScriptErrorsSuppressed = true;
            massIVEPage.DocumentCompleted += readMassIVEPage_Completed;
        }

        if (pxList.Contains(from.ToLower()))
        {
            pxdPage = new WebBrowser();
            pxdPage.ScriptErrorsSuppressed = true;
            pxdPage.DocumentCompleted += readPXDPage_Completed;
            readPXDPage();
        }

        if (from.ToLower().Equals(Metabolights))
        {
            readMLFileList();
        }
    }

    public void readMLFileList()
    {
        List<string> paths = HttpUtil.fetchFtpFilePaths(UrlConst.mlFtpUrl, identifier);
        if(paths == null)
        {
            MessageBox.Show("读取FTP文件列表异常");
        }
        else
        {
            renderList(paths);
        }
        
        loading(false);
    }
}