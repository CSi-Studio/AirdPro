using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;

namespace AirdPro.Domains.FileLogs
{
    public class APPLogTest
    {
        private static string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Syslog\";
        private static ILog logm = LogManager.GetLogger("Applog");

        static APPLogTest()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }

        private static void WriteLog(string msg, bool isWrite,Action<object> action)
        {
            if (isWrite)
            {
                string fileName = "Applog_" + DateTime.Now.ToString("yyyyMMdd_HH" + ".log");
                var repository = LogManager.GetRepository();
                var appenders = repository.GetAppenders();
                if (appenders.Length > 0)
                {
                    RollingFileAppender targetAptr = null;
                    foreach (var apdr in appenders)
                    {
                        if (apdr.Name == "Applog")
                        {
                            targetAptr = apdr as RollingFileAppender;
                            break;
                        }
                    }

                    if (targetAptr.Name == "Applog")
                    {

                    }
                }
            }
        }

        private static void WriteError(string msg, bool isWrite)
        {
            WriteLog(msg,isWrite,logm.Error);
        }
        private static void WriteInfo(string msg, bool isWrite)
        {
            WriteLog(msg, isWrite, logm.Info);
        }
        private static void WriteWarn(string msg, bool isWrite)
        {
            WriteLog(msg, isWrite, logm.Warn);
        }
    }
}
