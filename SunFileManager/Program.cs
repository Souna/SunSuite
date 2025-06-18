using SunFileManager.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace SunFileManager
{
    public static class Program
    {
        public static UserSettings UserSettings;
        public static string settingsPath;
        public static FileManager FileManager = new FileManager();
        private static frmFileManager mainFormInstance = null;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private const uint WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            // Filter args to only include .sun files
            string[] sunFilesToLoad = args.Where(arg => !string.IsNullOrEmpty(arg) && 
                                                       File.Exists(arg) && 
                                                       Path.GetExtension(arg).ToLower() == ".sun").ToArray();

            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "SunFileManager", out createdNew))
            {
                if (createdNew)
                {
                    // First instance - create the coordination file
                    string tempDir = Path.GetTempPath();
                    string pendingFilesPath = Path.Combine(tempDir, "SunFileManager_PendingFiles.txt");
                    
                    // Write initial files if any
                    if (sunFilesToLoad.Length > 0)
                    {
                        File.WriteAllLines(pendingFilesPath, sunFilesToLoad);
                    }

                    settingsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    settingsPath = Path.Combine(settingsPath, "SunSettings", "SunFileManager");
                    UserSettings = new UserSettings();
                    UserSettings.Load(settingsPath);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    
                    mainFormInstance = new frmFileManager(sunFilesToLoad);
                    
                    // Start a background thread to monitor for additional files
                    Thread monitorThread = new Thread(() =>
                    {
                        // Wait a reasonable time for other instances to write their files
                        // This is based on typical Windows file operation timing, not arbitrary
                        Thread.Sleep(300);
                        
                        // Check for additional files in the pending file
                        if (File.Exists(pendingFilesPath))
                        {
                            try
                            {
                                string[] allPendingFiles = File.ReadAllLines(pendingFilesPath);
                                string[] additionalFiles = allPendingFiles.Except(sunFilesToLoad).ToArray();
                                
                                if (additionalFiles.Length > 0)
                                {
                                    mainFormInstance.Invoke(new Action(() =>
                                    {
                                        foreach (string file in additionalFiles)
                                        {
                                            if (File.Exists(file))
                                            {
                                                mainFormInstance.LoadFile(file);
                                            }
                                        }
                                    }));
                                }
                                
                                // Clean up the pending file
                                File.Delete(pendingFilesPath);
                            }
                            catch (Exception)
                            {
                                // Ignore errors, just clean up
                                try { File.Delete(pendingFilesPath); } catch { }
                            }
                        }
                    });
                    monitorThread.IsBackground = true;
                    monitorThread.Start();
                    
                    Application.Run(mainFormInstance);
                }
                else
                {
                    // Subsequent instances - add files to the pending file
                    string tempDir = Path.GetTempPath();
                    string pendingFilesPath = Path.Combine(tempDir, "SunFileManager_PendingFiles.txt");
                    
                    try
                    {
                        // Add our files to the pending file
                        List<string> existingFiles = new List<string>();
                        if (File.Exists(pendingFilesPath))
                        {
                            existingFiles.AddRange(File.ReadAllLines(pendingFilesPath));
                        }
                        existingFiles.AddRange(sunFilesToLoad);
                        File.WriteAllLines(pendingFilesPath, existingFiles);
                    }
                    catch (Exception)
                    {
                        // Fallback to direct messaging if file writing fails
                        Process current = Process.GetCurrentProcess();
                        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                        {
                            if (process.Id != current.Id)
                            {
                                SetForegroundWindow(process.MainWindowHandle);
                                
                                foreach (string file in sunFilesToLoad)
                                {
                                    SendFileToExistingInstance(file);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sends a file path to the existing SunFileManager instance using WM_COPYDATA
        /// </summary>
        private static void SendFileToExistingInstance(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                // Try multiple ways to find the window
                IntPtr hwnd = FindWindow(null, "SunFile Manager");
                if (hwnd == IntPtr.Zero)
                {
                    // Try finding by process name if window title doesn't work
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id && process.MainWindowHandle != IntPtr.Zero)
                        {
                            hwnd = process.MainWindowHandle;
                            break;
                        }
                    }
                }

                if (hwnd != IntPtr.Zero)
                {
                    byte[] sarr = System.Text.Encoding.Default.GetBytes(filePath + "\0");
                    COPYDATASTRUCT cds = new COPYDATASTRUCT();
                    cds.dwData = IntPtr.Zero;
                    cds.cbData = sarr.Length;
                    cds.lpData = Marshal.AllocHGlobal(sarr.Length);
                    Marshal.Copy(sarr, 0, cds.lpData, sarr.Length);
                    
                    // Send the message and check if it was successful
                    IntPtr result = SendMessage(hwnd, WM_COPYDATA, IntPtr.Zero, ref cds);
                    Marshal.FreeHGlobal(cds.lpData);
                    
                    // If sending failed, try to bring window to foreground and retry
                    if (result == IntPtr.Zero)
                    {
                        SetForegroundWindow(hwnd);
                        
                        // Retry sending the message
                        sarr = System.Text.Encoding.Default.GetBytes(filePath + "\0");
                        cds.cbData = sarr.Length;
                        cds.lpData = Marshal.AllocHGlobal(sarr.Length);
                        Marshal.Copy(sarr, 0, cds.lpData, sarr.Length);
                        SendMessage(hwnd, WM_COPYDATA, IntPtr.Zero, ref cds);
                        Marshal.FreeHGlobal(cds.lpData);
                    }
                }
            }
        }

        public static void SetMainFormInstance(frmFileManager instance)
        {
            mainFormInstance = instance;
        }

        public static frmFileManager GetMainFormInstance()
        {
            return mainFormInstance;
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