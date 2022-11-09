using System.Data;
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

        public static void writeToFile(object obj, string outputFilePath)
        {
            var content = JsonConvert.SerializeObject(obj);
            var projectBytes = Encoding.UTF8.GetBytes(content);
            var stream = new FileStream(outputFilePath, FileMode.Create);
            stream.Write(projectBytes, 0, projectBytes.Length);
            stream.Close();
        }
    }
}