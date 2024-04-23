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
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using AirdPro.Asyncs;
using AirdPro.CommandLine;
using AirdPro.Domains;
using AirdPro.Forms;
using AirdPro.Repository;
using AirdPro.Storage.Config;
using AirdPro.Storage.Handler;
using CommandLine;
using HZH_Controls;

namespace AirdPro
{
    static class Program
    {
        public static MainForm mainForm { get; set; }
        public static AboutForm aboutForm { get; set; }
        public static ConversionForm conversionForm { get; set; }
        public static ConversionConfigListForm configListForm { get; set; }
        public static RedisForm redisForm { get; set; }
        public static ConversionConfigHandler conversionConfigHandler { get; set; }
        public static VendorFileSelectorForm fileSelector { get; set; }
        public static MLForm mlForm { get; set; }
        public static PXForm pxForm { get; set; }

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.ThreadException += UIThread_UnhandledException;

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                conversionForm = new ConversionForm();
                conversionConfigHandler = new ConversionConfigHandler();
                Application.Run(conversionForm);
            }
            else
            {
                AllocConsole();
                Console.WriteLine("This is AirdPro Command Line");
                Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(options =>
                    {
                        // 这里可以访问options对象的属性来获取命令行参数的值
                        string inputFilePath = options.InputFilePath;
                        string outputFilePath = options.OutputFilePath;
                        if (outputFilePath.IsEmpty())
                        {
                            outputFilePath = Path.GetDirectoryName(inputFilePath);
                        }

                        string acquisitionMethod = options.AcquisitionMethod;
                        if (acquisitionMethod.IsEmpty())
                        {
                            acquisitionMethod = JobInfo.AutoType;
                        }

                        string configName = options.ConfigName;
                        if (configName.IsEmpty())
                        {
                            configName = "Default";
                        }
                        conversionConfigHandler = new ConversionConfigHandler();
                        ConversionConfig config = Program.conversionConfigHandler.configMap[configName];
                        if (config == null && Program.conversionConfigHandler.configMap != null && Program.conversionConfigHandler.configMap.Count > 0)
                        {
                            config = Program.conversionConfigHandler.configMap.First().Value;
                        }

                        string suffix = options.Suffix;
                        if (suffix.IsEmpty())
                        {
                            config.suffix = suffix;
                        }
                        JobInfo jobInfo = new JobInfo(inputFilePath, outputFilePath, acquisitionMethod, config);
                        ConvertTaskManager.GetInstance().RunJob(jobInfo);
                        Console.WriteLine("JobInfo:"+jobInfo.GetJsonInfo());
                    }
                ).WithNotParsed<Options>(errs =>
                {
                    Console.WriteLine("Error Info：");
                    foreach (var error in errs)
                    {
                        Console.WriteLine(error);
                    }
                });
                Console.WriteLine("Conversion Complete");
                Console.ReadLine();
            }
        }

        #region Exception handling

        public static void HandleException(string title, Exception e)
        {
            string message = e?.ToString() ?? "Unknown exception.";
            if (e?.InnerException != null) message += "\n\nAdditional information: " + e.InnerException;
            MessageBox.Show(message,
                title,
                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                0, false);
        }

        private static void UIThread_UnhandledException(object sender, ThreadExceptionEventArgs e)
        {
            HandleException("Unhandled Exception", e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException("Unhandled Exception", e.ExceptionObject as Exception);
        }

        #endregion
    }
}