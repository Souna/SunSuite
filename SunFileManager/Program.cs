using SunFileManager.Config;
using SunFileManager.GUI.Input.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SunFileManager
{
    internal static class Program
    {
        public static UserSettings UserSettings;
        public static string settingsPath;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

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
    }
}