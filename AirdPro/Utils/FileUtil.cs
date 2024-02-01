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
}