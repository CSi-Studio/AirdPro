/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Properties;
using AirdPro.Repository.ProteomeXchange;
using AirdPro.Utils;
using HtmlAgilityPack;
using Newtonsoft.Json;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace AirdPro.Repository
{
    public partial class PXForm : Form
    {
        public static string configFileName = "PXConfigFile.json";

        private static readonly HttpClient client = new HttpClient();
        public DataTable projectsTable;
        public DataTable searchProjectsTable = new DataTable();

        public PXForm()
        {
            InitializeComponent();
            load();
        }

        private void load()
        {
            var configFolder = Settings.Default.ConfigFolder;
            if (configFolder.Equals(string.Empty) || !Directory.Exists(configFolder)) return;

            string content = FileUtil.readFromFile(Path.Combine(configFolder, configFileName));
            if (content == null)
            {
                return;
            }

            projectsTable = JsonConvert.DeserializeObject<DataTable>(content);
            searchProjectsTable = projectsTable.Clone();
            searchProjectsTable.Clear();

            setDataSource(projectsTable);
        }

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            btnLoad.Text = "Loading";
            var configFolder = Settings.Default.ConfigFolder;
            if (configFolder.Equals(string.Empty) || !Directory.Exists(configFolder))
            {
                MessageBox.Show("Config File not Exists");
                btnLoad.Text = "Load From Web";
                return;
            }

            string response = null;
            try
            {
                response = await client.GetStringAsync(UrlConst.pxListUrl);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Req Failed:" + exception.Message);
                btnLoad.Text = "Load From Web";
                return;
            }

            btnLoad.Text = "Start Parsing";
            var document = new HtmlDocument();
            document.LoadHtml(response);
            var tableNode = document.GetElementbyId("datatable");
            Console.WriteLine(tableNode.ChildNodes.Count);
            HtmlNode tbody = null;
            foreach (var node in tableNode.ChildNodes)
            {
                if (node.Name.Equals("tbody"))
                {
                    tbody = node;
                    break;
                }
            }

            var projects = new List<Project>();
            foreach (var trNode in tbody.ChildNodes)
            {
                if (trNode.Name.Equals("tr"))
                {
                    var pxProject = new Project();
                    var fields = new List<string>();
                    foreach (var tdNode in trNode.ChildNodes)
                    {
                        if (tdNode.Name.Equals("td"))
                        {
                            fields.Add(tdNode.InnerText);
                        }
                    }

                    if (fields.Count == 9)
                    {
                        pxProject.Identifier = fields[0];
                        pxProject.Title = fields[1];
                        pxProject.Repos = fields[2];
                        pxProject.Species = fields[3];
                        pxProject.Instrument = fields[4];
                        pxProject.Publication = fields[5];
                        pxProject.LabHead = fields[6];
                        pxProject.Announce = fields[7];
                        pxProject.Keywords = fields[8];

                        projects.Add(pxProject);
                    }
                    else
                    {
                        MessageBox.Show("Web API Exception!");
                        break;
                    }
                }
            }

            if (projects.Count != 0)
            {
                FileUtil.writeToFile(projects, Path.Combine(configFolder, configFileName));
                load();
            }

            btnLoad.Text = "Load From Web";
        }


        private void mainForm_Load(object sender, EventArgs e)
        {
            var configFolder = Settings.Default.ConfigFolder;
            if (configFolder.Equals(string.Empty))
            {
                var temp = Path.GetTempPath();
                temp = Path.Combine(temp, "AirdPro");
                if (!Directory.Exists(temp))
                {
                    Directory.CreateDirectory(temp);
                    Settings.Default.ConfigFolder = temp;
                    Settings.Default.Save();
                }
            }

            tbConfigFolder.Text = Settings.Default.ConfigFolder;
            tbRepos.Text = Settings.Default.PXReposFolder;
        }

        private void btnChangeConfigFolder_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Settings.Default.ConfigFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.ConfigFolder = dialog.SelectedPath;
                Settings.Default.Save();
                tbConfigFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnChangeRepos_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Settings.Default.PXReposFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.PXReposFolder = dialog.SelectedPath;
                Settings.Default.Save();
                tbRepos.Text = dialog.SelectedPath;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            search();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            setDataSource(projectsTable);
        }

        public void setDataSource(DataTable table)
        {
            projectListView.DataSource = table;
            lblResult.Text = table.Rows.Count + " Record(s)";
        }

        private async void btnDetail_Click(object sender, EventArgs e)
        {
            var repos = Settings.Default.PXReposFolder;
            if (repos.Equals(string.Empty) || !Directory.Exists(repos))
            {
                MessageBox.Show("Local Repo not Exists");
                return;
            }

            lblLoading.Text = "Loading";
            btnDetail.Enabled = false;
            //确认是否选中了某一行的数据
            if (projectListView.SelectedCells.Count > 0)
            {
                var index = projectListView.SelectedCells[0].RowIndex;
                //获取仓库ID
                string identifier = projectListView.Rows[index].Cells[0].Value.ToString();


                //检查本地仓库是否存在对应的文件夹
                string localDirectory = Path.Combine(Settings.Default.PXReposFolder, identifier);
                if (!Directory.Exists(localDirectory))
                {
                    //本地建仓
                    Directory.CreateDirectory(localDirectory);
                }

                List<string> remotes = new List<string>();
                List<string> locals = new List<string>();
                string remoteUrl = "";
                string homeUrl = "";
                try
                {
                    string response = await client.GetStringAsync(UrlConst.pxDetailJsonUrl + identifier);
                    RootProject rootProject = JsonConvert.DeserializeObject<RootProject>(response);
                    if (rootProject.datasetFiles != null && rootProject.datasetFiles.Count > 0)
                    {
                        foreach (Node fileNode in rootProject.datasetFiles)
                        {
                            remotes.Add(fileNode.value);
                            locals.Add(Path.Combine(localDirectory, Path.GetFileName(fileNode.value)));
                        }
                    }
                    else if (rootProject.fullDatasetLinks != null && rootProject.fullDatasetLinks.Count > 0)
                    {
                        //用于获取FTP文件夹根目录
                        foreach (Node link in rootProject.fullDatasetLinks)
                        {
                            if (link.accession.Equals("MS:1002852")) // Dataset FTP location
                            {
                                List<string> filePathList = HttpUtil.fetchFtpFilePaths(link.value);
                                foreach (string filePath in filePathList)
                                {
                                    string fileName = Path.GetFileName(filePath);
                                    remotes.Add(Path.Combine(link.value, fileName));
                                    locals.Add(Path.Combine(localDirectory, fileName));
                                }
                            }
                        }
                    }

                    if (rootProject.fullDatasetLinks != null && rootProject.fullDatasetLinks.Count > 0)
                    {
                        //用于获取FTP文件夹根目录
                        foreach (Node link in rootProject.fullDatasetLinks)
                        {
                            if (link.accession.Equals("MS:1002633"))
                            {
                                homeUrl = link.value;
                            }
                            else if (link.accession.Equals("MS:1002837"))
                            {
                                homeUrl = link.value;
                            }
                            else if (link.accession.Equals("MS:1002488"))
                            {
                                homeUrl = link.value;
                            }
                            else
                            {
                                homeUrl = UrlConst.pxDetailUrl + identifier;
                            }

                            if (link.accession.Equals("MS:1002852"))
                            {
                                remoteUrl = link.value;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Get " + identifier + " Failed," + exception.Message);
                }

                lblLoading.Text = "Loaded";
                btnDetail.Enabled = true;
                DownloadDetailForm asyncForm =
                    new DownloadDetailForm(identifier, homeUrl, remoteUrl, localDirectory, remotes, locals);
                asyncForm.Show();
            }
            else
            {
                MessageBox.Show("Select One Row First");
            }
        }

        private void btnUrl_Click(object sender, EventArgs e)
        {
            if (projectListView.SelectedCells.Count > 0)
            {
                var index = projectListView.SelectedCells[0].RowIndex;
                var identifier = projectListView.Rows[index].Cells[0].Value.ToString();
                Process.Start(UrlConst.pxDetailUrl + identifier);
            }
            else
            {
                MessageBox.Show("Select One Row First");
            }
        }

        private void search()
        {
            if (projectsTable.Rows.Count != 0)
            {
                searchProjectsTable.Clear();
                for (var i = 0; i < projectsTable.Rows.Count; i++)
                {
                    foreach (var obj in projectsTable.Rows[i].ItemArray)
                    {
                        if (obj.ToString().ToLower().Contains(tbSearch.Text.ToLower()))
                        {
                            searchProjectsTable.ImportRow(projectsTable.Rows[i]);
                            break;
                        }
                    }
                }

                setDataSource(searchProjectsTable);
            }
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                search();
            }
        }

        private async void btnDirectOpen_Click(object sender, EventArgs e)
        {
            var repos = Settings.Default.PXReposFolder;
            if (repos.Equals(string.Empty) || !Directory.Exists(repos))
            {
                MessageBox.Show("Local Repo not Exists");
                return;
            }

            lblLoading.Text = "Loading";
            btnDirectOpen.Enabled = false;

            //获取仓库ID
            string identifier = tbPXD.Text;
            if (identifier == null || identifier == String.Empty)
            {
                MessageBox.Show("PXD ID cannot be empty!");
                return;
            }

            //检查本地仓库是否存在对应的文件夹
            string localDirectory = Path.Combine(Settings.Default.PXReposFolder, identifier);
            if (!Directory.Exists(localDirectory))
            {
                //本地建仓
                Directory.CreateDirectory(localDirectory);
            }

            List<string> remotes = new List<string>();
            List<string> locals = new List<string>();
            string remoteUrl = "";
            string homeUrl = "";
            try
            {
                string response = await client.GetStringAsync(UrlConst.pxDetailJsonUrl + identifier);
                RootProject rootProject = JsonConvert.DeserializeObject<RootProject>(response);
                if (rootProject.datasetFiles != null && rootProject.datasetFiles.Count > 0)
                {
                    foreach (Node fileNode in rootProject.datasetFiles)
                    {
                        remotes.Add(fileNode.value);
                        locals.Add(Path.Combine(localDirectory, Path.GetFileName(fileNode.value)));
                    }
                }
                else if (rootProject.fullDatasetLinks != null && rootProject.fullDatasetLinks.Count > 0)
                {
                    //用于获取FTP文件夹根目录
                    foreach (Node link in rootProject.fullDatasetLinks)
                    {
                        if (link.accession.Equals("MS:1002852")) // Dataset FTP location
                        {
                            List<string> filePathList = HttpUtil.fetchFtpFilePaths(link.value);
                            foreach (string filePath in filePathList)
                            {
                                string fileName = Path.GetFileName(filePath);
                                remotes.Add(Path.Combine(link.value, fileName));
                                locals.Add(Path.Combine(localDirectory, fileName));
                            }
                        }
                    }
                }

                if (rootProject.fullDatasetLinks != null && rootProject.fullDatasetLinks.Count > 0)
                {
                    //用于获取FTP文件夹根目录
                    foreach (Node link in rootProject.fullDatasetLinks)
                    {
                        if (link.accession.Equals("MS:1002633"))
                        {
                            homeUrl = link.value;
                        }
                        else if (link.accession.Equals("MS:1002837"))
                        {
                            homeUrl = link.value;
                        }
                        else if (link.accession.Equals("MS:1002488"))
                        {
                            homeUrl = link.value;
                        }
                        else
                        {
                            homeUrl = UrlConst.pxDetailUrl + identifier;
                        }

                        if (link.accession.Equals("MS:1002852"))
                        {
                            remoteUrl = link.value;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Get " + identifier + " Failed," + exception.Message);
            }

            lblLoading.Text = "Loaded";
            btnDetail.Enabled = true;
            DownloadDetailForm asyncForm =
                new DownloadDetailForm(identifier, homeUrl, remoteUrl, localDirectory, remotes, locals);
            asyncForm.Show();
        }
    }
}