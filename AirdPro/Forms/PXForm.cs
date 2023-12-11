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
using AirdPro.Forms;
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

            string content = AirdProFileUtil.readFromFile(Path.Combine(configFolder, configFileName));
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
                AirdProFileUtil.writeToFile(projects, Path.Combine(configFolder, configFileName));
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
            if (projectsTable.Rows == null)
            {
                return;
            }

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

        private void btnDownloadLinks_Click(object sender, EventArgs e)
        {
            if (projectListView.SelectedCells.Count > 0)
            {
                var index = projectListView.SelectedCells[0].RowIndex;
                var identifier = projectListView.Rows[index].Cells[0].Value.ToString();
                var repo = projectListView.Rows[index].Cells[2].Value.ToString();
                new DownloadLinksForm(Froms.WEB_PX,repo, identifier).ShowDialog();
            }
            else
            {
                MessageBox.Show("Select One Row First");
            }
        }
    }
}