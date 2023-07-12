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
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using AirdPro.Forms;
using AirdPro.Storage.Handler;
using pwiz.CLI.msdata;

namespace AirdPro
{
    static class Program
    {
        public static MainForm mainForm { get; private set; }
        public static ConversionForm conversionForm { get; set; }
        public static ConversionConfigListForm configListForm { get; set; }
        public static GlobalSettingForm globalSettingForm { get; set; }
        public static ConversionConfigHandler conversionConfigHandler { get; private set; }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void Main(string[] args)
        {
            Application.ThreadException += UIThread_UnhandledException;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            conversionForm = new ConversionForm();
            // configListForm = new ConversionConfigListForm();
            // globalSettingForm = new GlobalSettingForm();
            conversionConfigHandler = new ConversionConfigHandler();
            mainForm = new MainForm();
            Application.Run(mainForm);
        
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