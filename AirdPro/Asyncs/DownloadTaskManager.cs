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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AirdPro.Asyncs;
using AirdPro.Repository;
using AirdPro.Repository.ProteomeXchange;
using AirdPro.Utils;

namespace AirdPro.Async
{
    internal class DownloadTaskManager
    {
        public int downloadBufferSize = 1024 * 200; //200kb

        public TaskFactory fac;

        public DownloadDetailForm detailForm;

        //存放已经完成转换的JobInfo,不管是否转换成功
        public Hashtable finishedTable = new Hashtable();

        public Queue<FileRow> jobQueue = new Queue<FileRow>();

        //存放全部的Job信息,用于根据JobId判定当前的Job是否已经存在
        public Hashtable jobTable = new Hashtable();

        public DownloadTaskManager(DownloadDetailForm detailForm)
        {
            fac = new TaskFactory(new LimitedConcurrencyLevelTaskScheduler(1));
            this.detailForm = detailForm;
        }

        //加入一个新的转换任务,如果该任务已经在转换完毕的列表内,则将其重新放入待转换队列重新转换
        public void pushJob(FileRow job)
        {
            if (finishedTable.ContainsKey(job.remotePath)) finishedTable.Remove(job.remotePath);

            if (!jobTable.Contains(job.remotePath))
            {
                jobQueue.Enqueue(job);
                jobTable.Add(job.remotePath, job);
            }
        }

        //将一个任务置为已完成状态
        public void finishedJob(FileRow job)
        {
            jobTable.Remove(job.remotePath);
            finishedTable.Add(job.remotePath, job);
        }

        //删除一个任务
        public void removeJob(FileRow jobInfo)
        {
            jobInfo.tokenSource.Cancel();
            jobTable.Remove(jobInfo.remotePath);
            finishedTable.Remove(jobInfo.remotePath);
        }

        public void run(HashSet<string> skipFormats)
        {
            while (true)
            {
                //如果队列中没有待执行的任务,那么进行休眠当前进程两秒
                if (jobQueue.Count == 0) return;

                FileRow fileRow = null;
                try
                {
                    fileRow = jobQueue.Dequeue();
                }
                catch
                {
                }

                if (fileRow == null)
                {
                    return;
                }

                fac.StartNew(() =>
                {
                    while (fileRow.retryTimes > 0)
                    {
                        try
                        {
                            download(fileRow, skipFormats);
                            break;
                        }
                        catch (Exception ex)
                        {
                            fileRow.retryTimes--;
                            if (fileRow.retryTimes > 0)
                            {
                                fileRow.status.Report("Retry After 3s:" + ex.Message);
                                Thread.Sleep(3000);
                            }
                            else
                            {
                                fileRow.status.Report("Error:" + ex.Message);
                            }
                        }
                    }

                    finishedJob(fileRow);
                }, fileRow.tokenSource.Token);
            }
        }

        private void download(FileRow currentRow, HashSet<string> skipFormats)
        {
            currentRow.status.Report("Running");
            // HttpUtil.fetchFileSize(currentRow);
            
            foreach (var skipFormat in skipFormats)
            {
                if (currentRow.remotePath.ToLower().EndsWith(skipFormat))
                {
                    currentRow.status.Report("Skip Format");
                    return;
                }
            }
        
            if (currentRow.fileType.Equals("Directory"))
            {
                Directory.CreateDirectory(currentRow.localPath);
                List<string> filePaths = HttpUtil.fetchFtpFilePaths(currentRow.remotePath);
                if (filePaths != null && filePaths.Count > 0)
                {
                    foreach (string filePath in filePaths)
                    {
                        string fileName = Path.GetFileName(filePath);
                        FileRow row = detailForm.buildItem(
                            Path.Combine(detailForm.remoteUrl, currentRow.parent, currentRow.fileName, fileName)
                                .Replace("\\", "/"),
                            Path.Combine(detailForm.localDirectory, currentRow.parent, currentRow.fileName, fileName)
                                .Replace("/", "\\"),
                            Path.Combine(currentRow.parent, currentRow.fileName));
                        download(row, skipFormats);
                    }
                    currentRow.status.Report("Finished");
                }
            }
            else
            {
                if (currentRow.remotePath.ToLower().StartsWith("ftp"))
                {
                    downloadAsFTP(currentRow);
                }
                else if (currentRow.remotePath.ToLower().StartsWith("http"))
                {
                    downloadAsHttp(currentRow);
                }
            }
        }

