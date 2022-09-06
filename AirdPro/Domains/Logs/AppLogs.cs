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
using log4net;
using log4net.Appender;

namespace AirdPro.Domains
{
    public class AppLogs
    {
        private static string filepath = AppDomain.CurrentDomain.BaseDirectory + @"\SysLog\";

        //申明一个封装类ILog 的对象
        private static readonly ILog logComm = LogManager.GetLogger("AppLog");

        static AppLogs()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
        }

        /// <summary>
        /// 输出系统日志
        /// </summary>
        /// <param name="msg">信息内容</param>
        private static void WriteLog(string msg, bool isWrite, Action<object> action)
        {
            if (isWrite)
            {
                string filename = "AppLog_" + DateTime.Now.ToString("yyyy_MM_dd") + ".log";
                var repository = LogManager.GetRepository();

                #region MyRegion

                var appenders = repository.GetAppenders();
                if (appenders.Length > 0)
                {
                    RollingFileAppender targetApder = null;
                    foreach (var Apder in appenders)
                    {
                        if (Apder.Name == "AppLog")
                        {
                            targetApder = Apder as RollingFileAppender;
                            break;
                        }
                    }

                    if (targetApder.Name == "AppLog") //如果是文件输出类型日志，则更改输出路径
                    {
                        if (targetApder != null)
                        {
                            if (!targetApder.File.Contains(filename))
                            {
                                targetApder.File = @"SysLog\" + filename;
                                targetApder.ActivateOptions();
                            }
                        }
                    }
                }

                #endregion

                action(msg);
            }
        }

        public static void WriteError(string msg, bool isWrite)
        {
            WriteLog(msg, isWrite, logComm.Error);
        }

        public static void WriteInfo(string msg, bool isWrite)
        {
            WriteLog(msg, isWrite, logComm.Info);
        }

        public static void WriteWarn(string msg, bool isWrite)
        {
            WriteLog(msg, isWrite, logComm.Warn);
        }
    }
}