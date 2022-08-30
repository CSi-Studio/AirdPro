using System;
using System.IO;

namespace AirdPro.Utils;

public class FileUtil
{
    /**
    * read all the string in the target file
    *
    * @param file 文件对象
    * @return content in the file
    */
    public static string readFile(FileInfo file)
    {
        string content = File.ReadAllText(file.FullName);
    }
}