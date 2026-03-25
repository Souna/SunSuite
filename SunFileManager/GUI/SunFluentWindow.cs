using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using Wpf.Ui.Controls;

namespace SunFileManager.GUI
{
    /// <summary>
    /// Base window class for all SunFileManager windows.
    /// Applies the correct DWM dark/light title-bar attribute when the HWND is ready,
    /// and exposes ApplyDwmTheme() so ApplyTheme() can refresh already-open windows.
    /// </summary>
    public class SunFluentWindow : FluentWindow
    {
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ApplyDwmTheme();
        }

        public void ApplyDwmTheme()
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            if (hwnd == IntPtr.Zero) return;
            int dark = Program.UserSettings.DarkMode ? 1 : 0;
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref dark, sizeof(int));
        }
    }
}
