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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using AirdPro.Constants;
using Newtonsoft.Json;

namespace AirdPro.Utils
{
    public class AirdProFileUtil
    {
        public static string GetSizeLabel(long size)
        {
            if (size == 0)
            {
                return "0";
            }

            if (size < 1024)
            {
                return size + " Byte";
            }

            if (size < 1024 * 1024)
            {
                return (size / 1024d).ToString("0.00") + "KB";
            }

            if (size < 1024 * 1024 * 1024)
            {
                return (size / 1024d / 1024).ToString("0.00") + "MB";
            }

            return (size / 1024d / 1024 / 1024).ToString("0.00") + "GB";
        }

        public static string ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            using (var fsRead = new FileStream(filePath, FileMode.Open))
            {
                var fsLen = (int)fsRead.Length;
                var heByte = new byte[fsLen];
                var r = fsRead.Read(heByte, 0, heByte.Length);
                var projectJson = Encoding.UTF8.GetString(heByte);
                fsRead.Close();

                return projectJson;
            }
        }

        public static T ReadFromFileAsJson<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default(T);

            using (var fsRead = new FileStream(filePath, FileMode.Open))
            {
                var fsLen = (int)fsRead.Length;
                var heByte = new byte[fsLen];
                var r = fsRead.Read(heByte, 0, heByte.Length);
                var projectJson = Encoding.UTF8.GetString(heByte);
                fsRead.Close();

                return JsonConvert.DeserializeObject<T>(projectJson);
            }
        }

        public static void WriteToFile(object obj, string outputFilePath)
        {
            var content = JsonConvert.SerializeObject(obj);
            var projectBytes = Encoding.UTF8.GetBytes(content);
            var stream = new FileStream(outputFilePath, FileMode.Create);
            stream.Write(projectBytes, 0, projectBytes.Length);
            stream.Close();
        }

        public static long GetDirectorySize(string directory)
        {
            long directorySize = 0;
            DirectoryInfo di = new DirectoryInfo(directory);
            if (!di.Exists)
            {
                return 0;
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                directorySize += fi.Length;
            }

            DirectoryInfo[] dirs = di.GetDirectories();
            foreach (DirectoryInfo sondir in dirs)
            {
                directorySize += GetDirectorySize(sondir.FullName);
            }

            return directorySize;
        }

        /**
         * 循环遍历指定文件夹下的所有质谱文件
         */
        public static List<string> Scan(string folderPath)
        {
            List<string> items = new List<string>();
            string[] dirs = Array.Empty<string>();
            //这个try catch是为了防止访问部分windows文件夹异常时做的容错逻辑
            try
            {
                dirs = Directory.GetDirectories(folderPath);
            }
            catch (Exception e)
            {
                // ignored
            }

            if (dirs.Length > 0)
            {
                foreach (string str in dirs)
                {
                    if (str.ToLower().EndsWith(FileFormat.DotD.ToLower()) ||
                        str.ToLower().EndsWith(FileFormat.DotRAW.ToLower()))
                    {
                        items.Add(str);
                    }
                    else
                    {
                        List<string> files = Scan(str);
                        if (files != null)
                        {
                            items.AddRange(files);
                        }
                    }
                }
            }

            foreach (string str in Directory.GetFiles(folderPath))
            {
                string extension = Path.GetExtension(str);
                if (FileFormat.DotWIFF.ToLower().Equals(extension.ToLower())
                    || FileFormat.DotRAW.ToLower().Equals(extension.ToLower())
                    || FileFormat.DotmzML.ToLower().Equals(extension.ToLower())
                    || FileFormat.DotmzXML.ToLower().Equals(extension.ToLower()))
                {
                    items.Add(str);
                }
            }

            return items;
        }

        public static string ReplaceLast(string input, string pattern, string replacement)
        {
            string output = Regex.Replace(input, pattern, match =>
            {
                if (match.Index == input.LastIndexOf(pattern, StringComparison.Ordinal))
                {
                    return replacement;
                }

                return match.Value;
            }, RegexOptions.IgnoreCase);
            return output;
        }

        public static void CopyFolder(string sourceFolderPath, string destinationFolderPath)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceFolderPath);
            DirectoryInfo destinationDirectory = new DirectoryInfo(destinationFolderPath);

            if (!sourceDirectory.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found.");
            }

            if (!destinationDirectory.Exists)
            {
                destinationDirectory.Create();
            }

            FileInfo[] files = sourceDirectory.GetFiles();

            foreach (FileInfo file in files)
            {
                string destinationFilePath = Path.Combine(destinationFolderPath, file.Name);
                file.CopyTo(destinationFilePath, true);
            }

            DirectoryInfo[] subDirectories = sourceDirectory.GetDirectories();

            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                string destinationSubFolderPath = Path.Combine(destinationFolderPath, subDirectory.Name);
                CopyFolder(subDirectory.FullName, destinationSubFolderPath);
            }
        }

        public static string GetAirdProTempPath()
        {
            return Path.Combine(Path.GetTempPath(), "AirdPro");
        }

        public static void ClearLocalTempFiles()
        {
            try
            {
                string directory = GetAirdProTempPath();
                DeleteAllFilesInFolder(directory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void DeleteAllFilesInFolder(string folderPath)
        {
            // 获取文件夹中的所有文件
            string[] files = Directory.GetFiles(folderPath);

            // 删除每个文件
            foreach (string file in files)
            {
                File.Delete(file);
                Console.WriteLine("File-" + file + "删除成功");
            }

            // 获取文件夹中的所有子文件夹
            string[] subfolders = Directory.GetDirectories(folderPath);

            // 递归删除子文件夹中的所有文件
            foreach (string subfolder in subfolders)
            {
                Directory.Delete(subfolder, true);
            }
        }
    }
}