        private void downloadAsFTP(FileRow fileRow)
        {
            FileStream outputStream = null;
            FtpWebResponse ftpResponse = null;
            Stream ftpStream = null;
            try
            {
                //比对本地文件
                var fileInfo = new FileInfo(fileRow.localPath);
                if (fileInfo.Exists && fileInfo.Length.Equals(fileRow.fileSize))
                {
                    fileRow.status.Report("已下载");
                    return;
                }

                //开始下载
                outputStream = new FileStream(fileRow.localPath, FileMode.OpenOrCreate);
                outputStream.SetLength(0);
                var ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(fileRow.remotePath));
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                ftpRequest.UseBinary = true;
                ftpRequest.KeepAlive = false;
                ftpRequest.Timeout = 3000;
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpStream = ftpResponse.GetResponseStream();
                var bufferSize = downloadBufferSize;
                int readCount;
                var ftpFileReadSize = 0;
                var buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    ftpFileReadSize += readCount;
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    fileRow.status.Report((ftpFileReadSize * 1.0d / fileRow.fileSize * 100).ToString("0.00") + "%");
                }

                fileRow.status.Report("Finished");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("FtpClient Exception：" + ex.Message);
                fileRow.status.Report("Exception:" + ex.Message);
            }
            finally
            {
                if (ftpStream != null) ftpStream.Close();
                if (outputStream != null) outputStream.Close();
                if (ftpResponse != null) ftpResponse.Close();
            }
        }

        private void downloadAsHttp(FileRow fileRow)
        {
            FileStream outputStream = null;
            HttpWebResponse httpResponse = null;
            Stream httpStream = null;
            try
            {
                //比对本地文件
                var fileInfo = new FileInfo(fileRow.localPath);
                if (fileInfo.Exists && fileInfo.Length.Equals(fileRow.fileSize))
                {
                    fileRow.status.Report("Finished");
                    return;
                }

                //开始下载
                outputStream = new FileStream(fileRow.localPath, FileMode.Create);
                var httpRequest = (HttpWebRequest)WebRequest.Create(new Uri(fileRow.remotePath));
                httpRequest.Method = WebRequestMethods.Http.Get;
                httpRequest.KeepAlive = false;
                httpRequest.Timeout = 3000;
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                httpStream = httpResponse.GetResponseStream();
                var bufferSize = downloadBufferSize;
                int readCount;
                var ftpFileReadSize = 0;
                var buffer = new byte[bufferSize];
                readCount = httpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    ftpFileReadSize += readCount;
                    outputStream.Write(buffer, 0, readCount);
                    readCount = httpStream.Read(buffer, 0, bufferSize);
                    fileRow.status.Report((ftpFileReadSize * 1.0d / fileRow.fileSize * 100).ToString("0.00") + "%");
                }

                fileRow.status.Report("Finished");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("HttpClient Exception：" + ex.Message);
                fileRow.status.Report("Exception:" + ex.Message);
            }
            finally
            {
                if (httpStream != null) httpStream.Close();
                if (outputStream != null) outputStream.Close();

                if (httpResponse != null) httpResponse.Close();
            }
        }

        public void clear()
        {
            jobQueue.Clear();
            jobTable.Clear();
        }

        public void close()
        {
            foreach (DictionaryEntry entry in jobTable)
            {
                FileRow row = entry.Value as FileRow;
                if (row.tokenSource.IsCancellationRequested)
                {
                    row.tokenSource.Cancel();
                }
            }
        }
    }
}