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
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace AirdPro.Utils
{
    public class FileUtil
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
    }
}