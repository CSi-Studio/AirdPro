using System;
using System.IO;
using System.Linq;

namespace AirdPro.Utils
{
    internal class FileNameUtil
    {
        public static string buildOutputFileName(string inputFilePath)
        {
            var outputFileName = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = Path.GetFileNameWithoutExtension(inputFilePath) ?? string.Empty;

                // this list is for Windows; it's a superset of the POSIX list
                const string illegalFilename = "\\/*:?<>|\"";
                foreach (var t in illegalFilename)
                {
                    if (outputFileName.Contains(t))
                    {
                        outputFileName = outputFileName.Replace(t, '_');
                    }   
                }
                
                string newFilename = outputFileName;
//                var fullPath = Path.Combine(outputPath, newFilename);
                return newFilename;
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(
                    string.Format("error generating output filename for input file '{0}' and output run id '{1}'",
                        inputFilePath, outputFileName), e);
            }
        }
    }
}