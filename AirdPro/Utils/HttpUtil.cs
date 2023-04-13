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
using AirdPro.Repository.ProteomeXchange;

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
            fileRow.fileSizeLabel.Report(FileUtil.getSizeLabel(size));
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
    }
}