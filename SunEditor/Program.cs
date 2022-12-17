using SunEditor.Config;
using SunEditor.GUI;
using System.IO;

namespace SunEditor
{
    internal static class Program
    {
        public static UserSettings UserSettings;
        public static string settingsPath;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            settingsPath = Path.Combine(settingsPath, "SunSettings", "SunEditor");
            UserSettings = new UserSettings();
            UserSettings.Load(settingsPath);

            ApplicationConfiguration.Initialize();
            Application.Run(new frmEditorForm());
        }
    }
}