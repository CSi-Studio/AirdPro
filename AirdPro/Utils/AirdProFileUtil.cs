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
        public static string getSizeLabel(long size)
        {
            if (size == 0)
            {
                return "文件夹";
            }

            if (size < 1024)
            {
                return size + " Byte";
            }
            else if (size >= 1024 && size < 1024 * 1024)
            {
                return (size / 1024d).ToString("0.00") + "KB";
            }
            else if (size >= 1024 * 1024 && size < 1024 * 1024 * 1024)
            {
                return (size / 1024d / 1024).ToString("0.00") + "MB";
            }
            else
            {
                return (size / 1024d / 1024 / 1024).ToString("0.00") + "GB";
            }
        }

        public static string readFromFile(string filePath)
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

        public static T readFromFileAsJSON<T>(string filePath)
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

        public static void writeToFile(object obj, string outputFilePath)
        {
            var content = JsonConvert.SerializeObject(obj);
            var projectBytes = Encoding.UTF8.GetBytes(content);
            var stream = new FileStream(outputFilePath, FileMode.Create);
            stream.Write(projectBytes, 0, projectBytes.Length);
            stream.Close();
        }

        public static long getDirectorySize(string directory)
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
                directorySize += getDirectorySize(sondir.FullName);
            }

            return directorySize;
        }
        
        /**
         * 循环遍历指定文件夹下的所有质谱文件
         */
        public static List<string> scan(string folderPath)
        {
            List<string> items = new List<string>();
            string[] dirs = new string[0];
            try
            {
                dirs = Directory.GetDirectories(folderPath);
            }
            catch (Exception e)
            {
                return null;
            }

            if (dirs.Length == 0)
            {
                return null;
            }
            
            foreach (string str in dirs)
            {
                if (str.ToLower().EndsWith(FileFormat.DotD.ToLower()) ||
                    str.ToLower().EndsWith(FileFormat.DotRAW.ToLower()))
                {
                    items.Add(str);
                }
                else
                {
                    List<string> children = scan(str);
                    if (children != null)
                    {
                        items.AddRange(children);
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

        public static string replaceLast(string input, string pattern, string replacement)
        {
            string output = Regex.Replace(input, pattern, match => {  
                if (match.Index == input.LastIndexOf(pattern)) {  
                    return replacement;  
                }  
                return match.Value;  
            }, RegexOptions.IgnoreCase);
            return output;
        }
        
    }
}