using System.Drawing;
using System.IO;
using System.Reflection;

namespace AirdPro.Utils;

public class ResourceUtil
{
    public static string resourceSpace = "AirdPro.Resources.";

    public static Image ReadImage(string resourceName)
    {
        resourceName = resourceSpace + resourceName;
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream stream = assembly.GetManifestResourceStream(resourceName);
        return Image.FromStream(stream);
    }

    public static byte[] ReadBytes(string resourceName)
    {
        resourceName = resourceSpace + resourceName;
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream stream = assembly.GetManifestResourceStream(resourceName);
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}