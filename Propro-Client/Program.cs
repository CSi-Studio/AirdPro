using System;
using System.Threading;
using System.Windows.Forms;
using Propro.Forms;

namespace Propro
{
    internal static class Program
    {
        public static AirdForm mainForm { get; private set; }

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