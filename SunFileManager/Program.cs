using SunFileManager.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SunFileManager
{
    public static class Program
    {
        public static UserSettings UserSettings;
        public static string settingsPath;
        public static FileManager FileManager = new FileManager();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            string sunfileToLoad = null;
            if (args.Length > 0)
            {
                sunfileToLoad = args[0];
            }

            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "SunFileManager", out createdNew))
            {
                if (createdNew)
                {
                    settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    settingsPath = Path.Combine(settingsPath, "SunSettings", "SunFileManager");
                    UserSettings = new UserSettings();
                    UserSettings.Load(settingsPath);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmFileManager(sunfileToLoad));
                }
                else
                {
                    if (args.Length > 0)
                        sunfileToLoad = args[0];
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            frmFileManager.LoadFile(sunfileToLoad);
                            break;
                        }
                    }
                }
            }
        }

        /*public static bool PrepareApplication(bool from_internal)
        {
            _ConfigurationManager = new ConfigurationManager();

            bool loaded = _ConfigurationManager.Load();
            if (!loaded)
            {
                Warning.Error(HaRepacker.Properties.Resources.ProgramLoadSettingsError);
                return true;
            }
            bool firstRun = Program.ConfigurationManager.ApplicationSettings.FirstRun;
            if (Program.ConfigurationManager.ApplicationSettings.FirstRun)
            {
                //new FirstRunForm().ShowDialog();
                Program.ConfigurationManager.ApplicationSettings.FirstRun = false;
                _ConfigurationManager.Save();
            }
            if (Program.ConfigurationManager.UserSettings.AutoAssociate && from_internal && IsUserAdministrator())
            {
                string path = Application.ExecutablePath;
                Registry.ClassesRoot.CreateSubKey(".wz").SetValue("", "SunFile");
                RegistryKey wzKey = Registry.ClassesRoot.CreateSubKey("SunFile");
                wzKey.SetValue("", "Wz File");
                wzKey.CreateSubKey("DefaultIcon").SetValue("", path + ",1");
                wzKey.CreateSubKey("shell\\open\\command").SetValue("", "\"" + path + "\" \"%1\"");
            }
            return firstRun;
        }*/
    }
}