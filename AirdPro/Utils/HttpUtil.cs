/*
 * Copyright (c) 2020 CSi Studio
 * AirdSDK and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AirdPro.Constants;
using AirdPro.Repository.ProteomeXchange;
using FluentFTP;

namespace AirdPro.Utils
{
    public class HttpUtil
    {
        public static void fetchFileSize(FileRow fileRow)
        {
            if (fileRow.remotePath == null || fileRow.remotePath.Equals(String.Empty))
            {
                return;
            }

            long size = 0;
            if (fileRow.remotePath.StartsWith("ftp"))
            {
                size = fetchFtpFileSize(fileRow.remotePath);
            }
            else if (fileRow.remotePath.StartsWith("http"))
            {
                size = fetchHttpFileSize(fileRow.remotePath);
            }

            fileRow.fileSize = size;
            fileRow.fileSizeLabel.Report(AirdProFileUtil.getSizeLabel(size));
        }

        public static FtpListItem[] getFtpFilesFromMetaboLights(FtpClient client, string remoteDir)
        {
            //用于获取FTP文件夹根目录
            FtpListItem[] items = null;
            try
            {
                items = client.GetListing(remoteDir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return items;
        }

        public static List<string> fetchFtpFilePaths(string remoteDirectory)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(remoteDirectory));
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse ftpResponse = ftpRequest.GetResponse();
                StreamReader reader = new StreamReader(ftpResponse.GetResponseStream());

                List<string> files = new List<string>();
                while (true)
                {
                    string fileName = reader.ReadLine();
                    if (fileName == null)
                    {
                        break;
                    }

                    files.Add(fileName);
                }

                return files;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取" + remoteDirectory + "下文件列表失败:" + ex.Message);
                return null;
            }
        }

        public static long fetchHttpFileSize(string httpPath)
        {
            HttpWebResponse httpResponse = null;
            long size = 0;
            try
            {
                var reqSize = (HttpWebRequest)WebRequest.Create(new Uri(httpPath));
                reqSize.Method = WebRequestMethods.Http.Head;
                httpResponse = (HttpWebResponse)reqSize.GetResponse();
                size = httpResponse.ContentLength;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (httpResponse != null)
                {
                    httpResponse.Close();
                }
            }

            return size;
        }

        public static long fetchFtpFileSize(string ftpPath)
        {
            FtpWebResponse ftpResponse = null;
            long size = 0;
            try
            {
                var reqSize = (FtpWebRequest)WebRequest.Create(new Uri(ftpPath));
                //首先读取文件总大小
                reqSize.Method = WebRequestMethods.Ftp.GetFileSize;
                ftpResponse = (FtpWebResponse)reqSize.GetResponse();
                size = ftpResponse.ContentLength;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (ftpResponse != null)
                {
                    ftpResponse.Close();
                }
            }

            return size;
        }

        public static async Task<string> getResponse(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 发送GET请求并获取响应
                    HttpResponseMessage response = await client.GetAsync(url);
                    // 检查响应是否成功
                    response.EnsureSuccessStatusCode();
                    // 读取响应内容作为数据流
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var reader = new System.IO.StreamReader(stream))
                        {
                            string responseData = await reader.ReadToEndAsync();
                            return responseData;
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Http Request Error: {ex.Message}");
                }
            }

            return null;
        }
    }
}