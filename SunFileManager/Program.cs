using SunFileManager.Config;
using SunFileManager.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace SunFileManager
{
    public static class Program
    {
        public static UserSettings UserSettings;
        private static MainWindow mainWindowInstance = null;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        private const uint WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        [STAThread]
        private static void Main(string[] args)
        {
            string[] sunFilesToLoad = args
                .Where(a => !string.IsNullOrEmpty(a) && File.Exists(a) &&
                            Path.GetExtension(a).Equals(".sun", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            bool createdNew;
            using (Mutex mutex = new Mutex(true, "SunFileManager", out createdNew))
            {
                if (createdNew)
                {
                    string pendingFilesPath = Path.Combine(Path.GetTempPath(), "SunFileManager_PendingFiles.txt");
                    if (sunFilesToLoad.Length > 0)
                        File.WriteAllLines(pendingFilesPath, sunFilesToLoad);

                    UserSettings = UserSettings.Load();

                    var app = new App();
                    app.InitializeComponent();
                    mainWindowInstance = new MainWindow(sunFilesToLoad);

                    Thread monitorThread = new Thread(() =>
                    {
                        Thread.Sleep(300);
                        if (File.Exists(pendingFilesPath))
                        {
                            try
                            {
                                string[] allPending = File.ReadAllLines(pendingFilesPath);
                                string[] additional = allPending.Except(sunFilesToLoad).ToArray();
                                if (additional.Length > 0)
                                {
                                    mainWindowInstance.Dispatcher.Invoke(() =>
                                    {
                                        foreach (string file in additional)
                                            if (File.Exists(file))
                                                mainWindowInstance.LoadFile(file);
                                    });
                                }
                                File.Delete(pendingFilesPath);
                            }
                            catch
                            {
                                try { File.Delete(pendingFilesPath); } catch { }
                            }
                        }
                    });
                    monitorThread.IsBackground = true;
                    monitorThread.Start();

                    app.Run(mainWindowInstance);
                }
                else
                {
                    if (sunFilesToLoad.Length == 0) return;

                    IntPtr hwnd = FindExistingInstanceWindow();
                    if (hwnd != IntPtr.Zero)
                    {
                        // Primary window is ready — send files via WM_COPYDATA
                        SetForegroundWindow(hwnd);
                        foreach (string file in sunFilesToLoad)
                            SendFileViaWmCopyData(file, hwnd);
                    }
                    else
                    {
                        // Primary window not ready yet (race at startup) — write to pending file
                        string pendingFilesPath = Path.Combine(Path.GetTempPath(), "SunFileManager_PendingFiles.txt");
                        try
                        {
                            List<string> existing = new List<string>();
                            if (File.Exists(pendingFilesPath))
                                existing.AddRange(File.ReadAllLines(pendingFilesPath));
                            existing.AddRange(sunFilesToLoad);
                            File.WriteAllLines(pendingFilesPath, existing.Distinct().ToArray());
                        }
                        catch { }
                    }
                }
            }
        }

        private static IntPtr FindExistingInstanceWindow()
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id != current.Id && process.MainWindowHandle != IntPtr.Zero)
                    return process.MainWindowHandle;
            }
            return IntPtr.Zero;
        }

        private static void SendFileViaWmCopyData(string filePath, IntPtr hwnd)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;
            byte[] sarr = System.Text.Encoding.Default.GetBytes(filePath + "\0");
            COPYDATASTRUCT cds = new COPYDATASTRUCT
            {
                dwData = IntPtr.Zero,
                cbData = sarr.Length,
                lpData = Marshal.AllocHGlobal(sarr.Length)
            };
            try
            {
                Marshal.Copy(sarr, 0, cds.lpData, sarr.Length);
                SendMessage(hwnd, WM_COPYDATA, IntPtr.Zero, ref cds);
            }
            finally
            {
                Marshal.FreeHGlobal(cds.lpData);
            }
        }

        public static void SetMainFormInstance(MainWindow instance) => mainWindowInstance = instance;
        public static MainWindow GetMainFormInstance() => mainWindowInstance;
    }
}
