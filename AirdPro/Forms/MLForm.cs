﻿/*
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
using System.Text;
using System.Windows.Forms;
using AirdPro.Constants;
using AirdPro.Properties;
using AirdPro.Repository.MetaboLights;
using AirdPro.Repository.ProteomeXchange;
using AirdPro.Utils;
using Newtonsoft.Json;

namespace AirdPro.Repository
{
    public partial class MLForm : Form
    {
        public static string configFileName = "MLConfigFile.json";
        public static string fastConfigFileName = "MLFastConfigFile.json";
        
        private static readonly HttpClient client = new HttpClient();
        public DataTable projectsTable;
        public DataTable searchProjectsTable = new DataTable();

        public MLForm()
        {
            InitializeComponent();
            load(true);
        }

        private void load(bool fastLoad)
        {
            var configFolder = Settings.Default.ConfigFolder;
            if (configFolder.Equals(string.Empty) || !Directory.Exists(configFolder)) return;

            var configFilePath = fastLoad? Path.Combine(configFolder, fastConfigFileName): Path.Combine(configFolder, configFileName);
            if (!File.Exists(configFilePath)) return;
            
            using (var fsRead = new FileStream(configFilePath, FileMode.Open))
            {
                var fsLen = (int)fsRead.Length;
                var heByte = new byte[fsLen];
                var r = fsRead.Read(heByte, 0, heByte.Length);
                var projectJson = Encoding.UTF8.GetString(heByte);
                fsRead.Close();
                projectsTable = JsonConvert.DeserializeObject<DataTable>(projectJson);
                searchProjectsTable = projectsTable.Clone();
                searchProjectsTable.Clear();

                setDataSource(projectsTable);
            }
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
                response = await client.GetStringAsync(UrlConst.mlListUrl);
                btnLoad.Text = "Start Parsing";
                StudySummary studies = JsonConvert.DeserializeObject<StudySummary>(response);
                
                long total = studies.studies;
                lblLoading.Text = "0/"+total+ " Loaded";
                List<Project> projects = new List<Project>();
                for (int i = 0; i < studies.content.Count; i++)
                {
                    response = await client.GetStringAsync(UrlConst.mlListUrl +"/" + studies.content[i]);
                    MetaboLights.Repository repository = JsonConvert.DeserializeObject<MetaboLights.Repository>(response);
                    Study study = repository.isaInvestigation.studies[0];
                    Project project = new Project();
                    project.Title = study.title;
                    project.Identifier = study.identifier;
                    project.Announce = study.publicReleaseDate;
                    if (study.publications != null && study.publications.Count > 0)
                    {
                        project.Publication = study.publications[0].title + ". DOI:" + study.publications[0].doi;
                    }

                    project.Repos = "MetaboLights";
                    projects.Add(project);
                    lblLoading.Text = (i + 1)+"/" + total + " Loaded";
                }

                FileUtil.writeToFile(projects, Path.Combine(configFolder, configFileName));
                load(false);
            }
            catch (Exception exception)
            {
                lblLoading.Text = "Req Failed:" + exception.Message;
            }
            finally
            {
                btnLoad.Text = "Load From Web";
            }
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
            tbRepos.Text = Settings.Default.MLReposFolder;
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
            dialog.SelectedPath = Settings.Default.MLReposFolder;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.MLReposFolder = dialog.SelectedPath;
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
            var repos = Settings.Default.MLReposFolder;
            if (repos.Equals(string.Empty) || !Directory.Exists(repos))
            {
                MessageBox.Show("Local Repo not Exists");
                return;
            }

           
            if (projectListView.SelectedCells.Count > 0)
            {
                var index = projectListView.SelectedCells[0].RowIndex;
                var identifier = projectListView.Rows[index].Cells[0].Value.ToString();
                //检查本地仓库是否存在对应的文件夹
                string localDirectory = Path.Combine(Settings.Default.MLReposFolder, identifier);
                if (!Directory.Exists(localDirectory))
                {
                    //本地建仓
                    Directory.CreateDirectory(localDirectory);
                }

                btnDetail.Enabled = false;
                List<string> remotes = new List<string>();
                List<string> locals = new List<string>();
                try
                {
                    string remoteUrl = UrlConst.mlFtpUrl + identifier;
                    //用于获取FTP文件夹根目录
                    List<string> filePathList = HttpUtil.fetchFtpFilePaths(remoteUrl);
                    foreach (string filePath in filePathList)
                    {
                        string fileName = Path.GetFileName(filePath);
                        remotes.Add(Path.Combine(remoteUrl, fileName));
                        locals.Add(Path.Combine(localDirectory, fileName));
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Get " + identifier + " Failed," + exception.Message);
                }

                DownloadDetailForm detailForm = new DownloadDetailForm(identifier,Path.Combine(UrlConst.mlDetailUrl, identifier),Path.Combine(UrlConst.mlFtpUrl, identifier) + "/", localDirectory, remotes, locals);
                detailForm.Show();
                lblLoading.Text = "Loaded";
                btnDetail.Enabled = true;
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
                Process.Start(UrlConst.mlDetailUrl + identifier);
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

        private async void btnFastLoad_Click(object sender, EventArgs e)
        {
            btnFastLoad.Text = "Loading";
            var configFolder = Settings.Default.ConfigFolder;
            if (configFolder.Equals(string.Empty) || !Directory.Exists(configFolder))
            {
                MessageBox.Show("Config File not Exists");
                btnFastLoad.Text = "Load From Web";
                return;
            }

            string response = null;
            try
            {
                response = await client.GetStringAsync(UrlConst.mlListUrl);
                btnFastLoad.Text = "Start Parsing";
                StudySummary studies = JsonConvert.DeserializeObject<StudySummary>(response);

                long total = studies.studies;
                lblLoading.Text = "0/" + total + " Loaded";
                List<Project> projects = new List<Project>();
                for (int i = 0; i < studies.content.Count; i++)
                {
                    Project project = new Project();
                    
                    project.Identifier = studies.content[i];
                    project.Repos = "MetaboLights";
                    projects.Add(project);
                    lblLoading.Text = (i + 1) + "/" + total + " Loaded";
                }
                FileUtil.writeToFile(projects, Path.Combine(configFolder, fastConfigFileName));
                load(true);
            }
            catch (Exception exception)
            {
                lblLoading.Text = "Req Failed:" + exception.Message;
            }
            finally
            {
                btnFastLoad.Text = "Load From Web";
            }
        }
    }
}