using System;
using System.IO;

namespace AirdPro.Utils;

public class FileUtil
{
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
        return Path.Combine(Path.GetTempPath(),"AirdPro");
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

    static void DeleteAllFilesInFolder(string folderPath)
    {
        // 获取文件夹中的所有文件
        string[] files = Directory.GetFiles(folderPath);

        // 删除每个文件
        foreach (string file in files)
        {
            File.Delete(file);
            Console.WriteLine("File-"+file+"删除成功");
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