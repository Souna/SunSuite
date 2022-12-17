using SunFileManager.Config;
using SunFileManager.GUI.Input.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace SunFileManager
{
    internal static class Program
    {
        public static UserSettings UserSettings;
        public static string settingsPath;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string sunfileToLoad = null;
            if (args.Length > 0)
            {
                sunfileToLoad= args[0];
            }
            settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            settingsPath = Path.Combine(settingsPath, "SunSettings", "SunFileManager");
            UserSettings = new UserSettings();
            UserSettings.Load(settingsPath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmFileManager(sunfileToLoad));
        }
    }
}