using System.Text.RegularExpressions;

namespace AirdPro.csimzMLParser.util
{
    public static class XMLHelper
    {
        public static string EnsureSafeXML(string input)
        {
            if (input == null)
                return string.Empty;

            // 使用正则表达式替换需要转义的字符
            string output = Regex.Replace(input, "&", "&amp;");
            output = Regex.Replace(output, "<", "&lt;");
            output = Regex.Replace(output, ">", "&gt;");
            output = Regex.Replace(output, "\"", "&quot;");

            return output;
        }
    }
}
