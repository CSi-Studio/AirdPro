/*
 * Copyright (c) 2020 CSi Studio
 * Aird and AirdPro are licensed under Mulan PSL v2.
 * You can use this software according to the terms and conditions of the Mulan PSL v2. 
 * You may obtain a copy of Mulan PSL v2 at:
 *          http://license.coscl.org.cn/MulanPSL2 
 * THIS SOFTWARE IS PROVIDED ON AN "AS IS" BASIS, WITHOUT WARRANTIES OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO NON-INFRINGEMENT, MERCHANTABILITY OR FIT FOR A PARTICULAR PURPOSE.  
 * See the Mulan PSL v2 for more details.
 */

using System;
using System.Windows.Forms;
using AirdPro.Domains.Job;
using AirdPro.Forms;

namespace AirdPro
{
    internal static class Program
    {
        public static AirdForm mainForm { get; private set; }
        public static GlobalConfigHandler globalConfigHandler { get; private set; }
        public static ConversionConfigHandler conversionConfigHandler { get; private set; }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                globalConfigHandler = new GlobalConfigHandler();
                conversionConfigHandler = new ConversionConfigHandler();

                mainForm = new AirdForm();
                Application.Run(mainForm);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        #region Exception handling
        public static void HandleException(Exception e)
        {
            MessageBox.Show(e.ToString(), "Error");
            return;
        }
        #endregion
    }
}