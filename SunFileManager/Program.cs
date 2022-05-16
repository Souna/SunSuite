using SunFileManager.Config;
using SunFileManager.GUI.Input.Forms;
using System;
using System.Windows.Forms;

namespace SunFileManager
{
    internal static class Program
    {
        public static UserSettings UserSettings;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            UserSettings = new UserSettings();
            UserSettings.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmFileManager());
        }
    }
}