using System.Diagnostics;

namespace AirdProTests
{
    [TestClass]
    public class CompetitorPerformanceTest
    {

        [TestMethod]
        public void TestConsoleWriteLine()
        {
            Console.WriteLine("开搞");
        }
        
        [TestMethod]
        public void BatchConvertFiles_ValidSourceFolder_ReturnsSuccessfulConversion()
        {
            
            // 设置要转换的源文件夹路径
            string sourceFolder = @"E:\msfile";

            // 设置转换后的输出文件夹路径
            string outputFolder = @"E:\msfile_converted\msconvert_mzml";

            // msconvert.exe所在路径
            string msconvertPath = @"E:\MSConvert-mzMLb\";

            // 获取源文件夹下一级的所有文件和文件夹
            string[] filesAndFolders = Directory.GetFileSystemEntries(sourceFolder);

            foreach (string entry in filesAndFolders)
            {
                // // 如果文件名不是以.raw结尾，跳出当前循环继续处理下一个文件
                // if (!entry.EndsWith("1.d", StringComparison.OrdinalIgnoreCase))
                // {
                //     continue;
                // }

                // 如果是文件或以.d或.raw结尾的文件夹，直接执行转换
                if (File.Exists(entry) || entry.EndsWith(".d", StringComparison.OrdinalIgnoreCase) || entry.EndsWith(".raw", StringComparison.OrdinalIgnoreCase))
                {
                    string outputFile = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(entry) + ".mzML");
                    string arguments = $"-o \"{outputFolder}\" --mzML \"{entry}\" --combineIonMobilitySpectra";
                    

                    // ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", $"/K \"{Path.Combine(msconvertPath, "msconvert.exe")}\" {arguments}");

                    ProcessStartInfo processInfo = new ProcessStartInfo(Path.Combine(msconvertPath, "msconvert.exe"), arguments);
                    processInfo.RedirectStandardError = true;
                    processInfo.RedirectStandardOutput = true;
                    processInfo.UseShellExecute = false;
                    
                    Trace.WriteLine(processInfo.Arguments);

                    
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    Process process = Process.Start(processInfo);

                    // 读取输出流
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    Console.WriteLine(output);
                    Console.WriteLine(error);
                    // using (StreamReader reader = process.StandardOutput)
                    // {
                    //     string output = reader.ReadToEnd();
                    //     Console.WriteLine(output); // 将输出写入控制台
                    // }

                    process.WaitForExit();
                    process.Close();

                    stopwatch.Stop();
                    TimeSpan elapsed = stopwatch.Elapsed;

                    FileInfo fileInfo = new FileInfo(outputFile);
                    Console.WriteLine($"文件 {entry} 转换时间: {elapsed.TotalSeconds} 秒, 转换后大小: {fileInfo.Length / 1024.0 / 1024.0} MB");
                }
            }

            Assert.IsTrue(filesAndFolders.Length > 0, "源文件夹及其子文件夹中没有文件需要转换");
        }
    }
}